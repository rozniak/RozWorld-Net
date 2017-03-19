/**
 * Oddmatics.RozWorld.Net.Packets.PacketType -- RozWorld Network Packet Type Identifiers
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
    /// Contains constants describing packet type IDs.
    /// </summary>
    public static class PacketType
    {
        /// <summary>
        /// The ID for server information packets.
        /// </summary>
        public const ushort SERVER_INFO_ID = 1;
        /// <summary>
        /// The ID for sign up packets.
        /// </summary>
        public const ushort SIGN_UP_ID = 2;
        /// <summary>
        /// The ID for log in packets.
        /// </summary>
        public const ushort LOG_IN_ID = 3;
        /// <summary>
        /// The ID for ping (keepalive) packets.
        /// </summary>
        public const ushort PING_ID = 4;
        /// <summary>
        /// The ID for chat message packets.
        /// </summary>
        public const ushort CHAT_MESSAGE_ID = 5;
        /// <summary>
        /// The ID for disconnect packets.
        /// </summary>
        public const ushort DISCONNECT_ID = 100;
        /// <summary>
        /// The ID for disconnect confirmation packets.
        /// (These aren't full IPacket classes, the server sends these to confirm a client disconnect safely)
        /// </summary>
        public const ushort CONFIRM_DISCONNECT_ID = 101;
        /// <summary>
        /// The ID for sign up initiation packets.
        /// </summary>
        public const ushort INITIATION_ID = 998;
        /// <summary>
        /// The ID for acknowledgement packets.
        /// </summary>
        public const ushort ACK_ID = 999;
    }
}
