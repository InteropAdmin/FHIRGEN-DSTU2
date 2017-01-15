using System;
using Hl7.Fhir.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PubOrderer = Hl7.Fhir.Publication.Specification.Profile;
using StructureDefinition = Hl7.Fhir.Model.StructureDefinition;

namespace Fhir.Publication.Tests.Specification.Profile
{
    [TestClass]
    public class Orderer
    {
        private readonly Resource _profile;

        public Orderer()
        {
            _profile = new StructureDefinition();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), " Only one Publish Order should be specified!")]
        public void Orderer_MetaDataIsValid_InvalidOperationExceptionThrownWhenMoreThanOnePublishOrderInMetaData()
        {
            var meta = new Meta();
            meta.Tag.AddRange(new[] { new Coding("urn:hscic:publishOrder", "1") , new Coding("urn:hscic:publishOrder", "2") });
            _profile.Meta = meta;

            PubOrderer.Orderer.GetOrder(_profile);
        }

        [TestMethod]
        public void Orderer_GetOrder_OrderIsZeroWhenMetaDataIsNull()
        {
            const int expected = 0;
            var actual  = PubOrderer.Orderer.GetOrder(_profile);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Orderer_GetOrder_OrderIsSeven()
        {
            const int expected = 7;
            var meta = new Meta();
            meta.Tag.AddRange(new[] { new Coding("urn:hscic:publishOrder", "7")});
            _profile.Meta = meta;

            var actual = PubOrderer.Orderer.GetOrder(_profile);

            Assert.AreEqual(expected, actual);
        }
    }
}