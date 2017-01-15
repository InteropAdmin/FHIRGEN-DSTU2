using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hl7.Fhir.Model;
using PubDescription = Hl7.Fhir.Publication.Specification.Profile.Structure.Description;
using PubSpec = Hl7.Fhir.Publication.Specification;

namespace Fhir.Publication.Tests.Specification.Profile.Structure.Description
{
    [TestClass]
    public class Definition
    {
        private readonly ElementDefinition _element;
        private readonly PubSpec.TableModel.Cell _cell;

        public Definition()
        {
            _element = new ElementDefinition();
            _element.Binding = new ElementDefinition.BindingComponent();
            _element.Fixed = new FhirUri(new Uri("http://www.Badger.com", UriKind.Absolute));
            _cell = new PubSpec.TableModel.Cell();
        }

        [TestMethod]
        public void Cell_Value_EightPiecesAddedToCell()
        {
            var fixedValue = new PubDescription.Definition(_cell, "Fixed Value", _element.Fixed.ToString());

            var pieceCount = fixedValue.Value.GetPieces().Count;
            Assert.IsTrue(fixedValue.Value.GetPieces().Count == 4);
        }

        [TestMethod]
        public void Cell_Value_CellLabelHasBoldFormatStyle()
        {
            var fixedValue = new PubDescription.Definition(_cell, "Fixed Value", _element.Fixed.ToString());
  
            Assert.AreSame("Fixed Value", fixedValue.Value.GetPieces().ElementAt(1).GetText());
            Assert.AreSame("font-weight:bold", fixedValue.Value.GetPieces().ElementAt(1).GetStyle());
        }

        [TestMethod]
        public void Cell_Value_CellContainsElementValue()
        {
            var fixedValue = new PubDescription.Definition(_cell, "Fixed Value", _element.Fixed.ToString());

            Assert.AreSame("http://www.Badger.com", fixedValue.Value.GetPieces().ElementAt(3).GetText());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Cell_ArgumentNullExceptionThrownWhenCellISNull()
        {
            var fixedValue = new PubDescription.Definition(null, "Fixed Value", _element.Fixed.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Cell_ArgumentExceptionThrownWhenLableIsNull()
        {
            var fixedValue = new PubDescription.Definition(_cell, null, _element.Fixed.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Cell_ArgumentExceptionThrownWhenLableIsEmpty()
        {
            var fixedValue = new PubDescription.Definition(_cell, string.Empty, _element.Fixed.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Cell_ArgumentExceptionThrownWhenValueIsNull()
        {
            var fixedValue = new PubDescription.Definition(_cell, "Fixed Value: ", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Cell_ArgumentExceptionThrownWhenValueIsEmpty()
        {
            var fixedValue = new PubDescription.Definition(_cell, "Fixed Value: ", string.Empty);
        }
    }
}
