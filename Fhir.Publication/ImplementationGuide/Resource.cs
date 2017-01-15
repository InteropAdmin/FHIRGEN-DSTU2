using System;

namespace Hl7.Fhir.Publication.ImplementationGuide
{
    internal class Resource : IResource
    {
        public Resource(
            string name, 
            string description, 
            string url, 
            Model.Meta meta,
            Model.ResourceType type)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("name cannot be null or empty!");

            if (string.IsNullOrEmpty(description))
                throw new ArgumentException("description cannot be null or empty!");

            if (string.IsNullOrEmpty(url))
                throw new ArgumentException("url cannot be null or empty!");

            Name = name;
            Description = description;
            Url = url;
            Meta = meta;
            Type = type;
        }

        public string Name { get; }
        public string Description { get; }
        public string Url { get; }
        public Model.Meta Meta { get; }
        public Model.ResourceType Type { get; }
    }
}