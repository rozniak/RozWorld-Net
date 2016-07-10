/**
 * Oddmatics.RozWorld.Net.Packets.IPlayerPacket -- RozWorld Player Associated Packet
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
    /// Represents a packet sent with associated player.
    /// </summary>
    public interface IPlayerPacket : IPacket
    {
        /// <summary>
        /// Gets the player ID associated with this IPlayerPacket.
        /// </summary>
        ushort PlayerId { get; }
    }
}
