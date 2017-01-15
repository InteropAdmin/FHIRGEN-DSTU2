using System;
using System.IO;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.ImplementationGuide;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OperationBindings = Hl7.Fhir.Publication.Specification.Profile.Operation.Bindings;

namespace Fhir.Publication.Tests.Specification.Profile.Operation.Bindings
{
    [TestClass]
    public class Table
    {
        [TestMethod]
        [IntegrationTest]
        public void Table_ToHtml_OperationDefinitionBindingsTabTableCreated()
        {
            var log = new Log(new Mock.ErrorLogger());
            var resourceStore = new ResourceStore(log);
            resourceStore.Add(new PackageResource("PackageOne", "gpconnect-schedule-operation-1-0", "http://fhir.nhs.net/OperationDefinition/gpconnect-schedule-operation-1-0", new OperationDefinition()));
            var resourceReference = new ResourceReference();
            resourceReference.Url = new Uri("http://fhir.nhs.net/OperationDefinition/gpconnect-schedule-operation-1-0");

            var categoryParam = new OperationDefinition.ParameterComponent();
            var categoryBinding = new OperationDefinition.BindingComponent();
            categoryBinding.Strength = BindingStrength.Example;
            categoryBinding.ValueSet = resourceReference;
            categoryParam.Name = "category";
            categoryParam.Binding = categoryBinding;
            categoryParam.Max = "1";

            var substanceParam = new OperationDefinition.ParameterComponent();
            var substanceBinding = new OperationDefinition.BindingComponent();
            substanceBinding.Strength = BindingStrength.Required;
            substanceBinding.ValueSet = resourceReference;
            substanceParam.Name = "substance";
            substanceParam.Binding = substanceBinding;
            substanceParam.Max = "*";

            var operationDefintion = new OperationDefinition();
            operationDefintion.Parameter.Add(categoryParam);
            operationDefintion.Parameter.Add(substanceParam);

            XElement actual = new OperationBindings.Table().ToHtml(operationDefintion, resourceStore, log);

            var reader = new StringReader(Resources.OperationDefinitionBindingsTabTable);
            XElement expected = XElement.Load(reader, LoadOptions.None);

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }
    }
}