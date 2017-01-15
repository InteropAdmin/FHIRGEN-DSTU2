namespace Hl7.Fhir.Publication.Framework
{
    internal interface IProcessor
    {
        ISelector Influx { get; set; }
        void Process(Document input, Stage output, Log log, IDirectoryCreator directoryCreator);
    }
}