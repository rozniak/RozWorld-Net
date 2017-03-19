/**
 * Oddmatics.RozWorld.Net.Packets.InitiationResponsePacket -- RozWorld Server Initiation Request Packet
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
    /// [SERVER --> CLIENT] Represents a sign up response packet.
    /// </summary>
    public class InitiationResponsePacket : IPacket
    {
        /// <summary>
        /// Gets the error message ID describing the reason for the initiation failure, if applicable.
        /// </summary>
        public readonly byte ErrorMessageId;

        /// <summary>
        /// Gets the ID of this InitiationResponsePacket.
        /// </summary>
        public ushort Id { get { return PacketType.INITIATION_ID; } }

        /// <summary>
        /// Gets the maximum send attempts for this InitiationResponsePacket.
        /// </summary>
        public byte MaxSendAttempts { get { return 0; } }

        /// <summary>
        /// Gets the sender of this InitiationResponsePacket.
        /// </summary>
        public SenderIs Sender { get { return SenderIs.Server; } }

        /// <summary>
        /// Gets the sender's IPEndPoint of InitiationResponsePacket.
        /// </summary>
        public IPEndPoint SenderEndPoint { get; private set; }

        /// <summary>
        /// Gets whether the initiation attempt was a success.
        /// </summary>
        public bool Success { get { return ErrorMessageId == ErrorMessage.NO_ERROR; } }

        /// <summary>
        /// Gets the time in milliseconds before a resend attempt is made.
        /// </summary>
        public ushort TimeUntilResend { get { return 0; } }

        /// <summary>
        /// Gets the token assigned to this response, if the initiation was successful.
        /// </summary>
        public readonly uint Token;


        /// <summary>
        /// Initialises a new instance of the InitiationResponsePacket class using network data.
        /// </summary>
        /// <param name="data">The network data describing this InitiationResponsePacket.</param>
        /// <param name="senderEndPoint">The IPEndPoint of the sender.</param>
        public InitiationResponsePacket(byte[] data, IPEndPoint senderEndPoint)
        {
            int currentIndex = 6; // Skip first six bytes for signature and ID
            ErrorMessageId = ByteParse.NextByte(data, ref currentIndex);
            Token = ByteParse.NextUInt(data, ref currentIndex);
        }

        /// <summary>
        /// Initialises a new instance of the InitiationResponsePacket class with specified properties.
        /// </summary>
        /// <param name="errorId">The error message ID for this attempt.</param>
        /// <param name="token">The token to define this communication context, 0 if this was a failure.</param>
        public InitiationResponsePacket(byte errorId, uint token)
        {
            ErrorMessageId = errorId;
            Token = token;
        }
        

        /// <summary>
        /// Creates an exact copy of this InitiationResponsePacket.
        /// </summary>
        /// <returns>The InitiationResponsePacket this method creates, cast as an object.</returns>
        public object Clone()
        {
            return new InitiationResponsePacket(ErrorMessageId, Token);
        }

        /// <summary>
        /// Gets the data in this InitiationResponsePacket as a byte array.
        /// </summary>
        /// <returns>A byte array containing the data in this InitiationResponsePacket.</returns>
        public byte[] GetBytes()
        {
            var data = new List<byte>();

            data.AddRange(Special.PACKET_SIGNATURE.GetBytes());
            data.AddRange(Id.GetBytes());
            data.Add(ErrorMessageId);
            data.AddRange(Token.GetBytes());

            return data.ToArray();
        }
    }
}
