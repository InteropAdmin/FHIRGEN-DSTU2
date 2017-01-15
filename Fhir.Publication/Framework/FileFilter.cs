using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Hl7.Fhir.Publication.Framework
{
    internal class FileFilter : ISelector
    {
        private readonly IDirectoryCreator _directoryCreator;
        private readonly Disk _disk;
        private string _filter;
        private Context _context;
        private bool _isRecursive;
        private bool _isFromOutput;
        private string[] _patterns;

        public FileFilter(IDirectoryCreator directoryCreator)
        {
            if (directoryCreator == null)
                throw new ArgumentNullException(
                    nameof(directoryCreator));

            _directoryCreator = directoryCreator;
            _disk = new Disk(_directoryCreator);
         }

        public string Mask { get; private set; }
      
        private string Directory => 
            _isFromOutput 
            ? _context.Target.Directory 
            : _context.Source.Directory;

        public IEnumerable<Document> Documents
        {
            get
            {
                foreach (string filename in FileNames())
                {
                    Document document = Document.CreateFromFullPath(_context, filename);
                    yield return document;
                }
            }
        }

        public void SetMask(string mask)
        {
            Mask = mask;
        }

        public void SetContext(Context context)
        {
            _context = context;
        }

        public void SetFilter(string filter)
        {
            _filter = filter;
            _patterns = ParseFilter(filter);
        }

        public void SetIsRecursive(bool isRecursive)
        {
            _isRecursive = isRecursive;
        }

        public void SetIsFromOutput(bool isFromOutput)
        {
            _isFromOutput = isFromOutput;
        }

        private bool IsMatch(string filePath)
        {
            string relativePath = ConvertAbsolutePathToRelativePath(filePath).ToLower();

            foreach (string pattern in _patterns)
            {
                bool match = Regex.IsMatch(relativePath, pattern);
                if (match)
                    return true;
            }

            return false;
        }

        private string ConvertAbsolutePathToRelativePath(string filePath)
        {
            string filename = _directoryCreator.GetFileName(filePath);
            string dir = _directoryCreator.GetDirectoryName(filePath);
            string location = Disk.GetRelativePath(Directory, dir);

            if (string.IsNullOrEmpty(filename))
                throw new InvalidOperationException(
                    $" {filename} filename does not exist");

            string relativePath = Path.Combine(location, filename).ToLower();

            return relativePath;
        }

        private static string[] ParseFilter(string mask)
        {
            return mask
                    .Split(',')
                    .Select(
                        Disk.FileMaskToRegExPattern)
                    .ToArray();
        }

        private IEnumerable<string> FileNames()
        {
            bool recursive = _isRecursive | IsMaskRecursive(_filter);

            return _disk.FilterFiles(Directory, recursive, IsMatch);
        }

        private static bool IsMaskRecursive(string mask)
        {
            return mask.Contains('\\');
        }

        public Document GetDocument(Context context, string mask)
        {
            ISelector filter = CreateFilter(context, mask);

            if (!filter.Documents.Any())
                throw new InvalidOperationException(
                    $" expected filter  {mask}  does not exist in Generation folder");
            
            if (filter.Documents.Count() > 1)
                throw new InvalidOperationException(
                    $" more than one make file called  {mask}  exists");

            Document doc = filter.Documents.Single();

            if (doc == null)
                throw new InvalidOperationException(
                    $"Mask {mask} yielded no results");         
            else
                return doc;
        }

        private ISelector CreateFilter(Context context, string mask, bool isRecursive = false, bool isFromoutput = false)
        {
            var filter = new FileFilter(_directoryCreator);
            filter._context = context;
            filter.SetFilter(mask);
            filter.SetIsRecursive(isRecursive);
            filter.SetIsFromOutput(isFromoutput);

            return filter;
        }

        public override string ToString()
        {
            return 
                $"{_context}\\{_filter} {(_isRecursive ? "-recursive" : "")}";
        }
    }
}