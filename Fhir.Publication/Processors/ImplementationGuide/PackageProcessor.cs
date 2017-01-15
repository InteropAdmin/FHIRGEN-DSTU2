using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;
using Hl7.Fhir.Publication.ImplementationGuide;
using Base = Hl7.Fhir.Publication.ImplementationGuide.Base;

namespace Hl7.Fhir.Publication.Processors.ImplementationGuide
{
    internal class IgPackageProcessor : IProcessor
    {
        private readonly Base _implementationGuide;
        private Log _log;
        private Document _input;
        private Factory _factory;
        private Persister _persister;
        private PackageBuilder _builder;
        private IDirectoryCreator _directoryCreator;
        
        public IgPackageProcessor(
            Base igFactory)
        {
            if (igFactory == null)
                throw new ArgumentNullException(
                    nameof(igFactory));

            _implementationGuide = igFactory;
        }

        public ISelector Influx { get; set; }

        private string ProfileFolderPath => _input.Context.ToString();

        private string ProfileFolder => ProfileFolderPath
            .Split('\\')
                .Single(
                    name =>
                        name.Contains('.'));

        public void Process(
            Document input,
            Stage output,
            Log log,
            IDirectoryCreator directoryCreator)
        {
            if (input == null)
                throw new ArgumentNullException(
                    nameof(input));

            if (output == null)
                throw new ArgumentNullException(
                    nameof(output));

            if (log == null)
                throw new ArgumentNullException(
                    nameof(log));

            if (directoryCreator == null)
                throw new ArgumentNullException(
                    nameof(directoryCreator));

            _log = log;
            _directoryCreator = directoryCreator;
            _input = input;

            _persister = new Persister(
                     _directoryCreator,
                     ProfileFolder,
                     _input.Context.Root.Source.ToString(),
                     _input.Context.Root.Target.ToString(),
                     _log);

            _log.Debug($"IG Creation - Adding {_input.Name} to IGResource.xml");

            if (!_directoryCreator.FileExists(_input.SourceFullPath))
                throw new InvalidOperationException($" File {_input.SourceFullPath} does not exist!");

            _implementationGuide.Load();

            _factory = new Factory(
                _log,
                _implementationGuide,
                _input,
                _directoryCreator,
                ProfileFolder,
                _input.SourceFullPath);

            _builder = new PackageBuilder(_directoryCreator, _factory);

            AddPackagesToImplementationGuide();

            ResourceAndFactory resource = AddResourceToImplementationGuide();

            PersistResourceExamples(resource.Resource.Meta, resource.Factory);

            _persister.SaveResource(resource.Resource, _input.FileName, resource.Resource.ResourceType);

            _implementationGuide.Save();

            _log.Debug($"IG Creation - {_input.Name} finished");
        }

        private void AddPackagesToImplementationGuide()
        {
            if (_implementationGuide.ImplementationGuide.Package
                .All(
                    package =>
                        package.Name != ProfileFolder))
                        _factory.AddPackage();
        }

        private ResourceAndFactory AddResourceToImplementationGuide()
        {
            ResourceAndFactory resource;

            switch (_factory.Type)
            {
                case ResourceType.StructureDefinition:
                    resource = _builder.AddStructureDefinition();
                    break;
                case ResourceType.OperationDefinition:
                    resource = _builder.AddOperationDefinition();
                    break;
                case ResourceType.ValueSet:
                    resource = _builder.AddValueset();
                    break;
                default:
                    throw new InvalidEnumArgumentException($" {_factory.Type} is not currently supported!");
            }

            return resource;
        }

      private void PersistResourceExamples(Meta meta, ResourceFactory resourceFactory)
        {
            if (meta != null && meta.Tag.Any(
                tag =>
                    tag.System == Urn.Example.GetUrnString()))
                        {
                            IEnumerable<string> exampleNames = _factory.AddResourceAsExample(resourceFactory);

                            foreach (string name in exampleNames)
                            {
                                _persister.SaveInExamplesFolder(name);
                            }
                        }
        }
    }
}