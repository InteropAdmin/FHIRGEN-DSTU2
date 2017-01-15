using System;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.ImplementationGuide;
using Hl7.Fhir.Publication.Specification.TableModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PubOperation = Hl7.Fhir.Publication.Specification.Profile.Operation;
using System.Diagnostics;

namespace Fhir.Publication.Tests.Specification.Profile.Operation
{
    [TestClass]
    public class Binding
    {
        private const string _url = "http://fhir.nhs.net/ValueSet/myValueSet";
        private const string _packageName = "ProfileOne";
        private readonly OperationDefinition.BindingComponent _bindingModel;
        private readonly Cell _cell;
        private readonly ResourceStore _resourceStore;
        private PubOperation.BindingFormatter _binding;
        
        public Binding()
        {
            _cell = new Cell("prefix", "reference","text", "hint", "suffix");
            _bindingModel = new OperationDefinition.BindingComponent();
            _resourceStore = new ResourceStore(new Hl7.Fhir.Publication.Framework.Log(new Mock.ErrorLogger()));
            var resource = new Hl7.Fhir.Model.ValueSet();
            resource.Name = "myValueSet";
            _resourceStore.Add(new PackageResource("ProfileOne", "myValueSet", _url, resource));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Binding_Add_ArgumentNullExceptionThrownWhenValuesetIsNull()
        {
            _binding = new PubOperation.BindingFormatter(_cell, _bindingModel, _resourceStore, _packageName);
        }

        [TestMethod]
        public void Binding_Add_BindingContainsUriInValueSet()
        {
            _bindingModel.ValueSet = new FhirUri(_url);
            _binding = new PubOperation.BindingFormatter(_cell, _bindingModel, _resourceStore, _packageName);
            Cell cell = _binding.Value;

            Assert.IsTrue(
                cell.GetPieces().Exists(
                    piece => piece.GetText() == _url));

        }

        [TestMethod]
        public void Binding_Add_BindingContainsRequiredStrength()
        {
            _bindingModel.ValueSet = new FhirUri(_url);
            _bindingModel.Strength = BindingStrength.Required;

            _binding = new PubOperation.BindingFormatter(_cell, _bindingModel, _resourceStore, _packageName);
            Cell cell = _binding.Value;

            Assert.IsTrue(
                cell.GetPieces().Exists(
                    piece => piece.GetText() == BindingStrength.Required.ToString()));

        }
    }
}