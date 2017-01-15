/*
//Copyright (c) 2011+, HL7, Inc
//All rights reserved.

//Redistribution and use in source and binary forms, with or without modification, 
//are permitted provided that the following conditions are met:

// * Redistributions of source code must retain the above copyright notice, this 
//   list of conditions and the following disclaimer.
// * Redistributions in binary form must reproduce the above copyright notice, 
//   this list of conditions and the following disclaimer in the documentation 
//   and/or other materials provided with the distribution.
// * Neither the name of HL7 nor the names of its contributors may be used to 
//   endorse or promote products derived from this software without specific 
//   prior written permission.

//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
//ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
//WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
//IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
//INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
//NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR 
//PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
//WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
//ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
//POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.IO;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hl7.Fhir.Publication.Specification.Profile
{
    internal class ResourceGenerator
    {
        private const string _xmlExtension = ".xml";
        private const string _jsonExtension = ".json";
        private readonly Log _log;
        private readonly IDirectoryCreator _directoryCreator;
        private readonly bool _xmlRequired;
        private readonly bool _jsonRequired;
        private string _targetPath;

        public ResourceGenerator(
            IDirectoryCreator directoryCreator,
            bool xmlRequired,
            bool jsonRequired,
            Log log)
        {
            if (directoryCreator == null)
                throw new ArgumentNullException(
                    nameof(directoryCreator));

            if (log == null)
                throw new ArgumentNullException(
                    nameof(log));

            _directoryCreator = directoryCreator;
            _log = log;
            _xmlRequired = xmlRequired;
            _jsonRequired = jsonRequired;
        }

        public void Generate(
            string item,
            string sourcePath)
        {
            if (string.IsNullOrEmpty(item))
                throw new ArgumentException(
                    nameof(item));

            if (string.IsNullOrEmpty(sourcePath))
                throw new ArgumentNullException(
                    nameof(sourcePath));

            _targetPath = sourcePath;

            string sourceFile = Path.Combine(
                sourcePath, 
                string.Concat(item, _xmlExtension));

            if (!_directoryCreator.DirectoryExists(_targetPath))
                throw new InvalidOperationException($" {_targetPath} does not exist!");

            if (_directoryCreator.FileExists(sourceFile))
                CreateFiles(sourceFile, item);
            else
                throw new InvalidOperationException($" {sourceFile} does not exist!");
        }

        private void CreateFiles(string fileName, string item)
        {
            string xml = _directoryCreator.ReadAllText(fileName);

            Resource resource;
            try
            {
                resource = FhirParser.ParseResourceFromXml(xml);
            }
            catch (Exception e)
            {
                _log.Error(e, $"{e.Message} Error in : {fileName} ");
                throw;
            }
            
            if (_xmlRequired)
                PersistXmlFile(resource, item);

            if (_jsonRequired)
                PersistJsonFile(resource, item);
        }

        private void PersistXmlFile(Resource resource, string item)
        {
            _directoryCreator.WriteAllText(
                Path.Combine(_targetPath, string.Concat(item, _xmlExtension)), 
                GetXml(resource));
        }

        private static string GetXml(Resource resource)
        {
            return FhirSerializer.SerializeResourceToXml(resource);
        }

        private void PersistJsonFile(Resource resource, string item)
        {
            _directoryCreator.WriteAllText(
                Path.Combine(_targetPath, string.Concat(item, _jsonExtension)), 
                GetJson(resource));
        }

        private static string GetJson(Resource resource)
        {
            string json = FhirSerializer.SerializeResourceToJson(resource);
            string jsonFormatted = JValue.Parse(json).ToString(Formatting.Indented);

            return jsonFormatted;

        }
    }
}