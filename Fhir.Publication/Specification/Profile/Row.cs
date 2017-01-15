namespace Hl7.Fhir.Publication.Specification.Profile
{
    internal static class Row
    {
        public static TableModel.Row Create(string iconType, ImplementationGuide.IResource definition, string structureType, string link)
        {
            var row = new TableModel.Row();

            row.SetIcon(iconType);
            row.GetCells()
                .Add(
                    new TableModel.Cell(null, KnowledgeProvider.GetLinkForLocalResource(definition.Name), definition.Name, null, null));

            row.GetCells()
                .Add(
                    new TableModel.Cell(null, link, structureType, null, null));

            row.GetCells()
                .Add(
                    new TableModel.Cell(null, null, definition.Description, null, null));

            return row;
        }
    }
}