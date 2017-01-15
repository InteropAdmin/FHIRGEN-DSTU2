using System;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Description
{
    internal class Definition
    {
        private const string _boldFormat = "font-weight:bold";
        private const string _lineBreak = "br";
        private readonly string _value;
        private readonly string _label;
        private readonly TableModel.Cell _cell;

        public Definition(TableModel.Cell cell, string label, string value)
        {
            if (cell == null)
                throw new ArgumentNullException(
                    nameof(cell));

            if (string.IsNullOrEmpty(value))
                throw new ArgumentException(
                    nameof(value));

            if (string.IsNullOrEmpty(label))
                throw new ArgumentException(
                    nameof(label));

            _cell = cell;
            _value = value;
            _label = label;
        }

        public TableModel.Cell Value
        {
            get
            {
                _cell.AddPiece(new TableModel.Piece(_lineBreak));

                _cell.GetPieces()
                    .Add(new TableModel.Piece("pub-grey", _label));

                _cell.GetPieces()
                 .Add(new TableModel.Piece(null, " ", null));

                _cell.GetPieces().Add(new TableModel.Piece(null, _value, null));

                return _cell;
            }
        }
    }
}
