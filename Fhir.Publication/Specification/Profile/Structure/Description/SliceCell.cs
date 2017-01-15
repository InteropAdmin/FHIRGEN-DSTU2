using System;
using System.Linq;
using System.Text;
using Hl7.Fhir.Publication.Specification.ExtensionMethods;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Description
{
    internal class SliceCell : Cell
    {
        private const string _boldFormat = "font-weight:bold";
        private const string _italicFormat = "font-style: italic";
        private const string _lineBreak = "br";
        private const string _ordered = "Ordered";
        private const string _unOrdered = "Unordered";
        private readonly Model.ElementDefinition.SlicingComponent _slicing;
        private readonly TableModel.Cell _cell;

        public SliceCell(TableModel.Cell cell, Model.ElementDefinition.SlicingComponent slicing)
        {
            if (cell == null)
                throw new ArgumentNullException(
                    nameof(cell));

            if (slicing == null)
                throw new ArgumentNullException(
                    nameof(slicing));

            _cell = cell;
            _slicing = slicing;
        }

        public override TableModel.Cell Value
        {
            get
            {
                if (_cell.GetPieces().Any())
                    _cell.AddPiece(new TableModel.Piece(_lineBreak));

                _cell.GetPieces()
                    .Add(new TableModel.Piece(null, "Slice: ", null)
                    .AddStyle(_boldFormat));

                _cell.GetPieces()
                    .Add(new TableModel.Piece(null, DescribeSlice(), null)
                    .AddStyle(_italicFormat));

                return _cell;
            }
        }

        private string DescribeSlice()
        {
            var builder = new StringBuilder();

            builder.Append( $"Ordering: {GetOrdering()},");
            
            if (_slicing.Discriminator.Any())
            {
                builder.Append(" Discriminator:");

                foreach (var disciminator in _slicing.Discriminator)
                    builder.Append(string.Concat(" ", disciminator, ", "));
            }

            builder.Append($" Rules: {GetRules()}");

            return builder.ToString();
        }

        private string GetOrdering()
        {
            if (_slicing.Ordered == null)
                throw new InvalidOperationException(" Slicing must have Ordering of Ordered or UnOrdered!");

            return _slicing.Ordered == true ? _ordered : _unOrdered;
        }

        private string GetRules()
        {
            string rules;

            switch (_slicing.Rules)
            {
                case Model.ElementDefinition.SlicingRules.Closed:
                    rules = Model.ElementDefinition.SlicingRules.Closed.AsFormattedString();
                    break;
                case Model.ElementDefinition.SlicingRules.Open:
                    rules = Model.ElementDefinition.SlicingRules.Open.AsFormattedString();
                    break;
                case Model.ElementDefinition.SlicingRules.OpenAtEnd:
                    rules = Model.ElementDefinition.SlicingRules.OpenAtEnd.AsFormattedString();
                    break;
                default:
                    throw new InvalidOperationException(" Slicing rules cannot be null!");
            }
            return rules;
        }
    }
}