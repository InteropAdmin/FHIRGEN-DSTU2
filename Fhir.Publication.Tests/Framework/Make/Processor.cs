using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PublicationMake = Hl7.Fhir.Publication.Framework.Make;
using PublicationFramework = Hl7.Fhir.Publication.Framework;

namespace Fhir.Publication.Tests.Framework.Make
{
    [TestClass]
    public class Processor
    {
        [TestMethod]
        public void Processor_ProcessorStatement_IsValidIsTrue()
        {
            var processor = new PublicationMake.Processor(">> IGCreate");

            Assert.IsTrue(processor.IsValid);
        }

        [TestMethod]
        public void Processor_ProcessorStatementNoChevrons_IsValidIsFalse()
        {
            var processor = new PublicationMake.Processor(" IGCreate");

            Assert.IsFalse(processor.IsValid);
        }

        [TestMethod]
        public void Processor_ProcessorStatementWithParameter_IsValidIsTrue()
        {
            var processor = new PublicationMake.Processor(">> makeall Profile.*");

            Assert.IsTrue(processor.IsValid);
        }

        [TestMethod]
        public void Processor_ProcessorStatementWithTwoParameters_IsValidIsTrue()
        {
            var processor = new PublicationMake.Processor(">> makeall Profile.* Resource.*");

            Assert.IsTrue(processor.IsValid);
        }
        
        [TestMethod]
        public void Processor_ProcessorStatementWithTwoParameters_ParameterCountEqualsTwo()
        {
            var processor = new PublicationMake.Processor(">> makeall Profile.* Resource.*");

            IEnumerable<string> parameters = processor.Parameters.ToArray();

            Assert.IsTrue(parameters.Count() == 2);
        }

        [TestMethod]
        public void Processor_IsFilteredBy_ContextIsNotFiltered()
        {
            var processor = new PublicationMake.Processor(">> makeall Profile.*");
            var root = new PublicationFramework.Root("sourceDir", "targetDir");
            var context = new PublicationFramework.Context(root);
            context.FilterPattern = "Profile.*";

            Assert.IsTrue(processor.IsFilteredBy(context));
        }

        [TestMethod]
        public void Processor_IsFilteredBy_ContextIsFilteredIsFalse()
        {
            var processor = new PublicationMake.Processor(">> makeall Profile");
            var root = new PublicationFramework.Root("Profile", "targetDir");
            var context = new PublicationFramework.Context(root);
            
            Assert.IsFalse(processor.IsFilteredBy(context));
        }

        [TestMethod]
        public void Processor_IsFilteredBy_FilterPatternIsEmptyReturnsFalse()
        {
            var processor = new PublicationMake.Processor(">> makeall Profile.*");
            var root = new PublicationFramework.Root("sourceDir", "targetDir");
            var context = new PublicationFramework.Context(root);
            context.FilterPattern = string.Empty;

            Assert.IsFalse(processor.IsFilteredBy(context));
        }

        [TestMethod]
        public void Processor_IsFilteredBy_FilterPatternIsNullReturnsFalse()
        {
            var processor = new PublicationMake.Processor(">> makeall Profile.*");
            var root = new PublicationFramework.Root("sourceDir", "targetDir");
            var context = new PublicationFramework.Context(root);
            context.FilterPattern = null;

            Assert.IsFalse(processor.IsFilteredBy(context));
        }

        [TestMethod]
        public void Processor_IsFilteredBy_NoFilteresReturnsFalse()
        {
            var processor = new PublicationMake.Processor(">> makeall");
            var root = new PublicationFramework.Root("sourceDir", "targetDir");
            var context = new PublicationFramework.Context(root);
            context.FilterPattern = "myFilter";

            Assert.IsFalse(processor.IsFilteredBy(context));
        }

        [TestMethod]
        public void Processor_Command_IsReturned()
        {
            var processor = new PublicationMake.Processor(">> IGCreate");

            Assert.IsTrue(processor.Command == "IGCreate");
        }
    }
}
