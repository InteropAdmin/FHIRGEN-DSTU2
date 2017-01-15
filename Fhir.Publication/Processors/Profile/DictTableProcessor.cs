using System;
using System.IO;
using System.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.ImplementationGuide;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;
using Hl7.Fhir.Publication.Razor;
using Hl7.Fhir.Publication.Specification.Page;
using Hl7.Fhir.Publication.Specification.Profile;
using Hl7.Fhir.Publication.Specification.Profile.Structure.Dictionary;
using Base = Hl7.Fhir.Publication.ImplementationGuide.Base;

namespace Hl7.Fhir.Publication.Processors.Profile
{
    internal class DictTableProcessor : IProcessor
    {
        private Model.ImplementationGuide _implementationGuide;
        private Document _input;
        private Stage _output;
        private Log _log;
        private KnowledgeProvider _knowledgeProvider;
        private PageRenderer _renderer;
        private IDirectoryCreator _directoryCreator;

        public ISelector Influx { get; set; }

        public void Process(
            Document input,
            Stage output,
            Log log,
            IDirectoryCreator directoryCreator)
        {
            if (log == null)
                throw new ArgumentNullException(
                    nameof(log));

            if (input == null)
                throw new ArgumentNullException(
                    nameof(input));

            if (output == null)
                throw new ArgumentNullException(
                    nameof(output));

            if (directoryCreator == null)
                throw new ArgumentNullException(
                    nameof(directoryCreator));

            _directoryCreator = directoryCreator;
            _input = input;
            _output = output;
            _log = log;
            _knowledgeProvider = new KnowledgeProvider(_log);
            _renderer = new PageRenderer(_log, Influx.Single(_input), _directoryCreator, _input.Context);

            CreateDictionaries();
        }

        private void CreateDictionaries()
        {
            _log.Debug($"Dictionary Page: {_input.Name}.");

            var baseResource = new Base(_directoryCreator, _input.Context);
            baseResource.Load();
            _implementationGuide = baseResource.ImplementationGuide;

            foreach (Model.ImplementationGuide.PackageComponent packageComponent in _implementationGuide.Package)
            {
                var package = new Package(packageComponent.Name, _input.Context, _log, _directoryCreator);
                package.LoadResources();

                string directory = Path.Combine(_input.Context.Target.Directory, packageComponent.Name);

                _log.Info($" Create directory: {directory}");

                _directoryCreator.CreateDirectory(directory);

                RenderPage(package, packageComponent);
            }
        }

        private void RenderPage(Package package, Model.ImplementationGuide.PackageComponent packageComponent)
        {
            var factory = new Specification.Profile.Structure.Dictionary.Factory(_directoryCreator, _input, package.ResourceStore);
      
            var generator = new HtmlGenerator(_knowledgeProvider);

            foreach (Model.ImplementationGuide.ResourceComponent resource in packageComponent.Resource
                       .Where(
                           resource =>
                               resource.Purpose == Model.ImplementationGuide.GuideResourcePurpose.Profile))
            {
                if (resource.Extension
                       .Exists(
                           extension =>
                               extension.Value.ToString() == ResourceType.StructureDefinition.ToString()))
                {
                    var packageName = packageComponent.Name.Split('.').Last();

                    var dictionary = factory.GenerateHtml(generator, package.Name, resource).ToString(); 
                    
                    var pageConfig = new Config(Content.Dictionary, packageName, string.Empty, dictionary, string.Empty, string.Empty, string.Empty, _implementationGuide);

                    Document document = _renderer.GetDictionaryPage(pageConfig, resource.Name, packageComponent.Name);

                    _output.Post(document);
                }
            }
        }
    }
}