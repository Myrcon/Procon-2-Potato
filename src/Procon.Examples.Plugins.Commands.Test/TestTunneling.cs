﻿#region Copyright
// Copyright 2014 Myrcon Pty. Ltd.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Procon.Core.Connections.Plugins;
using Procon.Core.Shared;

namespace Procon.Examples.Plugins.CommandRouting.Test {
    [TestFixture]
    public class TestTunneling {
        /// <summary>
        /// Test for you to debug.
        /// Set a breakpoint within Procon.Examples.CommandRouting.Program.SingleParameterCommand
        /// </summary>
        [Test]
        public void TestSingleParameterPassing() {
            // Create a new plugin controller to load up the test plugin
            CorePluginController plugins = (CorePluginController)new CorePluginController().Execute();

            // Enable the single plugin that was loaded, otherwise it won't recieve any tunneled
            // commands.
            plugins.Tunnel(new Command() {
                Origin = CommandOrigin.Local,
                CommandType = CommandType.PluginsEnable,
                Scope = {
                    PluginGuid = plugins.LoadedPlugins.First().PluginGuid
                }
            });

            ICommandResult result = plugins.Tunnel(new Command() {
                Name = "SingleParameterCommand",
                // We're cheating a little bit here and just saying the command came from
                // "local" as in it was generated by Procon itself.
                Origin = CommandOrigin.Local,
                Parameters = new List<ICommandParameter>() {
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
            CorePluginController plugins = (CorePluginController)new CorePluginController().Execute();

            // Enable the single plugin that was loaded, otherwise it won't recieve any tunneled
            // commands.
            plugins.Tunnel(new Command() {
                Origin = CommandOrigin.Local,
                CommandType = CommandType.PluginsEnable,
                Scope = {
                    PluginGuid = plugins.LoadedPlugins.First().PluginGuid
                }
            });

            ICommandResult result = plugins.Tunnel(new Command() {
                Name = "SingleConvertedParameterCommand",
                // We're cheating a little bit here and just saying the command came from
                // "local" as in it was generated by Procon itself.
                Origin = CommandOrigin.Local,
                Parameters = new List<ICommandParameter>() {
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
            CorePluginController plugins = (CorePluginController)new CorePluginController().Execute();

            // Enable the single plugin that was loaded, otherwise it won't recieve any tunneled
            // commands.
            plugins.Tunnel(new Command() {
                Origin = CommandOrigin.Local,
                CommandType = CommandType.PluginsEnable,
                Scope = {
                    PluginGuid = plugins.LoadedPlugins.First().PluginGuid
                }
            });

            ICommandResult result = plugins.Tunnel(new Command() {
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
            CorePluginController plugins = (CorePluginController)new CorePluginController().Execute();

            // Enable the single plugin that was loaded, otherwise it won't recieve any tunneled
            // commands.
            plugins.Tunnel(new Command() {
                Origin = CommandOrigin.Local,
                CommandType = CommandType.PluginsEnable,
                Scope = {
                    PluginGuid = plugins.LoadedPlugins.First().PluginGuid
                }
            });

            ICommandResult result = plugins.Tunnel(new Command() {
                Name = "ThisCommandIsInAChildObject",
                // We're cheating a little bit here and just saying the command came from
                // "local" as in it was generated by Procon itself.
                Origin = CommandOrigin.Local
            });

            Assert.AreEqual("ThisCommandIsInAChildObjectResult", result.Message);
        }
    }
}
