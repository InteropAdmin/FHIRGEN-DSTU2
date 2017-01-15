using System;
using Hl7.Fhir.Model;

namespace Hl7.Fhir.Publication.Specification.Profile.Operation.Model
{
    internal class Meta
    {
        private readonly KnowledgeProvider _knowledgeProvider;

        public Meta(OperationDefinition operationDefinition, KnowledgeProvider knowledgeProvider)
        {
            _knowledgeProvider = knowledgeProvider;

            Validator.Validate(operationDefinition);

            Table = TableModel.Model.GetOperationDefinitionMetaTable();

            Table.Rows.Add(GetCells("Name", FHIRDefinedType.String.ToString(), operationDefinition.Name));
            Table.Rows.Add(GetCells("Kind", operationDefinition.Kind?.ToString() , operationDefinition.Kind?.ToString()));
            Table.Rows.Add(GetCells("Description", FHIRDefinedType.String.ToString(), operationDefinition.Description));

            if(!string.IsNullOrEmpty(operationDefinition.Requirements))
                Table.Rows.Add(GetCells("Requirements", FHIRDefinedType.String.ToString(), operationDefinition.Requirements));

            Table.Rows.Add(GetCells("Code", FHIRDefinedType.Code.ToString(), operationDefinition.Code));
            Table.Rows.Add(GetCells("System", FHIRDefinedType.Boolean.ToString(), operationDefinition.System?.ToString()));
            Table.Rows.Add(GetCells("Instance", FHIRDefinedType.Boolean.ToString(), operationDefinition.Instance?.ToString()));
        }

        public TableModel.Model Table { get; }
        
        private TableModel.Row GetCells(string name, string type, string value)
        {
            var row = new TableModel.Row();

            var nameCell = new TableModel.Cell(null, null, name, null, null);
            row.GetCells().Add(nameCell);

            var typeCell = new TableModel.Cell(null, GetLink(type), type, null, null);
            row.GetCells().Add(typeCell);

            var valueCell = new TableModel.Cell(null, null, value, null, null);
            row.GetCells().Add(valueCell);

            return row;
        }

        private string GetLink(string type)
        {
            if (Enum.IsDefined(typeof(FHIRDefinedType), type))
                return _knowledgeProvider.GetLinkForTypeDocument((FHIRDefinedType)Enum.Parse(typeof(FHIRDefinedType), type, true));
            if (type == "Operation" || type == "Query")
                return KnowledgeProvider.OperationDefintionKindPath;
            else
                throw new InvalidOperationException(" Type is not recognised!");
        }
    }
}