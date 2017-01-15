using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.ImplementationGuide;
using Hl7.Fhir.Publication.Specification.TableModel;
using PubOperation = Hl7.Fhir.Publication.Specification.Profile.Operation;

namespace Fhir.Publication.Tests.Specification.Profile.Operation
{
    [TestClass]
    public class BoundProfile
    {
        private readonly Cell _cell;
        private readonly ResourceStore _resourceStore;

        public BoundProfile()
        {
            _cell = new Cell("prefix", "suffix", "text", "hint", "suffix");
            _resourceStore = new ResourceStore(new Hl7.Fhir.Publication.Framework.Log(new Mock.ErrorLogger()));
            _resourceStore.Add(
                new PackageResource(
                    "ProfileOne",
                    "gpconnect-message-bundle-1-0",
                    "http://fhir.nhs.net/StructureDefinition/gpconnect-message-bundle-1-0", 
                    new StructureDefinition()));
     
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Binding_Add_InvalidOperationExceptionThrownWhenReferenceNotFoundInResourceStore()
        {
            var profileBinding = new ResourceReference();
            const string expected = "http://fhir.nhs.net/StructureDefinition/gpconnect-message-bundle-1-0";
            profileBinding.Reference = expected;
            var resourceStore = new ResourceStore(new Hl7.Fhir.Publication.Framework.Log(new Mock.ErrorLogger()));
            
            var binding = new PubOperation.BoundProfile(_cell, profileBinding, resourceStore);
            Cell cell = binding.Value;
        }

        [TestMethod]
        public void Binding_Add_ReferenceAddedWhenResourceReferenceIsNotNull()
        {
            var profileBinding = new ResourceReference();
            const string expected = "http://fhir.nhs.net/StructureDefinition/gpconnect-message-bundle-1-0";
            profileBinding.Reference = expected;

            var binding = new PubOperation.BoundProfile(_cell, profileBinding, _resourceStore);
            Cell cell =  binding.Value;

            Assert.IsTrue(cell.GetPieces()
                .Count(
                    piece => 
                        piece.GetText() == expected && piece.GetHint() == "operation references structure definition") == 1);
        }

        [TestMethod]
        public void Binding_Add_ReferenceNotAddeddWhenResourceReferenceIsNull()
        {
            var profileBinding = new ResourceReference();
            const string expected = null;
            profileBinding.Reference = expected;

            var binding = new PubOperation.BoundProfile(_cell, profileBinding, _resourceStore);
            Cell cell = binding.Value;

            Assert.IsTrue(cell.GetPieces()
                 .Count(
                     piece =>
                         piece.GetHint() == "operation references structure definition") == 0);
        }

        [TestMethod]
        public void Binding_Add_DisplayTextAddedWhenResourceDisplayIsNotNull()
        {
            var profileBinding = new ResourceReference();
            const string expected = "This is display Text!";
            profileBinding.Reference = "http://fhir.nhs.net/StructureDefinition/gpconnect-message-bundle-1-0";
            profileBinding.Display = expected;

            var binding = new PubOperation.BoundProfile(_cell, profileBinding, _resourceStore);
            Cell cell = binding.Value;

            Assert.IsTrue(cell.GetPieces()
                .Count(
                    piece =>
                        piece.GetText() == expected)  == 1);
        }

        [TestMethod]
        public void Binding_Add_DisplayTextNotAddedWhenResourceDisplayIsNull()
        {
            var profileBinding = new ResourceReference();
            profileBinding.Reference = "http://fhir.nhs.net/StructureDefinition/gpconnect-message-bundle-1-0";

            var binding = new PubOperation.BoundProfile(_cell, profileBinding, _resourceStore);
            Cell cell = binding.Value;

            Assert.IsTrue(cell.GetPieces()
                .Count(
                    piece =>
                        piece.GetText() == "Display: ") == 0);
        }
    }
}