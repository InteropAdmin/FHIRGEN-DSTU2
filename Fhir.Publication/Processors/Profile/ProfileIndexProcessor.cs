using System.IO;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;
using Hl7.Fhir.Publication.ImplementationGuide;
using Hl7.Fhir.Publication.Razor;
using Hl7.Fhir.Publication.Specification.Page;
using Hl7.Fhir.Publication.Specification.Profile;

namespace Hl7.Fhir.Publication.Processors.Profile
{
    internal class ProfileIndexProcessor : IProcessor
    {
        Model.ImplementationGuide _implementationGuide;
        private Package _package;
        private Base _igFactory;
        private Document _input;
        private Log _log;
        private PageRenderer _renderer;
        private IDirectoryCreator _directoryCreator;

        public ISelector Influx { get; set; }

        public void Process(
            Document input,
            Stage output,
            Log log,
            IDirectoryCreator directoryCreator)
        {
            _log = log;
            _directoryCreator = directoryCreator;
            _input = input;
            _log.Debug(
                $"Profile Table: {_input.Name}.");

            _directoryCreator.CreateDirectory(input.Context.Target.ToString());
            _renderer = new PageRenderer(_log, Influx.Single(_input), _directoryCreator, _input.Context);

            _igFactory = new Base(directoryCreator, _input.Context);
            _igFactory.Load();

            _implementationGuide = _igFactory.ImplementationGuide;

            foreach (Model.ImplementationGuide.PackageComponent package in _implementationGuide.Package)
            {
                _package = new Package(package.Name, _input.Context, log, directoryCreator);
                _package.LoadResources();

                string directory = Path.Combine(input.Context.Target.Directory, package.Name);

                _log.Info($"Create directory {directory}");

                directoryCreator.CreateDirectory(directory);

                CreateProfileIndex(package.Name, output, package);                
            }

            _log.Debug(
                $"Profile Table: Finish - {_input.Name}");
        }

        private void CreateProfileIndex(string name, Stage output, Model.ImplementationGuide.PackageComponent package)
        {
            string profileIntro = string.Empty;
            var knowledgeProvider = new KnowledgeProvider(_log);
            var factory = new IndexFactory(_package, _directoryCreator, knowledgeProvider);

            if (package.Description != null)
            {
                profileIntro = Preamble.Generate("aaa", package.Description).ToString();
            }

            var text = factory.GetContent(name, package);
            var pageConfig = new Config(Content.OtherText, _implementationGuide.Name, profileIntro, text, string.Empty, string.Empty, string.Empty, _implementationGuide);

            Document document = _renderer.GetProfileIndexPage(pageConfig, name);

            output.Post(document);
        }  
    }
}