namespace Hl7.Fhir.Publication.Framework
{
    internal static class Work
    {
        public static void Append(this Make.Bulk bulk, ISelector filter, PipeLine pipeline)
        {
            var statement = new Statement(pipeline, filter);

            bulk.Append(statement);
        }
    }
}
