using System;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PubSpec = Hl7.Fhir.Publication.Specification.Profile.ValueSet;

namespace Fhir.Publication.Tests.Specification.Profile.ValueSet
{
    [TestClass]
    public class CodeSystem
    {
        private const string _url = "http://fhir.nhs.net/ValueSet/administrative-gender-ddmap-1-0";
        private const string _name = "Administrative-Gender-DDMap-1-0";
        private const string _definition = "Here is a valueset description";
        private const string _copyright = "This is copyright!";
        private readonly Log _log;
        private readonly Hl7.Fhir.Model.ValueSet _valueset;
        private DateTimeOffset _offsetTime;

        public CodeSystem()
        {
            _log = new Log(new Mock.ErrorLogger());
            _valueset = new Hl7.Fhir.Model.ValueSet();
            SetValueset();
        }

        private void SetValueset()
        {
            _valueset.Name = _name;
            _valueset.Url = _url;
            _valueset.Description = _definition;
            _valueset.Copyright = _copyright;
            _valueset.Status = ConformanceResourceStatus.Active;
            var date = new DateTime(2016, 9, 2, 9, 25, 02);
            _offsetTime = new DateTimeOffset(date, new TimeSpan());
            _valueset.Meta = new Meta();
            _valueset.Meta.LastUpdated = _offsetTime;

            var oid = string.Concat(Urn.Oid.GetUrnString(), "2.16.840.1.113883.2.1.3.2.4.16.25");
            var extensions = new System.Collections.Generic.List<Extension>();
            var extension = new Extension("url", new FhirUri(oid));
            extensions.Add(extension);
            _valueset.Extension = extensions;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CodeSystem_Definition_ArgumentNullExceptionThownWhenValueSetIsNull()
        {
            var codesystem = new PubSpec.CodeSystem(null, _log);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CodeSystem_Definition_ArgumentNullExceptionThownWhenLogIsNull()
        {
            var valueset = new Hl7.Fhir.Model.ValueSet();
            var codesystem = new PubSpec.CodeSystem(valueset, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CodeSystem_InvalidOperationExceptionThrownWhenValuesetCodeSystemIsNull()
        {
            var codesystem = new PubSpec.CodeSystem(_valueset, _log);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CodeSystem_InvalidOperationExceptionThrownWhenValuesetCodeSystemConceptIsNull()
        {
            _valueset.CodeSystem = new Hl7.Fhir.Model.ValueSet.CodeSystemComponent();

            var codesystem = new PubSpec.CodeSystem(_valueset, _log);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CodeSystem_InvalidOperationExceptionThrownWhenValuesetCodeSystemSystemIsNull()
        {
            _valueset.CodeSystem = new Hl7.Fhir.Model.ValueSet.CodeSystemComponent();
            _valueset.CodeSystem.Concept = new System.Collections.Generic.List<Hl7.Fhir.Model.ValueSet.ConceptDefinitionComponent>();
            _valueset.CodeSystem.Concept.Add(new Hl7.Fhir.Model.ValueSet.ConceptDefinitionComponent());
            _valueset.CodeSystem.System = null;

            var codesystem = new PubSpec.CodeSystem(_valueset, _log);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CodeSystem_InvalidOperationExceptionThrownWhenValuesetCodeSystemSystemIsEmpty()
        {
            _valueset.CodeSystem = new Hl7.Fhir.Model.ValueSet.CodeSystemComponent();
            _valueset.CodeSystem.Concept = new System.Collections.Generic.List<Hl7.Fhir.Model.ValueSet.ConceptDefinitionComponent>();
            _valueset.CodeSystem.Concept.Add(new Hl7.Fhir.Model.ValueSet.ConceptDefinitionComponent());
            _valueset.CodeSystem.System = string.Empty;

            var codesystem = new PubSpec.CodeSystem(_valueset, _log);
        }

        [TestMethod]
        [IntegrationTest]
        public void CodeSystem_Description_CodeSystemTableHasNoDefinitionColumn()
        {
            _valueset.CodeSystem = new Hl7.Fhir.Model.ValueSet.CodeSystemComponent();
            _valueset.CodeSystem.Concept = new System.Collections.Generic.List<Hl7.Fhir.Model.ValueSet.ConceptDefinitionComponent>();

            var concept1 = new Hl7.Fhir.Model.ValueSet.ConceptDefinitionComponent();
            concept1.Code = "1";
            concept1.Display = "Male";

            var concept2 = new Hl7.Fhir.Model.ValueSet.ConceptDefinitionComponent();
            concept2.Code = "2";
            concept2.Display = "Female";

            _valueset.CodeSystem.Concept.Add(concept1);
            _valueset.CodeSystem.Concept.Add(concept2);

            _valueset.CodeSystem.System = "http://fhir.nhs.net/ValueSet/administrative-gender-ddmap-1-0";

            var codesystem = new PubSpec.CodeSystem(_valueset, _log);
            string actual = codesystem.Description.ToString();

            Assert.AreEqual(Resources.ValueSetCodeSystemNoDefinition, actual);
        }

        [TestMethod]
        [IntegrationTest]
        public void CodeSystem_Description_CodeSystemTableHasNamedisplayAndDefinitionColumns()
        {
            _valueset.CodeSystem = new Hl7.Fhir.Model.ValueSet.CodeSystemComponent();
            _valueset.CodeSystem.Concept = new System.Collections.Generic.List<Hl7.Fhir.Model.ValueSet.ConceptDefinitionComponent>();

            var concept1 = new Hl7.Fhir.Model.ValueSet.ConceptDefinitionComponent();
            concept1.Code = "1";
            concept1.Display = "Male";
            concept1.Definition = "Code denoting male gender";

            var concept2 = new Hl7.Fhir.Model.ValueSet.ConceptDefinitionComponent();
            concept2.Code = "2";
            concept2.Display = "Female";
            concept2.Definition = "Code denoting female gender";

            _valueset.CodeSystem.Concept.Add(concept1);
            _valueset.CodeSystem.Concept.Add(concept2);

            _valueset.CodeSystem.System = "http://fhir.nhs.net/ValueSet/administrative-gender-ddmap-1-0";

            var codesystem = new PubSpec.CodeSystem(_valueset, _log);
            string actual = codesystem.Description.ToString();

            Assert.AreEqual(Resources.ValueSetcodeSystemTable, actual);
        }
    }
}