using System.ComponentModel;
using Hl7.Fhir.Publication.Framework.Config;

namespace Hl7.Fhir.Publication.Framework.ExtensionMethods
{
    internal static class ConfigKeyTypeExtensions
    {
        public static string GetConfigKeyTypeString(this KeyType key)
        {
            switch (key)
            {
                case KeyType.None:
                    return string.Empty;
                case KeyType.ExamplesInJson:
                    return "EXAMPLES_JSON";
                case KeyType.ExamplesInXml:
                    return "EXAMPLES_XML";
                case KeyType.DmsVersion:
                    return "DMS_VERSION";
                case KeyType.DmsTitle:
                    return "DMS_TITLE";
                case KeyType.DmsIsOnline:
                    return "DMS_ONLINE";
                case KeyType.AnalyticsCode:
                    return "DMS_ANALYTICS";
                case KeyType.Schemas:
                    return "SCHEMAS";
                case KeyType.ValuesetsInXml:
                    return "VALUESETS_XML";
                case KeyType.ValuesetsInJson:
                    return "VALUESETS_JSON";
                case KeyType.StructuresInXml:
                    return "STRUCTURES_XML";
                case KeyType.StructuresInJson:
                    return "STRUCTURES_JSON";
                case KeyType.OperationsInXml:
                    return "OPERATIONS_XML";
                case KeyType.OperationsInJson:
                    return "OPERATIONS_JSON";
                default:
                    throw new InvalidEnumArgumentException($"{key} is not currently supported!");
            }
        }
    }
}