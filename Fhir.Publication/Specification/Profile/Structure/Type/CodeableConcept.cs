using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.ImplementationGuide;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Type
{
    internal class CodeableConcept : ReferenceCell
    {
        public CodeableConcept(Package package,
            string url,
            KnowledgeProvider knowledgeProvider,
            FHIRDefinedType type)
            : base(package,
                url,
                knowledgeProvider)
        {
            Cell =  new TableModel.Cell(null, KnowledgeProvider.GetLinkForTypeDocument(type), type.ToString(), null, null);
        }
    }
}