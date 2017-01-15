using System;

namespace Hl7.Fhir.Publication.Framework
{
    internal class Log
    {
        private readonly IErrorLogger _errorLogger;

        public Log(IErrorLogger errorLogger)
        {
            if (errorLogger == null)
                throw new ArgumentNullException(
                    nameof(errorLogger));

            _errorLogger = errorLogger;
        }
        
        public void Info(string text)
        {
            _errorLogger.LogInfo(text);
        }

        public void Error(string text)
        {
            string message = string.Concat("Error: ", text);
            _errorLogger.LogError(message);
        }

        public void Error(Exception e, string text)
        {
            string message = string.Concat("Error: ", text);

            _errorLogger.LogError(e, message);
        }

        public void Debug(string text)
        {
            _errorLogger.LogDebug(text);
        }

        public void Warning(string text)
        {
            _errorLogger.Warning(text);
        }
    }
}