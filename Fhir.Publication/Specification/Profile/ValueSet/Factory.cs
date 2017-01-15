using System;
using System.Xml.Linq;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Specification.ExtensionMethods;
using Hl7.Fhir.Publication.Specification.Page;
using Hl7.Fhir.Support;
using Hl7.Fhir.Publication.Specification.Profile.ValueSet.Mapping;
using Hl7.Fhir.Publication.Specification.Profile.ValueSet.Summary;

namespace Hl7.Fhir.Publication.Specification.Profile.ValueSet
{
    internal class Factory
    {
        private readonly Log _log;
        private readonly ImplementationGuide.ResourceStore _resourceStore;
        private readonly bool _hasXmlResource;
        private readonly bool _hasJsonResource;
        private readonly string _packageName;
        private Model.ValueSet _valueset;

        public Factory(
            Log log, 
            ImplementationGuide.ResourceStore resourceStore,
            bool hasXmlResource,
            bool hasJsonResource,
            string packageName) 
        {
            if (log == null)
                throw new ArgumentNullException(
                    nameof(log));

            if (resourceStore == null)
                throw new ArgumentNullException(
                    nameof(resourceStore));

            if (string.IsNullOrEmpty(packageName))
                throw new ArgumentException(
                    nameof(packageName));

            _log = log;
            _resourceStore = resourceStore;
            _hasXmlResource = hasXmlResource;
            _hasJsonResource = hasJsonResource;
            _packageName = packageName;
        }
        
        private XElement Summary
        {
            get
            {
                _log.Info("     Get Summary table");

                return ValueSet.Summary.Table.ToHtml(new TableContents(_valueset, _hasXmlResource, _hasJsonResource, _packageName));
            }
        }
        
        public XElement GenerateValueset(Model.ValueSet valueset, string fullName)
        {
            _valueset = valueset;

            _log.Debug($"Creating valueset for {_valueset.Name} - {_valueset.Url}");

            if (Validator.IsValid(_valueset, _log))
            {
                var root = new XElement(XmlNs.XHTMLNS + "div", new XAttribute("class", "valueset-content"));

                root.Add(
                    new XElement(
                        XmlNs.XHTMLNS + "div", 
                        new XAttribute("class", "valueset-summary"), 
                        new XElement("h3"), "Summary"),
                        Summary);

                root.Add(Composition);
                root.Add(CodeSystem);
                root.Add(CompositionMultiCodeSystem);
                root.Add(ContainedResource);

                XElement definition = HeadedPanel.ToHtml(fullName, root);

                return definition;
            }

            return null;
        }

        private XElement Composition => _valueset.IsComposition() ? new Composition(_valueset, _log).Description : null;

        private XElement CompositionMultiCodeSystem => _valueset.IsCompositionMultiCodeSystem() ? new Composition(_valueset, _log).DescriptionMulti : null;

        private XElement CodeSystem => _valueset.IsCodesystem() ? new CodeSystem(_valueset, _log).Description : null;

        private XElement ContainedResource => _valueset.HasContainedResourse() ? new ContainedResource(_valueset, _log, _resourceStore, _packageName).Description : null;
    }
}