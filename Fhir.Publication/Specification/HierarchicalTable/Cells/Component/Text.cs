using System.Xml.Linq;

namespace Hl7.Fhir.Publication.Specification.HierarchicalTable.Cells.Component
{
	internal class Text : CellComponent
    {
		private readonly string _value;

		public Text(string value)
		{
			_value = value;
		}

		public override XObject ToHtml()
		{
			return new XText(_value ?? string.Empty);
		}
	}
}