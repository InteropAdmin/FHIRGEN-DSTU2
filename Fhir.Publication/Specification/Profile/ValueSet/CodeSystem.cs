using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Support;

namespace Hl7.Fhir.Publication.Specification.Profile.ValueSet
{
    internal class CodeSystem : Definition
    {
        private readonly XElement _table;
        private XElement _definition;

        public CodeSystem(Model.ValueSet valueset, Log log)
            :base(valueset, log)
        {
            CodeSystemValidator.IsValid(valueset);
            _table = new XElement(XmlNs.XHTMLNS + "div", new XAttribute("class", "table-rows"));
        }

        private string System => Valueset.CodeSystem.System.Split('/').Last();

        private bool HasDefinition => Concepts.Any(concept => !string.IsNullOrEmpty(concept.Definition));
        
        private IEnumerable<Concept> Concepts => Valueset.CodeSystem.Concept
                .Select(
                    concept =>
                        new Concept(System, concept.Code, concept.Display, concept.Definition))
                .ToList();

        private XElement Intro => 
            new XElement(XmlNs.XHTMLNS + "div",
                new XElement(XmlNs.XHTMLNS + "br"),
                new XElement(XmlNs.XHTMLNS + "p", $"This valueset includes all the codes for {Valueset.Name} ( {Valueset.CodeSystem.System} ) ",
                new XElement(XmlNs.XHTMLNS + "br")));
           

        public override XElement Description
        {
            get
            {
                Log.Info("     Get concept values");

                _definition = new XElement("div", new XAttribute("class", "in-line-valueset"));

                _definition.Add(Intro);
                _definition.Add(GetCodeSystemTable());

                return _definition;
            }
        }

        private XElement GetCodeSystemTable()
        {
            foreach (Concept concept in Concepts)
                _table.Add(Row.ToHtml(concept, HasDefinition));

            return Table.ToHtml(TableHeader.ToHtml(HasDefinition), _table);
        }
    }
}