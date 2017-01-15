using System;
using System.IO;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Specification.Page;
using Hl7.Fhir.Publication.Specification.Profile;

namespace Hl7.Fhir.Publication.Razor
{
    internal class PageRenderer
    {
        private const string _html = ".html";
        private readonly Log _log;
        private readonly Document _template;
        private readonly Context _context;
        private readonly IDirectoryCreator _dirCreator;

        public PageRenderer(Log log, Document template, IDirectoryCreator directoryCreator, Context context)
        {
            if (log == null)
                throw new ArgumentNullException(
                    nameof(log));

            if (template == null)
                throw new ArgumentNullException(
                    nameof(template));

            if (directoryCreator == null)
                throw new ArgumentNullException(
                    nameof(directoryCreator));

            if (context == null)
                throw new ArgumentNullException(
                    nameof(context));

            _log = log;
            _template = template;
            _dirCreator = directoryCreator;
            _context = context;
        }

        public Document GetResourcePage(Config pageConfig, string resourceName, string packagePath)
        {
            _log.Info($"Render page {resourceName}");

            var renderedPage = Razor.Render(_template.Text, pageConfig);

            Document document = Document.CreateInContext(
                _context,
                Path.Combine(
                    _context.Target.ToString(),
                    packagePath,
                   KnowledgeProvider.GetLinkForLocalResource(resourceName)),
                _dirCreator);

            document.Text = renderedPage;

            return document;
        }

        public Document GetDictionaryPage(Config pageConfig, string resourceName, string packagePath)
        {
            _log.Info($"Render dictionary page for {resourceName}");

            var renderedPage = Razor.Render(_template.Text, pageConfig);

            Document document = Document.CreateInContext(
                       _context,
                       Path.Combine(_context.Target.Directory, packagePath, KnowledgeProvider.GetLinkForProfileDict(resourceName)),
                       _dirCreator);

            document.Text = renderedPage;

            return document;
        }

        public Document GetProfileIndexPage(Config pageConfig, string resourceName)
        {
            _log.Info($"Render profile table {resourceName}");

            var renderedPage = Razor.Render(_template.Text, pageConfig);

            Document document = Document.CreateInContext(
                 _context,
                 Path.Combine(_context.Target.ToString(), resourceName, string.Concat(resourceName, _html)),
                 _dirCreator);

            document.Text = renderedPage;

            return document;
        }
    }
}