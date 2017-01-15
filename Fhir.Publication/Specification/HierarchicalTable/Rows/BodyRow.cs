using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Hl7.Fhir.Publication.Specification.HierarchicalTable.Rows
{
	internal class BodyRow : Row
    {
		private const int _firstCell = 1;

		private readonly TableModel.Row _row;
		private readonly IReadOnlyList<bool> _indents;
        private readonly IImageGenerator _imageGenerator;

        public BodyRow(TableModel.Row row, IReadOnlyList<bool> indents, IImageGenerator imageGenerator)
		{
			_row = row;
			_indents = indents;
            _imageGenerator = imageGenerator;
        }

		protected override XAttribute CreateStyle()
		{
			return new XAttribute(
					"style",
					"border: 0px; padding:0px; vertical-align: top; background-color: white;");
		}

		protected override IEnumerable<Cells.Cell> CreateCells()
		{
			IEnumerable<TableModel.Cell> cells = _row.GetCells();

			if (cells.Any())
			{
                yield return new Cells.IndentedCell(
					indents: _indents,
					hasChildren: _row.GetSubRows().Any(),
					icon: _row.GetIcon(),
					anchor: _row.GetAnchor(),
					pieces: cells.First().GetPieces(),
                    imageGenerator: _imageGenerator);

                foreach (TableModel.Cell cell in cells.Skip(_firstCell))
					yield return new Cells.BodyCell(cell.GetPieces());

			}
		}
	}
}