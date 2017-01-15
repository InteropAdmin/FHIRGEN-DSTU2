using System.Collections.Generic;
using Hl7.Fhir.Model;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Type
{
    internal class TypeRefComponentComparer : EqualityComparer<ElementDefinition.TypeRefComponent>
    {
        public override bool Equals(
            ElementDefinition.TypeRefComponent x,
            ElementDefinition.TypeRefComponent y)
        {
            if (x == null && y == null)
                return true;

            if (x?.Code == null)
                return false;

            if (y?.Code == null)
                return false;

            return x.Code.Value == y.Code.Value;
        }

        public override int GetHashCode(
            ElementDefinition.TypeRefComponent obj)
        {
            return
              obj?.Code?.GetHashCode() ?? 0;
        }
    }
}