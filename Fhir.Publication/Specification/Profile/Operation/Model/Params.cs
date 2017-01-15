using System;
using System.Collections.Generic;
using System.Linq;
using Hl7.Fhir.Model;

namespace Hl7.Fhir.Publication.Specification.Profile.Operation.Model
{
    internal class Params
    {
        private readonly ImplementationGuide.ResourceStore _resourseStore;
        private readonly KnowledgeProvider _knowledgeProvider;

        public Params(
            IEnumerable<OperationDefinition.ParameterComponent> parameters,
            ImplementationGuide.ResourceStore resourseStore,
            KnowledgeProvider knowledgeProvider)
        {
            _resourseStore = resourseStore;
            _knowledgeProvider = knowledgeProvider;

            Table = TableModel.Model.GetOperationDefinitionTable();

            foreach (OperationDefinition.ParameterComponent parameter in parameters)
            {
                string cardinality = Cardinality.Describe(parameter.Min.ToString(), parameter.Max);

                Table.Rows.Add(GetCells(
                     parameter.Name,
                     cardinality,
                     parameter.Type,
                     parameter.Documentation,
                     parameter.Profile,
                     parameter.Binding));
            }
        }

        public TableModel.Model Table { get; }

        private TableModel.Row GetCells(
          string name,
          string cardinality,
          string type,
          string description,
          ResourceReference profile,
          OperationDefinition.BindingComponent binding)
        {
            var row = new TableModel.Row();

            var nameCell = new TableModel.Cell(null, null, name, null, null);
            row.GetCells().Add(nameCell);

            var cardinalityCell = new TableModel.Cell(null, null, cardinality, null, null);
            row.GetCells().Add(cardinalityCell);

            var typeCell = new TableModel.Cell(null, GetLink(type), type, null, null);
            row.GetCells().Add(typeCell);
                 
            row.GetCells().Add(GetDescription(description, profile, binding));

            return row;
        }

        private string GetLink(string type)
        {
            return _knowledgeProvider.GetLinkForTypeDocument((FHIRDefinedType)Enum.Parse(typeof(FHIRDefinedType), type, true));
        }

        private TableModel.Cell GetDescription(
            string description,
            ResourceReference profile,
             OperationDefinition.BindingComponent binding)
        {
            var descriptionCell = new TableModel.Cell(null, null, description, null, null);

            if (profile != null)
                descriptionCell = new BoundProfile(descriptionCell, profile, _resourseStore).Value;

            if (binding != null)
            {
                string package = _resourseStore.Resources.First(
                    resource => 
                        resource.Url == binding.ValueSet.ToString()).Package;

                descriptionCell = new BindingFormatter(descriptionCell, binding, _resourseStore, package).Value;
            }

            return descriptionCell;
        }
    }
}