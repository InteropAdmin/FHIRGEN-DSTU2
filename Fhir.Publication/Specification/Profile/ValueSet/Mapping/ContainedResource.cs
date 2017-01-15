using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Support;

namespace Hl7.Fhir.Publication.Specification.Profile.ValueSet.Mapping
{
    internal class ContainedResource : Definition
    {
        private readonly ImplementationGuide.ResourceStore _resourceStore;
        private readonly XElement _header;
        private readonly SourceValueset _source;
        private readonly string _packageName;
        private XElement _mapDefinition;
        private XElement _contained;
        private Row _rows;
        private IEnumerable<CodeMapping> _mappedResources;
        private XElement _bodyTable;

        public ContainedResource(
            Model.ValueSet valueset,
            Log log,
            ImplementationGuide.ResourceStore resourceStore,
            string packageName)
            : base(valueset, log)
        {
            if (resourceStore == null)
                throw new ArgumentNullException(
                    nameof(resourceStore));

            if (string.IsNullOrEmpty(packageName))
                throw new ArgumentException(
                    nameof(packageName));

            _resourceStore = resourceStore;
            _packageName = packageName;

            ContainedResourceValidator.IsValid(valueset);

            _source = new SourceValueset(Valueset.Contained, Valueset.CodeSystem, Valueset.Compose);
            
            _header = TableHeader.ToHtml(_source.TargetName); 
        }

        private ConceptMapper Mapper => new ConceptMapper(_resourceStore, _source, Valueset.Name, _packageName, Log);

        public override XElement Description
        {
            get
            {
               Log.Info("     Get contained resources");

                var definition = new XElement(XmlNs.XHTMLNS + "div", new XAttribute("class", "contained-resources"));
                
                _mapDefinition = new XElement(XmlNs.XHTMLNS + "div", new XAttribute("class", "concept-map"));



                _mapDefinition.Add(Mapping);

                definition.Add(
                        new XElement(
                        XmlNs.XHTMLNS + "p", "This valueset includes a mapping to a seperate valueset",
                        new XElement(XmlNs.XHTMLNS + "br"))
                        );

                definition.Add(_mapDefinition);

                return definition;
            }
        }

        private XElement Mapping
        {
            get
            {
                _bodyTable = new XElement(XmlNs.XHTMLNS + "tr");

                _mappedResources = Mapper.MapResources();
                _rows = new Row(_bodyTable);

                foreach (CodeMapping item in _mappedResources)
                    _bodyTable = _rows.AddRow(item, _source.TargetName);

                _contained = new XElement(XmlNs.XHTMLNS + "div", new XAttribute("class", "resource-mapping"));

                _contained.AddFirst(Table.ToHtml(_header, _bodyTable));

                return 
                    _contained; 
            }
        }
    }
}