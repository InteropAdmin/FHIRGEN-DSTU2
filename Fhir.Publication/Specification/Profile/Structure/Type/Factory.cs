using System;
using System.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Specification.ExtensionMethods;
using ElementDefinition = Hl7.Fhir.Model.ElementDefinition;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Type
{
    internal class Factory
    {
        private const string _modifierExtension = "modifierExtension";
        private readonly ElementDefinition _elementDefinition;
        private readonly ImplementationGuide.Package _package;
        private readonly TableModel.Cell _cell;
        private readonly KnowledgeProvider _knowledgeProvider;
        private readonly string _resourceName;

        public Factory(
            ElementDefinition elementDefinition,
            string resourceName,
            ImplementationGuide.Package package,
            KnowledgeProvider knowledgeProvider)
        {
            if (elementDefinition == null)
                throw new ArgumentNullException(
                    nameof(elementDefinition));

            if (string.IsNullOrEmpty(resourceName))
                throw new ArgumentException(
                    nameof(resourceName));

            if (package == null)
                throw new ArgumentNullException(
                    nameof(package));

            if (knowledgeProvider == null)
                throw new ArgumentNullException(
                    nameof(knowledgeProvider));

            _elementDefinition = elementDefinition;
            _resourceName = resourceName;
            _package = package;
            _knowledgeProvider = knowledgeProvider;
            _cell = new TableModel.Cell();
        }

        public TableModel.Cell GetCells()
        {
            if (_elementDefinition.Type == null)
                return _cell;

            GetTypeCell();

            return _cell;
        }

        private static bool IsExtension(string path)
        {
            string extension = path.Split('.').Last();

            return
                extension == FHIRDefinedType.Extension.ToString().ToLower()
                || extension == _modifierExtension;
        }

        private void GetTypeCell()
        {
            if (_elementDefinition.IsResourceReference())
            {
                FormatResourceReferenceCell();
            }
            else
            {
                for (int i = 0; i < _elementDefinition.Type.Count; i++)
                {
                    if (i != 0)
                        _cell.AddPiece(new TableModel.Piece(null, " | ", null));

                    _cell.GetPieces()
                        .AddRange(
                            GetCell(_elementDefinition.Type[i], _resourceName, _elementDefinition.Type[i].ProfileElement.Count)
                                .Value
                                .GetPieces());
                }
            }   
        }
        
        private void FormatResourceReferenceCell()
        {
            _cell.AddPiece(
                  new TableModel.Piece(
                      KnowledgeProvider.ReferenceResourcePath,
                      FHIRDefinedType.Reference.ToString(),
                      null));

            _cell.AddPiece(
                  new TableModel.Piece(
                      null,
                      " (",
                      null));

            for (int i = 0; i < _elementDefinition.Type.Count; i++)
            {
                if (i != 0)
                    _cell.AddPiece(new TableModel.Piece(null, " | ", null));

                _cell.GetPieces()
                    .AddRange(
                        GetCell(_elementDefinition.Type[i], _resourceName, _elementDefinition.Type[i].ProfileElement.Count)
                            .Value
                            .GetPieces());
            }
  
            _cell.AddPiece(
                new TableModel.Piece(
                    null,
                    ")",
                    null));
        }

        private Cell GetCell(
            ElementDefinition.TypeRefComponent typeRefComponent, 
            string resourceName,
            int totalProfileElements)
        {
            if (typeRefComponent.Code == null)
                throw new InvalidOperationException(string.Concat(typeRefComponent.TypeName, " cannot be null!"));

            var code = (FHIRDefinedType)typeRefComponent.Code;

            if (IsResourceReference(typeRefComponent, totalProfileElements))
            {
                return GetReferenceCell(typeRefComponent, typeRefComponent.Profile.Single(), code);
            }
            else if (totalProfileElements > 0)
            {
                return new ResourceCell(resourceName, typeRefComponent.Profile.Single(), code, _package, _knowledgeProvider);
            }
            else if (_knowledgeProvider.HasLinkForTypeDocu(code))
            {
                return new DataTypeCell(code, _knowledgeProvider);
            }
            else
                return new NoHyperLinkCell(code.ToString());
        }

        private static bool IsResourceReference(ElementDefinition.TypeRefComponent typeRefComponent, int totalProfileElements)
        {
            return 
                (IsReference(typeRefComponent) && totalProfileElements > 0) 
                || 
                (IsCodeableConcept(typeRefComponent) && totalProfileElements > 0);
        }

        private static bool IsReference(ElementDefinition.TypeRefComponent typeRefComponent)
        {
            return typeRefComponent.Code == FHIRDefinedType.Reference;
        }

        private static bool IsCodeableConcept(ElementDefinition.TypeRefComponent typeRefComponent)
        {
            return typeRefComponent.Code == FHIRDefinedType.CodeableConcept;
        }

        private ReferenceCell GetReferenceCell(ElementDefinition.TypeRefComponent typeRefComponent, string url, FHIRDefinedType code)
        {
           if (IsCodeableConcept(typeRefComponent))
                return new CodeableConcept(_package, url, _knowledgeProvider, code);
            if (IsReference(typeRefComponent))
                return new ResourceReference(_package, url, _knowledgeProvider);
            else
                throw new InvalidOperationException("reference is neither resource reference nor codeableconcept!");
        }
    }
}