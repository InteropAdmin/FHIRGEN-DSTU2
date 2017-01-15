namespace Hl7.Fhir.Publication.Framework
{
    public class GeneratorModel
    {
        public string PublisherVersion;

        public GeneratorModel()
        {
            PublisherVersion = Versioner.GetVersion();
        }
    }
}