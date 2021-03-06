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
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Potato.Core.Shared.Serialization;

namespace Potato.Core.Shared {
    /// <summary>
    /// Config for saving JSON data
    /// </summary>
    public class Config : IConfig {
        public JObject Document { get; set; }

        public JArray Root { get; set; }

        /// <summary>
        /// Initializes the serializer
        /// </summary>
        public Config() {
            this.Document = new JObject();
        }

        public IConfig Append<T>(T data) {
            if (Equals(data, default(T))) throw new ArgumentNullException("data");

            this.Root.Add(JObject.FromObject(data, JsonSerialization.Readable));

            return this;
        }

        public JArray RootOf<T>() {
            return this.RootOf(typeof(T));
        }

        public JArray RootOf(Type type) {
            if (type == null) throw new ArgumentNullException("type");

            return this.RootOf(String.Format("{0}.{1}", type.Namespace, type.Name));
        }

        public JArray RootOf(string @namespace) {
            if (@namespace == null) throw new ArgumentNullException("namespace");

            return this.Document[@namespace] != null ? this.Document[@namespace].Value<JArray>() : new JArray();
        }

        public IConfig Union(IConfig config) {
            if (config == null) throw new ArgumentNullException("config");

            foreach (var property in config.Document.Children<JProperty>()) {
                if (this.Document[property.Name] == null) {
                    this.Document.Add(property);
                }
                else {
                    this.Document[property.Name].Value<JArray>().Add(property.Values());
                }
            }

            return this;
        }

        public IConfig Load(DirectoryInfo directory) {
            if (directory == null) throw new ArgumentNullException("directory");

            directory.Refresh();

            if (directory.Exists == true) {
                foreach (FileInfo file in directory.GetFiles("*.json").OrderBy(file => file.Name)) {
                    if (this.Root == null) {
                        this.Load(file);
                    }
                    else {
                        this.Union(new Config().Load(file));
                    }
                }
            }

            return this;
        }

        public IConfig Load(FileInfo file) {
            if (file == null) throw new ArgumentNullException("file");

            file.Refresh();

            if (file.Exists == true) {
                using (var stream = new StreamReader(file.FullName)) {
                    using (JsonReader reader = new JsonTextReader(stream)) {
                        try {
                            this.Document = JObject.Load(reader);

                            String name = file.Name.Replace(".json", "");

                            this.Root = this.Document[name] != null ? this.Document.Value<JArray>(name) : this.Document.First.First.Value<JArray>();
                        }
                        catch {
                            this.Root = new JArray();
                            this.Document = new JObject() {
                                new JProperty("Empty", this.Root)
                            };
                        }
                    }
                }
            }

            return this;
        }

        public IConfig Create<T>() {
            return this.Create(typeof(T));
        }

        public IConfig Create(Type type) {
            // Clear out all old data
            this.Root = new JArray();
            this.Document = new JObject() {
                new JProperty(String.Format("{0}.{1}", type.Namespace, type.Name), this.Root)
            };

            return this;
        }

        public IConfig Save(FileInfo file) {
            if (file == null) throw new ArgumentNullException("file");

            using (TextWriter writer = new StreamWriter(file.FullName, false)) {
                JsonSerialization.Readable.Serialize(writer, this.Document);
            }

            return this;
        }
    }
}
