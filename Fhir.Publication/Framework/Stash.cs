using System;
using System.Collections.Generic;

namespace Hl7.Fhir.Publication.Framework
{
    internal static class Stash
    {
        static readonly Dictionary<string, Stage> _stages = new Dictionary<string, Stage>();

        private static Stage New(string key)
        {
            Stage stage = Stage.New();
            _stages.Add(key, stage);
            return stage;
        }

        private static Stage Assert(string key)
        {
            return _stages.ContainsKey(key) ? _stages[key] : New(key);
        }

        private static Stage Find(string key)
        {
            return _stages.ContainsKey(key) ? _stages[key] : null;
        }

        public static void Push(string key, Document document)
        {
            Stage stage = Assert(key);
            stage.Post(document);
        }

        public static Document Get(string key, string name)
        {
            Stage stage = Find(key);

            if (stage == null)
                throw new InvalidOperationException(
                    $"Stash {key} does not exist");

            Document doc = stage.Find(name);

            if (doc == null)
                throw new InvalidOperationException(
                    $"Document {name} was not found in stash {key}");

            return doc;
        }

        public static Stage Get(string key)
        {
            return _stages[key];
        }
    }
}
