using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PublicationIG = Hl7.Fhir.Publication.ImplementationGuide;
using PublicationFramework = Hl7.Fhir.Publication.Framework;

namespace Fhir.Publication.Tests.Framework.ImplementationGuide
{
    [TestClass]
    public class ResourceStore
    {
        private const string _package = "PackageOne";
        private readonly PublicationFramework.Log _log;

        public ResourceStore()
        {
            _log = new PublicationFramework.Log(new Mock.ErrorLogger());             
        }

        [TestMethod]
        public void ResourceStore_GetStructureDefinitionNameByUrl_NameIsReturned()
        {
            const string expected = "badger-1-0";
            var store = new PublicationIG.ResourceStore(_log);
            var resource = new Hl7.Fhir.Model.StructureDefinition();
            resource.Name = expected;

            store.Add(new PublicationIG.PackageResource(_package, "badger-1-0", "http://fhir.nhs.net/StructureDefinition/badger-1-0", resource));
            string actual = store.GetStructureDefinitionNameByUrl("http://fhir.nhs.net/StructureDefinition/badger-1-0", _package);
            
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ResourceStore_GetStructureDefinitionNameByUrl_InvalidOperationExceptionThrownIfUrlNotFound()
        {
            var store = new PublicationIG.ResourceStore(_log);
            var resource = new Hl7.Fhir.Model.StructureDefinition();
            resource.Name = string.Empty;

            store.Add(new PublicationIG.PackageResource(_package, "http://fhir.nhs.net/StructureDefinition/patient-1-0", "patient1-0", resource));
            string actual = store.GetStructureDefinitionNameByUrl("http://fhir.nhs.net/StructureDefinition/badger-1-0", _package);
        }

        [TestMethod]
        public void ResourseStore_GetStructureDefinitionByUrl_StructureDefinitionReturned()
        {
            var store = new PublicationIG.ResourceStore(_log);
            var resource = new Hl7.Fhir.Model.StructureDefinition();
            resource.Name = "badger-1-0";

            store.Add(new PublicationIG.PackageResource(_package, "badger-1-0", "http://fhir.nhs.net/StructureDefinition/badger-1-0", resource));
            Hl7.Fhir.Model.StructureDefinition actual = store.GetStructureDefinitionByUrl("http://fhir.nhs.net/StructureDefinition/badger-1-0", _package);

            Assert.IsTrue(actual.Name == "badger-1-0");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ResourseStore_GetStructureDefinitionByUrl_InvalidOperationExceptionIfStructureDefinitionNotFound()
        {
            var store = new PublicationIG.ResourceStore(_log);
            var resource = new Hl7.Fhir.Model.OperationDefinition();
            resource.Name = "badger-1-0";

            store.Add(new PublicationIG.PackageResource(_package, "badger-1-0", "http://fhir.nhs.net/OperationDefinition/badger-1-0", resource));
            Hl7.Fhir.Model.StructureDefinition actual = store.GetStructureDefinitionByUrl("http://fhir.nhs.net/OperationDefinition/badger-1-0", _package);
        }

        [TestMethod]
        public void ResourseStore_GetOperationDefinitionByUrl_OperationDefinitionReturned()
        {
            var store = new PublicationIG.ResourceStore(_log);
            var resource = new Hl7.Fhir.Model.OperationDefinition();
            resource.Name = "badger-1-0";

            store.Add(new PublicationIG.PackageResource(_package, "badger-1-0", "http://fhir.nhs.net/OperationDefinition/badger-1-0", resource));
            Hl7.Fhir.Model.OperationDefinition actual = store.GetOperationDefinitionByUrl("http://fhir.nhs.net/OperationDefinition/badger-1-0", _package);

            Assert.IsTrue(actual.Name == "badger-1-0");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ResourseStore_GetOperationDefinitionByUrl_InvalidOperationExceptionThrownIfOperationDefinitionDoesNotExist()
        {
            var store = new PublicationIG.ResourceStore(_log);
            var resource = new Hl7.Fhir.Model.OperationDefinition();
            resource.Name = "badger-1-0";

            store.Add(new PublicationIG.PackageResource(_package, "badger-1-0", "http://fhir.nhs.net/OperationDefinition/patient-1-0", resource));
            Hl7.Fhir.Model.OperationDefinition actual = store.GetOperationDefinitionByUrl("http://fhir.nhs.net/OperationDefinition/badger-1-0", _package);
        }

        [TestMethod]
        public void ResourceStore_GetValuesteByUrl_ValuesetReturned()
        {
            var store = new PublicationIG.ResourceStore(_log);
            var resource = new Hl7.Fhir.Model.ValueSet();
            resource.Name = "badger-1-0";

            store.Add(new PublicationIG.PackageResource(_package, "badger-1-0", "http://fhir.nhs.net/Valueset/badger-1-0", resource));
            Hl7.Fhir.Model.ValueSet actual = store.GetValuesetByUrl("http://fhir.nhs.net/Valueset/badger-1-0", _package);

            Assert.IsTrue(actual.Name == "badger-1-0");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ResourseStore_GetValuesteByUrlInvalidOperationExceptionThrownIfValuesetDoesNotExist()
        {
            var store = new PublicationIG.ResourceStore(_log);
            var resource = new Hl7.Fhir.Model.ValueSet();
            resource.Name = "badger-1-0";

            store.Add(new PublicationIG.PackageResource(_package, "badger-1-0", "http://fhir.nhs.net/Valueset/patient-1-0", resource));
            Hl7.Fhir.Model.ValueSet actual = store.GetValuesetByUrl("http://fhir.nhs.net/Valueset/badger-1-0", _package);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ResourceStore_Add_InvalidOperationExceptionThrownIfDuplicateResource()
        {
            var store = new PublicationIG.ResourceStore(_log);
            var resource = new Hl7.Fhir.Model.ValueSet();

            store.Add(new PublicationIG.PackageResource(_package, "badger-1-0", "http://fhir.nhs.net/Valueset/badger-1-0", resource));
            store.Add(new PublicationIG.PackageResource(_package, "badger-1-0", "http://fhir.nhs.net/Valueset/badger-1-0", resource));
        }
    }
}