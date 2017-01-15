using System;
using System.IO;
using Hl7.Fhir.Publication.Framework.Directory;
using Hl7.Fhir.Publication.Framework.Make;

namespace Hl7.Fhir.Publication.Framework
{
    public class Publisher
    {
        private const string _version = "Furnace: Fhir publisher tool version: ";
        private const string _examples = "examples";
        private const string _constraints = "constraints";
        private readonly Log _log;
        private readonly Cleaner _cleaner;
        private readonly IDirectoryCreator _directoryCreator;
        private readonly string _sourceDir;
        private readonly string _targetDir;
        private readonly string _mask;
        private Formatter _formatter;

        public Publisher(
            IErrorLogger errorLogger,
            string sourceDir,
            string targetDir,
            string mask,
            IDirectoryCreator directoryCreator)
        {
            if (errorLogger == null)
                throw new ArgumentNullException(
                    nameof(errorLogger));

            if (string.IsNullOrEmpty(sourceDir))
                throw new ArgumentNullException(
                    nameof(sourceDir));

            if (string.IsNullOrEmpty(targetDir))
                throw new ArgumentNullException(
                    nameof(targetDir));

            if (string.IsNullOrEmpty(mask))
                throw new ArgumentNullException(
                    nameof(mask));

            if (directoryCreator == null)
                throw new ArgumentNullException(
                    nameof(directoryCreator));

            _log = new Log(errorLogger);
            _sourceDir = sourceDir;
            _targetDir = targetDir;
            _mask = mask;
            _directoryCreator = directoryCreator;

            _cleaner = new Cleaner(_log, _directoryCreator, _targetDir);
        }

        public void Publish()
        {
            try
            {
                _log.Info(string.Concat(_version, Versioner.GetVersion()));
                _log.Info("***Starting generation***");
                _log.Info($"Source directory: {_sourceDir} {Environment.NewLine} Target Directory: {_targetDir}");

                //TODO - Cleardown files
                _cleaner.DeleteGeneratedFolder();

                var root = new Root(_sourceDir, _targetDir);

                Context context = root.GetContext();

                _formatter = new Formatter(_directoryCreator, _log, context);
                _formatter.FormatOutputDirectory();

                var fileFilter = new FileFilter(_directoryCreator);
                Document makeDocument = fileFilter.GetDocument(context, _mask);

                IWork work = Interpreter.InterpretMakeFile(makeDocument.Text, makeDocument.Context, _directoryCreator);

                work.Execute(_log, _directoryCreator);

                //TODO - Cleardown files
                _cleaner.CleanGeneratedFolder(context);

                _log.Info(
                    $"***Rendering complete. Output to directory {_targetDir}***");
            }

            catch (Exception e)
            {
                _log.Error(e, string.Concat(e.GetType(), e.Message));
            }
        }
    }
}