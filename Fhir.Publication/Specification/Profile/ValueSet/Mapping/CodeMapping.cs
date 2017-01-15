using System;
using Hl7.Fhir.Model;

namespace Hl7.Fhir.Publication.Specification.Profile.ValueSet.Mapping
{
    internal class CodeMapping
    {
        public CodeMapping(
            string code, 
            string display,
            ConceptMap.ConceptMapEquivalence? equivalence,
            string definition,
            Model.ValueSet.ConceptDefinitionComponent codeMapping)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException(
                    nameof(code));

            if (string.IsNullOrEmpty(display))
                throw new ArgumentNullException(
                    nameof(display));

            if (string.IsNullOrEmpty(definition))
                throw new ArgumentNullException(
                    nameof(definition));

            if (equivalence == null)
                throw new ArgumentNullException(
                    nameof(equivalence));

            Code = code;
            Display = display;
            Definition = definition;

            Mapping = codeMapping == null ? string.Empty : codeMapping.Code;

            Equivalence = (ConceptMap.ConceptMapEquivalence)equivalence;
        }

        public string Code { get; private set; }

        public string Display { get; private set; }

        public string Definition { get; private set; }

        public string Mapping { get; private set; }

        public ConceptMap.ConceptMapEquivalence Equivalence { get; private set; }
    }
}