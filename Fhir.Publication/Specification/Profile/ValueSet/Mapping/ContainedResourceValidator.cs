using System;
using System.Linq;
using Hl7.Fhir.Model;

namespace Hl7.Fhir.Publication.Specification.Profile.ValueSet.Mapping
{
    internal static class ContainedResourceValidator
    {
        public static void IsValid(Model.ValueSet valueset)
        {
            if (valueset.Contained == null || valueset.Contained.Count < 1)
                throw new InvalidOperationException(" Valueset does not have at least one contained resource!");

            if (!valueset.Contained.All(resource => resource is ConceptMap))
                throw new InvalidOperationException(" Valueset contained resource is not a Concept Map!");

            if (valueset.Contained.Select(resource => (ConceptMap)resource).ToList().Count > 1)
                throw new InvalidOperationException(" Valueset contained resource has > 1 Concept Map!");
        }
    }
}