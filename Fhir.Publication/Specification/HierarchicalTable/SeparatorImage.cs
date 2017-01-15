using System.Drawing;

namespace Hl7.Fhir.Publication.Specification.HierarchicalTable
{
    internal class SeparatorImage
    {
        private readonly Bitmap _bitmap;
        private readonly string _filename;

        public SeparatorImage(Bitmap bitmap, string filename)
        {
            _bitmap = bitmap;
            _filename = filename;
        }

        public Bitmap Bitmap => _bitmap;
        public string Filename => _filename;
    }
}