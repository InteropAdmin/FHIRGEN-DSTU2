using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hl7.Fhir.Publication.Framework.Make
{
    internal static class Validator
    {
        public static bool TargetExists(Context context, IDirectoryCreator directoryCreator, string name)
        {
            string[] searchItems = name.Split(Path.DirectorySeparatorChar);

            DirectoryInfo sourceDir = directoryCreator.GetDirectoryInfo(context.Source.Directory);

            if (searchItems.Length == 1)
                return 
                    SearchForSingleTarget(
                        sourceDir.GetFiles(name, SearchOption.AllDirectories),
                        sourceDir,
                        name);

            return 
                searchItems.Length > 1 
                && SearchForFilesInFolder(searchItems, sourceDir);
        }

        private static bool SearchForSingleTarget(IEnumerable<FileInfo> files, DirectoryInfo sourceDir, string name)
        {
            if (files.Any())
                return true;

            DirectoryInfo[] directories = sourceDir.GetDirectories(name, SearchOption.AllDirectories);

            return directories.Any();
        }

        private static bool SearchForFilesInFolder(string[] searchItems, DirectoryInfo sourceDir)
        {
            string folderName = searchItems.First();

            DirectoryInfo[] directories = sourceDir.GetDirectories(folderName, SearchOption.AllDirectories);

            if (directories.Any())
            {
                foreach (DirectoryInfo dir in directories)
                {
                    foreach (string item in searchItems.Where(item => item != folderName))
                    {
                        FileInfo[] files = dir.GetFiles(item, SearchOption.AllDirectories);
                        if (files.Any())
                            return true;
                    }
                }
            }

            return false;
        }
    }
}