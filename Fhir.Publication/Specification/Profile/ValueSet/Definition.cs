using System;
using System.Xml.Linq;
using Hl7.Fhir.Publication.Framework;

namespace Hl7.Fhir.Publication.Specification.Profile.ValueSet
{
    internal abstract class Definition
    {
        protected readonly Model.ValueSet Valueset;
        protected readonly Log Log;

        protected Definition(Model.ValueSet valueset, Log log)
        {
            if (valueset == null)
                throw new ArgumentNullException(
                    nameof(valueset));

            if (log == null)
                throw new ArgumentNullException(
                    nameof(log));

            Valueset = valueset;
            Log = log;
        }

        public abstract XElement Description { get; }
    }
}