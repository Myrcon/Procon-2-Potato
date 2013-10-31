﻿using System;
using System.Collections.Generic;
using System.Linq;
using Procon.Core.Events;

namespace Procon.Core.Variables {

    public class VariableController : Executable {

        /// <summary>
        /// Anything in this list is volatile and will not be saved on
        /// exit.
        /// </summary>
        public List<Variable> VolatileVariables { get; set; }

        /// <summary>
        /// Anything in this list will be saved to the config
        /// </summary>
        protected List<Variable> ArchiveVariables { get; set; }

        public VariableController() : base() {
            this.VolatileVariables = new List<Variable>();
            this.ArchiveVariables = new List<Variable>();
        }

        // @todo this should be moved to something like "InstanceVariableController" or something.
        // It's otherwise too specialized for something that could be used in plugins.
        private void SetupDefaultVariables() {
            this.Set(new Command() {
                Origin = CommandOrigin.Local
            }, CommonVariableNames.TextCommandPublicPrefix, "!");

            this.Set(new Command() {
                Origin = CommandOrigin.Local
            }, CommonVariableNames.TextCommandProtectedPrefix, "#");

            this.Set(new Command() {
                Origin = CommandOrigin.Local
            }, CommonVariableNames.TextCommandPrivatePrefix, "@");

            this.Set(new Command() {
                Origin = CommandOrigin.Local
            }, CommonVariableNames.DatabaseMaximumSelectedRows, 20);

            Variable variable = this.Variable(CommonVariableNames.PackagesProcon2RepositoryUrl);
            variable.Value = "https://repo.myrcon.com/procon2/";
            variable.Readonly = true;
        }

        #region Executable

        /// <summary>
        /// Begins the execution of this variable controller.
        /// Assigns events and loads the config for this file.
        /// </summary>
        public override ExecutableBase Execute() {
            this.AssignEvents();

            this.SetupDefaultVariables();

            return base.Execute();
        }

        /// <summary>
        /// Information about this object is handled via it's parent interface.
        /// </summary>
        public override void Dispose() {
            foreach (Variable variable in this.VolatileVariables) {
                variable.Dispose();
            }

            foreach (Variable archiveVariable in this.ArchiveVariables) {
                archiveVariable.Dispose();
            }

            this.VolatileVariables.Clear();
            this.VolatileVariables = null;

            this.ArchiveVariables.Clear();
            this.ArchiveVariables = null;
        }

        /// <summary>
        /// Does nothing.  Information about this object is handled via it's parent interface.
        /// </summary>
        public override void WriteConfig(Config config) {
            foreach (Variable archiveVariable in this.ArchiveVariables) {
                config.Root.Add(new Command() {
                    CommandType = CommandType.VariablesSetA,
                    Parameters = new List<CommandParameter>() {
                        new CommandParameter() {
                            Data = {
                                Content = new List<String>() {
                                    archiveVariable.Name
                                }
                            }
                        },
                        new CommandParameter() {
                            Data = {
                                Content = new List<String>() {
                                    archiveVariable.ToString()
                                }
                            }
                        }
                    }
                }.ToConfigCommand());
            }
        }

        #endregion

        protected void AssignEvents() {
            
        }

        /// <summary>
        /// Fetches a variable by name
        /// </summary>
        /// <param name="name">The name of the variable object</param>
        /// <returns>The variable, if available. False otherwise.</returns>
        public Variable Variable(String name) {
            Variable variable = this.VolatileVariables.Find(x => String.Compare(x.Name, name, StringComparison.InvariantCultureIgnoreCase) == 0);

            if (variable == null) {
                variable = new Variable() {
                    Name = name
                };

                this.VolatileVariables.Add(variable);
            }

            return variable;
        }

        /// <summary>
        /// Alias of Variable(String)
        /// </summary>
        /// <param name="name">The name of the variable object</param>
        /// <returns>The variable, if available. False otherwise.</returns>
        public Variable Variable(CommonVariableNames name) {
            return this.Variable(name.ToString());
        }

        /// <summary>
        /// Supports '-' and '--' arguments.
        /// </summary>
        /// <param name="arguments">A list of arguments to pass.</param>
        public void ParseArguments(List<String> arguments) {
            for (int offset = 0; offset < arguments.Count; offset++) {
                String argument = arguments[offset];

                // if the argument is a switch.
                if (argument[0] == '-') {
                    // Trims any hyphens from the start of the argument. Allows for "-argument" and "--argument"
                    argument = argument.TrimStart('-');

                    Variable variable = null;

                    // Does another argument exist?
                    if (offset + 1 < arguments.Count && arguments[offset + 1][0] != '-') {
                        // No, the next string is not an argument switch. It's the value of the
                        // argument.
                        variable = this.Set(new Command() { Origin = CommandOrigin.Local }, argument, arguments[offset + 1]).Now.Variables.FirstOrDefault();
                    }
                    else {
                        // Set to "true"
                        variable = this.Set(new Command() { Origin = CommandOrigin.Local }, argument, true).Now.Variables.FirstOrDefault();
                    }

                    if (variable != null) {
                        variable.Readonly = true;
                    }
                }
            }
        }

        /// <summary>
        /// This will first set the value, then set the value in the archived list
        /// which will be saved to the config
        /// </summary>
        /// <param name="command">Details of the commands origin</param>
        /// <param name="name">The unique name of the variable to set</param>
        /// <param name="value">The value of the variable</param>
        /// <returns></returns>
        [CommandAttribute(CommandType = CommandType.VariablesSet)]
        public CommandResultArgs Set(Command command, String name, Object value) {
            CommandResultArgs result = null;

            if (command.Origin == CommandOrigin.Local || this.Security.DispatchPermissionsCheck(command, command.Name).Success == true) {
                if (name.Length > 0) {
                    Variable variable = this.Variable(name);

                    if (variable.Readonly == false) {
                        variable.Value = value;

                        result = new CommandResultArgs() {
                            Success = true,
                            Status = CommandResultType.Success,
                            Message = String.Format(@"Successfully set value of variable name ""{0}"" to ""{1}"".", variable.Name, variable.Value),
                            Now = new CommandData() {
                                Variables = new List<Variable>() {
                                    variable
                                }
                            }
                        };

                        if (this.Events != null) {
                            this.Events.Log(GenericEventArgs.ConvertToGenericEvent(result, GenericEventType.VariablesSet));
                        }
                    }
                    else {
                        // Variable set to read only and cannot be modified.
                        result = new CommandResultArgs() {
                            Success = false,
                            Status = CommandResultType.Failed,
                            Message = String.Format(@"Variable name ""{0}"" is set to read-only.", variable.Name)
                        };
                    }
                }
                else {
                    result = new CommandResultArgs() {
                        Success = false,
                        Status = CommandResultType.InvalidParameter,
                        Message = "A variable name must not be zero length"
                    };
                }
            }
            else {
                result = CommandResultArgs.InsufficientPermissions;
            }

            return result;
        }

        /// <summary>
        /// This will first set the value, then set the value in the archived list
        /// which will be saved to the config
        /// </summary>
        /// <param name="command">Details of the commands origin</param>
        /// <param name="name">The unique name of the variable to set</param>
        /// <param name="value">The value of the variable</param>
        /// <returns></returns>
        [CommandAttribute(CommandType = CommandType.VariablesSet)]
        public CommandResultArgs Set(Command command, CommonVariableNames name, Object value) {
            return this.Set(command, name.ToString(), value);
        }

        /// <summary>
        /// This will first set the value, then set the value in the archived list
        /// which will be saved to the config
        /// </summary>
        /// <param name="command">Details of the commands origin</param>
        /// <param name="name">The unique name of the variable to set</param>
        /// <param name="value">The value of the variable</param>
        /// <returns></returns>
        [CommandAttribute(CommandType = CommandType.VariablesSetA)]
        public CommandResultArgs SetA(Command command, String name, Object value) {
            CommandResultArgs result = null;

            if (command.Origin == CommandOrigin.Local || this.Security.DispatchPermissionsCheck(command, command.Name).Success == true) {
                CommandResultArgs volatileSetResult = this.Set(command, name, value);

                if (volatileSetResult.Success == true) {
                    // All good.
                    Variable variable = this.Set(command, name, value).Now.Variables.First();

                    if (this.ArchiveVariables.Find(x => x.Name == variable.Name) == null) {
                        this.ArchiveVariables.Add(variable);
                    }
                    else {
                        this.ArchiveVariables.Find(x => x.Name == variable.Name).Value = variable.Value;
                    }

                    result = new CommandResultArgs() {
                        Success = true,
                        Status = CommandResultType.Success,
                        Message = String.Format(@"Successfully set value of variable name ""{0}"" to ""{1}"".", variable.Name, variable.Value),
                        Now = new CommandData() {
                            Variables = new List<Variable>() {
                                variable
                            }
                        }
                    };
                }
                else {
                    // Bubble the error.
                    result = volatileSetResult;
                }
            }
            else {
                result = CommandResultArgs.InsufficientPermissions;
            }

            return result;
        }

        /// <summary>
        /// This will first set the value, then set the value in the archived list
        /// which will be saved to the config
        /// </summary>
        /// <param name="command">Details of the commands origin</param>
        /// <param name="name">The unique key of the variable to set</param>
        /// <param name="value">The value of the variable</param>
        /// <returns></returns>
        [CommandAttribute(CommandType = CommandType.VariablesSetA)]
        public CommandResultArgs SetA(Command command, CommonVariableNames name, Object value) {
            return this.SetA(command, name.ToString(), value);
        }

        /// <summary>
        /// Fetches the first value in the variable list of a type
        /// </summary>
        /// <typeparam name="T">The type of value to fetch from the variable list.</typeparam>
        /// <param name="defaultValue">The default value to use if no value is found</param>
        /// <returns>The first matching variable value or the default value if no value of type T is found</returns>
        public T Get<T>(T defaultValue = default(T)) {
            T result = defaultValue;
            Variable variable = null;

            if ((variable = this.VolatileVariables.Find(x => x.Value is T)) != null) {
                result = (T)variable.Value;
            }

            return result;
        }

        /// <summary>
        /// Gets and converts a value given a name
        /// </summary>
        /// <typeparam name="T">The type of value to return</typeparam>
        /// <param name="name">The unique name of the variable to fetch</param>
        /// <param name="defaultValue"></param>
        /// <returns>The converted value of the variable with the specified name</returns>
        public T Get<T>(String name, T defaultValue = default(T)) {
            T result = defaultValue;

            Variable variable = this.Variable(name);

            result = variable.ToType(defaultValue);

            return result;
        }

        /// <summary>
        /// Gets and converts a value given a name
        /// </summary>
        /// <typeparam name="T">The type of value to return</typeparam>
        /// <param name="name">The unique name of the variable to fetch</param>
        /// <param name="defaultValue"></param>
        /// <returns>The converted value of the variable with the specified kenamey</returns>
        public T Get<T>(CommonVariableNames name, T defaultValue = default(T)) {
            return this.Get(name.ToString(), defaultValue);
        }

        /// <summary>
        /// Proxy for the Get command that uses a blank defaultValue
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [CommandAttribute(CommandType = CommandType.VariablesGet)]
        public CommandResultArgs Get(Command command, String name) {
            return this.Get(command, name, null);
        }

        /// <summary>
        /// Gets a raw value given a knameey, returned as a Object
        /// </summary>
        /// <param name="command">Details of the commands origin</param>
        /// <param name="name">The unique name of the variable to fetch</param>
        /// <param name="defaultValue"></param>
        /// <returns>The raw object with no conversion</returns>
        [CommandAttribute(CommandType = CommandType.VariablesGet)]
        public CommandResultArgs Get(Command command, String name, Object defaultValue = null) {
            CommandResultArgs result = null;

            if (command.Origin == CommandOrigin.Local || this.Security.DispatchPermissionsCheck(command, command.Name).Success == true) {
                if (name.Length > 0) {
                    Variable variable = this.Variable(name);

                    result = new CommandResultArgs() {
                        Success = true,
                        Status = CommandResultType.Success,
                        Message = String.Format(@"Value of variable with name ""{0}"" is ""{1}"".", variable.Name, variable.Value),
                        Now = new CommandData() {
                            Variables = new List<Variable>() {
                                variable
                            }
                        }
                    };
                }
                else {
                    result = new CommandResultArgs() {
                        Success = false,
                        Status = CommandResultType.InvalidParameter,
                        Message = "A variable name must not be zero length"
                    };
                }
            }
            else {
                result = CommandResultArgs.InsufficientPermissions;
            }

            return result;
        }

        /// <summary>
        /// Gets a raw value given a name, returned as a Object
        /// </summary>
        /// <param name="command">Details of the commands origin</param>
        /// <param name="name">The unique name of the variable to fetch</param>
        /// <param name="defaultValue"></param>
        /// <returns>The raw object with no conversion</returns>
        [CommandAttribute(CommandType = CommandType.VariablesGet)]
        public CommandResultArgs Get(Command command, CommonVariableNames name, Object defaultValue = null) {
            return this.Get(command, name.ToString());
        }

        /// <summary>
        /// Fetches and converts a value from the variables archive. This is only exposed in VariableController
        /// because it's only use is for unit testing.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetA<T>(String name, T defaultValue = default(T)) {

            T result = defaultValue;
            Variable variable = null;

            if ((variable = this.ArchiveVariables.Find(x => x.Name == name)) != null) {
                result = variable.ToType<T>();
            }

            return result;
        }

        /// <summary>
        /// Proxy for GetA(string, T)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetA<T>(CommonVariableNames name, T defaultValue = default(T)) {
            return this.GetA(name.ToString(), defaultValue);
        }
    }
}