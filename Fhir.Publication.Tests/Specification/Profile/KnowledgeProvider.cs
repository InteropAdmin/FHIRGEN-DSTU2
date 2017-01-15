using System;
using Hl7.Fhir.Publication.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;
using PublicProfile = Hl7.Fhir.Publication.Specification;

namespace Fhir.Publication.Tests.Specification.Profile
{
    [TestClass]
    public class KnowledgeProvider
    {
        private const string _version = "v2";

        [TestMethod]
        public void ProfileKnowledgeProvider_GetSpecLink_LinkReturnedIsMatch()
        {
            Assert.AreEqual(
                string.Concat(Url.Dstu.GetUrlString(), _version),
                PublicProfile.Profile.KnowledgeProvider.GetSpecLink(_version), 
                "Spec links do not match");
        }

        [TestMethod]
        public void ProfileKnowledgeProvider_GetLinkForExamplesPage_LinkReturnedIsMatch()
        {
            const string resourceName = "myResource";
            const string expected = @"<a href=""Examples.html#myResource"">Examples</a>";
            string actual = PublicProfile.Profile.KnowledgeProvider.ExamplesPageLink(resourceName);

            Assert.AreEqual(expected, actual, "Examples page links do not match");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "myType does not begin with http://fhir.nhs.net/!")]
        public void ProfileKnowledgeProvider_RemovePrefix_ExceptionThrowWhenTypeDoesnotstartWithFhirPrefix()
        {
            PublicProfile.Profile.KnowledgeProvider.RemovePrefix("mytype");
        }

        [TestMethod]
        public void ProfileKnowldegeProvider_RemovePrefix_PrefixRemoved()
        {
            string actual = PublicProfile.Profile.KnowledgeProvider.RemovePrefix("http://fhir.nhs.net/KatType");
            const string expected = "KatType";

            Assert.AreEqual(expected, actual, "Prefix is not removed!");
        }
    }
}