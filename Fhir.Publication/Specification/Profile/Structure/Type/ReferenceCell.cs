using System;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Type
{
    internal abstract class ReferenceCell : Cell
    {
        protected readonly ImplementationGuide.Package Package;
        protected readonly KnowledgeProvider KnowledgeProvider;
        protected TableModel.Cell Cell;

        protected ReferenceCell(
           ImplementationGuide.Package package,
            string url,
            KnowledgeProvider knowledgeProvider)
        {
            if (package == null)
                throw new ArgumentNullException(
                    nameof(package));

            if (string.IsNullOrEmpty(url))
                throw new ArgumentException(
                    " TypeName cannot be null or empty!",
                    nameof(url));

            if (knowledgeProvider == null)
                throw new ArgumentNullException(
                    nameof(knowledgeProvider));

            Package = package;
            KnowledgeProvider = knowledgeProvider;
        }

        public override TableModel.Cell Value => Cell;
    }
}