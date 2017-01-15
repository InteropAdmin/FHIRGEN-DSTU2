using System;
using System.Collections.Generic;
using System.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PubIG = Hl7.Fhir.Publication.ImplementationGuide;
using PubFramework = Hl7.Fhir.Publication.Framework;

namespace Fhir.Publication.Tests.Framework.ImplementationGuide
{
    [TestClass]
    public class ResourceFactory
    {
        private readonly PubFramework.IDirectoryCreator _directoryCreator;

        public ResourceFactory()
        {
            _directoryCreator = new Mock.DirectoryCreator();    
        }

        [TestMethod]
        public void ResourceFactory_CreateProfileResource_ResourseHasDefinitionName()
        {
            var meta = new Meta();

            var resourceDefinition = new PubIG.Resource(
                        "StructureDefinitionName",
                        "StructureDefinitionDescription",
                        "http://structureDefinitionURL",
                         meta,
                        ResourceType.StructureDefinition);

            var factory = new PubIG.ResourceFactory(resourceDefinition, _directoryCreator);

            Hl7.Fhir.Model.ImplementationGuide.ResourceComponent actual = factory.CreateProfileResource("TestStructureDefinition.md");

            Assert.AreEqual("StructureDefinitionName", actual.Name);          
        }

        [TestMethod]
        public void ResourceFactory_CreateProfileResource_ResourceHasMdFileDescription()
        {
            var meta = new Meta();

            var resourceDefinition = new PubIG.Resource(
                        "StructureDefinitionName",
                        "StructureDefinitionDescription",
                        "http://structureDefinitionURL",
                         meta,
                        ResourceType.StructureDefinition);

            var factory = new PubIG.ResourceFactory(resourceDefinition, _directoryCreator);

            Hl7.Fhir.Model.ImplementationGuide.ResourceComponent actual = factory.CreateProfileResource("TestStructureDefinition.md");

            Assert.AreEqual("This is a description!", actual.Description);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ResourceFactory_CreateProfileResource_InvalidOperationExceptionThrownIfMdFileDoesNotExist()
        {
            var meta = new Meta();

            var resourceDefinition = new PubIG.Resource(
                        "StructureDefinitionName",
                        "StructureDefinitionDescription",
                        "http://structureDefinitionURL",
                         meta,
                        ResourceType.StructureDefinition);

            var factory = new PubIG.ResourceFactory(resourceDefinition, _directoryCreator);

            factory.CreateProfileResource("NotExists.md");
        }


        [TestMethod]
        public void ResourceFactory_CreateProfileResource_PublishOrderIsZeroWhenNotIncludedInMeta()
        {
            var meta = new Meta();

            var resourceDefinition = new PubIG.Resource(
                        "StructureDefinitionName",
                        "StructureDefinitionDescription",
                        "http://structureDefinitionURL",
                         meta,
                        ResourceType.StructureDefinition);

            var factory = new PubIG.ResourceFactory(resourceDefinition, _directoryCreator);

            Hl7.Fhir.Model.ImplementationGuide.ResourceComponent actual = factory.CreateProfileResource("TestStructureDefinition.md");

            Assert.AreEqual("0", actual.GetExtension(PubFramework.Urn.PublishOrder.GetUrnString()).Value.ToString());
        }

        [TestMethod]
        public void ResourceFactory_CreateProfileResource_ResourcePublishOrderIs33()
        {
            var meta = new Meta();
            var codings = new List<Coding>();
            var publishOrder = new Coding(PubFramework.Urn.PublishOrder.GetUrnString(), "33");
            codings.Add(publishOrder);

            meta.Tag = codings;

            var resourceDefinition = new PubIG.Resource(
                        "StructureDefinitionName",
                        "StructureDefinitionDescription",
                        "http://structureDefinitionURL",
                         meta,
                        ResourceType.StructureDefinition);

            var factory = new PubIG.ResourceFactory(resourceDefinition, _directoryCreator);

            Hl7.Fhir.Model.ImplementationGuide.ResourceComponent actual = factory.CreateProfileResource("TestStructureDefinition.md");

            Assert.AreEqual("33", actual.GetExtension(PubFramework.Urn.PublishOrder.GetUrnString()).Value.ToString());
        }

        [TestMethod]
        public void ResourceFactory_CreateProfileResource_ResourcePurposeIsProfile()
        {
            var meta = new Meta();

            var resourceDefinition = new PubIG.Resource(
                        "StructureDefinitionName",
                        "StructureDefinitionDescription",
                        "http://structureDefinitionURL",
                         meta,
                        ResourceType.StructureDefinition);

            var factory = new PubIG.ResourceFactory(resourceDefinition, _directoryCreator);

            Hl7.Fhir.Model.ImplementationGuide.ResourceComponent actual = factory.CreateProfileResource("TestStructureDefinition.md");

            Assert.AreEqual(Hl7.Fhir.Model.ImplementationGuide.GuideResourcePurpose.Profile, actual.Purpose);
        }

        [TestMethod]
        public void ResourceFactory_CreateProfileResource_ResourceSourceIsDefinitionUrl()
        {
            var meta = new Meta();
            var expected = new FhirUri("http://structureDefinitionURL");

            var resourceDefinition = new PubIG.Resource(
                        "StructureDefinitionName",
                        "StructureDefinitionDescription",
                        expected.Value,
                         meta,
                        ResourceType.StructureDefinition);

            var factory = new PubIG.ResourceFactory(resourceDefinition, _directoryCreator);

            Hl7.Fhir.Model.ImplementationGuide.ResourceComponent actual = factory.CreateProfileResource("TestStructureDefinition.md");

            var test = (FhirUri)actual.Source;

            Assert.AreSame(expected.Value, test.Value);
          
        }

        [TestMethod]
        public void ResourceFactory_CreateProfileResource_ResourceTypeIsStructureDefinition()
        {
            var meta = new Meta();
            var expected = new FhirUri("http://structureDefinitionURL");

            var resourceDefinition = new PubIG.Resource(
                        "StructureDefinitionName",
                        "StructureDefinitionDescription",
                        expected.Value,
                         meta,
                        ResourceType.StructureDefinition);

            var factory = new PubIG.ResourceFactory(resourceDefinition, _directoryCreator);

            Hl7.Fhir.Model.ImplementationGuide.ResourceComponent actual = factory.CreateProfileResource("TestStructureDefinition.md");

            Assert.AreEqual(ResourceType.StructureDefinition.ToString(), actual.GetExtension(PubFramework.Urn.ResourceType.GetUrnString()).Value.ToString());
        }

        [TestMethod]
        public void ResourceFactory_CreateExampleResources_ResourceHas3Examples()
        {
            var meta = new Meta();
            var codings = new List<Coding>();
            var example1 = new Coding(PubFramework.Urn.Example.GetUrnString(), "ExampleOne");
            var example2 = new Coding(PubFramework.Urn.Example.GetUrnString(), "ExampleTwo");
            var example3 = new Coding(PubFramework.Urn.Example.GetUrnString(), "ExampleThree");

            codings.AddRange(new [] { example1, example2, example3 });
            meta.Tag = codings;

            var resourceDefinition = new PubIG.Resource(
                        "StructureDefinitionName",
                        "StructureDefinitionDescription",
                        "http://structureDefinitionURL",
                         meta,
                        ResourceType.StructureDefinition);

            var factory = new PubIG.ResourceFactory(resourceDefinition, _directoryCreator);

            IEnumerable<Hl7.Fhir.Model.ImplementationGuide.ResourceComponent> actual = factory.CreateExampleResources();

            Assert.IsTrue(actual.Count() == 3);           
        }

        [TestMethod]
        public void ResourceFactory_CreateExampleResources_ZeroExamplesWhenUrnNotInMeta()
        {
            var meta = new Meta();
            var codings = new List<Coding>();
            var example1 = new Coding(PubFramework.Urn.ResourceType.GetUrnString(), "ExampleOne");


            codings.Add(example1);
            meta.Tag = codings;

            var resourceDefinition = new PubIG.Resource(
                        "StructureDefinitionName",
                        "StructureDefinitionDescription",
                        "http://structureDefinitionURL",
                         meta,
                        ResourceType.StructureDefinition);

            var factory = new PubIG.ResourceFactory(resourceDefinition, _directoryCreator);

            IEnumerable<Hl7.Fhir.Model.ImplementationGuide.ResourceComponent> actual = factory.CreateExampleResources();

            Assert.IsTrue(!actual.Any());
        }

        [TestMethod]
        public void ResourceFactory_CreateTerminologyResource_ResourceTypeIsValueset()
        {
            var meta = new Meta();
            var url = new FhirUri("http://valuesetUrl");

            var resourceDefinition = new PubIG.Resource(
                        "ValuesetName",
                        "ValuesetDescription",
                        url.Value,
                         meta,
                        ResourceType.ValueSet);

            var factory = new PubIG.ResourceFactory(resourceDefinition, _directoryCreator);

            Hl7.Fhir.Model.ImplementationGuide.ResourceComponent actual = factory.CreateTerminologyResource("TestValueset.md");

            Assert.AreEqual(ResourceType.ValueSet.ToString(), actual.GetExtension(PubFramework.Urn.ResourceType.GetUrnString()).Value.ToString());
        }

        [TestMethod]
        public void ResourceFactory_CreateTerminologyResource_ResourseHasDefinitionName()
        {
            var meta = new Meta();
            var url = new FhirUri("http://valuesetUrl");

            var resourceDefinition = new PubIG.Resource(
                       "ValuesetName",
                       "ValuesetDescription",
                       url.Value,
                        meta,
                       ResourceType.ValueSet);

            var factory = new PubIG.ResourceFactory(resourceDefinition, _directoryCreator);

            Hl7.Fhir.Model.ImplementationGuide.ResourceComponent actual = factory.CreateTerminologyResource("TestValueset.md");

            Assert.AreEqual("ValuesetName", actual.Name);
        }
    }
}