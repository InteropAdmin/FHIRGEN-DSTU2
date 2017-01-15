using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PublicationIG = Hl7.Fhir.Publication.ImplementationGuide;
using PublicationFramework = Hl7.Fhir.Publication.Framework;

namespace Fhir.Publication.Tests.Framework.ImplementationGuide
{
    [TestClass]
    public class ResourceLoader
    {
        private readonly PublicationFramework.IDirectoryCreator _directoryCreator;
        private readonly PublicationFramework.Log _log;
        private readonly PublicationIG.ResourceLoader _loader;

        public ResourceLoader()
        {
            _directoryCreator = new Mock.DirectoryCreator();
            _log = new PublicationFramework.Log(new Mock.ErrorLogger());
            _loader = new PublicationIG.ResourceLoader(_directoryCreator, _log);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ResourceLoader_ArgumentNullExceptionThrowWhenDirectoryCreatorIsNull()
        {
            var loader = new PublicationIG.ResourceLoader(null, _log);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ResourceLoader_ArgumentNullExceptionThrowWhenLogIsNull()
        {
            var loader = new PublicationIG.ResourceLoader(_directoryCreator, null);
        }

        [TestMethod]
        public void ResourceLoader_LoadResourceStore_ResourceStoreHasStructureDefinition()
        {
            IEnumerable<string> fileEntries =_directoryCreator.EnumerateFiles("sourceDir", "*.xml", SearchOption.AllDirectories);

            PublicationIG.ResourceStore store = _loader.LoadResourceStore(fileEntries);

            Assert.IsTrue(store.Resources.Count(resource => resource.Url == "http://fhir.nhs.net/StructureDefinition/chis-baby-patient-1-0") == 1);
        }

        [TestMethod]
        public void ResourceLoader_LoadResourcestore_ResourceStoreHasOperationDefinition()
        {
            IEnumerable<string> fileEntries = _directoryCreator.EnumerateFiles("sourceDir", "*.xml", SearchOption.AllDirectories);

            PublicationIG.ResourceStore store = _loader.LoadResourceStore(fileEntries);

            Assert.IsTrue(store.Resources.Count(resource => resource.Url == "http://fhir.nhs.net/OperationDefinition/ers-MyOperation-1-0") == 1);
        }

        [TestMethod]
        public void ResourceLoader_LoadResourceStore_ResourceStoreHasValueset()
        {
            IEnumerable<string> fileEntries = _directoryCreator.EnumerateFiles("sourceDir", "*.xml", SearchOption.AllDirectories);

            PublicationIG.ResourceStore store = _loader.LoadResourceStore(fileEntries);

            Assert.IsTrue(store.Resources.Count(resource => resource.Url == "http://fhir.nhs.net/ValueSet/administrative-gender-1-0") == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEnumArgumentException))]
        public void ResourceLoader_LoadResourceStore_UnsupportedResourceThrowsInvalidEnumArgumentException()
        {
            IEnumerable<string> fileEntries = _directoryCreator.EnumerateFiles("UnsupportedResourceSource", "*.xml", SearchOption.AllDirectories);

            PublicationIG.ResourceStore store = _loader.LoadResourceStore(fileEntries);
        }
    }
}