using System;
using System.Linq;
using System.Collections.Generic;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.Make;

namespace Hl7.Fhir.Publication.Processors
{
    internal class MakeForAllProcessor : IProcessor
    {
        private readonly string _pattern;

        public MakeForAllProcessor(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                throw new ArgumentNullException(
                    nameof(pattern));

            _pattern = pattern;
        }

        public ISelector Influx { get; set; }

        public void Process(
            Document input,
            Stage output,
            Log log,
            IDirectoryCreator directoryCreator)
        {
            IEnumerable<string> folders = new Disk(directoryCreator).GetDirectories(input.Context.Source.Directory, _pattern);

            foreach (var folder in folders)
            {
                var folderName = folder.TrimEnd('\\').Split('\\').Last();

                log.Info($"Create folder {folderName}");

                Context context = Context.CreateFromSource(input.Context.Root, folder);
                context.FilterPattern = _pattern.Replace("*", string.Empty);

                IWork work = Interpreter.InterpretMakeFile(input.Text, context, directoryCreator);
                work.Execute(log, directoryCreator);
            }
        }
    }
}
