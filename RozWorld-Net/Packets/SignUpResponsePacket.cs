/**
 * Oddmatics.RozWorld.Net.Packets.SignUpResponsePacket -- RozWorld Server Sign Up Response Packet
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
    /// [SERVER --> CLIENT] Represents a sign up response packet.
    /// </summary>
    public class SignUpResponsePacket : IPacket
    {
        /// <summary>
        /// Gets the error message ID describing the reason for the sign up failure, if applicable.
        /// </summary>
        public readonly byte ErrorMessageId;

        /// <summary>
        /// Gets the ID of this SignUpResponsePacket.
        /// </summary>
        public ushort Id { get { return PacketType.SIGN_UP_ID; } }

        /// <summary>
        /// Gets the maximum send attempts for this SignUpResponsePacket.
        /// </summary>
        public byte MaxSendAttempts { get { return 0; } }

        /// <summary>
        /// Gets the sender of this SignUpResponsePacket.
        /// </summary>
        public SenderIs Sender { get { return SenderIs.Server; } }

        /// <summary>
        /// Gets the sender's IPEndPoint of this SignUpResponsePacket.
        /// </summary>
        public IPEndPoint SenderEndPoint { get; private set; }

        /// <summary>
        /// Gets whether the sign up attempt was a success.
        /// </summary>
        public readonly bool Success;

        /// <summary>
        /// Gets the time in milliseconds before a resend attempt is made.
        /// </summary>
        public ushort TimeUntilResend { get { return 0; } }

        /// <summary>
        /// Gets the username that was registered on the server.
        /// </summary>
        public readonly string Username;


        /// <summary>
        /// Initialises a new instance of the SignUpResponsePacket class using network data.
        /// </summary>
        /// <param name="data">The network data describing this LogInRequestPacket.</param>
        /// <param name="senderEndPoint">The IPEndPoint of the sender.</param>
        public SignUpResponsePacket(byte[] data, IPEndPoint senderEndPoint)
        {
            int currentIndex = 6; // Skip first six bytes for signature and ID
            Success = ByteParse.NextBool(data, ref currentIndex);
            Username = ByteParse.NextStringByLength(data, ref currentIndex, 1, Encoding.UTF8);
            ErrorMessageId = ByteParse.NextByte(data, ref currentIndex);
        }

        /// <summary>
        /// Initialises a new instance of the SignUpResponsePacket class with specified properties.
        /// </summary>
        /// <param name="success">Whether the sign up attempt was a success.</param>
        /// <param name="username">The username that was registered.</param>
        /// <param name="errorId">The error message ID, if the sign up attempt failed.</param>
        public SignUpResponsePacket(bool success, string username, byte errorId)
        {
            if (!username.LengthWithinRange(1, 256))
                throw new ArgumentException("SignUpResponsePacket.New: Invalid username length.");

            ErrorMessageId = errorId;
            Success = success;
            Username = username;
        }


        /// <summary>
        /// Creates an exact copy of this SignUpResponsePacket.
        /// </summary>
        /// <returns>The SignUpResponsePacket this method creates, cast as an object.</returns>
        public object Clone()
        {
            return new SignUpResponsePacket(Success, Username, ErrorMessageId);
        }

        /// <summary>
        /// Gets the data in this SignUpResponsePacket as a byte array.
        /// </summary>
        /// <returns>A byte array containing the data in this SignUpResponsePacket.</returns>
        public byte[] GetBytes()
        {
            var data = new List<byte>();

            data.AddRange(Special.PACKET_SIGNATURE.GetBytes());
            data.AddRange(Id.GetBytes());
            data.AddRange(Success.GetBytes());
            data.AddRange(Username.GetBytesByLength(1, Encoding.UTF8));
            data.Add(ErrorMessageId);

            return data.ToArray();
        }
    }
}
