using System.Collections.Generic;
using System.IO;
namespace Hl7.Fhir.Publication.Framework
{
    public interface IDirectoryCreator
    {
        void WriteAllText(string path, string contents);
        bool FileExists(string path);
        string ReadAllText(string fileName);
        void DeleteFile(string path);
        void DeleteDirectory(string path);
        void Copy(string sourceName, string destinationName, bool overwrite);
        bool DirectoryExists(string path);
        void CreateDirectory(string path);
        IEnumerable<string> EnumerateFiles(string directory, bool isRecursive, FileMatch isMatch);
        string GetDirectoryName(string path);
        string GetFileName(string filePath);
        IEnumerable<string> EnumerateFiles(string directory, string pattern, SearchOption searchOption);
        IEnumerable<string> EnumerateDirectories(string directory, string pattern);
        DirectoryInfo GetDirectoryInfo(string directoryName);
        FileStream GetFileStream(string absoluteFilePath, FileMode fileMode);
    }
}