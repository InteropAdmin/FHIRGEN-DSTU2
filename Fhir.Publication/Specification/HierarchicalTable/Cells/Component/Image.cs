using System.Collections.Generic;
using System.Xml.Linq;
using Hl7.Fhir.Support;

namespace Hl7.Fhir.Publication.Specification.HierarchicalTable.Cells.Component
{
	internal class Image : CellComponent
	{
		private readonly string _icon;
		private readonly string _style;

		public Image(string icon)
			: this(icon, null)
		{
		}

	    protected Image(string icon, string style)
		{
			_icon = icon;
			_style = style;
		}

		public string Icon => _icon;

	    public override XObject ToHtml()
		{
			return new XElement(XmlNs.XHTMLNS + "img", CreateAttributes());
		}

		private IEnumerable<XAttribute> CreateAttributes()
		{
			yield return new XAttribute("src", SrcFor(_icon));

			yield return new XAttribute("class", "hierarchy");

			if(!string.IsNullOrEmpty(_style))
				yield return new XAttribute("style", _style);

			yield return new XAttribute("alt", ".");
		}

		private static string SrcFor(string filename)
		{
			return 
				string.Concat(Profile.KnowledgeProvider.RelativeImagesPath, filename);
		}
	}
}