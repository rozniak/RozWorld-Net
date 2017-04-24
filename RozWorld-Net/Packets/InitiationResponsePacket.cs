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

using Oddmatics.RozWorld.API.Generic;
using Oddmatics.Util.IO;
using System.Collections.Generic;
using System.Net;

namespace Oddmatics.RozWorld.Net.Packets
{
    /// <summary>
    /// [SERVER --> CLIENT] Represents a sign up response packet.
    /// </summary>
    public class InitiationResponsePacket : ITokenPacket
    {
        /// <summary>
        /// Gets the ID of this InitiationResponsePacket.
        /// </summary>
        public ushort Id { get { return PacketType.INITIATION_ID; } }

        /// <summary>
        /// Gets the maximum send attempts for this InitiationResponsePacket.
        /// </summary>
        public byte MaxSendAttempts { get { return 0; } }

        /// <summary>
        /// The result code of the request.
        /// </summary>
        public readonly RwResult ResultCode;

        /// <summary>
        /// Gets the sender of this InitiationResponsePacket.
        /// </summary>
        public SenderIs Sender { get { return SenderIs.Server; } }

        /// <summary>
        /// Gets the sender's IPEndPoint of InitiationResponsePacket.
        /// </summary>
        public IPEndPoint SenderEndPoint { get; private set; }

        /// <summary>
        /// Gets the time in milliseconds before a resend attempt is made.
        /// </summary>
        public ushort TimeUntilResend { get { return 0; } }

        /// <summary>
        /// Gets the token leased to authenticate the transaction, if it was accepted.
        /// </summary>
        public uint Token { get; private set; }


        /// <summary>
        /// Initialises a new instance of the InitiationResponsePacket class using network data.
        /// </summary>
        /// <param name="data">The network data describing this InitiationResponsePacket.</param>
        /// <param name="senderEndPoint">The IPEndPoint of the sender.</param>
        public InitiationResponsePacket(byte[] data, IPEndPoint senderEndPoint)
        {
            int currentIndex = 6; // Skip first six bytes for signature and ID
            ResultCode = (RwResult)ByteParse.NextUShort(data, ref currentIndex);
            Token = ByteParse.NextUInt(data, ref currentIndex);
        }

        /// <summary>
        /// Initialises a new instance of the InitiationResponsePacket class with specified properties.
        /// </summary>
        /// <param name="resultCode">The result code for the request.</param>
        /// <param name="token">The token to define this communication context, 0 if this was a failure.</param>
        public InitiationResponsePacket(RwResult resultCode, uint token)
        {
            ResultCode = resultCode;
            Token = token;
        }
        

        /// <summary>
        /// Creates an exact copy of this InitiationResponsePacket.
        /// </summary>
        /// <returns>The InitiationResponsePacket this method creates, cast as an object.</returns>
        public object Clone()
        {
            return new InitiationResponsePacket(ResultCode, Token);
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
            data.AddRange(((ushort)ResultCode).GetBytes());
            data.AddRange(Token.GetBytes());

            return data.ToArray();
        }
    }
}
