using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Rows
{
    internal static class ElementFlags
    {
        public static TableModel.Cell GetFlagsCell(ElementDefinition elementDefinition)
        {
            var _cell = new TableModel.Cell();

            if (elementDefinition.IsModifier != null && elementDefinition.IsModifier == true)
            {
                _cell.GetPieces()
                        .Add(new TableModel.Piece("flag-grey", "?!"));
            }

            if (elementDefinition.MustSupport != null && elementDefinition.MustSupport == true)
            {
                _cell.GetPieces()
                        .Add(new TableModel.Piece("flag-red", "S"));
            }

            if (elementDefinition.Constraint != null && elementDefinition.Constraint.Count > 0)
            {
                List<string> cons = elementDefinition.Constraint.ConvertAll(con => con.Key);

                cons.RemoveAll(p => p.StartsWith("ele"));
                cons.RemoveAll(p => p.StartsWith("dom"));

                if (cons.Count > 0)
                {
                    _cell.GetPieces()
                                .Add(new TableModel.Piece("flag-grey", "I"));
                }
            }

            if (elementDefinition.IsSummary != null && elementDefinition.IsSummary == true)
            {
                _cell.GetPieces()
                        .Add(new TableModel.Piece("flag-grey", "\u2211"));
            }

            return _cell;
        }
    }
}
