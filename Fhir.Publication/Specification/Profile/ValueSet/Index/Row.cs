using System.IO;
using System.Xml.Linq;
using Hl7.Fhir.Support;

namespace Hl7.Fhir.Publication.Specification.Profile.ValueSet.Index
{
    internal static class Row
    {
        private const string _relativePath = @"../Resources";
        private const string _xmlFileName = "file-xml-xs.png";
        private const string _jsonFileName = "file-json-xs.png";

        public static XElement ToHtml(
            string name,
            string description,
            string fileName,
            string packageName,
            ImplementationGuide.Base baseResource)
        {
            string xmlFileName = Path.Combine(_relativePath, packageName, "ValueSets", string.Concat(fileName, ".xml"));

            string jsonFileName = Path.Combine(_relativePath, packageName, "ValueSets", string.Concat(fileName, ".json"));

            return new XElement(XmlNs.XHTMLNS + "tr",
                      new XElement(XmlNs.XHTMLNS + "td", name),
                          new XElement(XmlNs.XHTMLNS + "td"), description,
                          baseResource.ValuesetsInXml ?
                              new XElement(XmlNs.XHTMLNS + "td",
                                  new XElement(XmlNs.XHTMLNS + "a",
                                      new XAttribute("target", "_blank"),
                                      new XAttribute("href", xmlFileName)),
                                      new XElement("img",
                                          new XAttribute("src", string.Concat(KnowledgeProvider.RelativeImagesPath, _xmlFileName))))
                                          : null,
                           baseResource.ValuesetsInJson ?
                              new XElement(XmlNs.XHTMLNS + "td",
                                  new XElement(XmlNs.XHTMLNS + "a",
                                      new XAttribute("target", "_blank"),
                                      new XAttribute("href", jsonFileName)),
                                      new XElement("img",
                                          new XAttribute("src", string.Concat(KnowledgeProvider.RelativeImagesPath, _jsonFileName))))
                                          : null);
        }
    }
}