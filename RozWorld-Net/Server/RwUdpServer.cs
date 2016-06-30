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
using Oddmatics.Util.IO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Oddmatics.RozWorld.Net.Server
{
    /// <summary>
    /// Represents a RozWorld server's UDP subsystem.
    /// </summary>
    public class RwUdpServer
    {
        /// <summary>
        /// The default port for RozWorld servers.
        /// </summary>
        public const int ROZWORLD_DEFAULT_PORT = 41715;


        /// <summary>
        /// Gets whether this RwUdpServer is currently active.
        /// </summary>
        public bool Active { get; private set; }

        /// <summary>
        /// The UdpClient instance for peforming the networking operations.
        /// </summary>
        private UdpClient Client;

        /// <summary>
        /// The IPEndPoint for networking operations.
        /// </summary>
        private IPEndPoint EndPoint;


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
            Client = new UdpClient(port);
            EndPoint = new IPEndPoint(IPAddress.Any, port);
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
