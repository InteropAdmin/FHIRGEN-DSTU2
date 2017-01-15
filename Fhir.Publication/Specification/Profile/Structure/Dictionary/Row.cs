using System;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Dictionary
{
    internal class Row
    {
        private readonly string _row = string.Empty;

        public Row(string name, string definitionReference, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (definitionReference != null)
                {
                    definitionReference = KnowledgeProvider.GetSpecLink(definitionReference);
                    _row = string.Concat("  <tr><td><a href=\"", definitionReference, "\">", name, "</a></td><td>", value, "</td></tr>", Environment.NewLine);
                }
                else
                    _row = string.Concat("  <tr><td>", name, "</td><td>", value, "</td></tr>\r\n");

            }
        }

        public string Value => _row;
    }
}