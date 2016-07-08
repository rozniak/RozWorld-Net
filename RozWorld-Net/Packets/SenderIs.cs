/**
 * Oddmatics.RozWorld.Net.Packets.SenderIs -- RozWorld Client-Server Packet Sender Enumeration
 *
 * This source-code is part of the netcode library for the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld-Net>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

namespace Oddmatics.RozWorld.Net.Packets
{
    /// <summary>
    /// Specifies constants for determining who sends the packet in question.
    /// </summary>
    public enum SenderIs
    {
        /// <summary>
        /// Represents that a client should have sent this packet.
        /// </summary>
        Client,
        /// <summary>
        /// Represents that a server should have sent this packet.
        /// </summary>
        Server,
        /// <summary>
        /// Represents that either the client or the server could have sent this packet.
        /// </summary>
        Either
    }
}
