using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;
using Hl7.Fhir.Publication.Specification.ExtensionMethods;
using Hl7.Fhir.Support;

namespace Hl7.Fhir.Publication.Specification.Profile.ValueSet.Summary
{
    internal class TableContents
    {
        private readonly Model.ValueSet _valueset;
        private readonly bool _hasXmlResource;
        private readonly bool _hasJsonResource;
        private readonly string _packageName;

        public TableContents(
            Model.ValueSet valueset,
            bool hasXmlResource,
            bool hasJsonResource,
            string packageName)
        {
            if (valueset == null)
                throw new ArgumentNullException(
                    nameof(valueset));

            if (string.IsNullOrEmpty(packageName))
                throw new ArgumentException(
                    nameof(packageName));

            _valueset = valueset;
            _hasXmlResource = hasXmlResource;
            _hasJsonResource = hasJsonResource;
            _packageName = packageName;
        }

        public string DefinitingUrl => _valueset.Url;

        public string Name => _valueset.Name;

        public string Definition => _valueset.Description;

        public string Requirement => _valueset.Requirements;

        public string Oid => _valueset.Extension
            .SingleOrDefault(
                ext =>
                    ext.Value.ToString()
                    .StartsWith(Urn.Oid.GetUrnString()))
                    ?.Value.ToString()
                    .Split(':')
                    .Last();

        public string Copyright => _valueset.Copyright;

        public string Status => _valueset.Status?.ToString();

        public string LastUpdated => _valueset.Meta?.LastUpdated?.ToString() ?? string.Empty;

        private string FilePath => Path.Combine(@"..\", Page.Content.Resource.GetPath(), _packageName, Page.Content.Valueset.GetPath(), _valueset.Url.Split('/').Last());

        public XElement SourceResource => 
                new XElement(XmlNs.XHTMLNS + "p",
                    _hasXmlResource 
                        ? GetTarget(".xml", "XML")
                        : null,
                    _hasXmlResource && _hasJsonResource 
                        ? " / " 
                        : null,
                    _hasJsonResource 
                        ? GetTarget(".json", "JSON") 
                        : null);

        private XElement GetTarget(string extension, string lable)
        {
            return 
                new XElement(XmlNs.XHTMLNS + "a",
                        new XAttribute("target", "_blank"),
                        new XAttribute("href", string.Concat(FilePath, extension)), lable);
        }
    }
}