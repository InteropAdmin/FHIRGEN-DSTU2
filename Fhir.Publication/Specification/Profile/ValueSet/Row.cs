using System.Xml.Linq;
using Hl7.Fhir.Support;

namespace Hl7.Fhir.Publication.Specification.Profile.ValueSet
{
    internal static class Row
    {
        public static XElement ToHtml(Concept concept, bool hasDefinition)
        {
            return
                new XElement(XmlNs.XHTMLNS + "tr",
                    new XElement(XmlNs.XHTMLNS +  "td", concept.Code),
                    new XElement(XmlNs.XHTMLNS +  "td",
                     new XElement(XmlNs.XHTMLNS + "p", concept.Display),
                        new XElement(XmlNs.XHTMLNS + "a", 
                            new XAttribute("name", concept.Display)), 
                    hasDefinition ? new XElement(XmlNs.XHTMLNS + "td", concept.Definition) : null));
        }
    }
}