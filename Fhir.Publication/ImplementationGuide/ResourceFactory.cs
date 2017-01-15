using System;
using System.Collections.Generic;
using System.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;

namespace Hl7.Fhir.Publication.ImplementationGuide
{
    internal class ResourceFactory
    {
        private const int _defaultPublishOrder = 0;
        private readonly IDirectoryCreator _directoryCreator;
        private string _url;
        private readonly IResource _definition;

        public ResourceFactory(IResource resourceDefinition, IDirectoryCreator directoryCreator)
        {
            if (resourceDefinition == null)
                throw new ArgumentNullException(
                    nameof(resourceDefinition));

            if (directoryCreator == null)
                throw new ArgumentNullException(
                    nameof(directoryCreator));

            _definition = resourceDefinition;
            _directoryCreator = directoryCreator;
        }

        public Model.ImplementationGuide.ResourceComponent CreateProfileResource(string mdFileName) 
        {
            var resource = new Model.ImplementationGuide.ResourceComponent();

            resource.Purpose = Model.ImplementationGuide.GuideResourcePurpose.Profile;

            resource.AddExtension(
               Urn.PublishOrder.GetUrnString(),
               new Integer(GetOrder()));

            return CreateResource(resource, mdFileName);
        }

        public Model.ImplementationGuide.ResourceComponent CreateTerminologyResource(string mdFileName)
        {
            var resource = new Model.ImplementationGuide.ResourceComponent();

            resource.Purpose = Model.ImplementationGuide.GuideResourcePurpose.Terminology;

            return CreateResource(resource, mdFileName);
        }

        private Model.ImplementationGuide.ResourceComponent CreateResource(Model.ImplementationGuide.ResourceComponent resource, string mdFilePath)
        {
            resource.Name = _definition.Name;
            resource.Source = new FhirUri(_definition.Url);
            _url = _definition.Url;

            resource.AddExtension(
                Urn.ResourceType.GetUrnString(),
                new FhirString(_definition.Type.ToString()));

            if (_directoryCreator.FileExists(mdFilePath) && !IsValueset(resource))
                resource.Description = _directoryCreator.ReadAllText(mdFilePath);
            else if (!IsValueset(resource))
                throw new InvalidOperationException(
                    $" Could not find .md file for {resource.Name}! {Environment.NewLine} {mdFilePath} is required");

            return resource;
        }

        private static bool IsValueset(Model.ImplementationGuide.ResourceComponent resource)
        {
            return resource.Purpose == Model.ImplementationGuide.GuideResourcePurpose.Terminology;
        }

        private int GetOrder()
        {
            int order = _defaultPublishOrder;

            if (_definition.Meta?.Tag != null)
            {
                foreach (Coding tag in _definition.Meta.Tag.Where(
                    tag =>
                        tag.System == Urn.PublishOrder.GetUrnString()))
                {
                    order = int.Parse(tag.Code);
                }
            }

            return order;
        }

        public IEnumerable<Model.ImplementationGuide.ResourceComponent> CreateExampleResources()
        {
            if (_definition.Meta?.Tag != null)
            {
                foreach (Coding tag in _definition.Meta.Tag)
                {
                    if (tag.System == Urn.Example.GetUrnString())
                    {
                        var example = new Model.ImplementationGuide.ResourceComponent();
                        example.Purpose = Model.ImplementationGuide.GuideResourcePurpose.Example;
                        example.Name = tag.Code;

                        example.ExampleFor = new ResourceReference()
                        {
                            Reference = _url
                        };

                        yield return example;
                    }
                }
            }
        }
    }
}