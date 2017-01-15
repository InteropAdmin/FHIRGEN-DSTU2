using System;
using Hl7.Fhir.Model;

namespace Hl7.Fhir.Publication.Specification.Profile.Example
{
    internal static class Validator
    {
        public static void ValidateProfile(Resource definition)
        {
            foreach (var coding in definition.Meta.Tag)
            {
                if (string.IsNullOrEmpty(coding.Code))
                    throw new ArgumentException("Example name is not populated");

                if (string.IsNullOrEmpty(coding.Display))
                    throw new ArgumentException("Example description is not populated");

             }
        }
    }
}