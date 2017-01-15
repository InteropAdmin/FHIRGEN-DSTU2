using System;
using System.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;

namespace Hl7.Fhir.Publication.Specification.Profile
{
    internal static class Orderer
    {
        public static int GetOrder(Resource profile)
        {
            int order = 0;
            bool result = false;

            if (MetaDataIsValid(profile.Meta))
            {
                result = int.TryParse(
                profile.Meta.Tag.SingleOrDefault(
                    coding =>
                        coding.System == Urn.PublishOrder.GetUrnString())?.Code,
                out order);
            }

            return result ? order : 0;
        }

        private static bool MetaDataIsValid(Meta meta)
        {
            if (meta != null)
            {
                if (meta.Tag.Count(coding =>
                         coding.System == Urn.PublishOrder.GetUrnString()) > 1)
                {
                    throw new InvalidOperationException(" Only one Publish Order should be specified!");
                }
                return true;
            }
            return false;
        }
    }
}