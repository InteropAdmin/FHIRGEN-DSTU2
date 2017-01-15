using System.Collections.Generic;
using System.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Specification.TableModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PubExtension = Hl7.Fhir.Publication.Specification.Profile.Structure.Rows;
using PubIG = Hl7.Fhir.Publication.ImplementationGuide;
using PubFramework = Hl7.Fhir.Publication.Framework;
using PubSpec = Hl7.Fhir.Publication.Specification;

namespace Fhir.Publication.Tests.Specification.Profile.Structure.Rows
{
    [TestClass]
    public class ReferenceToParentRow
    {
        private const int _nameCellIndex = 0;
        private const int _cardinalityCellIndex = 1;
        private const int _typeCellIndex = 2;
        private const int _descriptionCellIndex = 3;
        private string _reference;
        private ElementDefinition _elementDefinition;
        private Row _slicedElementRow;

        private PubExtension.ReferenceToParentRow RowHasDefinition
        {
            get
            {
                var row = new Row();
                var log = new PubFramework.Log(new Mock.ErrorLogger());
                var root = new PubFramework.Root("sourceDir", "targetDir");
                var location = new PubFramework.Location("targetDir");
                var context = new PubFramework.Context(root, location);
                var directoryCreator = new Mock.DirectoryCreator();
                var package = new PubIG.Package("ProfileOne", context, log, directoryCreator);
                var knowledgeProvider = new PubSpec.Profile.KnowledgeProvider(log);
                _reference = "MyReference";

                _elementDefinition = new ElementDefinition();
                _elementDefinition.Name = "MyElementDefinition";
                _elementDefinition.Path = "MyResource.coding.code";
                _elementDefinition.Min = 0;
                _elementDefinition.Max = "1";
                _elementDefinition.Definition = "This is my definition";
                _elementDefinition.Short = "This is my short description";

                var type = new ElementDefinition.TypeRefComponent();
                type.Code = FHIRDefinedType.Code;
                var types = new List<ElementDefinition.TypeRefComponent>();
                types.Add(type);
                _elementDefinition.Type = types;

                var parentReference = new FhirString();
                parentReference.Value = FHIRDefinedType.Coding.ToString();
                _elementDefinition.NameReferenceElement = parentReference;

                return new PubExtension.ReferenceToParentRow(_elementDefinition.Name, package, knowledgeProvider, row, _reference, _elementDefinition);
            }
        }

        private PubExtension.ReferenceToParentRow RowHasNoDefinition
        {
            get
            {
                var row = new Row();
                var log = new PubFramework.Log(new Mock.ErrorLogger());
                var root = new PubFramework.Root("sourceDir", "targetDir");
                var location = new PubFramework.Location("targetDir");
                var context = new PubFramework.Context(root, location);
                var directoryCreator = new Mock.DirectoryCreator();
                var package = new PubIG.Package("ProfileOne", context, log, directoryCreator);
                var knowledgeProvider = new PubSpec.Profile.KnowledgeProvider(log);
                _reference = "MyReference";

                _elementDefinition = new ElementDefinition();
                _elementDefinition.Min = 0;
                _elementDefinition.Max = "1";
                _elementDefinition.Path = "MyResource.coding.code";
                var parentReference = new FhirString();
                parentReference.Value = FHIRDefinedType.Coding.ToString();
                _elementDefinition.NameReferenceElement = parentReference;

                return new PubExtension.ReferenceToParentRow("Code", package, knowledgeProvider, row, _reference, _elementDefinition);
            }
        }

        private PubExtension.ReferenceToParentRow SlicedElementRow
        {
            get
            {
                _slicedElementRow = new Row();
                var log = new PubFramework.Log(new Mock.ErrorLogger());
                var root = new PubFramework.Root("sourceDir", "targetDir");
                var location = new PubFramework.Location("targetDir");
                var context = new PubFramework.Context(root, location);
                var directoryCreator = new Mock.DirectoryCreator();
                var package = new PubIG.Package("ProfileOne", context, log, directoryCreator);
                var knowledgeProvider = new PubSpec.Profile.KnowledgeProvider(log);
                _reference = "MyReference";

                _elementDefinition = new ElementDefinition();
                _elementDefinition.Name = "MyElementDefinition";
                _elementDefinition.Definition = "This is my definition";


                var slice = new ElementDefinition.SlicingComponent();
                slice.AddExtension("uri", new FhirString("SliceExtension"));
                slice.Rules = ElementDefinition.SlicingRules.OpenAtEnd;
                slice.Ordered = false;
                _elementDefinition.Slicing = slice;


                var type = new ElementDefinition.TypeRefComponent();
                type.Code = FHIRDefinedType.Code;
                var types = new List<ElementDefinition.TypeRefComponent>();
                types.Add(type);
                _elementDefinition.Type = types;

                _elementDefinition.Path = "MyResource.coding.code";
                var parentReference = new FhirString();
                parentReference.Value = FHIRDefinedType.Coding.ToString();
                _elementDefinition.NameReferenceElement = parentReference;

                return new PubExtension.ReferenceToParentRow("Code", package, knowledgeProvider, _slicedElementRow, _reference, _elementDefinition);
            }
        }

        private static string GetReference(IEnumerable<Piece> pieces)
        {
            return pieces.First().GetReference();
        }

        private static string GetText(IEnumerable<Piece> pieces)
        {
            return pieces.First().GetText();
        }

        private static string GetHint(IEnumerable<Piece> pieces)
        {
            return pieces.First().GetHint();
        }

        [TestMethod]
        public void ReferenceToParentRow_Value_NameCellReferencePieceContainsReference()
        {
            var nameCellPieces = RowHasDefinition.Value.GetCells()[_nameCellIndex].GetPieces();

            Assert.IsTrue(_reference == GetReference(nameCellPieces));
        }


        [TestMethod]
        public void ReferenceToParentRow_Value_NameCellTextIsElementDefinitionName()
        {
            var nameCellPieces = RowHasDefinition.Value.GetCells()[_nameCellIndex].GetPieces();

            Assert.IsTrue(_elementDefinition.Name == GetText(nameCellPieces));
        }

        [TestMethod]
        public void ReferenceToParentRow_Value_NameCellHintIsShortDefinitionIfHasDefinitionIsTrue()
        {
            var nameCellPieces = RowHasDefinition.Value.GetCells()[_nameCellIndex].GetPieces();

            Assert.IsTrue(_elementDefinition.Short == GetHint(nameCellPieces));
        }

        [TestMethod]
        public void ReferenceToParentRow_Value_NameCellHintIsNullIfHasDefinitionIsFalse()
        {
            var nameCellPieces = RowHasNoDefinition.Value.GetCells()[_nameCellIndex].GetPieces();

            Assert.IsTrue(null == GetHint(nameCellPieces));
        }

        [TestMethod]
        public void ReferenceToParentRow_Value_CardinalityCellTextContainsElementDefinitionMinAndMaxValues()
        {
            var cardinalityCellPieces = RowHasDefinition.Value.GetCells()[_cardinalityCellIndex].GetPieces();

            Assert.IsTrue("0..1" == GetText(cardinalityCellPieces));
        }

        [TestMethod]
        public void ReferenceToParentRow_Value_TypeHasReferenceToParentType()
        {
            var typeCellPieces = RowHasDefinition.Value.GetCells()[_typeCellIndex].GetPieces();

            Assert.IsTrue("see Coding" == GetText(typeCellPieces));
        }

        [TestMethod]
        public void ReferenceToParentRow_Value_DescriptionCellTextPieceContainsShortDescription()
        {
            var descriptionCellPieces = RowHasDefinition.Value.GetCells()[_descriptionCellIndex].GetPieces();

            Assert.IsTrue("This is my short description" == GetText(descriptionCellPieces));
        }

        [TestMethod]
        public void ReferenceToParentRow_Value_SlicedCellCardinalityCellContainsNoPieces()
        {
            var cellPieces = SlicedElementRow.Value.GetCells()[_cardinalityCellIndex].GetPieces();

            Assert.IsTrue(cellPieces.Count == 0);
        }

        [TestMethod]
        public void ReferenceToParentRow_Value_SlicedCellTypeContainsNoPieces()
        {
            var cellPieces = SlicedElementRow.Value.GetCells()[_typeCellIndex].GetPieces();

            Assert.IsTrue(cellPieces.Count == 0);
        }
    }
}
