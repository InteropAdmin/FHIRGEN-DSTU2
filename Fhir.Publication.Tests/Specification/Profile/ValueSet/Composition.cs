using System;
using System.Collections.Generic;
using Hl7.Fhir.Publication.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PubSpec = Hl7.Fhir.Publication.Specification.Profile.ValueSet;
using Model = Hl7.Fhir.Model;

namespace Fhir.Publication.Tests.Specification.Profile.ValueSet
{
    [TestClass]
    public class Composition
    {
        private readonly Log _log;
        private readonly Model.ValueSet _valueset;

        public Composition()
        {
            _log = new Log(new Mock.ErrorLogger());
            _valueset = new Model.ValueSet();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Composition_Definition_ArgumentNullExceptionThownWhenValueSetIsNull()
        {
            var composition = new PubSpec.Composition(null, _log);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Composition_Definition_ArgumentNullExceptionThownWhenLogIsNull()
        {
            var valueset = new Model.ValueSet();
            var composition = new PubSpec.Composition(valueset, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Composition_Description_InvalidOperationExceptionThrownWhenCompositionIsNull()
        {
            var composition = new PubSpec.Composition(_valueset, _log);
            _valueset.Compose = null;

            var actual = composition.Description;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Composition_Description_InvalidOperationExceptionThrownWhenIncludeSystemIsNull()
        {
            var compose = new Model.ValueSet.ComposeComponent();
            compose.Include = new List<Model.ValueSet.ConceptSetComponent>();
            var include = new Model.ValueSet.ConceptSetComponent();
            include.System = null;

            include.Filter = new List<Model.ValueSet.FilterComponent>();
            var filter = new Model.ValueSet.FilterComponent();
            filter.Property = "concept";
            filter.Op = Model.ValueSet.FilterOperator.In;
            filter.Value = "1391000000139";

            include.Filter.Add(filter);
            compose.Include.Add(include);
            _valueset.Compose = compose;

            var composition = new PubSpec.Composition(_valueset, _log);

            var actual = composition.Description;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Composition_Description_InvalidOperationExceptionThrownWhenIncludeSystemIsEmpty()
        {
            var compose = new Model.ValueSet.ComposeComponent();
            compose.Include = new List<Model.ValueSet.ConceptSetComponent>();
            var include = new Model.ValueSet.ConceptSetComponent();
            include.System = string.Empty;

            include.Filter = new List<Model.ValueSet.FilterComponent>();
            var filter = new Model.ValueSet.FilterComponent();
            filter.Property = "concept";
            filter.Op = Model.ValueSet.FilterOperator.In;
            filter.Value = "1391000000139";

            include.Filter.Add(filter);
            compose.Include.Add(include);
            _valueset.Compose = compose;

            var composition = new PubSpec.Composition(_valueset, _log);

            var actual = composition.Description;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Composition_Description_InvalidOperationExceptionThrownWhenIncludeFilterPropertyIsNull()
        {
            var compose = new Model.ValueSet.ComposeComponent();
            compose.Include = new List<Model.ValueSet.ConceptSetComponent>();
            var include = new Model.ValueSet.ConceptSetComponent();
            include.System = "http://snomed.info/sct";

            include.Filter = new List<Model.ValueSet.FilterComponent>();
            var filter = new Model.ValueSet.FilterComponent();
            filter.Property = null;
            filter.Op = Model.ValueSet.FilterOperator.In;
            filter.Value = "1391000000139";

            include.Filter.Add(filter);
            compose.Include.Add(include);
            _valueset.Compose = compose;

            var composition = new PubSpec.Composition(_valueset, _log);

            var actual = composition.Description;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Composition_Description_InvalidOperationExceptionThrownWhenIncludeFilterPropertyIsEmpty()
        {
            var compose = new Model.ValueSet.ComposeComponent();
            compose.Include = new List<Model.ValueSet.ConceptSetComponent>();
            var include = new Model.ValueSet.ConceptSetComponent();
            include.System = "http://snomed.info/sct";

            include.Filter = new List<Model.ValueSet.FilterComponent>();
            var filter = new Model.ValueSet.FilterComponent();
            filter.Property = string.Empty;
            filter.Op = Model.ValueSet.FilterOperator.In;
            filter.Value = "1391000000139";

            include.Filter.Add(filter);
            compose.Include.Add(include);
            _valueset.Compose = compose;

            var composition = new PubSpec.Composition(_valueset, _log);

            var actual = composition.Description;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Composition_Description_InvalidOperationExceptionThrownWhenIncludeFilterOperatorIsNull()
        {
            var compose = new Model.ValueSet.ComposeComponent();
            compose.Include = new List<Model.ValueSet.ConceptSetComponent>();
            var include = new Model.ValueSet.ConceptSetComponent();
            include.System = "http://snomed.info/sct";

            include.Filter = new List<Model.ValueSet.FilterComponent>();
            var filter = new Model.ValueSet.FilterComponent();
            filter.Property = "concept";
            filter.Op = null;
            filter.Value = "1391000000139";

            include.Filter.Add(filter);
            compose.Include.Add(include);
            _valueset.Compose = compose;

            var composition = new PubSpec.Composition(_valueset, _log);

            var actual = composition.Description;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Composition_Description_InvalidOperationExceptionThrownWhenIncludeFilterValueIsNull()
        {
            var compose = new Model.ValueSet.ComposeComponent();
            compose.Include = new List<Model.ValueSet.ConceptSetComponent>();
            var include = new Model.ValueSet.ConceptSetComponent();
            include.System = "http://snomed.info/sct";

            include.Filter = new List<Model.ValueSet.FilterComponent>();
            var filter = new Model.ValueSet.FilterComponent();
            filter.Property = "concept";
            filter.Op = Model.ValueSet.FilterOperator.In;
            filter.Value = null;

            include.Filter.Add(filter);
            compose.Include.Add(include);
            _valueset.Compose = compose;

            var composition = new PubSpec.Composition(_valueset, _log);

            var actual = composition.Description;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Composition_Description_InvalidOperationExceptionThrownWhenIncludeFilterValueIsEmpty()
        {
            var compose = new Model.ValueSet.ComposeComponent();
            compose.Include = new List<Model.ValueSet.ConceptSetComponent>();
            var include = new Model.ValueSet.ConceptSetComponent();
            include.System = "http://snomed.info/sct";

            include.Filter = new List<Model.ValueSet.FilterComponent>();
            var filter = new Model.ValueSet.FilterComponent();
            filter.Property = "concept";
            filter.Op = Model.ValueSet.FilterOperator.In;
            filter.Value = string.Empty;

            include.Filter.Add(filter);
            compose.Include.Add(include);
            _valueset.Compose = compose;

            var composition = new PubSpec.Composition(_valueset, _log);

            var actual = composition.Description;
        }

        [TestMethod]
        [IntegrationTest]
        public void Composition_Description_BulletListCreatedWhenIncludeHasFilter()
        {
            var compose = new Model.ValueSet.ComposeComponent();
            compose.Include = new List<Model.ValueSet.ConceptSetComponent>();
            var include = new Model.ValueSet.ConceptSetComponent();
            include.System = "http://snomed.info/sct";

            include.Filter = new List<Model.ValueSet.FilterComponent>();
            var filter = new Model.ValueSet.FilterComponent();
            filter.Property = "concept";
            filter.Op = Model.ValueSet.FilterOperator.In;
            filter.Value = "1391000000139";

            include.Filter.Add(filter);
            compose.Include.Add(include);
            _valueset.Compose = compose;

            var composition = new PubSpec.Composition(_valueset, _log);

            string actual = composition.Description.ToString();
            var expected = Resources.BulletListForCompositionWithFilter;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Composition_Description_InvalidOperationExceptionThrownWhenIncludeConceptCodeIsNull()
        {
            var compose = new Model.ValueSet.ComposeComponent();
            compose.Include = new List<Model.ValueSet.ConceptSetComponent>();
            var include = new Model.ValueSet.ConceptSetComponent();
            include.System = "http://snomed.info/sct";

            include.Concept = new List<Model.ValueSet.ConceptReferenceComponent>();
            var concept = new Model.ValueSet.ConceptReferenceComponent();
            concept.Code = null;
            concept.Display = "record extract (record artifact)";

            include.Concept.Add(concept);
            compose.Include.Add(include);
            _valueset.Compose = compose;

            var composition = new PubSpec.Composition(_valueset, _log);

            var actual = composition.Description;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Composition_Description_InvalidOperationExceptionThrownWhenIncludeConceptCodeIsEmpty()
        {
            var compose = new Model.ValueSet.ComposeComponent();
            compose.Include = new List<Model.ValueSet.ConceptSetComponent>();
            var include = new Model.ValueSet.ConceptSetComponent();
            include.System = "http://snomed.info/sct";

            include.Concept = new List<Model.ValueSet.ConceptReferenceComponent>();
            var concept = new Model.ValueSet.ConceptReferenceComponent();
            concept.Code = string.Empty;
            concept.Display = "record extract (record artifact)";

            include.Concept.Add(concept);
            compose.Include.Add(include);
            _valueset.Compose = compose;

            var composition = new PubSpec.Composition(_valueset, _log);

            var actual = composition.Description;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Composition_Description_InvalidOperationExceptionThrownWhenIncludeConceptDisplayIsNull()
        {
            var compose = new Model.ValueSet.ComposeComponent();
            compose.Include = new List<Model.ValueSet.ConceptSetComponent>();
            var include = new Model.ValueSet.ConceptSetComponent();
            include.System = "http://snomed.info/sct";

            include.Concept = new List<Model.ValueSet.ConceptReferenceComponent>();
            var concept = new Model.ValueSet.ConceptReferenceComponent();
            concept.Code = "425173008";
            concept.Display = null;

            include.Concept.Add(concept);
            compose.Include.Add(include);
            _valueset.Compose = compose;

            var composition = new PubSpec.Composition(_valueset, _log);

            var actual = composition.Description;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Composition_Description_InvalidOperationExceptionThrownWhenIncludeConceptDisplayIsEmpty()
        {
            var compose = new Model.ValueSet.ComposeComponent();
            compose.Include = new List<Model.ValueSet.ConceptSetComponent>();
            var include = new Model.ValueSet.ConceptSetComponent();
            include.System = "http://snomed.info/sct";

            include.Concept = new List<Model.ValueSet.ConceptReferenceComponent>();
            var concept = new Model.ValueSet.ConceptReferenceComponent();
            concept.Code = "425173008";
            concept.Display = string.Empty;

            include.Concept.Add(concept);
            compose.Include.Add(include);
            _valueset.Compose = compose;

            var composition = new PubSpec.Composition(_valueset, _log);

            var actual = composition.Description;
        }

        [TestMethod]
        [IntegrationTest]
        public void Composition_Description_BulletListCreatedWhenIncludeHasConcept()
        {
            var compose = new Model.ValueSet.ComposeComponent();
            compose.Include = new List<Model.ValueSet.ConceptSetComponent>();
            var include = new Model.ValueSet.ConceptSetComponent();
            include.System = "http://snomed.info/sct";

            include.Concept = new List<Model.ValueSet.ConceptReferenceComponent>();
            var concept = new Model.ValueSet.ConceptReferenceComponent();
            concept.Code = "425173008";
            concept.Display = "record extract (record artifact)";

            include.Concept.Add(concept);
            compose.Include.Add(include);
            _valueset.Compose = compose;

            var composition = new PubSpec.Composition(_valueset, _log);

            var actual = composition.Description.ToString();
            string expected = Resources.BulletListForCompositionWithConcept;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [IntegrationTest]
        public void Composition_Description_BulletListCreatedWhenComposeHasImport()
        {
            var compose = new Model.ValueSet.ComposeComponent();
            _valueset.Compose = compose;

            var imports = new List<string>();
            imports.Add("http://fhir.nhs.net/ValueSet/sds-job-role-name-1-0");
            compose.Import = imports;
            
            var composition = new PubSpec.Composition(_valueset, _log);

            var actual = composition.Description.ToString();
            string expected = Resources.BulletListforComposeWithImport;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [IntegrationTest]
        public void Composition_Description_BulletListsCreatedWhenComposeHasImportsIncludeConceptsAndFilters()
        {
            var compose = new Model.ValueSet.ComposeComponent();
            _valueset.Compose = compose;

            var imports = new List<string>();
            imports.Add("http://fhir.nhs.net/ValueSet/sds-job-role-name-1-0");
            compose.Import = imports;

            compose.Include = new List<Model.ValueSet.ConceptSetComponent>();
            var include = new Model.ValueSet.ConceptSetComponent();
            include.System = "http://snomed.info/sct";

            include.Concept = new List<Model.ValueSet.ConceptReferenceComponent>();
            var concept = new Model.ValueSet.ConceptReferenceComponent();
            concept.Code = "425173008";
            concept.Display = "record extract (record artifact)";

            include.Filter = new List<Model.ValueSet.FilterComponent>();
            var filter = new Model.ValueSet.FilterComponent();
            filter.Property = "concept";
            filter.Op = Model.ValueSet.FilterOperator.In;
            filter.Value = "1391000000139";

            include.Filter.Add(filter);
            include.Concept.Add(concept);
            compose.Include.Add(include);
            _valueset.Compose = compose;

            var composition = new PubSpec.Composition(_valueset, _log);

            var actual = composition.Description.ToString();
            string expected = Resources.BulletListsForComposeWithImportConceptAndFilter;

            Assert.AreEqual(expected, actual);
        }
    }
}