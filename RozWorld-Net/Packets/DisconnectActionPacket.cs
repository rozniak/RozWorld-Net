/**
 * Oddmatics.RozWorld.Net.Packets.DisconnectActionPacket -- RozWorld Disconnect Action Packet
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
    /// [EITHER] Represents a disconnect action packet.
    /// </summary>
    public class DisconnectActionPacket : IAcknowledgeable
    {
        /// <summary>
        /// Gets the acknowledgement ID for this DisconnectActionPacket.
        /// </summary>
        public ushort AckId { get; private set; }

        /// <summary>
        /// Gets the ID of this DisconnectActionPacket.
        /// </summary>
        public ushort Id { get { return PacketType.DISCONNECT_ID; } }

        /// <summary>
        /// Gets the maximum send attempts for this DisconnectActionPacket.
        /// </summary>
        public byte MaxSendAttempts { get { return PacketTimeout.RESEND_ATTEMPTS_AUTH; } }

        /// <summary>
        /// Gets the reason for the disconnect.
        /// </summary>
        public byte Reason { get; private set; }

        /// <summary>
        /// Gets the sender of this DisconnectActionPacket.
        /// </summary>
        public SenderIs Sender { get { return SenderIs.Either; } }

        /// <summary>
        /// Gets the sender's IPEndPoint of this DisconnectActionPacket.
        /// </summary>
        public IPEndPoint SenderEndPoint { get; private set; }

        /// <summary>
        /// Gets the time in milliseconds before a resend attempt is made.
        /// </summary>
        public ushort TimeUntilResend { get { return PacketTimeout.RESEND_TIMEOUT_AUTH; } }


        /// <summary>
        /// Initialises a new instance of the DisconnectActionPacket class using network data.
        /// </summary>
        /// <param name="data">The network data describing this DisconnectActionPacket.</param>
        /// <param name="senderEndPoint">The IPEndPoint of the sender.</param>
        public DisconnectActionPacket(byte[] data, IPEndPoint senderEndPoint)
        {
            int currentIndex = 6; // Skip first six bytes for signature and ID
            Reason = ByteParse.NextByte(data, ref currentIndex);
            AckId = ByteParse.NextUShort(data, ref currentIndex);

            SenderEndPoint = senderEndPoint;
        }

        /// <summary>
        /// Initialises a new instance of the DisconnectActionPacket class with a specified reason.
        /// </summary>
        /// <param name="reason">The reason for the disconnect.</param>
        /// <param name="ackId">The acknowledgement ID to use.</param>
        public DisconnectActionPacket(byte reason, ushort ackId)
        {
            Reason = reason;
            AckId = ackId;
        }


        /// <summary>
        /// Creates an exact copy of this DisconnectActionPacket
        /// </summary>
        /// <returns>The DisconnectActionPacket this method creates, cast as an object.</returns>
        public object Clone()
        {
            return new DisconnectActionPacket(Reason, AckId);
        }

        /// <summary>
        /// Gets the data in the DisconnectActionPacket as a byte array.
        /// </summary>
        /// <returns>A byte array containing the data in this DisconnectActionPacket.</returns>
        public byte[] GetBytes()
        {
            var data = new List<byte>();

            data.AddRange(Special.PACKET_SIGNATURE.GetBytes());
            data.AddRange(Id.GetBytes());
            data.AddRange(AckId.GetBytes());
            data.Add(Reason);

            return data.ToArray();
        }
    }
}
