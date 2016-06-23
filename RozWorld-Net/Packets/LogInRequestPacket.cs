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
using System.Security.Cryptography;

namespace Oddmatics.RozWorld.Net.Packets
{
    /// <summary>
    /// [CLIENT --> SERVER] Represents a log in request packet.
    /// </summary>
    public class LogInRequestPacket : IPacket
    {
        /// <summary>
        /// Gets the ID of this LogInRequestPacket.
        /// </summary>
        public ushort ID { get { return PacketType.LOG_IN_ID; } }

        /// <summary>
        /// Gets the maximum send attempts for this LogInRequestPacket.
        /// </summary>
        public byte MaxSendAttempts { get { return PacketTimeout.RESEND_ATTEMPTS_AUTH; } }

        /// <summary>
        /// Gets the hashed password of the sender of this LogInRequestPacket.
        /// </summary>
        public byte[] PasswordHash { get; private set; }

        /// <summary>
        /// Gets the sender of this LogInRequestPacket.
        /// </summary>
        public SenderIs Sender { get { return SenderIs.Client; } }

        /// <summary>
        /// Gets the sender's IPEndPoint of this LogInRequestPacket.
        /// </summary>
        public IPEndPoint SenderEndPoint { get; private set; }

        /// <summary>
        /// Gets whether the sender has skin downloads enabled.
        /// </summary>
        public bool SkinDownloads { get; private set; }

        /// <summary>
        /// Gets the time in milliseconds before a resend attempt is made.
        /// </summary>
        public ushort TimeUntilResend { get { return PacketTimeout.RESEND_TIMEOUT_AUTH; } }

        /// <summary>
        /// Gets the username of the sender of this LogInRequestPacket.
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// Gets the UTC time in ticks that the password was hashed.
        /// </summary>
        public long UtcHashTime { get; private set; }

        /// <summary>
        /// Gets whether the time of the hash is acceptable for logging in.
        /// </summary>
        public bool ValidHashTime { get { return UtcHashTime >= DateTime.UtcNow.AddMinutes(-0.25).Ticks; } }


        /// <summary>
        /// Initialises a new instance of the LogInRequestPacket class using network data.
        /// </summary>
        /// <param name="data">The network data describing this LogInRequestPacket.</param>
        /// <param name="senderEndPoint">The IPEndPoint of the sender.</param>
        public LogInRequestPacket(byte[] data, IPEndPoint senderEndPoint)
        {
            int currentIndex = 2; // Skip first two bytes for ID
            SkinDownloads = ByteParse.NextBool(data, ref currentIndex);
            Username = ByteParse.NextStringByLength(data, ref currentIndex, 1);
            UtcHashTime = ByteParse.NextLong(data, ref currentIndex);
            data.CopyTo(PasswordHash, currentIndex);

            SenderEndPoint = senderEndPoint;
        }

        /// <summary>
        /// Initialises a new instance of the LogInRequestPacket class with specified properties.
        /// </summary>
        /// <param name="skinDownloads">Whether skin downloads are accepted.</param>
        /// <param name="username">The username to log in as.</param>
        /// <param name="utcHashTime">The time in ticks of the hashed password.</param>
        /// <param name="passwordHash">The SHA-256 password hash to check against.</param>
        public LogInRequestPacket(bool skinDownloads, string username, long utcHashTime, byte[] passwordHash)
        {
            if (username.Length == 0 || username.Length > 127)
                throw new ArgumentException("LogInRequestPacket.New: Invalid username length.");

            if (passwordHash.Length != 32)
                throw new ArgumentException("LogInRequestPacket.New: Password hash incorrect length - use SHA256.");

            SkinDownloads = skinDownloads;
            PasswordHash = passwordHash;
            Username = username;
            UtcHashTime = utcHashTime;
        }


        /// <summary>
        /// Gets the data in this LogInRequestPacket as a byte array.
        /// </summary>
        /// <returns>A byte array containing the data in this LogInRequestPacket.</returns>
        public byte[] GetBytes()
        {
            var data = new List<byte>();

            data.AddRange(ID.GetBytes());
            data.AddRange(Username.GetBytesByLength(1));
            data.AddRange(UtcHashTime.GetBytes());
            data.AddRange(PasswordHash);

            return data.ToArray();
        }
    }
}
