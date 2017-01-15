using Hl7.Fhir.Publication.Framework;

namespace Hl7.Fhir.Publication.Processors
{
    internal class CopyProcessor : IProcessor
    {
        public ISelector Influx { get; set; }

        public void Process(
            Document input,
            Stage output,
            Log log,
            IDirectoryCreator directoryCreator)
        {       
            directoryCreator.CreateDirectory(input.Context.Target.ToString());
      
            directoryCreator.Copy(input.SourceFullPath, input.TargetFullPath, overwrite: true);
        }
    }
}