using System;
using System.Collections.Generic;
using System.IO;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;
using Newtonsoft.Json;

namespace Hl7.Fhir.Publication.Framework.Config
{
    public class Store
    {
        private const string _configFileName = @"config.json";
        private readonly IDirectoryCreator _directoryCreator;

        public Store(IDirectoryCreator directoryCreator)
        {
            if (directoryCreator == null)
                throw new ArgumentNullException(
                    nameof(directoryCreator));

            _directoryCreator = directoryCreator;
        }

        public Dictionary<string, string> GetConfigStore(Context context)
        {
            string configFileLocation = Path.Combine(context.Root.Source.ToString(), _configFileName);

            if (!_directoryCreator.FileExists(configFileLocation))
                throw new InvalidOperationException(
                    string.Concat(" Config File is missing. It should be here : ", configFileLocation));

            return ReadConfigStore(configFileLocation);
        }

        private Dictionary<string, string> ReadConfigStore(string fileName)
        {
            string configFile = _directoryCreator.ReadAllText(fileName);

            return 
                JsonConvert.DeserializeObject<Dictionary<string, string>>(configFile);
        }

        public static string GetConfigValue(KeyType key, Dictionary<string, string> configValues)
        {
            string dictionaryKey = key.GetConfigKeyTypeString();

            if (configValues.ContainsKey(dictionaryKey))
                return configValues[dictionaryKey];
            else
               throw new InvalidOperationException($" key: {key.GetConfigKeyTypeString()} is not found in the config store!");
        }
    }
}