using System;
using System.IO;
using System.Linq;

namespace Hl7.Fhir.Publication.Specification.Profile.Operation
{
    internal class BindingFormatter : Profile.BindingFormatter
    {
        private const string _lineBreak = "br";
        private const string _boldFormat = "font-weight:bold";
        private readonly TableModel.Cell _cell;
        private readonly Fhir.Model.OperationDefinition.BindingComponent _binding;
        private readonly ImplementationGuide.ResourceStore _resourceStore;
        private readonly string _packageName;

        public BindingFormatter(
            TableModel.Cell cell,
            Fhir.Model.OperationDefinition.BindingComponent binding,
            ImplementationGuide.ResourceStore resourceStore,
            string packageName)
            : base(binding.ValueSet)
        {
            if (cell == null)
                throw new ArgumentNullException(
                    nameof(cell));

            if (binding == null)
                throw new ArgumentNullException(
                    nameof(binding));

            if (resourceStore == null)
                throw new ArgumentNullException(
                    nameof(resourceStore));

            if (string.IsNullOrEmpty(packageName))
                throw new ArgumentException(
                    nameof(packageName));

            _cell = cell;
            _binding = binding;
            _resourceStore = resourceStore;
            _packageName = packageName;
        }

        public TableModel.Cell Value
        {
            get
            {
                if (_cell.GetPieces().Any())
                {
                    GetBinding(Binding.BindingUrl);

                    GetStrength();
                }

                return _cell;
            }
        }

        private string RelativeReference => Path.Combine(@"..\", _packageName, Binding.GetLink(_resourceStore, _packageName));

        private void GetBinding(string itemToBind)
        {
            _cell.AddPiece(new TableModel.Piece(_lineBreak));

            _cell.GetPieces()
                .Add(new TableModel.Piece("pub-grey", "Binding"));

            _cell.GetPieces()
                .Add(new TableModel.Piece(RelativeReference, itemToBind, null));

            _cell.AddPiece(new TableModel.Piece(_lineBreak));
        }

        private void GetStrength()
        {
            _cell.GetPieces()
               .Add(new TableModel.Piece("pub-grey", "Binding Strength"));

            _cell.GetPieces()
                .Add(new TableModel.Piece(null, " ", null));

            _cell.GetPieces()
                .Add(new TableModel.Piece(null, _binding.Strength.ToString(), null));

            _cell.AddPiece(new TableModel.Piece(_lineBreak));
            _cell.AddPiece(new TableModel.Piece(_lineBreak));
        }
    }
}