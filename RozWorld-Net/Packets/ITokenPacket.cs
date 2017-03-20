/**
 * Oddmatics.RozWorld.Net.Packets.ITokenPacket -- RozWorld Network Packet with Token
 *
 * This source-code is part of the netcode library for the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld-Net>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */


namespace Oddmatics.RozWorld.Net.Packets
{
    /// <summary>
    /// Represents a network packet with a token.
    /// </summary>
    public interface ITokenPacket : IPacket
    {
        /// <summary>
        /// Gets or sets the token used in this conversation.
        /// </summary>
        uint Token { get; }
    }
}
