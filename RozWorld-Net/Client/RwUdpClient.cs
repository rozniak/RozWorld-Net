/**
 * Oddmatics.RozWorld.Net.Client.RwUdpClient -- RozWorld UDP Client Subsystem
 *
 * This source-code is part of the netcode library for the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld-Net>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using Oddmatics.RozWorld.Net.Packets;
using Oddmatics.RozWorld.Net.Packets.Event;
using Oddmatics.RozWorld.Net.Server;
using Oddmatics.Util.IO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Timers;

namespace Oddmatics.RozWorld.Net.Client
{
    /// <summary>
    /// Represents a RozWorld client's UDP subsystem.
    /// </summary>
    public class RwUdpClient
    {
        /// <summary>
        /// The time in milliseconds until the remote server is deemed unreachable.
        /// </summary>
        public const ushort SERVER_TIMEOUT_TIME = 15000;

        /// <summary>
        /// The default broadcast IPEndPoint destination to RozWorld servers.
        /// </summary>
        public static readonly IPEndPoint SERVER_BROADCAST_ENDPOINT = new IPEndPoint(IPAddress.Broadcast,
            RwUdpServer.ROZWORLD_DEFAULT_PORT);


        /// <summary>
        /// Gets whether this RwUdpClient is currently active.
        /// </summary>
        public bool Active { get; private set; }

        /// <summary>
        /// The UdpClient instance for peforming the networking operations.
        /// </summary>
        private UdpClient Client;

        /// <summary>
        /// The IPEndPoint of the server being communicated to currently.
        /// </summary>
        private IPEndPoint EndPoint;

        /// <summary>
        /// The time in milliseconds since the last packet was receieved from the connected server.
        /// </summary>
        private ushort SinceServerPacket;

        /// <summary>
        /// The current ClientState of this RwUdpClient.
        /// </summary>
        public ClientState State { get; private set; }

        /// <summary>
        /// The Timer that advances timeout counts on important packets and time since last server response.
        /// </summary>
        public Timer TimeoutTimer { get; private set; }

        /// <summary>
        /// The currently watched packets as a Dictionary&lt;string, PacketWatcher&gt; collection.
        /// </summary>
        private Dictionary<string, PacketWatcher> WatchedPackets;


        /// <summary>
        /// Occurs when the current connection is terminated by the remote host - for local class usage only.
        /// </summary>
        private event EventHandler ConnectionError;

        /// <summary>
        /// Occurs when the current connection is terminated by the remote host.
        /// </summary>
        public event EventHandler ConnectionTerminated;

        /// <summary>
        /// Occurs when a server information response has been received.
        /// </summary>
        public event PacketEventHandler InfoResponseReceived;

        /// <summary>
        /// Occurs when a log in response has been received.
        /// </summary>
        public event PacketEventHandler LogInResponseReceieved;

        /// <summary>
        /// Occurs when a packet has timed out.
        /// </summary>
        public event PacketEventHandler PacketTimeout;

        /// <summary>
        /// Occurs when a sign up response has been received.
        /// </summary>
        public event PacketEventHandler SignUpResponseReceived;


        /// <summary>
        /// Initialises a new instance of the RwUdpClient class.
        /// </summary>
        public RwUdpClient()
        {
            TimeoutTimer = new Timer(10);
            TimeoutTimer.Enabled = true;
            TimeoutTimer.Start();
            WatchedPackets = new Dictionary<string, PacketWatcher>();
            Random random = new Random();
            bool successfulPort = false;

            while (!successfulPort)
            {
                try
                {
                    // Choose port number from dynamic range
                    Client = new UdpClient(random.Next(49152, 65535));
                    successfulPort = true;
                }
                catch (SocketException ex)
                {
                    // Most likely port not available
                }
            }
        }


        /// <summary>
        /// Begins networking operations and starts listening.
        /// </summary>
        public void Begin()
        {
            if (!Active)
            {
                Active = true;
                Client.BeginReceive(new AsyncCallback(Received), null);
            }
            else
                throw new InvalidOperationException("RwUdpClient.Begin: Already started.");
        }

        /// <summary>
        /// Begins broadcasting server information request packets.
        /// </summary>
        /// <param name="clientImplementation">The name of the client's implementation.</param>
        /// <param name="versionRaw">The raw version number of the client.</param>
        /// <param name="serverImplementation">The server implementation to look for (use * for a wildcard).</param>
        public void BroadcastServerScan(string clientImplementation, ushort versionRaw, string serverImplementation)
        {
            if (State == ClientState.Idle || State == ClientState.Broadcasting)
            {
                var packet = new ServerInfoRequestPacket(clientImplementation,
                    versionRaw, serverImplementation);

                if (State == ClientState.Broadcasting)
                    WatchedPackets["ServerInfoRequest"].Reset(packet);
                else
                {
                    var packetWatcher = new PacketWatcher(packet, SERVER_BROADCAST_ENDPOINT, this);
                    State = ClientState.Broadcasting;
                    packetWatcher.Timeout += new EventHandler(packetWatcher_Timeout_ServerScan);
                    WatchedPackets.Add("ServerInfoRequest", packetWatcher);
                    packetWatcher.Start();
                }
            }
            else
                throw new InvalidOperationException("RwUdpClient.BroadcastServerScan: Cannot broadcast in the current state.");
        }

        /// <summary>
        /// Kills the current receiving operation and returns the IPacket from the associated watched packets.
        /// </summary>
        /// <param name="requestType">The ongoing request operation to kill.</param>
        /// <returns>The IPacket in the watched packets Dictionary associated with the operation specified.</returns>
        private IPacket KillReceive(string requestType)
        {
            if (!WatchedPackets.ContainsKey(requestType))
                throw new ArgumentException("RwUdpClient.KillReceive: Unknown request type specified.");

            IPacket packet = (IPacket)WatchedPackets[requestType].Packet.Clone();
            WatchedPackets[requestType].Stop();

            switch (requestType)
            {
                case "SignUpRequest":
                    WatchedPackets[requestType].Timeout -= packetWatcher_Timeout_SignUp;
                    ConnectionError -= packetWatcher_Timeout_SignUp;
                    break;

                default: return packet; // Should never reach this point
            }

            WatchedPackets.Remove(requestType);

            return packet;
        }

        /// <summary>
        /// Sends a log in request packet to a remote server.
        /// </summary>
        /// <param name="username">The username to log in with.</param>
        /// <param name="passwordHash">The SHA-256 hashed password.</param>
        /// <param name="chatOnly">Whether to log in as a chat only client.</param>
        /// <param name="skinDownloads">Whether to enable skin downloading from the server.</param>
        /// <returns>True if a log in request was sent.</returns>
        public bool LogInToServer(string username, byte[] passwordHash, bool chatOnly, bool skinDownloads, IPEndPoint destination)
        {
            if (State == ClientState.Idle)
            {
                var finalHash = new List<byte>();
                DateTime currentUtc = DateTime.UtcNow;
                DateTime midnightUtc = new DateTime(currentUtc.Year, currentUtc.Month,
                    currentUtc.Day);
                int hashTickTime = (int)(currentUtc.Ticks - midnightUtc.Ticks);

                finalHash.AddRange(passwordHash);
                finalHash.AddRange(hashTickTime.GetBytes());

                var packet = new LogInRequestPacket(username, finalHash.ToArray(), hashTickTime,
                    chatOnly, skinDownloads);
                var packetWatcher = new PacketWatcher(packet, destination, this);

                State = ClientState.LoggingIn;
                packetWatcher.Timeout += new EventHandler(packetWatcher_Timeout_LogIn);
                ConnectionError += new EventHandler(packetWatcher_Timeout_LogIn);
                WatchedPackets.Add("LogInRequest", packetWatcher);
                packetWatcher.Start();

                return true;
            }

            return false;
        }

        void packetWatcher_Timeout_LogIn(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sends an IPacket to the specified IPEndPoint.
        /// </summary>
        /// <param name="packet">The IPacket to send.</param>
        /// <param name="destination">The destination IPEndPoint.</param>
        public void Send(IPacket packet, IPEndPoint destination)
        {
            byte[] txData = packet.GetBytes();
            Client.BeginSend(txData, txData.Length, destination, new AsyncCallback(Sent), null);
        }

        /// <summary>
        /// Sends a sign up request packet to a remote server.
        /// </summary>
        /// <param name="username">The username to sign up with.</param>
        /// <param name="passwordHash">The SHA-256 hashed password.</param>
        /// <param name="destination">The IPEndPoint of the remote server.</param>
        /// <returns>True if a sign up request was sent.</returns>
        public bool SignUpToServer(string username, byte[] passwordHash, IPEndPoint destination)
        {
            if (State == ClientState.Idle)
            {
                var packet = new SignUpRequestPacket(username, passwordHash);
                var packetWatcher = new PacketWatcher(packet, destination, this);

                State = ClientState.SigningUp;
                packetWatcher.Timeout += new EventHandler(packetWatcher_Timeout_SignUp);
                ConnectionError += new EventHandler(packetWatcher_Timeout_SignUp);
                WatchedPackets.Add("SignUpRequest", packetWatcher);
                packetWatcher.Start();

                return true;
            }

            return false;
        }


        /// <summary>
        /// [WatchedPackets[ServerInfoRequest].Timeout] Server scan packet timeout.
        /// </summary>
        private void packetWatcher_Timeout_ServerScan(object sender, EventArgs e)
        {
            State = ClientState.Idle;
            WatchedPackets["ServerInfoRequest"].Timeout -= packetWatcher_Timeout_ServerScan;
            WatchedPackets.Remove("ServerInfoRequest");
        }

        /// <summary>
        /// [WatchedPackets[SignUpRequest].Timeout] Sign up packet timeout.
        /// </summary>
        private void packetWatcher_Timeout_SignUp(object sender, EventArgs e)
        {
            State = ClientState.Idle;
            IPacket packet = KillReceive("SignUpRequest");

            if (PacketTimeout != null && sender is PacketWatcher)
                PacketTimeout(this, packet);
        }

        /// <summary>
        /// [BeginReceive Callback] UDP Packet Received.
        /// </summary>
        private void Received(IAsyncResult result)
        {
            IPEndPoint senderEP = new IPEndPoint(0, 0);
            byte[] rxData;

            try
            {
                rxData = Client.EndReceive(result, ref senderEP);
            }
            catch (SocketException socketEx) // Remote host unreachable
            {
                if (ConnectionError != null)
                    ConnectionError(this, EventArgs.Empty);
                if (ConnectionTerminated != null)
                    ConnectionTerminated(this, EventArgs.Empty);
                return;
            }
            
            Client.BeginReceive(new AsyncCallback(Received), null);

            int currentIndex = 0;
            ushort id = ByteParse.NextUShort(rxData, ref currentIndex);

            switch (id)
            {
                    // ServerInfoResponsePacket
                case PacketType.SERVER_INFO_ID:
                    if (InfoResponseReceived != null && State == ClientState.Broadcasting)
                        InfoResponseReceived(this, new ServerInfoResponsePacket(rxData, senderEP));
                    break;

                    // SignUpResponsePacket
                case PacketType.SIGN_UP_ID:
                    if (SignUpResponseReceived != null && State == ClientState.SigningUp &&
                        senderEP.Equals(WatchedPackets["SignUpRequest"].EndPoint))
                    {
                        State = ClientState.Idle;
                        KillReceive("SignUpRequest");
                        SignUpResponseReceived(this, new SignUpResponsePacket(rxData, senderEP));
                    }

                    break;

                case PacketType.LOG_IN_ID:
                    if (LogInResponseReceieved != null && State == ClientState.LoggingIn &&
                        senderEP.Equals(WatchedPackets["LogInRequest"].EndPoint))
                    {
                        KillReceive("LogInRequest");
                        var logInPacket = new LogInResponsePacket(rxData, senderEP);

                        if (logInPacket.Success)
                        {
                            State = ClientState.Connected;
                            EndPoint = senderEP;
                            TimeoutTimer.Elapsed += new ElapsedEventHandler(TimeoutTimer_Elapsed_ServerConnection);
                        }
                        else
                            State = ClientState.Idle;

                        if (LogInResponseReceieved != null)
                            LogInResponseReceieved(this, logInPacket);
                    }

                    break;

                case 0:
                default:
                    // Bad packet
                    break;
            }
        }

        /// <summary>
        /// [BeginSend Callback] UDP Packet Sent.
        /// </summary>
        private void Sent(IAsyncResult result)
        {
            Client.EndSend(result);
        }

        /// <summary>
        /// [TimeoutTimer.Elapsed] Timeout timer ticked.
        /// </summary>
        private void TimeoutTimer_Elapsed_ServerConnection(object sender, ElapsedEventArgs e)
        {
            SinceServerPacket += (ushort)TimeoutTimer.Interval;

            if (SinceServerPacket > SERVER_TIMEOUT_TIME)
            {
                TimeoutTimer.Elapsed -= TimeoutTimer_Elapsed_ServerConnection;
                State = ClientState.Idle;

                if (ConnectionTerminated != null)
                    ConnectionTerminated(this, EventArgs.Empty);
            }
        }
    }
}
