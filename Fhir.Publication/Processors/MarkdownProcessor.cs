using System;
using Hl7.Fhir.Publication.Framework;
using MarkdownDeep;

namespace Hl7.Fhir.Publication.Processors
{
    internal class MarkdownProcessor : IProcessor
    {
        private const string _html = ".html";

        public ISelector Influx { get; set; }

        public void Process(
            Document input,
            Stage stage,
            Log log,
            IDirectoryCreator directoryCreator)
        {
            log.Debug(
                $"Markdown rendering - {input.Name} \t: {directoryCreator.GetFileName(input.SourceFullPath)}");

            Document output = stage.CloneAndPost(input);

            if (string.IsNullOrEmpty(input.Text))
                throw new InvalidOperationException("input document must contain text.");

            output.Extension = _html;
            var markDown = new Markdown();

            markDown.SafeMode = false;
            markDown.ExtraMode = true;

            output.Text = markDown.Transform(input.Text);
        }
    }
}