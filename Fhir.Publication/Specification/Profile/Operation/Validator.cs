using System;
using System.Linq;
using Hl7.Fhir.Model;

namespace Hl7.Fhir.Publication.Specification.Profile.Operation
{
    internal static class Validator
    {
        public static bool IsValid(OperationDefinition operation)
        {
            if (!operation.Parameter.Any())
            {
                throw new InvalidOperationException($" {operation.Name} does not have any parameters!");
            }

           if (operation.Kind == OperationDefinition.OperationKind.Operation
                && operation.Parameter.Any(
                        param => param.Use == null))
            {
                throw new InvalidOperationException($" {operation.Name} has a kind of operation and does not have in/out use set!");
            }

            return true;
        }
    }
}