/**
 * Oddmatics.RozWorld.Net.Server.Event.PacketReceivedEventHandler -- RozWorld Server Packet Receieved Delegate
 *
 * This source-code is part of the netcode library for the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld-Net>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using Oddmatics.RozWorld.Net.Packets;

namespace Oddmatics.RozWorld.Net.Server.Event
{
    /// <summary>
    /// Represents the method that will handle packet received events.
    /// </summary>
    /// <param name="sender">The RwUdpServer instance that fired the event.</param>
    /// <param name="packet">The IPacket that was received.</param>
    public delegate void PacketReceivedEventHandler(RwUdpServer sender, IPacket packet);
}
