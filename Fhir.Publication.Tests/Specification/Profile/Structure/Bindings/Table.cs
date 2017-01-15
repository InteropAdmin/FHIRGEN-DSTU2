using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.ImplementationGuide;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureBindings = Hl7.Fhir.Publication.Specification.Profile.Structure.Bindings;

namespace Fhir.Publication.Tests.Specification.Profile.Structure.Bindings
{
    [TestClass]
    public class Table
    {
        [TestMethod]
        [IntegrationTest]
        public void Table_ToHtml_StructureDefinitionBindingsTabTableCreated()
        {
            var log = new Log(new Mock.ErrorLogger());
            var resourceStore = new ResourceStore(log);
            var resourceReference = new ResourceReference();
            resourceReference.Url = new Uri("http://fhir.nhs.net/OperationDefinition/gpconnect-schedule-operation-1-0");
            
            var categoryElement = new ElementDefinition();
            categoryElement.Name = "category";
            categoryElement.Path = "OperationOutcome.issue.category";
            var binding = new ElementDefinition.BindingComponent();
            categoryElement.Binding = binding;
            categoryElement.Binding.ValueSet = resourceReference;
            categoryElement.Binding.Strength = BindingStrength.Example;
            categoryElement.Max = "*";
            
            var substanceElement = new ElementDefinition();
            substanceElement.Name = "substance";
            substanceElement.Path = "OperationOutcome.issue.substance";
            var substanceBinding = new ElementDefinition.BindingComponent();
            substanceElement.Binding = substanceBinding;
            substanceElement.Binding.ValueSet = resourceReference;
            substanceElement.Binding.Strength = BindingStrength.Required;
            substanceElement.Max = "1";

            var elements = new List<ElementDefinition>();
            elements.Add(categoryElement);
            elements.Add(substanceElement);

            var differential = new StructureDefinition.DifferentialComponent();
            differential.Element = elements;
            var structureDefinition = new StructureDefinition();
            structureDefinition.Differential = differential;

            XElement actual = new StructureBindings.Table().ToHtml(structureDefinition, resourceStore, log, "ProfileOne");

            var reader = new StringReader(Resources.StructureDefinitionBindingsTabTable);
            XElement expected = XElement.Load(reader, LoadOptions.None);

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }
    }
}