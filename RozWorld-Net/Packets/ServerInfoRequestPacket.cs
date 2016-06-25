/**
 * Oddmatics.RozWorld.Net.Packets.ServerInfoRequestPacket -- RozWorld Server Information Request Packet
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
    /// [CLIENT --> SERVER] Represents a server info request packet.
    /// </summary>
    public class ServerInfoRequestPacket : IPacket
    {
        /// <summary>
        /// Gets the name of the client's implementation.
        /// </summary>
        public string ClientImplementation { get; private set; }

        /// <summary>
        /// Gets the client's version number in formatted form.
        /// </summary>
        public string ClientVersion
        {
            get
            {
                string versionName = ClientVersionRaw.ToString().PadLeft(3, '0');
                return versionName.Insert(versionName.Length - 2, ".").Insert(versionName.Length, ".");
            }
        }

        /// <summary>
        /// Gets the client's version number in its unformatted representation.
        /// </summary>
        public ushort ClientVersionRaw { get; private set; }

        /// <summary>
        /// Gets the ID of this ServerInfoRequestPacket.
        /// </summary>
        public ushort ID { get { return PacketType.SERVER_INFO_ID; } }

        /// <summary>
        /// Gets the maximum send attempts for this ServerInfoRequestPacket.
        /// </summary>
        public byte MaxSendAttempts { get { return 0; } }

        /// <summary>
        /// Gets the name of the server implementation the client is looking for.
        /// </summary>
        public string ServerImplementation { get; private set; }

        /// <summary>
        /// Gets the sender of this ServerInfoRequestPacket.
        /// </summary>
        public SenderIs Sender { get { return SenderIs.Client; } }

        /// <summary>
        /// Gets the sender's IPEndPoint of this ServerInfoRequestPacket.
        /// </summary>
        public IPEndPoint SenderEndPoint { get; private set; }

        /// <summary>
        /// Gets the time in milliseconds before a resend attempt is made.
        /// </summary>
        public ushort TimeUntilResend { get { return 3000; } } // Set to 3 seconds so the Timeout event will set off


        /// <summary>
        /// Initialises a new instance of the ServerInfoRequestPacket class using network data.
        /// </summary>
        /// <param name="data">The network data describing this ServerInfoRequestPacket.</param>
        /// <param name="senderEndPoint">The IPEndPoint of the sender.</param>
        public ServerInfoRequestPacket(byte[] data, IPEndPoint senderEndPoint)
        {
            int currentIndex = 2; // Skip first two bytes for ID
            ClientImplementation = ByteParse.NextStringByLength(data, ref currentIndex, 1);
            ClientVersionRaw = ByteParse.NextUShort(data, ref currentIndex);
            ServerImplementation = ByteParse.NextStringByLength(data, ref currentIndex, 1);

            if (ClientImplementation == String.Empty ||
                ServerImplementation == String.Empty)
                throw new ArgumentException("ServerInfoRequestPacket.New: Invalid data supplied.");

            SenderEndPoint = senderEndPoint;
        }

        /// <summary>
        /// Initialises a new instance of the ServerInfoRequestPacket class with specified properties.
        /// </summary>
        /// <param name="clientImplementation">The implementation name of the client.</param>
        /// <param name="versionRaw">The implementation version of the client.</param>
        /// <param name="serverImplementation">The implementation to look for (servers running this implementation should respond).</param>
        public ServerInfoRequestPacket(string clientImplementation, ushort versionRaw, string serverImplementation)
        {
            if (!clientImplementation.LengthWithinRange(1, 128) ||
                !serverImplementation.LengthWithinRange(1, 128))
                throw new ArgumentException("ServerInfoRequestPacket.New: Invalid implementation name length.");

            ClientImplementation = clientImplementation;
            ClientVersionRaw = versionRaw;
            ServerImplementation = serverImplementation;
        }


        /// <summary>
        /// Creates an exact copy of this ServerInfoRequestPacket.
        /// </summary>
        /// <returns>The ServerInfoRequestPacket this method creates, cast as an object.</returns>
        public object Clone()
        {
            return new ServerInfoRequestPacket(ClientImplementation, ClientVersionRaw, ServerImplementation);
        }

        /// <summary>
        /// Gets the data in this ServerInfoRequestPacket as a byte array.
        /// </summary>
        /// <returns>A byte array containing the data in this ServerInfoRequestPacket.</returns>
        public byte[] GetBytes()
        {
            var data = new List<byte>();

            data.AddRange(ID.GetBytes());
            data.AddRange(ClientImplementation.GetBytesByLength(1));
            data.AddRange(ClientVersionRaw.GetBytes());
            data.AddRange(ServerImplementation.GetBytesByLength(1));

            return data.ToArray();
        }
    }
}
