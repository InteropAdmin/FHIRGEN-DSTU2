using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;

namespace Hl7.Fhir.Publication.Specification.Profile
{
    internal class IndexFactory
    {
        private readonly ImplementationGuide.Package _package;
        private readonly IDirectoryCreator _diretoryCreator;
        private readonly TableFactory _factory;

        public IndexFactory(
            ImplementationGuide.Package package, 
            IDirectoryCreator directoryCreator,
            KnowledgeProvider knowledgeProvider)
        {
            _package = package;
            _diretoryCreator = directoryCreator;

            _factory = new TableFactory(knowledgeProvider);
        }

        public string GetContent(string name, Model.ImplementationGuide.PackageComponent implementationGuideComponent)
        {
            _package.SetResources(implementationGuideComponent.Resource);

            Model.StructureDefinition[] structureDefinitions =
                 _package.StructureDefinitions as Model.StructureDefinition[]
                ?? _package.StructureDefinitions.ToArray();

            foreach (Model.StructureDefinition definition in structureDefinitions.Where(
                definition => !definition.IsSnapshot()))
            {
                throw new InvalidOperationException(
                    string.Concat("StructureDefinition ", definition.Name, "is not a snapshot!"));
            }

            Model.OperationDefinition[] operationDefinitions =
                _package.OperationDefinitions as Model.OperationDefinition[]
                ?? _package.OperationDefinitions.ToArray();

            return GenerateContent(structureDefinitions, operationDefinitions, name, implementationGuideComponent);
        }

        private string GenerateContent(
            Model.StructureDefinition[] structureDefinitions,
            Model.OperationDefinition[] operationDefinitions,
            string name,
            Model.ImplementationGuide.PackageComponent implementationGuideComponent)
        {
            _package.SetResources(implementationGuideComponent.Resource);
            var builder = new StringBuilder();

            if (operationDefinitions.Any())
            {
                builder.Append("<p>The following operation definitions are available in this profile.</p>");
                builder.Append(GenerateTableForOperationDefintions(operationDefinitions, name));
            }
            if (structureDefinitions.Any())
            {
                builder.Append("<p>The following structure definitions are available in this profile.</p>");
                builder.Append(GenerateTableForNonExtensionResources(structureDefinitions, name));
            }

            return builder.ToString();
        }

        private string GenerateTableForNonExtensionResources(
             IEnumerable<Model.StructureDefinition> packageStructures,
             string packageName)
        {

            IOrderedEnumerable<Model.StructureDefinition> profiles =
              packageStructures
                .Where(
                    structureDefinition =>
                        !structureDefinition.IsExtension())
                .OrderBy(
                  Orderer.GetOrder);

            XElement xmldoc = _factory.GenerateProfile(profiles, packageName, Icon.Profile, _diretoryCreator);
            return xmldoc.ToString(SaveOptions.DisableFormatting);
        }

        private string GenerateTableForOperationDefintions(
             IEnumerable<Model.OperationDefinition> packageOperations,
             string packageName)
        {
            IOrderedEnumerable<Model.OperationDefinition> profiles =
              packageOperations.OrderBy(Orderer.GetOrder);

            XElement xmldoc = _factory.GenerateProfile(profiles, packageName, Icon.Profile, _diretoryCreator);
            return xmldoc.ToString(SaveOptions.DisableFormatting);
        }
    }
}