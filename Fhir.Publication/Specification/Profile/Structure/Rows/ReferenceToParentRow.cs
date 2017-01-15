using System;
using Hl7.Fhir.Model;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Rows
{
    internal class ReferenceToParentRow : Row
    {
        private const int _parentIndex = 2;
        private readonly string _elementName;

        public ReferenceToParentRow(
            string elementName,
            ImplementationGuide.Package package,
            KnowledgeProvider knowledgeProvider,
            TableModel.Row row,
            string reference,
            ElementDefinition elementDefinition)
            : base(
                  package,
                  knowledgeProvider,
                  row,
                  reference,
                  elementDefinition)
        {
            if (string.IsNullOrEmpty(elementName))
                throw new ArgumentException(
                    nameof(elementName));

            _elementName = elementName;
        }

        public override TableModel.Row Value
        {
            get
            {
                TableRow.GetCells().Add(Name);

                //TODO
                //TableRow.GetCells().Add(new TableModel.Cell());

                TableRow.GetCells().Add(Cardinality);

                TableRow.GetCells().Add(Type);

                TableRow.GetCells().Add(Description);

                if (ElementDefinition.Slicing != null)
                    FormatSliceRow();

                return TableRow;
            }
        }

        private TableModel.Cell Name => new TableModel.Cell(
                    null,
                    Reference,
                    _elementName,
                    !HasShortDescription ? null : ElementDefinition.Short,
                    null);

        private TableModel.Cell Type => GetTypeCell();

        private TableModel.Cell GetTypeCell()
        {
            string[] pathComponents = ElementDefinition.Path.Split('.');
            var parentType = pathComponents[pathComponents.Length - _parentIndex];

            var cell = new TableModel.Cell(null, null, string.Concat("see ", ElementDefinition.NameReferenceElement.Value), parentType, null);

            return cell;
        }
    }
}