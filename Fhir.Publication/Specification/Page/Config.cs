using System;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;

namespace Hl7.Fhir.Publication.Specification.Page
{
    public class Config
    {
        private readonly Model.ImplementationGuide _implementationGuide;

        public Config()
        {    
            //needed for Razor rendering       
        }

        public Config(
            Content content,
            string name,
            string intro,
            string text,
            string examples,
            string extensions,
            string bindings,
            Model.ImplementationGuide implementationGuide)
        {
            if (implementationGuide == null)
                throw new ArgumentNullException(
                    nameof(implementationGuide));

            ContentType = content;
            _implementationGuide = implementationGuide;
            Name = name;
            Intro = intro;
            Text = text;
            Examples = examples;
            Extensions = extensions;
            Bindings = bindings;
        }

        public Content ContentType { get; private set; }

        public bool IsOnline => _implementationGuide.GetBoolExtension(Urn.OnlineVersion.GetUrnString()) == true;

        public string OnlineAnalyticsKey => _implementationGuide.GetStringExtension(Urn.Analytics.GetUrnString());

        public string Publisher => _implementationGuide.Publisher;

        public string PublisherVersion => string.Concat("FHIR-Furnace : ", _implementationGuide.GetStringExtension(Urn.SoftwareVersion.GetUrnString()));

        public string Version => _implementationGuide.Version;

        public string Name { get; }

        public string Intro { get; }

        public string Text { get; }

        public string Examples { get; }

        public string Extensions { get; }

        public string Bindings { get; }

        public bool HasExamples => !string.IsNullOrEmpty(Examples);

        public bool HasExtensions => !string.IsNullOrEmpty(Extensions);

        public bool HasBindings => !string.IsNullOrEmpty(Bindings);
    }
}