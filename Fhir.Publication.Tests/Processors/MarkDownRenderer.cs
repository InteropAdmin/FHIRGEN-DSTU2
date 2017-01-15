using System;
using System.Collections.Generic;
using System.Linq;
using Hl7.Fhir.Publication.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PublicationProcessors = Hl7.Fhir.Publication.Processors;

namespace Fhir.Publication.Tests.Processors
{
    [TestClass]
    public class MarkDownRenderer
    {
        private const string _markdownToTransform =
            @"# About #\r\n\r\n### Overview: FHIR Implementation Guide for GP Systems of Choice (GPSoC) IM2 #\r\n\r\n";
        private const string _path = @"TestData\";
        private readonly IDirectoryCreator _dirCreator = new Mock.DirectoryCreator();
        private readonly Log _log;
        private readonly Stage _output;
        private Document _input;
       
        public MarkDownRenderer()
        {
            _log = new Log(new Mock.ErrorLogger());

            _output = new Stage(new List<Document>());
        }

        [TestMethod]
        public void MarkDownRenderer_Render_DocumentRenderedIsHtml()
        {
            var context = new Context(new Root("sourceDir", "targetDir"));
            _input = Document.CreateFromFullPath(context, _path);
            _input.Text = _markdownToTransform;

            var renderer = new PublicationProcessors.MarkdownProcessor();

            renderer.Process(_input, _output, _log, _dirCreator);

            Assert.IsTrue(_output.Documents.Single().Extension == ".html");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MarkDownRenderer_Render_InvalidOperationExceptionThownWhenInputTextIsEmpty()
        {
            var context = new Context(new Root("sourceDir", "targetDir"));
            _input = Document.CreateFromFullPath(context, _path);
            _input.Text = string.Empty;
           
            var renderer = new PublicationProcessors.MarkdownProcessor();
            renderer.Process(_input, _output, _log, _dirCreator);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MarkDownRenderer_Render_InvalidOperationExceptionThownWhenInputTextIsNull()
        {
            var context = new Context(new Root("sourceDir", "targetDir"));
            _input = Document.CreateFromFullPath(context, _path);
            _input.Text = null;
      
            var renderer = new PublicationProcessors.MarkdownProcessor();
            renderer.Process(_input, _output, _log, _dirCreator);
        }
    }
}