using System.Collections.Generic;
using Hl7.Fhir.Publication.Specification.HierarchicalTable;

namespace Fhir.Publication.Tests.Mock
{
    internal class ImageCreator : IImageGenerator
    {
        public string Generate(
            bool hasChildren,
            IReadOnlyList<bool> indents)
        {
            return "tbl_bck.png";
        }
    }
}
