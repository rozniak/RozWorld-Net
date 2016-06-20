/**
 * Oddmatics.RozWorld.Net.Client.Event.InfoResponseReceivedHandler -- RozWorld Server Information Response Packet Receieved Delegate
 *
 * This source-code is part of the netcode library for the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld-Net>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using Oddmatics.RozWorld.Net.Packets;

namespace Oddmatics.RozWorld.Net.Client.Event
{
    /// <summary>
    /// Represents the method that will handle an InfoResponseReceieved event.
    /// </summary>
    /// <param name="sender">The RwUdpClient instance that fired the event.</param>
    /// <param name="packet">The ServerInfoResponsePacket containing the packet's data.</param>
    public delegate void InfoResponseReceivedHandler(RwUdpClient sender, ServerInfoResponsePacket packet);
}
