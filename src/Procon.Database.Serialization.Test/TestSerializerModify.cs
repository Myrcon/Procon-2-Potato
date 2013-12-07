﻿using Procon.Database.Serialization.Builders;
using Procon.Database.Serialization.Builders.Equalities;
using Procon.Database.Serialization.Builders.Methods;
using Procon.Database.Serialization.Builders.Statements;
using Procon.Database.Serialization.Builders.Values;

namespace Procon.Database.Serialization.Test {
    public abstract class TestSerializerModify {

        #region TestModifyPlayerSetName

        protected IDatabaseObject TestModifyPlayerSetNameExplicit = new Modify()
            .Collection(new Collection() {
                Name = "Player"
            })
            .Assignment(new Assignment() {
                new Field() {
                    Name = "Name"
                },
                new StringValue() {
                    Data = "Phogue"
                }
            });

        protected IDatabaseObject TestModifyPlayerSetNameImplicit = new Modify()
            .Collection("Player")
            .Assignment("Name", "Phogue");

        public abstract void TestModifyPlayerSetName();

        #endregion

        #region TestModifyIntoPlayerSetNameScore

        protected IDatabaseObject TestModifyPlayerSetNameScoreExplicit = new Modify()
            .Collection(new Collection() {
                Name = "Player"
            })
            .Assignment(new Assignment() {
                new Field() {
                    Name = "Name"
                },
                new StringValue() {
                    Data = "Phogue"
                }
            })
            .Assignment(new Assignment() {
                new Field() {
                    Name = "Score"
                },
                new NumericValue() {
                    Integer = 50
                }
            });

        protected IDatabaseObject TestModifyPlayerSetNameScoreImplicit = new Modify()
            .Collection("Player")
            .Assignment("Name", "Phogue")
            .Assignment("Score", 50);

        public abstract void TestModifyPlayerSetNameScore();

        #endregion

        #region TestModifyPlayerSetScoreWhereNameEqualsPhogue

        protected IDatabaseObject TestModifyPlayerSetScoreWhereNameEqualsPhogueExplicit = new Modify()
            .Collection(new Collection() {
                Name = "Player"
            })
            .Assignment(new Assignment() {
                new Field() {
                    Name = "Score"
                },
                new NumericValue() {
                    Integer = 50
                }
            })
            .Condition(new Equals() {
                new Field() {
                    Name = "Name"
                },
                new StringValue() {
                    Data = "Phogue"
                }
            });

        protected IDatabaseObject TestModifyPlayerSetScoreWhereNameEqualsPhogueImplicit = new Modify()
            .Collection("Player")
            .Assignment("Score", 50)
            .Condition("Name", "Phogue");

        public abstract void TestModifyPlayerSetScoreWhereNameEqualsPhogue();

        #endregion


        #region TestModifyPlayerSetScoreWhereNameEqualsPhogueAndRankAbove10

        protected IDatabaseObject TestModifyPlayerSetScoreWhereNameEqualsPhogueAndRankAbove10Explicit = new Modify()
            .Collection(new Collection() {
                Name = "Player"
            })
            .Assignment(new Assignment() {
                new Field() {
                    Name = "Score"
                },
                new NumericValue() {
                    Integer = 50
                }
            })
            .Condition(new Equals() {
                new Field() {
                    Name = "Name"
                },
                new StringValue() {
                    Data = "Phogue"
                }
            })
            .Condition(new GreaterThan() {
                new Field() {
                    Name = "Rank"
                },
                new NumericValue() {
                    Integer = 10
                }
            });

        protected IDatabaseObject TestModifyPlayerSetScoreWhereNameEqualsPhogueAndRankAbove10Implicit = new Modify()
            .Collection("Player")
            .Assignment("Score", 50)
            .Condition("Name", "Phogue")
            .Condition("Rank", new GreaterThan(), 10);

        public abstract void TestModifyPlayerSetScoreWhereNameEqualsPhogueAndRankAbove10();

        #endregion
    }
}