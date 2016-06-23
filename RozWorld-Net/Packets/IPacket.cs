/**
 * Oddmatics.RozWorld.Net.Packets.IPacket -- RozWorld Network Packet
 *
 * This source-code is part of the netcode library for the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld-Net>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System;
using System.Net;

namespace Oddmatics.RozWorld.Net.Packets
{
    /// <summary>
    /// Represents a network packet.
    /// </summary>
    public interface IPacket
    {
        /// <summary>
        /// Gets the ID of this IPacket.
        /// </summary>
        ushort ID { get; }

        /// <summary>
        /// Gets the maximum send attempts for this IPacket.
        /// For infinite send attempts (until the total server timeout), set to 255.
        /// </summary>
        byte MaxSendAttempts { get; }

        /// <summary>
        /// Gets the sender of this IPacket.
        /// </summary>
        SenderIs Sender { get; }

        /// <summary>
        /// Gets the sender's IPEndPoint.
        /// </summary>
        IPEndPoint SenderEndPoint { get; }

        /// <summary>
        /// Gets the time in milliseconds before a resend attempt is made.
        /// </summary>
        ushort TimeUntilResend { get; }


        /// <summary>
        /// Gets this IPacket's data as bytes ready to transmit.
        /// </summary>
        /// <returns>This IPacket's data as bytes.</returns>
        byte[] GetBytes();
    }
}
