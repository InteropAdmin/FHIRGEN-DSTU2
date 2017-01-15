using System.Xml.Linq;
using Hl7.Fhir.Support;

namespace Hl7.Fhir.Publication.Specification.Profile.ValueSet
{
    internal static class TableHeader
    {
        public static XElement ToHtml(bool hasDefinition)
        {
            return new XElement(XmlNs.XHTMLNS + "tr",
                new XAttribute("class", "table-header"),
                       new XElement(XmlNs.XHTMLNS + "th", "Code",
                            new XAttribute("title", "Code that identifies concept"),
                            new XAttribute("class", "col-code")),
                       new XElement(XmlNs.XHTMLNS + "th", "Display",
                           new XAttribute("title", "Text to display to the user"),
                           new XAttribute("class", "col-display")),
                       hasDefinition ? new XElement(XmlNs.XHTMLNS + "th", "Definition", new XAttribute("title", "Formal definition"), new XAttribute("class", "col-definition")) : null);
        }
    }
}