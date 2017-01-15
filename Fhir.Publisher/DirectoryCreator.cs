using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hl7.Fhir.Publication.Framework;

namespace Hl7.Fhir
{
    internal class DirectoryCreator : IDirectoryCreator
    {
        public void WriteAllText(string path, string contents)
        {
            File.WriteAllText(path, contents);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public void DeleteFile(string path)
        {
            File.Delete(path);
        }

        public void DeleteDirectory(
            string path)
        {
            Directory.Delete(path, true);
        }

        public string ReadAllText(string fileName)
        {
            return File.ReadAllText(fileName);
        }

        public void Copy(string sourceName, string destinationName, bool overwrite)
        {
            File.Copy(sourceName, destinationName, overwrite);
        }

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public void CreateDirectory(string path)
        {
            DirectoryInfo dir = Directory.CreateDirectory(path);     
            dir.Attributes &= ~FileAttributes.ReadOnly;
        }

        public IEnumerable<string> EnumerateFiles(
            string directory,
            bool isRecursive,
            FileMatch isMatch)
        {
            SearchOption option = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            return
                Directory
                    .EnumerateFiles(directory, "*", option)
                    .Where(
                            filename =>
                                isMatch(filename));
        }

        public string GetDirectoryName(
            string path)
        {
            return Path.GetDirectoryName(path);
        }

        public string GetFileName(
            string filePath)
        {
            return Path.GetFileName(filePath);
        }

        public IEnumerable<string> EnumerateFiles(
            string directory,
            string pattern,
            SearchOption searchOption)
        {
            return Directory.EnumerateFiles(directory, pattern, searchOption);
        }

        public IEnumerable<string> EnumerateDirectories(
            string directory,
            string pattern)
        {
            return Directory.EnumerateDirectories(directory, pattern, SearchOption.AllDirectories);
        }

        public DirectoryInfo GetDirectoryInfo(
            string directoryName)
        {
            return new DirectoryInfo(directoryName);
        }

        public FileStream GetFileStream(
            string absoluteFilePath,
            FileMode fileMode)
        {
            return new FileStream(absoluteFilePath, fileMode);
        }
    }
}