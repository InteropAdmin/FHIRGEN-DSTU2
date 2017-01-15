using System.Xml.Linq;
using Hl7.Fhir.Support;

namespace Hl7.Fhir.Publication.Specification.Profile.ValueSet.Summary
{
    internal static class TableBody
    {
        public static XElement ToHtml(TableContents summary)
        {
            return
                new XElement(XmlNs.XHTMLNS + "tr",
                    new XElement(XmlNs.XHTMLNS + "td", "Defining URL:"),
                    new XElement(XmlNs.XHTMLNS + "td", summary.DefinitingUrl),
                new XElement(XmlNs.XHTMLNS + "tr",
                    new XElement(XmlNs.XHTMLNS + "td", "Name:"),
                    new XElement(XmlNs.XHTMLNS + "td", summary.Name)),
                new XElement(XmlNs.XHTMLNS + "tr",
                    new XElement(XmlNs.XHTMLNS + "td", "Definition:"),
                    new XElement(XmlNs.XHTMLNS + "td", summary.Definition)),
                !string.IsNullOrEmpty(summary.Oid) ?
                    new XElement(XmlNs.XHTMLNS + "tr",
                        new XElement(XmlNs.XHTMLNS + "td", "OID:"),
                        new XElement(XmlNs.XHTMLNS + "td", summary.Oid))
                    : null,
                !string.IsNullOrEmpty(summary.Copyright) ?
                    new XElement(XmlNs.XHTMLNS + "tr",
                        new XElement(XmlNs.XHTMLNS + "td", "Copyright:"),
                        new XElement(XmlNs.XHTMLNS + "td", summary.Copyright))
                    : null,
                new XElement(XmlNs.XHTMLNS + "tr",
                    new XElement(XmlNs.XHTMLNS + "td", "Status:"),
                    new XElement(XmlNs.XHTMLNS + "td", summary.Status)),
                !string.IsNullOrEmpty(summary.LastUpdated) ?
                    new XElement(XmlNs.XHTMLNS + "tr",
                        new XElement(XmlNs.XHTMLNS + "td", "Last Updated:"),
                        new XElement(XmlNs.XHTMLNS + "td", summary.LastUpdated))
                    : null,
                new XElement(XmlNs.XHTMLNS + "tr",
                    new XElement(XmlNs.XHTMLNS + "td", "Source Resource:"),
                    new XElement(XmlNs.XHTMLNS + "td", summary.SourceResource)));
        }
    }
}