﻿using System;
using System.Collections.Generic;
using System.Linq;
using Procon.Net.Shared;

namespace Procon.Net {
    public class PacketDispatcher : IPacketDispatcher {

        /// <summary>
        /// Array of dispatch handlers used to locate an appropriate method to call
        /// once we receieve a packet.
        /// </summary>
        public Dictionary<IPacketDispatch, Action<IPacketWrapper, IPacketWrapper>> Handlers;

        /// <summary>
        /// Handler to dispatch to if a request fails.
        /// </summary>
        public Action<IPacketDispatch, IPacketWrapper, IPacketWrapper> MissingDispatchHandler { get; set; }

        public PacketDispatcher() {
            this.Handlers = new Dictionary<IPacketDispatch, Action<IPacketWrapper, IPacketWrapper>>();
        }

        /// <summary>
        /// Appends a dispatch handler, first checking if an existing dispatch exists for this exact
        /// packet. If it exists then it will be overridden.
        /// </summary>
        /// <param name="handlers">A dictionary of handlers to append to the dispatch handlers.</param>
        public void Append(Dictionary<IPacketDispatch, Action<IPacketWrapper, IPacketWrapper>> handlers) {
            foreach (var handler in handlers) {
                if (this.Handlers.ContainsKey(handler.Key) == false) {
                    this.Handlers.Add(handler.Key, handler.Value);
                }
                else {
                    this.Handlers[handler.Key] = handler.Value;
                }
            }
        }

        public virtual void Dispatch(IPacketWrapper packet) {
            
        }

        /// <summary>
        /// Dispatches a recieved packet. Each game implementation needs to supply its own dispatch
        /// method as the protocol may be very different and have additional requirements beyond a 
        /// simple text match.
        /// </summary>
        /// <param name="identifer"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public virtual void Dispatch(IPacketDispatch identifer, IPacketWrapper request, IPacketWrapper response) {

            var dispatchMethods = this.Handlers.Where(dispatcher => dispatcher.Key.Name == identifer.Name)
                .Where(dispatcher => dispatcher.Key.Origin == PacketOrigin.None || dispatcher.Key.Origin == identifer.Origin)
                .Select(dispatcher => dispatcher.Value)
                .ToList();

            if (dispatchMethods.Any()) {
                foreach (Action<IPacketWrapper, IPacketWrapper> handler in dispatchMethods) {
                    handler(request, response);
                }
            }
            else {
                this.MissingDispatch(identifer, request, response);
            }
        }

        /// <summary>
        /// Called when dispatching, but no method matches the exact identifier used.
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public virtual void MissingDispatch(IPacketDispatch identifier, IPacketWrapper request, IPacketWrapper response) {
            if (this.MissingDispatchHandler != null) {
                this.MissingDispatchHandler(identifier, request, response);
            }
        }
    }
}
