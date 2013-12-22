﻿using System;
using System.Collections.Generic;
using Procon.Core.Shared;

namespace TestPlugin.Tests {
    public class TestPluginsCommands : ExecutableBase {

        public TestPluginsCommands() : base() {
            this.AppendDispatchHandlers(new Dictionary<CommandAttribute, CommandDispatchHandler>() {
                {
                    new CommandAttribute() {
                        Name = "TestPluginsCommandsZeroParameters"
                    },
                    new CommandDispatchHandler(this.TestPluginsCommandsZeroParameters)
                }
            });
        }

        protected CommandResultArgs TestPluginsCommandsZeroParameters(Command command, Dictionary<String, CommandParameter> parameters) {
            command.Result.Message = command.Name;
            command.Result.Status = CommandResultType.Success;

            return command.Result;
        }
    }
}
