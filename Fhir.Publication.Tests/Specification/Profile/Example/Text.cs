using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;
using Generator = Hl7.Fhir.Publication.Specification.Profile.Example;

namespace Fhir.Publication.Tests.Specification.Profile.Example
{
    [TestClass]
    public class Text
    {
        private readonly Hl7.Fhir.Model.StructureDefinition _structureDefinitionWithExample;
        private readonly Hl7.Fhir.Model.StructureDefinition _structureDefinitionWithoutExample;


        public Text()
        {
            _structureDefinitionWithExample = new Hl7.Fhir.Model.StructureDefinition();
            _structureDefinitionWithExample = CreateStructureDefinition("Appointment Example");

            _structureDefinitionWithoutExample = new Hl7.Fhir.Model.StructureDefinition();
            _structureDefinitionWithoutExample = CreateStructureDefinition(string.Empty);
        }

        private static Hl7.Fhir.Model.StructureDefinition CreateStructureDefinition(string exampleName)
        {
            var definition = new Hl7.Fhir.Model.StructureDefinition();
            definition.Name = "MyElement";
            var codings = new System.Collections.Generic.List<Coding>();

            var coding = new Coding(system: Urn.Example.GetUrnString(), code: exampleName);
            coding.Display = exampleName;
            codings.Add(coding);

            definition.Meta = new Meta();
            definition.Meta.Tag = codings;

            var uri = new FhirUri();
            uri.Value = "elementValue/MyElement";
            definition.BaseElement = uri;

            return definition;
        }

        [TestMethod]
        public void GetText_NoLinkGeneratedWhenStructureDefinitionHasNoExamples()
        {
            string actual = Hl7.Fhir.Publication.Specification.Profile.Example.Text.GetText(_structureDefinitionWithoutExample.Name, _structureDefinitionWithoutExample.GetExampleName());

            const string expected = "<p>Currently there are no examples for this resource</p>";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetText_LinkGeneratedWhenStructureDefinitionHasExamples()
        {
            string actual = Hl7.Fhir.Publication.Specification.Profile.Example.Text.GetText(_structureDefinitionWithExample.Name, _structureDefinitionWithExample.GetExampleName());

            const string expected = @"<div class='well well-sm'>Follow this link to view examples for MyElement: <a href=""Examples.html#MyElement"">Examples</a></div>";

            Assert.AreEqual(expected, actual);
        }
    }
}
