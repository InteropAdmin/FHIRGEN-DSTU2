using System;
using System.IO;
using System.Linq;
using System.Text;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;
using Hl7.Fhir.Publication.ImplementationGuide;
using Hl7.Fhir.Publication.Specification.Page;
using Hl7.Fhir.Publication.Specification.Profile;
using Factory = Hl7.Fhir.Publication.Specification.Profile.ValueSet.Index.Factory;

namespace Hl7.Fhir.Publication.Processors.Index
{
    internal class ValueSetProcessor : IProcessor
    {
        private const string _fileName = "valuesets.html";
        private Package _package;
        private Base _baseResource;
        private Model.ImplementationGuide _implementationGuide;
        private KnowledgeProvider _knowledgeProvider;
        private Log _log;
        private Context _inputContext;
        private IDirectoryCreator _directoryCreator;
        private string _targetDir;

        public ISelector Influx { get; set; }

        private string Content
        {
            get
            {
                var factory = new Factory(_knowledgeProvider, _log, _directoryCreator);

                var builder = new StringBuilder();

                foreach (Model.ImplementationGuide.PackageComponent package in _baseResource.ImplementationGuide.Package)
                {
                    _package = new Package(package.Name, _inputContext, _log, _directoryCreator);
                    _package.LoadResources();
                    _package.SetResources(package.Resource);

                    if (_package.ValueSets.Any())
                    {
                        string targetDir = Path.Combine(_inputContext.Root.Target.Directory, "Resources", _package.Name, "ValueSets");
                        
                        builder.Append(factory.Generate(targetDir, _package, _baseResource));
                    }
                }

                return builder.ToString();
            }
        }

        public void Process(
            Document input,
            Stage output,
            Log log,
            IDirectoryCreator directoryCreator)
        {
            _log = log;
            _log.Debug("ValueSets Index: Start");

            _directoryCreator = directoryCreator;
            _inputContext = input.Context;

            _knowledgeProvider = new KnowledgeProvider(_log);

            _baseResource = new Base(_directoryCreator, _inputContext);
            _baseResource.Load();
            _implementationGuide = _baseResource.ImplementationGuide;

            CreateDirectory();

            string examplesPagePath = Path.Combine(_targetDir, _fileName);

            Document document = Document.CreateInContext(_inputContext, examplesPagePath, _directoryCreator);

            CreatePage(document, input, output);

            _log.Debug("ValueSets Index: Finish");
        }

        private void CreateDirectory()
        {
            string sourceDir = _directoryCreator.EnumerateDirectories(_inputContext.Source.Directory, "Chapter.*.ValueSets").SingleOrDefault();

            if (string.IsNullOrEmpty(sourceDir))
                throw new InvalidOperationException(" ValueSets Chapter folder is not found in directory!");
            else
                _targetDir = Path.Combine(_inputContext.Target.Directory, sourceDir.Split(Path.DirectorySeparatorChar).Last());

            _directoryCreator.CreateDirectory(_targetDir);

            _log.Info($"Target ValueSets directory {_targetDir} created");
        }

        private void CreatePage(
         Document document,
         Document input,
         Stage output)
        {
            var pageConfig = new Config(
                Specification.Page.Content.OtherText,
                _implementationGuide.Name,
                string.Empty,
                Content,
                string.Empty,
                string.Empty,
                string.Empty,
                _implementationGuide);

            Document template = Influx.Single(input);

            var renderedPage = Razor.Razor.Render(template.Text, pageConfig);

            document.Text = renderedPage;

            output.Post(document);

            _log.Info($"{document.FileName} created");
        }
    }
}