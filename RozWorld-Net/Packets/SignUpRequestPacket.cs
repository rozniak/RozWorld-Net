/**
 * Oddmatics.RozWorld.Net.Packets.SignUpRequestPacket -- RozWorld Server Sign Up Request Packet
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
    /// [CLIENT --> SERVER] Represents a sign up request packet.
    /// </summary>
    public class SignUpRequestPacket : IPacket
    {
        /// <summary>
        /// Gets the ID of this SignUpRequestPacket.
        /// </summary>
        public ushort ID { get { return PacketType.SIGN_UP_ID; } }

        /// <summary>
        /// Gets the maximum send attempts for this SignUpRequestPacket.
        /// </summary>
        public byte MaxSendAttempts { get { return PacketTimeout.RESEND_ATTEMPTS_AUTH; } }

        /// <summary>
        /// Gets the hashed password of the sender of this SignUpRequestPacket.
        /// </summary>
        public byte[] PasswordHash { get; private set; }

        /// <summary>
        /// Gets the sender of this SignUpRequestPacket.
        /// </summary>
        public SenderIs Sender { get { return SenderIs.Client; } }

        /// <summary>
        /// Gets the sender's IPEndPoint of this SignUpRequestPacket.
        /// </summary>
        public IPEndPoint SenderEndPoint { get; private set; }

        /// <summary>
        /// Gets the time in milliseconds before a resend attempt is made.
        /// </summary>
        public ushort TimeUntilResend { get { return PacketTimeout.RESEND_TIMEOUT_AUTH; } }

        /// <summary>
        /// Gets the username of the sender of this SignUpRequestPacket.
        /// </summary>
        public string Username { get; private set; }


        /// <summary>
        /// Initialises a new instance of the LogInRequestPacket class using network data.
        /// </summary>
        /// <param name="data">The network data describing this LogInRequestPacket.</param>
        /// <param name="senderEndPoint">The IPEndPoint of the sender.</param>
        public SignUpRequestPacket(byte[] data, IPEndPoint senderEndPoint)
        {
            int currentIndex = 2; // Skip first two bytes for ID
            Username = ByteParse.NextStringByLength(data, ref currentIndex, 1);
            PasswordHash = new byte[data.Length - 1 - currentIndex];
            Array.Copy(data, currentIndex, PasswordHash, 0, data.Length - 1 - currentIndex);

            SenderEndPoint = senderEndPoint;
        }

        /// <summary>
        /// Initialises a new instance of the LogInRequestPacket with specified properties.
        /// </summary>
        /// <param name="username">The username to use.</param>
        /// <param name="passwordHash">The SHA-256 password hash to use.</param>
        public SignUpRequestPacket(string username, byte[] passwordHash)
        {
            if (username.LengthWithinRange(0, 128))
                throw new ArgumentException("LogInRequestPacket.New: Invalid username length.");

            if (passwordHash.Length != 32)
                throw new ArgumentException("LogInRequestPacket.New: Password hash incorrect length - use SHA256.");

            Username = username;
            PasswordHash = passwordHash;
        }


        /// <summary>
        /// Gets the data in this SignUpRequestPacket as a byte array.
        /// </summary>
        /// <returns>A byte array containing the data in this SignUpRequestPacket.</returns>
        public byte[] GetBytes()
        {
            var data = new List<byte>();

            data.AddRange(ID.GetBytes());
            data.AddRange(Username.GetBytesByLength(1));
            data.AddRange(PasswordHash);

            return data.ToArray();
        }
    }
}
