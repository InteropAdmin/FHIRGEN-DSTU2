using System.Collections.Generic;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Specification.TableModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PubModel = Hl7.Fhir.Publication.Specification.Profile.Operation.Model;

namespace Fhir.Publication.Tests.Specification.Profile.Operation.Model
{
    [TestClass]
    public class Meta
    {
        private const int _nameRowIndex = 0;
        private const int _kindRowIndex = 1;
        private const int _descriptionRowIndex = 2;
        private const int _requirementsRowIndex = 3;
        private const int _codeRowIndex = 4;
        private const int _systemRowIndex = 5;
        private const int _instanceRowIndex = 6;
        private readonly PubModel.Meta _meta;

        private readonly OperationDefinition _operationDefinition;

        public Meta()
        {
            _operationDefinition = new OperationDefinition();
            _operationDefinition.Name = "myOperation";
            _operationDefinition.Kind = OperationDefinition.OperationKind.Operation;
            _operationDefinition.Description = "this is  a description";
            _operationDefinition.Code = "MyCode";
            _operationDefinition.System = true;
            _operationDefinition.Instance = true;
            _operationDefinition.Requirements = "this is a requirement";

            var log = new Hl7.Fhir.Publication.Framework.Log(new Mock.ErrorLogger());
            var knowledgeProvider = new Hl7.Fhir.Publication.Specification.Profile.KnowledgeProvider(log);
            _meta = new PubModel.Meta(_operationDefinition, knowledgeProvider);
        }

        [TestMethod]
        public void Meta_Table_TableContainsThreeTitleColumns()
        {
            Assert.IsTrue(_meta.Table.Titles.Count == 3);
        }

        [TestMethod]
        public void Meta_Table_TableContainsNameTitle()
        {
            Assert.IsTrue(_meta.Table.Titles[0].GetPieces()[0].GetText() == "Name");
            Assert.IsTrue(_meta.Table.Titles[0].GetPieces()[0].GetHint() == "The logical name of the element");
        }

        [TestMethod]
        public void Meta_Table_TableContainsTypeTitle()
        {
            Assert.IsTrue(_meta.Table.Titles[1].GetPieces()[0].GetText() == "Type");
            Assert.IsTrue(_meta.Table.Titles[1].GetPieces()[0].GetHint() == "Reference to the type of the element");
        }

        [TestMethod]
        public void Meta_Table_TableContainsValueTitle()
        {
            Assert.IsTrue(_meta.Table.Titles[2].GetPieces()[0].GetText() == "Value");
            Assert.IsTrue(_meta.Table.Titles[2].GetPieces()[0].GetHint() == "Additional information about the element");
        }

        [TestMethod]
        public void Meta_Table_TableContainsNameRow()
        {
            List<Cell> nameRowcells = _meta.Table.Rows[_nameRowIndex].GetCells();

            Assert.IsTrue(nameRowcells[0].GetPieces().Exists(piece => piece.GetText() == "Name"));
        }

        [TestMethod]
        public void Meta_Table_NameRowHasTypeAndHyperlink()
        {
            List<Cell> nameRowcells = _meta.Table.Rows[_nameRowIndex].GetCells();

            Assert.IsTrue(nameRowcells[1].GetPieces().Exists(piece => piece.GetText() == "String"));
            Assert.IsTrue(nameRowcells[1].GetPieces().Exists(piece => piece.GetReference() == "http://www.hl7.org/fhir/datatypes.html#string"));
        }

        [TestMethod]
        public void Meta_Table_NameRowHasOperationDefinitionNameValue()
        {
            List<Cell> nameRowcells = _meta.Table.Rows[_nameRowIndex].GetCells();

            Assert.IsTrue(nameRowcells[2].GetPieces().Exists(piece => piece.GetText() == _operationDefinition.Name));
        }

        [TestMethod]
        public void Meta_Table_TableContainsKindRow()
        {
            List<Cell> kindRowCells = _meta.Table.Rows[_kindRowIndex].GetCells();

            Assert.IsTrue(kindRowCells[0].GetPieces().Exists(piece => piece.GetText() == "Kind"));
        }

        [TestMethod]
        public void Meta_Table_KindRowHasTypeAndHyperlink()
        {
            List<Cell> kindRowCells = _meta.Table.Rows[_kindRowIndex].GetCells();

            Assert.IsTrue(kindRowCells[1].GetPieces().Exists(piece => piece.GetText() == "Operation"));
            Assert.IsTrue(kindRowCells[1].GetPieces().Exists(piece => piece.GetReference() == "http://www.hl7.org/fhir/valueset-operation-kind.html"));
        }

        [TestMethod]
        public void Meta_Table_KindRowHasOperationDefinitionKindValue()
        {
            List<Cell> kindRowCells = _meta.Table.Rows[_kindRowIndex].GetCells();

            Assert.IsTrue(kindRowCells[2].GetPieces().Exists(piece => piece.GetText() == _operationDefinition.Kind.ToString()));
        }


        [TestMethod]
        public void Meta_Table_TableContainsDescriptionRow()
        {
            List<Cell> descriptionRowCells = _meta.Table.Rows[_descriptionRowIndex].GetCells();

            Assert.IsTrue(descriptionRowCells[0].GetPieces().Exists(piece => piece.GetText() == "Description"));
        }

        [TestMethod]
        public void Meta_Table_DescriptionRowHasTypeAndHyperlink()
        {
            List<Cell> descriptionRowCells = _meta.Table.Rows[_descriptionRowIndex].GetCells();

            Assert.IsTrue(descriptionRowCells[1].GetPieces().Exists(piece => piece.GetText() == "String"));
            Assert.IsTrue(descriptionRowCells[1].GetPieces().Exists(piece => piece.GetReference() == "http://www.hl7.org/fhir/datatypes.html#string"));
        }

        [TestMethod]
        public void Meta_Table_DescriptionRowHasOperationDefinitionKindValue()
        {
            List<Cell> descriptionRowCells = _meta.Table.Rows[_descriptionRowIndex].GetCells();

            Assert.IsTrue(descriptionRowCells[2].GetPieces().Exists(piece => piece.GetText() == _operationDefinition.Description));
        }

        [TestMethod]
        public void Meta_Table_TableContainsRequirementsRow()
        {
            List<Cell> requirementsRowCells = _meta.Table.Rows[_requirementsRowIndex].GetCells();

            Assert.IsTrue(requirementsRowCells[0].GetPieces().Exists(piece => piece.GetText() == "Requirements"));
        }

        [TestMethod]
        public void Meta_Table_RequirementsRowHasTypeAndHyperlink()
        {
            List<Cell> requirementsRowCells = _meta.Table.Rows[_requirementsRowIndex].GetCells();

            Assert.IsTrue(requirementsRowCells[1].GetPieces().Exists(piece => piece.GetText() == "String"));
            Assert.IsTrue(requirementsRowCells[1].GetPieces().Exists(piece => piece.GetReference() == "http://www.hl7.org/fhir/datatypes.html#string"));
        }

        [TestMethod]
        public void Meta_Table_RequirementsRowHasOperationDefinitionKindValue()
        {
            List<Cell> requirementsRowCells = _meta.Table.Rows[_requirementsRowIndex].GetCells();

            Assert.IsTrue(requirementsRowCells[2].GetPieces().Exists(piece => piece.GetText() == _operationDefinition.Requirements));
        }

        [TestMethod]
        public void Meta_Table_TableContainsCodeRow()
        {
            List<Cell> requirementsRowCells = _meta.Table.Rows[_codeRowIndex].GetCells();

            Assert.IsTrue(requirementsRowCells[0].GetPieces().Exists(piece => piece.GetText() == "Code"));
        }

        [TestMethod]
        public void Meta_Table_CodeRowHasTypeAndHyperlink()
        {
            List<Cell> codeRowCells = _meta.Table.Rows[_codeRowIndex].GetCells();

            Assert.IsTrue(codeRowCells[1].GetPieces().Exists(piece => piece.GetText() == "Code"));
            Assert.IsTrue(codeRowCells[1].GetPieces().Exists(piece => piece.GetReference() == "http://www.hl7.org/fhir/datatypes.html#code"));
        }

        [TestMethod]
        public void Meta_Table_CodeRowHasOperationDefinitionKindValue()
        {
            List<Cell> codeRowCells = _meta.Table.Rows[_codeRowIndex].GetCells();

            Assert.IsTrue(codeRowCells[2].GetPieces().Exists(piece => piece.GetText() == _operationDefinition.Code));
        }
        
        [TestMethod]
        public void Meta_Table_TableContainsSystemRow()
        {
            List<Cell> requirementsRowCells = _meta.Table.Rows[_systemRowIndex].GetCells();

            Assert.IsTrue(requirementsRowCells[0].GetPieces().Exists(piece => piece.GetText() == "System"));
        }

        [TestMethod]
        public void Meta_Table_SystemRowHasTypeAndHyperlink()
        {
            List<Cell> systemRowCells = _meta.Table.Rows[_systemRowIndex].GetCells();

            Assert.IsTrue(systemRowCells[1].GetPieces().Exists(piece => piece.GetText() == "Boolean"));
            Assert.IsTrue(systemRowCells[1].GetPieces().Exists(piece => piece.GetReference() == "http://www.hl7.org/fhir/datatypes.html#boolean"));
        }

        [TestMethod]
        public void Meta_Table_SystemRowHasOperationDefinitionKindValue()
        {
            List<Cell> systemRowCells = _meta.Table.Rows[_systemRowIndex].GetCells();

            Assert.IsTrue(systemRowCells[2].GetPieces().Exists(piece => piece.GetText() == _operationDefinition.System.ToString()));
        }

        [TestMethod]
        public void Meta_Table_TableContainsInstanceRow()
        {
            List<Cell> requirementsRowCells = _meta.Table.Rows[_instanceRowIndex].GetCells();

            Assert.IsTrue(requirementsRowCells[0].GetPieces().Exists(piece => piece.GetText() == "Instance"));
        }

        [TestMethod]
        public void Meta_Table_InstanceRowHasTypeAndHyperlink()
        {
            List<Cell> instanceRowCells = _meta.Table.Rows[_instanceRowIndex].GetCells();

            Assert.IsTrue(instanceRowCells[1].GetPieces().Exists(piece => piece.GetText() == "Boolean"));
            Assert.IsTrue(instanceRowCells[1].GetPieces().Exists(piece => piece.GetReference() == "http://www.hl7.org/fhir/datatypes.html#boolean"));
        }

        [TestMethod]
        public void Meta_Table_InstanceRowHasOperationDefinitionKindValue()
        {
            List<Cell> instanceRowCells = _meta.Table.Rows[_instanceRowIndex].GetCells();

            Assert.IsTrue(instanceRowCells[2].GetPieces().Exists(piece => piece.GetText() == _operationDefinition.Instance.ToString()));
        }
    }
}