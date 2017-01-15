using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dict = Hl7.Fhir.Publication.Specification.Profile.Structure.Dictionary;

namespace Fhir.Publication.Tests.Specification.Profile.Structure.Dictionary
{
    [TestClass]
    public class Row
    {
        [TestMethod]
        public void CreateRowDescription_valueIsPopulated_RowPopulatedWithNameLinkAndDescription()
        {
            string actual = new Dict.Row("Type", "datatypes.html", "DomainResource").Value;
            string expected = string.Concat("  <tr><td><a href=\"http://www.hl7.org/fhir/datatypes.html\">Type</a></td><td>DomainResource</td></tr>", Environment.NewLine);

            Assert.AreEqual(expected, actual, "Table row does not match");
        }

        [TestMethod]
        public void CreateRowDescription_valueIsNull_EmptyStringReturned()
        {
            string actual = new Dict.Row("Type", "datatypes.html", null).Value;
            string expected = string.Empty;

            Assert.AreEqual(expected, actual, "Table row does not match");
        }

        [TestMethod]
        public void CreateRowWithDescription_DefinitionReferenceIsNull_RowPopulatedWithNameAndDescription()
        {
            string actual = new Dict.Row("Type", null, "DomainResource").Value;
            string expected = string.Concat("  <tr><td>Type</td><td>DomainResource</td></tr>", Environment.NewLine);
            
            Assert.AreEqual(expected, actual, "Table row does not match");
        }

        [TestMethod]
        public void CreateRowWithEncodedDescription_valueIsPopulated_RowPopulatedWithNameLinkAndDescription()
        {
            string actual = new Dict.Row("Control", "conformance - rules.html#conformance", "1..1").Value;
            string expected = string.Concat(@"  <tr><td><a href=""http://www.hl7.org/fhir/conformance - rules.html#conformance"">Control</a></td><td>1..1</td></tr>", Environment.NewLine);

            Assert.AreEqual(expected, actual, "Table row does not match");
        }

        [TestMethod]
        public void CreateRowWithEncodedDescription_valueIsNull_EmptyStringReturned()
        {
            string actual = new Dict.Row("Control", "conformance - rules.html#conformance", null).Value;
            string expected = string.Empty;

            Assert.AreEqual(expected, actual, "Table row does not match");
        }

        [TestMethod]
        public void CreateRowWithEncodedDescription_DefinitionReferenceIsNull_RowPopulatedWithNameAndDescription()
        {
            string actual = new Dict.Row("Control", null, "1..1").Value;
            string expected = string.Concat("  <tr><td>Control</td><td>1..1</td></tr>", Environment.NewLine);

            Assert.AreEqual(expected, actual, "Table row does not match");
        }
    }
}
