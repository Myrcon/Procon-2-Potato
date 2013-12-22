﻿using System.Linq;
using NUnit.Framework;
using Procon.Core;
using Procon.Core.Connections.Plugins;
using Procon.Core.Shared;

namespace Procon.Examples.UserInterface.Test {
    [TestFixture]
    public class TestPageRendering {
        /// <summary>
        /// Test for you to debug.
        /// Set a breakpoint within Procon.Examples.PluginUserInterface.Program.PageIndex
        /// </summary>
        [Test]
        public void TestIndexRender() {
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
                Name = "/",
                // We're cheating a little bit here and just saying the command came from
                // "local" as in it was generated by Procon itself.
                Origin = CommandOrigin.Local
            });

            // The content below will be rendered in the UI sandbox
            // The UI will catch href and load the next page with the results of that command.
            Assert.AreEqual("Hey, this is the index of my plugin. The first page people will see! Check out the <a href=\"/settings\">Settings</a>.", result.Now.Content.First());
        }

        /// <summary>
        /// Test for you to debug.
        /// Set a breakpoint within Procon.Examples.PluginUserInterface.Program.PageSettings
        /// </summary>
        /// <remarks>You should note the fully qualified names within PluginUserInterface.Pages.SettingsPageView.tt</remarks>
        [Test]
        public void TestSettingsRender() {
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
                Name = "/settings",
                // We're cheating a little bit here and just saying the command came from
                // "local" as in it was generated by Procon itself.
                Origin = CommandOrigin.Local
            });

            // The content below will be rendered in the UI sandbox
            // The UI will catch href and load the next page with the results of that command.
            Assert.AreEqual("<h2>Settings</h2><b>Output of the variable!</b>Player1 (Score: 100)<br/>Player2 (Score: 250)<br/>", result.Now.Content.First());
        }
    }
}
