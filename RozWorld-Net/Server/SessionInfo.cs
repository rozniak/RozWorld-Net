/**
 * Oddmatics.RozWorld.Net.Server.SessionInfo -- RozWorld Server Networking Session Information
 *
 * This source-code is part of the netcode library for the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld-Net>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System;

namespace Oddmatics.RozWorld.Net.Server
{
    /// <summary>
    /// Represents generated server network session information.
    /// </summary>
    public class SessionInfo
    {
        /// <summary>
        /// Gets the amount of bytes received during this session.
        /// </summary>
        public ulong BytesReceived { get; private set; }

        /// <summary>
        /// Gets the amount of bytes transmitted during this session.
        /// </summary>
        public ulong BytesTransmitted { get; private set; }

        /// <summary>
        /// Gets the DateTime that this session was started.
        /// </summary>
        public DateTime StartedAt { get; private set; }

        /// <summary>
        /// Gets the total uptime during this session.
        /// </summary>
        public TimeSpan Uptime { get { return DateTime.UtcNow - StartedAt; } }


        /// <summary>
        /// Initialises a new instance of the SessionInfo class with a specified details.
        /// </summary>
        /// <param name="bytesReceived">The amount of bytes received in this session.</param>
        /// <param name="bytesTransmitted">The amount of bytes transmitted in this session.</param>
        /// <param name="startedAt">The DateTime that the session began.</param>
        public SessionInfo(ulong bytesReceived, ulong bytesTransmitted, DateTime startedAt)
        {
            BytesReceived = bytesReceived;
            BytesTransmitted = bytesTransmitted;
            StartedAt = startedAt;
        }
    }
}
