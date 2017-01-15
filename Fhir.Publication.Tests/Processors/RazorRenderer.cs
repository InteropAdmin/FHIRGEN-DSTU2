using System;
using System.Collections.Generic;
using Hl7.Fhir.Publication.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PubProcessors = Hl7.Fhir.Publication.Processors;

namespace Fhir.Publication.Tests.Processors
{
    [TestClass]
    public class RazorRenderer
    {
        private const string _path = @"TestData\";
        private readonly Mock.DirectoryCreator _dirCreator;
        private readonly Stage _output;
        private readonly Log _log;
        private Document _input;
        
        public RazorRenderer()
        {
            _dirCreator = new Mock.DirectoryCreator();
            _log = new Log(new Mock.ErrorLogger());

            var documents = new List<Document>();
            var context = new Context(new Root("SourceDir", "targetDir"));
            documents.Add(Document.CreateInContext(context, "fileName", _dirCreator));
            _output = new Stage(documents);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RazorRenderer_Render_InValidOperationExceptionThrownIfInputTextIsEmpty()
        {
            var context = new Context(new Root("sourceDir", "targetDir"));
            _input = Document.CreateFromFullPath(context, _path);
            _input.Text = string.Empty;
     
            var renderer = new PubProcessors.RazorProcessor();
            renderer.Process(_input, _output, _log, _dirCreator);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RazorRenderer_Render_ArgumentNullExceptionThrownIfOutputIsNull()
        {
            var context = new Context(new Root("sourceDir", "targetDir"));
            _input = Document.CreateFromFullPath(context, _path);
            _input.Text = "my text!";

            var renderer = new PubProcessors.RazorProcessor();
            renderer.Process(_input, null, _log, _dirCreator);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RazorRenderer_Render_ArgumentNullExceptionThrownIfDirCreatorIsNull()
        {
            var context = new Context(new Root("sourceDir", "targetDir"));
            _input = Document.CreateFromFullPath(context, _path);
            _input.Text = "my text!";

            var renderer = new PubProcessors.RazorProcessor();
            renderer.Process(_input, _output, _log, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RazorRenderer_Render_InValidOperationExceptionThrownIfInputTextIsNull()
        {
            var context = new Context(new Root("sourceDir", "targetDir"));
            _input = Document.CreateFromFullPath(context, _path);
            _input.Text = null;

            var renderer = new PubProcessors.RazorProcessor();
            renderer.Process(_input, _output, _log, _dirCreator);
        }
    }
}
