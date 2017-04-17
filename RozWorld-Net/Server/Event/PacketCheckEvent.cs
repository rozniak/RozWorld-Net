/**
 * Oddmatics.RozWorld.Net.Server.Event.PacketCheckEvent -- RozWorld Network Packet Checking Event
 *
 * This source-code is part of the netcode library for the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld-Net>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using Oddmatics.RozWorld.Net.Packets;
using Oddmatics.RozWorld.Net.Packets.Event;

namespace Oddmatics.RozWorld.Net.Server.Event
{
    /// <summary>
    /// Represents the method that will handle checking/approval of packets received that require it.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A PacketCheckEventArgs object that contains the event data.</param>
    public delegate void PacketCheckEventHandler(object sender, PacketCheckEventArgs e);

    /// <summary>
    /// Provides data for packet checking events.
    /// </summary>
    public sealed class PacketCheckEventArgs : PacketEventArgs
    {
        /// <summary>
        /// The result of the packet check.
        /// </summary>
        public byte Result;


        /// <summary>
        /// Initialises a new instance of the PacketCheckEventArgs class using the specified IPacket.
        /// </summary>
        /// <param name="packet">An instance that represents the subject IPacket.</param>
        public PacketCheckEventArgs(IPacket packet) : base(packet)
        {
            Packet = packet;
        }
    }
}
