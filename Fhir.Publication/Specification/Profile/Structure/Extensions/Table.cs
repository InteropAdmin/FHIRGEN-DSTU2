using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;
using Hl7.Fhir.Publication.ImplementationGuide;
using StructureDefinition = Hl7.Fhir.Model.StructureDefinition;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Extensions
{
    internal class Table
    {
        private List<StructureDefinition> _extensions;
        private List<StructureDefinition> _modifierExtensions;
        private TableFactory _factory;
        private XElement _table;
        private string _package;

        public XElement ToHtml(
            Log log, 
            IDirectoryCreator directoryCreator, 
            StructureDefinition structureDefinition, 
            ResourceStore resources,
            string packageName)
        {
            _package = packageName;

            _table = null;

            _factory = new TableFactory(new KnowledgeProvider(log));

            if (structureDefinition.HasExtensions())
            {
                _extensions = new List<StructureDefinition>();
                _modifierExtensions = new List<StructureDefinition>();

                GetResources(structureDefinition, resources);

                CreateTable(_package, directoryCreator);
            }

            return _table;
        }

        private void GetResources(StructureDefinition structureDefinition, ResourceStore resources)
        {
            foreach (var name in structureDefinition.GetExtensions())
            {
                StructureDefinition resource = resources.GetStructureDefinitionByUrl(name, _package);

                if (resource == null)
                    throw new InvalidOperationException($" referenced resource {name} is not found!");

                if (resource.IsExtension() && !resource.IsModifierExtension())
                    _extensions.Add(resource);
                else if (resource.IsModifierExtension())
                    _modifierExtensions.Add(resource);
            }
        }

        private void CreateTable(string packageName, IDirectoryCreator directoryCreator)
        {
            if (_extensions.Any())
                _table = _factory.GenerateProfile(_extensions, packageName, Icon.Extension, directoryCreator);

            if (_modifierExtensions.Any())
            {
                if (_table == null)
                    _table = _factory.GenerateProfile(_modifierExtensions, packageName, Icon.ModifierExtension, directoryCreator);
                else
                    _table.Add(_factory.GenerateProfile(_modifierExtensions, packageName, Icon.ModifierExtension, directoryCreator));
            }
        }
    }
}