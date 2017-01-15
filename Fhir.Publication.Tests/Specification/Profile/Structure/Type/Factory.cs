using System;
using System.Collections.Generic;
using System.Linq;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Specification.TableModel;
using Hl7.Fhir.Publication.ImplementationGuide;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PubModel = Hl7.Fhir.Model;
using PubProfile = Hl7.Fhir.Publication.Specification.Profile;

namespace Fhir.Publication.Tests.Specification.Profile.Structure.Type
{
    [TestClass]
    public class Factory
    {
        private readonly Row _row;
        private readonly PubModel.ElementDefinition _elementDefinition;
        private readonly PubModel.StructureDefinition _structureDefinition;
        private readonly Package _packageFactory;
        private readonly PubProfile.KnowledgeProvider _knowledgeProvider;
            
        public Factory()
        {
            _row = new Row();
            _elementDefinition = new PubModel.ElementDefinition();
            _structureDefinition = new PubModel.StructureDefinition();
            _structureDefinition.Name = "MyProfile";
            var log = new Log(new Mock.ErrorLogger());
            var context = new Context(new Root("sourceDir", "targetDir"));
            _packageFactory = new Package("Profile.GetRecordQueryResponse", context, log, new Mock.DirectoryCreator());
            _knowledgeProvider = new PubProfile.KnowledgeProvider(log);      
        }

        private void CreateExtensionElementDefintion()
        {
            _elementDefinition.Path = "extension";
            var typeRefComponent = new PubModel.ElementDefinition.TypeRefComponent();
            var typeUrl = new List<string>();
            typeUrl.Add("http://fhir.nhs.net/StructureDefinition/extension-ethnic-category-1-0");
            typeRefComponent.Profile = typeUrl;

            _elementDefinition.Type.Add(typeRefComponent);
        }

        private void CreateModifierExtensionElementDefintion()
        {
            _elementDefinition.Path = "modifierExtension";

            var typeRefComponent = new PubModel.ElementDefinition.TypeRefComponent();
            var typeUrl = new List<string>();
            typeUrl.Add("http://fhir.nhs.net/StructureDefinition/extension-ers-specialty-1-0");
            typeRefComponent.Profile = typeUrl;

            _elementDefinition.Type.Add(typeRefComponent);
        }

        private void CreateResourceReferenceElementDefinition()
        {
            _elementDefinition.Path = "AppointmentResponse.actor";

            var typeOne = new PubModel.ElementDefinition.TypeRefComponent();
            var typeUrl = new List<string>();
            typeUrl.Add("http://fhir.nhs.net/StructureDefinition/gpconnect-patient-1-0");
            typeOne.Profile = typeUrl;
            typeOne.Code = PubModel.FHIRDefinedType.Reference;

            var typeTwo = new PubModel.ElementDefinition.TypeRefComponent();
            typeUrl = new List<string>();
            typeUrl.Add("http://fhir.nhs.net/StructureDefinition/gpconnect-practitioner-1-0");
            typeTwo.Profile = typeUrl;
            typeTwo.Code = PubModel.FHIRDefinedType.Reference;

            var typeThree = new PubModel.ElementDefinition.TypeRefComponent();
            typeUrl = new List<string>();
            typeUrl.Add("http://fhir.nhs.net/StructureDefinition/gpconnect-location-1-0");
            typeThree.Profile = typeUrl;
            typeThree.Code = PubModel.FHIRDefinedType.Reference;

            _elementDefinition.Type.AddRange(new[] { typeOne, typeTwo, typeThree });
        }

        [TestMethod]
        [IntegrationTest]
        public void Factory_GetCells_ReferenceForExtensionTypeIsGenerated()
        {
            CreateExtensionElementDefintion();
            _packageFactory.LoadResources();

            var factory = new PubProfile.Structure.Type.Factory(_elementDefinition, _structureDefinition.Name, _packageFactory, _knowledgeProvider);
            _row.GetCells().Add(factory.GetCells());

            Assert.IsTrue(_row.GetCells().Count == 1);

            Assert.IsTrue(_row.GetCells()
                .Count(
                    cell => 
                        cell.GetPieces()
                            .Exists(
                                piece => 
                                    piece.GetReference() == "extension-ethniccategory-1-0.html")) == 1);
          
        }

        [TestMethod]
        [IntegrationTest]
        public void Factory_GetCells_ReferenceForModifierExtensionTypeIsGenerated()
        {
            CreateModifierExtensionElementDefintion();
            _packageFactory.LoadResources();

            var factory = new PubProfile.Structure.Type.Factory(_elementDefinition, _structureDefinition.Name, _packageFactory, _knowledgeProvider);
           _row.GetCells().Add(factory.GetCells());

            Assert.IsTrue(_row.GetCells().Count == 1);

            Assert.IsTrue(_row.GetCells()
                .Count(
                    cell =>
                        cell.GetPieces()
                            .Exists(
                                piece =>
                                    piece.GetReference() == "ers-specialty-1-0.html")) == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), " No extension types of http://fhir.nhs.net/StructureDefinition/extension have been found!")]
        public void Factory_GetCells_NoExtensionFoundThowsInvalidOperationException()
        {
            _elementDefinition.Path = "modifierExtension";

            var typeRefComponent = new PubModel.ElementDefinition.TypeRefComponent();
            var typeUrl = new List<string>();
            typeUrl.Add("http://fhir.nhs.net/StructureDefinition/ers-specialty-1-0");
            typeRefComponent.Profile = typeUrl;

            _elementDefinition.Type.Add(typeRefComponent);
            _packageFactory.LoadResources();

            var factory = new PubProfile.Structure.Type.Factory(_elementDefinition, _structureDefinition.Name, _packageFactory, _knowledgeProvider);
            _row.GetCells().Add(factory.GetCells());
        }

        [TestMethod]
        [IntegrationTest]
        public void Factory_GetCells_ModifierExtensionTextHasPrefixRemoved()
        {
            CreateModifierExtensionElementDefintion();
            _packageFactory.LoadResources();

            var factory = new PubProfile.Structure.Type.Factory(_elementDefinition, _structureDefinition.Name, _packageFactory, _knowledgeProvider);
            _row.GetCells().Add(factory.GetCells());
          
            Assert.IsTrue(_row.GetCells().Count == 1);

            Assert.IsTrue(_row.GetCells()
                .Count(
                    cell =>
                        cell.GetPieces()
                            .Exists(
                                piece =>
                                    piece.GetText() == "extension-ers-specialty-1-0")) == 1);
        }

        [TestMethod]
        [IntegrationTest]
        public void Factory_GetCells_ExtensionTextHasPrefixRemoved()
        {
            CreateExtensionElementDefintion();
            _packageFactory.LoadResources();

            var factory = new PubProfile.Structure.Type.Factory(_elementDefinition, _structureDefinition.Name, _packageFactory, _knowledgeProvider);
            _row.GetCells().Add(factory.GetCells());

            Assert.IsTrue(_row.GetCells().Count == 1);

            Assert.IsTrue(_row.GetCells()
                .Count(
                    cell =>
                        cell.GetPieces()
                            .Exists(
                                piece =>
                                    piece.GetText() == "extension-ethnic-category-1-0")) == 1);

        }

        [TestMethod]
        [IntegrationTest]
        public void Factory_GetCells_CellHasResourceReference()
        {
            CreateResourceReferenceElementDefinition();
            _packageFactory.LoadResources();

            var factory = new PubProfile.Structure.Type.Factory(_elementDefinition, _structureDefinition.Name, _packageFactory, _knowledgeProvider);
            _row.GetCells().Add(factory.GetCells());

            Assert.IsTrue(_row.GetCells().Count == 1);

            List<Piece> cellPieces = _row.GetCells()[0].GetPieces();

            Assert.IsTrue(cellPieces.Count == 8);

            Assert.IsTrue(cellPieces[0].GetText() == "Reference");
            Assert.IsTrue(cellPieces[1].GetText() == " (");
            Assert.IsTrue(cellPieces[2].GetText() == "gpconnect-patient-1-0");
            Assert.IsTrue(cellPieces[3].GetText() == " | ");
            Assert.IsTrue(cellPieces[4].GetText() == "gpconnect-practitioner-1-0");
            Assert.IsTrue(cellPieces[5].GetText() == " | ");
            Assert.IsTrue(cellPieces[6].GetText() == "gpconnect-location-1-0");
            Assert.IsTrue(cellPieces[7].GetText() == ")");
        }

        [TestMethod]
        [IntegrationTest]
        public void Factory_GetCells_ResourceReferenceCellHasLinkToHl7References()
        {
            CreateResourceReferenceElementDefinition();
            _packageFactory.LoadResources();

            var factory = new PubProfile.Structure.Type.Factory(_elementDefinition, _structureDefinition.Name, _packageFactory, _knowledgeProvider);
            _row.GetCells().Add(factory.GetCells());

            Assert.IsTrue(_row.GetCells().Count == 1);

            List<Piece> cellPieces = _row.GetCells()[0].GetPieces();

            Assert.IsTrue(cellPieces[0].GetReference() == "http://www.hl7.org/fhir/references.html");
        }
    }
}