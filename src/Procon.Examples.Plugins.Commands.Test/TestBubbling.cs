﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Procon.Core.Connections.Plugins;
using Procon.Core.Shared;

namespace Procon.Examples.Plugins.CommandRouting.Test {
    [TestFixture]
    public class TestBubbling {

        /// <summary>
        /// Test for you to debug.
        /// Set a breakpoint within Procon.Examples.CommandRouting.TunneledCommands.NoParameterBubbleCommand
        /// </summary>
        [Test]
        public void TestSingleConvertedParameterPassing() {

            // You can think of this test as showing how (within a plugin) to send commands up the stream
            // which is how you would send commands to Procon.

            // Create a new plugin controller to load up the test plugin
            CorePluginController plugins = new CorePluginController().Execute() as CorePluginController;

            // Enable the single plugin that was loaded, otherwise it won't recieve any tunneled
            // commands.
            plugins.Tunnel(new Command() {
                Origin = CommandOrigin.Local,
                CommandType = CommandType.PluginsEnable,
                ScopeModel = {
                    PluginGuid = plugins.LoadedPlugins.First().PluginGuid
                }
            });

            CommandResult result = plugins.Tunnel(new Command() {
                Name = "NoParameterBubbleCommand",
                // We're cheating a little bit here and just saying the command came from
                // "local" as in it was generated by Procon itself.
                Origin = CommandOrigin.Local,
                Parameters = new List<CommandParameter>() {
                    new CommandParameter() {
                        Data = {
                            Content = new List<String>() {
                                "100"
                            }
                        }
                    }
                }
            });

            Assert.AreEqual("200", result.Message);
        }

    }
}
