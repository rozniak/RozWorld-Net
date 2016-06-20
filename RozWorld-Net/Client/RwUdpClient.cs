/**
 * Oddmatics.RozWorld.Net.Client.RwUdpClient -- RozWorld UDP Client Subsystem
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

namespace Oddmatics.RozWorld.Net.Client
{
    /// <summary>
    /// Represents a RozWorld client's UDP subsystem.
    /// </summary>
    public class RwUdpClient
    {
        /// <summary>
        /// Gets whether this RwUdpClient is currently active.
        /// </summary>
        public bool Active { get; private set; }

        /// <summary>
        /// The UdpClient instance for peforming the networking operations.
        /// </summary>
        private UdpClient Client;

        /// <summary>
        /// Gets whether this RwUdpClient is connected to a server.
        /// </summary>
        public bool Connected { get { return EndPoint != null; } }

        /// <summary>
        /// The IPEndPoint of the server being listened to.
        /// </summary>
        private IPEndPoint EndPoint;


        /// <summary>
        /// Initialises a new instance of the RwUdpClient class with a specified port number.
        /// </summary>
        /// <param name="port">The port number to use.</param>
        public RwUdpClient()
        {
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
                throw new InvalidOperationException("RwUdpServer.Begin: Already started.");
        }

        /// <summary>
        /// [BeginReceive Callback] UDP Packet Received.
        /// </summary>
        private void Received(IAsyncResult result)
        {
            byte[] rxData = Client.EndReceive(result, ref EndPoint);
            Client.BeginReceive(new AsyncCallback(Received), null);

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
    }
}
