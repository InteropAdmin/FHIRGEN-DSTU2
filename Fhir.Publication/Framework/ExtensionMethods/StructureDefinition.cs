using System.Collections.Generic;
using System.Linq;
using Hl7.Fhir.Model;

namespace Hl7.Fhir.Publication.Framework.ExtensionMethods
{
    internal static class StructureDefinition
    {
        public static string GetExampleName(this Model.StructureDefinition definition)
        {
            List<Coding> tag = definition.Meta.Tag;

            foreach (Coding item in tag.Where(
                item => item.System == Urn.Example.GetUrnString()))
            {
                return item.Display;
            }

            return string.Empty;
        }

        public static bool IsExtension(this Model.StructureDefinition definition)
        {
            return
                definition.Snapshot != null
                &&
                definition.Snapshot.Element.Any(
                    element =>
                        element.Path == Model.StructureDefinition.ExtensionContext.Extension.ToString());
        }

        public static bool IsModifierExtension(this Model.StructureDefinition definition)
        {
            return
                definition.Snapshot != null
                &&
                definition.Snapshot.Element.Any(
                    element =>
                        element.Path == Model.StructureDefinition.ExtensionContext.Extension.ToString()
                        && element.IsModifier == true);

        }

        public static bool IsSnapshot(this Model.StructureDefinition definition)
        {
            return
                  definition.Snapshot != null;
        }

        public static bool HasExtensions(this Model.StructureDefinition definition)
        {
            return
                definition.ConstrainedType != FHIRDefinedType.Extension
                &&
                definition.Differential != null
                &&
                definition.Differential.Element.Any(
                    element =>
                        element.Type.Any(
                            type => 
                                type.Code?.ToString() == Model.StructureDefinition.ExtensionContext.Extension.ToString()));
        }

        public static IEnumerable<string> GetExtensions(this Model.StructureDefinition definition)
        {
            IEnumerable<string> extensions =
                definition.Differential?.Element
                    .Where(
                        element =>
                            element.Type.Any(
                                type =>
                                    type.Code?.ToString() == Model.StructureDefinition.ExtensionContext.Extension.ToString()))
                    .Select(
                        p =>
                            p.Type.SingleOrDefault()
                                ?.Profile.SingleOrDefault()
                                ?.ToString())
                    .Where(
                        ext => 
                            ext != null);

            return extensions;
        }
    }
}
