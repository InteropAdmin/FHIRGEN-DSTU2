using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hl7.Fhir.Publication.Specification.ExtensionMethods;
using Model = Hl7.Fhir.Model;

namespace Fhir.Publication.Tests.Specification.ExtensionMethods
{
    [TestClass]
    public class ElementDefinition
    {
        [TestMethod]
        public void ElementDefinition_AsFormattedString_SlicingRuleIsClosed()
        {
            Assert.AreEqual("Closed", Model.ElementDefinition.SlicingRules.Closed.AsFormattedString());
        }

        [TestMethod]
        public void ElementDefinition_AsFormattedString_SlicingRuleIsOpen()
        {
            Assert.AreEqual("Open", Model.ElementDefinition.SlicingRules.Open.AsFormattedString());
        }

        [TestMethod]
        public void ElementDefinition_AsFormattedString_SlicingRuleIsOpenAtEnd()
        {
            Assert.AreEqual("Open at End", Model.ElementDefinition.SlicingRules.OpenAtEnd.AsFormattedString());
        }

        [TestMethod]
        public void ElementDefinition_IsResourceReferenceIsTrue()
        {
            var firstType = new Model.ElementDefinition.TypeRefComponent();
            firstType.Code = Model.FHIRDefinedType.Reference;
            var secondType = new Model.ElementDefinition.TypeRefComponent();
            secondType.Code = Model.FHIRDefinedType.Reference;

            var types = new List<Model.ElementDefinition.TypeRefComponent>();
            types.Add(firstType);
            types.Add(secondType);

            var elementDefinition = new Model.ElementDefinition();
            elementDefinition.Type = types;
          
            Assert.IsTrue(elementDefinition.IsResourceReference());
        }

        [TestMethod]
        public void ElementDefinition_IsResourceReferenceIsFalse_HasMultipleTypesIsFalse()
        {
            var firstType = new Model.ElementDefinition.TypeRefComponent();
            firstType.Code = Model.FHIRDefinedType.Reference;

            var types = new List<Model.ElementDefinition.TypeRefComponent>();
            types.Add(firstType);

            var elementDefinition = new Model.ElementDefinition();
            elementDefinition.Type = types;

            Assert.IsFalse(elementDefinition.IsResourceReference());
        }

        [TestMethod]
        public void ElementDefinition_IsResourceReferenceIsFalse_HasDistinctTypesIsTrue()
        {
            var firstType = new Model.ElementDefinition.TypeRefComponent();
            firstType.Code = Model.FHIRDefinedType.Reference;
            var secondType = new Model.ElementDefinition.TypeRefComponent();
            secondType.Code = Model.FHIRDefinedType.String;

            var types = new List<Model.ElementDefinition.TypeRefComponent>();
            types.Add(firstType);
            types.Add(secondType);

            var elementDefinition = new Model.ElementDefinition();
            elementDefinition.Type = types;

            Assert.IsFalse(elementDefinition.IsResourceReference());
        }

        [TestMethod]
        public void ElementDefinition_IsResourceReferenceIsFalse_IsReferenceIsFalse()
        {
            var firstType = new Model.ElementDefinition.TypeRefComponent();
            firstType.Code = Model.FHIRDefinedType.String;
            var secondType = new Model.ElementDefinition.TypeRefComponent();
            secondType.Code = Model.FHIRDefinedType.String;

            var types = new List<Model.ElementDefinition.TypeRefComponent>();
            types.Add(firstType);
            types.Add(secondType);

            var elementDefinition = new Model.ElementDefinition();
            elementDefinition.Type = types;

            Assert.IsFalse(elementDefinition.IsResourceReference());
        }
    }
}