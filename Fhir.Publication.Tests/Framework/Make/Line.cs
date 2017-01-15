using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PublicationMake = Hl7.Fhir.Publication.Framework.Make;

namespace Fhir.Publication.Tests.Framework.Make
{
    [TestClass]
    public class Line
    {
        [TestMethod]
        public void Line_IsValid_IsTrue()
        {
            var line = new PublicationMake.Line("select config.json >> IGCreate");

            Assert.IsTrue(line.IsValid);
        }

        [TestMethod]
        public void Line_IsValid_IsFalseWhenNoSelect()
        {
            var line = new PublicationMake.Line("config.json >> IGCreate");

            Assert.IsFalse(line.IsValid);
        }

        [TestMethod]
        public void Line_IsValid_IsFalseWhenNoChevron()
        {
            var line = new PublicationMake.Line("select config.json IGCreate");

            Assert.IsFalse(line.IsValid);
        }

        [TestMethod]
        public void Line_File_IsConfigFile()
        {
            var line = new PublicationMake.Line("select config.json >> IGCreate");

            Assert.IsTrue(line.File == "config.json");
        }

        [TestMethod]
        public void Line_IsRecursive_IsTrue()
        {
            var line = new PublicationMake.Line("select index.md -recursive >> markdown");

            Assert.IsTrue(line.IsRecursive);
        }

        [TestMethod]
        public void Line_IsRecursive_IsFalse()
        {
            var line = new PublicationMake.Line("select index.md >> markdown");

            Assert.IsFalse(line.IsRecursive);
        }

        [TestMethod]
        public void Line_IsOutput_IsTrue()
        {
            var line = new PublicationMake.Line("select index.md -output >> markdown");

            Assert.IsTrue(line.IsOutput);
        }

        [TestMethod]
        public void Line_IsOutput_IsFalse()
        {
            var line = new PublicationMake.Line("select index.md -recursive >> markdown");

            Assert.IsFalse(line.IsOutput);
        }

        [TestMethod]
        public void Line_Processors_CountIsThree()
        {
            var line = new PublicationMake.Line("select index.md >> markdown >> template $template >> save");

            Assert.IsTrue(line.Processors.Count() == 3);
        }
    }
}
