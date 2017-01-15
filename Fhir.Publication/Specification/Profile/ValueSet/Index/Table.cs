using System.Xml.Linq;

namespace Hl7.Fhir.Publication.Specification.Profile.ValueSet.Index
{
    internal static class Table
    {
        public static XElement ToHtml(XElement header, XElement body)
        {
            var table = new XElement("div", new XAttribute("class", "valueset-index-table"),
                new XElement("table",
                    header,
                    body));

            return table;
        }
    }
}