using System;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Type
{
    internal class ResourceCell : Cell
    {
        private readonly ImplementationGuide.Package _package;
        private readonly string _referenceName;
        private readonly string _name;
        private readonly string _text;
        private readonly FHIRDefinedType _code;
        private readonly KnowledgeProvider _knowledgeProvider;
        private TableModel.Cell _cell;

        public ResourceCell(
            string name,
            string referenceName,
            FHIRDefinedType code,
            ImplementationGuide.Package package,
            KnowledgeProvider knowledgeProvider)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException(
                    nameof(name));

            if (string.IsNullOrEmpty(referenceName))
                throw new ArgumentException(
                    nameof(referenceName));

            if (package == null)
                throw new ArgumentNullException(
                    nameof(package));

            if (knowledgeProvider == null)
                throw new ArgumentNullException(
                    nameof(knowledgeProvider));

            _referenceName = referenceName;
            _package = package;
            _name = name;
            _code = code;
            _knowledgeProvider = knowledgeProvider;

            _text = KnowledgeProvider.GetLabelForProfileReference(referenceName);

            if (IsLocalResource())
                CreateLocalResourceCell();
            else
                CreateGlobalResourceCell();
        }

        public override TableModel.Cell Value => _cell;

        private bool IsLocalResource()
        {
            if (_referenceName.StartsWith(Url.FhirStructureDefintion.GetUrlString())) return true;
            if (_referenceName.StartsWith(Url.FhirHL7UKStructureDefintion.GetUrlString())) return true;
            if (_referenceName.StartsWith(Url.FhirNHSUKStructureDefintion.GetUrlString())) return true;

            return false;
        }

        private void CreateLocalResourceCell()
        {
            string reference = KnowledgeProvider.GetLinkForLocalResource(_package.ResourceStore.GetStructureDefinitionNameByUrl(_referenceName, _package.Name));
            _cell = new TableModel.Cell(null, reference, _text, _code.ToString(), null);
        }

        private void CreateGlobalResourceCell()
        {
            string reference = _knowledgeProvider.GetLinkForProfileReference(_name, _referenceName);
            _cell = new TableModel.Cell(null, reference, _text, _code.ToString(), null);
        }
    }
}