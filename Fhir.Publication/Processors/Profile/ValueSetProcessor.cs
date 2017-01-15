using System.IO;
using System.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;
using Hl7.Fhir.Publication.ImplementationGuide;
using Hl7.Fhir.Publication.Razor;
using Hl7.Fhir.Publication.Specification.ExtensionMethods;
using Hl7.Fhir.Publication.Specification.Page;
using Hl7.Fhir.Publication.Specification.Profile;
using Base = Hl7.Fhir.Publication.ImplementationGuide.Base;
using Content = Hl7.Fhir.Publication.Specification.Page.Content;

namespace Hl7.Fhir.Publication.Processors.Profile
{
    internal class ValueSetProcessor : IProcessor
    {
        private Log _log;
        private Package _package;
        private Model.ImplementationGuide _implementationGuide;
        private Base _baseImplementationGuide;
        private Document _input;
        private Stage _output;
        private PageRenderer _renderer;
        private Specification.Profile.ValueSet.Factory _factory;
        private ResourceGenerator _resourceGenerator;
        private ValueSet _valueset;
        private IDirectoryCreator _directoryCreator;
        private string _fileName;
        private string _packageName;

        public ISelector Influx { get; set; }

        private string Source => Path.Combine(_input.Context.Target.Directory, Content.Resource.GetPath(), _packageName, Content.Valueset.GetPath());

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

            _input = input;
            _output = output;

            _renderer = new PageRenderer(_log, Influx.Single(_input), _directoryCreator, _input.Context);
            _resourceGenerator = new ResourceGenerator(_directoryCreator, _baseImplementationGuide.ValuesetsInXml, _baseImplementationGuide.ValuesetsInJson, _log);

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
                
                foreach (Model.ImplementationGuide.ResourceComponent resource in package.Resource
                    .Where(
                        resource =>
                            resource.Purpose == Model.ImplementationGuide.GuideResourcePurpose.Terminology
                            &&
                            resource.Extension
                                .Exists(
                                    extension =>
                                        extension.Value.ToString() == ResourceType.ValueSet.ToString())))
                {
                    _packageName = package.Name;

                    _factory = new Specification.Profile.ValueSet.Factory(
                        _log,
                        _package.ResourceStore,
                        _baseImplementationGuide.ValuesetsInXml,
                        _baseImplementationGuide.ValuesetsInJson,
                        _packageName);
          
                    CreatePage(resource);
                    CreateResourceFiles();
                }
            }
        }

        private void CreateDirectory(string path)
        {
            if (!_directoryCreator.DirectoryExists(path))
                _directoryCreator.CreateDirectory(path);
        }

        private void CreatePage(
            Model.ImplementationGuide.ResourceComponent resource)
        {
            _log.Debug($"Valueset Page: Start - {resource.Name}");

            _valueset = _package.ResourceStore.GetValuesetByUrl(resource.Source.ToString(), _packageName);

            var definition = _factory
                .GenerateValueset(_valueset, resource.Name)
                .ToString();

            var pageConfig =
                new Config(
                    Content.Valueset,
                    _implementationGuide.Name,
                    string.Empty,
                    definition,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    _implementationGuide);

            _fileName = resource.Source.ToString().Split('/').Last();

            Document document = _renderer.GetResourcePage(pageConfig, _fileName, _packageName);

            _output.Post(document);

            _log.Debug($"Valueset Page: Finish - {document.Name}");
        }

        private void CreateResourceFiles()
        {
            _log.Info("Create resource files (ValueSets)");
            
            _resourceGenerator.Generate(_fileName, Source);
        }
    }
}