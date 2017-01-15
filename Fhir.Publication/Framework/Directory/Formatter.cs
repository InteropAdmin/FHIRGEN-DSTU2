using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Hl7.Fhir.Publication.Framework.Config;

namespace Hl7.Fhir.Publication.Framework.Directory
{
    internal class Formatter
    {
        private const string _schemas = "Schemas";
        private const string _dist = "dist";
        private readonly IDirectoryCreator _directoryCreator;
        private readonly Store _configStore;
        private readonly Log _log;
        private readonly Context _context;
        private Dictionary<string, string> _configValues;

        public Formatter(
            IDirectoryCreator directoryCreator, 
            Log log,
            Context context)
        {
            if (directoryCreator == null)
                throw new ArgumentNullException(
                    nameof(directoryCreator));

            if (log == null)
                throw new ArgumentNullException(
                    nameof(log));

            if (context == null)
                throw new ArgumentNullException(
                    nameof(context));

            _directoryCreator = directoryCreator;
            _log = log;
            _configStore = new Store(_directoryCreator);
            _context = context;
        }

        private bool IsSchemaRequired => bool.Parse(Store.GetConfigValue(KeyType.Schemas, _configValues));

        private static string AssemblyDir => GetExecutingAssemblyDirectory();

        public void FormatOutputDirectory()
        {
            _configValues = _configStore.GetConfigStore(_context);
            
            if (IsSchemaRequired)
                GetSchemas();

            GetDistFolder();
        }

        private void GetSchemas()
        {
            _log.Debug("Copying files into Schemas folder ...");

            if (AssemblyDir == null)
                throw new InvalidOperationException(" cannot locate executing assembly directory!");

            var sourceDir = Path.Combine(AssemblyDir, _schemas);
            var targetDir = Path.Combine(_context.Target.Directory, _schemas);

            List<string> filePaths = _directoryCreator.EnumerateFiles(sourceDir, "*.*", SearchOption.AllDirectories).ToList();

            if (filePaths.Any())
                CopyToTargetDir(_directoryCreator, targetDir, filePaths, _schemas);
        }

        private void GetDistFolder()
        {
            _log.Debug("Copying files into dist folder ...");

            if (AssemblyDir == null)
                throw new InvalidOperationException(" cannot locate executing assembly directory!");

            var sourceDir = Path.Combine(AssemblyDir, _dist);
            var targetDir = Path.Combine(_context.Target.Directory, _dist);

            List<string> filePaths = _directoryCreator.EnumerateFiles(sourceDir, "*.*", SearchOption.AllDirectories).ToList();

            if (filePaths.Any())
                CopyToTargetDir(_directoryCreator, targetDir, filePaths, _dist);
        }

        private static string GetExecutingAssemblyDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            string assemblyPath = Uri.UnescapeDataString(uri.Path);

            return Path.GetDirectoryName(assemblyPath);
        }

        private static void CopyToTargetDir(IDirectoryCreator directoryCreator, string targetDir, IEnumerable<string> files, string folder)
        {
            if (!directoryCreator.DirectoryExists(targetDir))
                directoryCreator.CreateDirectory(targetDir);

            foreach (var file in files)
            {
                string fileName = file.Substring(file.LastIndexOf(folder, StringComparison.Ordinal) + folder.Length);

                var targetFilePath = string.Concat(targetDir, fileName);

                if (!directoryCreator.DirectoryExists(Path.GetDirectoryName(targetFilePath)))
                    directoryCreator.CreateDirectory(Path.GetDirectoryName(targetFilePath));

                directoryCreator.Copy(file, targetFilePath, true);
            }
        }
    }
}