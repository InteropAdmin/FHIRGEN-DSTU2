using System;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Type
{
    internal class NoHyperLinkCell : Cell
    {
        private readonly TableModel.Cell _cell;

        public NoHyperLinkCell(string code)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentException(
                    nameof(code));

            _cell = new TableModel.Cell(null, null, code, null, null);
        }

        public override TableModel.Cell Value => _cell;
    }
}