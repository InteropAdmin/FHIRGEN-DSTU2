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
                Cell.AddPiece(new TableModel.Piece(url, url, null));
        }

        private static bool HasHl7Prefix(string typeName)
        {
            return typeName.StartsWith(Url.Hl7StructureDefintion.GetUrlString());
        }

        private void SetGlobalResourceReferenceCell(string url)
        {
            string type = url.Substring(Url.Hl7StructureDefintion.GetUrlString().Length);

            var definedType = (FHIRDefinedType)Enum.Parse(typeof(FHIRDefinedType), type);

            Cell = definedType == FHIRDefinedType.Resource
                ? new TableModel.Cell(null, null, definedType.ToString(), null, null)
                : new TableModel.Cell(null, KnowledgeProvider.GetLinkForTypeDocument(definedType), definedType.ToString(), null, null);
        }

        private static bool HasNhsFhirPrefix(string url)
        {
            return
                (
                url.StartsWith(Url.FhirPrefix.GetUrlString()) ||
                url.StartsWith(Url.FhirNHSUKPrefix.GetUrlString()) ||
                url.StartsWith(Url.FhirHL7UKPrefix.GetUrlString())
                );
        }

        private void SetLocalResourceReferenceCell(string url)
        {
            string reference = KnowledgeProvider.GetLinkForLocalResource(Package.ResourceStore.GetStructureDefinitionNameByUrl(url, Package.Name));

            string typeName = url.Split('/').Last();

            Cell = new TableModel.Cell(null, reference, typeName, null, null);
        }
    }
}