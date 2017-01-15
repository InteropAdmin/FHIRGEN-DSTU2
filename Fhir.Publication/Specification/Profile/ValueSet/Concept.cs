using System;

namespace Hl7.Fhir.Publication.Specification.Profile.ValueSet
{
    internal class Concept
    {
        public Concept(string system, string code, string display, string definition)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentException($" {system} concept code element is not populated!");

            if (string.IsNullOrEmpty(display))
                throw new ArgumentException($" {system} concept display element is not populated!");

            Code = code;
            Display = display;
            Definition = definition;
        }

        public string Code { get; private set; }

        public string Display { get; private set; }

        public string Definition { get; private set; }
    }
}