using System.Collections.Generic;
using Hl7.Fhir.Publication.Specification.TableModel;

namespace Hl7.Fhir.Publication.Specification.HierarchicalTable.Cells
{
	internal class LeadingCell : BodyCell
	{
		private readonly string _icon;
		private readonly string _anchor;

	    protected LeadingCell(
			string icon,
			string anchor,
			IEnumerable<Piece> pieces)
			: base(pieces)
		{
			_icon = icon;
			_anchor = anchor;
		}

		protected override IEnumerable<Component.CellComponent> CreateCellComponents()
		{
			if (!string.IsNullOrEmpty(_icon))
				yield return new Component.Icon(_icon);

			foreach (Component.CellComponent cellComponent in base.CreateCellComponents())
				yield return cellComponent;

			if (!string.IsNullOrEmpty(_anchor))
				yield return new Component.Anchor(_anchor);
		}
	}
}