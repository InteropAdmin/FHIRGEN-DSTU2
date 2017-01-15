using System;
using System.Collections.Generic;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.Config;
using Hl7.Fhir.Publication.ImplementationGuide;

namespace Hl7.Fhir.Publication.Processors.ImplementationGuide
{
    internal class IgCreateProcessor : IProcessor
    {
        private Store _configStore;
        private Base _igFactory;
        private Log _log;
        private Document _input;
        private IDirectoryCreator _directoryCreator;

        public ISelector Influx { get; set; }

        public void Process(
            Document input,
            Stage output,
            Log log,
            IDirectoryCreator directoryCreator)
        {
            if (input == null)
                throw new ArgumentNullException(
                    nameof(input));

            if (output == null)
                throw new ArgumentNullException(
                    nameof(output));

            if (log == null)
                throw new ArgumentNullException(
                    nameof(log));

            if (directoryCreator == null)
                throw new ArgumentNullException(
                    nameof(directoryCreator));

            _log = log;
            _directoryCreator = directoryCreator;
            _input = input;

            _log.Info("Start: creating IGResource.xml");

            _configStore = new Store(_directoryCreator);
            Dictionary<string, string> configValues = _configStore.GetConfigStore(input.Context);

            _igFactory = new Base(_directoryCreator, _input.Context);
            _igFactory.CreateImplementationGuide(configValues);
            _igFactory.Save();

            log.Info("End: IGResource.xml created");
        }
    }
}