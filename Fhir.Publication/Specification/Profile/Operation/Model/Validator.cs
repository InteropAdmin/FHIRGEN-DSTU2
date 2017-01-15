using System;
using Hl7.Fhir.Model;

namespace Hl7.Fhir.Publication.Specification.Profile.Operation.Model
{
    internal static class Validator
    {
        public static void Validate(OperationDefinition operationDefinition)
        {
            if (string.IsNullOrEmpty(operationDefinition.Name))
                throw new ArgumentException("operation defintion name cannot be null or empty!");

            if (operationDefinition.Kind == null)
                throw new ArgumentNullException(nameof(operationDefinition.Kind));

            if (string.IsNullOrEmpty(operationDefinition.Description))
                throw new ArgumentException("operation definition description cannot be null or empty!");

            if (string.IsNullOrEmpty(operationDefinition.Code))
                throw new ArgumentException("operation definition code cannot be null or empty!");

            if (operationDefinition.System == null)
                throw new ArgumentNullException(nameof(operationDefinition.System));

            if (operationDefinition.Instance == null)
                throw new ArgumentNullException(nameof(operationDefinition.Instance));
        }
    }
}
