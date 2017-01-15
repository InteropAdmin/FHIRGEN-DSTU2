using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProfileExample = Hl7.Fhir.Publication.Specification.Profile.Example;
using Base = Hl7.Fhir.Publication.ImplementationGuide.Base;

namespace Fhir.Publication.Tests.Specification.Profile.Example
{
    [TestClass]
    public class Factory
    {
        private ImplementationGuide _implementationGuide;
        private Base _baseImplementationGuide;
        private Context _context;
        private ImplementationGuide.ResourceComponent _profileResource;
        private ImplementationGuide.ResourceComponent _exampleResourceOne;
        private ImplementationGuide.ResourceComponent _exampleResourceTwo;
        private ImplementationGuide.ResourceComponent _exampleResourceThree;
        private List<Coding> _metaData;
        private IDirectoryCreator _dirCreator;

        [TestMethod]
        [IntegrationTest]
        public void Example_ToHtml_ExamplesAreListedAlpabetically()
        {
            var resourceReference = new ResourceReference();
            resourceReference.Reference = "http://fhir.nhs.net/StructureDefinition/gpconnect-location-1-0";

            _profileResource = new ImplementationGuide.ResourceComponent();
            _profileResource.Name = "GPConnect Location";
            _profileResource.Purpose = ImplementationGuide.GuideResourcePurpose.Profile;
            _profileResource.Source = new FhirString("http://fhir.nhs.net/StructureDefinition/gpconnect-location-1-0");

            _exampleResourceOne = new ImplementationGuide.ResourceComponent();
            _exampleResourceOne.Name = "Location-1a";
            _exampleResourceOne.Purpose = ImplementationGuide.GuideResourcePurpose.Example;
            _exampleResourceOne.Source = new FhirString("http://fhir.nhs.net/StructureDefinition/gpconnect-location-1-0");
            _exampleResourceOne.ExampleFor = resourceReference;

            _exampleResourceTwo = new ImplementationGuide.ResourceComponent();
            _exampleResourceTwo.Name = "Location-1b";
            _exampleResourceTwo.Purpose = ImplementationGuide.GuideResourcePurpose.Example;
            _exampleResourceTwo.Source = new FhirString("http://fhir.nhs.net/StructureDefinition/gpconnect-location-1-0");
            _exampleResourceTwo.ExampleFor = resourceReference;

           _exampleResourceThree = new ImplementationGuide.ResourceComponent();
           _exampleResourceThree.Name = "Location-2";
           _exampleResourceThree.Purpose = ImplementationGuide.GuideResourcePurpose.Example;
           _exampleResourceThree.Source = new FhirString("http://fhir.nhs.net/StructureDefinition/gpconnect-location-1-0");
           _exampleResourceThree.ExampleFor = resourceReference;
            
            var package = new ImplementationGuide.PackageComponent();
            package.Name = "Profile.GetScheduleQueryResponse";
            package.Resource.Add(_exampleResourceThree);
            package.Resource.Add(_exampleResourceTwo);
            package.Resource.Add(_exampleResourceOne);

            _implementationGuide = new ImplementationGuide();
            _implementationGuide.Package.Add(package);

            string path = package.Name;

            _dirCreator = new Mock.DirectoryCreator();
            var root = new Root("sourceDir", "targetDir");
            _context = new Context(root);
            _baseImplementationGuide = new Base(_dirCreator, _context);
            _baseImplementationGuide.Load();

            var locationOne = new Coding();
            locationOne.System = "urn:hscic:examples";
            locationOne.Code = "Location-1a";
            locationOne.Display = "Location Example - Physiotherapy Clinic";

            var locationTwo = new Coding();
            locationTwo.System = "urn:hscic:examples";
            locationTwo.Code = "Location-1b";
            locationTwo.Display = "Location Example - Antenatal Clinic - Charles Street Surgery";

            var locationThree = new Coding();
            locationThree.System = "urn:hscic:examples";
            locationThree.Code = "Location-2";
            locationThree.Display = "Location Example - Vaccination Clinic";

            _metaData = new List<Coding>();
            _metaData.Add(locationThree);
            _metaData.Add(locationOne);
            _metaData.Add(locationTwo);

            XElement actual = ProfileExample.Factory.ToHtml(_implementationGuide, path, _baseImplementationGuide, _profileResource, _metaData);

            var reader = new StringReader(Resources.ExampleTabTableIsAlphabetical);
            XElement expected = XElement.Load(reader, LoadOptions.None);

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }
    }
}