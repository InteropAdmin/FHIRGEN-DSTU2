using System;
using System.Collections.Generic;
using System.Linq;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;

namespace Hl7.Fhir.Publication.Specification.Profile.ValueSet.Mapping
{
    internal class ConceptMapper
    {
        private readonly ImplementationGuide.ResourceStore _resourceStore;
        private readonly Log _log;
        private readonly ISourceValueset _source;
        private readonly string _valuesetName;
        private readonly string _packageName;
        private List<CodeMapping> _mappedResources;

        public ConceptMapper(
            ImplementationGuide.ResourceStore resourceStore,
            ISourceValueset source,
            string valuesetName,
            string packageName,
            Log log)
        {
            if (resourceStore == null)
                throw new ArgumentNullException(
                    nameof(resourceStore));

            if (source == null)
                throw new ArgumentNullException(
                    nameof(source));

            if (string.IsNullOrEmpty(valuesetName))
                throw new ArgumentException(
                    nameof(valuesetName));

            if (log == null)
                throw new ArgumentNullException(
                    nameof(log));

            if (string.IsNullOrEmpty(packageName))
                throw new ArgumentException(
                    nameof(packageName));

            _resourceStore = resourceStore;
            _source = source;
            _valuesetName = valuesetName;
            _packageName = packageName;
            _log = log;
        }

        public IEnumerable<CodeMapping> MapResources()
        {
            _log.Info($" Map resources {_valuesetName} with {_source.TargetName}");

            _mappedResources = new List<CodeMapping>();

            Model.ValueSet.ConceptDefinitionComponent[] targetConcepts = GetConcepts().ToArray();

            if (targetConcepts.Length != 0)
            {
                //_mappedResources = _source.ConceptMap.Element.OuterJoin(targetConcepts,
                //   element => element.Target.Single().Code,
                //   concept => concept.Code,
                //        (element, concept) =>
                //        new CodeMapping(
                //            element.Code,
                //            _source.CodeSystem.Concept.Single(codesystemconcept => codesystemconcept.Code == element.Code).Display,
                //            element.Target.Single().Equivalence,
                //            _source.CodeSystem.Concept.Single(codesystemconcept => codesystemconcept.Code == element.Code).Definition,
                //           concept)
                //        ).ToList();

                _mappedResources = _source.ConceptMap.Element.OuterJoin(targetConcepts,
                       element => element.Target.Single().Code,
                       concept => concept.Code,
                            (element, concept) =>
                            new CodeMapping(
                              element.Code,
                               FindDisplay(element.Code),
                                element.Target.Single().Equivalence,
                               FindDefinition(element.Code),
                               concept)
                            ).ToList();
            }

            return _mappedResources;
        }

        private string FindDisplay(string code)
        {
            // Search primary codesystem
            if (_source.CodeSystem != null)
            {
                foreach (var item in _source.CodeSystem.Concept)
                {
                    if (code == item.Code)
                    {
                        return item.Display;
                    }
                }
            }

            // Search any "composed" codesyatems
            if (_source.Compose != null)
            {
                foreach (var includeItem in _source.Compose.Include)
                {
                    foreach (var item in includeItem.Concept)
                    {
                        if (code == item.Code)
                        {
                            return item.Display;
                        }
                    }
                }       
            }
           
            return code;
        }

        private string FindDefinition(string code)
        {
            return " ";
        }

        private IEnumerable<Model.ValueSet.ConceptDefinitionComponent> GetConcepts()
        {
            Model.ValueSet target = _resourceStore.GetValuesetByUrl(_source.ResourceReference, _packageName);

            return
                TargetValidator.HasValidConcepts(target, _source.TargetName, _source.ResourceReference, _log)
                ? target.CodeSystem.Concept
                : null;
        }
    }
}