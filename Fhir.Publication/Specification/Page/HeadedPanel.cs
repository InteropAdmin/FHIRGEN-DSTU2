using System.Xml.Linq;
using Hl7.Fhir.Support;

namespace Hl7.Fhir.Publication.Specification.Page
{
    internal static class HeadedPanel
    { 
        public static XElement ToHtml(string title, XElement body)
        {
            var elements = new XElement(
                XmlNs.XHTMLNS + "div", new XAttribute("class", "panel panel - default"),
                    new XElement(XmlNs.XHTMLNS + "div", new XAttribute("class", "panel-heading"),
                        new XElement(XmlNs.XHTMLNS + "h3", new XAttribute("class", "panel-title"), title)),
                    new XElement(XmlNs.XHTMLNS + "div", new XAttribute("class", "panel-body"),
                    body));

            return elements;
        }
    }
}