/**
 * Oddmatics.RozWorld.Net.Packets.LogInRequestPacket -- RozWorld Chat Message Action Packet
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
    /// [EITHER] Represents a chat message action packet.
    /// </summary>
    public class ChatPacket : IAcknowledgeable
    {
        /// <summary>
        /// Gets the acknowledgement ID for this ChatPacket.
        /// </summary>
        public ushort AckId { get; private set; }

        /// <summary>
        /// Gets the ID of this ChatPacket.
        /// </summary>
        public ushort Id { get { return PacketType.CHAT_MESSAGE_ID; } }

        /// <summary>
        /// Gets the maximum send attempts for this ChatPacket.
        /// </summary>
        public byte MaxSendAttempts { get { return PacketTimeout.RESEND_ATTEMPTS_CHAT; } }

        /// <summary>
        /// Gets the chat message being sent.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Gets the sender of this ChatPacket.
        /// </summary>
        public SenderIs Sender { get { return SenderIs.Client; } }

        /// <summary>
        /// Gets the sender's IPEndPoint of this ChatPacket.
        /// </summary>
        public IPEndPoint SenderEndPoint { get; private set; }

        /// <summary>
        /// Gets the time in milliseconds before a resend attempt is made.
        /// </summary>
        public ushort TimeUntilResend { get { return PacketTimeout.RESEND_TIMEOUT_AUTH; } }

        /// <summary>
        /// Gets the username associated with this ChatPacket.
        /// </summary>
        public string Username { get; private set; }


        /// <summary>
        /// Initialises a new instance of the ChatPacket class using network data. 
        /// </summary>
        /// <param name="data">The network data describing this ChatPacket.</param>
        /// <param name="senderEndPoint">The IPEndPoint of the sender.</param>
        public ChatPacket(byte[] data, IPEndPoint senderEndPoint)
        {
            int currentIndex = 6; // Skip first six bytes for signature and ID
            AckId = ByteParse.NextUShort(data, ref currentIndex);
            Username = ByteParse.NextStringByLength(data, ref currentIndex, 1, Encoding.UTF8);
            Message = ByteParse.NextStringByLength(data, ref currentIndex,
                2, Encoding.Unicode);

            SenderEndPoint = senderEndPoint;
        }

        /// <summary>
        /// Initialises a new instance of the ChatPacket class with specified properties.
        /// </summary>
        /// <param name="message">The chat message to send.</param>
        /// <param name="playerId">The username of the player sending the message.</param>
        /// <param name="ackId">The acknowledgement ID to use.</param>
        public ChatPacket(string message, string username, ushort ackId)
        {
            if (!message.LengthWithinRange(1, 256))
                throw new ArgumentException("ChatActionPacket.New: Invalid chat message length.");

            Message = message;
            Username = username;
            AckId = ackId;
        }


        /// <summary>
        /// Creates an exact copy of this ChatPacket.
        /// </summary>
        /// <returns>The ChatPacket this method creates, cast as an object.</returns>
        public object Clone()
        {
            return new ChatPacket(Message, Username, AckId);
        }

        /// <summary>
        /// Gets the data in this ChatPacket as a byte array.
        /// </summary>
        /// <returns>A byte array containing the data in this ChatPacket.</returns>
        public byte[] GetBytes()
        {
            var data = new List<byte>();

            data.AddRange(Special.PACKET_SIGNATURE.GetBytes());
            data.AddRange(Id.GetBytes());
            data.AddRange(AckId.GetBytes());
            data.AddRange(Username.GetBytesByLength(1, Encoding.UTF8));
            data.AddRange(Message.GetBytesByLength(2, Encoding.Unicode));

            return data.ToArray();
        }
    }
}
