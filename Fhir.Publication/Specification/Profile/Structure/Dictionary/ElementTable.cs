using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hl7.Fhir.Model;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Dictionary
{
    internal class ElementTable
    {
        private const int _first = 0;
        private readonly StringBuilder _xhtml;
        private readonly ImplementationGuide.ResourceStore _resources;
        private readonly ElementDefinition _elementDefinition;
        private readonly KnowledgeProvider _knowledgeProvider;
        private readonly string _resourceName;
        private readonly string _packageName;

        public ElementTable(
            ImplementationGuide.ResourceStore resources,
            ElementDefinition elementDefinition, 
            string resourceName,
            string packageName, 
            KnowledgeProvider knowledgeProvider)
        {
            if (resources == null)
                throw new ArgumentNullException(
                    nameof(resources));

            if (elementDefinition == null)
                throw new ArgumentNullException(
                    nameof(elementDefinition));

            if (string.IsNullOrEmpty(resourceName))
                throw new ArgumentException(
                    nameof(resourceName));

            if (knowledgeProvider == null)
                throw new ArgumentNullException(
                    nameof(knowledgeProvider));

            if (string.IsNullOrEmpty(packageName))
                throw new ArgumentException(
                    nameof(packageName));

            _xhtml = new StringBuilder();
            _resources = resources;
            _elementDefinition = elementDefinition;
            _resourceName = resourceName;
            _knowledgeProvider = knowledgeProvider;
            _packageName = packageName;
        }

        private string DefinitionRow => MarkDownTransformer.GenerateTableRow("Definition", _elementDefinition.Definition, _resourceName, _knowledgeProvider);

        private string ControlRow => new EncodedRow("Control","conformance-rules.html#conformance",string.Concat(Profile.Cardinality.Describe(_elementDefinition.Min.ToString(), _elementDefinition.Max), SummariseConditions(_elementDefinition.Condition))).Value;

        private string BindingRow => new Row("Binding", "terminologies.html", DescribeBinding(_elementDefinition, _resources)).Value;

        private string TypeRow => GetTypeRow();

        private string IsModifierRow => new EncodedRow("Is Modifier", "conformance-rules.html#ismodifier", DisplayBoolean(_elementDefinition.IsModifier)).Value;

        private string SupportRow => new EncodedRow("Must Support", "conformance-rules.html#mustSupport", DisplayBoolean(_elementDefinition.MustSupport)).Value;

        private string RequirementsRow => MarkDownTransformer.GenerateTableRow("Requirements", _elementDefinition.Requirements, _resourceName, _knowledgeProvider);

        private string AliasesRow => new EncodedRow(
                    "Aliases",
                    null,
                    _elementDefinition.Alias != null
                        ? string.Join(", ", _elementDefinition.Alias)
                        : null).Value;

        private string CommentsRow => MarkDownTransformer.GenerateTableRow("Comments", _elementDefinition.Comments, _resourceName, _knowledgeProvider);

        private string MaxLengthRow => new EncodedRow("Max Length", null, _elementDefinition.MaxLength?.ToString()).Value;

        private string FixedValueRow => new EncodedRow("Fixed Value", null, FixedValue).Value;

        private string FixedValue => _elementDefinition.Fixed?.ToString();

        private string DefaultValueRow => new EncodedRow("Default Value", null, _elementDefinition.DefaultValue?.ToString()).Value;

        private string ExampleValueRow => new EncodedRow("Example", null, _elementDefinition.Example?.ToString()).Value;

        public string Generate()
        {
            _xhtml.Append(DefinitionRow);
            _xhtml.Append(ControlRow);
            _xhtml.Append(BindingRow);
            _xhtml.Append(TypeRow);
            _xhtml.Append(IsModifierRow);
            _xhtml.Append(SupportRow);
            
            if (!string.IsNullOrEmpty(_elementDefinition.Requirements))
                _xhtml.Append(RequirementsRow);
            
            _xhtml.Append(AliasesRow);
            _xhtml.Append(CommentsRow);
            _xhtml.Append(MaxLengthRow);
            _xhtml.Append(FixedValueRow);
            _xhtml.Append(DefaultValueRow);

            if (string.IsNullOrEmpty(FixedValue))
                _xhtml.Append(ExampleValueRow);
            
            return _xhtml.ToString();
        }

        private string GetTypeRow()
        {
            return _elementDefinition.NameReference != null
                   ? new EncodedRow(
                       "Type",
                       null,
                       string.Concat(
                           "See ",
                           _elementDefinition.NameReference)).Value
                   : new Row(
                       "Type",
                       "datatypes.html",
                       GetType(_elementDefinition.Type)).Value;
        }

        private string DescribeBinding(ElementDefinition definition, ImplementationGuide.ResourceStore resourceStore)
        {
            if (definition.Binding == null) return string.Empty;

            if (definition.Binding.ValueSet == null) return "None Specified";
            
            return 
                definition.Binding != null 
                ? new BindingFormatter(definition.Binding, resourceStore, _packageName).Describe() 
                : string.Empty;
        }

        private string GetType(IReadOnlyList<ElementDefinition.TypeRefComponent> types)
        {
            if (types == null || !types.Any())
                return null;

            return types.Count == 1 ? DescribeSingleType(types) : DescribeMultipleTypes(types);
        }

        private string DescribeSingleType(IReadOnlyList<ElementDefinition.TypeRefComponent> types)
        {
            return new TypeFormatter(types[_first], _resources, _knowledgeProvider, _packageName).ToHtml();
        }

        private string DescribeMultipleTypes(IEnumerable<ElementDefinition.TypeRefComponent> types)
        {
            var response = new StringBuilder();
            response.Append("Choice of: <ul>");

            foreach (ElementDefinition.TypeRefComponent item in types)
            {
                response.Append(string.Concat("<li>", new TypeFormatter(item, _resources, _knowledgeProvider, _packageName).ToHtml(), "</li>"));
            }

            response.Append("</ul>");

            return response.ToString();
        }
        
        private static string SummariseConditions(IEnumerable<string> conditions)
        {
            // TODO: Not implemented yet
            if (conditions == null || !conditions.Any())
                return string.Empty;
            else
                return " ?";
        }

        private static string DisplayBoolean(bool? value)
        {
            if (value.HasValue && value == true)
                return bool.TrueString;
            else
                return null;
        }
    }
}