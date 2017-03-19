/**
 * Oddmatics.RozWorld.Net.Packets.SessionContext -- RozWorld Session Context
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
    /// Specifies constants for determining the context of a session.
    /// </summary>
    public enum SessionContext
    {
        /// <summary>
        /// The session is to be a logged in, active session.
        /// </summary>
        LoggedInSession,
        /// <summary>
        /// The session is to be used for signing up.
        /// </summary>
        SignUp
    }
}
