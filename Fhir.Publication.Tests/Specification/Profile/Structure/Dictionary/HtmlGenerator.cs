using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.ImplementationGuide;
using Hl7.Fhir.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PubFramework = Hl7.Fhir.Publication.Framework;
using PubDictionary = Hl7.Fhir.Publication.Specification.Profile.Structure.Dictionary;

namespace Fhir.Publication.Tests.Specification.Profile.Structure.Dictionary
{
    [TestClass]
    public class HtmlGenerator
    {
        private const string _package = "ProfileOne";
        private readonly PubDictionary.HtmlGenerator _htmlGenerator;
        private readonly Package _packageFactory;

        public HtmlGenerator()
        {
            var log = new PubFramework.Log(new Mock.ErrorLogger());
            var knowledgeProvider = new Hl7.Fhir.Publication.Specification.Profile.KnowledgeProvider(log);
            _htmlGenerator = new PubDictionary.HtmlGenerator(knowledgeProvider);
            var context = new PubFramework.Context(new PubFramework.Root("sourceDir", "targetDir"));
            _packageFactory = new Package("ProfileOne", context, log, new Mock.DirectoryCreator());
        }

        [TestMethod]
        [IntegrationTest]
        public void HtmlGenerator_Generate_DictionaryPageContainsAllElementsInBasicResource()
        {
            string resourceXml = Resources.BasicResourceNoEdits;

            Hl7.Fhir.Model.Resource resource =FhirParser.ParseResourceFromXml(resourceXml);

            var structureDefinition = (StructureDefinition)resource;

            var actual =  _htmlGenerator.Generate(structureDefinition, _packageFactory.ResourceStore, _package).ToString();
            string expected = Resources.BasicResourceNoEditsHtml;
            
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [IntegrationTest]
        public void HtmlGenerator_Generate_DictionaryPageDoesnotContainAuthor()
        {
            string resourceXml = Resources.BasicResourceAuthorHasZeroCardinality;

            Hl7.Fhir.Model.Resource resource = FhirParser.ParseResourceFromXml(resourceXml);

            var structureDefinition = (StructureDefinition)resource;

            var actual = _htmlGenerator.Generate(structureDefinition, _packageFactory.ResourceStore, _package).ToString();
            string expected = Resources.BasicResourceAuthorHasZeroCardinalityHtml;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [IntegrationTest]
        public void HtmlGenerator_Generate_DictionaryPageDoesnotContainReferenceOrDisplay()
        {
            string resourceXml = Resources.BasicResourceChildElementHasParentWithZeroCardinality;

            Hl7.Fhir.Model.Resource resource = FhirParser.ParseResourceFromXml(resourceXml);

            var structureDefinition = (StructureDefinition)resource;

            var actual = _htmlGenerator.Generate(structureDefinition, _packageFactory.ResourceStore, _package).ToString();

            string expected = Resources.BasicResourceChildElementHasParentWithZeroCardinalityHtml;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [IntegrationTest]
        public void HtmlGenerator_Generate_DictionaryPageContainsAllSliceElementsNHSNumberAndIdentifier()
        {
            string resourceXml = Resources.BasicResourceIdentifierIsSliced;

            Hl7.Fhir.Model.Resource resource = FhirParser.ParseResourceFromXml(resourceXml);

            var structureDefinition = (StructureDefinition)resource;

            var actual = _htmlGenerator.Generate(structureDefinition, _packageFactory.ResourceStore, _package).ToString();

            string expected = Resources.BasicResourceIdentifierIsSlicedHtml;

            Assert.AreEqual(expected, actual);
        }
    }
}