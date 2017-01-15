using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PubSpec = Hl7.Fhir.Publication.Specification;

namespace Fhir.Publication.Tests.Specification.HierarchicalTable
{
    [TestClass]
    public class Factory
    {
        private readonly PubSpec.HierarchicalTable.Factory _factory;
        private PubSpec.TableModel.Model _table;
        
        public Factory()
        {
            _table = new PubSpec.TableModel.Model();
            _factory = new PubSpec.HierarchicalTable.Factory(new Mock.ImageCreator());
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "table model must have rows!")]
        public void HierarchicalTableGenerator_ValidateModel_TableHAsNoRowsThrowsArgumentException()
        {
            _factory.CreateFrom(_table);   
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "table model must have titles!")]
        public void HierarchicalTableGenerator_ValidateModel_TableHasNoTitlesThrowsArgumentException()
        {
            _table.Rows.Add(new PubSpec.TableModel.Row());

            _factory.CreateFrom(_table);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "title cells must have text!")]
        public void HierarchicalTableGenerator_ValidateTitles_TableTitleHAsNoTextThrowsInvalidOperationException()
        {
            var row = new PubSpec.TableModel.Row();
            _table.Rows.Add(row);
            _table.Titles.Add(new PubSpec.TableModel.Title(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0));

            _factory.CreateFrom(_table);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "All rows must have the same number of columns as the titles")]
        public void HierarchicalTableGenerator_ValidateColumns_TitlesToCellsMisMatchThrowsArgumentException()
        {
            var row = new PubSpec.TableModel.Row();
            _table.Rows.Add(row);
            _table.Titles.Add(new PubSpec.TableModel.Title("MyPrefix", "MyReference", "MyText","MyHint", "MySufix", 3));

            _factory.CreateFrom(_table);
        }
        
        [TestMethod]
        public void HierarchicalTableGenerator_Generate_4x4TableCreated()
        {
            _table = PubSpec.TableModel.Model.GetGenericTable();
            var row = new PubSpec.TableModel.Row();
            _table.Rows.AddRange(new[] { row, row, row, row });
            
            foreach (PubSpec.TableModel.Row tableRow in _table.Rows)
            {

                tableRow.GetCells().Add(
                        new PubSpec.TableModel.Cell(
                        "MyPrefix",
                        "MyReference",
                        "MyText",
                        "MyHint",
                        "MySufix"));
             }

            string actual = _factory.CreateFrom(_table).ToHtml().ToString();

            Assert.AreEqual(actual, Resources.FourXFourHtmlTable);
        }
    }
}