using System.Collections.Generic;
using System.Xml.Linq;
using Hl7.Fhir.Support;

namespace Hl7.Fhir.Publication.Specification.HierarchicalTable.Cells.Component
{
    internal class SpanClass : StyledTag
    {
        private readonly string _labelType;
        private readonly string _text;

        public SpanClass(string labelType, string text)
            : base(null, "span")
        {
            _labelType = labelType;
            _text = text;
        }
        //private IEnumerable<XAttribute> CreateAttributes()
        //{
        //    yield return new XAttribute("class", "label label-" + _labelType);
        //}

        protected override void PopulateElement(XElement element)
        {
            element.Add(new XAttribute("class", "label label-" + _labelType + " label-big"));
            element.Add(new XText(_text));
        }

    }
}