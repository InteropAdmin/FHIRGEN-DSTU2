using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PubProfile = Hl7.Fhir.Publication.Specification;
using PubFramework = Hl7.Fhir.Publication.Framework;

namespace Fhir.Publication.Tests.Specification.HierarchicalTable
{
    [TestClass]
    public class Generator
    {
        private readonly Hl7.Fhir.Publication.Specification.HierarchicalTable.Factory _tableGenerator;
        private PubProfile.TableModel.Model _table;

        public Generator()
        {
            PubFramework.IDirectoryCreator directoryCreator = new Mock.DirectoryCreator();

            _table = new PubProfile.TableModel.Model();
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "table model must have rows!")]
        public void HierarchicalTableGenerator_ValidateModel_TableHAsNoRowsThrowsArgumentException()
        {
            _tableGenerator.Generate(_table);   
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "table model must have titles!")]
        public void HierarchicalTableGenerator_ValidateModel_TableHasNoTitlesThrowsArgumentException()
        {
            _table.Rows.Add(new PubProfile.TableModel.Row());

            _tableGenerator.Generate(_table);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "title cells must have text!")]
        public void HierarchicalTableGenerator_ValidateTitles_TableTitleHAsNoTextThrowsInvalidOperationException()
        {
            var row = new PubProfile.TableModel.Row();
            _table.Rows.Add(row);
            _table.Titles.Add(new PubProfile.TableModel.Title(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0));

            _tableGenerator.Generate(_table);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "All rows must have the same number of columns as the titles")]
        public void HierarchicalTableGenerator_ValidateColumns_TitlesToCellsMisMatchThrowsArgumentException()
        {
            var row = new PubProfile.TableModel.Row();
            _table.Rows.Add(row);
            _table.Titles.Add(new PubProfile.TableModel.Title("MyPrefix", "MyReference", "MyText","MyHint", "MySufix", 3));

            _tableGenerator.Generate(_table);
        }


        [TestMethod]
        public void HierarchicalTableGenerator_Generate_4x4TableCreated()
        {
            _table = PubProfile.TableModel.Model.CreateGenericTable();
            var row = new PubProfile.TableModel.Row();
            _table.Rows.AddRange(new[] { row, row, row, row });
            
            foreach (PubProfile.TableModel.Row tableRow in _table.Rows)
            {

                tableRow.GetCells().Add(
                        new PubProfile.TableModel.Cell(
                        "MyPrefix",
                        "MyReference",
                        "MyText",
                        "MyHint",
                        "MySufix"));
             }

            string actual = _tableGenerator.Generate(_table).ToString();

            Assert.AreEqual(actual, Resources.FourXFourHtmlTable);
        }
    }
}
