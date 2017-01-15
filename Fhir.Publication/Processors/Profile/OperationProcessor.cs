using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;
using Hl7.Fhir.Publication.ImplementationGuide;
using Hl7.Fhir.Publication.Razor;
using Hl7.Fhir.Publication.Specification.ExtensionMethods;
using Hl7.Fhir.Publication.Specification.Page;
using Hl7.Fhir.Publication.Specification.Profile;
using Base = Hl7.Fhir.Publication.ImplementationGuide.Base;
using Factory = Hl7.Fhir.Publication.Specification.Profile.Example.Factory;
using Resource = Hl7.Fhir.Model.Resource;
using Bindings = Hl7.Fhir.Publication.Specification.Profile.Operation.Bindings;
using Content = Hl7.Fhir.Publication.Specification.Page.Content;

namespace Hl7.Fhir.Publication.Processors.Profile
{
    internal class OperationProcessor : IProcessor
    {
        private Document _input;
        private Log _log;
        private Stage _output;
        private Package _package;
        private Model.ImplementationGuide _implementationGuide;
        private Base _baseImplementationGuide;
        private ResourceGenerator _exampleGenerator;
        private ResourceGenerator _resourceGenerator;
        private PageRenderer _renderer;
        private IDirectoryCreator _directoryCreator;
        private OperationDefinition _operationDefinition;
        private string _packageName;
        private ResourceStore _store;

        public ISelector Influx { get; set; }

        private string ResourceSource => Path.Combine(_input.Context.Target.Directory, Content.Resource.GetPath(), _packageName, Content.Operation.GetPath());

        private string SourceDir => Path.Combine(_input.Context.Root.Target.Directory, Content.Resource.GetPath());

        public void Process(
            Document input,
            Stage output,
            Log log,
            IDirectoryCreator directoryCreator)
        {
            _log = log;
            _directoryCreator = directoryCreator;
            _input = input;
            _output = output;

            _baseImplementationGuide = new Base(_directoryCreator, input.Context);
            _baseImplementationGuide.Load();

            var loader = new ResourceLoader(_directoryCreator, _log);

            IEnumerable<string> fileEntries = _directoryCreator.EnumerateFiles(SourceDir, "*.xml", SearchOption.AllDirectories);

            _store = loader.LoadResourceStore(fileEntries);

            _implementationGuide = _baseImplementationGuide.ImplementationGuide;

            _exampleGenerator = new ResourceGenerator(_directoryCreator, _baseImplementationGuide.ExamplesXml, _baseImplementationGuide.ExamplesJson, _log);

            _resourceGenerator = new ResourceGenerator(_directoryCreator, _baseImplementationGuide.OperationsInXml, _baseImplementationGuide.OperationsInJson, _log);

            _renderer = new PageRenderer(_log, Influx.Single(_input), _directoryCreator, _input.Context);

            CreatePagesInPackage();
        }

        private void CreatePagesInPackage()
        {
            foreach (Model.ImplementationGuide.PackageComponent item in _implementationGuide.Package)
            {
                _package = new Package(item.Name, _input.Context, _log, _directoryCreator);

                _package.LoadResources();

                var path = Path.Combine(_input.Context.Target.Directory, item.Name);

                if (!_directoryCreator.DirectoryExists(path))
                    _directoryCreator.CreateDirectory(path);

                var factory = new Specification.Profile.Operation.Factory(_store, _log);

                foreach (Model.ImplementationGuide.ResourceComponent resource in item.Resource
                   .Where(
                       resource =>
                           resource.Purpose == Model.ImplementationGuide.GuideResourcePurpose.Profile
                           &&
                           resource.Extension
                               .Exists(
                                   extension =>
                                       extension.Value.ToString() == ResourceType.OperationDefinition.ToString())))
                {
                    CreateOperationDefinitionPage(resource, factory, path, item.Name);
                    _packageName = path.Split(Path.DirectorySeparatorChar).Last();

                    CreateExampleFiles(_operationDefinition);
                    CreatResourceFiles();
                }
            }
        }

        private void CreateOperationDefinitionPage(
            Model.ImplementationGuide.ResourceComponent resource,
            Specification.Profile.Operation.Factory factory,
            string path,
            string packageName)
        {
            _operationDefinition = _package.ResourceStore.GetOperationDefinitionByUrl(resource.Source.ToString(), packageName);

            _log.Debug($"Operation Definition: Start {_operationDefinition.Name}.");

            var profileIntro = Preamble.Generate(_operationDefinition.Name, resource.Description).ToString();

            XElement definition = factory.GenerateOperation(_operationDefinition);

            IEnumerable<Coding> exampleMetaData =
                _operationDefinition.Meta?.Tag.Where(
                    tag => 
                        tag.System == Urn.Example.GetUrnString());

            var examples =
                Factory.ToHtml(
                    _implementationGuide,
                    path.Split(Path.DirectorySeparatorChar).Last(),
                    _baseImplementationGuide,
                    resource,
                    exampleMetaData)
                    ?.ToString();

            string bindings =
                new Bindings.Table()
                .ToHtml(_operationDefinition, _store, _log)
                ?.ToString();

            var pageConfig =
                new Config(
                    Content.Operation,
                    _implementationGuide.Name,
                    profileIntro,
                    definition.ToString(),
                    examples,
                    string.Empty,
                    bindings,
                    _implementationGuide);

            Document document = _renderer.GetResourcePage(pageConfig, resource.Name, path);

            _output.Post(document);

            _log.Debug($"Operation Definition: Finish - {_operationDefinition.Name}");
        }

        private void CreateExampleFiles(Resource operationDefinition)
        {
            _log.Info("Create example files.");

            IEnumerable<Coding> items =
                operationDefinition.Meta?.Tag.Where(
                    item =>
                        item.System == Urn.Example.GetUrnString())
                .ToList();

            if (items != null)
                foreach (Coding item in items)
                {
                    string source = Path.Combine(_input.Context.Target.Directory, Content.Example.GetPath(), _packageName);
                    _exampleGenerator.Generate(item.Code, source);
                }
        }

        private void CreatResourceFiles()
        {
            _log.Info("Create resource files (Operation Definitions)");
            
            _resourceGenerator.Generate(_operationDefinition.Name, ResourceSource);
        }
    }
}