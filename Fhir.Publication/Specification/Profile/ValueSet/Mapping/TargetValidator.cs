using System;
using Hl7.Fhir.Publication.Framework;

namespace Hl7.Fhir.Publication.Specification.Profile.ValueSet.Mapping
{
    internal static class TargetValidator
    {
        public static bool HasValidConcepts(
            Model.ValueSet target, 
            string targetName, 
            string resourceReference, 
            Log log)
        {
            if (target == null)
                throw new InvalidOperationException($" {resourceReference} has not been found!");

            if (target.CodeSystem == null)
                throw new InvalidOperationException($" {targetName} has no CodeSystem!");

            if (target.CodeSystem.Concept == null || target.CodeSystem.Concept.Count < 1)
                throw new InvalidOperationException($" {targetName} has no concepts!");

            foreach (Model.ValueSet.ConceptDefinitionComponent concept in target.CodeSystem.Concept)
            {
                if (string.IsNullOrEmpty(concept.Code))
                    throw new InvalidOperationException($" {targetName} has codesystem  concept with no code!");

                if (string.IsNullOrEmpty(concept.Display))
                    throw new InvalidOperationException($" {targetName} has codesystem  concept with no display!");

                if (string.IsNullOrEmpty(concept.Definition))
                    log.Warning($" ***{targetName} has codesystem  concept {concept.Display} has no definition!");
            }

            return true;
        }
    }
}