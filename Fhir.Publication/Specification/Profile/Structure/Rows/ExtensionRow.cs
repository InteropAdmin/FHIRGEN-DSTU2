using System;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;
using System.Linq;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Rows
{
    internal class ExtensionRow : Row
    {
        private readonly string _resourceName;
        private const string _lineBreak = "br";

        private ImplementationGuide.Package _package;

        public ExtensionRow(
            ImplementationGuide.Package package,
            KnowledgeProvider knowledgeProvider,
            TableModel.Row row,
            string reference,
            ElementDefinition elementDefinition,
            string resourceName)
            : base(
                  package,
                  knowledgeProvider,
                  row,
                  reference,
                  elementDefinition)
        {
            if (string.IsNullOrEmpty(resourceName))
                throw new ArgumentException(
                    nameof(resourceName));

            TableRow = row;
            _resourceName = resourceName;
            _package = package;
        }

        public override TableModel.Row Value
        {
            get
            {
                TableRow.GetCells().Add(Name);

                //TODO
                //TableRow.GetCells().Add(Cardinality);

                TableRow.GetCells().Add(Cardinality);

                TableRow.GetCells().Add(ExtensionType());

                TableRow.GetCells().Add(ExtensionDescription());

                return TableRow;
            }
        }

        private TableModel.Cell Name => new TableModel.Cell(
                null,
                Reference,
                ElementDefinition.Name,
                !HasShortDescription ? null : ElementDefinition.Short,
                null);

        private TableModel.Cell ExtensionType()
        {
            TableModel.Cell _cell = new TableModel.Cell(
                null,
                   KnowledgeProvider.GetLinkForTypeDocument(FHIRDefinedType.Extension),
                    null
                    ?? "Extension", null,
                    null);

            return _cell;
        }


        private TableModel.Cell ExtensionDescription()
        {
            TableModel.Cell _cell = new TableModel.Cell();
            var typeGenerator = new Type.Factory(ElementDefinition, _resourceName, Package, KnowledgeProvider);

            _cell.GetPieces()
                .Add(new TableModel.Piece(null, ElementDefinition.ShortElement.ToString(), null));

            _cell.AddPiece(new TableModel.Piece(_lineBreak));


            string typeName = "";

            try
            {
                typeName = ElementDefinition.Type
               .Single(type =>
                         type.Profile
                              .Any(profile =>
                                    profile.StartsWith(Url.FhirExtension.GetUrlString())
                                    || profile.StartsWith(Url.FhirNHSUKExtension.GetUrlString())
                                    || profile.StartsWith(Url.FhirHL7UKExtension.GetUrlString())
                                    || profile.StartsWith(Url.Hl7StructureDefintion.GetUrlString())))
               .ProfileElement
               .Single()
               .ToString();

                var link = typeName.StartsWith(Url.Hl7StructureDefintion.GetUrlString())
                    ? typeName
                    : KnowledgeProvider.GetLinkForLocalResource(_package.ResourceStore.GetStructureDefinitionNameByUrl(typeName, _package.Name));

                _cell.GetPieces()
                    .Add(new TableModel.Piece("pub-grey", "Extension"));
                _cell.AddPiece(new TableModel.Piece(null, " ", null));
                _cell.AddPiece(new TableModel.Piece(link, typeName, null));
                _cell.AddPiece(new TableModel.Piece(_lineBreak));
                _cell.AddPiece(new TableModel.Piece(_lineBreak));
            }
            catch (Exception)
            {

            }

            return _cell;
        }


        private void GetTypeCell()
        {
            var typeGenerator = new Type.Factory(ElementDefinition, _resourceName, Package, KnowledgeProvider);

            TableRow.GetCells().Add(typeGenerator.GetCells());

        }

        private void GetEmptyCell()
        {
            TableRow.GetCells().Add(new TableModel.Cell());
        }
    }
}