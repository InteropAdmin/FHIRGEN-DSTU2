using System.Collections.Generic;
using System.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Specification.ExtensionMethods;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure
{
    internal static class IconSetter
    {
        private const string _modifierExtension = "modifierExtension";
        private const string _reference = "Reference";
        private const string _extension = "extension";

        public static string GetIcon(
            FHIRDefinedType? typeCode, 
            string elementName, 
            bool hasShortDescription, 
            IReadOnlyCollection<Model.ElementDefinition.TypeRefComponent> elementDefinitionType,
            bool isReferenceToParentType,
            KnowledgeProvider knowledgeProvider)
        {
            if(isReferenceToParentType)
                return Icon.Reuse.GetFileName();

            if (elementName == FHIRDefinedType.Extension.ToString().ToLower())
                return Icon.Extension.GetFileName();

            if (elementName == _modifierExtension)
                return Icon.ModifierExtension.GetFileName();

            //if (!hasShortDescription || elementDefinitionType == null || elementDefinitionType.Count == 0)
            //    return Icon.Extension.GetFileName();

            if (elementDefinitionType.Count > 1)
            {
                return
                    AreAllSameType(
                       elementDefinitionType,
                        _reference)
                        ? Icon.Reference.GetFileName()
                        : Icon.Choice.GetFileName();
            }

            if (IsExtension(elementName))
            {
                return Icon.Extension.GetFileName();
            }


            //if (!hasShortDescription || elementDefinitionType == null || elementDefinitionType.Count == 0)
            //    return Icon.Extension.GetFileName();

            if (typeCode != null && knowledgeProvider.IsPrimitive((FHIRDefinedType)typeCode))
                return Icon.Primitive.GetFileName();

            if (typeCode != null && KnowledgeProvider.IsReference((FHIRDefinedType)typeCode))
                return Icon.Reference.GetFileName();

            if (typeCode != null && knowledgeProvider.IsComplexDataType((FHIRDefinedType)typeCode))
                return Icon.DataType.GetFileName();
            else 
                return Icon.Resource.GetFileName();
        }

        private static bool AreAllSameType(IEnumerable<Model.ElementDefinition.TypeRefComponent> types, string name)
        {
            return types.All(type => type.Code?.ToString() == name);
        }

        private static bool IsExtension(string elementName)
        {
            return elementName == _extension || elementName == _modifierExtension;
        }
    }
}