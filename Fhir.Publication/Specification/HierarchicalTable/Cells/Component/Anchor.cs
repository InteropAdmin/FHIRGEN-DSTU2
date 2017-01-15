using System.Xml.Linq;
using Hl7.Fhir.Support;

namespace Hl7.Fhir.Publication.Specification.HierarchicalTable.Cells.Component
{
	internal class Anchor : CellComponent
	{
		private readonly string _anchor;

		public Anchor(string anchor)
		{
			_anchor = anchor;
		}

		public override XObject ToHtml()
		{
			return new XElement(
				XmlNs.XHTMLNS + "a", 
				new XAttribute("name", TokenizeName(_anchor)));
		}

		private static string TokenizeName(string anchor)
		{
			return anchor.Replace("[", "_").Replace("]", "_");
		}
	}
}