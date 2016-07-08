﻿/**
 * Oddmatics.RozWorld.Net.Server.RwUdpServer -- RozWorld UDP Server Subsystem
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
using Oddmatics.RozWorld.Net.Server.Event;
using Oddmatics.Util.IO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Timers;

namespace Oddmatics.RozWorld.Net.Server
{
    /// <summary>
    /// Represents a RozWorld server's UDP subsystem.
    /// </summary>
    public class RwUdpServer
    {
        /// <summary>
        /// The time in milliseconds with no network activity in which a connected client is deemed disconnected.
        /// </summary>
        public const ushort CLIENT_TIMEOUT_TIME = 15000;

        /// <summary>
        /// The default port for RozWorld servers.
        /// </summary>
        public const int ROZWORLD_DEFAULT_PORT = 41715;


        /// <summary>
        /// Gets whether this RwUdpServer is currently active.
        /// </summary>
        public bool Active { get; private set; }

        /// <summary>
        /// Gets the amount of currently connected clients to this RwUdpServer.
        /// </summary>
        public int AmountOfConnections { get { return ConnectedClients.Count; } }

        /// <summary>
        /// The UdpClient instance for peforming the networking operations.
        /// </summary>
        private UdpClient Client;

        /// <summary>
        /// The currently connected clients contained alongside their individual IPEndPoints.
        /// </summary>
        private Dictionary<IPEndPoint, ConnectedClient> ConnectedClients;

        /// <summary>
        /// The IPEndPoint for networking operations.
        /// </summary>
        private IPEndPoint EndPoint;

        /// <summary>
        /// Gets the Timer that advances timeout counts for connected clients.
        /// </summary>
        public Timer TimeoutTimer { get; private set; }


        public event ClientDropEventHandler ClientDropped;

        /// <summary>
        /// Occurs when a server information request has been received.
        /// </summary>
        public event PacketEventHandler InfoRequestReceived;

        /// <summary>
        /// Occurs when a log in request has been received.
        /// </summary>
        public event PacketEventHandler LogInRequestReceived;

        /// <summary>
        /// Occurs when a sign up request has been received.
        /// </summary>
        public event PacketEventHandler SignUpRequestReceived;


        /// <summary>
        /// Initialises a new instance of the RwUdpServer class with a specified port number.
        /// </summary>
        /// <param name="port">The port number to use.</param>
        public RwUdpServer(int port)
        {
            TimeoutTimer = new Timer(10);
            TimeoutTimer.Enabled = true;
            TimeoutTimer.Start();
            Client = new UdpClient(port);
            ConnectedClients = new Dictionary<IPEndPoint, ConnectedClient>();
            EndPoint = new IPEndPoint(IPAddress.Any, port);
        }


        /// <summary>
        /// Creates and adds a new ConnectedClient to the collection in this RwUdpServer.
        /// </summary>
        /// <param name="clientEP">The IPEndPoint of the ConnectedClient to make.</param>
        /// <returns>True if the ConnectedClient was made and added (will return false if the IPEndPoint is already connected).</returns>
        public bool AddClient(IPEndPoint clientEP)
        {
            if (ConnectedClients.ContainsKey(clientEP))
                return false;

            var newClient = new ConnectedClient(clientEP, this);
            newClient.TimedOut += new EventHandler(ConnectedClient_TimedOut);

            ConnectedClients.Add(clientEP, newClient);

            newClient.Begin();

            return true;
        }

        /// <summary>
        /// Begins networking operations and starts listening.
        /// </summary>
        public void Begin()
        {
            if (!Active)
            {
                Active = true;
                Client.BeginReceive(new AsyncCallback(Received), EndPoint);
            }
            else
                throw new InvalidOperationException("RwUdpServer.Begin: Already started.");
        }

        /// <summary>
        /// Drops the ConnectedClient instance associated with the given IPEndPoint.
        /// Note: This should be done after performing proper disconnection, and not act as a substitute instead.
        /// </summary>
        /// <param name="clientEP">The IPEndPoint of the ConnectedClient.</param>
        /// <returns>True if the ConnectedClient was found and dropped.</returns>
        public bool DropClient(IPEndPoint clientEP)
        {
            if (ConnectedClients.ContainsKey(clientEP))
                ConnectedClients.Remove(clientEP);

            return false;
        }

        /// <summary>
        /// Gets a ConnectedClient instance from this RwUdpServer by its IPEndPoint.
        /// </summary>
        /// <param name="clientEP">The IPEndPoint of the ConnectedClient.</param>
        /// <returns>The ConnectedClient's instance if the IPEndPoint key matched, null otherwise.</returns>
        public ConnectedClient GetConnectedClient(IPEndPoint clientEP)
        {
            if (ConnectedClients.ContainsKey(clientEP))
                return ConnectedClients[clientEP];

            return null;
        }

        /// <summary>
        /// Sends an IPacket to the specified Socket.
        /// </summary>
        /// <param name="packet">The IPacket to send.</param>
        /// <param name="destination">The destination IPEndPoint.</param>
        public void Send(IPacket packet, IPEndPoint destination)
        {
            byte[] txData = packet.GetBytes();
            Client.BeginSend(txData, txData.Length, destination, new AsyncCallback(Sent), null);
        }


        /// <summary>
        /// [ConnectedClient.TimedOut] Connected client timed out.
        /// </summary>
        private void ConnectedClient_TimedOut(object sender, EventArgs e)
        {
            var client = (ConnectedClient)sender;

            client.TimedOut -= ConnectedClient_TimedOut; // Detach this event handler

            DropClient(client.EndPoint);

            if (ClientDropped != null)
                ClientDropped(this, new ClientDropEventArgs(client));
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
                    // ServerInfoRequestPacket
                case PacketType.SERVER_INFO_ID:
                    if (InfoRequestReceived != null)
                        InfoRequestReceived(this,
                            new PacketEventArgs(new ServerInfoRequestPacket(rxData, senderEP)));
                    break;

                    // SignUpRequestPacket
                case PacketType.SIGN_UP_ID:
                    if (SignUpRequestReceived != null)
                        SignUpRequestReceived(this,
                            new PacketEventArgs(new SignUpRequestPacket(rxData, senderEP)));
                    break;

                    // LogInRequestPacket
                case PacketType.LOG_IN_ID:
                    if (LogInRequestReceived != null)
                        LogInRequestReceived(this,
                            new PacketEventArgs(new LogInRequestPacket(rxData, senderEP)));
                    break;

                    // Generic ping (handle these internally)
                case PacketType.PING_ID:
                    if (ConnectedClients.ContainsKey(senderEP))
                        ConnectedClients[senderEP].ResetTimeoutCounter();

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
    }
}
