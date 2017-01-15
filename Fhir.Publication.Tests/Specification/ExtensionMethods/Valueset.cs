using System.Collections.Generic;
using Hl7.Fhir.Publication.Specification.ExtensionMethods;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using fhirModel = Hl7.Fhir.Model;

namespace Fhir.Publication.Tests.Specification.ExtensionMethods
{    
    [TestClass]
    public class Valueset
    {
        [TestMethod]
        public void Valueset_IsComposition_NullCompositionReturnsFalse()
        {
            var valueset = new fhirModel.ValueSet();

            Assert.IsFalse(valueset.IsComposition());
        }

        [TestMethod]
        public void Valueset_IsComposition_HasCompositionIncludesReturnsTrue()
        {
            var valueset = new fhirModel.ValueSet();
            var composition = new fhirModel.ValueSet.ComposeComponent();
            var concepts = new List<fhirModel.ValueSet.ConceptSetComponent>();
            concepts.Add(new fhirModel.ValueSet.ConceptSetComponent());

            composition.Include = concepts;
            valueset.Compose = composition;

            Assert.IsTrue(valueset.IsComposition());
        }

        [TestMethod]
        public void Valueset_IsComposition_HasCompositionImportsReturnsTrue()
        {
            var valueset = new fhirModel.ValueSet();
            var composition = new fhirModel.ValueSet.ComposeComponent();
            var concepts = new List<string>();
            concepts.Add("myUri");

            composition.Import = concepts;
            valueset.Compose = composition;

            Assert.IsTrue(valueset.IsComposition());
        }

        [TestMethod]
        public void Valueset_IsComposition_HasCompositionImportsAndIncludesReturnsTrue()
        {
            var valueset = new fhirModel.ValueSet();
            var composition = new fhirModel.ValueSet.ComposeComponent();
            var uris = new List<string>();
            uris.Add("myUri");
            composition.Import = uris;

            var concepts = new List<fhirModel.ValueSet.ConceptSetComponent>();
            concepts.Add(new fhirModel.ValueSet.ConceptSetComponent());

            composition.Include = concepts;
            valueset.Compose = composition;

            Assert.IsTrue(valueset.IsComposition());
        }

        [TestMethod]
        public void Valueset_IsCodeSystem_NullCodeSystemReturnsFalse()
        {
            var valueset = new fhirModel.ValueSet();

            Assert.IsFalse(valueset.IsCodesystem());
        }

        [TestMethod]
        public void Valueset_IsCodeSystem_HasCodeSystemAndCodeSystemConceptsReturnsTrue()
        {
            var valueset = new fhirModel.ValueSet();
            var codeSystem = new fhirModel.ValueSet.CodeSystemComponent();
            var concepts = new List<fhirModel.ValueSet.ConceptDefinitionComponent>();
            concepts.Add(new fhirModel.ValueSet.ConceptDefinitionComponent());
            codeSystem.Concept = concepts;
            valueset.CodeSystem = codeSystem;
            
            Assert.IsTrue(valueset.IsCodesystem());
        }
    }
}