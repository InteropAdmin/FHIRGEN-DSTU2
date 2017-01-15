using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;
using Hl7.Fhir.Publication.Specification.ExtensionMethods;
using StructureDefinition = Hl7.Fhir.Model.StructureDefinition;

namespace Hl7.Fhir.Publication.ImplementationGuide
{
    internal class Package
    {
        private readonly Context _context;
        private readonly Log _log;
        private readonly IDirectoryCreator _directoryCreator;
        private IEnumerable<Model.ImplementationGuide.ResourceComponent> _resources;

        internal Package(
            string name,
            Context context, 
            Log log, 
            IDirectoryCreator directoryCreator)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException(
                    nameof(name));

            if (context == null)
                throw new ArgumentNullException(
                    nameof(context));

            if (log == null)
                throw new ArgumentNullException(
                    nameof(log));

            if (directoryCreator == null)
                throw new ArgumentNullException(
                    nameof(log));

            Name = name;
            _context = context;
            _log = log;
            ResourceStore = new ResourceStore(log);
            _directoryCreator = directoryCreator;
        }

        public ResourceStore ResourceStore { get; private set; }

        public string Name { get; }

        private string SourceDir => Path.Combine(_context.Root.Target.Directory, Specification.Page.Content.Resource.GetPath(), Name);

        public void SetResources(IEnumerable<Model.ImplementationGuide.ResourceComponent> packageResources)
        {
            _resources = packageResources;
        }

        public void LoadResources()
        {
            IEnumerable<string> fileEntries =_directoryCreator.EnumerateFiles(SourceDir, "*.xml", SearchOption.AllDirectories);

            var loader = new ResourceLoader(_directoryCreator, _log);

            ResourceStore = loader.LoadResourceStore(fileEntries);
        }
                    
        public IEnumerable<StructureDefinition> StructureDefinitions
        {
            get
            {
                var result = new List<StructureDefinition>();

                foreach (Model.ImplementationGuide.ResourceComponent item in _resources)
                {
                    var type = item.GetExtension(Urn.ResourceType.GetUrnString())?.Value.ToString();

                    if (item.Purpose == Model.ImplementationGuide.GuideResourcePurpose.Profile
                       && type != null
                       && type == ResourceType.StructureDefinition.ToString()
                       && ResourceStore.Resources.Any(resource=> resource.Url == item.Source.ToString()))
                    {
                        result.AddRange(
                            ResourceStore.Resources
                                .Where(packageResource =>
                                        packageResource.Url == item.Source.ToString()
                                        && 
                                        packageResource.Resource.TypeName == ResourceType.StructureDefinition.ToString())
                                .Select(resource => resource.Resource)
                                .Cast<StructureDefinition>());
                    }

                }
                return result;
            }
        }

        public IEnumerable<OperationDefinition> OperationDefinitions
        {
            get
            {
                var result = new List<OperationDefinition>();

                foreach (Model.ImplementationGuide.ResourceComponent item in _resources)
                {
                    var type = item.GetExtension(Urn.ResourceType.GetUrnString())?.Value.ToString();

                    if (item.Purpose == Model.ImplementationGuide.GuideResourcePurpose.Profile
                       && type != null
                       && type == ResourceType.OperationDefinition.ToString()
                       && ResourceStore.Resources.Any(resource => resource.Url == item.Source.ToString()))
                    {
                        result.AddRange(
                           ResourceStore.Resources
                               .Where(packageResource =>
                                       packageResource.Url == item.Source.ToString())
                               .Select(resource => resource.Resource)
                               .Cast<OperationDefinition>());
                    }

                }
                return result;
            }           
        }

        public IEnumerable<ValueSet> ValueSets
        {
            get
            {
                var result = new List<ValueSet>();

                foreach (Model.ImplementationGuide.ResourceComponent item in _resources)
                {
                    var type = item.GetExtension(Urn.ResourceType.GetUrnString())?.Value.ToString();

                    if (item.Purpose == Model.ImplementationGuide.GuideResourcePurpose.Terminology
                       && type != null
                       && type == ResourceType.ValueSet.ToString()
                       && ResourceStore.Resources.Any(resource => resource.Url == item.Source.ToString()))
                    {
                        result.AddRange(
                           ResourceStore.Resources
                               .Where(packageResource =>
                                       packageResource.Url == item.Source.ToString())
                               .Select(resource => resource.Resource)
                               .Cast<ValueSet>());
                    }

                }
                return result;
            }
        }

        public bool HasExtensions
        {
            get
            {
                bool result = false;

                foreach (Model.ImplementationGuide.ResourceComponent item in _resources)
                {
                    var type = item.GetExtension(Urn.ResourceType.GetUrnString())?.Value.ToString();

                    if (item.Purpose == Model.ImplementationGuide.GuideResourcePurpose.Profile
                        && ResourceStore.Resources.Any(resource => resource.Url == item.Source.ToString())
                        && type != null
                        && type == ResourceType.StructureDefinition.ToString())
                    {
                        IEnumerable<StructureDefinition> definitions = ResourceStore.Resources
                               .Where(packageResource =>
                                       packageResource.Url == item.Source.ToString())
                               .Select(resource => resource.Resource)
                               .Cast<StructureDefinition>();

                        if (definitions.Any(definition => definition.IsExtension()))
                            result = true;
                    }
                }

                return result;
            }
        }

        public bool HasModifierExtensions
        {
            get
            {
                bool result = false;

                foreach (Model.ImplementationGuide.ResourceComponent item in _resources)
                {
                    var type = item.GetExtension(Urn.ResourceType.GetUrnString())?.Value.ToString();

                    if (item.Purpose == Model.ImplementationGuide.GuideResourcePurpose.Profile
                        && ResourceStore.Resources.Any(resource => resource.Url == item.Source.ToString())
                        && type != null
                        && type == ResourceType.StructureDefinition.ToString())
                    {
                        IEnumerable<StructureDefinition> definitions = ResourceStore.Resources
                              .Where(packageResource =>
                                      packageResource.Url == item.Source.ToString())
                              .Select(resource => resource.Resource)
                              .Cast<StructureDefinition>();

                        if (definitions.Any(definition =>definition.IsModifierExtension()))
                            result = true;
                    }
                }

                return result;
            }          
        }
    }
}