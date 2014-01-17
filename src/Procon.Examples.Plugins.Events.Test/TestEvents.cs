﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Procon.Core.Connections.Plugins;
using Procon.Core.Shared;
using Procon.Net.Shared;
using Procon.Net.Shared.Models;

namespace Procon.Examples.Plugins.Events.Test {
    /// <summary>
    /// This is even lazier than the other examples. There is zero testing as
    /// events are fired and forgotten by Procon.
    /// </summary>
    /// <remarks>This just allows you convientient way of testing plugins via debugging</remarks>
    [TestFixture]
    public class TestEvents {
        /// <summary>
        /// Test for you to debug.
        /// Set a breakpoint within Procon.Examples.Events.ClientEvent.ClientPacketReceived
        /// </summary>
        [Test]
        public void TestClientEventClientPacketReceived() {
            // Create a new plugin controller to load up the test plugin
            CorePluginController plugins = (CorePluginController)new CorePluginController().Execute();

            // Enable the single plugin that was loaded, otherwise it won't recieve any tunneled
            // commands or events.
            plugins.Tunnel(new Command() {
                Origin = CommandOrigin.Local,
                CommandType = CommandType.PluginsEnable,
                Scope = {
                    PluginGuid = plugins.LoadedPlugins.First().PluginGuid
                }
            });

            plugins.PluginFactory.ClientEvent(new ClientEventArgs() {
                EventType = ClientEventType.ClientPacketReceived,
                ConnectionState = ConnectionState.ConnectionLoggedIn,
                Now = {
                    Packets = new List<IPacket>() {
                        new Packet() {
                            RequestId = 1,
                            Origin = PacketOrigin.Client,
                            Type = PacketType.Response,
                            Text = "hello world!",
                            DebugText = "[0-hello] [1-world!]",
                            Words = new List<String>() {
                                "hello",
                                "world!"
                            }
                        }
                    }
                }
            });
        }

        /// <summary>
        /// Test for you to debug.
        /// Set a breakpoint within Procon.Examples.Events.GameEvent.GameChat
        /// </summary>
        [Test]
        public void TestGameEventGameChat() {
            // Create a new plugin controller to load up the test plugin
            CorePluginController plugins = (CorePluginController)new CorePluginController().Execute();

            // Enable the single plugin that was loaded, otherwise it won't recieve any tunneled
            // commands or events.
            plugins.Tunnel(new Command() {
                Origin = CommandOrigin.Local,
                CommandType = CommandType.PluginsEnable,
                Scope = {
                    PluginGuid = plugins.LoadedPlugins.First().PluginGuid
                }
            });

            plugins.PluginFactory.GameEvent(new ProtocolEventArgs() {
                ProtocolEventType = ProtocolEventType.ProtocolChat,
                Now = {
                    Chats = new List<Chat>() {
                        new Chat() {
                            Origin = NetworkOrigin.Player,
                            Scope = {
                                Groups = new List<Grouping>() {
                                    new Grouping() {
                                        Type = Grouping.Team,
                                        Uid = "1"
                                    }
                                }
                            },
                            Now = {
                                Content = new List<String>() {
                                    "Hello!"
                                },
                                Players = new List<Player>() {
                                    new Player() {
                                        Uid = "EA_1234",
                                        Name = "Phogue"
                                        // other details..
                                    }
                                }
                            }
                        }
                    }
                }
            });
        }


        /// <summary>
        /// Test for you to debug.
        /// Set a breakpoint within Procon.Examples.Events.GenericEvent.PluginsPluginEnabled
        /// </summary>
        [Test]
        public void TestGenericEventPluginsPluginEnabled() {
            // Create a new plugin controller to load up the test plugin
            CorePluginController plugins = (CorePluginController)new CorePluginController().Execute();

            // Enable the single plugin that was loaded, otherwise it won't recieve any tunneled
            // commands or events.
            plugins.Tunnel(new Command() {
                Origin = CommandOrigin.Local,
                CommandType = CommandType.PluginsEnable,
                Scope = {
                    PluginGuid = plugins.LoadedPlugins.First().PluginGuid
                }
            });

            // That's it. Enabling the plugin will have this event fired.
        }
    }
}