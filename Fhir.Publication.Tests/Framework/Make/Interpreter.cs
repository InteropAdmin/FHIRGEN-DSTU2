using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PublicationMake = Hl7.Fhir.Publication.Framework.Make;
using PublicationFramework = Hl7.Fhir.Publication.Framework;

namespace Fhir.Publication.Tests.Framework.Make
{
    [TestClass]
    public class Interpreter
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Interpreter_InterpretLine_InvalidOperationExceptionThrownWhenLineInvalid()
        {
            string makeFileText = "config.json >> IGCreate";
            var root = new PublicationFramework.Root("sourceDir", "targetDir");
            var context = new PublicationFramework.Context(root);
            var directoryCreator = new Mock.DirectoryCreator();

            PublicationMake.Interpreter.InterpretMakeFile(makeFileText, context, directoryCreator);
        }
    }
}
