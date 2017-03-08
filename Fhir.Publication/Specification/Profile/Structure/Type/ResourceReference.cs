using System;
using System.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;
using Hl7.Fhir.Publication.ImplementationGuide;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Type
{
    internal class ResourceReference : ReferenceCell
    {
        public ResourceReference(
            Package package,
            string url,
            KnowledgeProvider knowledgeProvider)
            : base(package,
                url,
                knowledgeProvider)
        {
            if (HasHl7Prefix(url))
                SetGlobalResourceReferenceCell(url);
            else if (HasNhsFhirPrefix(url))
                SetLocalResourceReferenceCell(url);
            else
                throw new InvalidOperationException(string.Concat(" Invalid resource reference ", url));
        }

        private static bool HasHl7Prefix(string typeName)
        {
            return typeName.ToUpper().StartsWith(Url.Hl7StructureDefintion.GetUrlString().ToUpper());
        }

        private static bool HasNhsFhirPrefix(string url)
        {
            url = url.ToUpper();

            return
                (
                url.StartsWith(Url.FhirPrefix.GetUrlString().ToUpper()) ||
                url.StartsWith(Url.FhirNHSUKPrefix.GetUrlString().ToUpper()) ||
                url.StartsWith(Url.FhirHL7UKPrefix.GetUrlString().ToUpper())
                );
        }


        private void SetGlobalResourceReferenceCell(string url)
        {
            string type = url.Substring(Url.Hl7StructureDefintion.GetUrlString().Length);

            var definedType = (FHIRDefinedType)Enum.Parse(typeof(FHIRDefinedType), type);

            Cell = definedType == FHIRDefinedType.Resource
                ? new TableModel.Cell(null, null, definedType.ToString(), null, null)
                : new TableModel.Cell(null, KnowledgeProvider.GetLinkForTypeDocument(definedType), definedType.ToString(), null, null);
        }

        private void SetLocalResourceReferenceCell(string url)
        {
            string reference = KnowledgeProvider.GetLinkForLocalResource(Package.ResourceStore.GetStructureDefinitionNameByUrl(url, Package.Name));

            string typeName = url.Split('/').Last();

            Cell = new TableModel.Cell(null, reference, typeName, null, null);
        }
    }
}