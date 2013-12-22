﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Procon.Core.Connections.TextCommands.Parsers;
using Procon.Core.Localization;
using Procon.Core.Shared;
using Procon.Core.Shared.Events;
using Procon.Core.Shared.Models;
using Procon.Net.Shared.Models;

namespace Procon.Core.Connections.TextCommands {

    /// <summary>
    /// Manages registering, dispatching text commands
    /// </summary>
    public class TextCommandController : Executable {

        /// <summary>
        /// Full list of text commands to check against.
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public List<TextCommandModel> TextCommands { get; protected set; }

        /// <summary>
        /// The owner of this controller, used to lookup the game with all the player data and such in it.
        /// </summary>
        [XmlIgnore, JsonIgnore]
        public Connection Connection { get; set; }
        
        /// <summary>
        /// Creates new controller with the default attributes set
        /// </summary>
        public TextCommandController() {
            this.TextCommands = new List<TextCommandModel>();

            this.AppendDispatchHandlers(new Dictionary<CommandAttribute, CommandDispatchHandler>() {
                {
                    new CommandAttribute() {
                        CommandType = CommandType.TextCommandsExecute,
                        ParameterTypes = new List<CommandParameterType>() {
                            new CommandParameterType() {
                                Name = "text",
                                Type = typeof(String)
                            }
                        }
                    },
                    new CommandDispatchHandler(this.ExecuteTextCommand)
                }, {
                    new CommandAttribute() {
                        CommandType = CommandType.TextCommandsPreview,
                        ParameterTypes = new List<CommandParameterType>() {
                            new CommandParameterType() {
                                Name = "text",
                                Type = typeof(String)
                            }
                        }
                    },
                    new CommandDispatchHandler(this.PreviewTextCommand)
                }, {
                    new CommandAttribute() {
                        CommandType = CommandType.TextCommandsRegister,
                        ParameterTypes = new List<CommandParameterType>() {
                            new CommandParameterType() {
                                Name = "textCommand",
                                Type = typeof(TextCommandModel)
                            }
                        }
                    },
                    new CommandDispatchHandler(this.RegisterTextCommand)
                }, {
                    new CommandAttribute() {
                        CommandType = CommandType.TextCommandsUnregister,
                        ParameterTypes = new List<CommandParameterType>() {
                            new CommandParameterType() {
                                Name = "textCommand",
                                Type = typeof(TextCommandModel)
                            }
                        }
                    },
                    new CommandDispatchHandler(this.UnregisterTextCommand)
                }
            });
        }

        /// <summary>
        /// Does nothing.  Information about this object is handled via it's parent interface.
        /// </summary>
        public override void Dispose() {
            foreach (TextCommandModel textCommand in this.TextCommands) {
                textCommand.Dispose();
            }

            this.TextCommands.Clear();
            this.TextCommands = null;

            this.Connection = null;

            base.Dispose();
        }

        /// <summary>
        /// Executes a text command using the NLP parser.
        /// </summary>
        /// <param name="speaker">The player executing the command</param>
        /// <param name="speakerAccount">The account executing the command</param>
        /// <param name="commands">A list of commands to check against </param>
        /// <param name="prefix">The first valid character of the command being executed</param>
        /// <param name="text">The next, minus the first character</param>
        /// <returns>The generated event, if any.</returns>
        protected CommandResultArgs ParseFuzzy(Player speaker, AccountModel speakerAccount, List<TextCommandModel> commands, String prefix, String text) {

            CommandResultArgs commandResult = null;
            Language selectedLanguage = null;

            if (speakerAccount != null && speakerAccount.PreferredLanguageCode != String.Empty) {
                selectedLanguage = this.Languages.LoadedLanguageFiles.Find(language => language.LanguageModel.LanguageCode == speakerAccount.PreferredLanguageCode);
            }
            else {
                selectedLanguage = this.Languages.Default;
            }

            if (selectedLanguage != null) {
                ITextCommandParser parser = new FuzzyParser() {
                    Connection = this.Connection,
                    TextCommands = commands,
                    Document = selectedLanguage.Root,
                    SpeakerPlayer = speaker,
                    SpeakerAccount = speakerAccount
                };

                commandResult = parser.Parse(prefix, text);
            }

            return commandResult;
        }

        /// <summary>
        /// Runs through the various parsers for all of the text commands.
        /// </summary>
        /// <remarks>
        /// This method may fire multiple events to execute multiple commands
        /// when more than one parser is included in the future. This is expected
        /// behaviour.
        /// </remarks>
        /// <param name="speakerNetworkPlayer"></param>
        /// <param name="speakerAccount"></param>
        /// <param name="prefix"></param>
        /// <param name="text"></param>
        /// <returns>The generated event, if any.</returns>
        protected CommandResultArgs Parse(Player speakerNetworkPlayer, AccountModel speakerAccount, String prefix, String text) {

            // This could execute more in the future, in which case
            // this.TextCommands.Where(x => x.Parser == Parser.NLP).ToList()
            // would be passed to ExecuteNLP
            return this.ParseFuzzy(speakerNetworkPlayer, speakerAccount, this.TextCommands, prefix, text);
        }

        /// <summary>
        /// Fetches a player in the current game connection that is
        /// asociated with the account executing this command.
        /// 
        /// This is used so an account over a layer can issue a command like
        /// "kick me", but we only know "me" from the context of the account
        /// issuing the command. We use this to fetch the player in the game
        /// so we know who to kick.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="speaker"></param>
        /// <returns></returns>
        protected Player GetAccountNetworkPlayer(Command command, AccountModel speaker) {
            Player player = this.Connection.GameState.Players.FirstOrDefault(x => x.Uid == command.Uid);

            if (speaker != null) {
                AccountPlayerModel accountPlayer = speaker.Players.FirstOrDefault(p => p.GameType == this.Connection.ConnectionModel.GameType.Type);

                if (accountPlayer != null) {
                    player = this.Connection.GameState.Players.FirstOrDefault(x => x.Uid == accountPlayer.Uid);
                }
            }

            return player;
        }

        /// <summary>
        /// Parses then fires an event to execute a text command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        /// <returns>The generated event, if any.</returns>
        public CommandResultArgs ExecuteTextCommand(Command command, Dictionary<String, CommandParameter> parameters) {
            CommandResultArgs result = null;

            String text = parameters["text"].First<String>();

            if (this.Security.DispatchPermissionsCheck(command, command.Name).Success == true) {
                AccountModel speakerAccount = this.Security.GetAccount(command);

                // @todo pull the prefix from the text?

                String prefix = this.Variables.Get<String>(CommonVariableNames.TextCommandPublicPrefix);

                result = this.Parse(this.GetAccountNetworkPlayer(command, speakerAccount), speakerAccount, prefix, text) ?? command.Result;

                // todo fire event? GenericEventType.TextCommandExecuted
            }
            else {
                result = CommandResultArgs.InsufficientPermissions;
            }

            return result;
        }

        /// <summary>
        /// Parses then fires an event to preview the results of a text command.
        /// 
        /// Essentially does everything that parsing does, but fires a different event.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        /// <returns>The generated event, if any.</returns>
        public CommandResultArgs PreviewTextCommand(Command command, Dictionary<String, CommandParameter> parameters) {
            CommandResultArgs result = null;

            String text = parameters["text"].First<String>();

            if (this.Security.DispatchPermissionsCheck(command, command.Name).Success == true) {
                AccountModel speakerAccount = this.Security.GetAccount(command);

                // @todo pull the prefix from the text?

                String prefix = this.Variables.Get<String>(CommonVariableNames.TextCommandPublicPrefix);

                result = this.Parse(this.GetAccountNetworkPlayer(command, speakerAccount), speakerAccount, prefix, text) ?? command.Result;

                // todo fire event? GenericEventType.TextCommandPreviewed
            }
            else {
                result = CommandResultArgs.InsufficientPermissions;
            }

            return result;
        }

        /// <summary>
        /// Register a text command with this controller
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public CommandResultArgs RegisterTextCommand(Command command, Dictionary<String, CommandParameter> parameters) {
            CommandResultArgs result = null;

            TextCommandModel textCommand = parameters["textCommand"].First<TextCommandModel>();

            if (this.Security.DispatchPermissionsCheck(command, command.Name).Success == true) {
                TextCommandModel existingRegisteredCommand = this.TextCommands.Find(existingCommand => existingCommand.PluginUid == textCommand.PluginUid && existingCommand.PluginCommand == textCommand.PluginCommand);

                if (existingRegisteredCommand == null) {
                    this.TextCommands.Add(textCommand);

                    result = new CommandResultArgs() {
                        Success = true,
                        Status = CommandResultType.Success,
                        Scope = {
                            Connections = new List<ConnectionModel>() {
                                this.Connection != null ? this.Connection.ConnectionModel : null
                            }
                        },
                        Now = new CommandData() {
                            TextCommands = new List<TextCommandModel>() {
                                textCommand
                            }
                        }
                    };

                    if (this.Events != null) {
                        this.Events.Log(GenericEventArgs.ConvertToGenericEvent(result, GenericEventType.TextCommandRegistered));
                    }
                }
                else {
                    result = new CommandResultArgs() {
                        Success = false,
                        Status = CommandResultType.AlreadyExists
                    };
                }
            }
            else {
                result = CommandResultArgs.InsufficientPermissions;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public CommandResultArgs UnregisterTextCommand(Command command, Dictionary<String, CommandParameter> parameters) {
            CommandResultArgs result = null;

            TextCommandModel textCommand = parameters["textCommand"].First<TextCommandModel>();

            if (this.Security.DispatchPermissionsCheck(command, command.Name).Success == true) {
                TextCommandModel existingRegisteredCommand = this.TextCommands.Find(existingCommand => existingCommand.PluginUid == textCommand.PluginUid && existingCommand.PluginCommand == textCommand.PluginCommand);

                if (existingRegisteredCommand != null) {
                    this.TextCommands.Remove(existingRegisteredCommand);

                    result = new CommandResultArgs() {
                        Success = true,
                        Status = CommandResultType.Success,
                        Scope = {
                            Connections = new List<ConnectionModel>() {
                                this.Connection != null ? this.Connection.ConnectionModel : null
                            }
                        },
                        Now = new CommandData() {
                            TextCommands = new List<TextCommandModel>() {
                                textCommand
                            }
                        }
                    };

                    if (this.Events != null) {
                        this.Events.Log(GenericEventArgs.ConvertToGenericEvent(result, GenericEventType.TextCommandUnregistered));
                    }
                }
                else {
                    result = new CommandResultArgs() {
                        Success = false,
                        Status = CommandResultType.DoesNotExists
                    };
                }
            }
            else {
                result = CommandResultArgs.InsufficientPermissions;
            }

            return result;
        }

        /// <summary>
        /// Checks if a prefix is an allowed prefix
        /// </summary>
        /// <param name="prefix">The prefix to check (e.g !, @ etc.)</param>
        /// <returns>The parameter prefix, or null if the prefix is invalid</returns>
        public String GetValidTextCommandPrefix(String prefix) {
            String result = null;

            if (prefix == this.Variables.Get<String>(CommonVariableNames.TextCommandPublicPrefix) ||
                prefix == this.Variables.Get<String>(CommonVariableNames.TextCommandProtectedPrefix) ||
                prefix == this.Variables.Get<String>(CommonVariableNames.TextCommandPrivatePrefix)) {
                result = prefix;
            }

            return result;
        }
    }
}
