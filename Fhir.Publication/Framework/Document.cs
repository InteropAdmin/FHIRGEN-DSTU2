using System.Collections.Generic;
using System.IO;

namespace Hl7.Fhir.Publication.Framework
{
    internal class Document
    {
        public readonly List<Document> Attachments = new List<Document>();
        private DocumentState _documentState;
        private string _content;
      
        private enum DocumentState
        {
            Closed,
            Open
        }

        private Document()
        {
        }

        public Context Context { get; private set; }
        public string Name { get; private set; }
        public string Extension { get; set; }

        public string FileName
        {
            get
            {
                string extension = Extension;
                if (!extension.StartsWith("."))
                    extension = string.Concat(".", extension);

                return string.Concat(Name, extension);
            }
        }

        public string Text
        {
            get
            {
                Load();
                return _content;
            }
            set
            {
                _content = value;
                _documentState = DocumentState.Open;
            }
        }

        public string SourceFullPath => Path.Combine(Context.Source.Directory, FileName);

        public string TargetFullPath => Path.Combine(Context.Target.Directory, FileName);

        public void Attach(IEnumerable<Document> attachments)
        {
            Attachments.AddRange(attachments);
        }
        
        private void Load()
        {
            lock (this)
            {
                if (_documentState == DocumentState.Closed)
                {
                    _content = File.ReadAllText(SourceFullPath);
                    _documentState = DocumentState.Open;
                }
            }
        }

        public void Save(IDirectoryCreator directoryCreator)
        {
            Load();

            Context.CreateTargetDirectory();
            directoryCreator.WriteAllText(TargetFullPath, _content);
        }

       // Create a new Item, based on the current item, but with a new stream
        public Document CloneMetadata()
        {
            var doc = new Document();
            doc.Context = Context.Clone();
            doc.Name = Name;
            doc.Extension = Extension;
            doc._documentState = _documentState;

            return doc;
        }

        public Document Clone()
        {
            Document clone = CloneMetadata();
            clone.Text = Text;

             return clone;
        }

        public void SetFilename(string filename, string extension = null)
        {
            Name = Path.GetFileNameWithoutExtension(filename);
            Extension = extension ?? Path.GetExtension(filename);
        }

        public static Document CreateFromFullPath(Context context, string fullpath)
        {
            var document = new Document();
            document.Context = Context.CreateFromSource(context.Root, fullpath);
            document.SetFilename(fullpath);

            return document;
        }
        
        public static Document CreateInContext(Context context, string filename, IDirectoryCreator directoryCreator)
        {
            var document = new Document();
            document.Context = context.Clone();
            string dir = directoryCreator.GetDirectoryName(filename);
            document.Context.MoveTo(dir);
            document.SetFilename(filename);

            return document;
        }

        public override string ToString()
        {
            return $"{Context.Source}\\{FileName}";
        }
    }
}