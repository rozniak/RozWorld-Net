/**
 * Oddmatics.RozWorld.Net.Packets.IAcknowledgeable -- RozWorld Network Acknowledgeable Packet
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
    /// Represents a packet that should be acknowledged.
    /// </summary>
    public interface IAcknowledgeable : IPacket
    {
        /// <summary>
        /// Gets the acknowledgement ID for this IPacket.
        /// </summary>
        ushort AckId { get; }
    }
}
