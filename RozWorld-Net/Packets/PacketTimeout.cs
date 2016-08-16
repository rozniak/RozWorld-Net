/**
 * Oddmatics.RozWorld.Net.Packets.PacketTimeout -- RozWorld Network Packet Timeout Details
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
    /// Contains constants describing packet timeout details.
    /// </summary>
    public static class PacketTimeout
    {
        /// <summary>
        /// The amount of resend attempts to make for authentication packets.
        /// </summary>
        public const byte RESEND_ATTEMPTS_AUTH = 3;

        /// <summary>
        /// The time in milliseconds until a resend attempt is made for authentication packets.
        /// </summary>
        public const ushort RESEND_TIMEOUT_AUTH = 1000;

        /// <summary>
        /// The amount of resend attempts to make for chat message packets.
        /// </summary>
        public const byte RESEND_ATTEMPTS_CHAT = 6;

        /// <summary>
        /// The amount of resend attempts to make for movement packets.
        /// </summary>
        public const byte RESEND_ATTEMPTS_MOVEMENTS = 255;

        /// <summary>
        /// The time in milliseconds until a resend attempt is made for movement packets.
        /// </summary>
        public const ushort RESEND_TIMEOUT_MOVEMENTS = 200;

        /// <summary>
        /// The time in milliseconds with no network interaction before a keepalive ping should be transmitted.
        /// </summary>
        public const ushort SEND_TIMEOUT_PING = 2000;

        /// <summary>
        /// The time in milliseconds until a server endpoint is deemed unreachable.
        /// </summary>
        public const ushort TOTAL_TIMEOUT = 15000;
    }
}
