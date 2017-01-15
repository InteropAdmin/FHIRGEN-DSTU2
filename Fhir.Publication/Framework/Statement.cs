namespace Hl7.Fhir.Publication.Framework
{
    internal class Statement : IWork
    {
        private ISelector _selector;
        private readonly PipeLine _pipeLine;

        public Statement()
        {
            _pipeLine = new PipeLine();
        }

        public Statement(PipeLine pipeline, ISelector selector)
        {
            _pipeLine = pipeline;
            _selector = selector;
        }

        public void SetSelector(ISelector selector)
        {
            _selector = selector;
        }

        public void Add(IProcessor processor)
        {
            _pipeLine.Add(processor);
        }

        public void Execute(Log log, IDirectoryCreator directoryCreator)
        {
            _pipeLine.Process(_selector, log, directoryCreator);
        }

        public override string ToString()
        {
            return $"{_selector}: {_pipeLine}";
        }
    }
}