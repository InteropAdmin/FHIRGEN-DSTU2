using System;
using System.IO;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Specification.ExtensionMethods;
using Hl7.Fhir.Serialization;
using Content = Hl7.Fhir.Publication.Specification.Page.Content;
using System.ComponentModel;
using System.Linq;

namespace Hl7.Fhir.Publication.ImplementationGuide
{
    internal class Persister
    {
        private const string _xmlExtension = @".xml";
        private readonly IDirectoryCreator _directoryCreator;
        private readonly string _profileFolder;
        private readonly string _sourcePath;
        private readonly string _targetPath;
        private readonly Log _log;
        private ResourceType _type;

        public Persister(
            IDirectoryCreator directoryCreator,
            string profileFolder,
            string sourcePath,
            string targetPath,
            Log log)
        {
            if (directoryCreator == null)
                throw new ArgumentNullException(
                    nameof(directoryCreator));

            if (string.IsNullOrEmpty(profileFolder))
                throw new ArgumentException("profileFolde name cannot be null or empty!");

            if (string.IsNullOrEmpty(sourcePath))
                throw new ArgumentException("sourcePath cannot be null or empty!");

            if (string.IsNullOrEmpty(targetPath))
                throw new ArgumentException("targetPath cannot be null or empty!");

            if (log == null)
                throw new ArgumentNullException(
                    nameof(log));

            _directoryCreator = directoryCreator;
            _profileFolder = profileFolder;
            _sourcePath = sourcePath;
            _targetPath = targetPath;
            _log = log;
        }

        private string ProfileDir => Path.Combine(_targetPath, Content.Resource.GetPath(), _profileFolder, string.Concat(_type.ToString(), "s"));

        private string TargetDir => Path.Combine(_targetPath, Content.Example.GetPath(), _profileFolder);

        public void SaveResource(Model.Resource resource, string fileName, ResourceType type)
        {
            string realName;
            string url;

            switch (type)
            {
                case ResourceType.OperationDefinition:
                    url = ((OperationDefinition)resource).Url;
                    break;
                case ResourceType.ValueSet:
                    url = ((ValueSet)resource).Url;
                    break;
                case ResourceType.StructureDefinition:
                    url = ((StructureDefinition)resource).Url;
                    break;
                default:
                    throw new InvalidEnumArgumentException($" {type} is not currently supported!");
  
            }
            _type = type;
            realName = url.Split('/').Last() + ".xml";
     
            _log.Info($"            - Saving {fileName} in {_type} folder");

            if (!_directoryCreator.DirectoryExists(ProfileDir))
                _directoryCreator.CreateDirectory(ProfileDir);

            string targetPath = Path.Combine(ProfileDir, realName);

            _directoryCreator.WriteAllText(targetPath, GetResourceXml(resource));
        }

        private static string GetResourceXml(Model.Resource resource)
        {
            return FhirSerializer.SerializeResourceToXml(resource);
        }

        public void SaveInExamplesFolder(string exampleName)
        {
            string fileName = string.Concat(exampleName, _xmlExtension);

            _log.Info(
                $"            - Saving {fileName} in examples folder");

            string sourceExample = Path.Combine(
                _sourcePath,
                _profileFolder,
                Content.Example.GetPath(),
                fileName);

            string targetExample = Path.Combine(
                _targetPath,
                Content.Example.GetPath(),
                _profileFolder,
                fileName);

            if (!_directoryCreator.DirectoryExists(TargetDir))
                _directoryCreator.CreateDirectory(TargetDir);

            _directoryCreator.Copy(sourceExample, targetExample, true);
        }
    }
}