using System.Xml.Linq;
using Hl7.Fhir.Support;

namespace Hl7.Fhir.Publication.Specification.Profile.ValueSet.Summary
{
    internal static class TableHeader
    {
        public static XElement ToHtml()
        {
            return new XElement(XmlNs.XHTMLNS + "tr",
                new XAttribute("class", "valueset-summary-table-header"),
                       new XElement(XmlNs.XHTMLNS + "th", "Name",
                            new XAttribute("title", "The name of the element."),
                            new XAttribute("class", "col-name")),
                       new XElement(XmlNs.XHTMLNS + "th", "Description",
                           new XAttribute("title", "The value of the element."),
                           new XAttribute("class", "col-description")));
        }
    }
}