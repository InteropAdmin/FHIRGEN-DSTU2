using System;
using System.Linq;

namespace Hl7.Fhir.Publication.Specification.HierarchicalTable
{
    internal static class ModelValidator
    {
        public static void Validate(TableModel.Model model)
        {
            if (!model.Rows.Any())
                throw new ArgumentException("table model must have rows!");

            if (!model.Titles.Any())
                throw new ArgumentException("table model must have titles!");

            foreach (TableModel.Title title in model.Titles)
                ValidateTitles(title);

            foreach (TableModel.Row row in model.Rows)
                ValidateColumns(row, model.Titles.Count);
        }

        private static void ValidateTitles(TableModel.Cell cell)
        {
            bool hasText = false;

            foreach (TableModel.Piece piece in cell.GetPieces())
                if (!string.IsNullOrEmpty(piece.GetText()))
                    hasText = true;

            if (!hasText)
                throw new InvalidOperationException("title cells must have text!");
        }

        private static void ValidateColumns(TableModel.Row row, int totalTitles)
        {
            if (row.GetCells().Count != totalTitles)
                throw new ArgumentException("All rows must have the same number of columns as the titles");

            foreach (TableModel.Row subRow in row.GetSubRows())
                ValidateColumns(subRow, totalTitles);
        }
    }
}