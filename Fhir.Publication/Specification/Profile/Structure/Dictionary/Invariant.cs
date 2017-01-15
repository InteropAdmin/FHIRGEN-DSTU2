using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Hl7.Fhir.Model;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Dictionary
{
    internal class Invariant
    {
        public static string Describe(IReadOnlyCollection<ElementDefinition.ConstraintComponent> constraints)
        {
            if (constraints == null || !constraints.Any())
                return string.Empty;

            var stringBuilder = new StringBuilder();

            if (constraints.Any())
            {
                stringBuilder.Append("<b>Defined on this element</b><br/>\r\n");

                var isLineBreak = false;

                foreach (var invariant in constraints.OrderBy(constr => constr.Key))
                {
                    if (isLineBreak)
                        stringBuilder.Append("<br/>");
                    else
                        isLineBreak = true;

                    string invariantIdentifier =
                        string.Concat("<b title=\"Formal Invariant Identifier\">Inv-",
                        invariant.Key,
                        "</b>: ",
                        WebUtility.HtmlEncode(invariant.Human),
                        " (xpath: ",
                        WebUtility.HtmlEncode(invariant.Xpath), ")");

                    stringBuilder.Append(invariantIdentifier);
                }
            }

            return stringBuilder.ToString();
        }
    }
}