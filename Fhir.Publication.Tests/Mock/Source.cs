using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Specification.Profile.ValueSet.Mapping;

namespace Fhir.Publication.Tests.Mock
{
    internal class Source : ISourceValueset
    {
        public Source(
            ConceptMap conceptMap,
            ValueSet.CodeSystemComponent codesystem,
            string reference,
            string name)
        {
            ConceptMap = conceptMap;
            CodeSystem = codesystem;
            ResourceReference = reference;
            TargetName = name;
        }

        public ConceptMap ConceptMap { get; }
        public ValueSet.CodeSystemComponent CodeSystem { get; }
        public string ResourceReference { get; }
        public string TargetName { get; }
    }
}