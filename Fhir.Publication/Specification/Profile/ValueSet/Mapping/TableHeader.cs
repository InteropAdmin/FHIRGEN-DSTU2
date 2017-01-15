using System.Xml.Linq;
using Hl7.Fhir.Support;

namespace Hl7.Fhir.Publication.Specification.Profile.ValueSet.Mapping
{
    internal static class TableHeader
    {
        public static XElement ToHtml(string resourceName)
        {
            return new XElement(XmlNs.XHTMLNS + "tr",
                new XAttribute("class", "table-header"),
                new XElement("th", "Code",
                     new XAttribute("title", "Code that identifies concept"),
                     new XAttribute("class", "mapping-col-code")),
                new XElement("th", "Display",
                    new XAttribute("title", "Text to display to the user"),
                    new XAttribute("class", "mapping-col-display")),
                //new XElement("th", "Definition",
                //    new XAttribute("title", "Formal definition"),
                //    new XAttribute("class", "mapping-col-definition")),
                new XElement("th", 
                    new XElement("a", "Mapping", 
                        new XAttribute("href", KnowledgeProvider.GetLinkForLocalResource(resourceName))),
                    new XAttribute("title", "Associate element."),
                    new XAttribute("class", "mapping-col-mapping")));
        }
    }
}