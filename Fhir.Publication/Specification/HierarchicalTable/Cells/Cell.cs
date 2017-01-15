using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Support;

namespace Hl7.Fhir.Publication.Specification.HierarchicalTable.Cells
{
	internal abstract class Cell
	{
		private readonly string _name;
		private IEnumerable<Component.CellComponent> _cellComponents;

		protected Cell(string name)
		{
			_name = name;
		}

		protected abstract IEnumerable<Component.CellComponent> CreateCellComponents();
        protected virtual string Style => "vertical-align:top; text-align:left; padding:0px 4px 0px 4px";

	    public IEnumerable<Component.CellComponent> CellComponents => _cellComponents ?? (_cellComponents = CreateCellComponents());

	    public XObject ToHtml()
		{
			var cell = new XElement(XmlNs.XHTMLNS + _name, new XAttribute("class", "hierarchy"));

            cell.Add(new XAttribute("style", Style));

			foreach (var cellComponent in CellComponents.Select(c => c.ToHtml()))
			{
				cell.Add(cellComponent);
			}

			return cell;
		}
	}
}