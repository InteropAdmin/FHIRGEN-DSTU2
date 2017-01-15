using System;
using System.Collections.Generic;
using System.Linq;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Model;

namespace Hl7.Fhir.Publication.ImplementationGuide
{
    internal class ResourceStore
    {
        private readonly Log _log;

        public ResourceStore(Log log)
        {
            Resources = new HashSet<PackageResource>(new PackageResourceComparer());
            _log = log;
        }

        public HashSet<PackageResource> Resources { get; }

        public void Add(PackageResource resource)
        {
            if(!Resources.Add(resource))
               throw new InvalidOperationException($" Duplicate resource {resource.Name} found in {resource.Package}!");
        }

        public string GetStructureDefinitionNameByUrl(string url, string package)
        {
            string result = string.Empty;

            StructureDefinition resource = GetStructureDefinitionByUrl(url, package);

            if (resource != null)
                result = resource.Name;

            return result;
        }

        public StructureDefinition GetStructureDefinitionByUrl(string url, string package)
        {
            StructureDefinition structureDefinition = null;

            if (Resources.Any(
                resource =>
                    resource.Url == url
                    && resource.Package == package
                    && resource.Resource.ResourceType.ToString() == ResourceType.StructureDefinition.ToString()))
            {
                Model.Resource resource = Resources.Single(item => item.Url == url && item.Package == package).Resource;

                bool isStructureDefinition = resource is StructureDefinition;

                if (isStructureDefinition)
                    structureDefinition = (StructureDefinition)resource;
            }
            else
                throw new InvalidOperationException($" StructureDefinition resource with url: {url} does not exist in {package}!");

            return structureDefinition;
        }

        public OperationDefinition GetOperationDefinitionByUrl(string url, string package)
        {
            OperationDefinition operationDefinition = null;

            if (Resources.Any(
               resource =>
                   resource.Url == url
                   && resource.Package == package
                   && resource.Resource.ResourceType.ToString() == ResourceType.OperationDefinition.ToString()))
            {
                Model.Resource resource = Resources.Single(item => item.Url == url && item.Package == package).Resource;

                bool isOperationDefinition = resource is OperationDefinition;

                if (isOperationDefinition)
                    operationDefinition = (OperationDefinition)resource;
            }
            else
                throw new InvalidOperationException($" OperationDefinition resource with url: {url} does not exist in {package}!");

            return operationDefinition;
        }

        public ValueSet GetValuesetByUrl(string url, string package)
        {
            ValueSet valueset = null;

            if (Resources.Any(
                resource =>
                    resource.Url == url
                    && resource.Package == package
                    && resource.Resource.ResourceType.ToString() == ResourceType.ValueSet.ToString()))
            {
                Model.Resource resource = Resources.Single(item => item.Url == url && item.Package == package).Resource;

                bool isValueset = resource is ValueSet;

                if (isValueset)
                    valueset = (ValueSet)resource;
            }
            else
                throw new InvalidOperationException($" ValueSet resource with url: {url} does not exist in {package}!");

            return valueset;
        }

        private ValueSet GetValuesetByUrl(string url)
        {
            ValueSet valueset = null;

            if (Resources.Any(
                resource =>
                    resource.Url == url
                    && resource.Resource.ResourceType.ToString() == ResourceType.ValueSet.ToString()))
            {
                Model.Resource resource = Resources.Single(item => item.Url == url).Resource;

                bool isValueset = resource is ValueSet;

                if (isValueset)
                    valueset = (ValueSet)resource;
            }
            else
                throw new InvalidOperationException($" resource with url: {url} does not exist in the DMS!");

            return valueset;
        }

        public string GetValuesetNameByUrl(string url, string package)
        {
            string result = string.Empty;

            ValueSet resource = string.IsNullOrEmpty(package) ? GetValuesetByUrl(url) : GetValuesetByUrl(url, package);

            if (resource != null)
                result = url.Split('/').Last(); 
            else
                _log.Error($" resource with url: {url} does not exist in {package}!");

            return result;
        }
    }
}