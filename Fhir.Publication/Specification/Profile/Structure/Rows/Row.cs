using System;
using Hl7.Fhir.Model;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Rows
{
    internal abstract class Row
    {
        private const string _sliceIcon = "icon_slice.png";
        private const int _cardinalityCellIndex = 1;
        private const int _typeCellIndex = 2;
        protected readonly ImplementationGuide.Package Package;
        protected readonly string Reference;
        protected readonly ElementDefinition ElementDefinition;
        protected readonly bool HasShortDescription;
        protected TableModel.Row TableRow;

        protected Row(
            ImplementationGuide.Package package,
            KnowledgeProvider knowledgeProvider,
            TableModel.Row row,
            string reference,
            ElementDefinition elementDefinition)
        {
            if (package == null)
                throw new NullReferenceException(
                    nameof(package));

            if (knowledgeProvider == null)
                throw new ArgumentNullException(
                    nameof(knowledgeProvider));

            if (row == null)
                throw new ArgumentNullException(
                    nameof(row));

            if (string.IsNullOrEmpty(reference))
                throw new ArgumentException(
                    nameof(reference));

            if (elementDefinition == null)
                throw new ArgumentNullException(
                    nameof(elementDefinition));

            Package = package;
            KnowledgeProvider = knowledgeProvider;
            TableRow = row;
            Reference = reference;
            ElementDefinition = elementDefinition;
            HasShortDescription = elementDefinition.Short != null; 
        }

        protected KnowledgeProvider KnowledgeProvider { get; }

        public abstract TableModel.Row Value { get; }

        protected TableModel.Cell Cardinality => new TableModel.Cell(
                  null,
                  null,
                  new Cardinality(ElementDefinition, null).Value,
                  null,
                  null);

        protected TableModel.Cell Description => new Description.Formatter(
                ElementDefinition,
                null,
                Package.ResourceStore,
                Package.Name)
            .Cell;

        protected void FormatSliceRow()
        {
            TableRow.SetIcon(_sliceIcon);
           // TableRow.GetCells()[_cardinalityCellIndex].GetPieces().Clear();
            TableRow.GetCells()[_typeCellIndex].GetPieces().Clear();

        }
    }
}