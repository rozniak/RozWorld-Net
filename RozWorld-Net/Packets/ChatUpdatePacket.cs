/**
 * Oddmatics.RozWorld.Net.Packets.ChatUpdatePacket -- RozWorld Chat Message Update Packet
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
    /// [SERVER --> CLIENT] Represents a chat message action packet.
    /// </summary>
    public class ChatUpdatePacket : IAcknowledgeable, IPlayerPacket
    {
        /// <summary>
        /// Gets the acknowledgement ID for this ChatUpdatePacket.
        /// </summary>
        public ushort AckId { get; private set; }

        /// <summary>
        /// Gets the ID of this ChatUpdatePacket.
        /// </summary>
        public ushort Id { get { return PacketType.CHAT_MESSAGE_ID; } }

        /// <summary>
        /// Gets the maximum send attempts for this ChatUpdatePacket.
        /// </summary>
        public byte MaxSendAttempts { get { return PacketTimeout.RESEND_ATTEMPTS_MOVEMENTS; } }

        /// <summary>
        /// Gets the chat message being sent.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Gets the player ID associated with this ChatUpdatePacket.
        /// </summary>
        public ushort PlayerId { get; private set; }

        /// <summary>
        /// Gets the sender of this ChatUpdatePacket.
        /// </summary>
        public SenderIs Sender { get { return SenderIs.Server; } }

        /// <summary>
        /// Gets the sender's IPEndPoint of this ChatUpdatePacket.
        /// </summary>
        public IPEndPoint SenderEndPoint { get; private set; }

        /// <summary>
        /// Gets the time in milliseconds before a resend attempt is made.
        /// </summary>
        public ushort TimeUntilResend { get { return PacketTimeout.RESEND_ATTEMPTS_MOVEMENTS; } }


        /// <summary>
        /// Initialises a new instance of the ChatUpdatePacket class using network data.
        /// </summary>
        /// <param name="data">The network data describing this ChatUpdatePacket.</param>
        /// <param name="senderEndPoint">The IPEndPoint of the sender.</param>
        public ChatUpdatePacket(byte[] data, IPEndPoint senderEndPoint)
        {
            int currentIndex = 2; // Skip first two bytes for ID
            AckId = ByteParse.NextUShort(data, ref currentIndex);
            PlayerId = ByteParse.NextUShort(data, ref currentIndex);
            Message = ByteParse.NextStringByLength(data, ref currentIndex,
                2, Encoding.Unicode);

            SenderEndPoint = senderEndPoint;
        }

        /// <summary>
        /// Initialises a new instance of the ChatUpdatePacket class with specified properties.
        /// </summary>
        /// <param name="message">The chat message to send.</param>
        /// <param name="playerId">The ID of the player to update the message with.</param>
        /// <param name="ackId">The acknowledgement ID to use.</param>
        public ChatUpdatePacket(string message, ushort playerId, ushort ackId)
        {
            if (!message.LengthWithinRange(1, 300))
                throw new ArgumentException("ChatUpdatePacket.New: Invalid chat message length.");

            Message = message;
            PlayerId = playerId;
            AckId = ackId;
        }


        /// <summary>
        /// Creates an exact copy of this ChatUpdatePacket.
        /// </summary>
        /// <returns>The ChatUpdatePacket this method creates, cast as an object.</returns>
        public object Clone()
        {
            return new ChatActionPacket(Message, PlayerId, AckId);
        }

        /// <summary>
        /// Gets the data in this ChatUpdatePacket as a byte array.
        /// </summary>
        /// <returns>A byte array containing the data in this ChatUpdatePacket.</returns>
        public byte[] GetBytes()
        {
            var data = new List<byte>();

            data.AddRange(Id.GetBytes());
            data.AddRange(AckId.GetBytes());
            data.AddRange(PlayerId.GetBytes());
            data.AddRange(Message.GetBytesByLength(2, Encoding.Unicode));

            return data.ToArray();
        }
    }
}
