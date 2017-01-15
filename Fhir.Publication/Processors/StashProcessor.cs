using System;
using Hl7.Fhir.Publication.Framework;

namespace Hl7.Fhir.Publication.Processors
{
    internal class StashProcessor : IProcessor
    {
        private readonly string _key;

        public StashProcessor(string key)
        {
            if (key == null)
                throw new ArgumentNullException(
                    nameof(key));

            _key = key;
        }

        public ISelector Influx { get; set; }

        public void Process(
            Document input,
            Stage output,
            Log log,
            IDirectoryCreator directoryCreator)
        {
            Stash.Push(_key, input.Clone());
        }
    }
}