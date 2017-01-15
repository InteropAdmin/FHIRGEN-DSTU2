using System.IO;
using Hl7.Fhir.Publication.Framework;

namespace Hl7.Fhir.Publication.Processors
{
    internal class DeleteProcessor : IProcessor
    {
        public ISelector Influx { get; set; }

        public void Process(
            Document input,
            Stage output,
            Log log,
            IDirectoryCreator directoryCreator)
        {
            log.Info($"Deleting {input.SourceFullPath}");
            directoryCreator.CreateDirectory(input.Context.Target.ToString());
            directoryCreator.DeleteFile(input.SourceFullPath);
        }
    }
}