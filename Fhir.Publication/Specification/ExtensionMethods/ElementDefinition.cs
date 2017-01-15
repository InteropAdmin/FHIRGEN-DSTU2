using System.ComponentModel;
using System.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Specification.Profile.Structure.Type;

namespace Hl7.Fhir.Publication.Specification.ExtensionMethods
{
    internal static class ElementDefinition
    {
        public static string AsFormattedString(this Model.ElementDefinition.SlicingRules rule)
        {
            switch (rule)
            {
                case Model.ElementDefinition.SlicingRules.Closed:
                    return "Closed";
                case Model.ElementDefinition.SlicingRules.Open:
                    return "Open";
                case Model.ElementDefinition.SlicingRules.OpenAtEnd:
                    return "Open at End";
                default:
                    throw new InvalidEnumArgumentException(string.Concat(rule, "is not a valid enum."));
            }
        }

        public static bool IsResourceReference(this Model.ElementDefinition elementDefinition)
        {
            //return HasMultipleTypes(elementDefinition) && !HasDistinctTypes(elementDefinition) && IsReference(GetDistinctType(elementDefinition));

            return !HasDistinctTypes(elementDefinition) && IsReference(GetDistinctType(elementDefinition));
        }

        private static bool HasMultipleTypes(Model.ElementDefinition elementDefinition)
        {
            return elementDefinition.Type.Count > 1;
        }

        private static bool HasDistinctTypes(Model.ElementDefinition elementDefinition)
        {
            return
                elementDefinition.Type
                    .Distinct(new TypeRefComponentComparer())
                    .Count() != 1;
        }

        private static Model.ElementDefinition.TypeRefComponent GetDistinctType(Model.ElementDefinition elementDefinition)
        {
            return
                 elementDefinition.Type
                     .Distinct(new TypeRefComponentComparer())
                     .Single();
        }

        private static bool IsReference(Model.ElementDefinition.TypeRefComponent typeRefComponent)
        {
            return typeRefComponent.Code == FHIRDefinedType.Reference;
        }
    }
}