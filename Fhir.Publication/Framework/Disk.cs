using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hl7.Fhir.Publication.Framework
{
    public delegate bool FileMatch(string name);

    public class Disk
    {
        private readonly IDirectoryCreator _directoryCreator;

        public Disk(IDirectoryCreator directoryCreator)
        {
            if (directoryCreator == null)
                throw new ArgumentNullException(
                    nameof(directoryCreator));

            _directoryCreator = directoryCreator;
        }

        public IEnumerable<string> FilterFiles(string directory, bool isRecursive, FileMatch isMatch)
        {
            return _directoryCreator.EnumerateFiles(directory, isRecursive, isMatch);
        }

        public static string FileMaskToRegExPattern(string mask)
        {
            string pattern = mask
                .ToLower()
                .Replace("\\", "\\\\")
                .Replace(".", "\\.")
                .Replace("*", ".*")
                .Replace("?", ".?");

            return pattern;
        }

        public static string ParseMask(string name, string mask)
        {
            string[] parts = name.Split('.');

            string result = mask;

            for (int i = 0; i <= parts.Count()-1; i++)
            {
                string alias = $"${i + 1}";
                result = result.Replace(alias, parts[i]);
            }
            return result;
        }

        public static string GetRelativePath(string basepath, string path)
        {
            
            if (path.StartsWith(basepath))
            {
                string relativePath = path.Remove(0, basepath.Length).TrimStart('\\');
                return relativePath;
            }
            else
            {
                return path;
            }
        }

        public IEnumerable<string> GetDirectories(string directory, string pattern)
        {
            return _directoryCreator
                .EnumerateDirectories(directory, pattern)
                .Select(s => s.TrimEnd('\\') + "\\"); ;
        }  
    }
}