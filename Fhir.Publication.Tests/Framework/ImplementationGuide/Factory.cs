using System;
using Hl7.Fhir.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PublicationFramework = Hl7.Fhir.Publication.Framework;
using PublicationIG = Hl7.Fhir.Publication.ImplementationGuide;

namespace Fhir.Publication.Tests.Framework.ImplementationGuide
{
    [TestClass]
    public class Factory
    {
        private readonly PublicationFramework.Log _log;
        private readonly PublicationIG.Base _base;
        private readonly PublicationFramework.IDirectoryCreator _directoryCreator;
        private readonly PublicationFramework.Context _context;

        public Factory()
        {
            _log = new PublicationFramework.Log(new Mock.ErrorLogger());
            _directoryCreator = new Mock.DirectoryCreator();
            var root = new PublicationFramework.Root("sourceDir", "targetDir");
            _context = new PublicationFramework.Context(root);
            _base = new PublicationIG.Base(_directoryCreator, _context);        
        }

        [TestMethod]
        public void Factory_GetResourceTypeFromXml_ResourceTypeIsStructureDefinition()
        {
            var input = PublicationFramework.Document.CreateFromFullPath(_context, "fullPath");
            var factory = new PublicationIG.Factory(_log, _base, input, _directoryCreator, "Profile.BirthNotification", "BirthNotification_BabyPatient.xml");
          
            Assert.IsTrue(factory.Type == ResourceType.StructureDefinition);
        }

        [TestMethod]
        public void Factory_GetResourceTypeFromXml_ResourceTypeIsValueset()
        {
            var input = PublicationFramework.Document.CreateFromFullPath(_context, "fullPath");
            var factory = new PublicationIG.Factory(_log, _base, input, _directoryCreator, "Profile.BirthNotification", "Administrative-Gender-1-0.xml");

            Assert.IsTrue(factory.Type == ResourceType.ValueSet);
        }

        [TestMethod]
        public void Factory_GetResourceTypeFromXml_ResourceTypeIsOperationDefinition()
        {
            var input = PublicationFramework.Document.CreateFromFullPath(_context, "fullPath");
            var factory = new PublicationIG.Factory(_log, _base, input, _directoryCreator, "Profile.BirthNotification", "MyOperation.xml");

            Assert.IsTrue(factory.Type == ResourceType.OperationDefinition);
        }

        [TestMethod]
        public void Factory_GetProfileAsStructureDefinition_StructureDefinitionReturned()
        {
            var input = PublicationFramework.Document.CreateFromFullPath(_context, "fullPath");
            var implementationGuide = new PublicationIG.Factory(_log, _base, input, _directoryCreator, "Profile.BirthNotification", "BirthNotification_BabyPatient.xml");

            var structureDefinition = implementationGuide.GetProfileAsStructureDefinition();
          
            Assert.IsTrue(structureDefinition != null && structureDefinition.Name == "CHIS-BabyPatient-Patient-1-0");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Factory_GetProfileAsStructureDefinition_InvalidOperationExceptionThrownWhenResourceIsNotAStructureDefinition()
        {
            var input = PublicationFramework.Document.CreateFromFullPath(_context, "fullPath");
            var implementationGuide = new PublicationIG.Factory(_log, _base, input, _directoryCreator, "Profile.BirthNotification", "MyOperation.xml");

            implementationGuide.GetProfileAsStructureDefinition();
        }

        [TestMethod]
        public void Factory_GetProfileAsOperationDefinition_OperationDefinitionReturned()
        {
            var input = PublicationFramework.Document.CreateFromFullPath(_context, "fullPath");
            var implementationGuide = new PublicationIG.Factory(_log, _base, input, _directoryCreator, "Profile.BirthNotification", "MyOperation.xml");

            var operationDefinition = implementationGuide.GetProfileAsOperationDefinition();

            Assert.IsTrue(operationDefinition != null && operationDefinition.Name == "MyOperation");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Factory_GetProfileAsOperationDefinition_InvalidOperationExceptionThrownWhenResourceIsNotAnOperationDefinition()
        {
            var input = PublicationFramework.Document.CreateFromFullPath(_context, "fullPath");
            var implementationGuide = new PublicationIG.Factory(_log, _base, input, _directoryCreator, "Profile.BirthNotification", "BirthNotification_BabyPatient.xml");

            implementationGuide.GetProfileAsOperationDefinition();
        }

        [TestMethod]
        public void Factory_GetProfileAsValueset_ValuesetReturned()
        {
            var input = PublicationFramework.Document.CreateFromFullPath(_context, "fullPath");
            var implementationGuide = new PublicationIG.Factory(_log, _base, input, _directoryCreator, "Profile.BirthNotification", "Administrative-Gender-1-0.xml");

            var valueSet = implementationGuide.GetProfileAsValueset();

            Assert.IsTrue(valueSet != null && valueSet.Name == "Administrative-Gender-1-0");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Factory_GetProfileAsValueset_InvalidOperationExceptionThrownWhenResourceIsNotAValueset()
        {
            var input = PublicationFramework.Document.CreateFromFullPath(_context, "fullPath");
            var implementationGuide = new PublicationIG.Factory(_log, _base, input, _directoryCreator, "Profile.BirthNotification", "BirthNotification_BabyPatient.xml");

            implementationGuide.GetProfileAsValueset();
        }
    }
}
