using System;

namespace Hl7.Fhir.Publication.Framework
{
    public class Root
    {
        public Root(string sourceDir, string targetDir)
        {
            if(string.IsNullOrEmpty(sourceDir))
                throw new ArgumentNullException(
                    nameof(sourceDir));

            if (string.IsNullOrEmpty(targetDir))
                throw new ArgumentNullException(
                    nameof(targetDir));

            Source = new Location(sourceDir);
            Target = new Location(targetDir);
        }

        public Location Source { get; }

        public Location Target { get; }

        public Context GetContext()
        {
            return new Context(this);
        }
    }
}
