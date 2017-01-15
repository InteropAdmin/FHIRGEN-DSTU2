using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Publication.Specification.TableModel;

namespace Hl7.Fhir.Publication.Specification.HierarchicalTable.Rows
{
	internal class TitleRow : Row
	{
		private readonly IEnumerable<Title> _titles;

		public TitleRow(IEnumerable<Title> titles)
		{
			_titles = titles;
		}

		protected override IEnumerable<Cells.Cell> CreateCells()
		{
			return _titles.Select(title => new Cells.TitleCell(title));
		}

		protected override XAttribute CreateStyle()
		{
			return new XAttribute(
					"style",
					"border: 1px #F0F0F0 solid; font-size: 11px; font-family: verdana; vertical-align: top;");
		}
	}
}