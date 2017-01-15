using System;
using Hl7.Fhir.Publication.Specification.TableModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PubDescription = Hl7.Fhir.Publication.Specification.Profile.Structure.Description;

namespace Fhir.Publication.Tests.Specification.Profile.Structure.Description
{
    [TestClass]
    public class SliceCell
    {
        private PubDescription.SliceCell SlicedCell
        {
            get
            {
                var slicing = new Hl7.Fhir.Model.ElementDefinition.SlicingComponent();
                slicing.Rules = Hl7.Fhir.Model.ElementDefinition.SlicingRules.Closed;
                slicing.Ordered = false;
                var tableCell = new Cell();
                return new PubDescription.SliceCell(tableCell, slicing);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SliceCell_Value_InvalidoperationExceptionThrownWhenSlicingRulesAreNull()
        {
            var slicing = new Hl7.Fhir.Model.ElementDefinition.SlicingComponent();
            var tableCell = new Cell();
            var sliceCell = new PubDescription.SliceCell(tableCell, slicing);
            var actual = sliceCell.Value;
        }


        [TestMethod]
        public void SliceCell_Value_ValueContainsFormattedSlicePrefix()
        {
            Cell actual = SlicedCell.Value;

            Assert.IsTrue(actual.GetPieces().Count == 2);
            Assert.IsTrue(actual.GetPieces()[0].GetStyle() == "font-weight:bold");
            Assert.IsTrue(actual.GetPieces()[0].GetText() == "Slice: ");
        }

        [TestMethod]
        public void SliceCell_Value_ValueContainsClosedFormattedSliceDescription()
        {
            Cell actual = SlicedCell.Value;

            Assert.IsTrue(actual.GetPieces().Count == 2);
            Assert.IsTrue(actual.GetPieces()[1].GetStyle() == "font-style: italic");
        }

        [TestMethod]
        public void SliceCell_Value_ValueContainsClosedSlicingRules()
        {
            Cell actual = SlicedCell.Value;

            Assert.IsTrue(actual.GetPieces().Count == 2);
            Assert.IsTrue(actual.GetPieces()[1].GetStyle() == "font-style: italic");
            Assert.IsTrue(actual.GetPieces()[1].GetText() == "Ordering: Unordered, Rules: Closed");
        }

        [TestMethod]
        public void SliceCell_Value_ValueContainsClosedSlicingSingleDiscriminator()
        {
            var slicing = new Hl7.Fhir.Model.ElementDefinition.SlicingComponent();
            slicing.Rules = Hl7.Fhir.Model.ElementDefinition.SlicingRules.Closed;
            slicing.Discriminator = new[] { "type" };
            slicing.Ordered = false;
            var tableCell = new Cell();
            var sliceCell = new PubDescription.SliceCell(tableCell, slicing);

            Cell actual = sliceCell.Value;

            Assert.IsTrue(actual.GetPieces().Count == 2);
            Assert.IsTrue(actual.GetPieces()[1].GetText() == "Ordering: Unordered, Discriminator: type,  Rules: Closed");
        }

        [TestMethod]
        public void SliceCell_Value_ValueContainsClosedSlicingMultipleDiscriminators()
        {
            var slicing = new Hl7.Fhir.Model.ElementDefinition.SlicingComponent();
            slicing.Rules = Hl7.Fhir.Model.ElementDefinition.SlicingRules.Closed;
            slicing.Discriminator = new[] { "type", "code", "system" };
            slicing.Ordered = false;
            var tableCell = new Cell();
            var sliceCell = new PubDescription.SliceCell(tableCell, slicing);

            Cell actual = sliceCell.Value;

            Assert.IsTrue(actual.GetPieces().Count == 2);
            Assert.IsTrue(actual.GetPieces()[1].GetText() == "Ordering: Unordered, Discriminator: type,  code,  system,  Rules: Closed");
        }

        [TestMethod]
        public void SliceCell_Value_ValueContainsOrderedOrdering()
        {
            var slicing = new Hl7.Fhir.Model.ElementDefinition.SlicingComponent();
            slicing.Rules = Hl7.Fhir.Model.ElementDefinition.SlicingRules.Closed;
            slicing.Ordered = true;
            var tableCell = new Cell();
            var sliceCell = new PubDescription.SliceCell(tableCell, slicing);

            Cell actual = sliceCell.Value;

            Assert.IsTrue(actual.GetPieces().Count == 2);
            Assert.IsTrue(actual.GetPieces()[1].GetText() == "Ordering: Ordered, Rules: Closed");
        }

        [TestMethod]
        public void SliceCell_Value_ValueContainsUnOrderedOrdering()
        {
            var slicing = new Hl7.Fhir.Model.ElementDefinition.SlicingComponent();
            slicing.Rules = Hl7.Fhir.Model.ElementDefinition.SlicingRules.Closed;
            slicing.Ordered = false;
            var tableCell = new Cell();
            var sliceCell = new PubDescription.SliceCell(tableCell, slicing);

            Cell actual = sliceCell.Value;

            Assert.IsTrue(actual.GetPieces().Count == 2);
            Assert.IsTrue(actual.GetPieces()[1].GetText() == "Ordering: Unordered, Rules: Closed");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SliceCell_Value_InvalidOperationExceptionThrownWhenOrderedIsNull()
        {
            var slicing = new Hl7.Fhir.Model.ElementDefinition.SlicingComponent();
            slicing.Rules = Hl7.Fhir.Model.ElementDefinition.SlicingRules.Closed;
            slicing.Ordered = null;
            var tableCell = new Cell();
            var sliceCell = new PubDescription.SliceCell(tableCell, slicing);

            Cell actual = sliceCell.Value;
        }
    }
}
