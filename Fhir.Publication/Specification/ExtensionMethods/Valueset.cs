using System.Linq;
using Hl7.Fhir.Model;

namespace Hl7.Fhir.Publication.Specification.ExtensionMethods
{
    internal static class Valueset
    {
        public static bool IsComposition(this ValueSet valueset)
        {
            if (valueset.Compose != null)
            {
                if (valueset.Compose.Import.Any()) return true;

                if (valueset.Compose.Include.Any() && valueset.Compose.Include[0].Filter.Any()) return true;
            }
            return false;

        }

        public static bool IsCompositionMultiCodeSystem(this ValueSet valueset)
        {
            if (valueset.Compose != null)
            {
                if (valueset.Compose.Import.Any()) return false;

                if (valueset.Compose.Include.Any() && !IsComposition(valueset)) return true;
            }
            return false;

        }

        public static bool IsCodesystem(this ValueSet valueset)
        {
            // return valueset.CodeSystem != null && valueset.CodeSystem.Concept.Any() && !HasContainedResource(valueset);
            return valueset.CodeSystem != null && valueset.CodeSystem.Concept.Any();
        }

        private static bool HasContainedResource(DomainResource valueset)
        {
            return valueset.Contained != null && valueset.Contained.Count > 0;
        }

        public static bool HasContainedResourse(this ValueSet valueset)
        {
            return valueset.Contained != null && valueset.Contained.Count > 0;
        }
    }
}