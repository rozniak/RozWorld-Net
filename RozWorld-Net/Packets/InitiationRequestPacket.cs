/**
 * Oddmatics.RozWorld.Net.Packets.InitiationRequestPacket -- RozWorld Server Initiation Request Packet
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
    /// [CLIENT --> SERVER] Represents an initiation request packet.
    /// </summary>
    public class InitiationRequestPacket : IPacket
    {
        /// <summary>
        /// Gets the ID of this InitiationRequestPacket.
        /// </summary>
        public ushort Id { get { return PacketType.INITIATION_ID; } }

        /// <summary>
        /// Gets or sets the type of context to request the initiation of.
        /// </summary>
        public readonly SessionContext InitiationContextType;

        /// <summary>
        /// Gets the maximum send attempts for this InitiationRequestPacket.
        /// </summary>
        public byte MaxSendAttempts { get { return PacketTimeout.RESEND_ATTEMPTS_AUTH; } }

        /// <summary>
        /// Gets the sender of this InitiationRequestPacket.
        /// </summary>
        public SenderIs Sender { get { return SenderIs.Client; } }

        /// <summary>
        /// Gets the sender's IPEndPoint of this InitiationRequestPacket.
        /// </summary>
        public IPEndPoint SenderEndPoint { get; private set; }

        /// <summary>
        /// Gets the time in milliseconds before a resend attempt is made.
        /// </summary>
        public ushort TimeUntilResend { get { return PacketTimeout.RESEND_TIMEOUT_AUTH; } }


        /// <summary>
        /// Initialises a new instance of the InitiationRequestPacket class using network data.
        /// </summary>
        /// <param name="data">The network data describing this InitiationRequestPacket.</param>
        /// <param name="senderEndPoint">The IPEndPoint of the sender.</param>
        public InitiationRequestPacket(byte[] data, IPEndPoint senderEndPoint)
        {
            int currentIndex = 6; // Skip first six bytes for signature and ID
            InitiationContextType = (SessionContext)ByteParse.NextByte(data, ref currentIndex);
        }

        /// <summary>
        /// Initialises a new instance of the InitiationRequestPacket with specified properties.
        /// </summary>
        /// <param name="initiationContextType">The context to request.</param>
        public InitiationRequestPacket(SessionContext initiationContextType)
        {
            InitiationContextType = initiationContextType;
        }


        /// <summary>
        /// Creates an exact copy of this InitiationRequestPacket.
        /// </summary>
        /// <returns>The InitiationRequestPacket this methods creates, cast as an object.</returns>
        public object Clone()
        {
            return new InitiationRequestPacket(InitiationContextType);
        }

        /// <summary>
        /// Gets the data in this InitiationRequestPacket as a byte array.
        /// </summary>
        /// <returns>A byte array containing the data in this InitiationRequestPacket.</returns>
        public byte[] GetBytes()
        {
            var data = new List<byte>();

            data.AddRange(Special.PACKET_SIGNATURE.GetBytes());
            data.AddRange(Id.GetBytes());
            data.Add((byte)InitiationContextType);

            return data.ToArray();
        }
    }
}
