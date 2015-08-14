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
using System.Collections.Generic;

namespace Potato.Fuzzy.Tokens.Object {
    public class SelfReflectionThingObjectToken : ThingObjectToken {
        public new static Phrase Parse(IFuzzyState state, Phrase phrase) {
            List<SelfReflectionThingObjectToken> createdTokens = null;
            TokenReflection.CreateDescendants(state, phrase, out createdTokens);

            if (createdTokens != null && createdTokens.Count > 0) {
                foreach (var self in createdTokens) {
                    state.ParseSelfReflectionThing(state, self);
                }
            }

            return phrase;
        }
    }
}