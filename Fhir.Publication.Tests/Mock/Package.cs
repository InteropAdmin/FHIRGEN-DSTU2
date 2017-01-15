using System;
using System.Collections.Generic;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.ImplementationGuide;

namespace Fhir.Publication.Tests.Mock
{
    internal class Package : IPackage
    {
        public void LoadStructures()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StructureDefinition> GetStructuresInPackage(ImplementationGuide.ImplementationGuidePackageComponent package)
        {
            var result = new List<StructureDefinition>();
            var definition = new StructureDefinition();
            definition.Name = "MyStructureDefintion";

            result.Add(definition);
            return result;
        }

        public StructureDefinition GetStructureDefinitionByUrl(
            string url)
        {
            throw new NotImplementedException();
        }

        public OperationDefinition GetOperationDefinitionByUrl(
            string url)
        {
            throw new NotImplementedException();
        }

        public string GetNameByUrl(
            string url)
        {
            throw new NotImplementedException();
        }

        public bool PackageHasExtensions(
            ImplementationGuide.ImplementationGuidePackageComponent package)
        {
            throw new NotImplementedException();
        }

        public bool PackageHasModifierExtensions(
            ImplementationGuide.ImplementationGuidePackageComponent package)
        {
            throw new NotImplementedException();
        }
    }
}