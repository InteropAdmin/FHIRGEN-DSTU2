using System.Collections.Generic;
using Hl7.Fhir.Model;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Bindings
{
    internal class ElementDefinitionComparer : EqualityComparer<ElementDefinition>
    {
        public override bool Equals(ElementDefinition x, ElementDefinition y)
        {
            if (x == null && y == null)
                return true;

            if (x == null)
                return false;

            if (y == null)
                return false;

            var xBinding = new BindingUrl(x);
            var yBinding = new BindingUrl(y);

            return x.Name == y.Name && xBinding.Url == yBinding.Url && x.Binding.Strength == y.Binding.Strength;
        }

        public override int GetHashCode(ElementDefinition obj)
        {
            return 
                obj == null 
                ? 0 
                : obj.Name.GetHashCode() ^ new BindingUrl(obj).Url.GetHashCode() ^ obj.Binding.Strength.GetHashCode();
        }
    }
}