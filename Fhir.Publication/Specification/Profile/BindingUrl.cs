using Hl7.Fhir.Model;

namespace Hl7.Fhir.Publication.Specification.Profile
{
    internal class BindingUrl : BindingFormatter
    {
        public BindingUrl(ElementDefinition elementDefinition)
            :base(elementDefinition.Binding.ValueSet)
        {
        }

        public string Url => Binding.BindingUrl;
    }
}