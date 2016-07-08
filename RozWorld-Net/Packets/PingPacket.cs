/**
 * Oddmatics.RozWorld.Net.Packets.PingPacket -- RozWorld Network Keepalive Ping Packet
 *
 * This source-code is part of the netcode library for the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld-Net>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using Oddmatics.Util.IO;
using System.Collections.Generic;
using System.Net;

namespace Oddmatics.RozWorld.Net.Packets
{
    /// <summary>
    /// [EITHER] Represents a ping packet for keeping UDP hole punched connections alive.
    /// </summary>
    public class PingPacket : IPacket
    {
        /// <summary>
        /// Gets the ID of this PingPacket.
        /// </summary>
        public ushort Id { get { return PacketType.PING_ID; } }

        /// <summary>
        /// Gets the maximum send attempts for this PingPacket.
        /// </summary>
        public byte MaxSendAttempts { get { return 0; } }

        /// <summary>
        /// Gets the sender of this PingPacket.
        /// </summary>
        public SenderIs Sender { get { return SenderIs.Either; } }

        /// <summary>
        /// Gets the sender's IPEndPoint of this PingPacket.
        /// </summary>
        public IPEndPoint SenderEndPoint { get; private set; }

        /// <summary>
        /// Gets the time in milliseconds before a resend attempt is made.
        /// </summary>
        public ushort TimeUntilResend { get { return 0; } }


        /// <summary>
        /// Creates an exact copy of this PingPacket.
        /// </summary>
        /// <returns>The PingPacket this method creates, cast as an object.</returns>
        public object Clone()
        {
            return new PingPacket();
        }

        /// <summary>
        /// Gets the data in this PingPacket as a byte array.
        /// </summary>
        /// <returns>A byte array containing the data in this PingPacket.</returns>
        public byte[] GetBytes()
        {
            return Id.GetBytes(); // Only need the ID for pings
        }
    }
}
