using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Serialization;

namespace Hl7.Fhir.Publication.ImplementationGuide
{
    internal class ResourceLoader
    {
        private readonly IDirectoryCreator _directoryCreator;
        private readonly Log _log;
        private readonly ResourceStore _store;

        public ResourceLoader(IDirectoryCreator directoryCreator, Log log)
        {
            if (directoryCreator == null)
                throw new ArgumentNullException(
                    nameof(directoryCreator));

            if (log == null)
                throw new ArgumentNullException(
                    nameof(log));

            _directoryCreator = directoryCreator;
            _log = log;
            _store = new ResourceStore(_log);
        }
        
        public ResourceStore LoadResourceStore(IEnumerable<string> fileEntries)
        {
            _log.Info("Loading resources into resource store");

            foreach (string item in fileEntries)
            {
                if (_directoryCreator.FileExists(item))
                {
                    string resourceXml = _directoryCreator.ReadAllText(item);
                    Model.Resource resource = FhirParser.ParseResourceFromXml(resourceXml);

                    var type = (ResourceType)Enum.Parse(typeof(ResourceType), resource.TypeName);
                    
                    string packageName = item
                        .Split(Path.DirectorySeparatorChar)
                        .Single(r => r.Contains("Profile."));

                    AddToStore(packageName, type, resource);
                }
            }

            return _store;
        }

        private void AddToStore(string packageName, ResourceType type, Model.Resource resource)
        {
            switch (type)
            {
                case ResourceType.StructureDefinition:
                    var structureDefinition = (StructureDefinition)resource;

                    _store.Add(new PackageResource(packageName, structureDefinition.Name, structureDefinition.Url, resource));

                    break;
                case ResourceType.OperationDefinition:
                    var operationDefintion = (OperationDefinition)resource;

                    _store.Add(new PackageResource(packageName, operationDefintion.Name, operationDefintion.Url, resource));

                    break;
                case ResourceType.ValueSet:
                    var valueSet = (ValueSet)resource;

                    _store.Add(new PackageResource(packageName, valueSet.Name, valueSet.Url, resource));

                    break;
                default:
                    throw new InvalidEnumArgumentException($" {type} is not a supported resource type!");
            }
        } 
    }
}