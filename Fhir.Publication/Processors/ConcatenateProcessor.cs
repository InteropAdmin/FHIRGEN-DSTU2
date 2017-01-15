using System.Text;
using Hl7.Fhir.Publication.Framework;

namespace Hl7.Fhir.Publication.Processors
{
    internal class ConcatenateProcessor : IProcessor
    {
        public ISelector Influx { get; set; }

        public void Process(
            Document input,
            Stage output,
            Log log,
            IDirectoryCreator directoryCreator)
        {
            var builder = new StringBuilder(input.Text);

            foreach (Document doc in input.Attachments)
            {
                builder.Append(doc.Text);
            }

            foreach (Document doc in Influx.Documents)
            {
                builder.Append(doc.Text);
            }

            Document result = output.CloneAndPost(input);
            result.Text = builder.ToString();
        }
    }   
}