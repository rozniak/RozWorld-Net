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

namespace Oddmatics.RozWorld.Net.Packets.Event
{
    /// <summary>
    /// Represents the method that will handle packet received events.
    /// </summary>
    /// <param name="sender">The instance that fired the event.</param>
    /// <param name="packet">The IPacket that was received.</param>
    public delegate void PacketEventHandler(object sender, IPacket packet);
}
