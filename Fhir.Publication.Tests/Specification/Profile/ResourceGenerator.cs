using System;
using System.IO;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Fhir.Publication.Tests.Specification.Profile
{
    [TestClass]
    public class ResourceGenerator
    {
        private const string _path = @"C:\filePath";
        private readonly string _xml;
        private readonly Log _log;
        private IDirectoryCreator _fileCreator;
        private Hl7.Fhir.Publication.Specification.Profile.ResourceGenerator _fileGenerator;
        private Mock.DirectoryCreator _mockDirectoryCreator;

        private bool _xmlRequired;
        private bool _jsonRequired;

        public ResourceGenerator()
        {
            _log = new Log(new Mock.ErrorLogger());
            _xml = File.ReadAllText(@"TestData\BirthNotification-BabyLength.xml");
            var root = new Root("sourceDir", "targetDir");
            var package = new ImplementationGuide.PackageComponent();
            package.Name = "packageName";
        }

        private void CreateMockFileCreater_ExampleFileExists()
        {
            var mockFileCreator = new Mock<IDirectoryCreator>(MockBehavior.Strict);
            mockFileCreator.Setup(creator => creator.FileExists(It.IsAny<string>())).Returns(true);
            mockFileCreator.Setup(creator => creator.ReadAllText(It.IsAny<string>())).Returns(_xml);
            mockFileCreator.Setup(creator => creator.WriteAllText(_path, _xml.ToString()));

            _fileCreator = mockFileCreator.Object;
        }

        private void CreateMockFileCreator_ExampleFileDoesNotExist()
        {
            var mockFileCreator = new Mock<IDirectoryCreator>(MockBehavior.Strict);
            mockFileCreator.Setup(creator => creator.FileExists(It.IsAny<string>())).Returns(false);
            mockFileCreator.Setup(creator => creator.ReadAllText(It.IsAny<string>())).Returns(_xml);
            mockFileCreator.Setup(creator => creator.WriteAllText(_path, _xml.ToString()));

            _fileCreator = mockFileCreator.Object;
        }

        private void CreateMockFileCreator(bool xmlRequired, bool jsonRequired)
        {
            _xmlRequired = xmlRequired;
            _jsonRequired = jsonRequired;
            _mockDirectoryCreator = new Mock.DirectoryCreator();
            _fileGenerator = new Hl7.Fhir.Publication.Specification.Profile.ResourceGenerator(_mockDirectoryCreator, _xmlRequired, _jsonRequired, _log);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ResourceGenerator_DirectoryCreatorIsNull_ArgumentNullExceptionThrown()
        {
            _fileGenerator = new Hl7.Fhir.Publication.Specification.Profile.ResourceGenerator(null, _xmlRequired, _jsonRequired, _log);
        }

        [TestMethod]
        public void ResourceGenerator_ExampleFileExists_CreateXmlFileIsCalled()
        {
            CreateMockFileCreator(true, true);
            _fileGenerator.Generate("BirthNotification_BabyPatient", "source");

            Assert.IsTrue(_mockDirectoryCreator.XmlCreated);
        }

        [TestMethod]
        public void ResourceGenerator_ExampleFileExists_CreateJsonFileIsCalled()
        {
            CreateMockFileCreator(false, true);
            _fileGenerator.Generate("BirthNotification_BabyPatient", "source");

            Assert.IsTrue(_mockDirectoryCreator.JsonCreated);
        }

        [TestMethod]
        public void ResourceGenerator_XmlFileRequestedNotJson_OnlyCreateXmlFileIsCalled()
        {
            CreateMockFileCreator(true, false);
            _fileGenerator.Generate("BirthNotification_BabyPatient", "source");

            Assert.IsTrue(_mockDirectoryCreator.XmlCreated);
            Assert.IsFalse(_mockDirectoryCreator.JsonCreated);
        }

        [TestMethod]
        public void ResourceGenerator_JsonFileRequestedNotXml_OnlyCreateJsonFileIsCalled()
        {
            CreateMockFileCreator(false, true);
            _fileGenerator.Generate("BirthNotification_BabyPatient", "source");

            Assert.IsTrue(_mockDirectoryCreator.JsonCreated);
            Assert.IsFalse(_mockDirectoryCreator.XmlCreated);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ResourceGenerator_GenerateWithNullItemCodingItem_ArgumentExceptionThrow()
        {
            CreateMockFileCreator_ExampleFileDoesNotExist();
            _fileGenerator = new Hl7.Fhir.Publication.Specification.Profile.ResourceGenerator(_fileCreator, _xmlRequired, _jsonRequired, _log);
            _fileGenerator.Generate(null, "source");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ResourceGenerator_GenerateWithEmptyItemCodingItem_ArgumentExceptionThrow()
        {
            CreateMockFileCreator_ExampleFileDoesNotExist();
            _fileGenerator = new Hl7.Fhir.Publication.Specification.Profile.ResourceGenerator(_fileCreator, _xmlRequired, _jsonRequired, _log);
            _fileGenerator.Generate(null, "source");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ResourceGenerator_GenerateWithNullSource_ArgumentExceptionThrow()
        {
            CreateMockFileCreator_ExampleFileDoesNotExist();
            _fileGenerator = new Hl7.Fhir.Publication.Specification.Profile.ResourceGenerator(_fileCreator, _xmlRequired, _jsonRequired, _log);
            _fileGenerator.Generate(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ResourceGenerator_GenerateWithEmptySource_ArgumentExceptionThrow()
        {
            CreateMockFileCreator_ExampleFileDoesNotExist();
            _fileGenerator = new Hl7.Fhir.Publication.Specification.Profile.ResourceGenerator(_fileCreator, _xmlRequired, _jsonRequired, _log);
            _fileGenerator.Generate(null, string.Empty);
        }
    }
}
