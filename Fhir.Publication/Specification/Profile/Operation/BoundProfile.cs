using System;
using System.IO;
using System.Linq;
using Hl7.Fhir.Publication.ImplementationGuide;

namespace Hl7.Fhir.Publication.Specification.Profile.Operation
{
    internal class BoundProfile
    {
        private const string _profilePrefix = "Profile: ";
        private const string _displayPrefix = "Display: ";
        private const string _lineBreak = "br";
        private const string _boldFormat = "font-weight:bold";
        private readonly Fhir.Model.ResourceReference _boundProfile;
        private readonly TableModel.Cell _cell;
        private readonly ResourceStore _resourceStore;

        public BoundProfile(
            TableModel.Cell cell,
            Fhir.Model.ResourceReference boundProfile,
            ResourceStore resourceStore)
        {
            if (cell == null)
                throw new ArgumentNullException(
                    nameof(cell));

            if (boundProfile == null)
                throw new ArgumentNullException(
                    nameof(boundProfile));

            if (resourceStore == null)
                throw new ArgumentNullException(
                    nameof(resourceStore));

            _cell = cell;
            _boundProfile = boundProfile;
            _resourceStore = resourceStore;
        }

        public TableModel.Cell Value
        {
            get
            {
                if (_cell.GetPieces().Any())
                {
                    if (_boundProfile.Reference != null)
                        GetProfileReference();

                    if (_boundProfile.Display != null)
                        GetDisplay();

                    _cell.AddPiece(new TableModel.Piece(_lineBreak));
                }

                return _cell;
            }
        }

        private void GetProfileReference()
        {
            Validate();

            var packageName = _resourceStore.Resources.Single(
                res =>
                    res.Url == _boundProfile.Reference).Package;

            _cell.AddPiece(new TableModel.Piece(_lineBreak));
            _cell.AddPiece(new TableModel.Piece(_lineBreak));

            _cell.GetPieces()
                .Add(new TableModel.Piece(null, _profilePrefix, null)
                .AddStyle(_boldFormat));

            string reference = Path.Combine(@"..\", packageName, KnowledgeProvider.GetLinkForLocalResource(_boundProfile.Reference.Split('/').Last()));

            _cell.GetPieces()
                .Add(new TableModel.Piece(reference, _boundProfile.Reference, "operation references structure definition"));                  
        }

        private void Validate()
        {
            if (_resourceStore.Resources.All(
                resource =>
                    resource.Url != _boundProfile.Reference))
                            throw new InvalidOperationException($" {_boundProfile.Reference} does not exist in DMS!");

            if (_resourceStore.Resources.Count(
                res =>
                    res.Url == _boundProfile.Reference) > 1)
                throw new InvalidOperationException($" There is > 1 {_boundProfile.Reference} in this DMS!");
        }

        private void GetDisplay()
        {
            _cell.AddPiece(new TableModel.Piece(_lineBreak));

            _cell.GetPieces()
            .Add(new TableModel.Piece(null, _displayPrefix, null)
            .AddStyle(_boldFormat));

            _cell.GetPieces()
            .Add(new TableModel.Piece(null, _boundProfile.Display, null));
        }
    }
}