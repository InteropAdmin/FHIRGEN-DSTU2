using System;
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
    public class SourceValueset
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SourceValueset_SourceValueset_InvalidOperationExceptionThrownWhenValuesetHasNoConceptMaps()
        {
            var valueset = new Model.ValueSet();
            valueset.CodeSystem = new Model.ValueSet.CodeSystemComponent();
            var source = new PubSpec.Mapping.SourceValueset(valueset.Contained, valueset.CodeSystem);
        }
    }
}