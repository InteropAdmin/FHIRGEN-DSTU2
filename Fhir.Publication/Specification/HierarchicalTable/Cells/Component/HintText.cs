using System.Xml.Linq;

namespace Hl7.Fhir.Publication.Specification.HierarchicalTable.Cells.Component
{
	internal class HintText : StyledText
	{
		private readonly string _hint;

		public HintText(string hint, string style, string text)
			: base(style, text)
		{
			_hint = hint;
		}

		protected override void PopulateElement(XElement element)
		{
			element.Add(new XAttribute("title", _hint));
			base.PopulateElement(element);
		}
	}
}