using System;
using Hl7.Fhir.Model;
using System.Collections.Generic;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Rows
{
    internal class ElementRow : Row
    {
        private readonly string _elementName;
        private readonly string _resourceName;
        private readonly FHIRDefinedType? _constrainedType;

        public ElementRow(
            KnowledgeProvider knowledgeProvider,
            ElementDefinition elementDefinition,
            string referenceLink,
            TableModel.Row row,
            ImplementationGuide.Package package,
            string elementName,
            string resourceName,
            FHIRDefinedType? constrainedType)
            : base(
                  package,
                  knowledgeProvider,
                  row,
                  referenceLink,
                  elementDefinition)
        {
            if (string.IsNullOrEmpty(elementName))
                throw new ArgumentNullException(
                    nameof(elementName));

            if (string.IsNullOrEmpty(resourceName))
                throw new ArgumentException(
                    nameof(resourceName));

            _elementName = elementName;
            _resourceName = resourceName;
            _constrainedType = constrainedType;
        }

        public override TableModel.Row Value
        {
            get
            {
                TableRow.GetCells().Add(Name);

                TableRow.GetCells().Add(Flags);

                TableRow.GetCells().Add(Cardinality);

                TableRow.GetCells().Add(Type);

                TableRow.GetCells().Add(Description);

                if (ElementDefinition.Slicing != null)
                    FormatSliceRow();

                return TableRow;
            }
        }

        private TableModel.Cell Name => GetNameCell();

        private TableModel.Cell Flags => ElementFlags.GetFlagsCell(ElementDefinition);

        private new TableModel.Cell Cardinality => new TableModel.Cell(null, null, GetCardinalityText(), null, null);

        private TableModel.Cell Type =>
            ElementDefinition.Definition != null
                ? new Type.Factory(ElementDefinition, _resourceName, Package, KnowledgeProvider).GetCells()
                : new TableModel.Cell();

        private string GetCardinalityText()
        {
            return !HasShortDescription || IsRootElement(_constrainedType, ElementDefinition.Path)
            ? string.Empty
            : new Cardinality(ElementDefinition, null).Value;
        }

        private TableModel.Cell GetNameCell()
        {
            if (ElementDefinition.Name != null & IsSliced())
                return new TableModel.Cell(null, Reference, string.Concat(_elementName, " (", ElementDefinition.Name, ")"), ElementDefinition.Definition, null);
            else
                return new TableModel.Cell(null, Reference, _elementName, !HasShortDescription ? null : ElementDefinition.Short, null);
        }

        private TableModel.Cell GetFlagsCell()
        {
            var _cell = new TableModel.Cell();

            if (ElementDefinition.IsModifier != null && ElementDefinition.IsModifier == true)
            {
                _cell.GetPieces()
                        .Add(new TableModel.Piece("flag-grey", "?!"));  
            }

            if (ElementDefinition.MustSupport != null && ElementDefinition.MustSupport == true)
            {
                _cell.GetPieces()
                        .Add(new TableModel.Piece("flag-red", "S"));
            }

            if (ElementDefinition.Constraint != null && ElementDefinition.Constraint.Count > 0)
            {
                List<string> cons = ElementDefinition.Constraint.ConvertAll(con => con.Key);

                cons.RemoveAll(p => p.StartsWith("ele"));
                cons.RemoveAll(p => p.StartsWith("dom"));

                if (cons.Count > 0)
                {
                    _cell.GetPieces()
                                .Add(new TableModel.Piece("flag-grey", "I"));
                }
            }

            if (ElementDefinition.IsSummary != null && ElementDefinition.IsSummary == true)
            {
                _cell.GetPieces()
                        .Add(new TableModel.Piece("flag-grey", "\u2211"));
            }

            return _cell;
        }

        private static bool IsRootElement(FHIRDefinedType? constrainedType, string path)
        {
            return
                constrainedType.ToString() == path;
        }

        private bool IsSliced()
        {
            return _elementName != ElementDefinition.Name;
        }
    }
}