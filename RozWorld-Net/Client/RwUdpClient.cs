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

using Oddmatics.RozWorld.Net.Client.Event;
using Oddmatics.RozWorld.Net.Packets;
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
        private ClientState State;

        /// <summary>
        /// The Timer that advances timeout counts on important packets and time since last server response.
        /// </summary>
        public Timer TimeoutTimer { get; private set; }

        /// <summary>
        /// The currently watched packets as a Dictionary&lt;string, PacketWatcher&gt; collection.
        /// </summary>
        private Dictionary<string, PacketWatcher> WatchedPackets;


        /// <summary>
        /// Occurs when a server information response has been received.
        /// </summary>
        public event InfoResponseReceivedHandler InfoResponseReceived;


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
                    State = ClientState.Broadcasting;
                    var packetWatcher = new PacketWatcher(packet, SERVER_BROADCAST_ENDPOINT, this);
                    packetWatcher.Timeout += new EventHandler(packetWatcher_Timeout_ServerScan);
                    WatchedPackets.Add("ServerInfoRequest", packetWatcher);
                    packetWatcher.Start();
                }
            }
            else
                throw new InvalidOperationException("RwUdpClient.BroadcastServerScan");
        }

        /// <summary>
        /// [BeginReceive Callback] UDP Packet Received.
        /// </summary>
        private void Received(IAsyncResult result)
        {
            IPEndPoint senderEP = new IPEndPoint(0, 0);
            byte[] rxData = Client.EndReceive(result, ref senderEP);
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

                case PacketType.SIGN_UP_ID:
                    
                    break;

                case PacketType.LOG_IN_ID:

                    break;

                case 0:
                default:
                    // Bad packet
                    break;
            }
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
        /// [BeginSend Callback] UDP Packet Sent.
        /// </summary>
        private void Sent(IAsyncResult result)
        {
            Client.EndSend(result);
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
    }
}
