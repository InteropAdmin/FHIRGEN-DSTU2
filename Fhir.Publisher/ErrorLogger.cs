using System;
using System.IO;
using Hl7.Fhir.Publication.Framework;

namespace Hl7.Fhir
{
   internal class ErrorLogger : IErrorLogger
    {
        private string _currentLog;

        public void SetLogPath(string logFilePath)
        {
            if (string.IsNullOrEmpty(logFilePath))
                throw new ArgumentException(
                    nameof(logFilePath));

            _currentLog = logFilePath;
        }

       public void LogError(
           Exception e,
           string message)
       {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();

            string usefulErrorText = $"**Error** {Environment.NewLine} Message: {e.Message} {Environment.NewLine} " +
                                     $"Stacktrace: {e.StackTrace} {Environment.NewLine} " +
                                     $"InnerException: {e.InnerException}";

            if (!string.IsNullOrEmpty(_currentLog))
                WriteToFile(usefulErrorText);
        }

       public void LogError(
           string message)
       {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();

            if (!string.IsNullOrEmpty(_currentLog))
                WriteToFile(message);
        }

       public void LogDebug(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();

            if (!string.IsNullOrEmpty(_currentLog))
                WriteToFile(message);
        }

        public void LogInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.ResetColor();

            if (!string.IsNullOrEmpty(_currentLog))
                WriteToFile(message);
        }

       public void Warning(
           string message)
       {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(message);
            Console.ResetColor();

            if (!string.IsNullOrEmpty(_currentLog))
                WriteToFile(message);
        }

       private void WriteToFile(string message)
        {
            using (StreamWriter stream = File.AppendText(_currentLog))
            {
                stream.Write(string.Concat(Environment.NewLine, message));
            }
        }
    }
}