using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hl7.Fhir.Publication.Processors;
using Hl7.Fhir.Publication.Processors.ImplementationGuide;
using Hl7.Fhir.Publication.Processors.Index;
using Hl7.Fhir.Publication.Processors.Profile;
using ValueSetProcessor = Hl7.Fhir.Publication.Processors.Index.ValueSetProcessor;

namespace Hl7.Fhir.Publication.Framework.Make
{
    internal static class Interpreter
    {
        private static IEnumerable<string> ReadLines(string makeFileText)
        {
            using (var reader = new StringReader(makeFileText))
            {
                string textLine;
                while((textLine = reader.ReadLine()) != null)
                {
                    yield return textLine;
                }
            }
        }

        public static IWork InterpretMakeFile(string makeFileText, Context context, IDirectoryCreator directoryCreator)
        {
            var bulk = new Bulk();

            foreach (Statement work in ReadLines(makeFileText)
                .Where(line => !SkipLine(line))
                .Select(line => new Line(line))
                .Select(line => InterpretLine(context, line, directoryCreator))
                .Where(statement => statement != null))
            {
                bulk.Append(work);
            }

            return bulk;
        }

        private static bool SkipLine(string statement)
        {
            return (string.IsNullOrWhiteSpace(statement)) || IsComment(statement);
        }

        private static bool IsComment(string statement)
        {
            return statement.StartsWith("//");
        }

        private static Statement InterpretLine(Context context, Line line, IDirectoryCreator directoryCreator)
        {
            if (!line.IsValid)
                throw new InvalidOperationException($" Invalid line: {line}");
             
            if (line.Processors.Any(processor => processor.IsFilteredBy(context)))
                return null;

            if (!IsStashedFile(line.File) && !Validator.TargetExists(context, directoryCreator, line.File))
                throw new InvalidOperationException(
                    $" {line.File} does not exist in {context.Source.Directory}!");

            var statement = new Statement();

            statement.SetSelector(Selector.Create(context, directoryCreator, line));

            foreach (IProcessor proc in line.Processors.Select(
                processor =>
                    InterpretProcessor(context, processor, directoryCreator)))
            {
                statement.Add(proc);
            }
            
            return statement;
        }

        private static bool IsStashedFile(string name)
        {
            return name.StartsWith("$");
        }

        private static IProcessor InterpretProcessor(Context context, Processor processor, IDirectoryCreator directoryCreator)
        {
            if (!processor.IsValid)
                throw new InvalidOperationException($" processor is not valid: {processor}");

            try
            {
                string command = processor.Command;

                ProcessorType processorType = Enum
                    .GetValues(typeof(ProcessorType))
                    .Cast<ProcessorType>()
                    .SingleOrDefault(type => string.Compare(type.ToString(), command, StringComparison.OrdinalIgnoreCase) == 0);

                if (processorType == ProcessorType.None)
                    throw new InvalidOperationException($"Unsupported processor type {command}");

                switch (processorType)
                {
                    case ProcessorType.None:
                    case ProcessorType.Markdown:
                        return new MarkdownProcessor();

                    case ProcessorType.Razor:
                        {
                            var param = processor.Parameters.SingleOrDefault();

                            var renderProcessor = new RazorProcessor();

                            if (param == null)
                                throw new InvalidOperationException("cshtml template not provided for razor processor!");

                            renderProcessor.Influx = Selector.Create(context, directoryCreator, processor);

                            return renderProcessor;
                        }
                    case ProcessorType.Delete:
                        return new DeleteProcessor();

                    case ProcessorType.Copy:
                        return new CopyProcessor();

                    case ProcessorType.IGCreate:
                        return new IgCreateProcessor();

                    case ProcessorType.IGPackage:
                        {
                            var igFactory = new ImplementationGuide.Base(directoryCreator, context);
                            igFactory.Load();
                            return new IgPackageProcessor(igFactory);
                        }
                    case ProcessorType.Save:
                        {
                            string mask = processor.Parameters.FirstOrDefault();
                            var saveProcessor = new SaveProcessor();
                            saveProcessor.Mask = mask;
                            return saveProcessor;
                        }
                    case ProcessorType.Stash:
                        {
                            string key = processor.Parameters.Single(
                                content => 
                                    content.StartsWith(Selector.Stashprefix));
                            
                           return new StashProcessor(key);
                        }
                    case ProcessorType.Attach:
                        {
                            IProcessor attachProcessor = new AttachProcessor();
                            attachProcessor.Influx = Selector.Create(context, directoryCreator, processor);
                            return attachProcessor;
                        }
                    case ProcessorType.Concatenate:
                        return new ConcatenateProcessor();

                    case ProcessorType.Make:
                        return new MakeProcessor();

                    case ProcessorType.Makeall:
                        return new MakeForAllProcessor(processor.Parameters.FirstOrDefault());

                    case ProcessorType.Profiletable:
                        {
                            var param = processor.Parameters.SingleOrDefault();
                            var profileProcessor = new ProfileIndexProcessor();

                            if (param == null)
                                throw new InvalidOperationException("cshtml template not provided for profileIndex processor!");

                            profileProcessor.Influx = Selector.Create(context, directoryCreator, processor);
                            return profileProcessor;
                        }   
                    case ProcessorType.Structure:
                        {
                            var param = processor.Parameters.SingleOrDefault();
                            var structureProcessor = new StructureProcessor();

                            if (param == null)
                                throw new InvalidOperationException("cshtml template not provided for structure processor!");

                            structureProcessor.Influx = Selector.Create(context, directoryCreator, processor);
                            return structureProcessor;
                        }
                    case ProcessorType.Dict:
                        {
                            var param = processor.Parameters.SingleOrDefault();
                            var dictProcessor = new DictTableProcessor();

                            if (param == null)
                                throw new InvalidOperationException("cshtml template not provided for dictionary processor!");

                            dictProcessor.Influx = Selector.Create(context, directoryCreator, processor);
                            return dictProcessor;
                        }

                    case ProcessorType.ExampleIndex:
                        {
                            var indexProcessor = new ExampleProcessor();
                            var param = processor.Parameters.SingleOrDefault();

                            if (param == null)
                                throw new InvalidOperationException("cshtml template not provided for example index processor!");

                            indexProcessor.Influx = Selector.Create(context, directoryCreator, processor);

                            return indexProcessor;
                        }
                    case ProcessorType.Valueset:
                        {
                            var valueSetProcessor = new Processors.Profile.ValueSetProcessor();
                            var param = processor.Parameters.SingleOrDefault();

                            if (param == null)
                                throw new InvalidOperationException("cshtml tenplate not provided for valueset processor!");

                            valueSetProcessor.Influx = Selector.Create(context, directoryCreator, processor);

                            return valueSetProcessor;
                        }
                    case ProcessorType.Operation:
                        {
                            var param = processor.Parameters.SingleOrDefault();
                            var operationProcessor = new OperationProcessor();

                            if (param == null)
                                throw new InvalidOperationException("cshtml template not provided for operation processor!");

                            operationProcessor.Influx = Selector.Create(context, directoryCreator, processor);

                            return operationProcessor;
                        }
                    case ProcessorType.ValuesetIndex:
                        {
                            var indexProcessor = new ValueSetProcessor();
                            var param = processor.Parameters.SingleOrDefault();

                            if (param == null)
                                throw new InvalidOperationException("cshtml template not provided for valueset index processor!");

                            indexProcessor.Influx = Selector.Create(context, directoryCreator, processor);

                            return indexProcessor;
                        }

                    default:
                        throw new InvalidEnumArgumentException(
                            string.Concat("Unknown processing command: ", command));
                }
            }
            catch
            {
                throw new InvalidOperationException($"Invalid processor statement: {processor}");
            }
        }
    }
}