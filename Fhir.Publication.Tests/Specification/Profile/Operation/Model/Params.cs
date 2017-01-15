using System.Collections.Generic;
using Hl7.Fhir.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cell = Hl7.Fhir.Publication.Specification.TableModel.Cell;
using PubImplementation = Hl7.Fhir.Publication.ImplementationGuide;
using PubModel = Hl7.Fhir.Publication.Specification.Profile.Operation.Model;

namespace Fhir.Publication.Tests.Specification.Profile.Operation.Model
{
    [TestClass]
    public class Params
    {
        private const int _namecolumnIndex = 0;
        private const int _cardinalityColumnIndex = 1;
        private const int _typeColumnIndex = 2;
        private const int _descriptionColumnIndex = 3;
        private readonly PubModel.Params _params;
        private readonly OperationDefinition.ParameterComponent _parameter;

        public Params()
        {
            var operationDefinition = new OperationDefinition();
            var parameters = new List<OperationDefinition.ParameterComponent>();
            _parameter = new OperationDefinition.ParameterComponent();

            _parameter.Name = "patientNHSNumber";
            _parameter.Min = 1;
            _parameter.Max = "*";
            _parameter.Type = "Identifier";
            _parameter.Documentation = "Patient that matches the NHS Number(s)";

            parameters.Add(_parameter);
            operationDefinition.Parameter = parameters;
            var log = new Hl7.Fhir.Publication.Framework.Log(new Mock.ErrorLogger());
            var resourceStore = new PubImplementation.ResourceStore(log);
            var knowledgeProvider = new Hl7.Fhir.Publication.Specification.Profile.KnowledgeProvider(log);

            _params = new PubModel.Params(operationDefinition.Parameter, resourceStore, knowledgeProvider);
        }

        [TestMethod]
        public void Params_Table_TableContainsFourTitleColumns()
        {
            Assert.IsTrue(_params.Table.Titles.Count == 4);
        }

        [TestMethod]
        public void Params_Table_TableContainsNameTitle()
        {
            Assert.IsTrue(_params.Table.Titles[0].GetPieces()[0].GetText() == "Name");
            Assert.IsTrue(_params.Table.Titles[0].GetPieces()[0].GetHint() == "The logical name of the element");
        }

        [TestMethod]
        public void Params_Table_TableContainsCardinalityTitle()
        {
            Assert.IsTrue(_params.Table.Titles[1].GetPieces()[0].GetText() == "Card.");
            Assert.IsTrue(_params.Table.Titles[1].GetPieces()[0].GetHint() == "Minimum and maximum # of times the element can appear in the instance");
        }

        [TestMethod]
        public void Params_Table_TableContainsTypeTitle()
        {
            Assert.IsTrue(_params.Table.Titles[2].GetPieces()[0].GetText() == "Type");
            Assert.IsTrue(_params.Table.Titles[2].GetPieces()[0].GetHint() == "Reference to the type of the element");
        }

        [TestMethod]
        public void Params_Table_TableContainsDescritionTitle()
        {
            Assert.IsTrue(_params.Table.Titles[3].GetPieces()[0].GetText() == "Description");
            Assert.IsTrue(_params.Table.Titles[3].GetPieces()[0].GetHint() == "Additional information about the element");
        }

        [TestMethod]
        public void Params_Table_ParameterNameIsInNameColumn()
        {
            List<Cell> cells = _params.Table.Rows[0].GetCells();

            Assert.IsTrue(cells[_namecolumnIndex].GetPieces().Exists(piece => piece.GetText() == _parameter.Name));
        }

        [TestMethod]
        public void Params_Table_ParameterMinAndMaxValuesAreInCardinalityColumn()
        {
            List<Cell> cells = _params.Table.Rows[0].GetCells();

            Assert.IsTrue(cells[_cardinalityColumnIndex].GetPieces().Exists(piece => piece.GetText() == "1..*"));
        }

        [TestMethod]
        public void Params_Table_ParameterTypeIsInTypeColumn()
        {
            List<Cell> cells = _params.Table.Rows[0].GetCells();

            Assert.IsTrue(cells[_typeColumnIndex].GetPieces().Exists(piece => piece.GetText() == _parameter.Type));
        }

        [TestMethod]
        public void Params_Table_ParameterDescriptionIsInTypeColumn()
        {
            List<Cell> cells = _params.Table.Rows[0].GetCells();

            Assert.IsTrue(cells[_descriptionColumnIndex].GetPieces().Exists(piece => piece.GetText() == _parameter.Documentation));
        }
    }
}
