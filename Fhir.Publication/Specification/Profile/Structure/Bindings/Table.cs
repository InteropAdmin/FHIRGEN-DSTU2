using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.ImplementationGuide;
using StructureDefinition = Hl7.Fhir.Model.StructureDefinition;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Bindings
{
    internal class Table 
    {
        private Formatter _formatter;
        private Log _log;
        private string _package;

        public XElement ToHtml(StructureDefinition structureDefinition, ResourceStore resourceStore, Log log, string package)
        {
            if (log == null)
                throw new ArgumentNullException(
                    nameof(log));

            if (string.IsNullOrEmpty(package))
                throw new ArgumentException(
                    nameof(package));

            _log = log;
            _package = package;

            _log.Info(" create Bindings table");

            IEnumerable<ElementDefinition> elementBindings = structureDefinition.Snapshot.Element
                .Where(
                   element =>
                       element.Binding != null
                       && element.Max != "0")
                 .ToArray();

            if (elementBindings.Any())
            {
                TableModel.Model table = GetTable(elementBindings, resourceStore);

                var factory = new HierarchicalTable.Factory(null);

                return table.Rows.Any() ? factory.CreateFrom(table).ToHtml() : null;
            }

            return null;
        }

        private TableModel.Model GetTable(IEnumerable<ElementDefinition> elementBindings, ResourceStore resourceStore)
        {
            TableModel.Model table = TableModel.Model.GetBindingsTable();

            ElementDefinition[] elementDefinitionsArray = elementBindings.ToArray();

            foreach (ElementDefinition element in elementDefinitionsArray)
            {
                element.Name = HasName(element) ? element.Name : element.Path.Split('.').Last();
            }

            foreach (ElementDefinition element in elementDefinitionsArray.Distinct(new ElementDefinitionComparer()))
            {
                _log.Info($" format binding for {element.Path}");

                _formatter = new Formatter(element.Name, _package, element.Binding, resourceStore);
                   
                table.Rows.Add(
                    GetCells(
                        element.Path,
                        element.Binding.Strength.ToString()));
            }

            return table;
        }

        private bool HasName(ElementDefinition element)
        {
            bool hasName = !string.IsNullOrEmpty(element.Name);

            if (!hasName)
                _log.Warning($"*** element Name for {element.Path} has not been populated!");

            return hasName;
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

            var referenceCell = new TableModel.Cell(null, _formatter.Reference, _formatter.Url, null, null);
            row.GetCells().Add(referenceCell);

            return row;
        }
    }
}