using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Hl7.Fhir.Publication.Framework.Make
{
    internal class Processor
    {
        private const string _command = @"(?<Command>\w+)";
        private const string _parameter = @"(\s+(?<Parameter>\S+))*";
        
        private static readonly Regex _expression = new Regex($@">>\s+{_command}{_parameter}", RegexOptions.Compiled);
        private readonly Match _match;

        public Processor(string statement)
        {
            _match = _expression.Match(statement);
        }

        public bool IsValid => _match.Success;

        public string Command => _match.Groups["Command"].Value;

        public IEnumerable<string> Parameters
        {
            get
            {
                foreach (Capture capture in _match.Groups["Parameter"].Captures)
                    yield return capture.Value;
            }
        }

        public bool IsFilteredBy(Context context)
        {
            if (string.IsNullOrEmpty(context.FilterPattern))
                return false;

            IEnumerable<string> filters = Parameters
                .Where(parameter => parameter.StartsWith(context.FilterPattern))
                .ToArray();

            if (!filters.Any())
                return false;

            return filters.All(
                filter => filter != context.FolderName);
        }

        public override string ToString()
        {
            return $" {Command} {Parameters.Aggregate((s1, s2) => s1 + " " + s2)}";
        }
    }
}
