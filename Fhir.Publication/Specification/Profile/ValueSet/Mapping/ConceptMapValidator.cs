using System;
using Hl7.Fhir.Model;

namespace Hl7.Fhir.Publication.Specification.Profile.ValueSet.Mapping
{
    internal static class ConceptMapValidator
    {
        public static void IsValid(ConceptMap conceptMap)
        {
            if (conceptMap.Element == null || conceptMap.Element.Count < 1)
                throw new InvalidOperationException($" {conceptMap.Name} has no mapping elements!");

            foreach (ConceptMap.SourceElementComponent element in conceptMap.Element)
            {
                if (element.Target == null || element.Target.Count < 1)
                    throw new InvalidOperationException($" {conceptMap.Name} has element with no targets!");

                foreach (ConceptMap.TargetElementComponent target in element.Target)
                {
                    if (target.Code == null)
                        throw new InvalidOperationException($" {conceptMap.Name} element has no Code element!");

                    if (target.Equivalence == null)
                        throw new InvalidOperationException($" {conceptMap.Name} element has no equivalence set!");
                }              
            }
        }
    }
}