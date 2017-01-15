using System.Collections.Generic;

namespace Hl7.Fhir.Publication.Framework
{
    internal interface ISelector
    {
        string Mask { get;}
        IEnumerable<Document> Documents { get; }
    }
}
