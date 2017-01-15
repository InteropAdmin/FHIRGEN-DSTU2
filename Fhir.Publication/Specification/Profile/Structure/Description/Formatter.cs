using Hl7.Fhir.Model;
using System;
using System.Linq;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Description
{
    internal class Formatter
    {
        private const string _lineBreak = "br";
        private const string _extension = "extension";
        private const string _modifierExtension = "modifierExtension";

        private readonly Model.ElementDefinition _element;
        private readonly Model.ElementDefinition _fallback;
        private readonly ImplementationGuide.ResourceStore _resourceStore;
        private readonly string _package;

        public Formatter(
            Model.ElementDefinition element,
            Model.ElementDefinition fallback,
            ImplementationGuide.ResourceStore resourceStore,
            string package)
        {
            if (element == null)
                throw new ArgumentNullException(
                    nameof(element));

            if (resourceStore == null)
                throw new ArgumentNullException(
                    nameof(resourceStore));

            if (string.IsNullOrEmpty(package))
                throw new ArgumentException(
                    nameof(package));

            _element = element;
            _fallback = fallback;
            _resourceStore = resourceStore;
            _package = package;

            Cell = new TableModel.Cell();

            AddShortDescription();
            AddSlice();

            if (HasDefinition && !IsMetaData)
                AddDefinitionValues();

            if (Cell.GetPieces().Any())
            {
                Cell.AddPiece(new TableModel.Piece(_lineBreak));
                Cell.AddPiece(new TableModel.Piece(_lineBreak));
            }
        }

        public TableModel.Cell Cell { get; private set; }

        private bool HasDefinition => _element.Definition != null;

        private bool IsMetaData => _element.Path.Contains(".meta.tag");

        private static bool IsExtension(string elementName)
        {
            var pos = elementName.LastIndexOf(".");

            var tt = pos != -1 ? elementName.Substring(pos + 1) : elementName;

            return tt == _extension || elementName == _modifierExtension;
        }

        private bool HasExample => (_element.Example != null) && (_element.Fixed == null) && !string.IsNullOrEmpty(_element.Example.ToString());

        private void AddShortDescription()
        {
            if (_element.Definition != null && _element.Short != null)
            {
                AddShortDescriptionToCell(_element.Short);
            }
            else if (_fallback?.Short != null)
            {
                AddShortDescriptionToCell(_fallback.Short);
            }
            else
            {
                AddShortDescriptionToCell(" ");
                //TODO - This should be reported but there is no access to the log here.
            }
        }

        private void AddSlice()
        {
            if (_element.Slicing != null)
            {
                var sliceCell = new SliceCell(Cell, _element.Slicing);
                Cell = sliceCell.Value;
            }
        }

        private void AddShortDescriptionToCell(string shortDefinition)
        {
            if (Cell.GetPieces().Any())
                Cell.AddPiece(new TableModel.Piece(_lineBreak));

            Cell.AddPiece(new TableModel.Piece(null, shortDefinition, null));
        }

        private void AddDefinitionValues()
        {

            if (IsExtension(_element.Path))
            {
                //var examplecell = new Definition(Cell, "Extension", "Boom!!");

                //Cell = examplecell.Value;
            }

            if (_element.Binding != null)
            {
                if (_element.Binding.ValueSet == null)
                {
                    _element.Binding.ValueSet = new FhirString("kk");
                }

                var binding = new BindingFormatter(_element.Binding, _resourceStore, Cell, _package);
                Cell = binding.GetFormattedBinding();
            }
            if (_element.Fixed != null)
            {
                var fixedValue = new Definition(Cell, "Fixed Value", _element.Fixed.ToString());
                Cell = fixedValue.Value;
            }

            if (_element.DefaultValue != null)
            {
                var defaultValue = new Definition(Cell, "Default Value", _element.DefaultValue.ToString());
                Cell = defaultValue.Value;
            }

            if (HasExample)
            {
                var examplecell = new Definition(Cell, "Example Value", _element.Example.ToString());
                Cell = examplecell.Value;
            }
        }
    }
}