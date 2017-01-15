using Hl7.Fhir.Publication.Specification.Page;
using Westwind.RazorHosting;

namespace Hl7.Fhir.Publication.Razor
{
    internal static class Razor
    {
        public static string Render(string templateText, Config page)
        {
            var host = new RazorEngine();
            
            string result = host.RenderTemplate(templateText, page);

            return result;
        }
    }
}