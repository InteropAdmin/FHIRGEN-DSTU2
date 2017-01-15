using System;
using System.Collections.Generic;
using System.Linq;

namespace Hl7.Fhir.Publication.Framework
{
    internal class Stage 
    {
        private readonly Queue<Document> _queue = new Queue<Document>();
        
        public Stage(IEnumerable<Document> documents)
        {
            if (documents == null)
                throw new ArgumentNullException(
                    nameof(documents));

            Post(documents);
        }

        public IEnumerable<Document> Documents
        {
            get
            {
                return _queue;
            }
        }
        
        public void Post(Document item)
        {
            _queue.Enqueue(item);
        }
        
        public void Post(IEnumerable<Document> documents)
        {
            foreach (var doc in documents)
            {
                _queue.Enqueue(doc);
            }
        }

        public Document Take()
        {
            return _queue.Dequeue();
        }

        public Document CloneAndPost(Document source)
        {
            Document item = source.CloneMetadata();
            Post(item);
            return item;
        }

        public Document Find(string name)
        {
            return _queue.FirstOrDefault(d => d.Name == name || d.FileName == name);
        }

        public static Stage operator +(Stage stage, IEnumerable<Document> documents)
        {
            stage.Post(documents);
            return stage;
        }

        public static Stage New()
        {
            return new Stage(Enumerable.Empty<Document>());
        }

        public override string ToString()
        {
            return Documents.ToString();
        }
    }
}
