﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Procon.Core.Connections.Plugins;
using Procon.Core.Shared;

namespace Procon.Examples.CommandRouting.Test {
    [TestFixture]
    public class TestTunneling {
        /// <summary>
        /// Test for you to debug.
        /// Set a breakpoint within Procon.Examples.CommandRouting.Program.SingleParameterCommand
        /// </summary>
        [Test]
        public void TestSingleParameterPassing() {
            // Create a new plugin controller to load up the test plugin
            PluginController plugins = new PluginController().Execute() as PluginController;

            // Enable the single plugin that was loaded, otherwise it won't recieve any tunneled
            // commands.
            plugins.Tunnel(new Command() {
                Origin = CommandOrigin.Local,
                CommandType = CommandType.PluginsEnable,
                Scope = {
                    PluginGuid = plugins.Plugins.First().PluginModel.PluginGuid
                }
            });

            CommandResultArgs result = plugins.Tunnel(new Command() {
                Name = "SingleParameterCommand",
                // We're cheating a little bit here and just saying the command came from
                // "local" as in it was generated by Procon itself.
                Origin = CommandOrigin.Local,
                Parameters = new List<CommandParameter>() {
                    new CommandParameter() {
                        Data = {
                            Content = new List<String>() {
                                "SingleParameterCommandParameter"
                            }
                        }
                    }
                }
            });

            Assert.AreEqual("SingleParameterCommandParameter", result.Message);
        }

        /// <summary>
        /// Test for you to debug.
        /// Set a breakpoint within Procon.Examples.CommandRouting.Program.SingleConvertedParameterCommand
        /// </summary>
        [Test]
        public void TestSingleConvertedParameterPassing() {
            // Create a new plugin controller to load up the test plugin
            PluginController plugins = new PluginController().Execute() as PluginController;

            // Enable the single plugin that was loaded, otherwise it won't recieve any tunneled
            // commands.
            plugins.Tunnel(new Command() {
                Origin = CommandOrigin.Local,
                CommandType = CommandType.PluginsEnable,
                Scope = {
                    PluginGuid = plugins.Plugins.First().PluginModel.PluginGuid
                }
            });

            CommandResultArgs result = plugins.Tunnel(new Command() {
                Name = "SingleConvertedParameterCommand",
                // We're cheating a little bit here and just saying the command came from
                // "local" as in it was generated by Procon itself.
                Origin = CommandOrigin.Local,
                Parameters = new List<CommandParameter>() {
                    new CommandParameter() {
                        Data = {
                            Content = new List<String>() {
                                "10"
                            }
                        }
                    }
                }
            });

            Assert.AreEqual("20", result.Message);
        }

        /// <summary>
        /// Test for you to debug.
        /// Set a breakpoint within Procon.Examples.CommandRouting.Program.NoParameterCommand
        /// </summary>
        [Test]
        public void TestNoParameterPassing() {
            // Create a new plugin controller to load up the test plugin
            PluginController plugins = new PluginController().Execute() as PluginController;

            // Enable the single plugin that was loaded, otherwise it won't recieve any tunneled
            // commands.
            plugins.Tunnel(new Command() {
                Origin = CommandOrigin.Local,
                CommandType = CommandType.PluginsEnable,
                Scope = {
                    PluginGuid = plugins.Plugins.First().PluginModel.PluginGuid
                }
            });

            CommandResultArgs result = plugins.Tunnel(new Command() {
                Name = "NoParameterCommand",
                // We're cheating a little bit here and just saying the command came from
                // "local" as in it was generated by Procon itself.
                Origin = CommandOrigin.Local
            });

            Assert.AreEqual("NoParameterCommandSetResult", result.Message);
        }

        /// <summary>
        /// Test for you to debug.
        /// Set a breakpoint within Procon.Examples.CommandRouting.TunneledCommands.ThisCommandIsInAChildObject
        /// </summary>
        [Test]
        public void TestTunneledCommandPassing() {
            // Create a new plugin controller to load up the test plugin
            PluginController plugins = new PluginController().Execute() as PluginController;

            // Enable the single plugin that was loaded, otherwise it won't recieve any tunneled
            // commands.
            plugins.Tunnel(new Command() {
                Origin = CommandOrigin.Local,
                CommandType = CommandType.PluginsEnable,
                Scope = {
                    PluginGuid = plugins.Plugins.First().PluginModel.PluginGuid
                }
            });

            CommandResultArgs result = plugins.Tunnel(new Command() {
                Name = "ThisCommandIsInAChildObject",
                // We're cheating a little bit here and just saying the command came from
                // "local" as in it was generated by Procon itself.
                Origin = CommandOrigin.Local
            });

            Assert.AreEqual("ThisCommandIsInAChildObjectResult", result.Message);
        }
    }
}
