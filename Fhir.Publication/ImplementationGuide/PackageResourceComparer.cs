using System.Collections.Generic;

namespace Hl7.Fhir.Publication.ImplementationGuide
{
    internal class PackageResourceComparer : EqualityComparer<PackageResource>
    {
        public override bool Equals(
            PackageResource x,
            PackageResource y)
        {
            if (x == null && y == null)
                return true;

            if (x == null)
                return false;

            if (y == null)
                return false;

            return x.Package == y.Package && x.Url == y.Url;
        }

        public override int GetHashCode(
            PackageResource obj)
        {
            return
               obj?.Name.GetHashCode() ^ obj?.Url.GetHashCode() ^ obj?.Package.GetHashCode() ?? 0;
        }
    }
}