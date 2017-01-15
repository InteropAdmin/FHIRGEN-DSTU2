using System.Collections.Generic;

namespace Hl7.Fhir.Publication.Framework
{
    internal class StashFilter : ISelector
    {
        private readonly string _key;

        public StashFilter(string key, string mask)
        {
            _key = key;
            Mask = mask;
        }

        public string Mask { get; }

        public IEnumerable<Document> Documents => Stash.Get(_key).Documents;

        public override string ToString()
        {
            return $"Stash {_key} ({Mask})";
        }
    }
}
