using System.Collections.Generic;
using System.Linq;
using Hl7.Fhir.Publication.Specification.TableModel;

namespace Hl7.Fhir.Publication.Specification.HierarchicalTable.Cells
{
	internal class TitleCell : Cell
	{
		private readonly Title _title;

		public TitleCell(Title title)
			: base ("th")
		{
			_title = title;
		}

        protected override string Style => _title.Width != 0
            ? base.Style + $"; width:{_title.Width}px"
            : base.Style;

	    protected override IEnumerable<Component.CellComponent> CreateCellComponents()
		{
            Piece piece = Piece;

            if (!string.IsNullOrEmpty(piece.GetHint()))
                yield return new Component.HintText(piece.GetHint(), null, piece.GetText());
            else
                yield return new Component.Text(piece.GetText());
		}

		private Piece Piece => _title
		    .GetPieces()
		    .Single();
	}
}