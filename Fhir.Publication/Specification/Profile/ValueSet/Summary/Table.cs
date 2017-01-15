using System;
using System.Xml.Linq;
using Hl7.Fhir.Support;

namespace Hl7.Fhir.Publication.Specification.Profile.ValueSet.Summary
{
    internal static class Table
    {
        public static XElement ToHtml(TableContents summary)
        {
            if (summary == null)
                throw new ArgumentNullException(
                    nameof(summary));
             
            return 
                new XElement(XmlNs.XHTMLNS + "div",
                    new XAttribute("class", "valueset-summary-table"),
                    new XElement(XmlNs.XHTMLNS + "table", 
                        TableHeader.ToHtml(), 
                        TableBody.ToHtml(summary)),
                    new XElement(XmlNs.XHTMLNS + "p", summary.Requirement)
                        );       
        }
    }
}