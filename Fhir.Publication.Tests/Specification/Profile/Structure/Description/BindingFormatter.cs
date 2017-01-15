using System;
using System.Linq;
using Hl7.Fhir.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PubDescription = Hl7.Fhir.Publication.Specification.Profile.Structure.Description;

namespace Fhir.Publication.Tests.Specification.Profile.Structure.Description
{
    [TestClass]
    public class BindingFormatter
    {
        private const string _package = "ProfileOne";
        private readonly ElementDefinition.BindingComponent _binding;
        private readonly Hl7.Fhir.Publication.ImplementationGuide.ResourceStore _resourceStore;
        private readonly Hl7.Fhir.Publication.Specification.TableModel.Cell _cell;

        public BindingFormatter()
        {
            var log = new Hl7.Fhir.Publication.Framework.Log(new Mock.ErrorLogger());
            _binding = new ElementDefinition.BindingComponent();
            _resourceStore = new Hl7.Fhir.Publication.ImplementationGuide.ResourceStore(log);
            _cell = new Hl7.Fhir.Publication.Specification.TableModel.Cell();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void BindingFormatter_InvalidOperationExceptionThrownIfDescriptionIsNull()
        {
            _cell.AddPiece(new Hl7.Fhir.Publication.Specification.TableModel.Piece("reference", "text", "hint"));
            _binding.ValueSet = new FhirUri("http://fhir.nhs.net/Badger");
            var formatter = new PubDescription.BindingFormatter(_binding, _resourceStore, _cell, _package);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void BindingFormatter_InvalidOperationExceptionThrownIfCellContainsNoPieces()
        {
            _binding.ValueSet = new FhirUri("http://fhir.nhs.net/Badger");
            _binding.Description = "This is a description";
            var formatter = new PubDescription.BindingFormatter(_binding, _resourceStore, _cell, _package);
        }

        [TestMethod]
        public void BindingFormatter_GetFormattedBinding_CellContainsBindingStrength()
        {
            _cell.AddPiece(new Hl7.Fhir.Publication.Specification.TableModel.Piece("reference", "text", "hint"));
            _binding.ValueSet = new FhirUri("http://fhir.nhs.net/Badger");
            _binding.Strength = BindingStrength.Required;
            _binding.Description = "This is a description";
            var formatter = new PubDescription.BindingFormatter(_binding, _resourceStore, _cell, _package);
            
            var cell = formatter.GetFormattedBinding();

            Assert.IsTrue(cell.GetPieces().Any(piece => piece.GetText() == "Binding Strength"));
            Assert.IsTrue(cell.GetPieces().Any(piece => piece.GetText() == BindingStrength.Required.ToString()));
        }

        [TestMethod]
        public void BindingFormatter_GetFormattedBinding_CellContainsBindingDescription()
        {
            _cell.AddPiece(new Hl7.Fhir.Publication.Specification.TableModel.Piece("reference", "text", "hint"));
            _binding.ValueSet = new FhirUri("http://fhir.nhs.net/Badger");
            _binding.Description = "This is a description";
            _binding.Strength = BindingStrength.Required;

            var formatter = new PubDescription.BindingFormatter(_binding, _resourceStore, _cell, _package);
            var cell = formatter.GetFormattedBinding();

            Assert.IsTrue(cell.GetPieces().Any(piece => piece.GetText() == "Binding"));
            Assert.IsTrue(cell.GetPieces().Any(piece => piece.GetText() == "This is a description"));
        }

        [TestMethod]
        public void BindingFormatter_GetFormattedBinding_CellContainsBindingReference()
        {
            _cell.AddPiece(new Hl7.Fhir.Publication.Specification.TableModel.Piece("reference", "text", "hint"));
            _binding.ValueSet = new FhirUri("http://fhir.nhs.net/Badger");
            _binding.Description = "This is a description";
            _binding.Strength = BindingStrength.Required;
            var formatter = new PubDescription.BindingFormatter(_binding, _resourceStore, _cell, _package);

            var cell = formatter.GetFormattedBinding();

            Assert.IsTrue(cell.GetPieces().Any(piece => piece.GetReference() == "http://fhir.nhs.net/Badger"));
        }

        [TestMethod]
        public void BindingFormatter_GetFormattedBinding_CellContainsBindingUrl()
        {
            _cell.AddPiece(new Hl7.Fhir.Publication.Specification.TableModel.Piece("reference", "text", "hint"));
            _binding.ValueSet = new FhirUri("http://fhir.nhs.net/Badger");
            _binding.Description = "This is a description";
            _binding.Strength = BindingStrength.Required;
            var formatter = new PubDescription.BindingFormatter(_binding, _resourceStore, _cell, _package);

            var cell = formatter.GetFormattedBinding();

            Assert.IsTrue(cell.GetPieces().Any(piece => piece.GetText() == " (http://fhir.nhs.net/Badger)"));
        }
    }
}