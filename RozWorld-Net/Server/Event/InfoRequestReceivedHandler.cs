/**
 * Oddmatics.RozWorld.Net.Packets.ServerInfoResponsePacket -- RozWorld Server Information Response Packet
 *
 * This source-code is part of the server library for the RozWorld project by rozza of Oddmatics:
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
    /// Represents the method that will handle an InfoRequestReceived event.
    /// </summary>
    /// <param name="sender">The RwUdpServer instance that fired the event.</param>
    /// <param name="packet">The ServerInfoRequestPacket instance containing the packet's data.</param>
    public delegate void InfoRequestReceivedHandler(RwUdpServer sender, ServerInfoRequestPacket packet);
}
