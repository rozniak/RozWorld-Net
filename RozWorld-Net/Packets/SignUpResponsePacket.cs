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

namespace Oddmatics.RozWorld.Net.Packets
{
    /// <summary>
    /// [SERVER --> CLIENT] Represents a sign up request packet.
    /// </summary>
    public class SignUpResponsePacket : IPacket
    {
        /// <summary>
        /// Gets the error message describing the failed sign up attempt, if applicable.
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Gets the ID of this SignUpResponsePacket.
        /// </summary>
        public ushort ID { get { return PacketType.SIGN_UP_ID; } }

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
        public bool Success { get; private set; }

        /// <summary>
        /// Gets the username that was registered on the server.
        /// </summary>
        public string Username { get; private set; }


        /// <summary>
        /// Initialises a new instance of the SignUpResponsePacket class using network data.
        /// </summary>
        /// <param name="data">The network data describing this LogInRequestPacket.</param>
        /// <param name="senderEndPoint">The IPEndPoint of the sender.</param>
        public SignUpResponsePacket(byte[] data, IPEndPoint senderEndPoint)
        {
            int currentIndex = 2; // Skip first two bytes for ID
            Success = ByteParse.NextBool(data, ref currentIndex);
            Username = ByteParse.NextStringByLength(data, ref currentIndex, 1);

            if (!Success)
                ErrorMessage = ByteParse.NextStringByLength(data, ref currentIndex, 2);
        }

        /// <summary>
        /// Initialises a new instance of the SignUpResponsePacket class with specified properties.
        /// </summary>
        /// <param name="success">Whether the sign up attempt was a success.</param>
        /// <param name="username">The username that was registered.</param>
        /// <param name="error">A message describing an error, if the sign up attempt failed.</param>
        public SignUpResponsePacket(bool success, string username, string error = "")
        {
            if (username.Length == 0 || username.Length > 127)
                throw new ArgumentException("SignUpResponsePacket.New: Invalid username length.");

            if ((!success && error.Length == 0) || error.Length > (ushort.MaxValue / 2) - 1)
                throw new ArgumentException("SignUpResponsePacket.New: Invalid error message length.");

            ErrorMessage = error;
            Success = success;
            Username = username;
        }


        /// <summary>
        /// Gets the data in this SignUpResponsePacket as a byte array.
        /// </summary>
        /// <returns>A byte array containing the data in this SignUpResponsePacket.</returns>
        public byte[] GetBytes()
        {
            var data = new List<byte>();

            data.AddRange(ID.GetBytes());
            data.AddRange(Success.GetBytes());
            data.AddRange(Username.GetBytesByLength(1));

            if (!Success)
                data.AddRange(ErrorMessage.GetBytesByLength(2));

            return data.ToArray();
        }
    }
}
