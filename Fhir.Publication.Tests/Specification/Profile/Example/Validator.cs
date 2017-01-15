using System;
using System.Collections.Generic;
using Hl7.Fhir.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validation = Hl7.Fhir.Publication.Specification.Profile.Example.Validator;

namespace Fhir.Publication.Tests.Specification.Profile.Example
{
    [TestClass]
    public class Validator
    {
        private readonly Bundle _profile;

        public Validator()
        {
            _profile = new Bundle();
            var entry = new List<Bundle.EntryComponent>();
            var bundleEntry = new Bundle.EntryComponent();
            _profile.Meta = new Meta();
            entry.Add(bundleEntry);
            _profile.Entry = entry;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Example name is not populated")]
        public void ValidateProfile_ExampleNameIsNull_ArgumentExceptionThrown()
        {
            var tag = new List<Coding>();
            tag.Add(new Coding());
            _profile.Meta.Tag = tag;

            Validation.ValidateProfile(_profile);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Example name is not populated")]
        public void ValidateProfile_ExampleNameIsEmpty_ArgumentExceptionThrown()
        {
            var tag = new List<Coding>();
            var coding = new Coding();
            coding.Code = string.Empty;
            tag.Add(coding);
            _profile.Meta.Tag = tag;

            Validation.ValidateProfile(_profile);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Example description is not populated")]
        public void ValidateProfile_ExampleDescriptionIsNull_ArgumentExceptionThrown()
        {
            var tag = new List<Coding>();
            var coding = new Coding();
            coding.Code = "a code";
            tag.Add(coding);
            _profile.Meta.Tag = tag;

            Validation.ValidateProfile(_profile);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Example description is not populated")]
        public void ValidateProfile_ExampleDescriptionIsEmpty_ArgumentExceptionThrown()
        {
            var tag = new List<Coding>();
            var coding = new Coding();
            coding.Code = "a code";
            coding.Display = string.Empty;
            tag.Add(coding);
            _profile.Meta.Tag = tag;

            Validation.ValidateProfile(_profile);
        }
    }
}
