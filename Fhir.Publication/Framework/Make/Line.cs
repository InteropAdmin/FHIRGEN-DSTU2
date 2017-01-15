using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Hl7.Fhir.Publication.Framework.Make
{
    internal class Line
    {
        private const string _file = @"(?<File>\S+)";
        private const string _flag = @"((?<Flag>-recursive|-output)\s+){0,1}";
        private const string _processor = @"(?<Processor>>>\s+[^>]+)+";

        private static readonly Regex _expression = new Regex($@"select\s+{_file}\s+{_flag}{_processor}", RegexOptions.Compiled);
        private readonly Match _match;
        private readonly string _statement;

        public Line(string statement)
        {
            _match = _expression.Match(statement);
            _statement = statement;
        }

        public bool IsValid => _match.Success;

        public string File => _match.Groups["File"].Value;

        private string Flag => _match.Groups["Flag"].Value;

        public bool IsRecursive => Flag == "-recursive";

        public bool IsOutput => Flag == "-output";

        public IEnumerable<Processor> Processors
        {
            get
            {
                foreach (Capture capture in _match.Groups["Processor"].Captures)
                    yield return new Processor(capture.Value);
            }
        }

        public override string ToString()
        {
            return _statement;
        }
    }
}
