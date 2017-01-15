using System.Xml.Linq;
using Hl7.Fhir.Support;

namespace Hl7.Fhir.Publication.Specification.Profile.Example
{
    internal static class TableHeader
    {
        public static XElement ToHtml(bool xmlFiles, bool jsonFiles)
        {

            return new XElement(XmlNs.XHTMLNS + "tr",
                new XElement(XmlNs.XHTMLNS + "th", "Examples",
                     new XAttribute("class", "col-name")),
                new XElement(XmlNs.XHTMLNS + "th", "Description",
                    new XAttribute("class", "col-description")),
               xmlFiles ? new XElement(XmlNs.XHTMLNS + "th", "XML File", new XAttribute("class", "col-xml-file")) : null,
               jsonFiles ? new XElement(XmlNs.XHTMLNS + "th", "JSON File", new XAttribute("class", "col-json-file")) : null);
        }
    }
}