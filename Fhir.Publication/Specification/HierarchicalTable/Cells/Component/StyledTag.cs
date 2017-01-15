using System.Xml.Linq;
using Hl7.Fhir.Support;

namespace Hl7.Fhir.Publication.Specification.HierarchicalTable.Cells.Component
{
	internal class StyledTag : CellComponent
    {
		private readonly string _style;
		private readonly string _tag;

		public StyledTag(string style, string tag)
		{
			_style = style;
			_tag = tag;
		}

		public override XObject ToHtml()
		{
			var styledComponent = new XElement(XmlNs.XHTMLNS + _tag);

            if (!string.IsNullOrEmpty(_style))
			    styledComponent.Add(new Style(_style).ToHtml());

			PopulateElement(styledComponent);

			return styledComponent;
		}

		protected virtual void PopulateElement(XElement element)
		{
		}
	}
}