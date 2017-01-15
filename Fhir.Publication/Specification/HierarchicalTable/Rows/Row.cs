using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Support;

namespace Hl7.Fhir.Publication.Specification.HierarchicalTable.Rows
{
	internal abstract class Row
	{
		private IEnumerable<Cells.Cell> _cells;

		protected abstract XAttribute CreateStyle();
		protected abstract IEnumerable<Cells.Cell> CreateCells();

		public IEnumerable<Cells.Cell> Cells => _cells ?? (_cells = CreateCells());

	    public XObject ToHtml()
		{
			var row = new XElement(XmlNs.XHTMLNS  + "tr", CreateStyle());

			foreach(XObject cell in Cells.Select(c => c.ToHtml()))
			{
				row.Add(cell);
			}

			return row;
		}
	}
}