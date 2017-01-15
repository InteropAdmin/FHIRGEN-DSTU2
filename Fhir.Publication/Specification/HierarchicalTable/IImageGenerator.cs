using System.Collections.Generic;

namespace Hl7.Fhir.Publication.Specification.HierarchicalTable
{
    internal interface IImageGenerator
    {
        string Generate(bool hasChildren, IReadOnlyList<bool> indents);
    }
}