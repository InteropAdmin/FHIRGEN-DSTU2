using System;
using System.Linq;
using System.Collections.Generic;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.ImplementationGuide;
using Hl7.Fhir.Publication.Specification.Profile.ValueSet.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PubSpec = Hl7.Fhir.Publication.Specification.Profile.ValueSet;
using Model = Hl7.Fhir.Model;

namespace Fhir.Publication.Tests.Specification.Profile.ValueSet.Mapping
{
    [TestClass]
    public class ConceptMapper
    {
        private const string _package = "ProfileOne";
        private readonly ResourceStore _resourceStore;
        private readonly ISourceValueset _sourceValueset;
        private readonly string _targetReference;
        private readonly string _resourceName;
        private readonly Model.ValueSet _valueset;
        private readonly Log _log;
        private readonly Model.ValueSet.CodeSystemComponent _valuesetCodeSystem;

        public ConceptMapper()
        {
            _log = new Log(new Mock.ErrorLogger());
            _resourceStore = new ResourceStore(_log);

            var conceptMap = new Model.ConceptMap();
            _targetReference = "http://fhir.nhs.net/ValueSet/administrative-gender-ddmap-1-0";
            _resourceName = "administrative-gender-ddmap-1-0";
            _valueset = new Model.ValueSet();
            _valueset.Name = "administrative-gender-1-0";
            _valuesetCodeSystem = new Model.ValueSet.CodeSystemComponent();

            var codeSystemConcepts = new List<Model.ValueSet.ConceptDefinitionComponent>();
            var codeSystemConcept = new Model.ValueSet.ConceptDefinitionComponent();
            codeSystemConcept.Code = "male";
            codeSystemConcept.Display = "Male";
            codeSystemConcept.Definition = "Gender is male.";
            codeSystemConcepts.Add(codeSystemConcept);
            _valuesetCodeSystem.Concept = codeSystemConcepts;
            _valueset.CodeSystem = _valuesetCodeSystem;


            var targetCodeSystem = new Model.ValueSet.CodeSystemComponent();
            var targetCodeSystemConcepts = new List<Model.ValueSet.ConceptDefinitionComponent>();
            var targetCodeSystemConcept = new Model.ValueSet.ConceptDefinitionComponent();
            targetCodeSystemConcept.Code = "1";
            targetCodeSystemConcept.Display = "Male";
            targetCodeSystemConcepts.Add(targetCodeSystemConcept);
            targetCodeSystem.Concept = targetCodeSystemConcepts;

            var targetValueset = new Model.ValueSet();
            targetValueset.CodeSystem = targetCodeSystem;

            _resourceStore.Add(new PackageResource(_package, _resourceName, _targetReference, targetValueset));
            _sourceValueset = new Mock.Source(conceptMap, _valuesetCodeSystem, _targetReference, _valueset.Name); 
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConceptMapper_ConceptMapper_ArgumentNullExceptionThrownWhenResourceStoreIsNull()
        {
            var mapper = new PubSpec.Mapping.ConceptMapper(null, _sourceValueset, _valueset.Name, _package, _log);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConceptMapper_ConceptMapper_ArgumentNullExceptionThrownWhenSourceIsNull()
        {
            var mapper = new PubSpec.Mapping.ConceptMapper(_resourceStore, null, _valueset.Name, _package, _log);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConceptMapper_ConceptMapper_ArgumentExceptionThrownWhenValuesetNameIsNull()
        {
            var mapper = new PubSpec.Mapping.ConceptMapper(_resourceStore, _sourceValueset, null, _package, _log);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConceptMapper_ConceptMapper_ArgumentExceptionThrownWhenValuesetNameIsEmpty()
        {
            var mapper = new PubSpec.Mapping.ConceptMapper(_resourceStore, _sourceValueset, string.Empty, _package, _log);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConceptMapper_ConceptMapper_ArgumentNullExceptionThrownWhenLogIsNull()
        {
            var mapper = new PubSpec.Mapping.ConceptMapper(_resourceStore, _sourceValueset, _valueset.Name, _package, null);
        }

       [TestMethod]
        public void ConceptMapper_MapResources_SourceConceptMapWithOneConceptMapsToTargetConceptMapWIthOneConcept()
        {
            var conceptMap = new Model.ConceptMap();
            var elements = new List<Model.ConceptMap.SourceElementComponent>();
            var element = new Model.ConceptMap.SourceElementComponent();
            var targets = new List<Model.ConceptMap.TargetElementComponent>();
            var target = new Model.ConceptMap.TargetElementComponent();

            element.Code = "male";
            target.Code = "1"; 
            target.Equivalence = Model.ConceptMap.ConceptMapEquivalence.Equivalent;
            targets.Add(target);
            element.Target = targets;
            elements.Add(element);
            conceptMap.Element = elements;
            
            var sourceValuest = new Mock.Source(conceptMap, _valuesetCodeSystem, _targetReference, _resourceName);
            var mapper = new PubSpec.Mapping.ConceptMapper(_resourceStore, sourceValuest, _valueset.Name, _package, _log);
            List<CodeMapping> mappedResources = mapper.MapResources().ToList();

            Assert.AreEqual("male", mappedResources[0].Code);
            Assert.AreEqual("Male", mappedResources[0].Display);
            Assert.AreEqual("Male", mappedResources[0].Mapping);
            Assert.AreEqual("Gender is male.", mappedResources[0].Definition);
            Assert.AreEqual(Model.ConceptMap.ConceptMapEquivalence.Equivalent, mappedResources[0].Equivalence);
        }

        [TestMethod]
        public void ConceptMapper_MapResources_SourceConceptMapWithTwoConceptsToTargetConceptMapWIthOneConcept()
        {
            var conceptMap = new Model.ConceptMap();
            var elements = new List<Model.ConceptMap.SourceElementComponent>();
            var element1 = new Model.ConceptMap.SourceElementComponent();
            var element2 = new Model.ConceptMap.SourceElementComponent();
            var targets1 = new List<Model.ConceptMap.TargetElementComponent>();
            var targets2 = new List<Model.ConceptMap.TargetElementComponent>();
            var target1 = new Model.ConceptMap.TargetElementComponent();
            var target2 = new Model.ConceptMap.TargetElementComponent();

            element1.Code = "male";
            target1.Code = "1";
            target1.Equivalence = Model.ConceptMap.ConceptMapEquivalence.Equivalent;
            targets1.Add(target1);
            element1.Target = targets1;

            element2.Code = "female";
            target2.Code = "2";
            target2.Equivalence = Model.ConceptMap.ConceptMapEquivalence.Unmatched;
            targets2.Add(target2);
            element2.Target = targets2;

            elements.Add(element1);
            elements.Add(element2);
            conceptMap.Element = elements;

            var valuesetCodeSystem = new Model.ValueSet.CodeSystemComponent();
            var codeSystemConcepts = new List<Model.ValueSet.ConceptDefinitionComponent>();
            var codeSystemConcept1 = new Model.ValueSet.ConceptDefinitionComponent();
            var codeSystemConcept2 = new Model.ValueSet.ConceptDefinitionComponent();
            codeSystemConcept1.Code = "male";
            codeSystemConcept1.Display = "Male";
            codeSystemConcept1.Definition = "Gender is male.";
            codeSystemConcept2.Code = "female";
            codeSystemConcept2.Display = "Female";
            codeSystemConcept2.Definition = "Gender is female.";

            codeSystemConcepts.Add(codeSystemConcept1);
            codeSystemConcepts.Add(codeSystemConcept2);
            valuesetCodeSystem.Concept = codeSystemConcepts;
           
            var sourceValuest = new Mock.Source(conceptMap, valuesetCodeSystem, _targetReference, _resourceName);
            var mapper = new PubSpec.Mapping.ConceptMapper(_resourceStore, sourceValuest, _valueset.Name, _package, _log);
            List<CodeMapping> mappedResources = mapper.MapResources().ToList();

            Assert.AreEqual("male", mappedResources[0].Code);
            Assert.AreEqual("Male", mappedResources[0].Display);
            Assert.AreEqual("Male", mappedResources[0].Mapping);
            Assert.AreEqual("Gender is male.", mappedResources[0].Definition);
            Assert.AreEqual(Model.ConceptMap.ConceptMapEquivalence.Equivalent, mappedResources[0].Equivalence);

            Assert.AreEqual("female", mappedResources[1].Code);
            Assert.AreEqual("Female", mappedResources[1].Display);
            Assert.AreEqual(string.Empty, mappedResources[1].Mapping);
            Assert.AreEqual("Gender is female.", mappedResources[1].Definition);
            Assert.AreEqual(Model.ConceptMap.ConceptMapEquivalence.Unmatched, mappedResources[1].Equivalence);
        }
    }
}