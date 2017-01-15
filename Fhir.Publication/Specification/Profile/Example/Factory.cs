using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Base = Hl7.Fhir.Publication.ImplementationGuide.Base;

namespace Hl7.Fhir.Publication.Specification.Profile.Example
{
    internal static class Factory
    {
        public static XElement ToHtml(
            Model.ImplementationGuide implementationGuide, 
            string path, 
            Base baseImplementationGuide, 
            Model.ImplementationGuide.ResourceComponent resource,
            IEnumerable<Coding> metaData)
        {
            XElement exampleTable = null;

            List<Description> examplesWithDescriptions = implementationGuide.Package.Single(
                package =>
                    package.Name == path)
                .Resource.Where(
                    profile =>
                        profile.Purpose == Model.ImplementationGuide.GuideResourcePurpose.Example
                        && profile.ExampleFor.Reference == resource.Source.ToString())
                .Select(
                    element =>
                        element.Name)
                .Join(
                    metaData,
                    example => example,
                    meta => meta.Code,
                    (example, meta) => new Description(example, meta.Display))
                .OrderByDescending(
                    meta =>
                        meta.Name)
                .ToList();

            if (examplesWithDescriptions.Any())
                exampleTable = ProfileTable.ToHtml(
                    baseImplementationGuide,
                    resource.Name,
                    path.Split(Path.DirectorySeparatorChar).Last(),
                    examplesWithDescriptions);

            return exampleTable;
        }
    }
}