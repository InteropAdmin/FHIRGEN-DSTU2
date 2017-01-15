using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PublicationIG = Hl7.Fhir.Publication.ImplementationGuide;
using PublicationFramework = Hl7.Fhir.Publication.Framework;

namespace Fhir.Publication.Tests.Framework.ImplementationGuide
{
    [TestClass]
    public class Package
    {
        private readonly PublicationIG.Package _package;

        public Package()
        {
            var root = new PublicationFramework.Root("sourceDir", "targetDir");
            var context = new PublicationFramework.Context(root);
            PublicationFramework.IDirectoryCreator directoryCreator = new Mock.DirectoryCreator();
            var log = new PublicationFramework.Log(new Mock.ErrorLogger());    
            _package = new PublicationIG.Package("ProfileOne", context, log, directoryCreator);
      }

        [TestMethod]
        [IntegrationTest]
        public void Package_StructureDefinitions_ResourceReturnedExistsInResourceStore()
        {
            _package.LoadResources();

            var resources = new List<Hl7.Fhir.Model.ImplementationGuide.ResourceComponent>();
            var resource = new Hl7.Fhir.Model.ImplementationGuide.ResourceComponent();

            resource.Purpose = Hl7.Fhir.Model.ImplementationGuide.GuideResourcePurpose.Profile;
            resource.Name = "CHIS-BabyPatient-Patient-1-0";
            resource.Source = new FhirUri("http://fhir.nhs.net/StructureDefinition/chis-baby-patient-1-0");

            resource.AddExtension(
                PublicationFramework.Urn.ResourceType.GetUrnString(),
                new FhirString("StructureDefinition"));

            resources.Add(resource);
            _package.SetResources(resources);

            Assert.IsTrue(_package.StructureDefinitions
                .Count(
                    definition => 
                        definition.Name == "CHIS-BabyPatient-Patient-1-0") == 1);
        }

        [TestMethod]
        [IntegrationTest]
        public void Package_StructureDefinitions_ResourceNotReturnedDoesNotExistInResourceStore()
        {
            _package.LoadResources();

            var resources = new List<Hl7.Fhir.Model.ImplementationGuide.ResourceComponent>();
            var resource = new Hl7.Fhir.Model.ImplementationGuide.ResourceComponent();

            resource.Purpose = Hl7.Fhir.Model.ImplementationGuide.GuideResourcePurpose.Profile;
            resource.Name = "patient-1-0";
            resource.Source = new FhirUri("http://fhir.nhs.net/StructureDefinition/patient-1-0");

            resource.AddExtension(
                PublicationFramework.Urn.ResourceType.GetUrnString(),
                new FhirString("StructureDefinition"));

            resources.Add(resource);
            _package.SetResources(resources);

            Assert.IsTrue(!_package.StructureDefinitions.Any());
        }

        [TestMethod]
        [IntegrationTest]
        public void Package_OperationDefinitions_ResourceReturnedExistsInResourceStore()
        {
            _package.LoadResources();

            var resources = new List<Hl7.Fhir.Model.ImplementationGuide.ResourceComponent>();
            var resource = new Hl7.Fhir.Model.ImplementationGuide.ResourceComponent();

            resource.Purpose = Hl7.Fhir.Model.ImplementationGuide.GuideResourcePurpose.Profile;
            resource.Name = "MyOperation";
            resource.Source = new FhirUri("http://fhir.nhs.net/OperationDefinition/ers-MyOperation-1-0");

            resource.AddExtension(
                PublicationFramework.Urn.ResourceType.GetUrnString(),
                new FhirString("OperationDefinition"));

            resources.Add(resource);
            _package.SetResources(resources);

            Assert.IsTrue(_package.OperationDefinitions
                .Count(
                    definition =>
                        definition.Name == "MyOperation") == 1);
        }

        [TestMethod]
        [IntegrationTest]
        public void Package_OperationDefinitions_ResourceNotReturnedDoesNotExistInResourceStore()
        {
            _package.LoadResources();

            var resources = new List<Hl7.Fhir.Model.ImplementationGuide.ResourceComponent>();
            var resource = new Hl7.Fhir.Model.ImplementationGuide.ResourceComponent();

            resource.Purpose = Hl7.Fhir.Model.ImplementationGuide.GuideResourcePurpose.Profile;
            resource.Name = "ers-MyOperation-1-0";
            resource.Source = new FhirUri("http://fhir.nhs.net/OperationDefinition/MyOperation-1-0");

            resource.AddExtension(
                PublicationFramework.Urn.ResourceType.GetUrnString(),
                new FhirString("OperationDefinition"));

            resources.Add(resource);
            _package.SetResources(resources);

            Assert.IsTrue(!_package.OperationDefinitions.Any());
        }

        [TestMethod]
        [IntegrationTest]
        public void Package_HasModifierExtensions_IsTrue()
        {
            _package.LoadResources();

            var resources = new List<Hl7.Fhir.Model.ImplementationGuide.ResourceComponent>();
            var resource = new Hl7.Fhir.Model.ImplementationGuide.ResourceComponent();

            resource.Purpose = Hl7.Fhir.Model.ImplementationGuide.GuideResourcePurpose.Profile;
            resource.Name = "eRS-Specialty-1-0";
            resource.Source = new FhirUri("http://fhir.nhs.net/StructureDefinition/extension-ers-specialty-1-0");

            resource.AddExtension(
                PublicationFramework.Urn.ResourceType.GetUrnString(),
                new FhirString("StructureDefinition"));

            resources.Add(resource);
            _package.SetResources(resources);

            Assert.IsTrue(_package.HasModifierExtensions);
        }

        [TestMethod]
        [IntegrationTest]
        public void Package_HasModifierExtensions_IsFalse()
        {
            _package.LoadResources();

            var resources = new List<Hl7.Fhir.Model.ImplementationGuide.ResourceComponent>();
            var resource = new Hl7.Fhir.Model.ImplementationGuide.ResourceComponent();

            resource.Purpose = Hl7.Fhir.Model.ImplementationGuide.GuideResourcePurpose.Profile;
            resource.Name = "CHIS-BabyPatient-Patient-1-0";
            resource.Source = new FhirUri("http://fhir.nhs.net/StructureDefinition/chis-BabyPatient-Patient-1-0");

            resource.AddExtension(
                PublicationFramework.Urn.ResourceType.GetUrnString(),
                new FhirString("StructureDefinition"));

            resources.Add(resource);
            _package.SetResources(resources);

            Assert.IsFalse(_package.HasModifierExtensions);
        }

        [TestMethod]
        [IntegrationTest]
        public void Package_HasExtensions_IsTrue()
        {
            _package.LoadResources();

            var resources = new List<Hl7.Fhir.Model.ImplementationGuide.ResourceComponent>();
            var resource = new Hl7.Fhir.Model.ImplementationGuide.ResourceComponent();

            resource.Purpose = Hl7.Fhir.Model.ImplementationGuide.GuideResourcePurpose.Profile;
            resource.Name = "Extension-EthnicCategory-1-0";
            resource.Source = new FhirUri("http://fhir.nhs.net/StructureDefinition/extension-ethnic-category-1-0");

            resource.AddExtension(
                PublicationFramework.Urn.ResourceType.GetUrnString(),
                new FhirString("StructureDefinition"));

            resources.Add(resource);
            _package.SetResources(resources);

            Assert.IsTrue(_package.HasExtensions);
        }

        [TestMethod]
        [IntegrationTest]
        public void Package_HasExtensions_IsFalse()
        {
            _package.LoadResources();

            var resources = new List<Hl7.Fhir.Model.ImplementationGuide.ResourceComponent>();
            var resource = new Hl7.Fhir.Model.ImplementationGuide.ResourceComponent();

            resource.Purpose = Hl7.Fhir.Model.ImplementationGuide.GuideResourcePurpose.Profile;
            resource.Name = "MyOperation";
            resource.Source = new FhirUri("http://fhir.nhs.net/OperationDefinition/ers-MyOperation-1-0");

            resource.AddExtension(
                PublicationFramework.Urn.ResourceType.GetUrnString(),
                new FhirString("OperationDefinition"));

            resources.Add(resource);
            _package.SetResources(resources);

            Assert.IsFalse(_package.HasExtensions);
        }
    }
}
