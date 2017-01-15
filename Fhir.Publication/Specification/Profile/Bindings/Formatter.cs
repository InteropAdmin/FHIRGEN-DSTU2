using System;
using System.IO;
using System.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.ImplementationGuide;

namespace Hl7.Fhir.Publication.Specification.Profile.Bindings
{
    internal abstract class Formatter : BindingFormatter
    {
        protected Formatter(string name, string package, object valueSet)
        : base(valueSet)
        {
            if (string.IsNullOrEmpty(package))
                throw new ArgumentException(
                    nameof(package));

            Name = !string.IsNullOrEmpty(name) ? name : Binding.BindingUrl.Split('/').Last();
            _package = package;    
        }

        private readonly string _package;

        public string Reference => Binding.GetLink(ResourceStore, _package);

        public string RelativeReference => Path.Combine(@"..\", _package, Reference);

        public string BindingStrength => KnowledgeProvider.GetBindingStrengthLink(Strength);

        public string Name { get; }

        public string Url => Binding.BindingUrl;

        protected ResourceStore ResourceStore { private get; set; }

        protected BindingStrength Strength { private get; set; }
    }
}