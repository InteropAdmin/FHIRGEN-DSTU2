using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Specification.ExtensionMethods;
using Hl7.Fhir.Publication.Specification.Page;
using Hl7.Fhir.Support;

namespace Hl7.Fhir.Publication.Specification.Profile.ValueSet
{
    internal class Composition : Definition
    {
        private readonly List<string> _definition;
        private XElement _table;

        public Composition(Model.ValueSet valueset, Log log)
            : base(valueset, log)
        {
            _definition = new List<string>();
            CompositionValidator.IsValid(valueset);
        }

        private static XElement Intro => new XElement(
                    XmlNs.XHTMLNS + "p", "This value set includes codes from the following code systems:",
                    new XElement(XmlNs.XHTMLNS + "br"));

        public override XElement Description
        {
            get
            {
                Log.Info("     Get Content Logical Definition");

                var definition = new XElement(XmlNs.XHTMLNS + "div", new XAttribute("class", "logical-definition"));

                definition.Add(Intro);
                definition.Add(Include);
                definition.Add(Import);

                return definition;
            }
        }

        public XElement DescriptionMulti
        {
            get
            {
                Log.Info("     Get Content Logical Definition");

                var definition = new XElement(XmlNs.XHTMLNS + "div", new XAttribute("class", "logical-definition"));

                // Only include the following text is there is a ValueSet.codeSystem
                if (Valueset.CodeSystem != null)
                {
                    definition.Add(
                        new XElement(
                        XmlNs.XHTMLNS + "p", "In addition, this value set includes codes from other code systems.",
                        new XElement(XmlNs.XHTMLNS + "br"))
                        );
                }

                foreach (var item in Valueset.Compose.Include)
                {
                    definition.Add(
                       new XElement(
                       XmlNs.XHTMLNS + "p", "Include these codes as defined in " + item.System + " :",
                       new XElement(XmlNs.XHTMLNS + "br"))
                       );

                    string System = item.System.Split('/').Last();
                    _table = new XElement(XmlNs.XHTMLNS + "div", new XAttribute("class", "table-rows"));


                    IEnumerable<Concept> Concepts = item.Concept
                     .Select(
                         concept =>
                             new Concept(System, concept.Code, concept.Display, "xxx"))
                     .ToList();

                    foreach (Concept concept in Concepts)
                        _table.Add(Row.ToHtml(concept, false));

                    definition.Add(
                        Table.ToHtml(TableHeader.ToHtml(false), _table)
                        );
                }

                return definition;
            }
        }

        private XElement Include
        {
            get
            {
                var include = new XElement(XmlNs.XHTMLNS + "div", new XAttribute("class", "include"));

                XElement filters = GetFilters();
                include.Add(filters);
                if (filters != null)
                    return include;
                else
                    return null;
            }
        }

        private XElement Import
        {
            get
            {
                var import = new XElement(XmlNs.XHTMLNS + "div", new XAttribute("class", "import"));

                import.Add(GetImports());

                return GetImports() != null ? import : null;
            }
        }

        private XElement GetImports()
        {
            var externalRefs = new List<XElement>();

            foreach (string uri in Valueset.Compose.Import)
                externalRefs.Add(
                    new XElement("p", "Externally maintained code system: ",
                        new XElement("a",
                            new XAttribute("href", uri), $"{ uri }")));

            return externalRefs.Any() ? BulletList.ToHtml(externalRefs) : null;
        }

        private XElement GetFilters()
        {
            _definition.Clear();

            foreach (Model.ValueSet.ConceptSetComponent include in Valueset.Compose.Include)
            {
                CompositionValidator.ValidateFilters(include, Valueset.Name);

                foreach (Model.ValueSet.FilterComponent filter in include.Filter)
                    _definition.Add(
                        $" Include codes from {include.System} where {filter.Property} {filter.Op.GetFilterOperatorString()} {filter.Value}");
            }

            return _definition.Any() ? BulletList.ToHtml(_definition) : null;
        }

        private XElement GetConcepts()
        {
            _definition.Clear();

            foreach (Model.ValueSet.ConceptSetComponent include in Valueset.Compose.Include)
            {
                CompositionValidator.ValidateConcepts(include, Valueset.Name);

                foreach (Model.ValueSet.ConceptReferenceComponent concept in include.Concept)
                    _definition.Add(
                         $@" Include {include.System} where code is {concept.Code} and display is '{concept.Display}'");
            }

            return _definition.Any() ? BulletList.ToHtml(_definition) : null;
        }
    }
}