using System;
using Hl7.Fhir.Publication.ImplementationGuide;
using Resource = Hl7.Fhir.Model.Resource;

namespace Hl7.Fhir.Publication.Processors.ImplementationGuide
{
    internal class ResourceAndFactory
    {
        public ResourceAndFactory(Resource resource, ResourceFactory factory)
        {
            if (resource == null)
                throw new ArgumentNullException(
                    nameof(resource));

            if (factory == null)
                throw new ArgumentNullException(
                    nameof(factory));

            Resource = resource;
            Factory = factory;
        }

        public Resource Resource { get; private set; }

        public ResourceFactory Factory { get; private set; }
    }
}