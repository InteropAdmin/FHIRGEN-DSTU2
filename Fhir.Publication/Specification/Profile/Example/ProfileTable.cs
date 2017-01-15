using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Hl7.Fhir.Support;

namespace Hl7.Fhir.Publication.Specification.Profile.Example
{
    internal static class ProfileTable
    {
        private const string _relativePath = @"..\Examples";

        public static XElement ToHtml(
              ImplementationGuide.Base baseResource,
              string resourceName,
              string packageName,
              IEnumerable<Description> examples)
        {
            var bodyTable = new XElement(XmlNs.XHTMLNS + "div",
                new XElement(XmlNs.XHTMLNS + "tr"));

            var rows = new Row(bodyTable);

            foreach (Description example in examples)
            {
                bodyTable = rows.AddRow(
                    example.Name,
                    example.Display,
                    baseResource.ExamplesXml,
                    Path.Combine(_relativePath, packageName, string.Concat(example.Name, ".xml")),
                    baseResource.ExamplesJson,
                    Path.Combine(_relativePath, packageName, string.Concat(example.Name, ".json")));
            }

            XElement header = TableHeader.ToHtml(baseResource.ExamplesXml, baseResource.ExamplesJson);

            XElement table = Table.ToHtml(header, bodyTable, resourceName);

            return table;
        }
    }
}