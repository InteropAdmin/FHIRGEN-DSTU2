namespace Hl7.Fhir.Publication.Specification.HierarchicalTable.Cells.Component
{
	internal class StyledText : StyledTag
    {
		private readonly Text _text;

		public StyledText(string style, string text)
			: base(style, "span")
		{
			_text = new Text(text);
		}

		protected override void PopulateElement(System.Xml.Linq.XElement element)
		{
			element.Add(_text.ToHtml());
		}
	}
}