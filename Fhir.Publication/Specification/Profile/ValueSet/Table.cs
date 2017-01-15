using System.Xml.Linq;
using Hl7.Fhir.Support;

namespace Hl7.Fhir.Publication.Specification.Profile.ValueSet
{
    internal static class Table
    {
        public static XElement ToHtml(XElement header, XElement body)
        {
            return new XElement(XmlNs.XHTMLNS + "div", new XAttribute("class", "codeSystem-table"),
                        new XElement(XmlNs.XHTMLNS + "table", header, body));

        }
    }
}