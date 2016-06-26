/**
 * Oddmatics.RozWorld.Net.Server.Packets.ErrorMessage -- RozWorld Network Error Messages
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
    /// Contains constants for determining a RozWorld network error.
    /// </summary>
    public static class ErrorMessage
    {
        /// <summary>
        /// No error occurred.
        /// </summary>
        public const byte NO_ERROR = 0;
        /// <summary>
        /// The account name to sign up with has already been used.
        /// </summary>
        public const byte ACCOUNT_NAME_TAKEN = 1;
        /// <summary>
        /// The account name specified contains invalid characters.
        /// </summary>
        public const byte ACCOUNT_NAME_INVALID = 2;
        /// <summary>
        /// The password hash is invalid.
        /// </summary>
        public const byte PASSWORD_INVALID = 3;
        /// <summary>
        /// The max account restriction for this client has been reached.
        /// </summary>
        public const byte TOO_MANY_ACCOUNTS = 4;
        /// <summary>
        /// This account or IP has been banned from the server.
        /// </summary>
        public const byte BANNED = 5;
        /// <summary>
        /// The log in details given were incorrect.
        /// </summary>
        public const byte INCORRECT_LOGIN = 6;
        /// <summary>
        /// The maximum allowed log in attempts has been reached.
        /// </summary>
        public const byte ATTEMPTS_EXPIRED = 7;
        /// <summary>
        /// The network function has not been fully implemented.
        /// </summary>
        public const byte NOT_IMPLEMENTED = 100;
        /// <summary>
        /// The was an error on the server that prevented the operation from completing.
        /// </summary>
        public const byte INTERNAL_ERROR = 255;
    }
}
