using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Support;

namespace Hl7.Fhir.Publication.Specification.Page
{
    internal static class BulletList
    {
        public static XElement ToHtml(IEnumerable<string> listItems)
        {
            string[] items = listItems.ToArray();

            if (items.Any())
            {
                var root = new XElement(
                XmlNs.XHTMLNS + "div",
                    new XElement(XmlNs.XHTMLNS + "ul", new XAttribute("class", "list-unstyled")));

                var children = new XElement(XmlNs.XHTMLNS + "listItems");

                foreach (var item in items)
                    children.Add(new XElement(XmlNs.XHTMLNS + "li", item));

                root.Add(children);

                return root;
            }

            return null;
        }

        public static XElement ToHtml(IEnumerable<XElement> listItems)
        {
            XElement[] items = listItems.ToArray();

            if (items.Any())
            {
                var root = new XElement(
                XmlNs.XHTMLNS + "div",
                    new XElement(XmlNs.XHTMLNS + "ul", new XAttribute("class", "list-unstyled")));

                var children = new XElement(XmlNs.XHTMLNS + "listItems");

                foreach (XElement item in items)
                    children.Add(new XElement(XmlNs.XHTMLNS + "li", item));

                root.Add(children);

                return root;
            }

            return null;
        }
    }
}