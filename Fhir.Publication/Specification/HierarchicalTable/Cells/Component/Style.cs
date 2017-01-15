using System;
using System.Xml.Linq;

namespace Hl7.Fhir.Publication.Specification.HierarchicalTable.Cells.Component
{
	internal class Style : CellComponent
    {
		private readonly string _style;

		public Style(string style)
		{
            if (style == null)
                throw new ArgumentNullException(nameof(style));

			_style = style;
		}

		public override XObject ToHtml()
		{
			return new XAttribute("style", _style);
		}
	}
}