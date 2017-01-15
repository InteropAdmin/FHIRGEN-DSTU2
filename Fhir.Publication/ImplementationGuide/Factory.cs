using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Serialization;

namespace Hl7.Fhir.Publication.ImplementationGuide
{
    internal class Factory
    {
        private const string _mdExtension = @".md";
        private const string _profile = "Profile";
        private const string _profileIntro = "profileintro";
        private readonly Base _implementationGuide;
        private readonly Log _log;
        private readonly Document _input;
        private readonly IDirectoryCreator _directoryCreator;
        private readonly string _profileFolder;
        private readonly string _fileName;
        private Model.Base _baseResource;
        private string _name;

        public Factory(
            Log log,
            Base implementationGuide,
            Document input,
            IDirectoryCreator directoryCreator,
            string profileFolder,
            string fileName)
        {
            if (log == null)
                throw new ArgumentNullException(
                    nameof(log));

            if (directoryCreator == null)
                throw new ArgumentNullException(
                    nameof(directoryCreator));

            if (input == null)
                throw new ArgumentNullException(
                    nameof(input));

            if (implementationGuide == null)
                throw new ArgumentNullException(
                    nameof(implementationGuide));

            if (string.IsNullOrEmpty(profileFolder))
                throw new ArgumentException(
                    "profileFolder cannot be null or empty!");

            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException(
                    "fileName cannot be null or empty!");

            _log = log;
            _directoryCreator = directoryCreator;
            _profileFolder = profileFolder;
            _implementationGuide = implementationGuide;
            _input = input;
            _fileName = fileName;
            GetResourceFromXml();
        }

        public ResourceType Type => (ResourceType)Enum.Parse(typeof(ResourceType), _baseResource.TypeName);

        private void GetResourceFromXml()
        {
            string resourceXml = _directoryCreator.ReadAllText(_fileName);

            _baseResource = FhirParser.ParseFromXml(resourceXml);
        }

        public void AddPackage()
        {
            _log.Info(
                $"            - Adding {_profileFolder} package to IGResource.xml");

            var package = new Model.ImplementationGuide.PackageComponent();

            package.Name = _profileFolder;

            string mdFileName = package.Name.Replace(_profile, _profileIntro);

            var inputFileName =
                Path.Combine(
                    _input.Context.Root.Source.ToString(),
                    package.Name,
                    string.Concat("description", _mdExtension));

            if (_directoryCreator.FileExists(inputFileName))
                package.Description = _directoryCreator.ReadAllText(inputFileName);

            _implementationGuide.ImplementationGuide.Package.Add(package);
        }

        public StructureDefinition GetProfileAsStructureDefinition()
        {
            var structureDefinition = _baseResource as StructureDefinition;

            if (structureDefinition == null)
                throw new InvalidOperationException($" Could not cast {_fileName} as StructureDefinition!");

            _name = structureDefinition.Name;

            return structureDefinition;
        }

        public OperationDefinition GetProfileAsOperationDefinition()
        {
            var operationDefinition = _baseResource as OperationDefinition;

            if (operationDefinition == null)
                throw new InvalidOperationException($" Could not cast {_fileName} as OperationDefinition!");

            _name = operationDefinition.Name;

            return operationDefinition;
        }

        public ValueSet GetProfileAsValueset()
        {
            var valueset = _baseResource as ValueSet;

            if (valueset == null)
                throw new InvalidOperationException($" Could not case {_fileName} as Valueset!");

            _name = valueset.Url.Split('/').Last();

            return valueset;
        }

        public void AddResourceAsProfile(ResourceFactory resourceFactory)
        {
            _implementationGuide.ImplementationGuide.Package
                .Single(
                     package =>
                         package.Name == _profileFolder)
                .Resource.Add(resourceFactory.CreateProfileResource(
                     Path.Combine(
                         _input.Context.Root.Source.ToString(), 
                         _profileFolder, 
                         string.Concat(
                             Specification.Profile.KnowledgeProvider.TokenizeName(_name).ToLower(), 
                             _mdExtension))));
        }

        public void AddResourceAsValueset(ResourceFactory resourceFactory)
        {
            _implementationGuide.ImplementationGuide.Package
                .Single(
                    package =>
                        package.Name == _profileFolder)
                .Resource.Add(resourceFactory.CreateTerminologyResource(
                    Path.Combine(
                        _input.Context.Root.Source.ToString(),
                         _profileFolder,
                         string.Concat(
                             Specification.Profile.KnowledgeProvider.TokenizeName(_name).ToLower(),
                             _mdExtension))));
        }

        public IEnumerable<string> AddResourceAsExample(ResourceFactory resourceFactory)
        {
            Model.ImplementationGuide.ResourceComponent[] exampleResources = resourceFactory.CreateExampleResources().ToArray();

            foreach (Model.ImplementationGuide.ResourceComponent example in exampleResources)
            {
                _implementationGuide.ImplementationGuide.Package
                          .Single(
                               package =>
                                   package.Name == _profileFolder)
                          .Resource.Add(example);

                yield return example.Name;
            }
        }
    }
}