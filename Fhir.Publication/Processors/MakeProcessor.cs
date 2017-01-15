using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.Make;

namespace Hl7.Fhir.Publication.Processors
{
    internal class MakeProcessor : IProcessor
    {
        public ISelector Influx { get; set; }

        public void Process(
            Document input,
            Stage output,
            Log log,
            IDirectoryCreator directoryCreator)
        {
            IWork work = Interpreter.InterpretMakeFile(input.Text, input.Context, directoryCreator);
            work.Execute(log, directoryCreator);
        }
    }
}