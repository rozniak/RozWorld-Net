/**
 * Oddmatics.RozWorld.Net.Server.Packets.DisconnectReason -- RozWorld Network Client Disconnect Reasons
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
    /// Contains constants for determining a disconnect reason
    /// </summary>
    public static class DisconnectReason
    {
        /// <summary>
        /// The client quit the server gracefully.
        /// </summary>
        public const byte QUIT = 1;
        /// <summary>
        /// The mods installed on the client do not match those on the server.
        /// (Client may require downloading of mods that the server has but client doesn't)
        /// </summary>
        public const byte MODS_INCOMPATIBLE = 2;
        /// <summary>
        /// The client crashed but still managed to issue a disconnect.
        /// </summary>
        public const byte CLIENT_CRASH = 3;
        /// <summary>
        /// The server has shut down.
        /// </summary>
        public const byte SERVER_SHUTDOWN = 100;
        /// <summary>
        /// The server is down for maintenance.
        /// </summary>
        public const byte SERVER_MAINTENANCE = 101;
        /// <summary>
        /// The server kicked the client.
        /// </summary>
        public const byte KICKED = 102;
    }
}
