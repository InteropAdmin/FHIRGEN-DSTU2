using System.ComponentModel;

namespace Hl7.Fhir.Publication.Framework.ExtensionMethods
{
    internal static class UrlExtensions
    {
        public static string GetUrlString(this Url url)
        {
            switch (url)
            {
                case Url.None:
                    return string.Empty;
                case Url.Hl7StructureDefintion:
                    return "http://hl7.org/fhir/StructureDefinition/"; 
                case Url.V2SystemPrefix:
                    return "http://hl7.org/fhir/v2/"; 
                case Url.V3SystemPrefix:
                    return "http://hl7.org/fhir/v3/";
                case Url.FhirSystemPrefix:
                    return "http://hl7.org/fhir/";
                case Url.Dstu:
                    return "http://www.hl7.org/fhir/DSTU2/";
                case Url.FhirExtension:
                    return "http://fhir.nhs.net/StructureDefinition/extension";
                case Url.FhirStructureDefintion:
                    return "http://fhir.nhs.net/StructureDefinition/";
                case Url.FhirValueSet:
                    return "http://fhir.nhs.net/ValueSet/";
                case Url.FhirPrefix:
                    return "http://fhir.nhs.net/";
                default:
                    throw new InvalidEnumArgumentException($" {url} is not a supported url!");
            }
        }
    }
}