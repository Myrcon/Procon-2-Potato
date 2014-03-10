using System;
using System.Collections.Generic;
using System.Globalization;
using Procon.Core.Shared.Events;
using Procon.Core.Shared.Models;
using Procon.Database.Shared;

namespace Procon.Core.Shared {
    /// <summary>
    /// Helps shortcut some of the command code by building and returning a command.
    /// </summary>
    /// <remarks>
    ///     <para>This class will be added to as I go, just to cleanup some of the existing code.</para>
    /// </remarks>
    public static class CommandBuilder {
        /// <summary>
        /// Builds a command to send a DatabaseQuery with the database group to use.
        /// </summary>
        /// <param name="group">The name of the database group to use</param>
        /// <param name="queries">The queries to send </param>
        /// <returns>The built command to dispatch</returns>
        public static ICommand DatabaseQuery(String group, params IDatabaseObject[] queries) {
            return new Command() {
                CommandType = CommandType.DatabaseQuery,
                Parameters = new List<ICommandParameter>() {
                    new CommandParameter() {
                        Data = {
                            Content = new List<String>() {
                                group
                            }
                        }
                    },
                    new CommandParameter() {
                        Data = {
                            Queries = new List<IDatabaseObject>(queries)
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Builds a command to send a DatabaseQuery.
        /// </summary>
        /// <param name="queries">The queries to send </param>
        /// <returns>The built command to dispatch</returns>
        public static ICommand DatabaseQuery(params IDatabaseObject[] queries) {
            return new Command() {
                CommandType = CommandType.DatabaseQuery,
                Parameters = new List<ICommandParameter>() {
                    new CommandParameter() {
                        Data = {
                            Queries = new List<IDatabaseObject>(queries)
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Builds a command to send a InstanceServiceRestart signal
        /// </summary>
        /// <returns>The built command to the dispatch</returns>
        public static ICommand InstanceServiceRestart() {
            return new Command() {
                CommandType = CommandType.InstanceServiceRestart
            };
        }

        /// <summary>
        /// Builds a command to send a InstanceServiceMergePackage signal
        /// </summary>
        /// <param name="uri">The uri of the repository to find the package source in</param>
        /// <param name="packageId">The package id to install</param>
        /// <returns>The built command to the dispatch</returns>
        public static ICommand InstanceServiceMergePackage(String uri, String packageId) {
            return new Command() {
                CommandType = CommandType.InstanceServiceMergePackage,
                Parameters = new List<ICommandParameter>() {
                    new CommandParameter() {
                        Data = {
                            Content = new List<String>() {
                                uri
                            }
                        }
                    },
                    new CommandParameter() {
                        Data = {
                            Content = new List<String>() {
                                packageId
                            }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Builds a command to send a InstanceServiceUninstallPackage signal
        /// </summary>
        /// <param name="packageId">The package id to uninstall</param>
        /// <returns>The built command to the dispatch</returns>
        public static ICommand InstanceServiceUninstallPackage(String packageId) {
            return new Command() {
                CommandType = CommandType.InstanceServiceUninstallPackage,
                Parameters = new List<ICommandParameter>() {
                    new CommandParameter() {
                        Data = {
                            Content = new List<String>() {
                                packageId
                            }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Builds a command to send a PackagesMergePackage
        /// </summary>
        /// <param name="packageId">The package id to install</param>
        /// <returns>The built command to the dispatch</returns>
        public static ICommand PackagesMergePackage(String packageId) {
            return new Command() {
                CommandType = CommandType.PackagesMergePackage,
                Parameters = new List<ICommandParameter>() {
                    new CommandParameter() {
                        Data = {
                            Content = new List<String>() {
                                packageId
                            }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Builds a command to send a PackagesUninstallPackage
        /// </summary>
        /// <param name="packageId">The package id to install</param>
        /// <returns>The built command to the dispatch</returns>
        public static ICommand PackagesUninstallPackage(String packageId) {
            return new Command() {
                CommandType = CommandType.PackagesUninstallPackage,
                Parameters = new List<ICommandParameter>() {
                    new CommandParameter() {
                        Data = {
                            Content = new List<String>() {
                                packageId
                            }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Builds a command to send a PackagesFetchPackages
        /// </summary>
        /// <returns>The built command to the dispatch</returns>
        public static ICommand PackagesFetchPackages() {
            return new Command() {
                CommandType = CommandType.PackagesFetchPackages
            };
        }

        /// <summary>
        /// Builds a command to send a SecurityAccountAuthenticate
        /// </summary>
        /// <param name="username">The username to attach to the command and parameter</param>
        /// <param name="passwordPlainText">The plain text password to add as a parameter</param>
        /// <returns>The build command to dispatch</returns>
        public static ICommand SecurityAccountAuthenticate(String username, String passwordPlainText) {
            return new Command() {
                Authentication = {
                    Username = username
                },
                CommandType = CommandType.SecurityAccountAuthenticate,
                Parameters = new List<ICommandParameter>() {
                    new CommandParameter() {
                        Data = {
                            Content = new List<String>() {
                                username
                            }
                        }
                    },
                    new CommandParameter() {
                        Data = {
                            Content = new List<String>() {
                                passwordPlainText
                            }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Builds a command to send a VariablesSet
        /// </summary>
        /// <param name="name">The name of the variable to set</param>
        /// <param name="value">The value to assign to the variable</param>
        /// <returns>The built command to dispatch</returns>
        public static ICommand VariablesSet(String name, String value) {
            return new Command() {
                CommandType = CommandType.VariablesSet,
                Parameters = new List<ICommandParameter>() {
                    new CommandParameter() {
                        Data = {
                            Content = new List<String>() {
                                name
                            }
                        }
                    },
                    new CommandParameter() {
                        Data = {
                            Content = new List<String>() {
                                value
                            }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Builds a command to send a VariablesSet
        /// </summary>
        /// <param name="name">The name of the variable to set</param>
        /// <param name="value">The value to assign to the variable</param>
        /// <returns>The built command to dispatch</returns>
        public static ICommand VariablesSet(String name, List<String> value) {
            return new Command() {
                CommandType = CommandType.VariablesSet,
                Parameters = new List<ICommandParameter>() {
                    new CommandParameter() {
                        Data = {
                            Content = new List<String>() {
                                name
                            }
                        }
                    },
                    new CommandParameter() {
                        Data = {
                            Content = value
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Builds a command to send a VariablesSet
        /// </summary>
        /// <param name="name">The name of the variable to set</param>
        /// <param name="value">The value to assign to the variable</param>
        /// <returns>The built command to dispatch</returns>
        public static ICommand VariablesSet(CommonVariableNames name, String value) {
            return VariablesSet(name.ToString(), value);
        }

        /// <summary>
        /// Builds a command to send a VariablesSet
        /// </summary>
        /// <param name="name">The name of the variable to set</param>
        /// <param name="value">The value to assign to the variable</param>
        /// <returns>The built command to dispatch</returns>
        public static ICommand VariablesSet(CommonVariableNames name, List<String> value) {
            return VariablesSet(name.ToString(), value);
        }

        /// <summary>
        /// Builds a command to send a VariablesSetA
        /// </summary>
        /// <param name="name">The name of the variable to set</param>
        /// <param name="value">The value to assign to the variable</param>
        /// <returns>The built command to dispatch</returns>
        public static ICommand VariablesSetA(String name, String value) {
            return new Command() {
                CommandType = CommandType.VariablesSetA,
                Parameters = new List<ICommandParameter>() {
                    new CommandParameter() {
                        Data = {
                            Content = new List<String>() {
                                name
                            }
                        }
                    },
                    new CommandParameter() {
                        Data = {
                            Content = new List<String>() {
                                value
                            }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Builds a command to send a VariablesSetA
        /// </summary>
        /// <param name="name">The name of the variable to set</param>
        /// <param name="value">The value to assign to the variable</param>
        /// <returns>The built command to dispatch</returns>
        public static ICommand VariablesSetA(String name, List<String> value) {
            return new Command() {
                CommandType = CommandType.VariablesSetA,
                Parameters = new List<ICommandParameter>() {
                    new CommandParameter() {
                        Data = {
                            Content = new List<String>() {
                                name
                            }
                        }
                    },
                    new CommandParameter() {
                        Data = {
                            Content = value
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Builds a command to send a VariablesSetA
        /// </summary>
        /// <param name="name">The name of the variable to set</param>
        /// <param name="value">The value to assign to the variable</param>
        /// <returns>The built command to dispatch</returns>
        public static ICommand VariablesSetA(CommonVariableNames name, String value) {
            return VariablesSetA(name.ToString(), value);
        }

        /// <summary>
        /// Builds a command to send a VariablesSetA
        /// </summary>
        /// <param name="name">The name of the variable to set</param>
        /// <param name="value">The value to assign to the variable</param>
        /// <returns>The built command to dispatch</returns>
        public static ICommand VariablesSetA(CommonVariableNames name, List<String> value) {
            return VariablesSetA(name.ToString(), value);
        }

        /// <summary>
        /// Builds a command to send a VariablesSetF
        /// </summary>
        /// <param name="name">The name of the variable to set</param>
        /// <param name="value">The value to assign to the variable</param>
        /// <returns>The built command to dispatch</returns>
        public static ICommand VariablesSetF(String name, String value) {
            return new Command() {
                CommandType = CommandType.VariablesSetF,
                Parameters = new List<ICommandParameter>() {
                    new CommandParameter() {
                        Data = {
                            Content = new List<String>() {
                                name
                            }
                        }
                    },
                    new CommandParameter() {
                        Data = {
                            Content = new List<String>() {
                                value
                            }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Builds a command to send a VariablesSetF
        /// </summary>
        /// <param name="name">The name of the variable to set</param>
        /// <param name="value">The value to assign to the variable</param>
        /// <returns>The built command to dispatch</returns>
        public static ICommand VariablesSetF(CommonVariableNames name, String value) {
            return VariablesSetF(name.ToString(), value);
        }

        /// <summary>
        /// Builds a command to send a VariablesGet
        /// </summary>
        /// <param name="name">The name of the variable to set</param>
        /// <returns>The built command to dispatch</returns>
        public static ICommand VariablesGet(String name) {
            return new Command() {
                CommandType = CommandType.VariablesGet,
                Parameters = new List<ICommandParameter>() {
                    new CommandParameter() {
                        Data = {
                            Content = new List<String>() {
                                name
                            }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Builds a command to send a VariablesGet
        /// </summary>
        /// <param name="name">The name of the variable to set</param>
        /// <returns>The built command to dispatch</returns>
        public static ICommand VariablesGet(CommonVariableNames name) {
            return VariablesGet(name.ToString());
        }

        /// <summary>
        /// Builds a command to send a EventsLog
        /// </summary>
        /// <returns>The built command to dispatch</returns>
        public static ICommand EventsLog(IGenericEvent e) {
            return new Command() {
                CommandType = CommandType.EventsLog,
                Parameters = new List<ICommandParameter>() {
                    new CommandParameter() {
                        Data = {
                            Events = new List<IGenericEvent>() {
                                e
                            }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Builds a command to send a SecurityAddGroup
        /// </summary>
        /// <param name="groupName">The name of the group create+add</param>
        /// <returns>The built command to dispatch</returns>
        public static ICommand SecurityAddGroup(String groupName) {
            return new Command() {
                CommandType = CommandType.SecurityAddGroup,
                Parameters = new List<ICommandParameter>() {
                    new CommandParameter() {
                        Data = {
                            Content = new List<String>() {
                                groupName
                            }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Builds a command to send a SecurityRemoveGroup
        /// </summary>
        /// <param name="groupName">The name of the group to remove</param>
        /// <returns>The built command to dispatch</returns>
        public static ICommand SecurityRemoveGroup(String groupName) {
            return new Command() {
                CommandType = CommandType.SecurityRemoveGroup,
                Parameters = new List<ICommandParameter>() {
                    new CommandParameter() {
                        Data = {
                            Content = new List<String>() {
                                groupName
                            }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Builds a command to send a SecurityGroupAddAccount
        /// </summary>
        /// <param name="groupName">The name of the group to add an account to</param>
        /// <param name="accountName">The name of the account to create+add</param>
        /// <returns>The built command to dispatch</returns>
        public static ICommand SecurityGroupAddAccount(String groupName, String accountName) {
            return new Command() {
                CommandType = CommandType.SecurityGroupAddAccount,
                Parameters = new List<ICommandParameter>() {
                    new CommandParameter() {
                        Data = {
                            Content = new List<String>() {
                                groupName
                            }
                        }
                    },
                    new CommandParameter() {
                        Data = {
                            Content = new List<String>() {
                                accountName
                            }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Builds a command to send a SecurityRemoveAccount
        /// </summary>
        /// <param name="accountName">The name of the account to remove</param>
        /// <returns>The built command to dispatch</returns>
        public static ICommand SecurityRemoveAccount(String accountName) {
            return new Command() {
                CommandType = CommandType.SecurityRemoveAccount,
                Parameters = new List<ICommandParameter>() {
                    new CommandParameter() {
                        Data = {
                            Content = new List<String>() {
                                accountName
                            }
                        }
                    }
                }
            };
        }
        
        /// <summary>
        /// Builds a command to send a SecurityGroupSetPermission
        /// </summary>
        /// <param name="groupName">The name of the group to modify the permission of</param>
        /// <param name="permissionName">The name of the permission to set the authority of</param>
        /// <param name="authority">The level of authority to set for this permission</param>
        /// <returns>The built command to dispatch</returns>
        public static ICommand SecurityGroupSetPermission(String groupName, String permissionName, int authority) {
            return new Command() {
                CommandType = CommandType.SecurityGroupSetPermission,
                Parameters = new List<ICommandParameter>() {
                    new CommandParameter() {
                        Data = {
                            Content = new List<String>() {
                                groupName
                            }
                        }
                    },
                    new CommandParameter() {
                        Data = {
                            Content = new List<String>() {
                                permissionName
                            }
                        }
                    },
                    new CommandParameter() {
                        Data = {
                            Content = new List<String>() {
                                authority.ToString(CultureInfo.InvariantCulture)
                            }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Alias of SecurityGroupSetPermission(String, String, int)
        /// </summary>
        /// <param name="groupName">The name of the group to modify the permission of</param>
        /// <param name="permissionName">The name of the permission to set the authority of</param>
        /// <param name="authority">The level of authority to set for this permission</param>
        /// <returns>The built command to dispatch</returns>
        public static ICommand SecurityGroupSetPermission(String groupName, CommandType permissionName, int authority) {
            return CommandBuilder.SecurityGroupSetPermission(groupName, permissionName.ToString(), authority);
        }

        /// <summary>
        /// Builds a command to send a SecuritySetPredefinedStreamPermissions
        /// </summary>
        /// <param name="groupName">The name of the group to modify the permissions of</param>
        /// <returns>The built command to dispatch</returns>
        public static ICommand SecuritySetPredefinedStreamPermissions(String groupName) {
            return new Command() {
                CommandType = CommandType.SecuritySetPredefinedStreamPermissions,
                Parameters = new List<ICommandParameter>() {
                    new CommandParameter() {
                        Data = {
                            Content = new List<String>() {
                                groupName
                            }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Builds a command to send a SecuritySetPredefinedAdministratorsPermissions
        /// </summary>
        /// <param name="groupName">The name of the group to modify the permissions of</param>
        /// <returns>The built command to dispatch</returns>
        public static ICommand SecuritySetPredefinedAdministratorsPermissions(String groupName) {
            return new Command() {
                CommandType = CommandType.SecuritySetPredefinedAdministratorsPermissions,
                Parameters = new List<ICommandParameter>() {
                    new CommandParameter() {
                        Data = {
                            Content = new List<String>() {
                                groupName
                            }
                        }
                    }
                }
            };
        }
    }
}
