using System;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.ImplementationGuide;
using Resource = Hl7.Fhir.Publication.ImplementationGuide.Resource;

namespace Hl7.Fhir.Publication.Processors.ImplementationGuide
{
    internal class PackageBuilder
    {
        private readonly IDirectoryCreator _directoryCreator;
        private readonly Factory _factory;
        private ResourceFactory _resourceFactory;

        public PackageBuilder(
            IDirectoryCreator directoryCreator,
            Factory factory)
        {
            if (directoryCreator == null)
                throw new ArgumentNullException(
                    nameof(directoryCreator));

            if (factory == null)
                throw new ArgumentNullException(
                    nameof(factory));

            _directoryCreator = directoryCreator;
            _factory = factory;
        }

        public ResourceAndFactory AddStructureDefinition()
        {
            StructureDefinition structureDefinition = _factory.GetProfileAsStructureDefinition();

            var igResource = new Resource(
                structureDefinition.Name,
                structureDefinition.Description,
                structureDefinition.Url,
                structureDefinition.Meta,
                ResourceType.StructureDefinition);

            _resourceFactory = new ResourceFactory(igResource, _directoryCreator);

            StructureDefinition resource = structureDefinition;

            _factory.AddResourceAsProfile(_resourceFactory);

            return new ResourceAndFactory(resource, _resourceFactory);
        }

        public ResourceAndFactory AddOperationDefinition()
        {
            OperationDefinition operationDefinition = _factory.GetProfileAsOperationDefinition();

            var igResource = new Resource(
                operationDefinition.Name,
                operationDefinition.Description,
                operationDefinition.Url,
                operationDefinition.Meta,
                ResourceType.OperationDefinition);

            _resourceFactory = new ResourceFactory(igResource, _directoryCreator);

            OperationDefinition resource = operationDefinition;

            _factory.AddResourceAsProfile(_resourceFactory);

            return new ResourceAndFactory(resource, _resourceFactory);
        }

        public ResourceAndFactory AddValueset()
        {
            ValueSet valueset = _factory.GetProfileAsValueset();

            var igResource = new Resource(
                valueset.Name,
                valueset.Description,
                valueset.Url,
                valueset.Meta,
                ResourceType.ValueSet);

            _resourceFactory = new ResourceFactory(igResource, _directoryCreator);

            ValueSet resource = valueset;

            _factory.AddResourceAsValueset(_resourceFactory);

            return new ResourceAndFactory(resource, _resourceFactory);
        }
    }
}