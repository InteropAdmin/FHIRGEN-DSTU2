using System.ComponentModel;

namespace Hl7.Fhir.Publication.Specification.ExtensionMethods
{
    internal static class Equivalence
    {
        public static string GetSymbol(this Model.ConceptMap.ConceptMapEquivalence equivalence)
        {
            switch (equivalence)
            {
                case Model.ConceptMap.ConceptMapEquivalence.Equivalent:
                    return "~";
                case Model.ConceptMap.ConceptMapEquivalence.Narrower:
                    return ">";
                case Model.ConceptMap.ConceptMapEquivalence.Wider:
                    return "<";
                case Model.ConceptMap.ConceptMapEquivalence.Equal:
                    return "=";
                case Model.ConceptMap.ConceptMapEquivalence.Disjoint:
                case Model.ConceptMap.ConceptMapEquivalence.Inexact:
                case Model.ConceptMap.ConceptMapEquivalence.Specializes:
                case Model.ConceptMap.ConceptMapEquivalence.Subsumes:
                case Model.ConceptMap.ConceptMapEquivalence.Unmatched:
                default:
                    throw new InvalidEnumArgumentException($" {equivalence} is not a supported mapping equivalence!");
            }
        }
    }
}