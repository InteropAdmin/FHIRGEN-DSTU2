using System.IO;

namespace Hl7.Fhir.Publication.Framework
{
    public class Location 
    {
        public string Directory { get; private set; }

        public static Location Combine(Location one, Location two)
        {
            var result = new Location();
            result.Directory = Path.Combine(one.Directory, two.Directory);
            return result;
        }

        public Location() 
        {
            Directory = string.Empty;
        }

        public Location(string dir)
        {
            Directory = dir;
        }

        private static Location RelativeFrom(string basedir, string dir)
        {
            Location location = new Location();
            location.Directory = Disk.GetRelativePath(basedir, dir);
            return location;
        }

        public static Location RelativeFrom(Location baseloc, string dir)
        {
            return RelativeFrom(baseloc.Directory, dir);
        }

        public static Location RelativeFrom(Location baseloc, Location relative)
        {
            return RelativeFrom(baseloc.Directory, relative.Directory);
        }

        public Location Clone()
        {
            var clone = new Location();
            clone.Directory = Directory;
            return clone;
        }

        public override string ToString()
        {
            return Directory;
        }
    }
}