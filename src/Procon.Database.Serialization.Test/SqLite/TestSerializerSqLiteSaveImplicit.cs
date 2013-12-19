﻿using System.Linq;
using NUnit.Framework;
using Procon.Database.Serialization.Serializers.Sql;

namespace Procon.Database.Serialization.Test.SqLite {
    [TestFixture]
    public class TestSerializerSqLiteSaveImplicit : TestSerializerSave {
        [Test]
        public override void TestSaveIntoPlayerSetName() {
            Assert.AreEqual(@"INSERT INTO `Player` (`Name`) VALUES (""Phogue"")", new SerializerSqLite().Parse(this.TestSaveIntoPlayerSetNameImplicit).Compile().Compiled.First());
        }

        [Test]
        public override void TestSaveIntoPlayerSetNameScore() {
            Assert.AreEqual(@"INSERT INTO `Player` (`Name`, `Score`) VALUES (""Phogue"", 50)", new SerializerSqLite().Parse(this.TestSaveIntoPlayerSetNameScoreImplicit).Compile().Compiled.First());
        }

        [Test]
        public override void TestSaveIntoPlayerSetNameAndStamp() {
            Assert.AreEqual(@"INSERT INTO `Player` SET `Name` = ""Phogue"", `Stamp` = ""2013-12-19 01:08:00""", new SerializerMySql().Parse(this.TestSaveIntoPlayerSetNameAndStampImplicit).Compile().Compiled.First());
        }
    }
}