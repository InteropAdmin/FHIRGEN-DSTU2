using System.Xml.Linq;

namespace Hl7.Fhir.Publication.Specification.Profile.ValueSet.Mapping
{
    internal static class Intro
    {
        public static XElement ToHtml(string requirements, string resourceReference)
        {
            var intro = new XElement("br");

            //if (requirements != null)
            //{
            //    intro.Add(new XElement(
            //        "p", $"{requirements}"));
            //}
            intro.Add(new XElement(
                "p", $"This value set has an inline code system {resourceReference}, which defines the following codes:"));
            intro.Add(new XElement("br"));

            return intro;
        }
    }
}