using System;
using System.IO;
using System.Linq;
using Hl7.Fhir.Publication.Framework;

namespace Hl7.Fhir
{
    public static class Program
    {
        static void Main(string[] args)
        {
            string input = args[0];

            string filter = Path.GetFileName(input);
            string sourcedir = Path.GetDirectoryName(input);

            if (!Path.IsPathRooted(sourcedir))
                sourcedir = Directory.GetCurrentDirectory();

            var directoryCreator = new DirectoryCreator();

            var log = new ErrorLogger();

            var logCreator = new LogCreator();
            string currentLog = logCreator.SetupLog(directoryCreator, sourcedir, log, DateTime.Now);

            log.SetLogPath(currentLog);

            if (!args.Any()) 
            {
                log.LogError("You must provide a valid make file.");
                return;
            }
       
            if (string.IsNullOrEmpty(sourcedir))
            {
                log.LogError(string.Concat(sourcedir, " not found"));
                return;
            }

            var dir = Directory.GetParent(sourcedir).FullName;

            string targetdir = Path.Combine(dir, "Generated");

            var publisher = new Publisher(
                    log, 
                    sourcedir, 
                    targetdir, 
                    filter,
                    directoryCreator);

            publisher.Publish();
                 
            Console.ReadLine();
        }
    }
}