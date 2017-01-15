using Hl7.Fhir.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PubProfile = Hl7.Fhir.Publication.Specification.Profile;

namespace Fhir.Publication.Tests.Specification.Profile
{
    [TestClass]
    public class Cardinality
    {
        private readonly ElementDefinition _definition;

        public Cardinality()
        {
            _definition = new ElementDefinition();
            _definition.Min = 0;
        }

        [TestMethod]
        public void Cardinality_Describe_MaxIsNullGivesZeroToMany()
        {
            _definition.Max = null;
            Assert.IsTrue(PubProfile.Cardinality.Describe(_definition.Min.ToString(), _definition.Max) == "0..*");
        }

        [TestMethod]
        public void Cardinality_Describe_MaxIsMinusOneGivesZeroToMany()
        {
            _definition.Max = "-1";
            Assert.IsTrue(PubProfile.Cardinality.Describe(_definition.Min.ToString(), _definition.Max) == "0..*");
        }

        [TestMethod]
        public void Cardinality_Describe_MaxIsOneGivesZeroToOne()
        {
            _definition.Max = "1";
            Assert.IsTrue(PubProfile.Cardinality.Describe(_definition.Min.ToString(), _definition.Max) == "0..1");
        }

        [TestMethod]
        public void Cardinality_Describe_MinIsNullMAxIsOneGivesZeroToOne()
        {
            _definition.Max = "1";
            Assert.IsTrue(PubProfile.Cardinality.Describe(null, _definition.Max) == "0..1");
        }
    }
}
