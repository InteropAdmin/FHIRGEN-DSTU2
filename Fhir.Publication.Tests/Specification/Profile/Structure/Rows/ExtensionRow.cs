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
    public class ExtensionRow
    {
        private const int _nameCellIndex = 0;
        private const int _cardinalityCellIndex = 1;
        private const int _typeCellIndex = 2;
        private const int _descriptionCellIndex = 3;
        private ElementDefinition _elementDefinition;
        private string _reference;
        private string _resourceName;

        private PubExtension.ExtensionRow RowHasDefinition
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
                _resourceName = "MyResource";

                _elementDefinition = new ElementDefinition();
                _elementDefinition.Name = "MyElementDefinition";
                _elementDefinition.Path = "Path";
                _elementDefinition.Min = 0;
                _elementDefinition.Max = "1";
                _elementDefinition.Definition = "This is my definition";
                _elementDefinition.Short = "This is my short description";

                var type = new ElementDefinition.TypeRefComponent();
                type.Code = FHIRDefinedType.Code;
                var types = new List<ElementDefinition.TypeRefComponent>();
                types.Add(type);
                _elementDefinition.Type = types;
                
                return new PubExtension.ExtensionRow(package, knowledgeProvider, row, _reference, _elementDefinition, _resourceName);
            }
        }

        private PubExtension.ExtensionRow RowHasNoDefinition
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
                _resourceName = "MyResource";

                _elementDefinition = new ElementDefinition();

                return new PubExtension.ExtensionRow(package, knowledgeProvider, row, _reference, _elementDefinition, _resourceName);
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
        public void ExtensionRow_Value_NameCellReferencePieceContainsReference()
        {
            var nameCellPieces = RowHasDefinition.Value.GetCells()[_nameCellIndex].GetPieces();

            Assert.IsTrue(_reference == GetReference(nameCellPieces));
        }


        [TestMethod]
        public void ExtensionRow_Value_NameCellTextIsElementDefinitionName()
        {
            var nameCellPieces = RowHasDefinition.Value.GetCells()[_nameCellIndex].GetPieces();

            Assert.IsTrue(_elementDefinition.Name == GetText(nameCellPieces));
        }

        [TestMethod]
        public void ExtensionRow_Value_NameCellHintIsShortDefinitionIfHasDefinitionIsTrue() 
        {
            var nameCellPieces = RowHasDefinition.Value.GetCells()[_nameCellIndex].GetPieces();
            
            Assert.IsTrue(_elementDefinition.Short == GetHint(nameCellPieces));
        }

        [TestMethod]
        public void ExtensionRow_Value_NameCellHintIsNullIfHasDefinitionIsFalse()
        {
            var nameCellPieces = RowHasNoDefinition.Value.GetCells()[_nameCellIndex].GetPieces();

            Assert.IsTrue(null == GetHint(nameCellPieces));
        }

        [TestMethod]
        public void ExtensionRow_Value_CardinalityCellTextContainsElementDefinitionMinAndMaxValues()
        {
            var cardinalityCellPieces = RowHasDefinition.Value.GetCells()[_cardinalityCellIndex].GetPieces();

            Assert.IsTrue("0..1" == GetText(cardinalityCellPieces));
        }

        [TestMethod]
        public void ExtensionRow_Value_TypeCellContainsNoPiecesIfElementDefinitionNameIsNull()
        {
            var typeCellPieces = RowHasNoDefinition.Value.GetCells()[_typeCellIndex].GetPieces();

            Assert.IsTrue(typeCellPieces.Count == 0);
        }

        [TestMethod]
        public void ExtensionRow_Value_TypeCellReferencePieceContainsTypeReference()
        {
            var typeCellPieces = RowHasDefinition.Value.GetCells()[_typeCellIndex].GetPieces();

            Assert.IsTrue("http://www.hl7.org/fhir/datatypes.html#code" == GetReference(typeCellPieces));
        }

        [TestMethod]
        public void ExtensionRow_Value_TypeCellTextPieceContainsType()
        {
            var typeCellPieces = RowHasDefinition.Value.GetCells()[_typeCellIndex].GetPieces();

            Assert.IsTrue("Code" == GetText(typeCellPieces));
        }

        [TestMethod]
        public void ExtensionRow_Value_DescriptionCellTextPieceContainsShortDescription()
        {
            var descriptionCellPieces = RowHasDefinition.Value.GetCells()[_descriptionCellIndex].GetPieces();

            Assert.IsTrue("This is my short description" == GetText(descriptionCellPieces));
        }
    }
}