using System;
using System.Net;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Dictionary
{
    internal static class MarkDownTransformer
    {
        public static string GenerateTableRow(
            string name, 
            string value, 
            string resourceName,
            KnowledgeProvider knowledgeProvider)
        {
            string text;

            if (value == null)
                text = string.Empty;
            else
            {
                text = value.Replace("||", "\r\n\r\n");
                //while (text.Contains("[[["))
                //{
                    //string left = text.Substring(0, text.IndexOf("[[[", StringComparison.Ordinal));
                    //string linkText = text.Substring(text.IndexOf("[[[", StringComparison.Ordinal) + 3, text.IndexOf("]]]", StringComparison.Ordinal));
                    //string right = text.Substring(text.IndexOf("]]]", StringComparison.Ordinal) + 3);

                    //var url = knowledgeProvider.GetLinkForProfileReference(resourceName, linkText);

                    //text = string.Concat(left, "[", linkText, "](", url, ")", right);
                //}
            }

            var markdown = new MarkdownDeep.Markdown();
            markdown.SafeMode = true;
            markdown.ExtraMode = true;

            var formatted = markdown.Transform(WebUtility.HtmlEncode(text));

            return
                string.Concat(@"  <tr><td width='150px'>", name, "</td><td>", formatted, "</td></tr>\r\n");
        }
    }
}