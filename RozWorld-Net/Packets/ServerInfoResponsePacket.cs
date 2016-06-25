/**
 * Oddmatics.RozWorld.Net.Packets.ServerInfoResponsePacket -- RozWorld Server Information Response Packet
 *
 * This source-code is part of the netcode library for the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld-Net>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using Oddmatics.Util.IO;
using System;
using System.Collections.Generic;
using System.Net;

namespace Oddmatics.RozWorld.Net.Packets
{
    /// <summary>
    /// [SERVER --> CLIENT] Represents a server info response packet.
    /// </summary>
    public class ServerInfoResponsePacket : IPacket
    {
        /// <summary>
        /// Gets whether the client is compatible the server.
        /// </summary>
        public bool ClientCompatible { get; private set; }

        /// <summary>
        /// Gets the ID of this ServerInfoResponsePacket.
        /// </summary>
        public ushort ID { get { return PacketType.SERVER_INFO_ID; } }

        /// <summary>
        /// Gets the maximum players on the server.
        /// </summary>
        public short MaxPlayers { get; private set; }

        /// <summary>
        /// Gets the maximum send attempts for this ServerInfoResponsePacket.
        /// </summary>
        public byte MaxSendAttempts { get { return 0; } }

        /// <summary>
        /// Gets the amount of currently online players on the server.
        /// </summary>
        public short OnlinePlayers { get; private set; }

        /// <summary>
        /// Gets the sender of this ServerInfoResponsePacket.
        /// </summary>
        public SenderIs Sender { get { return SenderIs.Server; } }

        /// <summary>
        /// Gets the sender's IPEndPoint of this ServerInfoResponsePacket.
        /// </summary>
        public IPEndPoint SenderEndPoint { get; private set; }

        /// <summary>
        /// Gets the server's name to show in the browser.
        /// </summary>
        public string ServerName { get; private set; }

        /// <summary>
        /// Gets the name of the server's implementation.
        /// </summary>
        public string ServerImplementation { get; private set; }

        /// <summary>
        /// Gets the time in milliseconds before a resend attempt is made.
        /// </summary>
        public ushort TimeUntilResend { get { return 0; } }


        /// <summary>
        /// Initialises a new instance of the ServerInfoResponsePacket class using network data.
        /// </summary>
        /// <param name="data">The network data describing this </param>
        /// <param name="senderEndPoint">The IPEndPoint of the sender.</param>
        public ServerInfoResponsePacket(byte[] data, IPEndPoint senderEndPoint)
        {
            int currentIndex = 2; // Skip first two bytes for ID

            ClientCompatible = ByteParse.NextBool(data, ref currentIndex);
            MaxPlayers = ByteParse.NextShort(data, ref currentIndex);
            OnlinePlayers = ByteParse.NextShort(data, ref currentIndex);
            ServerImplementation = ByteParse.NextStringByLength(data, ref currentIndex, 1);
            ServerName = ByteParse.NextStringByLength(data, ref currentIndex, 1);

            if (String.IsNullOrWhiteSpace(ServerName))
                ServerName = "A RozWorld Server";

            SenderEndPoint = senderEndPoint;
        }

        /// <summary>
        /// Initialises a new instance of the ServerInfoResponsePacket class with specified properties.
        /// </summary>
        public ServerInfoResponsePacket(bool clientCompatible, short maxPlayers, short onlinePlayers, string serverImplementation, string serverName)
        {
            if (!serverName.LengthWithinRange(1, 128) ||
                !serverImplementation.LengthWithinRange(1, 128))
                throw new ArgumentException("ServerInfoResponsePacket.New: Invalid server name/implementation length.");

            ClientCompatible = clientCompatible;
            MaxPlayers = maxPlayers;
            OnlinePlayers = onlinePlayers;
            ServerImplementation = serverImplementation;
            ServerName = serverName;
        }


        /// <summary>
        /// Creates an exact copy of this ServerInfoResponsePacket.
        /// </summary>
        /// <returns>The ServerInfoResponsePacket this method creates, cast as an object.</returns>
        public object Clone()
        {
            return new ServerInfoResponsePacket(ClientCompatible, MaxPlayers, OnlinePlayers,
                ServerImplementation, ServerName);
        }

        /// <summary>
        /// Gets the data in this ServerInfoResponsePacket as a byte array.
        /// </summary>
        /// <returns>A byte array containing the data in this ServerInfoResponsePacket.</returns>
        public byte[] GetBytes()
        {
            var data = new List<byte>();

            data.AddRange(ID.GetBytes());
            data.AddRange(ClientCompatible.GetBytes());
            data.AddRange(MaxPlayers.GetBytes());
            data.AddRange(OnlinePlayers.GetBytes());
            data.AddRange(ServerImplementation.GetBytesByLength(1));
            data.AddRange(ServerName.GetBytesByLength(1));

            return data.ToArray();
        }
    }
}
