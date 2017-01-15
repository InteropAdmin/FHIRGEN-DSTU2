using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;
using Hl7.Fhir.Publication.Specification.HierarchicalTable.Cells.Component;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Dictionary
{
    internal class TypeFormatter
    {
        private readonly ElementDefinition.TypeRefComponent _typeComponent;
        private readonly ImplementationGuide.ResourceStore _resources;
        private readonly StringBuilder _formattedType;
        private readonly KnowledgeProvider _knowledgeProvider;
        private readonly FHIRDefinedType _code;
        private readonly string _packageName;
        
        public TypeFormatter(
            ElementDefinition.TypeRefComponent typeComponent,
            ImplementationGuide.ResourceStore resources,
            KnowledgeProvider knowledgeProvider, 
            string packageName)
        {
            if (typeComponent == null)
                throw new ArgumentNullException(
                    nameof(typeComponent));

            if (resources == null)
                throw new ArgumentNullException(
                    nameof(resources));

            if (knowledgeProvider == null)
                throw new ArgumentNullException(
                    nameof(knowledgeProvider));

            if (string.IsNullOrEmpty(packageName))
                throw new ArgumentException(
                    nameof(packageName));

            _typeComponent = typeComponent;
            _resources = resources;
            _formattedType = new StringBuilder();
            _knowledgeProvider = knowledgeProvider;
            _packageName = packageName;

            if (_typeComponent.Code == null)
                throw new InvalidOperationException($" {_typeComponent} Code cannot be null!");

            _code = (FHIRDefinedType)_typeComponent.Code;
        }

        public string ToHtml()
        {
            if (_knowledgeProvider.HasLinkForTypeDocu(_code) && !KnowledgeProvider.IsReference(_code))
            {
                var reference = new Reference(null, _knowledgeProvider.GetLinkForTypeDocument(_code), null, _typeComponent.Code.ToString());
                return reference.ToHtml().ToString();
            }
            else
            {
                if (_typeComponent.Code != FHIRDefinedType.Reference)
                    _formattedType.Append(_typeComponent.Code);
            }
             
            List<FhirUri> profileReferences = _typeComponent.ProfileElement;

            if (profileReferences.Any())
            {
                string profileType = profileReferences.Single().ToString();

                if (profileType.StartsWith(Url.Hl7StructureDefintion.GetUrlString()))
                {
                    string typeName = profileType.Substring(Url.Hl7StructureDefintion.GetUrlString().Length);

                    FHIRDefinedType definedType;
                    if (Enum.TryParse(typeName, true, out definedType))
                    {
                        var reference = new Reference(null, _knowledgeProvider.GetLinkForTypeDocument(definedType), null, typeName);
                        _formattedType.Append(reference.ToHtml());
                    }
                    else
                    {
                        var reference = new Reference(null, profileType, null, typeName);
                        _formattedType.Append(reference.ToHtml());
                    }
                }
                else if(!profileType.StartsWith(Url.FhirStructureDefintion.GetUrlString()))
                {
                    var link = KnowledgeProvider.GetLinkForLocalResource(_resources.GetStructureDefinitionNameByUrl(profileType, _packageName));
                    var name = KnowledgeProvider.GetLabelForProfileReference(_typeComponent.Profile.First());

                    var reference = new Reference(null, link, null, name);
                    _formattedType.Append(reference.ToHtml());
                }
            }

            return _formattedType.ToString();
        }
    }
}