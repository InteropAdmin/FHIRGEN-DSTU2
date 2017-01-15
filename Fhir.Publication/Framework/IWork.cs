namespace Hl7.Fhir.Publication.Framework
{
    internal interface IWork
    {
        void Execute(Log log, IDirectoryCreator directoryCreator);
    }
}
