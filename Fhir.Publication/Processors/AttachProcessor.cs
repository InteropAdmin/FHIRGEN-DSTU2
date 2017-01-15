using Hl7.Fhir.Publication.Framework;

namespace Hl7.Fhir.Publication.Processors
{
    internal class AttachProcessor : IProcessor
    {
        public ISelector Influx { get; set; }

        public void Process(
            Document input,
            Stage output,
            Log log,
            IDirectoryCreator directoryCreator)
        {
            input.Attach(Influx.Documents);
            output.Post(input);
        }
    }
}