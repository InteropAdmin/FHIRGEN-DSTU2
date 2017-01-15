using System;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.ImplementationGuide;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dict = Hl7.Fhir.Publication.Specification.Profile.Structure.Dictionary;

namespace Fhir.Publication.Tests.Specification.Profile.Structure.Dictionary
{
    [TestClass]
    public class Binding
    {
        private const string _package = "ProfileOne";
        private readonly ElementDefinition.BindingComponent _binding;
        private readonly ResourceStore _store;

        public Binding()
        {
            _binding = new ElementDefinition.BindingComponent();
            _store = new ResourceStore(new Log(new Mock.ErrorLogger()));
            _store.Add(new PackageResource("PackageOne","http://myResource", "http://myResource", new Hl7.Fhir.Model.ValueSet()));
        }

        [TestMethod]
        public void ElementDefinitionExtensions_GenerateBindingDescription_LinkAndLabelEqualWhenBindingValuesetIsFhirUri()
        {
            _binding.ValueSet = new FhirUri(new Uri("http://myUri"));
            _binding.Strength = BindingStrength.Required;
            var binding = new Dict.BindingFormatter(_binding, _store, _package);
            string actual = binding.Describe();

            Assert.AreEqual(Resources.FhirUriBinding, actual);
        }

        [TestMethod]
        public void ElementDefinitionExtensions_GenerateBindingDescription_LinkAndLabelAreEqualWhenReferencingaResource()
        {
           var reference = new ResourceReference();
            reference.Reference = "http://myResource";
            _binding.ValueSet = reference;
            _binding.Strength = BindingStrength.Required;
            var binding = new Dict.BindingFormatter(_binding, _store, _package);
            string actual = binding.Describe();

           Assert.AreEqual(Resources.ResourceBindingDescription, actual);
        }

        [TestMethod]
        public void ElementDefinitionExtensions_GenerateBindingDescription_LocalLinkReturnedWhenReferencingaFhirValueSet()
        {
            const string url = "http://fhir.nhs.net/ValueSet/message-event-2-0";
            
            _store.Add(new PackageResource(_package, "message-event-2-0", url,  new Hl7.Fhir.Model.ValueSet()));
            var reference = new ResourceReference();
            reference.Reference = url;
            _binding.ValueSet = reference;
            _binding.Strength = BindingStrength.Required;
            var binding = new Dict.BindingFormatter(_binding, _store, _package);
            string actual = binding.Describe();

            Assert.AreEqual(Resources.localLinkBindingDescription, actual);
        }
    }
}