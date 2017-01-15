using System;
using System.Xml.Linq;
using Hl7.Fhir.Publication.Specification.ExtensionMethods;

namespace Hl7.Fhir.Publication.Specification.Profile.ValueSet.Mapping
{
    internal class Row
    {
        private readonly XElement _table;

        public Row(XElement table)
        {
            if (table == null)
                throw new ArgumentNullException(
                    nameof(table));

            _table = table;
        }

        public XElement AddRow(CodeMapping mapping, string resourceName)
        {
            string anchorLink = string.Concat(KnowledgeProvider.GetLinkForLocalResource(resourceName), "#", mapping.Display);

            _table.Add(
                new XElement("tr",
                    new XElement("td", mapping.Code),
                    new XElement("td", mapping.Display),
                    //new XElement("td", mapping.Definition),
                    new XElement("td",
                        new XElement("title", mapping.Equivalence.ToString()),
                        mapping.Equivalence.GetSymbol(),
                        new XElement("a", new XAttribute("href", anchorLink),
                        mapping.Mapping))));

            return _table;
        }
    }
}