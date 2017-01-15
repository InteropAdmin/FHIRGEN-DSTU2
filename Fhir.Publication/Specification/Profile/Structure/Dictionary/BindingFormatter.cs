using System;
using System.Xml.Linq;
using Hl7.Fhir.Publication.ImplementationGuide;
using Hl7.Fhir.Publication.Specification.ExtensionMethods;
using Hl7.Fhir.Support;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Dictionary
{
    internal class BindingFormatter : Profile.Bindings.Formatter
    {
        private readonly Model.ElementDefinition.BindingComponent _binding;
            
        public BindingFormatter(Model.ElementDefinition.BindingComponent binding, ResourceStore resourceStore, string package)
            : base(binding.ValueSet.ToString(), package, binding.ValueSet)
        {
            if (binding == null)
                throw new ArgumentNullException(
                    nameof(binding));

            _binding = binding;

            ResourceStore = resourceStore;
        }

        private string Description => _binding.Description;

        public string Describe()
        {
            return _binding.TypeName == null ? Description : ToHtml().ToString();
        }

        private XElement ToHtml()
        { 
            return
                new XElement(XmlNs.XHTMLNS + "div",
                    new XAttribute("class", "binding-description"),
                    string.IsNullOrEmpty(Description) ? null : new XElement(XmlNs.XHTMLNS + "p", Description),
                    new XElement(XmlNs.XHTMLNS + "p", _binding.Strength.GetText(),
                        new XElement(XmlNs.XHTMLNS + "a",
                            new XAttribute("href", Reference), Url)),
                    string.IsNullOrEmpty(GetOtherCodesText()) ? null : GetOtherCodesText());
        }

        private string GetOtherCodesText()
        {
            if (_binding.Strength == Model.BindingStrength.Preferred || _binding.Strength == Model.BindingStrength.Example)
                return "; other codes may be used where these codes are not suitable";
            else
                return string.Empty;
        }
    }
}
