using System;
using System.Linq;
using Hl7.Fhir.Model;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Description
{
    internal class BindingFormatter : Profile.Bindings.Formatter
    {
        private const string _lineBreak = "br";
        private const string _boldFormat = "font-weight:bold";
        private readonly ElementDefinition.BindingComponent _binding;
        private readonly TableModel.Cell _cell;

        public BindingFormatter(
           ElementDefinition.BindingComponent binding,
           ImplementationGuide.ResourceStore resourceStore,
           TableModel.Cell cell,
           string package)
           : base(binding.ValueSet.ToString(), package, binding.ValueSet)
        {
            if (binding == null)
                throw new ArgumentNullException(
                    nameof(binding));

            if (resourceStore == null)
                throw new ArgumentNullException(
                    nameof(resourceStore));

            if (cell == null)
                throw new ArgumentNullException(
                    nameof(cell));

            _binding = binding;
            ResourceStore = resourceStore;
            _cell = cell;

            if (string.IsNullOrEmpty(_binding.Description))
                _binding.Description = string.Empty;
//throw new InvalidOperationException($" {Binding.BindingUrl} : Binding description cannot be empty!");

            if (_binding.Strength == null)
                throw new InvalidOperationException($" {Binding.BindingUrl} : Binding strength cannot be empty!");

            if (!_cell.GetPieces().Any())
                throw new InvalidOperationException(" Cell contains no pieces!");
        }

        private string Description => _binding.Description;

        private new string Strength => _binding.Strength.ToString();

        public TableModel.Cell GetFormattedBinding()
        {
            _cell.AddPiece(new TableModel.Piece(_lineBreak));

            _cell.GetPieces()
                 .Add(new TableModel.Piece("pub-grey", "Binding"));

            _cell.GetPieces()
                 .Add(new TableModel.Piece(null, " ", null));

            _cell.GetPieces()
                 .Add(new TableModel.Piece(null,Description, null));

            _cell.GetPieces()
                .Add(new TableModel.Piece(Reference, string.Concat(" (", Url, ")"), null));

            _cell.AddPiece(new TableModel.Piece(_lineBreak));

            _cell.GetPieces()
             .Add(new TableModel.Piece("pub-grey", "Binding Strength"));

            _cell.GetPieces()
                 .Add(new TableModel.Piece(null, " ", null));

            _cell.GetPieces()
                .Add(new TableModel.Piece(null,Strength, null));

            return _cell;
        }
    }
}