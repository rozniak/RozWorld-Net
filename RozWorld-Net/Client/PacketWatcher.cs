/**
 * Oddmatics.RozWorld.Net.Client.PacketWatcher -- RozWorld Network Packet Watcher
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
using System.Net;
using System.Timers;

namespace Oddmatics.RozWorld.Net.Client
{
    /// <summary>
    /// Represents a watcher for a packet, including a timeout and time to reply.
    /// </summary>
    public class PacketWatcher
    {
        /// <summary>
        /// The amount of attempts to send this packet.
        /// </summary>
        private byte Attempts;

        /// <summary>
        /// Destination IPEndPoint to receive from.
        /// </summary>
        public IPEndPoint EndPoint { get; private set; }

        /// <summary>
        /// The IPacket for resending.
        /// </summary>
        public IPacket Packet { get; private set; }

        /// <summary>
        /// The parent RwUdpClient instance.
        /// </summary>
        private RwUdpClient Parent;

        /// <summary>
        /// The time in milliseconds since the last send attempt.
        /// </summary>
        private ushort SinceLastSend;

        /// <summary>
        /// Whether this PacketWatcher has started or not.
        /// </summary>
        private bool Started;


        /// <summary>
        /// Occurs when the maximum send attempts have been hit for this PacketWatcher.
        /// </summary>
        public event EventHandler Timeout;


        /// <summary>
        /// Initialises a new instance of the PacketWatcher class with packet details.
        /// </summary>
        /// <param name="packet">The IPacket to watch.</param>
        /// <param name="destination">The destination IPEndPoint of the remote host.</param>
        /// <param name="parent">The parent RwUdpClient instance.</param>
        public PacketWatcher(IPacket packet, IPEndPoint destination, RwUdpClient parent)
        {
            if (packet == null || destination == null || parent == null)
                throw new ArgumentException("PacketWatcher.New: Cannot initialise with null arguments.");

            Attempts = 0;
            EndPoint = destination;
            Packet = packet;
            Parent = parent;
        }


        /// <summary>
        /// Resets this PacketWatcher.
        /// </summary>
        /// <param name="newPacket">If the IPacket in question has been updated, pass the new one here.</param>
        public void Reset(IPacket newPacket = null)
        {
            if (Started)
            {
                if (newPacket != null)
                    Packet = newPacket;

                Attempts = 0;
                SinceLastSend = 0;
                Parent.Send(Packet, EndPoint);
            }
        }

        /// <summary>
        /// Starts this PacketWatcher.
        /// </summary>
        public void Start()
        {
            if (!Started)
            {
                Parent.Send(Packet, EndPoint);
                Parent.TimeoutTimer.Elapsed += new ElapsedEventHandler(TimeoutTimer_Elapsed);
                Started = true;
            }
        }

        /// <summary>
        /// Stops this PacketWatcher.
        /// </summary>
        public void Stop()
        {
            if (Started)
            {
                Parent.TimeoutTimer.Elapsed -= TimeoutTimer_Elapsed;
                Started = false;
                SinceLastSend = 0;
                Attempts = 0;
            }
        }


        /// <summary>
        /// [Parent.TimeoutTimer.Elapsed] Timeout timer ticked.
        /// </summary>
        private void TimeoutTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (SinceLastSend > Packet.TimeUntilResend)
            {
                if (++Attempts > Packet.MaxSendAttempts)
                {
                    if (Timeout != null)
                        Timeout(this, EventArgs.Empty);

                    Parent.TimeoutTimer.Elapsed -= TimeoutTimer_Elapsed;
                    Started = false;
                }
                else
                    Parent.Send(Packet, EndPoint);

                SinceLastSend = 0;
            }
            else
                SinceLastSend += (ushort)((Timer)sender).Interval;
        }
    }
}
