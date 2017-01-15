using System.Xml.Linq;

namespace Hl7.Fhir.Publication.Specification.HierarchicalTable.Cells.Component
{
	internal class Reference : StyledTag
	{
		private readonly string _reference;
		private readonly string _hint;
		private readonly string _text;
				   
		public Reference(string style, string reference, string hint, string text)
			: base(style, "a")
		{
			_reference = reference;
			_hint = hint;
			_text = text;
		}

		protected override void PopulateElement(XElement element)
		{
			element.Add(new XAttribute("href", _reference));

			if (!string.IsNullOrEmpty(_hint))
				element.Add(new XAttribute("title", _hint));

			element.Add(new XText(_text));
		}
	}
}