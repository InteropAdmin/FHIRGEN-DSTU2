using System;
using System.IO;
using Hl7.Fhir.Publication.Framework;

namespace Hl7.Fhir
{
    internal class LogCreator
    {
        private string _currentLog;

        public string SetupLog(
            IDirectoryCreator directoryCreator, 
            string sourceDir, 
            IErrorLogger log,
            DateTime date)
        {
            var logPath = Path.Combine(sourceDir, "Log");

            try
            {
                if (!directoryCreator.DirectoryExists(logPath))
                    directoryCreator.CreateDirectory(logPath);

                string logDate = $"{date.ToLongDateString()} {date.TimeOfDay.Hours}{date.TimeOfDay.Minutes}";

                _currentLog = string.Concat(Path.Combine(logPath, string.Concat("Log ", logDate)), ".txt");

                directoryCreator.WriteAllText(
                    _currentLog,
                    string.Concat(Environment.NewLine, $"***Furnace log date run: {logDate} ***"));
            }

            catch (UnauthorizedAccessException e)
            {
                log.LogError($"{ e.Message} : {_currentLog}");
            }

            return _currentLog;
        }
    }
}