using Hl7.Fhir.Publication.Framework;

namespace Hl7.Fhir.Publication.Processors
{
    internal class SaveProcessor : IProcessor
    {
        public ISelector Influx { get; set; }

        public void Process(
            Document input,
            Stage output,
            Log log,
            IDirectoryCreator directoryCreator)
        {
            if (Mask != null)
            {
                if (Mask.StartsWith("."))
                {
                    input.Extension = Mask;
                }
                else
                {
                    string s = Disk.ParseMask(input.Name, Mask);
                    input.SetFilename(s);
                }
            }

            input.Save(directoryCreator);
        }

        public string Mask { private get; set; }
    }
}
