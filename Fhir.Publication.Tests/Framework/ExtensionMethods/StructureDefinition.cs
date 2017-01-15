using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;

namespace Fhir.Publication.Tests.Framework.ExtensionMethods
{
    [TestClass]
    public class StructureDefinition
    {
        private const bool _ignoreCase = true;
        private readonly Hl7.Fhir.Model.StructureDefinition _structureDefinition;
        private readonly Hl7.Fhir.Model.StructureDefinition _structureDefinitionWithExample;
        private readonly Hl7.Fhir.Model.StructureDefinition _structureDefinitionWithoutExample;
        private readonly Hl7.Fhir.Model.StructureDefinition _structureDefinitionWithModifierExtension;

        public StructureDefinition()
        {
            _structureDefinition = CreateStructureDefinition(string.Empty);
            _structureDefinitionWithExample = CreateStructureDefinition("Appointment Example");
            _structureDefinitionWithoutExample = CreateStructureDefinition(string.Empty);
            _structureDefinitionWithModifierExtension = CreateStructureDefinition(string.Empty);
        }

        private static Hl7.Fhir.Model.StructureDefinition CreateStructureDefinition(string exampleName)
        {
            var definition = new Hl7.Fhir.Model.StructureDefinition();

            var codings = new List<Coding>();

            var coding = new Coding(system: Urn.Example.GetUrnString(), code: exampleName);
            coding.Display = exampleName;
            codings.Add(coding);

            definition.Meta = new Meta();
            definition.Meta.Tag = codings;

           return definition;
        }

        [TestMethod]
        public void StructureDefinition_GetExampleName_NameReturned()
        {
            const string expectedName = "Appointment Example";
         
            string actualName = _structureDefinitionWithExample.GetExampleName();

            Assert.AreEqual(expectedName, actualName, _ignoreCase, $"actual name is: {actualName}");
        }

        [TestMethod]
        public void StructureDefinition_GetExampleName_NameNotReturned()
        {
            string expectedname = string.Empty;

            string actualName = _structureDefinitionWithoutExample.GetExampleName();

            Assert.AreEqual(expectedname, actualName, _ignoreCase, $"actual name is: {actualName}");
        }

        [TestMethod]
        public void StructureDefinition_HasExtension_IsTrue()
        {
            var snapshot = new Hl7.Fhir.Model.StructureDefinition.SnapshotComponent();
            var element = new List<ElementDefinition>();
            var elementDefinition = new ElementDefinition();

            elementDefinition.Path = Hl7.Fhir.Model.StructureDefinition.ExtensionContext.Extension.ToString();
            element.Add(elementDefinition);
            snapshot.Element = element;
            _structureDefinitionWithExample.Snapshot = snapshot;
            
            Assert.IsTrue(
                _structureDefinitionWithExample.IsExtension(),
                "extension not found");
        }

        [TestMethod]
        public void StructureDefinition_HasExtension_IsFalse()
        {
            Assert.IsFalse(
                _structureDefinitionWithoutExample.IsExtension(),
                "extension has been found");
        }

        [TestMethod]
        public void StructureDefintion_IsModifierExtension_IsTrue()
        {
            var snapshot = new Hl7.Fhir.Model.StructureDefinition.SnapshotComponent();
            var elements = new List<ElementDefinition>();
            var element = new ElementDefinition();

            element.Path = Hl7.Fhir.Model.StructureDefinition.ExtensionContext.Extension.ToString();
            element.IsModifier = true;
            elements.Add(element);
            snapshot.Element = elements;
            _structureDefinitionWithModifierExtension.Snapshot = snapshot;
            
            Assert.IsTrue(_structureDefinitionWithModifierExtension.IsModifierExtension());
        }

        [TestMethod]
        public void StructureDefintion_IsModifierExtension_IsFalse()
        {
            var snapshot = new Hl7.Fhir.Model.StructureDefinition.SnapshotComponent();
            var elements = new List<ElementDefinition>();
            var element = new ElementDefinition();

            element.Path = Hl7.Fhir.Model.StructureDefinition.ExtensionContext.Extension.ToString();
            elements.Add(element);
            snapshot.Element = elements;
            _structureDefinitionWithModifierExtension.Snapshot = snapshot;

            Assert.IsFalse(_structureDefinitionWithModifierExtension.IsModifierExtension());
        }

        [TestMethod]
        public void StructureDefinition_IsSnaphsot_IsTrue()
        {
            var snapshot = new Hl7.Fhir.Model.StructureDefinition.SnapshotComponent();

            _structureDefinition.Snapshot = snapshot;

            Assert.IsTrue(_structureDefinition.IsSnapshot());
        }

        [TestMethod]
        public void StructureDefinition_IsSnaphsot_IsFalse()
        {
            Assert.IsFalse(_structureDefinition.IsSnapshot());
        }
    }
}