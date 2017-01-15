using System;
using System.Collections.Generic;
using System.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Type
{
    internal class ExtensionCell : Cell
    {
        private readonly TableModel.Cell _cell;

        public ExtensionCell(
           List<ElementDefinition.TypeRefComponent> elementTypes,
           ImplementationGuide.Package package)
        {
            if (elementTypes == null)
                throw new ArgumentNullException(
                    nameof(elementTypes));

            if (package == null)
                throw new ArgumentNullException(
                    nameof(package));

            _cell = new TableModel.Cell();

            if (!elementTypes
                .Any(type =>
                          type.Profile
                               .Any(p =>
                                     p.StartsWith(Url.FhirExtension.GetUrlString())
                                     || p.StartsWith(Url.Hl7StructureDefintion.GetUrlString()))))
            {
                throw new InvalidOperationException(
                    $" No extension types of {Url.FhirExtension.GetUrlString()} or {Url.Hl7StructureDefintion.GetUrlString()}  have been found!");
            }

            string typeName = elementTypes
                .Single(type =>
                          type.Profile
                               .Any(profile =>
                                     profile.StartsWith(Url.FhirExtension.GetUrlString())
                                     || profile.StartsWith(Url.Hl7StructureDefintion.GetUrlString())))
                .ProfileElement
                .Single()
                .ToString();

            var text = KnowledgeProvider.RemovePrefix(typeName);

            text = "Extension";

            var link = typeName.StartsWith(Url.Hl7StructureDefintion.GetUrlString())
                ? typeName
                : KnowledgeProvider.GetLinkForLocalResource(package.ResourceStore.GetStructureDefinitionNameByUrl(typeName, package.Name));

            _cell.AddPiece(new TableModel.Piece(link, text, null));
        }

        public override TableModel.Cell Value => _cell;
    }
}