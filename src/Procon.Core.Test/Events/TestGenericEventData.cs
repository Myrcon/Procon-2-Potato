﻿#region

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Procon.Core.Shared;
using Procon.Core.Shared.Events;
using Procon.Net.Shared;
using Procon.Net.Shared.Models;

#endregion

namespace Procon.Core.Test.Events {
    [TestFixture]
    public class TestGenericEventData {
        [SetUp]
        public void Initialize() {
            SharedReferences.Setup();
        }

        [Test]
        public void TestGameEventObjectConversion() {
            var gameEventData = new ProtocolEventData() {
                Bans = new List<Ban>() {
                    new Ban() {
                        Now = {
                            Content = new List<String>() {
                                "I should remove this property"
                            }
                        }
                    }
                },
                Chats = new List<Chat>() {
                    new Chat() {
                        Now = {
                            Content = new List<String>() {
                                "I should remove this property"
                            }
                        }
                    }
                },
                Kicks = new List<Kick>() {
                    new Kick() {
                        Scope = {
                            Content = new List<String>() {
                                "I should remove this property"
                            }
                        }
                    }
                },
                Kills = new List<Kill>() {
                    new Kill() {
                        Scope = {
                            Content = new List<String>() {
                                "I should remove this property"
                            }
                        }
                    }
                },
                Players = new List<Player>() {
                    new Player() {
                        Name = "Phogue",
                        Ping = 100
                    }
                },
                Spawns = new List<Spawn>() {
                    new Spawn() {
                        Player = new Player() {
                            Name = "Phogue",
                            Ping = 100
                        }
                    }
                }
            };

            CommandData genericEventData = GenericEventData.Parse(gameEventData);

            Assert.AreEqual("I should remove this property", genericEventData.Bans.First().Now.Content.First());

            Assert.AreEqual("I should remove this property", genericEventData.Chats.First().Now.Content.First());

            Assert.AreEqual("I should remove this property", genericEventData.Kicks.First().Scope.Content.First());

            Assert.AreEqual("I should remove this property", genericEventData.Kills.First().Scope.Content.First());

            Assert.AreEqual("Phogue", genericEventData.Players.First().Name);
            Assert.AreEqual(100, genericEventData.Players.First().Ping);

            Assert.AreEqual("Phogue", genericEventData.Spawns.First().Player.Name);
            Assert.AreEqual(100, genericEventData.Spawns.First().Player.Ping);
        }
    }
}