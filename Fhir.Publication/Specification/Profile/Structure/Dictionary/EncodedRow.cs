using System.Net;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Dictionary
{
    internal class EncodedRow : Row
    {
        public EncodedRow(string name,
            string definitionReference,
            string value)
            : base(name,
                definitionReference,
                WebUtility.HtmlEncode(value))
        {
        }
    }
}
