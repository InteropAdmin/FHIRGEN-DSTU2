using System;
using System.Collections.Generic;

namespace Hl7.Fhir.Publication.Framework.Make
{
    internal class Bulk : IWork
    {
        private readonly List<IWork> _worklist = new List<IWork>();

        public void Append(params IWork[] work)
        {
            if (work == null)
                throw new ArgumentNullException(
                    nameof(work));

            _worklist.AddRange(work);
        }

        public void Execute(
            Log log,
            IDirectoryCreator directoryCreator)
        {
            foreach (IWork work in _worklist)
            {
                work.Execute(log, directoryCreator);
            }
        }
    }
}