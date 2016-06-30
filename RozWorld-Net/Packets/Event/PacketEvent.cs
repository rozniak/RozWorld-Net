/**
 * Oddmatics.RozWorld.Net.Packets.Event.PacketEventHandler -- RozWorld Network Packet Receieved Delegate
 *
 * This source-code is part of the netcode library for the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld-Net>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System;

namespace Oddmatics.RozWorld.Net.Packets.Event
{
    /// <summary>
    /// Represents the method that will handle packet received events.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A PacketEventArgs object that contains the event data.</param>
    public delegate void PacketEventHandler(object sender, PacketEventArgs e);

    /// <summary>
    /// Provides data for packet events.
    /// </summary>
    public class PacketEventArgs : EventArgs
    {
        /// <summary>
        /// The IPacket subject of the event.
        /// </summary>
        public IPacket Packet { get; private set; }


        /// <summary>
        /// Initialises a new instance of the PacketEventArgs class using the specified IPacket.
        /// </summary>
        /// <param name="packet">An instance that represents the subject IPacket.</param>
        public PacketEventArgs(IPacket packet)
        {
            Packet = packet;
        }
    }
}
