﻿using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;
using Procon.Config.Core.Models;
using Procon.Service.Shared;

namespace Procon.Config.Test.Models {
    [TestFixture]
    public class TestCertificateModel {
        /// <summary>
        /// Tests a certificate will be generated and can be read by .NET
        /// </summary>
        [Test]
        public void TestGenerate() {
            CertificateModel model = new CertificateModel();

            // Delete the certificate if it exists.
            Defines.CertificatesDirectoryCommandServerPfx.Delete();

            // Create a new certificate
            model.Generate();

            // Certificate exists
            Defines.CertificatesDirectoryCommandServerPfx.Refresh();
            Assert.IsTrue(Defines.CertificatesDirectoryCommandServerPfx.Exists);

            // Loads the certificates
            var loadedCertificate = new X509Certificate2(Defines.CertificatesDirectoryCommandServerPfx.FullName, model.Password);
            
            // Certificate can be loaded.
            Assert.IsNotNull(loadedCertificate);
            Assert.IsNotNull(loadedCertificate.PrivateKey);
        }
    }
}