﻿#region

using System.Collections.Generic;
using NUnit.Framework;
using Procon.Core.Shared;
using Procon.Net.Shared.Actions;
using Procon.Net.Shared.Models;

#endregion

namespace Procon.Core.Test.TextCommands.Fuzzy {
    [TestFixture]
    public class TestFuzzyBasicObjectMatching : TestFuzzyBase {
        [Test]
        public void TestBasicAlternateKickPhogueCommandSevereTypo() {
            CommandResultArgs result = CreateTextCommandController().ExecuteTextCommand(new Command() {
                Username = "Phogue",
                Origin = CommandOrigin.Local
            }, new Dictionary<string, CommandParameter>() {
                {"text", new CommandParameter() {
                    Data = {
                        Content = new List<string>() {
                            "getch rids of phogue"
                        }
                    }
                }}
            });

            // No event should be fired. The command has a much lower tolerance for typos (good thing)
            Assert.IsNull(result, "Argument has passed, but should have failed due to severe typo in command");
        }

        [Test]
        public void TestBasicAlternateKickPhogueCommandSmallTypo() {
            AssertCommandPlayerListMapList(CreateTextCommandController(), "get rdi of phogue", TextCommandKick, new List<Player>() {
                PlayerPhogue
            }, new List<Map>());
        }

        [Test]
        public void TestBasicKickDiacritic() {
            AssertCommandPlayerListMapList(CreateTextCommandController(), "kick MrDiacritic", TextCommandKick, new List<Player>() {
                PlayerMrDiacritic
            }, new List<Map>());
        }

        [Test]
        public void TestBasicKickPhogue() {
            AssertCommandPlayerListMapList(CreateTextCommandController(), "kick phogue", TextCommandKick, new List<Player>() {
                PlayerPhogue
            }, new List<Map>());
        }

        [Test]
        public void TestBasicKickPhogueCommandSevereTypo() {
            CommandResultArgs result = CreateTextCommandController().ExecuteTextCommand(new Command() {
                Username = "Phogue",
                Origin = CommandOrigin.Local
            }, new Dictionary<string, CommandParameter>() {
                {"text", new CommandParameter() {
                    Data = {
                        Content = new List<string>() {
                            "kik phogue"
                        }
                    }
                }}
            });

            // No event should be fired. The command has a much lower tolerance for typos (good thing)
            Assert.IsNull(result, "Argument has passed, but should have failed due to severe typo in command");
        }

        [Test]
        public void TestBasicKickPhogueCommandSmallTypo() {
            CommandResultArgs result = CreateTextCommandController().ExecuteTextCommand(new Command() {
                Username = "Phogue",
                Origin = CommandOrigin.Local
            }, new Dictionary<string, CommandParameter>() {
                {"text", new CommandParameter() {
                    Data = {
                        Content = new List<string>() {
                            "kcik phogue"
                        }
                    }
                }}
            });

            // No event should be fired. The command has a much lower tolerance for typos (good thing)
            Assert.IsNull(result, "Argument has passed, but should have failed due to severe typo in command");
        }

        [Test]
        public void TestBasicKickPhogueNameTypo() {
            AssertCommandPlayerListMapList(CreateTextCommandController(), "kick phouge", TextCommandKick, new List<Player>() {
                PlayerPhogue
            }, new List<Map>());
        }

        [Test]
        public void TestBasicKickSelf() {
            AssertCommandPlayerListMapList(CreateTextCommandController(), "kick me", TextCommandKick, new List<Player>() {
                PlayerPhogue
            }, new List<Map>());
        }

        [Test]
        public void TestKickPhogueButNotPhogueIsAButterflyWithHighSimilarity() {
            AssertCommandPlayerListMapList(CreateTextCommandController(), "kick phogue is perhaps not a butterfly", TextCommandKick, new List<Player>() {
                PlayerPhogue
            }, new List<Map>());
        }

        /// <summary>
        ///     Kicks Phogue and morpheus using a comma to seperate the two items.
        /// </summary>
        [Test]
        public void TestKickPhogueCommaMorpheus() {
            AssertCommandPlayerListMapList(CreateTextCommandController(), "kick phogue, morpheus(aut)", TextCommandKick, new List<Player>() {
                PlayerPhogue,
                PlayerMorpheus
            }, new List<Map>());
        }

        [Test]
        public void TestKickPhogueIsAButterfly() {
            AssertCommandPlayerListMapList(CreateTextCommandController(), "kick phogue is a butterfly", TextCommandKick, new List<Player>() {
                PlayerPhogueIsAButterfly
            }, new List<Map>());
        }

        [Test]
        public void TestKickPhogueIsAButterflySmallTypo() {
            AssertCommandPlayerListMapList(CreateTextCommandController(), "kick phogue is a buttrfly", TextCommandKick, new List<Player>() {
                PlayerPhogueIsAButterfly
            }, new List<Map>());
        }

        [Test]
        public void TestKickPhogueMorpheusSevereTypo() {
            AssertCommandPlayerListMapList(CreateTextCommandController(), "kick phage, marphius aut", TextCommandKick, new List<Player>() {
                PlayerPhogue,
                PlayerMorpheus
            }, new List<Map>());
        }

        [Test]
        public void TestKickPhogueMorpheusTruncated() {
            AssertCommandPlayerListMapList(CreateTextCommandController(), "kick pho, morph", TextCommandKick, new List<Player>() {
                PlayerPhogue,
                PlayerMorpheus
            }, new List<Map>());
        }

        [Test]
        public void TestKickSplitNameDoubleSubsetMatch() {
            AssertCommandPlayerListMapList(CreateTextCommandController(), "kick say nish", TextCommandKick, new List<Player>() {
                PlayerSayaNishino
            }, new List<Map>());
        }
    }
}