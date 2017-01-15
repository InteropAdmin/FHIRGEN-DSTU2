using Hl7.Fhir.Model;

namespace Hl7.Fhir.Publication.Specification.Profile.ValueSet.Mapping
{
    internal interface ISourceValueset
    {
        ConceptMap ConceptMap { get; }

        Model.ValueSet.ComposeComponent Compose { get; }

        Model.ValueSet.CodeSystemComponent CodeSystem { get; }

        string ResourceReference { get; }

        string TargetName { get; }
    }
}