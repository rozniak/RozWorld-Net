/**
 * Oddmatics.RozWorld.Net.Server.Event.ClientDropEvent -- RozWorld Server Client Connection Dropped Event
 *
 * This source-code is part of the netcode library for the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld-Net>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using System;

namespace Oddmatics.RozWorld.Net.Server.Event
{
    /// <summary>
    /// Represents the method that will handle the ClientDropped event of an RwUdpServer.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A ClientDropEventArgs object that contains event data.</param>
    public delegate void ClientDropEventHandler(object sender, ClientDropEventArgs e);


    /// <summary>
    /// Provides data for the RwUdpServer.ClientDropped event.
    /// </summary>
    public class ClientDropEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the dropped ConnectedClient instance.
        /// </summary>
        public ConnectedClient Client { get; private set; }


        /// <summary>
        /// Initialises a new instance of the ClientDropEventArgs class using the specified ConnectedClient.
        /// </summary>
        /// <param name="client">The ConnectedClient instance that was dropped.</param>
        public ClientDropEventArgs(ConnectedClient client)
        {
            Client = client;
        }
    }
}