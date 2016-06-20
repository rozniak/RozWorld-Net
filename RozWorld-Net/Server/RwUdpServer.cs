/**
 * Oddmatics.RozWorld.Net.Server.RwUdpServer -- RozWorld UDP Server Subsystem
 *
 * This source-code is part of the server library for the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld-Net>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Oddmatics.RozWorld.Net.Packets;
using Oddmatics.Util.IO;

namespace Oddmatics.RozWorld.Net.Server
{
    /// <summary>
    /// Represents a RozWorld server's UDP subsystem.
    /// </summary>
    public class RwUdpServer
    {
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
                Client.BeginReceive(new AsyncCallback(Received), EndPoint);
            }
            else
                throw new InvalidOperationException("RwUdpServer.Begin: Already started.");
        }

        /// <summary>
        /// [BeginReceive Callback] UDP Packet Received.
        /// </summary>
        private void Received(IAsyncResult result)
        {
            IList<byte> rxData = new List<byte>(Client.EndReceive(result, ref EndPoint)).AsReadOnly();
            Client.BeginReceive(new AsyncCallback(Received), EndPoint);

            int currentIndex = 0;
            ushort id = ByteParse.NextUShort(rxData, ref currentIndex);

            switch (id)
            {
                case 0:
                default:
                    // Bad packet
                    break;
            }
        }

        /// <summary>
        /// Sends an IPacket to the specified Socket.
        /// </summary>
        /// <param name="packet">The IPacket to send.</param>
        /// <param name="destination">The destination Socket.</param>
        public void Send(IPacket packet, Socket destination)
        {
            byte[] txData = packet.GetBytes();
            Client.BeginSend(txData, txData.Length, new AsyncCallback(Sent), destination);

            // TODO: finish coding and testing this
        }

        /// <summary>
        /// [BeginSend Callback] UDP Packet Sent.
        /// </summary>
        private void Sent(IAsyncResult result)
        {
            Client.EndSend(result);

            // TODO: finish coding and testing this
        }
    }
}
