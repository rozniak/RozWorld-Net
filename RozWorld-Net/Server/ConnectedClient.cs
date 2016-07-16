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

using Oddmatics.RozWorld.Net.Packets;
using System;
using System.Collections.Generic;
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
        /// Gets whether this ConnectedClient is alive or not.
        /// </summary>
        public bool Alive { get; private set; }

        /// <summary>
        /// The IPEndPoint of the client.
        /// </summary>
        public IPEndPoint EndPoint { get; private set; }

        /// <summary>
        /// The freed, previously used, acknowledgement IDs.
        /// </summary>
        private Queue<ushort> FreedAckIds;

        /// <summary>
        /// Gets the next available acknowledgement ID to use.
        /// </summary>
        public ushort NextAckId
        {
            get
            {
                if (FreedAckIds.Count > 0)
                    return FreedAckIds.Dequeue();
                else
                    return _NextAckId++;
            }
        }
        private ushort _NextAckId;

        /// <summary>
        /// The parent RwUdpServer instance.
        /// </summary>
        private RwUdpServer Parent;

        /// <summary>
        /// The time in milliseconds since the last packet was receieved from the client.
        /// </summary>
        private ushort SinceLastPacketReceived;

        /// <summary>
        /// The time in milliseconds since the last packet was sent to the client.
        /// </summary>
        private ushort SinceLastPacketSent;


        /// <summary>
        /// Occurs when the last packet received's time exceeds the timeout requirement to drop the connection for this client.
        /// </summary>
        public event EventHandler TimedOut;


        /// <summary>
        /// Initialises a new instance of the ConnectedClient class with a specified IPEndPoint of the client and a parent RwUdpServer.
        /// </summary>
        /// <param name="clientEP">The IPEndPoint of the client.</param>
        /// <param name="parent">The parent RwUdpServer of this ConnectedClient.</param>
        public ConnectedClient(IPEndPoint clientEP, RwUdpServer parent)
        {
            if (clientEP == null || parent == null)
                throw new ArgumentException("ConnectedClient.New: Null arguments are not allowed.");

            Parent = parent;
            EndPoint = clientEP;
        }


        /// <summary>
        /// Registers an ID that has been acknowledged.
        /// </summary>
        /// <param name="id">The acknoledgement ID to free up.</param>
        public void Acknowledge(ushort id)
        {
            if (id >= _NextAckId || FreedAckIds.Contains(id))
                throw new ArgumentException("ConnectedClient.Acknowledge: The ack ID given is not valid.");

            FreedAckIds.Enqueue(id);
        }

        /// <summary>
        /// Enables this ConnectedClient.
        /// </summary>
        public void Begin()
        {
            if (Alive)
                throw new InvalidOperationException("ConnectedClient.Begin: Already started.");

            if (Parent.GetConnectedClient(EndPoint) != this)
                throw new ArgumentException("ConnectedClient.Begin: The clientEP specified for this instance does not match the parent's reference through the same IPEndPoint value.");

            Parent.TimeoutTimer.Elapsed += new ElapsedEventHandler(TimeoutTimer_Elapsed);
            Alive = true;
        }

        /// <summary>
        /// Resets the timeout counter.
        /// </summary>
        public void ResetTimeoutCounter()
        {
            if (Alive)
                SinceLastPacketReceived = 0;
            else
                throw new InvalidOperationException("ConnectedClient.ResetTimeoutCounter: Cannot reset timeout for a timed out client.");
        }

        /// <summary>
        /// Sends the specified IPacket to the client.
        /// </summary>
        /// <param name="packet">The IPacket to send.</param>
        public void SendPacket(IPacket packet)
        {
            if (Alive)
            {
                Parent.Send(packet, EndPoint);
                SinceLastPacketSent = 0;
            }
        }


        /// <summary>
        /// [Parent.TimeoutTimer.Elapsed] Timeout timer ticked.
        /// </summary>
        private void TimeoutTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var timer = (Timer)sender;
            SinceLastPacketReceived += (ushort)timer.Interval;
            SinceLastPacketSent += (ushort)timer.Interval;

            if (SinceLastPacketSent > PacketTimeout.SEND_TIMEOUT_PING)
                SendPacket(new PingPacket());

            if (SinceLastPacketReceived > RwUdpServer.CLIENT_TIMEOUT_TIME)
            {
                timer.Stop();
                timer.Elapsed -= TimeoutTimer_Elapsed; // Detach this event handler

                Alive = false;

                if (TimedOut != null)
                    TimedOut(this, EventArgs.Empty);
            }
        }
    }
}
