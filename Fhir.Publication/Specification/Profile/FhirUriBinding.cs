namespace Hl7.Fhir.Publication.Specification.Profile
{
    internal class FhirUriBinding : Binding
    {
        private readonly Model.FhirUri _fhirUri;

        public FhirUriBinding(Model.FhirUri fhirUri)
        {
            _fhirUri = fhirUri;
        }

        public override string BindingUrl => FormatUrl(_fhirUri.ToString());

        protected override string Reference => _fhirUri.Value;
    }
}