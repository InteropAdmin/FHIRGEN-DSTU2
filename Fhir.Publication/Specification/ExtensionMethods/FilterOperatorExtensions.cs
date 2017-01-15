using System.ComponentModel;
using Hl7.Fhir.Model;

namespace Hl7.Fhir.Publication.Specification.ExtensionMethods
{
    internal static class FilterOperatorExtensions
    {
        public static string GetFilterOperatorString(this ValueSet.FilterOperator? filter)
        {
            switch (filter)
            {
                case ValueSet.FilterOperator.Equal:
                    return " = ";
                case ValueSet.FilterOperator.IsA:
                    return " is a ";
                case ValueSet.FilterOperator.IsNotA:
                    return " is not a ";
                case ValueSet.FilterOperator.Regex:
                    return " regex ";
                case ValueSet.FilterOperator.In:
                    return " in ";
                case ValueSet.FilterOperator.NotIn:
                    return " not in ";
                case null:
                    return string.Empty;
                default:
                    throw new InvalidEnumArgumentException($" {filter} is not a supported filter operator!");
            }
        }
    }
}