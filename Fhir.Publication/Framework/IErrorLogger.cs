using System;

namespace Hl7.Fhir.Publication.Framework
{
    public interface IErrorLogger
    {
        void LogError(Exception e, string message);
        void LogError(string message);
        void LogDebug(string message);
        void LogInfo(string message);
        void Warning(string message);
    }
}