/**
 * Oddmatics.RozWorld.Net.Packets.ServerDisconnectPacket -- RozWorld Server Disconnect Packet
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
using System.Text;

namespace Oddmatics.RozWorld.Net.Packets
{
    /// <summary>
    /// [SERVER --> CLIENT] Represents a server disconnect packet.
    /// </summary>
    public class ServerDisconnectPacket : IPacket
    {
        /// <summary>
        /// Getsthe ID of this ServerDisconnectPacket.
        /// </summary>
        public ushort Id { get { return PacketType.DISCONNECT_ID; } }

        /// <summary>
        /// Gets the maximum send attempts for this ServerDisconnectPacket.
        /// </summary>
        public byte MaxSendAttempts { get { return PacketTimeout.RESEND_ATTEMPTS_AUTH; } }

        /// <summary>
        /// Gets the reason for this disconnection.
        /// </summary>
        public byte Reason { get; private set; }

        /// <summary>
        /// Gets the sender of this ServerDisconnectPacket.
        /// </summary>
        public SenderIs Sender { get { return SenderIs.Client; } }

        /// <summary>
        /// Gets the sender's IPEndPoint of this ServerDisconnectPacket.
        /// </summary>
        public IPEndPoint SenderEndPoint { get; private set; }

        /// <summary>
        /// Gets the time in milliseconds before a resend attempt is made.
        /// </summary>
        public ushort TimeUntilResend { get { return PacketTimeout.RESEND_TIMEOUT_AUTH; } }

        /// <summary>
        /// Gets the username of the player on the client being disconnected.
        /// </summary>
        public string Username { get; private set; }


        /// <summary>
        /// Initialises a new instance of the ServerDisconnectPacket class using network data.
        /// </summary>
        /// <param name="data">The network data describing this ServerDisconnectPacket.</param>
        /// <param name="senderEndPoint">The IPEndPoint of the sender.</param>
        public ServerDisconnectPacket(byte[] data, IPEndPoint senderEndPoint)
        {
            int currentIndex = 2; // Skip the first two bytes for ID
            Reason = ByteParse.NextByte(data, ref currentIndex);
            Username = ByteParse.NextStringByLength(data, ref currentIndex, 1, Encoding.UTF8);

            SenderEndPoint = senderEndPoint;
        }

        /// <summary>
        /// Initialises a new instance of the ServerInfoRequestPacket class with specified properties.
        /// </summary>
        /// <param name="reason">The reason for the disconnect.</param>
        /// <param name="username">The username of the player on the client to disconnect.</param>
        public ServerDisconnectPacket(byte reason, string username)
        {
            if (!username.LengthWithinRange(1, 256))
                throw new ArgumentException("ServerDisconnectPacket.New: Invalid username length.");

            Reason = reason;
            Username = username;
        }


        /// <summary>
        /// Creates an exact copy of this ClientDisconnectPacket.
        /// </summary>
        /// <returns>The ClientDisconnectPacket this method creates, cast as an object.</returns>
        public object Clone()
        {
            return new ServerDisconnectPacket(Reason, Username);
        }

        /// <summary>
        /// Gets the data in this ClientDisconnectPacket as a byte array.
        /// </summary>
        /// <returns>A byte array containing the data in this ClientDisconnectPacket.</returns>
        public byte[] GetBytes()
        {
            var data = new List<byte>();

            data.AddRange(Id.GetBytes());
            data.Add(Reason);
            data.AddRange(Username.GetBytesByLength(1, Encoding.UTF8));

            return data.ToArray();
        }
    }
}
