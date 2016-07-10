/**
 * Oddmatics.RozWorld.Net.Packets.LogInRequestPacket -- RozWorld Log In Request Packet
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
    /// [CLIENT --> SERVER] Represents a chat
    /// </summary>
    public class ChatActionPacket : IAcknowledgeable, IPlayerPacket
    {
        /// <summary>
        /// Gets the acknowledgement ID for this ChatActionPacket.
        /// </summary>
        public ushort AckId { get; private set; }

        /// <summary>
        /// Gets the ID of this ChatActionPacket.
        /// </summary>
        public ushort Id { get { return PacketType.CHAT_MESSAGE_ID; } }

        /// <summary>
        /// Gets the maximum send attempts for this ChatActionPacket.
        /// </summary>
        public byte MaxSendAttempts { get { return PacketTimeout.RESEND_ATTEMPTS_MOVEMENTS; } }

        /// <summary>
        /// Gets the chat message being sent.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Gets the player ID associated with this IPlayerPacket.
        /// </summary>
        public ushort PlayerId { get; private set; }

        /// <summary>
        /// Gets the sender of this ChatActionPacket.
        /// </summary>
        public SenderIs Sender { get { return SenderIs.Client; } }

        /// <summary>
        /// Gets the sender's IPEndPoint of this ChatActionPacket.
        /// </summary>
        public IPEndPoint SenderEndPoint { get; private set; }

        /// <summary>
        /// Gets the time in milliseconds before a resend attempt is made.
        /// </summary>
        public ushort TimeUntilResend { get { return PacketTimeout.RESEND_TIMEOUT_AUTH; } }


        public ChatActionPacket(byte[] data, IPEndPoint senderEndPoint)
        {
            int currentIndex = 2; // Skip first two bytes for ID
            AckId = ByteParse.NextUShort(data, ref currentIndex);
            PlayerId = ByteParse.NextUShort(data, ref currentIndex);
            Message = ByteParse.NextStringByLength(data, ref currentIndex,
                2, Encoding.Unicode);

            SenderEndPoint = senderEndPoint;
        }

        public ChatActionPacket(string message, ushort playerId, ushort ackId)
        {
            if (!message.LengthWithinRange(1, 256))
                throw new ArgumentException("ChatActionPacket.New: Invalid chat message length.");

            Message = message;
            PlayerId = playerId;
            AckId = ackId;
        }


        public object Clone()
        {
            return new ChatActionPacket(Message, PlayerId, AckId);
        }

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
