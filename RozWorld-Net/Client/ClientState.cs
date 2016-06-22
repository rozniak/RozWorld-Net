/**
 * Oddmatics.RozWorld.Net.Client.ClientState -- RozWorld Client State
 *
 * This source-code is part of the netcode library for the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld-Net>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

namespace Oddmatics.RozWorld.Net.Client
{
    /// <summary>
    /// Specifies constants for determining the current state of a RozWorld client.
    /// </summary>
    public enum ClientState
    {
        /// <summary>
        /// Represents the idle (not connected, no activity) state.
        /// </summary>
        Idle,
        /// <summary>
        /// Represents the broadcasting (server scan) state.
        /// </summary>
        Broadcasting,
        /// <summary>
        /// Represents the signing-up state.
        /// </summary>
        SigningUp,
        /// <summary>
        /// Represents the logging-in state.
        /// </summary>
        LoggingIn,
        /// <summary>
        /// Represents the connected state.
        /// </summary>
        Connected
    }
}
