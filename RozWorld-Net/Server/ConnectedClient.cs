/**
 * Oddmatics.RozWorld.Net.Server.ConnectedClient -- RozWorld UDP Server Connected Client Tracker
 *
 * This source-code is part of the netcode library for the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld-Net>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System;
using System.Net;
using System.Timers;

namespace Oddmatics.RozWorld.Net.Server
{
    /// <summary>
    /// Represents a client connected to a RozWorld server.
    /// </summary>
    public class ConnectedClient
    {
        /// <summary>
        /// 
        /// </summary>
        public IPEndPoint EndPoint { get; private set; }

        /// <summary>
        /// The parent RwUdpServer instance.
        /// </summary>
        private RwUdpServer Parent;

        /// <summary>
        /// The time in milliseconds since the last packet was receieved from this ConnectedClient.
        /// </summary>
        private ushort SinceLastPacket;



        public ConnectedClient(IPEndPoint clientEP, RwUdpServer parent)
        {
            Parent = parent;

            if (Parent.GetConnectedClient(clientEP) != this)
                throw new ArgumentException("ConnectedClient.New: The clientEP specified for this instance does not match the parent's reference through the same IPEndPoint value.");

            Parent.TimeoutTimer.Elapsed += new System.Timers.ElapsedEventHandler(TimeoutTimer_Elapsed);
        }

        private void TimeoutTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SinceLastPacket += (ushort)((Timer)sender).Interval;

            if (SinceLastPacket > RwUdpServer.CLIENT_TIMEOUT_TIME)
            {
                // Fire timeout event - this client has timed out
            }
        }
    }
}
