/**
 * Oddmatics.RozWorld.Net.Packets.ServerInfoResponsePacket -- RozWorld Server Information Response Packet
 *
 * This source-code is part of the server library for the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld-Net>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System.Net;

namespace Oddmatics.RozWorld.Net.Packets
{
    /// <summary>
    /// [SERVER --> CLIENT] Represents a server info response packet.
    /// </summary>
    public class ServerInfoResponsePacket : IPacket
    {
        /// <summary>
        /// Gets whether the client is compatible the server.
        /// </summary>
        public bool ClientCompatible { get; private set; }

        /// <summary>
        /// Gets the ID of this ServerInfoResponsePacket.
        /// </summary>
        public ushort ID { get { return 1; } }

        /// <summary>
        /// Gets the maximum players on the server.
        /// </summary>
        public short MaxPlayers { get; private set; }

        /// <summary>
        /// Gets the amount of currently online players on the server.
        /// </summary>
        public short OnlinePlayers { get; private set; }

        /// <summary>
        /// Gets the sender of this ServerInfoResponsePacket.
        /// </summary>
        public SenderIs Sender { get { return SenderIs.Server; } }

        /// <summary>
        /// Gets the sender's IPEndPoint of this ServerInfoResponsePacket.
        /// </summary>
        public IPEndPoint SenderEndPoint { get; private set; }

        /// <summary>
        /// Gets the server's name to show in the browser.
        /// </summary>
        public string ServerName { get; private set; }

        /// <summary>
        /// Gets the name of the server's implementation.
        /// </summary>
        public string ServerImplementation { get; private set; }


        /// <summary>
        /// Initialises a new instance of the ServerInfoResponsePacket class using network data.
        /// </summary>
        /// <param name="data">The network data describing this </param>
        /// <param name="senderEndPoint">The IPEndPoint of the sender.</param>
        public ServerInfoResponsePacket(byte[] data, IPEndPoint senderEndPoint)
        {
            // TODO: code this
        }

        /// <summary>
        /// Initialises a new instance of the ServerInfoResponsePacket class with specified properties.
        /// </summary>
        public ServerInfoResponsePacket()
        {
            // TODO: code this
        }


        /// <summary>
        /// Gets the data in this ServerInfoResponsePacket as a byte array.
        /// </summary>
        /// <returns>A byte array containing the data in this ServerInfoResponsePacket.</returns>
        public byte[] GetBytes()
        {
            // TODO: code this
            throw new System.NotImplementedException();
        }
    }
}
