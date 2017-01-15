using System;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.ImplementationGuide;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Bindings
{
    internal class Formatter : Profile.Bindings.Formatter
    {
        public Formatter(string elementName, string package, ElementDefinition.BindingComponent binding, ResourceStore resourceStore)
            : base(elementName, package, binding.ValueSet)
        {
            if (resourceStore == null)
                throw new ArgumentNullException(
                    nameof(resourceStore));

            ResourceStore = resourceStore;

            if (binding.Strength == null)
                throw new ArgumentException(" Binding strengh has not been set!");

            Strength = (BindingStrength)binding.Strength;
        }
    }
}