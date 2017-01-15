using System;
using System.Diagnostics;

namespace Hl7.Fhir.Publication.ImplementationGuide
{
    [DebuggerDisplay("Profile = {Name} {Url}")]
    internal class PackageResource
    {
        public PackageResource(
            string package, 
            string name,
            string url, 
            Model.Resource resource)
        {
            if (string.IsNullOrEmpty(package))
                throw new ArgumentException(
                    nameof(package));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException(
                    nameof(name));

            if (string.IsNullOrEmpty(url))
                throw new ArgumentException(
                    nameof(url));

            if (resource == null)
                throw new ArgumentNullException(
                    nameof(resource));

            Package = package;
            Name = name;
            Url = url;
            Resource = resource;
        }

        public string Package { get; private set; }
        public string Name { get; private set; }
        public string Url { get; private set; }
        public Model.Resource Resource { get; private set; }
    }
}