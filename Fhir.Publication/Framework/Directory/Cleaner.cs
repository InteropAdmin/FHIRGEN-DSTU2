using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework.Config;
using Hl7.Fhir.Publication.Specification.ExtensionMethods;
using Content = Hl7.Fhir.Publication.Specification.Page.Content;

namespace Hl7.Fhir.Publication.Framework.Directory
{
    internal class Cleaner
    {
        private const string _xml = "xml";
        private const string _json = "json";
        private readonly Log _log;
        private readonly Store _store;
        private readonly IDirectoryCreator _directoryCreator;
        private readonly string _targetDir;
        private readonly string _resourcesDir;
        private Dictionary<string, string> _configValues;

        public Cleaner(
            Log log, 
            IDirectoryCreator directoryCreator, 
            string targetDir)
        {
            if (log == null)
                throw new ArgumentNullException(
                    nameof(log));

            if (directoryCreator == null)
                throw new ArgumentNullException(
                    nameof(directoryCreator));

            _log = log;
            _directoryCreator = directoryCreator;
            _targetDir = targetDir;
            _resourcesDir = Path.Combine(_targetDir, Content.Resource.GetPath());
            _store = new Store(_directoryCreator);
        }

        private bool HasValuesetXml => bool.Parse(Store.GetConfigValue(KeyType.ValuesetsInXml, _configValues));

        private bool HasValuesetJson => bool.Parse(Store.GetConfigValue(KeyType.ValuesetsInJson, _configValues));

        private bool HasStructuresXml => bool.Parse(Store.GetConfigValue(KeyType.StructuresInXml, _configValues));

        private bool HasStructuresJson => bool.Parse(Store.GetConfigValue(KeyType.StructuresInJson, _configValues));

        private bool HasOperationsXml => bool.Parse(Store.GetConfigValue(KeyType.OperationsInXml, _configValues));

        private bool HasOperationsJson => bool.Parse(Store.GetConfigValue(KeyType.OperationsInJson, _configValues));
        
        public void CleanGeneratedFolder(Context context)
        {
            _configValues = _store.GetConfigStore(context);

            if (!HasValuesetXml)
                DeleteFiles(ResourceType.ValueSet, _xml);

            if (!HasValuesetJson)
                DeleteFiles(ResourceType.ValueSet, _json);

            if (!HasStructuresXml)
                DeleteFiles(ResourceType.StructureDefinition, _xml);

            if (!HasStructuresJson)
                DeleteFiles(ResourceType.StructureDefinition, _json);

            if (!HasOperationsXml)
                DeleteFiles(ResourceType.OperationDefinition, _xml);

            if (!HasOperationsJson)
                DeleteFiles(ResourceType.OperationDefinition, _json);

            if (!HasFiles(_resourcesDir))
                DeleteFiles(_resourcesDir);
        }
        
        public void DeleteGeneratedFolder()
        {
            DeleteFiles(_targetDir);
        }

        private void DeleteFiles(string directory)
        {
            _log.Info($"Clear down {directory.TrimEnd('\\').Split('\\').Last()} folder");

            if (_directoryCreator.DirectoryExists(directory))
            {
                foreach (var folder in _directoryCreator.EnumerateFiles(directory, "*", SearchOption.AllDirectories))
                    _directoryCreator.DeleteFile(folder);

                _directoryCreator.DeleteDirectory(directory);
            }
        }

        private void DeleteFiles(ResourceType resourceType, string extension)
        {
            _log.Info($@"Clear down {extension} in resources\{resourceType}s folder");

            string directory = Path.Combine(_resourcesDir, string.Concat(resourceType, "s"));

            if (_directoryCreator.DirectoryExists(directory))
            {
                foreach (var folder in _directoryCreator.EnumerateFiles(directory, string.Concat("*.", extension), SearchOption.AllDirectories))
                    _directoryCreator.DeleteFile(folder);

                if(!HasFiles(directory))
                    _directoryCreator.DeleteDirectory(directory);
            }
        }

        private bool HasFiles(string directory)
        {
            return _directoryCreator.EnumerateFiles(directory, "*.*", SearchOption.AllDirectories).Any();
        }
    }
}