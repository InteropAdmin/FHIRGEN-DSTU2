using System;
using System.IO;
using System.Linq;

namespace Hl7.Fhir.Publication.Framework
{
    public class Context
    {
        private const string _root = "(root)";
        private Location _location;

        public Context(Root root, Location location = null)
        {
            if (root == null)
                throw new ArgumentNullException(
                    nameof(root));

            Root = root;
            _location = location ?? new Location();
        }

        public Root Root { get; }

        public Location Source => Location.Combine(Root.Source, _location);

        public Location Target => Location.Combine(Root.Target, _location);

        public string FilterPattern { get; set; }

        public string FolderName => Source.ToString().Split('\\').Last();

        public void CreateTargetDirectory()
        {
            System.IO.Directory.CreateDirectory(Target.Directory);
        }

        public void MoveTo(string dir)
        {
            _location = Location.Combine(_location, new Location(dir));
        }

        public static Context CreateFromSource(Root root, string path)
        {
            path = Path.GetDirectoryName(path);
            Location location = Location.RelativeFrom(root.Source, path);
            return new Context(root, location);
        }

        public Context Clone()
        {
            var context = new Context(Root);
            context._location = _location.Clone();
            
            return context;
        }

        public override string ToString()
        {
            string directory = _location.Directory;

            if (string.IsNullOrWhiteSpace(directory))
                directory = _root;

            return directory;
        }
    }
}