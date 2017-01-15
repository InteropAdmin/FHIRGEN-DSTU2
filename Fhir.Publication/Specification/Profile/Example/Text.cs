namespace Hl7.Fhir.Publication.Specification.Profile.Example
{
    internal static class Text
    {
        public static string GetText(string resourceName, string exampleName)
        {
            return !string.IsNullOrEmpty(exampleName)
                ? $"<div class='well well-sm'>Follow this link to view examples for {resourceName}: {KnowledgeProvider.ExamplesPageLink(resourceName)}</div>"
                : "<p>Currently there are no examples for this resource</p>";
        }
    }
}