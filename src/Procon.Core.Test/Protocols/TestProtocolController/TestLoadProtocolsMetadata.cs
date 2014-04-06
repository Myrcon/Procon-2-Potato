﻿using System.IO;
using System.Linq;
using NUnit.Framework;
using Procon.Core.Protocols;
using Procon.Service.Shared;

namespace Procon.Core.Test.Protocols.TestProtocolController {
    [TestFixture]
    public class TestLoadProtocolsMetadata {
        /// <summary>
        /// Clears out all files in the packages directory and ensures the packages directory is created.
        /// </summary>
        [SetUp]
        public void CleanPackagesDirectory() {
            Defines.PackagesDirectory.Delete(true);
            Defines.PackagesDirectory.Create();
        }

        /// <summary>
        /// Tests that a protocol is loaded from a protocol package
        /// </summary>
        [Test]
        public void TestLoadedWithSingleVersionOfPackage() {
            DirectoryInfo package = new DirectoryInfo(Path.Combine(Defines.PackagesDirectory.FullName, "Something.Protocols.Something"));

            var dll = new FileInfo(Path.Combine(package.FullName, "lib", "Something.Protocols.Something.dll"));
            if (dll.Directory != null) dll.Directory.Create();
            File.WriteAllText(dll.FullName, @"binary");

            var json = new FileInfo(Path.Combine(package.FullName, "Content", "Something.Protocols.Something.json"));
            if (json.Directory != null) json.Directory.Create();
            File.WriteAllText(json.FullName, @"{ }");


            var protocols = new ProtocolController();

            protocols.LoadProtocolsMetadata();

            Assert.AreEqual(1, protocols.Protocols.Count);
        }

        /// <summary>
        /// Tests that a protocol is loaded from a protocol metadata
        /// </summary>
        /// <remarks>We only test the process is succesful, loading protocol metadata is tested elsewhere.</remarks>
        [Test]
        public void TestProtocolVariablesLoaded() {
            DirectoryInfo package = new DirectoryInfo(Path.Combine(Defines.PackagesDirectory.FullName, "Something.Protocols.Something"));

            var dll = new FileInfo(Path.Combine(package.FullName, "lib", "Something.Protocols.Something.dll"));
            if (dll.Directory != null) dll.Directory.Create();
            File.WriteAllText(dll.FullName, @"binary");

            var json = new FileInfo(Path.Combine(package.FullName, "Content", "Something.Protocols.Something.json"));
            if (json.Directory != null) json.Directory.Create();
            File.WriteAllText(json.FullName, @"{ ""ProtocolTypes"": [ { ""Provider"": ""Myrcon"",""Name"": ""Battlefield 4"",""Type"": ""DiceBattlefield4"" } ] }");

            var protocols = new ProtocolController();

            protocols.LoadProtocolsMetadata();

            Assert.AreEqual("DiceBattlefield4", protocols.Protocols.First().ProtocolTypes.First().Type);
        }

        /// <summary>
        /// Tests that a single protocol will be loaded when multiple versions of the package are available.
        /// </summary>
        [Test]
        public void TestSingleProtocolLoadedWithMultipleVersionsOfPackage() {
            DirectoryInfo newest = new DirectoryInfo(Path.Combine(Defines.PackagesDirectory.FullName, "Something.Protocols.Something.2.0.0"));
            DirectoryInfo oldest = new DirectoryInfo(Path.Combine(Defines.PackagesDirectory.FullName, "Something.Protocols.Something.1.0.0"));

            var newestdll = new FileInfo(Path.Combine(newest.FullName, "lib", "Something.Protocols.Something.dll"));
            if (newestdll.Directory != null) newestdll.Directory.Create();
            File.WriteAllText(newestdll.FullName, @"binary");

            var newestjson = new FileInfo(Path.Combine(newest.FullName, "Content", "Something.Protocols.Something.json"));
            if (newestjson.Directory != null) newestjson.Directory.Create();
            File.WriteAllText(newestjson.FullName, @"{ }");

            var oldestdll = new FileInfo(Path.Combine(newest.FullName, "lib", "Something.Protocols.Something.dll"));
            if (oldestdll.Directory != null) oldestdll.Directory.Create();
            File.WriteAllText(oldestdll.FullName, @"binary");

            var oldestjson = new FileInfo(Path.Combine(newest.FullName, "Content", "Something.Protocols.Something.json"));
            if (oldestjson.Directory != null) oldestjson.Directory.Create();
            File.WriteAllText(oldestjson.FullName, @"{ }");

            var protocols = new ProtocolController();

            protocols.LoadProtocolsMetadata();

            Assert.AreEqual(1, protocols.Protocols.Count);
        }

        /// <summary>
        /// Tests the latest version of the packag is loaded.
        /// </summary>
        [Test]
        public void TestLatestLoadedLoadedWithMultipleVersionsOfPackage() {
            DirectoryInfo newest = new DirectoryInfo(Path.Combine(Defines.PackagesDirectory.FullName, "Something.Protocols.Something.2.0.0"));
            DirectoryInfo oldest = new DirectoryInfo(Path.Combine(Defines.PackagesDirectory.FullName, "Something.Protocols.Something.1.0.0"));

            var newestdll = new FileInfo(Path.Combine(newest.FullName, "lib", "Something.Protocols.Something.dll"));
            if (newestdll.Directory != null) newestdll.Directory.Create();
            File.WriteAllText(newestdll.FullName, @"binary");

            var newestjson = new FileInfo(Path.Combine(newest.FullName, "Content", "Something.Protocols.Something.json"));
            if (newestjson.Directory != null) newestjson.Directory.Create();
            File.WriteAllText(newestjson.FullName, @"{ }");

            var oldestdll = new FileInfo(Path.Combine(newest.FullName, "lib", "Something.Protocols.Something.dll"));
            if (oldestdll.Directory != null) oldestdll.Directory.Create();
            File.WriteAllText(oldestdll.FullName, @"binary");

            var oldestjson = new FileInfo(Path.Combine(newest.FullName, "Content", "Something.Protocols.Something.json"));
            if (oldestjson.Directory != null) oldestjson.Directory.Create();
            File.WriteAllText(oldestjson.FullName, @"{ }");

            var protocols = new ProtocolController();

            protocols.LoadProtocolsMetadata();

            Assert.AreEqual(newest.FullName, protocols.Protocols.First().Directory.FullName);
        }
    }
}
