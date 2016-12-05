/**
 * Oddmatics.RozWorld.Net.Packets.Special -- RozWorld Special Network Packet Constants
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
    /// Contains special constant values for RozWorld network packets.
    /// </summary>
    public class Special
    {
        /// <summary>
        /// The initial signature that identifies RozWorld packets.
        /// </summary>
        public const uint PACKET_SIGNATURE = 0xBAE07777;
    }
}
