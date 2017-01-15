using System;
using framework = Hl7.Fhir.Publication.Framework;

namespace Fhir.Publication.Tests.Mock
{
    internal class ErrorLogger : framework.IErrorLogger
    {
        public void LogError(
            Exception e,
            string message)
        {
        }

        public void LogError(
            string message)
        {
        }

        public void LogDebug(
            string message)
        {
        }

        public void LogInfo(
            string message)
        {
        }

        public void Warning(
            string message)
        {
        }
    }
}