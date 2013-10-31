﻿using System;

namespace Procon.Core.Events {

    /// <summary>
    /// Common event names that originate from Procon.Core.
    /// </summary>
    [Serializable]
    public enum GenericEventType {
        None,

        /// <summary>
        /// A connection has been added.
        /// </summary>
        InstanceConnectionAdded,
        /// <summary>
        /// A connection has been removed.
        /// </summary>
        InstanceConnectionRemoved,
        /// <summary>
        /// The instance has posted a restart signal.
        /// </summary>
        InstanceServiceRestarting,

        /// <summary>
        /// A new group has been added.
        /// </summary>
        SecurityGroupAdded,
        /// <summary>
        /// A group has been removed, along with all of the accounts attached to it.
        /// </summary>
        SecurityGroupRemoved,
        /// <summary>
        /// An authority level on a permission attached to a group has been modified.
        /// </summary>
        SecurityGroupPermissionAuthorityChanged,
        /// <summary>
        /// A security group has cloned its permissions from another group.
        /// </summary>
        SecurityGroupPermissionsCopied,
        /// <summary>
        /// An account has been added to a group.
        /// </summary>
        SecurityAccountAdded,
        /// <summary>
        /// An account has been removed (deleted permanently)
        /// </summary>
        SecurityAccountRemoved,
        /// <summary>
        /// A player has been added to an account
        /// </summary>
        SecurityPlayerAdded,
        /// <summary>
        /// A player has been removed from an account
        /// </summary>
        SecurityPlayerRemoved,

        /// <summary>
        /// The repository controller has rebuilt what it knows about the packages (local, remote, updated etc.)
        /// </summary>
        RepositoriesPackagesRebuilt,
        /// <summary>
        /// A package has been loaded (as in, Procon recognizes a new package from a repository)
        /// </summary>
        RepositoriesPackageLoaded,
        /// <summary>
        /// A package has change state (installed, updated etc.)
        /// </summary>
        RepositoriesPackageStateChanged,
        /// <summary>
        /// A remote repository has been added
        /// </summary>
        RepositoriesRepositoryAdded,
        /// <summary>
        /// A remote repository has been removed
        /// </summary>
        RepositoriesRepositoryRemoved,

        /// <summary>
        /// A variable has been set
        /// </summary>
        VariablesSet,

        /// <summary>
        /// A text command has been registered with the text controller
        /// </summary>
        TextCommandRegistered,
        /// <summary>
        /// A text command has been removed from the text controller.
        /// </summary>
        TextCommandUnregistered,
        /// <summary>
        /// A text command has been executed (matched & dispatched to plugin)
        /// </summary>
        TextCommandExecuted,
        /// <summary>
        /// A text command has been previewed (matched, but not executed)
        /// </summary>
        TextCommandPreviewed,

        /// <summary>
        /// A plugin has been loaded
        /// </summary>
        PluginsPluginAdded,
        /// <summary>
        /// A plugin has been unloaded.
        /// </summary>
        PluginsPluginRemoved,

        /// <summary>
        /// All methods are currently being registered so the plugin can communicate back with procon
        /// </summary>
        PluginsRegisteringCallbacks,
        /// <summary>
        /// Methods have been registered, the plugin can now function as normal
        /// </summary>
        PluginsCallbacksRegistered,


        /// <summary>
        /// The path and config name have been set, the config may now be loaded.
        /// </summary>
        ConfigSetup,
        /// <summary>
        /// The config for is being loaded
        /// </summary>
        ConfigLoading,
        /// <summary>
        /// The config for has been loaded
        /// </summary>
        ConfigLoaded
    }
}