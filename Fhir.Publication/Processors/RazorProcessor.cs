using System;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;
using Hl7.Fhir.Publication.ImplementationGuide;
using Hl7.Fhir.Publication.Specification.Page;
using Hl7.Fhir.Publication.Specification.Profile;

namespace Hl7.Fhir.Publication.Processors
{
    internal class RazorProcessor : IProcessor
    {
        private const string _html = ".html";
        private Model.ImplementationGuide _implementationGuide;

        public ISelector Influx { get; set; }

        public void Process(
            Document input,
            Stage output,
            Log log,
            IDirectoryCreator directoryCreator)
        {
            log.Info($"Razor rendering {input.Name}");

            if (string.IsNullOrEmpty(input.Text))
                throw new InvalidOperationException(
                    $"{input.Name} cannot be empty! .cshtml template needed for Razor engine!");

            if (output == null)
                throw new ArgumentNullException(
                    nameof(output));

            if (directoryCreator == null)
                throw new ArgumentNullException(
                    nameof(directoryCreator));

            var implementationGuide = new Base(directoryCreator, input.Context);
            implementationGuide.Load();
            _implementationGuide = implementationGuide.ImplementationGuide;

            var config = new Config(Content.OtherText, _implementationGuide.Name, string.Empty, input.Text, string.Empty, string.Empty, string.Empty, _implementationGuide);

            Document template = Influx.Single(input);
            template.Extension = _html;
            
            var renderedPage = Razor.Razor.Render(template.Text, config);

           Document document = Document.CreateInContext(
                input.Context,
                input.FileName,
                directoryCreator);

            document.Text = renderedPage;

            output.Post(document);
        }
    }
}