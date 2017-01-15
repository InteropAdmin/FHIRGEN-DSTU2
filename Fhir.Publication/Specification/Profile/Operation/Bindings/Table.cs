using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.ImplementationGuide;

namespace Hl7.Fhir.Publication.Specification.Profile.Operation.Bindings
{
    internal class Table
    {
        private Log _log;
        private Formatter _formatter;

        public XElement ToHtml(
            OperationDefinition operationDefinition,
            ResourceStore resourceStore,
            Log log)
        {
            if (operationDefinition == null)
                throw new ArgumentNullException(
                    nameof(operationDefinition));

            if (resourceStore == null)
                throw new ArgumentNullException(
                    nameof(resourceStore));

            if (log == null)
                throw new ArgumentNullException(nameof(log));

            _log = log;

            _log.Info("Create Bindings table");

            if (operationDefinition.Parameter.Any(element => element.Binding != null && element.Max != "0"))
            {
                IEnumerable<OperationDefinition.ParameterComponent> bindings =
                   operationDefinition.Parameter.Where(
                       element =>
                        element.Binding != null && element.Max != "0");

                TableModel.Model table = GetTable(bindings, resourceStore);

                var factory = new HierarchicalTable.Factory(null);

                return table.Rows.Any() ? factory.CreateFrom(table).ToHtml() : null;
            }

            return null;
        }

        private TableModel.Model GetTable(IEnumerable<OperationDefinition.ParameterComponent> parameters, ResourceStore resourceStore)
        {
            TableModel.Model table = TableModel.Model.GetBindingsTable();

            foreach (OperationDefinition.ParameterComponent parameter in parameters)
            {
                _log.Info($" format binding for {parameter.Binding.ValueSet}");

                string package = resourceStore.Resources.First(
                    resource =>
                        resource.Url == parameter.Binding.ValueSet.ToString()).Package;

                _formatter = new Formatter(parameter.Name, package, parameter.Binding, resourceStore);

                table.Rows.Add(
                    GetCells(
                        parameter.Name,
                        parameter.Binding.Strength.ToString()));
            }

            return table;
        }

        private TableModel.Row GetCells(string path, string bindingStrength)
        {
            var row = new TableModel.Row();

            var pathCell = new TableModel.Cell(null, null, path, null, null);
            row.GetCells().Add(pathCell);

            var nameCell = new TableModel.Cell(null, null, _formatter.Name, null, null);
            row.GetCells().Add(nameCell);

            var bindingStrengthCell = new TableModel.Cell(null, _formatter.BindingStrength, bindingStrength, null, null);
            row.GetCells().Add(bindingStrengthCell);

            var referenceCell = new TableModel.Cell(null, _formatter.RelativeReference, _formatter.Url, null, null);
            row.GetCells().Add(referenceCell);

            return row;
        }
    }
}