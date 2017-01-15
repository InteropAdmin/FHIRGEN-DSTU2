using System.Linq;

namespace Hl7.Fhir.Publication.Framework.Make
{
   internal static class Selector
    {        
        public const string Stashprefix = "$";

        public static ISelector Create(Context context, IDirectoryCreator directoryCreator, Line line)
        {
            return line.File.StartsWith(Stashprefix)
                ? (ISelector)new StashFilter(line.File, null)
                : CreateFilter(line.File, null, line.IsRecursive, line.IsOutput, context, directoryCreator);
        }

        public static ISelector Create(Context context, IDirectoryCreator directoryCreator, Processor processor)
        {
            string firstParameter = processor.Parameters.First();

            return firstParameter.StartsWith(Stashprefix)
           ? (ISelector)new StashFilter(firstParameter, null)
           : CreateFilter(firstParameter, null, false, false, context, directoryCreator);
        }

        private static FileFilter CreateFilter(
            string parameter, 
            string mask,
            bool isRecursive, 
            bool isOutput, 
            Context context,
            IDirectoryCreator directoryCreator)
        {
            var filter = new FileFilter(directoryCreator);

            filter.SetFilter(parameter);
            filter.SetIsRecursive(isRecursive);
            filter.SetIsFromOutput(isOutput);
            filter.SetContext(context);
            filter.SetMask(mask);

            return filter;
        }
    }
}