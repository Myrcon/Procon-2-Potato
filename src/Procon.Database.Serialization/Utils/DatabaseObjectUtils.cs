﻿using System.Collections.Generic;
using System.Linq;

namespace Procon.Database.Serialization.Utils {
    /// <summary>
    /// Utilities related to the IDatabaseObject/DatabaseObject
    /// </summary>
    public static class DatabaseObjectUtils {

        /// <summary>
        /// Find all descendants of type, returning them as an enumerable list.
        /// </summary>
        /// <typeparam name="T">The type of IDatabaseObject to find</typeparam>
        /// <param name="self">The object to search for sub values on</param>
        /// <returns></returns>
        public static IEnumerable<T> Descendants<T>(this IDatabaseObject self) where T : IDatabaseObject {
            List<T> items = self.Where(item => item is T).Cast<T>().ToList();

            items.AddRange(self.SelectMany(item => item.Descendants<T>()));

            return items;
        }

        /// <summary>
        /// Find all descendants of type, returning them as an enumerable list. The initial self object will
        /// also be tested and returned if it matches the type
        /// </summary>
        /// <typeparam name="T">The type of IDatabaseObject to find</typeparam>
        /// <param name="self">The object to search for sub values on</param>
        /// <returns></returns>
        public static IEnumerable<T> DescendantsAndSelf<T>(this IDatabaseObject self) where T : IDatabaseObject {
            List<T> items = self.Where(item => item is T).Cast<T>().ToList();

            items.AddRange(self.SelectMany(item => item.Descendants<T>()));
            
            if (self is T) items.Add((T)self);

            return items;
        }
    }
}