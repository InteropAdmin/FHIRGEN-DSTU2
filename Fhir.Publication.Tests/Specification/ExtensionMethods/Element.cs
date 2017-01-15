using Hl7.Fhir.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hl7.Fhir.Publication.Specification.ExtensionMethods;

namespace Fhir.Publication.Tests.Specification.ExtensionMethods
{
    [TestClass]
    public class Element
    {
        [TestMethod]
        public void Element_ForDisplay_CodingElementReturnsNameInTitleCase()
        {
            var coding = new Coding();
            Assert.IsTrue("Coding" == coding.ForDisplay());            
        }

        [TestMethod]
        public void Element_ForDisplay_DateTimeElementReturnsNameInTitleCase()
        {
            var timeStamp = new FhirDateTime();
            Assert.IsTrue("DateTime" == timeStamp.ForDisplay());
        }
    }
}