/**
 * Oddmatics.RozWorld.Net.Packets.LogInResponsePacket -- RozWorld Log In Response Packet
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
    /// [SERVER --> CLIENT] Represents a log in response packet.
    /// </summary>
    public class LogInResponsePacket : IPacket
    {
        /// <summary>
        /// Gets the error message ID describing the reason for the sign up failure, if applicable.
        /// </summary>
        public byte ErrorMessageId { get; private set; }

        /// <summary>
        /// Gets the ID of this LogInResponsePacket.
        /// </summary>
        public ushort Id { get { return PacketType.LOG_IN_ID; } }

        /// <summary>
        /// Gets the maximum send attempts for this LogInResponsePacket.
        /// </summary>
        public byte MaxSendAttempts { get { return 0; } }

        /// <summary>
        /// Gets the sender of this LogInResponsePacket.
        /// </summary>
        public SenderIs Sender { get { return SenderIs.Server; } }

        /// <summary>
        /// Gets the sender's IPEndPoint of this LogInResponsePacket.
        /// </summary>
        public IPEndPoint SenderEndPoint { get; private set; }

        /// <summary>
        /// Gets whether the log in attempt was a success.
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// Gets the time in milliseconds before a resend attempt is made.
        /// </summary>
        public ushort TimeUntilResend { get { return 0; } }

        /// <summary>
        /// Gets the username that was logged into.
        /// </summary>
        public string Username { get; private set; }


        /// <summary>
        /// Initialises a new instance of the LogInResponsePacket class using network data.
        /// </summary>
        /// <param name="data">The network data describing this LogInResponsePacket.</param>
        /// <param name="senderEndPoint">The IPEndPoint of the sender.</param>
        public LogInResponsePacket(byte[] data, IPEndPoint senderEndPoint)
        {
            int currentIndex = 2; // Skip first two bytes for ID
            Success = ByteParse.NextBool(data, ref currentIndex);
            Username = ByteParse.NextStringByLength(data, ref currentIndex, 1);
            ErrorMessageId = ByteParse.NextByte(data, ref currentIndex);
        }

        /// <summary>
        /// Initialises a new instance of the LogInResponsePacket class with specified properties.
        /// </summary>
        /// <param name="success">Whether the log in attempt was a success.</param>
        /// <param name="username">The username that was registered.</param>
        /// <param name="errorId">The error message ID, if the log in attempt failed.</param>
        public LogInResponsePacket(bool success, string username, byte errorId)
        {
            if (!username.LengthWithinRange(1, 128))
                throw new ArgumentException("LogInResponsePacket.New: Invalid username length.");

            ErrorMessageId = errorId;
            Success = success;
            Username = username;
        }


        /// <summary>
        /// Creates an exact copy of this LogInResponsePacket.
        /// </summary>
        /// <returns>The LogInResponsePacket this method creates, cast as an object.</returns>
        public object Clone()
        {
            return new LogInResponsePacket(Success, Username, ErrorMessageId);
        }

        /// <summary>
        /// Gets the data in this LogInResponsePacket as a byte array.
        /// </summary>
        /// <returns>A byte array containing the data in this LogInResponsePacket.</returns>
        public byte[] GetBytes()
        {
            var data = new List<byte>();

            data.AddRange(Id.GetBytes());
            data.AddRange(Success.GetBytes());
            data.AddRange(Username.GetBytesByLength(1));
            data.Add(ErrorMessageId);

            return data.ToArray();
        }
    }
}
