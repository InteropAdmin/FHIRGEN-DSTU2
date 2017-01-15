using System;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.ImplementationGuide;
using Binding = Hl7.Fhir.Publication.Specification.Profile.Structure.Bindings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fhir.Publication.Tests.Specification.Profile.Structure.Bindings
{
    [TestClass]
    public class Formatter
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Bindings_Formatter_ArgumentNullExceptionThrownWhenResourceStoreIsNull()
        {
            var binding = new ElementDefinition.BindingComponent();
        
            var formatter = new Binding.Formatter("elementName", "PackageName", binding, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Bindings_Formatter_ArgumentExceptionThrownWhenBindingStrengthIsNull()
        {
            var binding = new ElementDefinition.BindingComponent();
            binding.ValueSet = new ResourceReference();
            var store = new ResourceStore(new Log(new Mock.ErrorLogger()));

            var formatter = new Binding.Formatter("elementName", "PackageName", binding, store);
        }
        
        [TestMethod]
        public void Bindings_Formatter_BindingStrengthUrlContainsExample()
        {
            var binding = new ElementDefinition.BindingComponent();
            binding.ValueSet = new ResourceReference();
            binding.Strength = BindingStrength.Example;
            var store = new ResourceStore(new Log(new Mock.ErrorLogger()));

            var formatter = new Binding.Formatter("elementName", "PackageName", binding, store);

            Assert.IsTrue(formatter.BindingStrength == "http://www.hl7.org/fhir/terminologies.html#example");
        }

        [TestMethod]
        public void Bindings_Formatter_NameContainsActor()
        {
            var binding = new ElementDefinition.BindingComponent();
            binding.ValueSet = new ResourceReference();
            binding.Strength = BindingStrength.Example;
            var store = new ResourceStore(new Log(new Mock.ErrorLogger()));

            var formatter = new Binding.Formatter("Actor", "PackageName", binding, store);

            Assert.IsTrue(formatter.Name == "Actor");
        }

        [TestMethod]
        public void Bindings_Formatter_ReferenceContainsExpectedlUrl()
        {
            const string expected = "http://fhir.nhs.net/OperationDefinition/gpconnect-schedule-operation-1-0";
            var binding = new ElementDefinition.BindingComponent();
            var resourceReference = new ResourceReference();
            resourceReference.Url = new Uri(expected);

            binding.ValueSet = resourceReference;
            binding.Strength = BindingStrength.Example;
            var store = new ResourceStore(new Log(new Mock.ErrorLogger()));

            var formatter = new Binding.Formatter("elementName", "PackageName", binding, store);

           Assert.AreEqual(expected, formatter.Reference);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Bindings_Formatter_InvalidOperationExceptionThrownWhenReferenceUrlIsNull()
        {
            var binding = new ElementDefinition.BindingComponent();
            var resourceReference = new ResourceReference();
            resourceReference.Url = null;

            binding.ValueSet = resourceReference;
            binding.Strength = BindingStrength.Example;
            var store = new ResourceStore(new Log(new Mock.ErrorLogger()));

            var formatter = new Binding.Formatter("elementName", "PackageName", binding, store);
            var actual = formatter.Reference;
        }

        [TestMethod]
        public void Bindings_Formatter_UrlHasExpectedUrl()
        {
            const string expected = "http://fhir.nhs.net/OperationDefinition/gpconnect-schedule-operation-1-0";
            var binding = new ElementDefinition.BindingComponent();
            var resourceReference = new ResourceReference();
            resourceReference.Url = new Uri(expected);

            binding.ValueSet = resourceReference;
            binding.Strength = BindingStrength.Example;
            var store = new ResourceStore(new Log(new Mock.ErrorLogger()));

            var formatter = new Binding.Formatter("elementName", "PackageName", binding, store);

            Assert.AreEqual(expected, formatter.Url);
        }
    }
}