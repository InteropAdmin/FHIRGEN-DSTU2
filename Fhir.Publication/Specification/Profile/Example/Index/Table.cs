using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;
using Hl7.Fhir.Support;

namespace Hl7.Fhir.Publication.Specification.Profile.Example.Index
{
    internal static class Table
    {
        private const string _relativePath = @"../Examples";

        public static XElement ToHtml(
            string resourceName, 
            string packageName, 
            ImplementationGuide.Base baseResource,
            List<Coding> metaTags)
        {
            XElement table = null;

            if (metaTags != null && GetMetaData(metaTags).Any())
            {
                XElement header = TableHeader.ToHtml(baseResource.ExamplesXml, baseResource.ExamplesJson);

                IEnumerable<Coding> examples = GetMetaData(metaTags);

                var bodyTable = new XElement(
                    XmlNs.XHTMLNS + "div", 
                    new XElement(XmlNs.XHTMLNS + "tr"));

                var rows = new Row(bodyTable);

                foreach (Coding item in examples.OrderByDescending(example => example.Code))
                {
                    bodyTable = rows.AddRow(
                        item.Code,
                        item.Display,
                        baseResource.ExamplesXml,
                        Path.Combine(_relativePath, packageName, string.Concat(item.Code, ".xml")),
                        baseResource.ExamplesJson,
                        Path.Combine(_relativePath, packageName, string.Concat(item.Code, ".json")));
                }

                table = Example.Table.ToHtml(header, bodyTable, resourceName);
            }

            return table;
        }

        private static IEnumerable<Coding> GetMetaData(IEnumerable<Coding> metaTags)
        {
            return
                metaTags.Where(
                    item =>
                        item.System == Urn.Example.GetUrnString())
                    .ToList();
        }
    }
}