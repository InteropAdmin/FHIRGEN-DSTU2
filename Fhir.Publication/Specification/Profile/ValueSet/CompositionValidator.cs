using System;
using System.Collections.Generic;
using System.Linq;

namespace Hl7.Fhir.Publication.Specification.Profile.ValueSet
{
    internal static class CompositionValidator
    {
        public static void IsValid(Model.ValueSet valueset)
        {
            if (valueset.Compose == null)
                throw new InvalidOperationException($" {valueset.Name} does not have a composition!");
        }

        public static void ValidateFilters(Model.ValueSet.ConceptSetComponent include, string name)
        {
            if (string.IsNullOrEmpty(include.System))
                throw new InvalidOperationException($" {name} includes compose component with no system value!");

            foreach (Model.ValueSet.FilterComponent filter in include.Filter)
            {
                if (string.IsNullOrEmpty(filter.Property))
                    throw new InvalidOperationException($" {name} includes compose filter with no property value!");

                if (filter.Op == null)
                    throw new InvalidOperationException($" {name} contains a filter operator with no value!");

                if (string.IsNullOrEmpty(filter.Value))
                    throw new InvalidOperationException($" {name} includes filter with has no value!");
            }
        }

        public static void ValidateConcepts(Model.ValueSet.ConceptSetComponent include, string name)
        {
            if (string.IsNullOrEmpty(include.System))
                throw new InvalidOperationException($" {name} includes compose component with no system value!");

            foreach (Model.ValueSet.ConceptReferenceComponent concept in include.Concept)
            {
                if (string.IsNullOrEmpty(concept.Code))
                    throw new InvalidOperationException($" {name} includes a compose concept with no code value!");

                if (string.IsNullOrEmpty(concept.Display))
                    throw new InvalidOperationException($" {name} includes a compose concept with no display value!");
            }
        }
    }
}