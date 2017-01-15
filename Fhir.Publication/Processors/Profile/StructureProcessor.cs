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
using Extensions = Hl7.Fhir.Publication.Specification.Profile.Structure.Extensions;
using Factory = Hl7.Fhir.Publication.Specification.Profile.Example.Factory;
using Bindings = Hl7.Fhir.Publication.Specification.Profile.Structure.Bindings;
using Content = Hl7.Fhir.Publication.Specification.Page.Content;

namespace Hl7.Fhir.Publication.Processors.Profile
{
    internal class StructureProcessor : IProcessor
    {
        private Log _log;
        private Package _package;
        private Model.ImplementationGuide _implementationGuide;
        private Base _baseImplementationGuide;
        private ResourceGenerator _exampleGenerator;
        private ResourceGenerator _resourceGenerator;
        private KnowledgeProvider _knowledgeProvider;
        private Document _input;
        private Stage _output;
        private PageRenderer _renderer;
        private Specification.Profile.Structure.Factory _generator;
        private Model.StructureDefinition _structureDefinition;
        private string _packageName;
        private IDirectoryCreator _directoryCreator;
        
        public ISelector Influx { get; set; }

        private string ResourceSource => Path.Combine(_input.Context.Target.Directory, Content.Resource.GetPath(),_packageName,  Content.Structure.GetPath());

        public void Process(
            Document input,
            Stage output,
            Log log,
            IDirectoryCreator directoryCreator)
        {
            _log = log;
            _directoryCreator = directoryCreator;

            _baseImplementationGuide = new Base(directoryCreator, input.Context);
            _baseImplementationGuide.Load();
            _implementationGuide = _baseImplementationGuide.ImplementationGuide;
            _knowledgeProvider = new KnowledgeProvider(_log);
            _input = input;
            _output = output;

            _exampleGenerator = new ResourceGenerator(_directoryCreator, _baseImplementationGuide.ExamplesXml, _baseImplementationGuide.ExamplesJson, _log);
            _resourceGenerator = new ResourceGenerator(_directoryCreator, _baseImplementationGuide.StructuresInXml, _baseImplementationGuide.StructuresInJson, _log);

            _renderer = new PageRenderer(_log, Influx.Single(_input), _directoryCreator, _input.Context);

            CreatePagesInPackage();
        }

        private void CreatePagesInPackage()
        {
            foreach (Model.ImplementationGuide.PackageComponent package in _implementationGuide.Package)
            {
                _package = new Package(package.Name, _input.Context, _log, _directoryCreator);
                _package.LoadResources();


                string packageDir = Path.Combine(_input.Context.Target.Directory, package.Name);

                CreateDirectory(packageDir);

                _generator = new Specification.Profile.Structure.Factory(_knowledgeProvider, _package, _log, _directoryCreator);

                foreach (Model.ImplementationGuide.ResourceComponent resource in package.Resource
                    .Where(
                        resource =>
                            resource.Purpose == Model.ImplementationGuide.GuideResourcePurpose.Profile 
                            &&
                            resource.Extension
                                .Exists(
                                    extension =>
                                        extension.Value.ToString() == ResourceType.StructureDefinition.ToString())))
                {
                    _packageName = package.Name;
                    CreateStructureDefinitionPage(resource);
                    CreateExampleFiles(_structureDefinition);
                    CreatResourceFiles();
                }
            }
        }

        private void CreateDirectory(string path)
        {
            if (!_directoryCreator.DirectoryExists(path))
                _directoryCreator.CreateDirectory(path);
        }

        private void CreateStructureDefinitionPage(
            Model.ImplementationGuide.ResourceComponent resource)
        {
            _log.Debug($"Structure Page: Start - {_packageName} : {resource.Name}");

            _structureDefinition = _package.ResourceStore.GetStructureDefinitionByUrl(resource.Source.ToString(), _packageName);
            
            var profileIntro = Preamble.Generate(_structureDefinition.Name, resource.Description).ToString();

            var definition = _generator
                .GenerateStructure(_structureDefinition, false)
                .ToString(SaveOptions.DisableFormatting);

            List<Coding> exampleMetaData =
                _structureDefinition.Meta?.Tag.Where(
                    tag => 
                        tag.System == Urn.Example.GetUrnString())
                    .ToList();
          
            var examples = 
                Factory.ToHtml(
                    _implementationGuide,
                    _packageName, 
                    _baseImplementationGuide, 
                    resource, 
                    exampleMetaData)?.ToString();

            string extensions = 
                new Extensions.Table()
                .ToHtml(_log, _directoryCreator, _structureDefinition, _package.ResourceStore, _packageName)
                ?.ToString();

            string bindings = 
                new Bindings.Table()
                .ToHtml(_structureDefinition, _package.ResourceStore, _log, _packageName)
                ?.ToString();
            
            var pageConfig = 
                new Config(
                    Content.Structure, 
                    _implementationGuide.Name, 
                    profileIntro, 
                    definition, 
                    examples, 
                    extensions, 
                    bindings, 
                    _implementationGuide);

            Document document = _renderer.GetResourcePage(pageConfig, resource.Name, _packageName);

            _output.Post(document);

            _log.Debug($"Structure Page: Finish - {document.Name}");
        }

        private void CreateExampleFiles(Model.Resource structureDefinition)
        {
            _log.Info("Create example files");

            IEnumerable<Coding> items = structureDefinition.Meta?.Tag.Where(
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
            _log.Info("Create resource files (Structure Definitions)");
         
            _resourceGenerator.Generate(_structureDefinition.Name, ResourceSource);
        }
    }
}