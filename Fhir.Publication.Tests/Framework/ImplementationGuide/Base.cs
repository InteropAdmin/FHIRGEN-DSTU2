using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IG = Hl7.Fhir.Publication.ImplementationGuide;
using PublicationFramework = Hl7.Fhir.Publication.Framework;

namespace Fhir.Publication.Tests.Framework.ImplementationGuide
{
    [TestClass]
    public class Base
    {
        private readonly PublicationFramework.IDirectoryCreator _directoryCreator;
        private readonly PublicationFramework.Context _context;
        public Base()
        {
            _directoryCreator = new Mock.DirectoryCreator();
            var root = new PublicationFramework.Root("sourceDir", "targetDir");
            _context = new PublicationFramework.Context(root);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ImplementationGuide_Base_ArgumentNullExceptionThrownWhenDirectoryCreatorIsNull()
        {
          var guide = new IG.Base(null, _context);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ImplementationGuide_Base_ArgumentNullExceptionThrownWhenContextIsNull()
        {
           var guide = new IG.Base(_directoryCreator, null);
        }

        [TestMethod]
        public void ImplementationGuide_Base_CreateImplementationGuide_ExamplePropertiesAreTrue()
        {
            var guide = new IG.Base(_directoryCreator, _context);
            var configValues = new Dictionary<string, string>();

            configValues.Add("DMS_ONLINE", "FALSE");
            configValues.Add("DMS_ANALYTICS", "UA-67795986-1");
            configValues.Add("EXAMPLES_XML", "TRUE");
            configValues.Add("EXAMPLES_JSON", "TRUE");
            configValues.Add("VALUESETS_XML", "FALSE");
            configValues.Add("VALUESETS_JSON", "FALSE");
            configValues.Add("STRUCTURES_XML", "FALSE");
            configValues.Add("STRUCTURES_JSON", "FALSE");
            configValues.Add("OPERATIONS_XML", "FALSE");
            configValues.Add("OPERATIONS_JSON", "FALSE");
            configValues.Add("DMS_TITLE", "Kats Unicorn DMS");
            configValues.Add("DMS_VERSION", "Version 1.0");
            configValues.Add("SCHEMAS", "True");

            guide.CreateImplementationGuide(configValues);

            Assert.IsTrue(guide.ExamplesXml);
            Assert.IsTrue(guide.ExamplesJson);
        }

        [TestMethod]
        public void ImplementationGuide_Base_CreateImplementationGuide_ExamplePropertiesAreFalse()
        {
            var guide = new IG.Base(_directoryCreator, _context);
            var configValues = new Dictionary<string, string>();

            configValues.Add("DMS_ONLINE", "FALSE");
            configValues.Add("DMS_ANALYTICS", "UA-67795986-1");
            configValues.Add("EXAMPLES_XML", "FALSE");
            configValues.Add("EXAMPLES_JSON", "FALSE");
            configValues.Add("VALUESETS_XML", "FALSE");
            configValues.Add("VALUESETS_JSON", "FALSE");
            configValues.Add("STRUCTURES_XML", "FALSE");
            configValues.Add("STRUCTURES_JSON", "FALSE");
            configValues.Add("OPERATIONS_XML", "FALSE");
            configValues.Add("OPERATIONS_JSON", "FALSE");
            configValues.Add("DMS_TITLE", "Kats Unicorn DMS");
            configValues.Add("DMS_VERSION", "Version 1.0");
            configValues.Add("SCHEMAS", "True");

            guide.CreateImplementationGuide(configValues);

            Assert.IsFalse(guide.ExamplesXml);
            Assert.IsFalse(guide.ExamplesJson);
        }

        [TestMethod]
        public void ImplementationGuide_Base_CreateImplementationGuide_ValuesetsPropertiesAreTrue()
        {
            var guide = new IG.Base(_directoryCreator, _context);
            var configValues = new Dictionary<string, string>();

            configValues.Add("DMS_ONLINE", "FALSE");
            configValues.Add("DMS_ANALYTICS", "UA-67795986-1");
            configValues.Add("EXAMPLES_XML", "FALSE");
            configValues.Add("EXAMPLES_JSON", "FALSE");
            configValues.Add("VALUESETS_XML", "TRUE");
            configValues.Add("VALUESETS_JSON", "TRUE");
            configValues.Add("STRUCTURES_XML", "FALSE");
            configValues.Add("STRUCTURES_JSON", "FALSE");
            configValues.Add("OPERATIONS_XML", "FALSE");
            configValues.Add("OPERATIONS_JSON", "FALSE");
            configValues.Add("DMS_TITLE", "Kats Unicorn DMS");
            configValues.Add("DMS_VERSION", "Version 1.0");
            configValues.Add("SCHEMAS", "True");

            guide.CreateImplementationGuide(configValues);
            
            Assert.IsTrue(guide.ValuesetsInXml);
            Assert.IsTrue(guide.ValuesetsInJson);
        }

        [TestMethod]
        public void ImplementationGuide_Base_CreateImplementationGuide_ValuesetsPropertiesAreFalse()
        {
            var guide = new IG.Base(_directoryCreator, _context);
            var configValues = new Dictionary<string, string>();

            configValues.Add("DMS_ONLINE", "FALSE");
            configValues.Add("DMS_ANALYTICS", "UA-67795986-1");
            configValues.Add("EXAMPLES_XML", "FALSE");
            configValues.Add("EXAMPLES_JSON", "FALSE");
            configValues.Add("VALUESETS_XML", "FALSE");
            configValues.Add("VALUESETS_JSON", "FALSE");
            configValues.Add("STRUCTURES_XML", "FALSE");
            configValues.Add("STRUCTURES_JSON", "FALSE");
            configValues.Add("OPERATIONS_XML", "FALSE");
            configValues.Add("OPERATIONS_JSON", "FALSE");
            configValues.Add("DMS_TITLE", "Kats Unicorn DMS");
            configValues.Add("DMS_VERSION", "Version 1.0");
            configValues.Add("SCHEMAS", "True");

            guide.CreateImplementationGuide(configValues);

            Assert.IsFalse(guide.ValuesetsInXml);
            Assert.IsFalse(guide.ValuesetsInJson);
        }

        [TestMethod]
        public void ImplementationGuide_Base_CreateImplementationGuide_StructuresPropertiesAreTrue()
        {
            var guide = new IG.Base(_directoryCreator, _context);
            var configValues = new Dictionary<string, string>();

            configValues.Add("DMS_ONLINE", "FALSE");
            configValues.Add("DMS_ANALYTICS", "UA-67795986-1");
            configValues.Add("EXAMPLES_XML", "FALSE");
            configValues.Add("EXAMPLES_JSON", "FALSE");
            configValues.Add("VALUESETS_XML", "FALSE");
            configValues.Add("VALUESETS_JSON", "FALSE");
            configValues.Add("STRUCTURES_XML", "TRUE");
            configValues.Add("STRUCTURES_JSON", "TRUE");
            configValues.Add("OPERATIONS_XML", "FALSE");
            configValues.Add("OPERATIONS_JSON", "FALSE");
            configValues.Add("DMS_TITLE", "Kats Unicorn DMS");
            configValues.Add("DMS_VERSION", "Version 1.0");
            configValues.Add("SCHEMAS", "True");

            guide.CreateImplementationGuide(configValues);

            Assert.IsTrue(guide.StructuresInXml);
            Assert.IsTrue(guide.StructuresInJson);
        }

        [TestMethod]
        public void ImplementationGuide_Base_CreateImplementationGuide_StructuresPropertiesAreFalse()
        {
            var guide = new IG.Base(_directoryCreator, _context);
            var configValues = new Dictionary<string, string>();

            configValues.Add("DMS_ONLINE", "FALSE");
            configValues.Add("DMS_ANALYTICS", "UA-67795986-1");
            configValues.Add("EXAMPLES_XML", "FALSE");
            configValues.Add("EXAMPLES_JSON", "FALSE");
            configValues.Add("VALUESETS_XML", "FALSE");
            configValues.Add("VALUESETS_JSON", "FALSE");
            configValues.Add("STRUCTURES_XML", "FALSE");
            configValues.Add("STRUCTURES_JSON", "FALSE");
            configValues.Add("OPERATIONS_XML", "FALSE");
            configValues.Add("OPERATIONS_JSON", "FALSE");
            configValues.Add("DMS_TITLE", "Kats Unicorn DMS");
            configValues.Add("DMS_VERSION", "Version 1.0");
            configValues.Add("SCHEMAS", "True");

            guide.CreateImplementationGuide(configValues);

            Assert.IsFalse(guide.StructuresInXml);
            Assert.IsFalse(guide.StructuresInJson);
        }

        [TestMethod]
        public void ImplementationGuide_Base_CreateImplementationGuide_OperationsPropertiesAreTrue()
        {
            var guide = new IG.Base(_directoryCreator, _context);
            var configValues = new Dictionary<string, string>();

            configValues.Add("DMS_ONLINE", "FALSE");
            configValues.Add("DMS_ANALYTICS", "UA-67795986-1");
            configValues.Add("EXAMPLES_XML", "FALSE");
            configValues.Add("EXAMPLES_JSON", "FALSE");
            configValues.Add("VALUESETS_XML", "FALSE");
            configValues.Add("VALUESETS_JSON", "FALSE");
            configValues.Add("STRUCTURES_XML", "FALSE");
            configValues.Add("STRUCTURES_JSON", "FALSE");
            configValues.Add("OPERATIONS_XML", "TRUE");
            configValues.Add("OPERATIONS_JSON", "TRUE");
            configValues.Add("DMS_TITLE", "Kats Unicorn DMS");
            configValues.Add("DMS_VERSION", "Version 1.0");
            configValues.Add("SCHEMAS", "True");

            guide.CreateImplementationGuide(configValues);

            Assert.IsTrue(guide.OperationsInXml);
            Assert.IsTrue(guide.OperationsInJson);
        }

        [TestMethod]
        public void ImplementationGuide_Base_CreateImplementationGuide_OperationsPropertiesAreFalse()
        {
            var guide = new IG.Base(_directoryCreator, _context);
            var configValues = new Dictionary<string, string>();

            configValues.Add("DMS_ONLINE", "FALSE");
            configValues.Add("DMS_ANALYTICS", "UA-67795986-1");
            configValues.Add("EXAMPLES_XML", "FALSE");
            configValues.Add("EXAMPLES_JSON", "FALSE");
            configValues.Add("VALUESETS_XML", "FALSE");
            configValues.Add("VALUESETS_JSON", "FALSE");
            configValues.Add("STRUCTURES_XML", "FALSE");
            configValues.Add("STRUCTURES_JSON", "FALSE");
            configValues.Add("OPERATIONS_XML", "FALSE");
            configValues.Add("OPERATIONS_JSON", "FALSE");
            configValues.Add("DMS_TITLE", "Kats Unicorn DMS");
            configValues.Add("DMS_VERSION", "Version 1.0");
            configValues.Add("SCHEMAS", "True");

            guide.CreateImplementationGuide(configValues);

            Assert.IsFalse(guide.OperationsInXml);
            Assert.IsFalse(guide.OperationsInJson);
        }

        [TestMethod]
        public void ImplementationGuide_Base_Load_ExamplePropertiesUpdated()
        {
            var guide = new IG.Base(_directoryCreator, _context);
            guide.Load();

            Assert.IsTrue(guide.ExamplesXml);
            Assert.IsTrue(guide.ExamplesJson);
        }

        [TestMethod]
        public void ImplementationGuide_Base_Load_ValuesetsPropertiesUpdated()
        {
            var guide = new IG.Base(_directoryCreator, _context);
            guide.Load();

            Assert.IsTrue(guide.ValuesetsInXml);
            Assert.IsTrue(guide.ValuesetsInJson);
        }

        [TestMethod]
        public void ImplementationGuide_Base_Load_StructuresPropertiesUpdated()
        {
            var guide = new IG.Base(_directoryCreator, _context);
            guide.Load();

            Assert.IsTrue(guide.StructuresInXml);
            Assert.IsTrue(guide.StructuresInXml);
        }

        [TestMethod]
        public void ImplementationGuide_Base_Load_OperationsPropertiesUpdated()
        {
            var guide = new IG.Base(_directoryCreator, _context);
            guide.Load();

            Assert.IsTrue(guide.OperationsInXml);
            Assert.IsTrue(guide.OperationsInJson);
        }
    }
}