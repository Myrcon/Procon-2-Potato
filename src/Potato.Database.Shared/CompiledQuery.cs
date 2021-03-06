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
using System.Collections.Generic;

namespace Potato.Database.Shared {
    /// <summary>
    /// A compiled version of the query with as basic information 
    /// as we can boil the complex query down to
    /// </summary>
    [Serializable]
    public class CompiledQuery : QueryData, ICompiledQuery {
        /// <summary>
        /// List of compiled child queries.
        /// </summary>
        public List<ICompiledQuery> Children { get; set; }

        /// <summary>
        /// The combined and compield version of the query
        /// </summary>
        public List<String> Compiled { get; set; }

        /// <summary>
        /// Initializes all defaults
        /// </summary>
        public CompiledQuery() : base() {
            this.Children = new List<ICompiledQuery>();
            this.Compiled = new List<String>();
        }
    }
}
