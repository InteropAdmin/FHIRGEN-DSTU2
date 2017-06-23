using System;
using System.Collections.Generic;
using System.Linq;

namespace Hl7.Fhir.Publication.Framework
{
    internal class PipeLine 
    {
        public readonly List<IProcessor> Processors;

        public PipeLine()
        {
            Processors = new List<IProcessor>();
        }

        public void Add(IProcessor processor)
        {
            if (processor != null)
                Processors.Add(processor);
            else
                throw new ArgumentNullException(
                    nameof(processor));   
        }

        public override string ToString()
        {
            return string.Join(" >> ", Processors.Select(p => p.GetType().Name));
        }
    }
}