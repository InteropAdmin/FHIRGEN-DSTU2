using System;

namespace Hl7.Fhir.Publication.Specification.Profile.ValueSet
{
    internal static class CodeSystemValidator
    {
        public static void IsValid(Model.ValueSet valueset)
        {
            if (valueset.CodeSystem == null)
                throw new InvalidOperationException($" {valueset.Name} has no Codesystem!");

            if (valueset.CodeSystem.Concept == null || valueset.CodeSystem.Concept.Count == 0)
                throw new InvalidOperationException($" {valueset.Name} Codesystem has no concepts!");

            if (string.IsNullOrEmpty(valueset.CodeSystem.System))
                throw new InvalidOperationException($" {valueset.Name} CodeSystem has no System set!");
        }
    }
}