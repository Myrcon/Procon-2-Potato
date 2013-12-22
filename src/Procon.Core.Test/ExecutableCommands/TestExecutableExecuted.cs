﻿#region

using System;
using System.Collections.Generic;
using NUnit.Framework;
using Procon.Core.Shared;
using Procon.Core.Test.ExecutableCommands.Objects;

#endregion

namespace Procon.Core.Test.ExecutableCommands {
    /// <summary>
    ///     Summary description for ExecutableBase
    /// </summary>
    [TestFixture]
    public class TestExecutableExecuted {
        /// <summary>
        ///     Tests we can match and set a VariableModel in a executable class.
        /// </summary>
        [Test]
        public void TestExecutablePreviewSetPassThrough() {
            var tester = new ExecutableExecutedTester() {
                TestNumber = 0
            };

            tester.Tunnel(new Command() {
                Origin = CommandOrigin.Local,
                CommandType = CommandType.VariablesSet,
                Parameters = TestHelpers.ObjectListToContentList(new List<Object>() {
                    50
                })
            });

            Assert.AreEqual(tester.TestNumber, 50);
            Assert.AreEqual(tester.ExecutedTestValue, 100);
        }
    }
}