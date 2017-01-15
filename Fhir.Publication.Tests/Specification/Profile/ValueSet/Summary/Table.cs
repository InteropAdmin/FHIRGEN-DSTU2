using System;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PubSummary = Hl7.Fhir.Publication.Specification.Profile.ValueSet.Summary;

namespace Fhir.Publication.Tests.Specification.Profile.ValueSet.Summary
{
    [TestClass]
    public class Table
    {
        private const string _packageName = "Profile.GetRecordQueryResponse-Phase-3";
        private const string _url = "http://fhir.nhs.net/ValueSet/administrative-gender-ddmap-1-0";
        private const string _name = "Administrative-Gender-DDMap-1-0";
        private const string _definition = "Here is a valueset description";
        private const string _copyright = "This is copyright!";
        private XElement _summaryTable;
        private XElement _summaryTableNoOID;
        private XElement _summaryTableNoCopyright;
        private XElement _summaryTableNoLastUpdated;
        private DateTimeOffset _offsetTime;

        public Table()
        {
            SetSummary();
            SetSummaryNoOID();
            SetSummaryNoCopyright();
            SetSummaryNoLastUpdatedDate();
        }

        private void SetSummary()
        {
            var valueset = new Hl7.Fhir.Model.ValueSet();
            valueset.Name = _name;
            valueset.Url = _url;
            valueset.Description = _definition;
            valueset.Copyright = _copyright;
            valueset.Status = ConformanceResourceStatus.Active;
            var date = new DateTime(2016, 9, 2, 9, 25, 02);
            _offsetTime = new DateTimeOffset(date, new TimeSpan());
            valueset.Meta = new Meta();
            valueset.Meta.LastUpdated = _offsetTime;

            var oid = string.Concat(Urn.Oid.GetUrnString(), "2.16.840.1.113883.2.1.3.2.4.16.25");
            var extensions = new System.Collections.Generic.List<Extension>();
            var extension = new Extension("url", new FhirUri(oid));
            extensions.Add(extension);
            valueset.Extension = extensions;

            _summaryTable = PubSummary.Table.ToHtml(new PubSummary.TableContents(valueset, true, true, _packageName));
        }

        private void SetSummaryNoOID()
        {
            var valueset = new Hl7.Fhir.Model.ValueSet();
            valueset.Name = _name;
            valueset.Url = _url;
            valueset.Description = _definition;
            valueset.Copyright = _copyright;
            valueset.Status = ConformanceResourceStatus.Active;
            var date = new DateTime(2016, 9, 2, 9, 25, 02);
            _offsetTime = new DateTimeOffset(date, new TimeSpan());
            valueset.Meta = new Meta();
            valueset.Meta.LastUpdated = _offsetTime;

            _summaryTableNoOID = PubSummary.Table.ToHtml(new PubSummary.TableContents(valueset, true, true, _packageName));
        }

        private void SetSummaryNoCopyright()
        {
            var valueset = new Hl7.Fhir.Model.ValueSet();
            valueset.Name = _name;
            valueset.Url = _url;
            valueset.Description = _definition;
            valueset.Status = ConformanceResourceStatus.Active;
            var date = new DateTime(2016, 9, 2, 9, 25, 02);
            _offsetTime = new DateTimeOffset(date, new TimeSpan());
            valueset.Meta = new Meta();
            valueset.Meta.LastUpdated = _offsetTime;

            var oid = string.Concat(Urn.Oid.GetUrnString(), "2.16.840.1.113883.2.1.3.2.4.16.25");
            var extensions = new System.Collections.Generic.List<Extension>();
            var extension = new Extension("url", new FhirUri(oid));
            extensions.Add(extension);
            valueset.Extension = extensions;

            _summaryTableNoCopyright = PubSummary.Table.ToHtml(new PubSummary.TableContents(valueset, true, true, _packageName));
        }

        private void SetSummaryNoLastUpdatedDate()
        {
            var valueset = new Hl7.Fhir.Model.ValueSet();
            valueset.Name = _name;
            valueset.Url = _url;
            valueset.Description = _definition;
            valueset.Copyright = _copyright;
            valueset.Status = ConformanceResourceStatus.Active;

            var oid = string.Concat(Urn.Oid.GetUrnString(), "2.16.840.1.113883.2.1.3.2.4.16.25");
            var extensions = new System.Collections.Generic.List<Extension>();
            var extension = new Extension("url", new FhirUri(oid));
            extensions.Add(extension);
            valueset.Extension = extensions;

            _summaryTableNoLastUpdated = PubSummary.Table.ToHtml(new PubSummary.TableContents(valueset, true, true, _packageName));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Table_NullTableContentsThrowsArgumentNullException()
        {
           var table = PubSummary.Table.ToHtml(null);
        }

        [TestMethod]
        [IntegrationTest]
        public void Table_ToHtml_FullyPopulatedSummaryTable()
        {
            var actual = _summaryTable.ToString();

            Assert.AreEqual(actual, Resources.ValuesetFullSummaryTable); 
        }

        [TestMethod]
        [IntegrationTest]
        public void Table_ToHtml_SummaryTableHasNoOIDRow()
        {
            var actual = _summaryTableNoOID.ToString();

            Assert.AreEqual(actual, Resources.ValuesetSummaryNoOID);
        }

        [TestMethod]
        [IntegrationTest]
        public void Table_ToHtml_SummaryTableHasNoCopyrightRow()
        {
            var actual = _summaryTableNoCopyright.ToString();

            Assert.AreEqual(actual, Resources.ValuesetSummaryNoCopyrightRow);
        }

        [TestMethod]
        [IntegrationTest]
        public void Table_ToHtml_SummaryTableHasNoLastupdatedRow()
        {
            var actual = _summaryTableNoLastUpdated.ToString();

            Assert.AreEqual(actual, Resources.ValuesetSummaryNoLastupdatedRow);
        }
    }
}