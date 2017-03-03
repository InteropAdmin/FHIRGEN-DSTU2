namespace Hl7.Fhir.Publication.Framework
{
    internal enum Url
    {
        None,
        Hl7StructureDefintion,
        V2SystemPrefix,
        V3SystemPrefix,
        FhirSystemPrefix,
        Dstu,
        
        FhirStructureDefintion,

        FhirPrefix,
        FhirNHSUKPrefix,
        FhirHL7UKPrefix,

        FhirExtension,
        FhirNHSUKExtension,
        FhirHL7UKExtension,

        FhirValueSet,
        FhirNHSUKValueSet,
        FhirHL7UKValueSet,
        FhirHL7UKStructureDefintion,
        FhirNHSUKStructureDefintion
    }
}