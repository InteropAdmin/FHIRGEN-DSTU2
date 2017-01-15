using System;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Specification.Page;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Dictionary
{
    internal class Factory
    {
        private readonly ImplementationGuide.ResourceStore _resources;

        public Factory(
            IDirectoryCreator directoryCreator,
            Document input,
            ImplementationGuide.ResourceStore resources)
        {
            if (directoryCreator == null)
                throw new ArgumentNullException(
                    nameof(directoryCreator));

            if (input == null)
                throw new ArgumentNullException(
                    nameof(input));

            _resources = resources;
        }

        public XElement GenerateHtml(
            HtmlGenerator generator,
            string packageName,
            Model.ImplementationGuide.ResourceComponent resource)
        {
            StructureDefinition structureDefinition = _resources.GetStructureDefinitionByUrl(resource.Source.ToString(), packageName);

            XElement table = generator
                .Generate(structureDefinition, _resources, packageName);
              
            return HeadedPanel.ToHtml(structureDefinition.Name, table);
        }
    }
}