﻿using System;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Procon.Core.Connections.Plugins {
    using Procon.Core.Events;
    using Procon.Net;
    using Procon.Core.Utils;

    /// <summary>
    /// This is the Procon side class to handle the proxy to the app domain, as well as the plugins
    /// cleanup.
    /// </summary>
    public sealed class Plugin : Executable {

        /// <summary>
        /// The name of the plugin, also used as it's namespace
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// The loaded plugin GUID
        /// </summary>
        public Guid PluginGuid {
            get {
                return this.AppDomainPlugin != null ? this.AppDomainPlugin.PluginGuid : Guid.Empty;
            }
            // ReSharper disable ValueParameterNotUsed
            set { }
            // ReSharper restore ValueParameterNotUsed
        }

        [XmlIgnore, JsonIgnore]
        public String Path { get; set; }

        /// <summary>
        /// Reference to the plugin loader proxy
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public PluginLoaderProxy PluginFactory { get; set; }

        /// <summary>
        /// Ultimately the owner of this plugin.
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public Connection Connection { get; set; }

        /// <summary>
        /// Reference to the plugin loaded in the AppDomain for remoting calls.
        /// </summary>
        private IPluginBase AppDomainPlugin { get; set; }

        public override object InitializeLifetimeService() {
            return null;
        }

        public override ExecutableBase Execute() {
            if (File.Exists(this.Path) == true) {
                this.Name = new FileInfo(this.Path).Name.Replace(".dll", "");

                this.AppDomainPlugin = this.PluginFactory.Create(this.Path, this.Name + ".Program");

                if (this.AppDomainPlugin != null) {

                    // Tell the plugin we are about the setup the callbacks
                    this.AppDomainPlugin.GenericEvent(new GenericEventArgs() {
                        GenericEventType = GenericEventType.PluginsRegisteringCallbacks
                    });

                    this.AppDomainPlugin.ProxyExecuteCallback = new PluginBase.CommandHandler(this.ProxyExecute);

                    // register game specific call backs. Connection can be null during unit testing.
                    if (this.Connection != null && this.Connection.Game != null) {

                        this.AppDomainPlugin.ConnectionGuid = this.Connection.ConnectionGuid;

                        this.Connection.Game.ClientEvent += new Game.ClientEventHandler(Connection_ClientEvent);
                        this.Connection.Game.GameEvent += new Game.GameEventHandler(Connection_GameEvent);
                    }

                    // Tell the plugin that callback have been registered and it may start sending out commands.
                    this.AppDomainPlugin.GenericEvent(new GenericEventArgs() {
                        GenericEventType = GenericEventType.PluginsCallbacksRegistered
                    });

                    // Connection and Game could be null if we're unit testing.
                    if (this.Connection != null && this.Connection.Game != null) {

                        // check the plugin's config directory
                        this.AppDomainPlugin.ConfigDirectoryInfo = new DirectoryInfo(System.IO.Path.Combine(Defines.ConfigsDirectory, System.IO.Path.Combine(PathValidator.Valdiate(String.Format("{0}_{1}", this.Connection.Hostname, this.Connection.Port)), PathValidator.Valdiate(this.Name))));
                        this.AppDomainPlugin.ConfigDirectoryInfo.Create();

                        // check the plugin's log directory
                        this.AppDomainPlugin.LogDirectoryInfo = new DirectoryInfo(System.IO.Path.Combine(Defines.LogsDirectory, System.IO.Path.Combine(PathValidator.Valdiate(String.Format("{0}_{1}", this.Connection.Hostname, this.Connection.Port)), PathValidator.Valdiate(this.Name))));

                        if (!this.AppDomainPlugin.LogDirectoryInfo.Exists) {
                            this.AppDomainPlugin.LogDirectoryInfo.Create();
                        }
                    }

                    // Tell the plugin that everything is setup and ready for it to start loading
                    // its config.
                    this.AppDomainPlugin.GenericEvent(new GenericEventArgs() {
                        GenericEventType = GenericEventType.ConfigSetup
                    });
                }
            }

            return this;
        }

        private void Connection_ClientEvent(Game sender, ClientEventArgs e) {
            if (this.AppDomainPlugin != null && e.EventType != ClientEventType.ClientPacketReceived && e.EventType != ClientEventType.ClientPacketSent) {
                this.AppDomainPlugin.ClientEvent(e);
            }
        }

        private void Connection_GameEvent(Game sender, GameEventArgs e) {
            if (this.AppDomainPlugin != null) {
                this.AppDomainPlugin.GameEvent(e);
            }
        }

        /// <summary>
        /// Executes a command in the scope of connection or the entire instance of procon.
        /// </summary>
        /// <remarks><para>This is a proxy called from the plugins appdomain.</para></remarks>
        /// <param name="command"></param>
        /// <returns></returns>
        public CommandResultArgs ProxyExecute(Command command) {
            CommandResultArgs result = null;

            command.Origin = CommandOrigin.Plugin;

            // We check for null's on these in case of unit testing.
            if (this.Connection != null && this.Connection.Instance != null) {
                if (command.Scope != null && command.Scope.ConnectionGuid != Guid.Empty) {
                    command.Scope.ConnectionGuid = this.Connection.ConnectionGuid;

                    // Optimization to bypass Instance (and other connections), but passing this to Instance would have the same effect.
                    result = this.Connection.Execute(command);
                }
                else {
                    result = this.Connection.Instance.Execute(command);
                }
            }

            return result;
        }

        public override void Dispose() {

            if (this.AppDomainPlugin != null) {
                this.AppDomainPlugin.ProxyExecuteCallback = null;

                this.AppDomainPlugin.Dispose();
            }

            // Disposed of in the plugin controller.
            this.PluginFactory = null;

            this.Connection = null;

            base.Dispose();
        }
    }
}