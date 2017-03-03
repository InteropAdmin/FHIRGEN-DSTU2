using System;
using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;
using Hl7.Fhir.Support;

namespace Hl7.Fhir.Publication.Specification.Profile.ValueSet.Index
{
    internal class Factory
    {
        private readonly Log _log;
        private readonly IDirectoryCreator _directoryCreator;
        private XElement _xhtml;
        private ImplementationGuide.Base _baseResource;

        public Factory(
            KnowledgeProvider profileKnowledgeProvider,
            Log log,
            IDirectoryCreator directoryCreator)
        {
            if (profileKnowledgeProvider == null)
                throw new ArgumentNullException(
                    nameof(profileKnowledgeProvider));

            if (log == null)
                throw new ArgumentNullException(
                    nameof(log));

            if (directoryCreator == null)
                throw new ArgumentNullException(
                    nameof(directoryCreator));
            _log = log;
            _directoryCreator = directoryCreator;
        }

        public XElement Generate(
          string sourceDirectory,
          ImplementationGuide.Package package,
          ImplementationGuide.Base baseResource)
        {
            _baseResource = baseResource;

            var generator = new ResourceGenerator(_directoryCreator, baseResource.ValuesetsInXml, baseResource.ValuesetsInJson, _log);
            
            _xhtml = new XElement(
                XmlNs.XHTMLNS + "div",
                new XElement(XmlNs.XHTMLNS + "br"),
                new XElement(XmlNs.XHTMLNS + "h3", package.Name.Split('.').Last()));

            XElement header = TableHeader.ToHtml(baseResource.ValuesetsInXml, baseResource.ValuesetsInJson);

            XElement body = GetValueSets(package, generator, sourceDirectory);

            _xhtml.Add(Table.ToHtml(header, body));

            return _xhtml;
        }

        private XElement GetValueSets(ImplementationGuide.Package package, ResourceGenerator generator, string sourceDir)
        {
            var table = new XElement(XmlNs.XHTMLNS + "div");

            foreach (Model.ValueSet valueSet in package.ValueSets)
            {
                // if (valueSet.Url.Contains(Url.FhirValueSet.GetUrlString()))
                if (LocalValueSet(valueSet.Url))
                {
                    var fileName = valueSet.Url.Split('/').Last();

                    table.Add(Row.ToHtml(valueSet.Name, valueSet.Description, fileName, package.Name, _baseResource));
                    
                    generator.Generate(fileName, sourceDir);
                }
            }

            return table;
        }

        private bool LocalValueSet(string url)
        {
            bool res = false;

            if (url.Contains(Url.FhirValueSet.GetUrlString())) res = true;
            if (url.Contains(Url.FhirNHSUKValueSet.GetUrlString())) res = true;
            if (url.Contains(Url.FhirHL7UKValueSet.GetUrlString())) res = true;

            return res;
        }
    }
}