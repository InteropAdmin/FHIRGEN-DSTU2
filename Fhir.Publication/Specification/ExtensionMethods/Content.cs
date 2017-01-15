using System.ComponentModel;

namespace Hl7.Fhir.Publication.Specification.ExtensionMethods
{
    internal static class Content
    {
        public static string GetPath(this Page.Content content)
        {
            switch (content)
            {
                case Page.Content.Structure:
                    return "StructureDefinitions";
                case Page.Content.Dictionary:
                    return string.Empty;
                case Page.Content.Operation:
                    return "OperationDefinitions";
                case Page.Content.OtherText:
                    return "string.Empty";
                case Page.Content.Valueset:
                    return "ValueSets";
                case Page.Content.Example:
                    return "Examples";
                case Page.Content.Resource:
                    return "Resources";
                default:
                    throw new InvalidEnumArgumentException($" {content} is not a supported resource!");
            }
        }
    }
}