using System.Reflection;

namespace Hl7.Fhir.Publication.Framework
{
    internal static class Versioner
    {
        public static string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}