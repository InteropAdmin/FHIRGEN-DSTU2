using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Publication.Specification.HierarchicalTable.Rows;
using Hl7.Fhir.Support;

namespace Hl7.Fhir.Publication.Specification.Page
{
	internal class Table 
	{
		private readonly IEnumerable<Row> _rows;

		public Table(IEnumerable<Row> rows)
		{
			_rows = rows;
		}

		public XElement ToHtml()
		{
            var table = new XElement(XmlNs.XHTMLNS + "table");

            foreach (XObject row in _rows.Select(r => r.ToHtml()))
				table.Add(row);

			return table;
		}
    }
}