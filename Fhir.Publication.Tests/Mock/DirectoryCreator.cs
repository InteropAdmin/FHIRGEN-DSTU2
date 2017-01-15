using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hl7.Fhir.Publication.Framework;

namespace Fhir.Publication.Tests.Mock
{
    internal class DirectoryCreator : IDirectoryCreator
    {
        public bool XmlCreated { get; private set; }
        public bool JsonCreated { get; private set; }

        public void WriteAllText(string path, string contents)
        {
            if (path.Split('.').Last() == "xml")
                XmlCreated = true;
            if (path.Split('.').Last() == "json")
                JsonCreated = true;
        }

        public bool FileExists(string path)
        {
            if (path.Contains("NotExists.md"))
                return false;
            else
            return path != "../Generated/dist/images/tbl_bck0.png";
        }

        public string ReadAllText(string fileName)
        {
            if (fileName.Contains("example") || fileName.Contains("BirthNotification_BabyPatient"))
                return Resources.BirthNotification_BabyPatient;
            if (fileName.Contains("Extension-EthnicCategory"))
                return Resources.Extension_EthnicCategory;
            if (fileName.Contains("eRS-Specialty"))
                return Resources.eRS_Specialty;
            if (fileName.Contains("MyOperation"))
                return Resources.MyOperation;
            if (fileName.Contains("ProcedureRequest"))
                return Resources.ProcedureRequest;
            if (fileName.Contains("TestStructureDefinition.md"))
                return "This is a description!";
            if (fileName.Contains("TestValueset.md"))
                return "This is a valueset introduction!";
            if (fileName.Contains("IGResource2Packages"))
                return Resources.IGResource2Packages;
            if (fileName.Contains("GPConnect-CareRecord-Composition-1-0"))
                return Resources.GPConnect_CareRecord_Composition_1_0;
            if (fileName.Contains("GPConnect-Patient-1-0"))
                return Resources.GPConnect_Patient_1_0;
            if (fileName.Contains("GPConnect-Practitioner-1-0"))
                return Resources.GPConnect_Practitioner_1_0;
            if (fileName.Contains("GPConnect-Location-1-0"))
                return Resources.GPConnect_Location_1_0;
            if (fileName.Contains("Administrative-Gender-1-0"))
                return Resources.Administrative_Gender_1_0;
            return fileName.Contains("IGResource") 
                ? Resources.IGResource 
                : string.Empty;
        }

        public void DeleteFile(string path)
        {
           //do nothing
        }

        public void DeleteDirectory(
            string path)
        {
            throw new NotImplementedException();
        }

        public void Copy(
            string sourceName,
            string destinationName,
            bool overwrite)
        {
            throw new NotImplementedException();
        }

        public bool DirectoryExists(
            string path)
        {
            return true;
        }

        public void CreateDirectory(
            string path)
        {
        }

        public IEnumerable<string> EnumerateFiles(
            string directory,
            bool isRecursive,
            FileMatch isMatch)
        {
            return new List<string>();
        }

        public string GetDirectoryName(
            string path)
        {
            return path.TrimEnd('.');
        }

        public string GetFileName(
            string filePath)
        {
            return "myFileName";
        }

        public IEnumerable<string> EnumerateFiles(
            string directory,
            string pattern,
            SearchOption searchOption)
        {
            var files = new List<string>();

            if (directory == "UnsupportedResourceSource\\Resources\\ProfileOne" 
                || directory == "UnsupportedResourceSource")
            {
                files.Add(@"C:\Projects\Profile.One\ProcedureRequest");
            }
            else
            {
                files.Add(@"C:\Projects\Profile.GetRecordQueryResponse\BirthNotification_BabyPatient.xml");
                files.Add(@"C:\Projects\Profile.GetRecordQueryResponse\Extension-EthnicCategory.xml");
                files.Add(@"C:\Projects\Profile.GetRecordQueryResponse\eRS-Specialty.xml");
                files.Add(@"C:\Projects\Profile.GetRecordQueryResponse\MyOperation.xml");
                files.Add(@"C:\Projects\Profile.GetRecordQueryResponse\GPConnect-CareRecord-Composition-1-0.xml");
                files.Add(@"C:\Projects\Profile.GetRecordQueryResponse\GPConnect-Patient-1-0.xml");
                files.Add(@"C:\Projects\Profile.GetRecordQueryResponse\GPConnect-Practitioner-1-0.xml");
                files.Add(@"C:\Projects\Profile.GetRecordQueryResponse\GPConnect-Location-1-0.xml");
                files.Add(@"C:\Projects\Profile.GetRecordQueryResponse\Administrative-Gender-1-0.xml");
            }

            return files;
        }

        public IEnumerable<string> EnumerateDirectories(
            string directory,
            string path)
        {
            var directories = new List<string>();
            directories.Add(path);
            return directories;
        }

        public DirectoryInfo GetDirectoryInfo(
            string directoryName)
        {
            var dir = new DirectoryInfo(directoryName);
            dir.CreateSubdirectory("config.json");

            return dir;
        }

        public FileStream GetFileStream(
            string absoluteFilePath,
            FileMode fileMode)
        {
            string fileName = absoluteFilePath.Split('/').Last();

            if (!File.Exists(fileName))
                File.Create(fileName);
            
            return new FileStream(fileName, fileMode);
        }
    }
}