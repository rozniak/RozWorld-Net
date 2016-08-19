/**
 * Oddmatics.RozWorld.Net.Server.UserClient -- RozWorld UDP Server User Client Tracker
 *
 * This source-code is part of the netcode library for the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld-Net>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System;
using System.Collections.Generic;

namespace Oddmatics.RozWorld.Net.Server
{
    /// <summary>
    /// Represent user specific details for a client tracker.
    /// </summary>
    public class UserClient
    {
        /// <summary>
        /// Gets the chat message queue.
        /// </summary>
        public Queue<string> ChatMessages { get; private set; }

        /// <summary>
        /// The current chat message waiting to be acknowledged.
        /// </summary>
        public Tuple<ushort, string> CurrentChatMessage;


        /// <summary>
        /// Initialises a new instance of the UserClient class.
        /// </summary>
        public UserClient()
        {
            ChatMessages = new Queue<string>();
        }
    }
}
