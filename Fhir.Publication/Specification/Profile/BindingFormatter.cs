using System;

namespace Hl7.Fhir.Publication.Specification.Profile
{
    internal abstract class BindingFormatter
    {
        protected readonly Binding Binding;

        protected BindingFormatter(object valueSet)
        {
            if (valueSet == null)
                throw new ArgumentNullException(
                    nameof(valueSet), "  ValueSet has not been bound! ");

            var resourceRefence = valueSet as Model.ResourceReference;

            if (resourceRefence != null)
            {
                Binding = new ResourceReferenceBinding(resourceRefence);
                return;
            }

            var fhirUri = valueSet as Model.FhirUri;

            if (fhirUri != null)
            {
                Binding = new FhirUriBinding(fhirUri);
                return;
            }

            //TODO - Tidy this up
            Binding = new FhirUriBinding(new Model.FhirUri("http://example.com/valueset"));
        }
    }
}