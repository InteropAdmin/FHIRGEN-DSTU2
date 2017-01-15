using System.Text;
using Hl7.Fhir.Publication.Framework;

namespace Hl7.Fhir.Publication.Razor
{
    public abstract class RazorTemplate<T>
    {
        private readonly StringBuilder _stringReader;

        protected RazorTemplate()
        {
            _stringReader = new StringBuilder();
        }

        public T Model { get; set; }

        public abstract void Execute();

        public virtual void Write(object value)
        {
            WriteLiteral(value);
        }

        public void Include(string filename, IDirectoryCreator directoryCreator)
        {
            WriteLiteral(directoryCreator.ReadAllText(filename));
        }

        protected virtual void WriteLiteral(object value)
        {
            _stringReader.Append(value);
        }

        public string Render()
        {
            Execute();
            string renderedText = _stringReader.ToString();
            _stringReader.Clear();

            return renderedText;
        }
    }
}
