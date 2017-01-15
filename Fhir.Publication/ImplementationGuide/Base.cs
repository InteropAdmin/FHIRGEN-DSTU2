using System;
using System.Collections.Generic;
using System.IO;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.Config;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;
using Hl7.Fhir.Serialization;

namespace Hl7.Fhir.Publication.ImplementationGuide
{
    internal class Base
    {
        private const string _igFileName = @"IGResource.xml";
        private const string _hscic = "HSCIC";
        private readonly IDirectoryCreator _directoryCreator;
        private readonly Context _context;

        public Base(IDirectoryCreator directoryCreator, Context context)
        {
            if (directoryCreator == null)
                throw new ArgumentNullException(
                    nameof(directoryCreator));

            if (context == null)
                throw new ArgumentNullException(
                    nameof(context));

            _directoryCreator = directoryCreator;
            _context = context;
            ImplementationGuide = new Model.ImplementationGuide();
        }

        public Model.ImplementationGuide ImplementationGuide { get; private set; }

        public bool ExamplesJson { get; private set; }

        public bool ExamplesXml { get; private set; }

        private bool Schemas { get; set; }

        public bool ValuesetsInXml { get; private set; }

        public bool ValuesetsInJson { get; private set; }

        public bool StructuresInXml { get; private set; }

        public bool StructuresInJson { get; private set; }

        public bool OperationsInXml { get; private set; }

        public bool OperationsInJson { get; private set; }

        private string SoftwareVersion => Versioner.GetVersion();

        private string FilePath => Path.Combine(_context.Root.Source.ToString(), _igFileName);

        public void CreateImplementationGuide(Dictionary<string, string> configValues)
        {
            Delete();

            ImplementationGuide.Name = Store.GetConfigValue(KeyType.DmsTitle, configValues);
            ImplementationGuide.Version = Store.GetConfigValue(KeyType.DmsVersion, configValues);
            ImplementationGuide.Publisher = _hscic;
            ImplementationGuide.Copyright = _hscic;
            ImplementationGuide.Date = Date.Today().ToString();

            ExamplesJson = bool.Parse(Store.GetConfigValue(KeyType.ExamplesInJson, configValues));
            ExamplesXml = bool.Parse(Store.GetConfigValue(KeyType.ExamplesInXml, configValues));

            ValuesetsInXml = bool.Parse(Store.GetConfigValue(KeyType.ValuesetsInXml, configValues));
            ValuesetsInJson = bool.Parse(Store.GetConfigValue(KeyType.ValuesetsInJson, configValues));

            StructuresInXml = bool.Parse(Store.GetConfigValue(KeyType.StructuresInXml, configValues));
            StructuresInJson = bool.Parse(Store.GetConfigValue(KeyType.StructuresInJson, configValues));

            OperationsInXml = bool.Parse(Store.GetConfigValue(KeyType.OperationsInXml, configValues));
            OperationsInJson = bool.Parse(Store.GetConfigValue(KeyType.OperationsInJson, configValues));

            Schemas = bool.Parse(Store.GetConfigValue(KeyType.Schemas, configValues));

            AddExtensions(configValues);
        }

        private void AddExtensions(Dictionary<string, string> configValues)
        {
            ImplementationGuide.AddExtension(
                Urn.IgExamplesXml.GetUrnString(),
                new FhirBoolean(ExamplesXml));

            ImplementationGuide.AddExtension(
                Urn.IgExamplesJson.GetUrnString(),
                new FhirBoolean(ExamplesJson));

            ImplementationGuide.AddExtension(
                Urn.IgValuesetsXml.GetUrnString(),
                new FhirBoolean(ValuesetsInXml));

            ImplementationGuide.AddExtension(
                Urn.IgValuesetsJson.GetUrnString(),
                new FhirBoolean(ValuesetsInJson));

            ImplementationGuide.AddExtension(
                Urn.IgStructuresInXml.GetUrnString(),
                new FhirBoolean(StructuresInXml));

            ImplementationGuide.AddExtension(
              Urn.IgStructuresInJson.GetUrnString(),
              new FhirBoolean(StructuresInJson));

            ImplementationGuide.AddExtension(
                Urn.IgOperationsInXml.GetUrnString(),
                new FhirBoolean(OperationsInXml));

            ImplementationGuide.AddExtension(
                Urn.IgOperationsInJson.GetUrnString(),
                new FhirBoolean(OperationsInJson));

            ImplementationGuide.AddExtension(
                Urn.Schemas.GetUrnString(),
                new FhirBoolean(Schemas));

            ImplementationGuide.AddExtension(
               Urn.OnlineVersion.GetUrnString(),
               new FhirBoolean(
                   bool.Parse(
                       Store.GetConfigValue(
                           KeyType.DmsIsOnline,
                           configValues))));

            ImplementationGuide.AddExtension(
               Urn.Analytics.GetUrnString(),
               new FhirString(
                   Store.GetConfigValue(
                       KeyType.AnalyticsCode,
                       configValues)));

            ImplementationGuide.AddExtension(
                Urn.SoftwareVersion.GetUrnString(),
                new FhirString(
                    Versioner.GetVersion()));
        }

        private void Delete()
        {
            string igFileLocation = Path.Combine(_context.Root.Source.ToString(), _igFileName);

            if (_directoryCreator.FileExists(igFileLocation))
                _directoryCreator.DeleteFile(igFileLocation);
        }

        public void Save()
        {
            string output = FhirSerializer.SerializeResourceToXml(ImplementationGuide);
            _directoryCreator.WriteAllText(FilePath, output);
        }

        public void Load()
        {
            string input = _directoryCreator.ReadAllText(FilePath);

            ImplementationGuide = (Model.ImplementationGuide)FhirParser.ParseFromXml(input);

            ExamplesXml = 
                bool.Parse(
                    ImplementationGuide.GetExtension(
                        Urn.IgExamplesXml.GetUrnString())
                    .Value.ToString());

            ExamplesJson = 
                bool.Parse(
                    ImplementationGuide.GetExtension(
                        Urn.IgExamplesJson.GetUrnString())
                        .Value.ToString());

            ValuesetsInXml =
                bool.Parse(
                    ImplementationGuide.GetExtension(
                        Urn.IgValuesetsXml.GetUrnString())
                    .Value.ToString());

            ValuesetsInJson =
                bool.Parse(
                    ImplementationGuide.GetExtension(
                        Urn.IgValuesetsJson.GetUrnString())
                    .Value.ToString());

            StructuresInXml =
                bool.Parse(
                    ImplementationGuide.GetExtension(
                        Urn.IgStructuresInXml.GetUrnString())
                    .Value.ToString());

            StructuresInJson =
                bool.Parse(
                    ImplementationGuide.GetExtension(
                        Urn.IgStructuresInJson.GetUrnString())
                    .Value.ToString());

            OperationsInXml =
                bool.Parse(
                    ImplementationGuide.GetExtension(
                        Urn.IgOperationsInXml.GetUrnString())
                    .Value.ToString());

            OperationsInJson =
                bool.Parse(
                    ImplementationGuide.GetExtension(
                        Urn.IgOperationsInJson.GetUrnString())
                   .Value.ToString());

            Schemas =
                bool.Parse(
                    ImplementationGuide.GetExtension(
                        Urn.Schemas.GetUrnString())
                        .Value.ToString());
         }
    }
}