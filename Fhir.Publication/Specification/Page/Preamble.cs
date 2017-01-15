using System.Xml.Linq;

namespace Hl7.Fhir.Publication.Specification.Page
{
    internal static class Preamble
    {
        public static XElement Generate(string name, string description)
        {
            var descriptionHtml = TransformMarkdown(description);

            XElement elements = XElement.Parse("<div>" + descriptionHtml + "</div>");

            return HeadedPanel.ToHtml(name, elements);
        }

        private static string TransformMarkdown(string description)
        {
            var mark = new MarkdownDeep.Markdown();
            mark.SafeMode = true;
            mark.ExtraMode = true;

            return mark.Transform(description);
        }
    }
}