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
        private IPEndPoint EndPoint;

        /// <summary>
        /// The IPacket for resending.
        /// </summary>
        private IPacket Packet;

        /// <summary>
        /// The parent RwUdpClient instance.
        /// </summary>
        private RwUdpClient Parent;

        /// <summary>
        /// The time in milliseconds since the last send attempt.
        /// </summary>
        private ushort SinceLastSend;


        public event EventHandler Timeout;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="destinationEP"></param>
        /// <param name="parent"></param>
        public PacketWatcher(IPacket packet, IPEndPoint destinationEP, RwUdpClient parent)
        {
            Attempts = 0;
            EndPoint = destinationEP;
            Packet = packet;
            Parent = parent;
            Parent.TimeoutTimer.Elapsed += new ElapsedEventHandler(TimeoutTimer_Elapsed);
        }


        /// <summary>
        /// [Parent.TimeoutTimer.Elapsed] Timeout timer ticked.
        /// </summary>
        private void TimeoutTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SinceLastSend += (ushort)((Timer)sender).Interval;

            if (SinceLastSend > Packet.TimeUntilResend)
            {
                if (++Attempts > Packet.MaxSendAttempts)
                {
                    if (Timeout != null)
                        Timeout(this, EventArgs.Empty);

                    Parent.TimeoutTimer.Elapsed -= TimeoutTimer_Elapsed;
                }

                SinceLastSend = 0;
            }
        }


        public void Reset(IPacket newPacket = null)
        {
            
        }
    }
}
