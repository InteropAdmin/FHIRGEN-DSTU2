using System;
using Hl7.Fhir.Model;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure
{
    internal class Cardinality
    {
        public Cardinality(
            ElementDefinition definition,
            ElementDefinition fallback)
        {
            int? min = definition.Min;
            string max = definition.Max;

            if (min == null && fallback != null)
                min = fallback.Min;
            if (max == null && fallback != null)
                max = fallback.Max;

            if (min == null && max == null)
                Value = null;
            else
                Value = min == null ? string.Empty : string.Concat(min, "..", max ?? string.Empty);
        }
        
        public string Value { get; private set; }

        public static bool IsZeroCardinality(string cardinality)
        {
            if (cardinality == "*")
                return false;
            else
            {
                int value;

                if (int.TryParse(cardinality, out value))
                {
                    return value == 0;
                }
            }
            throw new InvalidOperationException("cardinality is not in the correct format!");
        }
    }
}