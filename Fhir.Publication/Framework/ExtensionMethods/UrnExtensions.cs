using System.ComponentModel;

namespace Hl7.Fhir.Publication.Framework.ExtensionMethods
{
    internal static class UrnExtensions
    {
        public static string GetUrnString(this Urn urn)
        {
            switch (urn)
            {
                case Urn.None:
                    return string.Empty;
                case Urn.IgExamplesXml:
                    return "urn:fhir.nhs.uk:extension/IG-ExamplesXML";
                case Urn.IgExamplesJson:
                    return "urn:fhir.nhs.uk:extension/IG-ExamplesJSON";
                case Urn.IgValuesetsXml:
                    return "urn:fhir.nhs.uk:extension/IG-ValuesetsXML";
                case Urn.IgValuesetsJson:
                    return "urn:fhir.nhs.uk:extension/IG-ValuesetsJSON";
                case Urn.OnlineVersion:
                    return "urn:fhir.nhs.uk:extension/IG-OnlineVersion";
                case Urn.PublishOrder:
                    return "urn:hscic:publishOrder";
                case Urn.Analytics:
                    return "urn:fhir.nhs.uk:extension/IG-OnlineAnalytics";
                case Urn.Example:
                    return "urn:hscic:examples";
                case Urn.ResourceType:
                    return "urn:hscic:resourceType";
                case Urn.Schemas:
                    return "urn:fhir.nhs.uk:extension/Schemas";
                case Urn.SoftwareVersion:
                    return "urn:fhir.nhs.uk:extension/SoftwareVersion";
                case Urn.Oid:
                    return "urn:oid:";
                case Urn.IgStructuresInXml:
                    return "urn:fhir.nhs.uk:extension/IG-StructuresXML";
                case Urn.IgStructuresInJson:
                    return "urn:fhir.nhs.uk:extension/IG-StructuresJSON";
                case Urn.IgOperationsInXml:
                    return "urn:fhir.nhs.uk:extension/IG-OperationsXML";
                case Urn.IgOperationsInJson:
                    return "urn:fhir.nhs.uk:extension/IG-OperationsJSON";
                default:
                    throw new InvalidEnumArgumentException($" {urn} is not a supported urn!");
            }
        }
    }
}