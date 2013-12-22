using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using Procon.Database.Shared;

namespace Procon.Database.Serialization.Builders.Values {
    /// <summary>
    /// A list of documents
    /// </summary>
    [Serializable]
    public class CollectionValue : Value {

        /// <summary>
        /// Converts each sub-value attached to this object into a JObject,
        /// then returning a JArray of JObject's
        /// </summary>
        /// <returns>An array of the sub documents converted to JObject</returns>
        public JArray ToJArray() {
            JArray array = new JArray();

            foreach (Value value in Enumerable.Where<IDatabaseObject>(this, statement => statement is Value)) {
                DocumentValue document = value as DocumentValue;

                array.Add(document != null ? new JObject(document.ToJObject()) : new JObject(value.ToObject()));
            }

            return array;
        }

        /// <summary>
        /// Converts a JObject to a series of assignment objects, set to this document.
        /// </summary>
        /// <param name="data">The data to convert</param>
        /// <returns>this</returns>
        public IDatabaseObject FromJObject(JArray data) {
            foreach (var item in data) {
                this.Add(new DocumentValue().FromJObject(item as JObject));
            }

            return this;
        }

        public override object ToObject() {
            return Enumerable.Where<IDatabaseObject>(this, statement => statement is Value).Cast<Value>().Select(statement => statement.ToObject()).ToList();
        }
    }
}
