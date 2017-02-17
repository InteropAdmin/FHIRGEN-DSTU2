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
                case Url.FhirNHSUKExtension:
                    return "https://fhir.nhs.uk/StructureDefinition/extension";
                case Url.FhirHL7UKExtension:
                    return "https://fhir.hl7.org.uk/StructureDefinition/Extension";
                case Url.FhirStructureDefintion:
                    return "http://fhir.nhs.net/StructureDefinition/";
                case Url.FhirValueSet:
                    return "http://fhir.nhs.net/ValueSet/";
                case Url.FhirNHSUKValueSet:
                    return "https://fhir.nhs.uk/ValueSet/";
                case Url.FhirHL7UKValueSet:
                    return "https://fhir.hl7.org.uk/ValueSet/";
                case Url.FhirPrefix:
                    return "http://fhir.nhs.net/";
                case Url.FhirNHSUKPrefix:
                    return "https://fhir.nhs.uk/";
                case Url.FhirHL7UKPrefix:
                    return "https://fhir.hl7.org.uk/";
                default:
                    throw new InvalidEnumArgumentException($" {url} is not a supported url!");
            }
        }
    }
}