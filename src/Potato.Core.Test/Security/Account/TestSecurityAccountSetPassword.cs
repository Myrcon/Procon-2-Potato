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
using NUnit.Framework;
using Potato.Core.Security;
using Potato.Core.Shared;
using Potato.Core.Shared.Models;
using Potato.Net.Shared.Utils;

namespace Potato.Core.Test.Security.Account {
    [TestFixture]
    public class TestSecurityAccountSetPassword {
        [SetUp]
        public void Initialize() {
            SharedReferences.Setup();
        }

        /// <summary>
        ///     Test that setting a new password can be authenticated against
        /// </summary>
        [Test]
        public void TestSuccess() {
            String generatedPassword = StringExtensions.RandomString(10);

            var security = new SecurityController();
            security.Tunnel(CommandBuilder.SecurityAddGroup("GroupName").SetOrigin(CommandOrigin.Local));
            security.Tunnel(CommandBuilder.SecurityGroupAddAccount("GroupName", "Phogue").SetOrigin(CommandOrigin.Local));

            // Now change the password of the account.
            security.Tunnel(CommandBuilder.SecurityAccountSetPassword("Phogue", generatedPassword).SetOrigin(CommandOrigin.Local));

            // Now validate that we can authenticate against the newly set password.
            ICommandResult result = security.Tunnel(CommandBuilder.SecurityAccountAuthenticate("Phogue", generatedPassword, String.Empty).SetOrigin(CommandOrigin.Local));

            // Validate that we could authenticate with our new password.
            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.CommandResultType, CommandResultType.Success);
        }

        /// <summary>
        ///     Tests we get an empty command result back if the account we try to set a password on
        ///     does not exist.
        /// </summary>
        [Test]
        public void TestAccountDoesNotExist() {
            var security = new SecurityController();
            security.Tunnel(CommandBuilder.SecurityAddGroup("GroupName").SetOrigin(CommandOrigin.Local));
            security.Tunnel(CommandBuilder.SecurityGroupAddAccount("GroupName", "ThisExists").SetOrigin(CommandOrigin.Local));

            // Now change the password of the account.
            ICommandResult result = security.Tunnel(CommandBuilder.SecurityAccountSetPassword("DoesNotExist", "password").SetOrigin(CommandOrigin.Local));

            // Validate that we could not set a password and the result returned false.
            Assert.IsFalse(result.Success);
            Assert.AreEqual(CommandResultType.DoesNotExists, result.CommandResultType);
        }

        /// <summary>
        ///     Tests that we cannot set the password of an account if we do not have permission to do so.
        ///     Thinking about this, we may need to write this to allow users to set their own passwords. This should be
        ///     done within the SecurityController to determine if a CommandName can edit it's own account details.
        /// </summary>
        [Test]
        public void TestAccountInsufficientPermission() {
            var security = new SecurityController();
            security.Tunnel(CommandBuilder.SecurityAddGroup("GroupName").SetOrigin(CommandOrigin.Local));
            security.Tunnel(CommandBuilder.SecurityGroupAddAccount("GroupName", "Phogue").SetOrigin(CommandOrigin.Local));

            ICommandResult result = security.Tunnel(CommandBuilder.SecurityAccountSetPassword("Phogue", "password").SetOrigin(CommandOrigin.Remote).SetAuthentication(new CommandAuthenticationModel() {
                Username = "Phogue"
            }));

            Assert.IsFalse(result.Success);
            Assert.AreEqual(CommandResultType.InsufficientPermissions, result.CommandResultType);
        }

        /// <summary>
        ///     Validates that we can't set an empty password.
        /// </summary>
        [Test]
        public void TestEmptyPassword() {
            var security = new SecurityController();
            security.Tunnel(CommandBuilder.SecurityAddGroup("GroupName").SetOrigin(CommandOrigin.Local));
            security.Tunnel(CommandBuilder.SecurityGroupAddAccount("GroupName", "Phogue").SetOrigin(CommandOrigin.Local));

            // Now change the password of the account.
            ICommandResult result = security.Tunnel(CommandBuilder.SecurityAccountSetPassword("Phogue", String.Empty).SetOrigin(CommandOrigin.Local));

            // Validate that we could not set a password and the result returned false.
            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.CommandResultType, CommandResultType.InvalidParameter);
        }
    }
}
