﻿using System;
using System.Collections.Generic;
using Procon.Net.Protocols.Myrcon.Frostbite.Objects;
using Procon.Net.Shared.Actions;
using Procon.Net.Shared.Models;

namespace Procon.Net.Protocols.Myrcon.Frostbite.Battlefield.Battlefield3.Objects {
    [Serializable]
    public class Battlefield3FrostbiteMapList : FrostbiteMapList {

        public override FrostbiteMapList Parse(List<string> words) {

            this.Clear();

            if (words.Count >= 2) {
                int mapsCount = 0, wordsPerMap = 0;

                if (int.TryParse(words[0], out mapsCount) == true && int.TryParse(words[1], out wordsPerMap) == true) {

                    for (int mapOffset = 0, wordIndex = 2; mapOffset < mapsCount && wordIndex < words.Count; mapOffset++, wordIndex += wordsPerMap) {
                        int rounds = 0;
                        if (int.TryParse(words[wordIndex + 2], out rounds) == true) {
                            this.Add(
                                new Map() {
                                    Index = mapOffset,
                                    Rounds = rounds == 0 ? 2 : rounds,
                                    Name = words[wordIndex],
                                    GameMode = new GameMode() {
                                        Name = words[wordIndex + 1]
                                    },
                                    ActionType = NetworkActionType.NetworkMapListed
                                }
                            );
                        }
                    }
                }
            }

            return this;
        }

    }
}
