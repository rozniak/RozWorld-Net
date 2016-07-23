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
using System.Text;

namespace Oddmatics.RozWorld.Net.Packets
{
    /// <summary>
    /// [CLIENT --> SERVER] Represents a log in request packet.
    /// </summary>
    public class LogInRequestPacket : IPacket
    {
        /// <summary>
        /// Gets whether the sender is logging in as chat only.
        /// </summary>
        public bool ChatOnly { get; private set; }

        /// <summary>
        /// Gets the ID of this LogInRequestPacket.
        /// </summary>
        public ushort Id { get { return PacketType.LOG_IN_ID; } }

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
        /// Gets the time the password was hashed in ticks.
        /// </summary>
        public long UtcHashTime { get; private set; }

        /// <summary>
        /// Gets whether the time of the hash is acceptable for logging in.
        /// Acceptable time difference is less than 5 seconds.
        /// </summary>
        public bool ValidHashTime
        {
            get
            {
                long utcTicks = DateTime.UtcNow.Ticks;
                return utcTicks - UtcHashTime < TimeSpan.TicksPerSecond * 5;
            }
        }


        /// <summary>
        /// Initialises a new instance of the LogInRequestPacket class using network data.
        /// </summary>
        /// <param name="data">The network data describing this LogInRequestPacket.</param>
        /// <param name="senderEndPoint">The IPEndPoint of the sender.</param>
        public LogInRequestPacket(byte[] data, IPEndPoint senderEndPoint)
        {
            int currentIndex = 2; // Skip first two bytes for ID
            ChatOnly = ByteParse.NextBool(data, ref currentIndex);
            SkinDownloads = ByteParse.NextBool(data, ref currentIndex);
            Username = ByteParse.NextStringByLength(data, ref currentIndex, 1, Encoding.UTF8);
            UtcHashTime = ByteParse.NextLong(data, ref currentIndex);
            PasswordHash = new byte[data.Length - currentIndex];
            Array.Copy(data, currentIndex, PasswordHash, 0, data.Length - currentIndex);

            SenderEndPoint = senderEndPoint;
        }

        /// <summary>
        /// Initialises a new instance of the LogInRequestPacket class with specified properties.
        /// </summary>
        /// <param name="username">The username to log in as.</param>
        /// <param name="password">The password to log in with.</param>
        /// <param name="chatOnly">Whether to log on in chat-only mode.</param>
        /// <param name="skinDownloads">Whether skin downloads are accepted.</param>
        public LogInRequestPacket(string username, string password, bool chatOnly, bool skinDownloads)
        {
            if (!username.LengthWithinRange(1, 256))
                throw new ArgumentException("LogInRequestPacket.New: Invalid username length.");

            var sha256 = new SHA256Managed();
            byte[] passwordHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

            long ticks = DateTime.UtcNow.Ticks;

            var hashByteList = new List<byte>(passwordHash);
            hashByteList.AddRange(ticks.GetBytes());

            ChatOnly = chatOnly;
            PasswordHash = sha256.ComputeHash(hashByteList.ToArray());
            SkinDownloads = skinDownloads;
            Username = username;
            UtcHashTime = ticks;
        }

        /// <summary>
        /// Initialises a new instance of the LogInRequestPacket class with specified properties.
        /// </summary>
        /// <param name="username">The username to log in as.</param>
        /// <param name="passwordHash">The SHA-256 password hash to check against.</param>
        /// <param name="utcHashTime">The UTC time in ticks when the password hash was created.</param>
        /// <param name="chatOnly">Whether to log on in chat-only mode.</param>
        /// <param name="skinDownloads">Whether skin downloads are accepted.</param>
        private LogInRequestPacket(string username, byte[] passwordHash, long utcHashTime, bool chatOnly, bool skinDownloads)
        {
            if (!username.LengthWithinRange(1, 256))
                throw new ArgumentException("LogInRequestPacket.New: Invalid username length.");

            if (passwordHash.Length != 32)
                throw new ArgumentException("LogInRequestPacket.New: Password hash incorrect length - use SHA256.");

            ChatOnly = chatOnly;
            SkinDownloads = skinDownloads;
            PasswordHash = passwordHash;
            Username = username;
            UtcHashTime = utcHashTime;
        }


        /// <summary>
        /// Creates an exact copy of this LogInRequestPacket.
        /// </summary>
        /// <returns>The LogInRequestPacket this method creates, cast as an object.</returns>
        public object Clone()
        {
            return new LogInRequestPacket(Username, PasswordHash, UtcHashTime, ChatOnly, SkinDownloads);
        }

        /// <summary>
        /// Gets the data in this LogInRequestPacket as a byte array.
        /// </summary>
        /// <returns>A byte array containing the data in this LogInRequestPacket.</returns>
        public byte[] GetBytes()
        {
            var data = new List<byte>();

            data.AddRange(Id.GetBytes());
            data.AddRange(ChatOnly.GetBytes());
            data.AddRange(SkinDownloads.GetBytes());
            data.AddRange(Username.GetBytesByLength(1, Encoding.UTF8));
            data.AddRange(UtcHashTime.GetBytes());
            data.AddRange(PasswordHash);

            return data.ToArray();
        }
    }
}
