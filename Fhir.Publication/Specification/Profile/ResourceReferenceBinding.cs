namespace Hl7.Fhir.Publication.Specification.Profile
{
    internal class ResourceReferenceBinding : Binding
    {
        private readonly Model.ResourceReference _resourceReference;

        public ResourceReferenceBinding(Model.ResourceReference resourceReference)
        {
            _resourceReference = resourceReference;
        }

        public override string BindingUrl => FormatUrl(_resourceReference.Url.ToString());

        protected override string Reference => _resourceReference.Reference;
    }
}