
namespace Hl7.Fhir.Publication.Framework
{
    internal static class Processor
    {
        private static Stage Process(
            IProcessor processor, 
            Stage input, 
            Log log, 
            IDirectoryCreator directoryCreator)
        {
            Stage output = Stage.New();
            
            foreach (Document document in input.Documents)
            {
                processor.Process(document, output, log, directoryCreator);
            }
            return output;
        }

        private static void Process(
            PipeLine pipeline,
            Stage input,
            Log log,
            IDirectoryCreator fileCreator)
        {
            Stage output = input;

            foreach (IProcessor processor in pipeline.Processors)
            {
                output = Process(processor, output, log, fileCreator);
            }
        }

        public static void Process(this PipeLine pipeline, ISelector filter, Log log, IDirectoryCreator fileCreator)
        {
            var stage = new Stage(filter.Documents);
            Process(pipeline, stage, log, fileCreator);
        }
    }
}