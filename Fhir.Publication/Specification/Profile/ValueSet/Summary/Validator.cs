using System;
using System.Linq;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;

namespace Hl7.Fhir.Publication.Specification.Profile.ValueSet.Summary
{
    internal static class Validator
    {
        public static bool IsValid(Model.ValueSet valueset, Log log)
        {
            if (valueset.Name == null)
                throw new InvalidOperationException("valueset has no name defined!");

            if (valueset.Url == null)
                throw new InvalidOperationException($" {valueset.Name} has no defining url!");

            if (valueset.Description == null)
                throw new InvalidOperationException($" {valueset.Name} has no Description!");

            if (valueset.Status == null)
                throw new InvalidOperationException($" {valueset.Name} has no Status!");

            if (valueset.Copyright == null)
                log.Warning($"*** {valueset.Name} has no Copyright description!");

            if (!valueset.Extension.Any(extension => extension.Value.ToString().Contains(Urn.Oid.GetUrnString())))
                log.Warning($"*** {valueset.Name} has no OID!");

            if(valueset.Meta == null)
                log.Warning($"*** {valueset.Name} has no meta data!");

            if (string.IsNullOrEmpty(valueset.Meta?.LastUpdated?.ToString()))
                log.Warning($"*** {valueset.Name} has no Last Updated date!");

            return true;
        }
    }
}