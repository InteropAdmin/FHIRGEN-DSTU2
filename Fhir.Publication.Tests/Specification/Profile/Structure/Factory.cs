using System;
using System.Collections.Generic;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.ImplementationGuide;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PubFramework = Hl7.Fhir.Publication.Framework;
using PubProfile = Hl7.Fhir.Publication.Specification;
using PubStructure = Hl7.Fhir.Publication.Specification.Profile.Structure;

namespace Fhir.Publication.Tests.Specification.Profile.Structure
{
    [TestClass]
    public class Factory
    {
        private readonly PubProfile.Profile.KnowledgeProvider _profileKnowledgeProvider;
        private readonly Package _packageFactory;
        private readonly PubFramework.Log _log;
        private readonly PubFramework.IDirectoryCreator _directoryCreator;
        private readonly bool _isDifferential;

        public Factory()
        {
            _log = new PubFramework.Log(new Mock.ErrorLogger());
            _profileKnowledgeProvider = new PubProfile.Profile.KnowledgeProvider(_log);
            var context = new PubFramework.Context(new PubFramework.Root("sourceDir", "targetDir"));
            _packageFactory = new Package("ProfileOne", context, _log, new Mock.DirectoryCreator());
            _directoryCreator = new Mock.DirectoryCreator();
            _isDifferential = false;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Factory_ArgumentNullExceptionThrownWhenProfileKnowledgeProviderIsNull()
        {
            var factory = new PubStructure.Factory(null, _packageFactory, _log, _directoryCreator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Factory_ArgumentNullExceptionThrownWhenPackageFactoryIsNull()
        {
            var factory = new PubStructure.Factory(_profileKnowledgeProvider, null, _log, _directoryCreator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Factory_ArgumentNullExceptionThrownWhenLogIsNull()
        {
            var factory = new PubStructure.Factory(_profileKnowledgeProvider, _packageFactory, null, _directoryCreator);
        }

        [TestMethod]
        [IntegrationTest]
        public void Factory_CreateRow_NonSlicedSnapshotElementWithShortDescriptionCreatesRowHtml()
        {
            var factory = new PubStructure.Factory(_profileKnowledgeProvider, _packageFactory, _log, _directoryCreator);

            var structureDefinition = new StructureDefinition();
            var snapshot = new StructureDefinition.SnapshotComponent();
            var snapshotElements = new List<ElementDefinition>();

            var element = new ElementDefinition();
            element.Max = "1";
            element.Path = "system.coding.code";
            element.Short = "this is a short description";

            var types = new List<ElementDefinition.TypeRefComponent>();
            var type = new ElementDefinition.TypeRefComponent();
            type.Code = FHIRDefinedType.Code;
            types.Add(type);
            element.Type = types;

            snapshotElements.Add(element);
            snapshot.Element = snapshotElements;
            structureDefinition.Snapshot = snapshot;
            structureDefinition.Name = "MyProfile";

            string actual = factory.GenerateStructure(structureDefinition, _isDifferential).ToString();
            var expected = Resources.NonSlicedSnapshotElementWithShortDescription;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [IntegrationTest]
        public void Factory_CreateRow_SlicedSnapshotElementWithShortDescriptionCreatesRowHtml()
        {
            var factory = new PubStructure.Factory(_profileKnowledgeProvider, _packageFactory, _log, _directoryCreator);

            var structureDefinition = new StructureDefinition();
            var snapshot = new StructureDefinition.SnapshotComponent();
            var snapshotElements = new List<ElementDefinition>();

            var element = new ElementDefinition();
            element.Max = "1";
            element.Path = "system.coding.code";
            element.Short = "this is a short description";

            var types = new List<ElementDefinition.TypeRefComponent>();
            var type = new ElementDefinition.TypeRefComponent();
            type.Code = FHIRDefinedType.Code;
            types.Add(type);
            element.Type = types;
            var slice = new ElementDefinition.SlicingComponent();
            slice.AddExtension("uri", new FhirString("SliceExtension"));
            slice.Rules = ElementDefinition.SlicingRules.Closed;
            slice.Ordered = false;
            element.Slicing = slice;

            snapshotElements.Add(element);
            snapshot.Element = snapshotElements;
            structureDefinition.Snapshot = snapshot;
            structureDefinition.Name = "MyProfile";

            string actual = factory.GenerateStructure(structureDefinition, _isDifferential).ToString();

            var expected = Resources.SlicedSnapshotElementWithShortDescription;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [IntegrationTest]
        public void Factory_CreateRow_ReferenceToParentSnapshotElementWithShortDescriptionCreatesRowHtml()
        {
            var factory = new PubStructure.Factory(_profileKnowledgeProvider, _packageFactory, _log, _directoryCreator);

            var structureDefinition = new StructureDefinition();
            var snapshot = new StructureDefinition.SnapshotComponent();
            var snapshotElements = new List<ElementDefinition>();

            var element = new ElementDefinition();
            element.Max = "1";
            element.Path = "system.coding.code";
            element.Short = "this is a short description";

            var types = new List<ElementDefinition.TypeRefComponent>();
            var type = new ElementDefinition.TypeRefComponent();
            type.Code = FHIRDefinedType.Code;
            types.Add(type);
            element.Type = types;

            var parentReference = new FhirString();
            parentReference.Value = FHIRDefinedType.Coding.ToString();
            element.NameReferenceElement = parentReference;

            snapshotElements.Add(element);
            snapshot.Element = snapshotElements;
            structureDefinition.Snapshot = snapshot;
            structureDefinition.Name = "MyProfile";

            string actual = factory.GenerateStructure(structureDefinition, _isDifferential).ToString();

            var expected = Resources.ReferenceToParentElementWithShortDescription;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [IntegrationTest]
        public void Factory_CreateRow_ExtensionSnapshotElementWithShortDescriptionCreatesRowHtml()
        {
            var factory = new PubStructure.Factory(_profileKnowledgeProvider, _packageFactory, _log, _directoryCreator);

            var structureDefinition = new StructureDefinition();
            var snapshot = new StructureDefinition.SnapshotComponent();
            var snapshotElements = new List<ElementDefinition>();

            var element = new ElementDefinition();
            element.Max = "1";
            element.Path = "system.coding.code.extension";
            element.Short = "this is a short description";

            var types = new List<ElementDefinition.TypeRefComponent>();
            var type = new ElementDefinition.TypeRefComponent();
            type.Code = FHIRDefinedType.Code;
            type.Profile = new[] { "http://fhir.nhs.net/StructureDefinition/extension" };
            types.Add(type);
            element.Type = types;
            element.Name = "CodeExtension";

            snapshotElements.Add(element);
            snapshot.Element = snapshotElements;
            structureDefinition.Snapshot = snapshot;
            structureDefinition.Name = "MyProfile";

            _packageFactory.ResourceStore.Add(new PackageResource("ProfileOne", element.Name, "http://fhir.nhs.net/StructureDefinition/extension", structureDefinition));

            string actual = factory.GenerateStructure(structureDefinition, _isDifferential).ToString();

            var expected = Resources.ExtensionSnaphotElementWithShortDescription;

            Assert.AreEqual(expected, actual);
        }
    }
}