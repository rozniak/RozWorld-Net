/**
 * Oddmatics.RozWorld.Net.Packets.AcknowledgePacket -- RozWorld Acknowledgement Packet
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
    /// [EITHER] Represents an acknowledgement packet.
    /// </summary>
    public class AcknowledgePacket : IAcknowledgeable
    {
        /// <summary>
        /// Gets the packet's acknowledgement ID that was acknowledged.
        /// </summary>
        public ushort AckId { get; private set; }

        /// <summary>
        /// Gets the ID of this AcknowledgePacket.
        /// </summary>
        public ushort Id { get { return PacketType.ACK_ID; } }

        /// <summary>
        /// Get the maximum send attempts for this AcknowledgePacket.
        /// </summary>
        public byte MaxSendAttempts { get { return 0; } }

        /// <summary>
        /// Gets the sender of this AcknowledgePacket.
        /// </summary>
        public SenderIs Sender { get { return SenderIs.Either; } }

        /// <summary>
        /// Gets the sender's IPEndPoint of this AcknowledgePacket.
        /// </summary>
        public IPEndPoint SenderEndPoint { get; private set; }

        /// <summary>
        /// Gets the time in milliseconds before a resend attempt is made.
        /// </summary>
        public ushort TimeUntilResend { get { return 0; } }


        /// <summary>
        /// Initialises a new instance of the AcknowledgePacket class using network data.
        /// </summary>
        /// <param name="data">The network data describing this AcknowledgePacket.</param>
        /// <param name="senderEndPoint">The IPEndPoint of the sender.</param>
        public AcknowledgePacket(byte[] data, IPEndPoint senderEndPoint)
        {
            int currentIndex = 2; // Skip first two bytes for ID
            AckId = ByteParse.NextUShort(data, ref currentIndex);

            SenderEndPoint = senderEndPoint;
        }

        /// <summary>
        /// Initialises a new instance of the AcknowledgePacket class with a specified acknowledgement ID.
        /// </summary>
        /// <param name="ackId">The acknowledgement ID of the packet to acknowledge.</param>
        public AcknowledgePacket(ushort ackId)
        {
            AckId = ackId;
        }


        /// <summary>
        /// Creates an exact copy of this AcknowledgePacket.
        /// </summary>
        /// <returns>The AcknowledgePacket this method creates, cast as an object.</returns>
        public object Clone()
        {
            return new AcknowledgePacket(AckId);
        }

        /// <summary>
        /// Gets the data in this AcknowledgePacket as a byte array.
        /// </summary>
        /// <returns>A byte array containing the data in this AcknowledgePacket.</returns>
        public byte[] GetBytes()
        {
            var data = new List<byte>();

            data.AddRange(Id.GetBytes());
            data.AddRange(AckId.GetBytes());

            return data.ToArray();
        }
    }
}
