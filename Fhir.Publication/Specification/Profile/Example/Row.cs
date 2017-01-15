using System;
using System.Xml.Linq;
using Hl7.Fhir.Support;

namespace Hl7.Fhir.Publication.Specification.Profile.Example
{
    internal class Row
    {
        private const string _xmlFileName = "file-xml-xs.png";
        private const string _jsonFileName = "file-json-xs.png";
        private readonly XElement _table;

        public Row(XElement table)
        {
            if (table == null)
                throw new ArgumentNullException(
                    nameof(table));

            _table = table;
        }

        public XElement AddRow(
            string exampleName,
            string description,
            bool xmlFilesRequired,
            string xmlFileName,
            bool jsonFilesRequired,
            string jsonFileName)
        {
            _table.Element(XmlNs.XHTMLNS + "tr")
                ?.AddAfterSelf(
                    new XElement(XmlNs.XHTMLNS + "tr",
                        new XElement(XmlNs.XHTMLNS + "td", exampleName),
                            new XElement(XmlNs.XHTMLNS + "td"), description,
                            xmlFilesRequired ?
                                new XElement(XmlNs.XHTMLNS + "td",
                                    new XElement(XmlNs.XHTMLNS + "a",
                                        new XAttribute("target", "_blank"),
                                        new XAttribute("href", xmlFileName)),
                                        new XElement("img",
                                            new XAttribute("src", string.Concat(KnowledgeProvider.RelativeImagesPath,_xmlFileName))))
                                            : null,
                             jsonFilesRequired ?
                                new XElement(XmlNs.XHTMLNS + "td",
                                    new XElement(XmlNs.XHTMLNS + "a",
                                        new XAttribute("target", "_blank"),
                                        new XAttribute("href", jsonFileName)),
                                        new XElement("img",
                                            new XAttribute("src", string.Concat(KnowledgeProvider.RelativeImagesPath,_jsonFileName))))
                                            : null));
          
            return _table;
        }
    }
}