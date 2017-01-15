using System;
using System.Collections.Generic;
using Hl7.Fhir.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PublicationSpec = Hl7.Fhir.Publication.Specification.Profile;

namespace Fhir.Publication.Tests.Specification.Profile
{
    [TestClass]
    public class TableFactory
    {
        private readonly Hl7.Fhir.Publication.Framework.IDirectoryCreator _directoryCreator;
        public TableFactory()
        {
            _directoryCreator = new Mock.DirectoryCreator();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "myPackage failed to generated any rows!")]
        public void ProfileTableGenerator_Generate_InvalidOperationExceptionThrownIfNoRowsGeneratedForAPackage()
        {
            var factory = new PublicationSpec.TableFactory(new Hl7.Fhir.Publication.Specification.Profile.KnowledgeProvider(new Hl7.Fhir.Publication.Framework.Log(new Mock.ErrorLogger())));
            factory.GenerateProfile(new List<StructureDefinition>(), "myPackage", PublicationSpec.Icon.Profile, _directoryCreator);
        }
    }
}