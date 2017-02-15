using System;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;

namespace Hl7.Fhir.Publication.Specification.Profile
{
    internal abstract class Binding
    {
        private const string _indexExtension = @"/index.html";
        private const string _valueSet = @"vs/";

        public abstract string BindingUrl { get; }
        protected abstract string Reference { get; }

        public string GetLink(ImplementationGuide.ResourceStore resourceStore, string packageName)
        {
            if (Reference == null)
                throw new InvalidOperationException( "Binding reference has not been set!");

            string reference = Reference.Trim();

            if (reference.StartsWith(string.Concat(Url.V3SystemPrefix.GetUrlString(), _valueSet)))
                return KnowledgeProvider.GetSpecLink(string.Concat("v3/", reference.Substring(Url.V3SystemPrefix.GetUrlString().Length), _indexExtension));

            if (reference.StartsWith(string.Concat(Url.V2SystemPrefix.GetUrlString(), _valueSet)))
                return KnowledgeProvider.GetSpecLink(string.Concat("v2/", reference.Substring(Url.V2SystemPrefix.GetUrlString().Length), _indexExtension));

            if (reference.StartsWith(Url.FhirStructureDefintion.GetUrlString()))
                return KnowledgeProvider.GetLinkForLocalResource(resourceStore.GetStructureDefinitionNameByUrl(reference, packageName));

            if (reference.StartsWith(Url.FhirNHSUKValueSet.GetUrlString()))
                return KnowledgeProvider.GetLinkForLocalResource(resourceStore.GetValuesetNameByUrl(reference, packageName));

            if (reference.StartsWith(Url.FhirHL7UKValueSet.GetUrlString()))
                return KnowledgeProvider.GetLinkForLocalResource(resourceStore.GetValuesetNameByUrl(reference, packageName));

            return reference.StartsWith(Url.FhirValueSet.GetUrlString())
                ? KnowledgeProvider.GetLinkForLocalResource(resourceStore.GetValuesetNameByUrl(reference, packageName))
                : reference;
        }

        protected static string FormatUrl(string url)
        {
            const string html = ".html";
            
            return url.EndsWith(html) ? url.Remove(url.Length - html.Length) : url;
        }
    }
}