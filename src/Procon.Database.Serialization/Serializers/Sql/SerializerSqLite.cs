﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Procon.Database.Serialization.Builders;
using Procon.Database.Serialization.Builders.Equalities;
using Procon.Database.Serialization.Builders.FieldTypes;
using Procon.Database.Serialization.Builders.Logicals;
using Procon.Database.Serialization.Builders.Methods;
using Procon.Database.Serialization.Builders.Modifiers;
using Procon.Database.Serialization.Builders.Statements;
using Procon.Database.Serialization.Builders.Values;

namespace Procon.Database.Serialization.Serializers.Sql {
    /// <summary>
    /// Serializer for SqLite support.
    /// </summary>
    public class SerializerSqLite : SerializerSql {

        /// <summary>
        /// Parses an index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected virtual String ParseIndex(Index index) {
            String parsed = String.Empty;

            // PRIMARY KEY (`Name`)
            if (index.Any(attribute => attribute is Primary)) {
                parsed = String.Format("PRIMARY KEY ({0})", String.Join(", ", this.ParseSortings(index)));
            }

            return parsed;
        }

        /// <summary>
        /// Parses the list of indexes 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual List<String> ParseIndices(IDatabaseObject query) {
            return query.Where(statement => statement is Index).Union(new List<IDatabaseObject>() { query }.Where(owner => owner is Index)).Select(index => this.ParseIndex(index as Index)).Where(index => String.IsNullOrEmpty(index) == false).ToList();
        }

        /// <summary>
        /// Formats the database name.
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        protected virtual String ParseDatabase(Builders.Database database) {
            return String.Format("`{0}`", database.Name);
        }

        /// <summary>
        /// Parses the list of databases.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual List<String> ParseDatabases(IDatabaseObject query) {
            return query.Where(statement => statement is Builders.Database).Select(database => this.ParseDatabase(database as Builders.Database)).ToList();
        }

        /// <summary>
        /// Fetches the method from the type of query 
        /// </summary>
        /// <param name="method"></param>
        protected virtual List<string> ParseMethod(Method method) {
            List<string> parsed = new List<string>();

            if (method is Find) {
                parsed.Add("SELECT");
            }
            else if (method is Save) {
                parsed.Add("INSERT");
            }
            else if (method is Merge) {
                parsed.Add("REPLACE");
            }
            else if (method is Create) {
                parsed.Add("CREATE");
            }
            else if (method is Modify) {
                parsed.Add("UPDATE");
            }
            else if (method is Remove) {
                parsed.Add("DELETE");
            }
            else if (method is Drop) {
                parsed.Add("DROP");
            }
            else if (method is Index) {
                parsed.Add("CREATE");
            }

            return parsed;
        }

        protected virtual String ParseCollection(Collection collection) {
            return String.Format("`{0}`", collection.Name);
        }

        protected virtual String ParseSort(Sort sort) {
            Collection collection = sort.FirstOrDefault(statement => statement is Collection) as Collection;

            List<String> parsed = new List<String> {
                collection == null ? String.Format("`{0}`", sort.Name) : String.Format("`{0}`.`{1}`", collection.Name, sort.Name)
            };

            if (sort.Any(attribute => attribute is Descending)) {
                parsed.Add("DESC");
            }

            return String.Join(" ", parsed);
        }

        protected virtual String ParseType(Builders.FieldType type) {
            List<String> parsed = new List<String>();

            Length length = type.FirstOrDefault(attribute => attribute is Length) as Length;

            if (type is StringType) {
                parsed.Add(String.Format("VARCHAR({0})", length == null ? 255 : length.Value));
            }
            else if (type is IntegerType) {
                parsed.Add("INTEGER");
            }

            if (type.Any(attribute => attribute is Builders.Modifiers.Nullable) == false) {
                parsed.Add("NOT NULL");
            }

            if (type.Any(attribute => attribute is AutoIncrement) == true) {
                parsed.Add("AUTO INCREMENT");
            }

            return String.Join(" ", parsed);
        }

        protected virtual String ParseField(Field field) {
            List<String> parsed = new List<String>();

            if (field.Any(attribute => attribute is Distinct)) {
                parsed.Add("DISTINCT");
            }

            Collection collection = field.FirstOrDefault(statement => statement is Collection) as Collection;

            parsed.Add(collection == null ? String.Format("`{0}`", field.Name) : String.Format("`{0}`.`{1}`", collection.Name, field.Name));

            if (field.Any(attribute => attribute is Builders.FieldType)) {
                parsed.Add(this.ParseType(field.First(attribute => attribute is Builders.FieldType) as Builders.FieldType));
            }

            return String.Join(" ", parsed);
        }

        protected virtual String ParseEquality(Equality equality) {
            String parsed = "";

            if (equality is Equals) {
                parsed = "=";
            }
            else if (equality is GreaterThan) {
                parsed = ">";
            }
            else if (equality is GreaterThanEquals) {
                parsed = ">=";
            }
            else if (equality is LessThan) {
                parsed = "<";
            }
            else if (equality is LessThanEquals) {
                parsed = "<=";
            }

            return parsed;
        }

        protected virtual String ParseValue(Value value) {
            String parsed = "";

            NumericValue numeric = value as NumericValue;
            StringValue @string = value as StringValue;
            RawValue raw = value as RawValue;

            if (numeric != null) {
                if (numeric.Long.HasValue == true) {
                    parsed = numeric.Long.Value.ToString(CultureInfo.InvariantCulture);
                }
                else if (numeric.Float.HasValue == true) {
                    parsed = numeric.Float.Value.ToString(CultureInfo.InvariantCulture);
                }
            }
            else if (@string != null) {
                // Note that the string should have been parsed by the driver to escape it.
                parsed = String.Format(@"""{0}""", @string.Data);
            }
            else if (raw != null) {
                parsed = raw.Data;
            }

            return parsed;
        }

        protected virtual List<String> ParseValues(IDatabaseObject query) {
            return query.Where(statement => statement is Value).Select(value => this.ParseValue(value as Value)).ToList();
        }

        protected virtual List<String> ParseFields(IDatabaseObject query) {
            List<String> parsed = new List<String>();

            // If we have a distinct field, but no specific fields
            if (query.Any(attribute => attribute is Distinct) == true && query.Any(logical => logical is Field) == false) {
                parsed.Add("DISTINCT *");
            }
            // If we have a distinct field and only one field available
            else if (query.Any(attribute => attribute is Distinct) == true && query.Count(logical => logical is Field) == 1) {
                Field field = query.First(logical => logical is Field) as Field;

                if (field != null) {
                    field.Add(query.First(attribute => attribute is Distinct));

                    parsed.Add(this.ParseField(field));
                }
            }
            // Else, no distinct in the global query space
            else {
                parsed.AddRange(query.Where(logical => logical is Field).Select(field => this.ParseField(field as Field)));
            }

            return parsed;
        }

        /// <summary>
        /// Parse collections for all methods, used like "FROM ..."
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual List<String> ParseCollections(IDatabaseObject query) {
            return query.Where(logical => logical is Collection).Select(collection => this.ParseCollection(collection as Collection)).ToList();
        }

        protected virtual List<String> ParseEqualities(IDatabaseObject query) {
            List<String> equalities = new List<String>();

            foreach (Equality equality in query.Where(statement => statement is Equality)) {
                Field field = equality.FirstOrDefault(statement => statement is Field) as Field;
                Value value = equality.FirstOrDefault(statement => statement is Value) as Value;

                if (field != null && value != null) {
                    equalities.Add(String.Format("{0} {1} {2}", this.ParseField(field), this.ParseEquality(equality), this.ParseValue(value)));
                }
            }

            return equalities;
        }

        /// <summary>
        /// Parse any logicals 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual List<String> ParseLogicals(IDatabaseObject query) {
            List<String> logicals = new List<String>();

            foreach (Logical logical in query.Where(logical => logical is Logical)) {
                String separator = logical is Or ? " OR " : " AND ";

                String childLogicals = String.Join(separator, this.ParseLogicals(logical));
                String childEqualities = String.Join(separator, this.ParseEqualities(logical));

                if (String.IsNullOrEmpty(childLogicals) == false) logicals.Add(String.Format("({0})", childLogicals));
                if (String.IsNullOrEmpty(childEqualities) == false) logicals.Add(String.Format("({0})", childEqualities));
            }

            return logicals;
        }

        /// <summary>
        /// Parse the conditions of a SELECT, UPDATE and DELETE query. Used like "SELECT ... "
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual List<String> ParseConditions(IDatabaseObject query) {
            List<String> conditions = this.ParseLogicals(query);

            conditions.AddRange(this.ParseEqualities(query));

            return conditions;
        }

        /// <summary>
        /// Parse field assignments, similar to conditions, but without the conditionals.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual List<String> ParseAssignments(IDatabaseObject query) {
            List<String> assignments = new List<String>();

            foreach (Assignment assignment in query.Where(statement => statement is Assignment)) {
                Field field = assignment.FirstOrDefault(statement => statement is Field) as Field;
                Value value = assignment.FirstOrDefault(statement => statement is Value) as Value;

                if (field != null && value != null) {
                    assignments.Add(String.Format("{0} = {1}", this.ParseField(field), this.ParseValue(value)));
                }
            }

            return assignments;
        }

        protected virtual List<String> ParseSortings(IDatabaseObject query) {
            return query.Where(sort => sort is Sort).Select(sort => this.ParseSort(sort as Sort)).ToList();
        }

        public override ICompiledQuery Compile(IParsedQuery parsed) {
            CompiledQuery serializedQuery = new CompiledQuery() {
                Children = parsed.Children.Select(this.Compile).Where(child => child != null).ToList(),
                Root = parsed.Root,
                Methods = parsed.Methods,
                Skip = parsed.Skip,
                Limit = parsed.Limit
            };

            List<String> compiled = new List<String>() {
                parsed.Methods.FirstOrDefault()
            };

            if (parsed.Root is Merge) {
                ICompiledQuery save = serializedQuery.Children.FirstOrDefault(child => child.Root is Save);
                ICompiledQuery modify = serializedQuery.Children.FirstOrDefault(child => child.Root is Modify);

                if (save != null && modify != null) {
                    compiled.Add("INTO");
                    compiled.Add(save.Collections.FirstOrDefault());
                    compiled.Add("SET");
                    compiled.Add(save.Assignments.FirstOrDefault());
                    //compiled.Add("ON DUPLICATE KEY UPDATE");
                    //compiled.Add(modify.Assignments.FirstOrDefault());
                }
            }
            else if (parsed.Root is Index) {
                Primary primary = parsed.Root.FirstOrDefault(attribute => attribute is Primary) as Primary;
                Unique unique = parsed.Root.FirstOrDefault(attribute => attribute is Unique) as Unique;

                if (primary == null) {
                    // UNIQUE INDEX `Score_UNIQUE` (`Score` ASC)
                    if (unique != null) {
                        compiled.Add("UNIQUE INDEX IF NOT EXISTS");

                        // todo move the name element to a modifier?
                        compiled.Add(String.Format("`{0}`", ((Index)parsed.Root).Name));
                    }
                    // INDEX `Name_INDEX` (`Name` ASC)
                    else {
                        compiled.Add("INDEX IF NOT EXISTS");

                        // todo move the name element to a modifier?
                        compiled.Add(String.Format("`{0}`", ((Index)parsed.Root).Name));
                    }

                    compiled.Add("ON");

                    if (parsed.Collections.Any() == true) {
                        serializedQuery.Collections.Add(String.Join(", ", parsed.Collections));
                        compiled.Add(serializedQuery.Collections.FirstOrDefault());
                    }

                    if (parsed.Sortings.Any() == true) {
                        serializedQuery.Sortings.Add(String.Join(", ", parsed.Sortings));
                        compiled.Add(String.Format("({0})", serializedQuery.Sortings.FirstOrDefault()));
                    }
                }
                else {
                    // SQLite does not support adding primary indexes after a table has been created.
                    serializedQuery = null;
                }
            }
            else if (parsed.Root is Find) {
                serializedQuery.Fields = new List<String>(parsed.Fields);
                compiled.Add(parsed.Fields.Any() == true ? String.Join(", ", parsed.Fields) : "*");

                if (parsed.Collections.Any() == true) {
                    serializedQuery.Collections.Add(String.Join(", ", parsed.Collections));
                    compiled.Add("FROM");
                    compiled.Add(serializedQuery.Collections.FirstOrDefault());
                }

                if (parsed.Conditions.Any() == true) {
                    serializedQuery.Conditions.Add(String.Join(" AND ", parsed.Conditions));
                    compiled.Add("WHERE");
                    compiled.Add(serializedQuery.Conditions.FirstOrDefault());
                }

                if (parsed.Sortings.Any() == true) {
                    serializedQuery.Sortings.Add(String.Join(", ", parsed.Sortings));
                    compiled.Add("ORDER BY");
                    compiled.Add(serializedQuery.Sortings.FirstOrDefault());
                }

                if (parsed.Limit != null) {
                    compiled.Add("LIMIT");
                    compiled.Add(parsed.Limit.Value.ToString(CultureInfo.InvariantCulture));
                }

                if (parsed.Skip != null) {
                    compiled.Add("OFFSET");
                    compiled.Add(parsed.Skip.Value.ToString(CultureInfo.InvariantCulture));
                }
            }
            else if (parsed.Root is Create) {
                if (parsed.Databases.Any() == true) {
                    serializedQuery.Databases.Add(parsed.Databases.FirstOrDefault());
                    compiled = new List<String> {
                        "ATTACH",
                        "DATABASE",
                        serializedQuery.Databases.FirstOrDefault()
                    };
                }
                else if (parsed.Collections.Any() == true) {
                    compiled.Add("TABLE");
                    compiled.Add(parsed.Collections.FirstOrDefault());

                    if (parsed.Indices.Any() == true) {
                        List<String> fieldsIndicesCombination = new List<String>(parsed.Fields);
                        fieldsIndicesCombination.AddRange(parsed.Indices);

                        serializedQuery.Indices.Add(String.Join(", ", fieldsIndicesCombination.ToArray()));

                        compiled.Add(String.Format("({0})", serializedQuery.Indices.FirstOrDefault()));
                    }
                    else {
                        compiled.Add(String.Format("({0})", String.Join(", ", parsed.Fields.ToArray())));
                    }
                }
            }
            else if (parsed.Root is Save) {
                if (parsed.Collections.Any() == true) {
                    compiled.Add("INTO");
                    serializedQuery.Collections.Add(parsed.Collections.FirstOrDefault());
                    compiled.Add(serializedQuery.Collections.FirstOrDefault());
                }

                if (parsed.Fields.Any() == true) {
                    serializedQuery.Fields.Add(String.Join(", ", parsed.Fields));
                    compiled.Add(String.Format("({0})", serializedQuery.Fields.FirstOrDefault()));
                }

                compiled.Add("VALUES");

                if (parsed.Values.Any() == true) {
                    serializedQuery.Values.Add(String.Join(", ", parsed.Values));
                    compiled.Add(String.Format("({0})", serializedQuery.Values.FirstOrDefault()));
                }

                if (parsed.Assignments.Any() == true) {
                    serializedQuery.Assignments.Add(String.Join(", ", parsed.Assignments));
                }
            }
            else if (parsed.Root is Modify) {
                if (parsed.Collections.Any() == true) {
                    serializedQuery.Collections.Add(String.Join(", ", parsed.Collections));
                    compiled.Add(serializedQuery.Collections.FirstOrDefault());
                }

                if (parsed.Assignments.Any() == true) {
                    serializedQuery.Assignments.Add(String.Join(", ", parsed.Assignments));
                    compiled.Add("SET");
                    compiled.Add(serializedQuery.Assignments.FirstOrDefault());
                }

                if (parsed.Conditions.Any() == true) {
                    serializedQuery.Conditions.Add(String.Join(" AND ", parsed.Conditions));
                    compiled.Add("WHERE");
                    compiled.Add(serializedQuery.Conditions.FirstOrDefault());
                }
            }
            else if (parsed.Root is Remove) {
                if (parsed.Collections.Any() == true) {
                    serializedQuery.Collections.Add(String.Join(", ", parsed.Collections));
                    compiled.Add("FROM");
                    compiled.Add(serializedQuery.Collections.FirstOrDefault());
                }

                if (parsed.Conditions.Any() == true) {
                    serializedQuery.Conditions.Add(String.Join(" AND ", parsed.Conditions));
                    compiled.Add("WHERE");
                    compiled.Add(serializedQuery.Conditions.FirstOrDefault());
                }
            }
            else if (parsed.Root is Drop) {
                if (parsed.Databases.Any() == true) {
                    serializedQuery.Databases.Add(parsed.Databases.FirstOrDefault());
                    compiled = new List<String> {
                        "DETACH",
                        "DATABASE",
                        serializedQuery.Databases.FirstOrDefault()
                    };
                }
                else if (parsed.Collections.Any() == true) {
                    compiled.Add("TABLE");
                    serializedQuery.Collections.Add(parsed.Collections.FirstOrDefault());
                    compiled.Add(serializedQuery.Collections.FirstOrDefault());
                }
            }
            
            if (serializedQuery != null) serializedQuery.Compiled.Add(String.Join(" ", compiled));

            return serializedQuery;
        }

        public override ISerializer Parse(Method method, IParsedQuery parsed) {
            parsed.Root = method;

            parsed.Children = this.ParseChildren(method);

            parsed.Methods = this.ParseMethod(method);

            parsed.Skip = this.ParseSkip(method);

            parsed.Limit = this.ParseLimit(method);

            parsed.Databases = this.ParseDatabases(method);

            parsed.Indices = this.ParseIndices(method);

            parsed.Fields = this.ParseFields(method);
            parsed.Fields.AddRange(method.Where(statement => statement is Assignment).SelectMany(this.ParseFields));

            parsed.Values = method.Where(statement => statement is Assignment).SelectMany(this.ParseValues).ToList();

            parsed.Assignments = this.ParseAssignments(method);

            parsed.Conditions = this.ParseConditions(method);

            parsed.Collections = this.ParseCollections(method);

            parsed.Sortings = this.ParseSortings(method);

            return this;
        }
    }
}
