using Procon.Database.Serialization.Builders;

namespace Procon.Database.Serialization.Test {
    public abstract class TestSerializerFind {

        #region TestSelectAllFromPlayer

        protected IQuery TestSelectAllFromPlayerExplicit = new Find()
            .Collection(new Collection() {
                Name = "Player"
            });

        protected IQuery TestSelectAllFromPlayerImplicit = new Find()
            .Collection("Player");

        public abstract void TestSelectAllFromPlayer();

        #endregion

        #region TestSelectDistinctAllFromPlayer

        protected IQuery TestSelectDistinctAllFromPlayerExplicit = new Find()
            .Attribute(new Distinct())
            .Collection(new Collection() {
                Name = "Player"
            });

        protected IQuery TestSelectDistinctAllFromPlayerImplicit = new Find()
            .Attribute(new Distinct())
            .Collection("Player");

        public abstract void TestSelectDistinctAllFromPlayer();

        #endregion

        #region TestSelectAllFromPlayerWhereNameEqualsPhogue

        protected IQuery TestSelectAllFromPlayerWhereNameEqualsPhogueExplicit = new Find()
            .Condition(new Equals() {
                    new Field() {
                        Name = "Name"
                    },
                    new StringValue() {
                        Data = "Phogue"
                    }
                })
            .Collection(new Collection() {
                Name = "Player"
            });

        protected IQuery TestSelectAllFromPlayerWhereNameEqualsPhogueImplicit = new Find()
            .Condition("Name", "Phogue")
            .Collection("Player");

        public abstract void TestSelectAllFromPlayerWhereNameEqualsPhogue();

        #endregion

        #region TestSelectAllFromPlayerWherePlayerNameEqualsPhogue

        protected IQuery TestSelectAllFromPlayerWherePlayerNameEqualsPhogueExplicit = new Find()
            .Condition(new Equals() {
                    new Field() {
                        Name = "Name",
                        Collection = new Collection() {
                            Name = "Player"
                        }
                    },
                    new StringValue() {
                        Data = "Phogue"
                    }
                })
            .Collection(new Collection() {
                Name = "Player"
            });

        protected IQuery TestSelectAllFromPlayerWherePlayerNameEqualsPhogueImplicit = new Find()
            .Condition("Player.Name", "Phogue")
            .Collection("Player");

        public abstract void TestSelectAllFromPlayerWherePlayerNameEqualsPhogue();

        #endregion

        #region TestSelectScoreFromPlayerWhereNameEqualsPhogue

        protected IQuery TestSelectScoreFromPlayerWhereNameEqualsPhogueExplicit = new Find()
            .Condition(new Equals() {
                    new Field() {
                        Name = "Name"
                    },
                    new StringValue() {
                        Data = "Phogue"
                    }
                })
            .Collection(new Collection() {
                Name = "Player"
            })
            .Field(new Field() {
                Name = "Score"
            });

        protected IQuery TestSelectScoreFromPlayerWhereNameEqualsPhogueImplicit = new Find()
            .Condition("Name", "Phogue")
            .Collection("Player")
            .Field("Score");

        public abstract void TestSelectScoreFromPlayerWhereNameEqualsPhogue();

        #endregion

        #region TestSelectScoreRankFromPlayerWhereNameEqualsPhogue

        protected IQuery TestSelectScoreRankFromPlayerWhereNameEqualsPhogueExplicit = new Find()
            .Condition(new Equals() {
                    new Field() {
                        Name = "Name"
                    },
                    new StringValue() {
                        Data = "Phogue"
                    }
                })
            .Collection(new Collection() {
                Name = "Player"
            })
            .Field(new Field() {
                Name = "Score"
            })
            .Field(new Field() {
                Name = "Rank"
            });

        protected IQuery TestSelectScoreRankFromPlayerWhereNameEqualsPhogueImplicit = new Find()
            .Condition("Name", "Phogue")
            .Collection("Player")
            .Field("Score")
            .Field("Rank");

        public abstract void TestSelectScoreRankFromPlayerWhereNameEqualsPhogue();

        #endregion

        #region TestSelectAllFromPlayerWhereNameEqualsPhogueAndScoreEqualsTen

        protected IQuery TestSelectAllFromPlayerWhereNameEqualsPhogueAndScoreEqualsTenExplicit = new Find()
            .Condition(new Equals() {
                    new Field() {
                        Name = "Name"
                    },
                    new StringValue() {
                        Data = "Phogue"
                    }
                })
            .Condition(new Equals() {
                    new Field() {
                        Name = "Score"
                    },
                    new NumericValue() {
                        Integer = 10
                    }
                })
            .Collection(new Collection() {
                Name = "Player"
            });

        protected IQuery TestSelectAllFromPlayerWhereNameEqualsPhogueAndScoreEqualsTenImplicit = new Find()
            .Condition("Name", "Phogue")
            .Condition("Score", 10)
            .Collection("Player");

        public abstract void TestSelectAllFromPlayerWhereNameEqualsPhogueAndScoreEqualsTen();

        #endregion

        #region TestSelectAllFromPlayerWhereNameEqualsPhogueOrZaeed

        protected IQuery TestSelectAllFromPlayerWhereNameEqualsPhogueOrZaeedExplicit = new Find()
            .Condition(new Or() {
                    new Equals() {
                        new Field() {
                            Name = "Name"
                        },
                        new StringValue() {
                            Data = "Phogue"
                        }
                    },
                    new Equals() {
                        new Field() {
                            Name = "Name"
                        },
                        new StringValue() {
                            Data = "Zaeed"
                        }
                    }
                })
            .Collection(new Collection() {
                Name = "Player"
            });

        protected IQuery TestSelectAllFromPlayerWhereNameEqualsPhogueOrZaeedImplicit = new Find()
            .Condition(new Or().Condition("Name", "Phogue").Condition("Name", "Zaeed"))
            .Collection("Player");

        public abstract void TestSelectAllFromPlayerWhereNameEqualsPhogueOrZaeed();

        #endregion

        #region TestSelectAllFromPlayerWhereNameEqualsPhogueOrZaeedAndScoreAbove10AndBelow20

        protected IQuery TestSelectAllFromPlayerWhereNameEqualsPhogueOrZaeedAndScoreAbove10AndBelow20Explicit = new Find()
            .Condition(new Or() {
                    new Equals() {
                        new Field() {
                            Name = "Name"
                        },
                        new StringValue() {
                            Data = "Phogue"
                        }
                    },
                    new Equals() {
                        new Field() {
                            Name = "Name"
                        },
                        new StringValue() {
                            Data = "Zaeed"
                        }
                    }
                })
            .Condition(new GreaterThan() {
                new Field() {
                    Name = "Score"
                },
                new NumericValue() {
                    Integer = 10
                }
            })
            .Condition(new LessThan() {
                new Field() {
                    Name = "Score"
                },
                new NumericValue() {
                    Integer = 20
                }
            })
            .Collection(new Collection() {
                Name = "Player"
            });

        protected IQuery TestSelectAllFromPlayerWhereNameEqualsPhogueOrZaeedAndScoreAbove10AndBelow20Implicit = new Find()
            .Condition(new Or().Condition("Name", "Phogue").Condition("Name", "Zaeed"))
            .Condition("Score", new GreaterThan(), 10)
            .Condition("Score", new LessThan(), 20)
            .Collection("Player");

        public abstract void TestSelectAllFromPlayerWhereNameEqualsPhogueOrZaeedAndScoreAbove10AndBelow20();

        #endregion

        #region TestSelectAllFromPlayerWhereNameEqualsPhogueAndScoreAbove50OrNameEqualsZaeedAndScoreBelow50

        protected IQuery TestSelectAllFromPlayerWhereNameEqualsPhogueAndScoreAbove50OrNameEqualsZaeedAndScoreBelow50Explicit = new Find()
            .Condition(new Or() {
                    new And() {
                        new Equals() {
                            new Field() {
                                Name = "Name"
                            },
                            new StringValue() {
                                Data = "Phogue"
                            }
                        },
                        new GreaterThan() {
                            new Field() {
                                Name = "Score"
                            },
                            new NumericValue() {
                                Integer = 50
                            }
                        }
                    },
                    new And() {
                        new Equals() {
                            new Field() {
                                Name = "Name"
                            },
                            new StringValue() {
                                Data = "Zaeed"
                            }
                        },
                        new LessThan() {
                            new Field() {
                                Name = "Score"
                            },
                            new NumericValue() {
                                Integer = 50
                            }
                        }
                    }
                })
            .Collection(new Collection() {
                Name = "Player"
            });

        protected IQuery TestSelectAllFromPlayerWhereNameEqualsPhogueAndScoreAbove50OrNameEqualsZaeedAndScoreBelow50Implicit = new Find()
        .Condition(new Or()
            .Condition(
                new And().Condition("Name", "Phogue").Condition("Score", new GreaterThan(), 50)
            )
            .Condition(
                new And().Condition("Name", "Zaeed").Condition("Score", new LessThan(), 50)
            )
        ).Collection("Player");

        public abstract void TestSelectAllFromPlayerWhereNameEqualsPhogueAndScoreAbove50OrNameEqualsZaeedAndScoreBelow50();

        #endregion

        #region TestSelectAllFromPlayerSortByScore

        protected IQuery TestSelectAllFromPlayerSortByScoreExplicit = new Find()
            .Collection(new Collection() {
                Name = "Player"
            })
            .Sort(new Sort() {
                Name = "Score"
            });

        protected IQuery TestSelectAllFromPlayerSortByScoreImplicit = new Find()
            .Collection("Player")
            .Sort("Score");

        public abstract void TestSelectAllFromPlayerSortByScore();

        #endregion

        #region TestSelectAllFromPlayerSortByNameThenScoreDescending

        protected IQuery TestSelectAllFromPlayerSortByNameThenScoreDescendingExplicit = new Find()
            .Collection(new Collection() {
                Name = "Player"
            })
            .Sort(new Sort() {
                Name = "Name"
            })
            .Sort(new Sort() {
                Name = "Score"
            }.Attribute(new Descending()));

        protected IQuery TestSelectAllFromPlayerSortByNameThenScoreDescendingImplicit = new Find()
            .Collection("Player")
            .Sort("Name")
            .Sort(new Sort() { Name = "Score" }.Attribute(new Descending()));

        public abstract void TestSelectAllFromPlayerSortByNameThenScoreDescending();

        #endregion
    }
}