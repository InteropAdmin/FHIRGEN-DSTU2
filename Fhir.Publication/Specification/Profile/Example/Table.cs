using System.Xml.Linq;

namespace Hl7.Fhir.Publication.Specification.Profile.Example
{
    internal static class Table
    {
        public static XElement ToHtml(XElement header, XElement body, string name)
        {
            var table = new XElement("div", new XAttribute("class", "example-table"),
                new XElement("p",
                    new XElement("b", name)),
                new XElement("table",
                    header,
                    body));

            return table;
        }
    }
}
