using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hl7.Fhir.Publication.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PublicationStore = Hl7.Fhir.Publication.Framework.Config;

namespace Fhir.Publication.Tests.Framework.Config
{
    [TestClass]
    public class Store
    {
        private readonly byte[] _jsonConfigFile;
        private readonly Context _context;
        private PublicationStore.Store _configStore;

        private IDirectoryCreator _directoryCreator;

        public Store()
        {
            _context = new Context(new Root(@"\..\TestData", "targetDir"));
            _jsonConfigFile = Resources.config;
        }

        private void CreateMockDirCreator_FileExists()
        {
            var mockFileCreator = new Mock<IDirectoryCreator>(MockBehavior.Strict);
            mockFileCreator.Setup(creator => creator.FileExists(It.IsAny<string>())).Returns(true);
            mockFileCreator.Setup(creator => creator.ReadAllText(It.IsAny<string>())).Returns(Encoding.Default.GetString(_jsonConfigFile));
            _directoryCreator = mockFileCreator.Object;
        }

        private void CreateMockDirCreator_FileDoesNotExist()
        {
            var mockFileCreator = new Mock<IDirectoryCreator>(MockBehavior.Strict);
            mockFileCreator.Setup(creator => creator.FileExists(It.IsAny<string>())).Returns(false);

            _directoryCreator = mockFileCreator.Object;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConfigStore_FileCreatorIsNull_ArgumentNullExceptionThrown()
        {
            _configStore = new PublicationStore.Store(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConfigStore_GetConfigStore_InvalidOperationExceptionThrowIfFileDoesNotExist()
        {
            CreateMockDirCreator_FileDoesNotExist();
            _configStore = new PublicationStore.Store(_directoryCreator);
            _configStore.GetConfigStore(_context);
        }

        [TestMethod]
        public void ConfigStore_GetConfigStore_ConfigFileContainsSixElements()
        {
            CreateMockDirCreator_FileExists();
            _configStore = new PublicationStore.Store(_directoryCreator);
            Dictionary<string, string> config = _configStore.GetConfigStore(_context);

            Assert.IsTrue(config.Count == 6);
        }

        [TestMethod]
        public void ConfigStore_GetConfigStore_ConfigFileElementsAreReturned()
        {
            CreateMockDirCreator_FileExists();
            _configStore = new PublicationStore.Store(_directoryCreator);
            Dictionary<string, string> config = _configStore.GetConfigStore(_context);

            Assert.IsTrue(config.Keys.Contains("EXAMPLES_XML"));
            Assert.IsTrue(config["EXAMPLES_XML"] == "TRUE");

            Assert.IsTrue(config.Keys.Contains("EXAMPLES_JSON"));
            Assert.IsTrue(config["EXAMPLES_JSON"] == "TRUE");
            
            Assert.IsTrue(config.Keys.Contains("DMS_ONLINE"));
            Assert.IsTrue(config["DMS_ONLINE"] == "TRUE");
            
            Assert.IsTrue(config.Keys.Contains("DMS_ANALYTICS"));
            Assert.IsTrue(config["DMS_ANALYTICS"] == "UA-67795986-1");

            Assert.IsTrue(config.Keys.Contains("DMS_TITLE"));
            Assert.IsTrue(config["DMS_TITLE"] == "FHIR API implementation guide for GPSoC IM2");

            Assert.IsTrue(config.Keys.Contains("DMS_VERSION"));
            Assert.IsTrue(config["DMS_VERSION"] == "Version 1.0 : DRAFT A");
        }

        [TestMethod]
        public void ConfigStore_GetConfigValue_ValueExists_()
        {
            CreateMockDirCreator_FileExists();
            _configStore = new PublicationStore.Store(_directoryCreator);
            Dictionary<string, string> config = _configStore.GetConfigStore(_context);

            string actual = PublicationStore.Store.GetConfigValue(PublicationStore.KeyType.DmsTitle, config);
            const string expected = "FHIR API implementation guide for GPSoC IM2";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConfigStore_GetConfigValue_InvalidOperationExceptionThrownWhenValueDoeNotExist_()
        {
            CreateMockDirCreator_FileExists();
            _configStore = new PublicationStore.Store(_directoryCreator);
            Dictionary<string, string> config = _configStore.GetConfigStore(_context);

            PublicationStore.Store.GetConfigValue(PublicationStore.KeyType.None, config);
        }
    }
}
