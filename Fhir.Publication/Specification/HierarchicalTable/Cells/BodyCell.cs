using System.Collections.Generic;
using Hl7.Fhir.Publication.Specification.HierarchicalTable.Cells.Component;
using Hl7.Fhir.Publication.Specification.TableModel;

namespace Hl7.Fhir.Publication.Specification.HierarchicalTable.Cells
{
	internal class BodyCell : Cell
	{
		private readonly IEnumerable<Piece> _pieces;

		public BodyCell(IEnumerable<Piece> pieces)
			: base("td")
		{
			_pieces = pieces;
		}

		protected override IEnumerable<CellComponent> CreateCellComponents()
		{
			if (_pieces != null)
			{
				foreach (Piece piece in _pieces)
				{
					if (!string.IsNullOrEmpty(piece.GetTag()))
					{
						yield return new StyledTag(piece.GetStyle(), piece.GetTag());
					}
					else if (!string.IsNullOrEmpty(piece.GetReference()))
					{
						yield return new Reference(piece.GetStyle(), piece.GetReference(), piece.GetHint(), piece.GetText());
					}
					else if (!string.IsNullOrEmpty(piece.GetHint()))
					{
						yield return new HintText(piece.GetHint(), piece.GetStyle(), piece.GetText());
					}
					else if (piece.GetStyle() != null)
					{
						yield return new StyledText(piece.GetStyle(), piece.GetText());
					}
                    else if (piece.GetLabel() != null)
                    {
                        yield return new SpanClass(piece.GetLabel(), piece.GetText());
                    }
                    else
					{
						yield return new Text(piece.GetText());
					}
				}
			}
		}
	}
}