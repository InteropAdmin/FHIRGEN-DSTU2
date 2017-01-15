using System;
using System.Collections.Generic;
using System.Linq;
using Hl7.Fhir.Model;

namespace Hl7.Fhir.Publication.Specification.Profile.ValueSet.Mapping
{
    internal class SourceValueset : ISourceValueset
    {
        public SourceValueset(
            List<Resource> contained,
            Model.ValueSet.CodeSystemComponent codeSystem,
            Model.ValueSet.ComposeComponent compose
            )
        {
            if (contained == null)
                throw new ArgumentNullException(
                    nameof(contained));

            if (codeSystem == null)
                throw new ArgumentNullException(
                    nameof(codeSystem));

            if (!contained.Select(resource => (ConceptMap)resource).Any())
                throw new InvalidOperationException(" Source valueset has no Concept Map!");

            CodeSystem = codeSystem;
            Compose = compose;
            ConceptMap = contained.Select(resource => (ConceptMap)resource).Single();

            ConceptMapValidator.IsValid(ConceptMap);

            ReferencedResource = (ResourceReference)ConceptMap.Target;
            var source = (ResourceReference)ConceptMap.Source;
            Url = source.Url.ToString();
            ResourceReference = ReferencedResource.Url.ToString();
            TargetName = ResourceReference.Split('/').Last();
        }

        public ConceptMap ConceptMap { get; }

        public Model.ValueSet.CodeSystemComponent CodeSystem { get; }

        private ResourceReference ReferencedResource { get; }

        public string ResourceReference { get; }

        public string TargetName { get; }

        public string Url { get; }

        public Model.ValueSet.ComposeComponent Compose { get; }
    }
}