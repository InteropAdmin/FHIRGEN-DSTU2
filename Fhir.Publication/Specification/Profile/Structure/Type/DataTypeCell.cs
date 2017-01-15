using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Type
{
    internal class DataTypeCell : Cell
    {
        private readonly TableModel.Cell _cell;

        public DataTypeCell(FHIRDefinedType dataType, KnowledgeProvider knowledgeProvider)
        {
            string prefixRemoved = null;
            string type = dataType.ToString();

            if (dataType.ToString().StartsWith(Url.FhirPrefix.GetUrlString()))
                prefixRemoved = KnowledgeProvider.RemovePrefix(type);

            _cell = new TableModel.Cell(
                null,
                   knowledgeProvider.GetLinkForTypeDocument(dataType),
                    prefixRemoved
                    ?? type, null,
                    null);
        }

        public override TableModel.Cell Value => _cell;
    }
}