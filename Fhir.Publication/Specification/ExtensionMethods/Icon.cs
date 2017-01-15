using System.ComponentModel;

namespace Hl7.Fhir.Publication.Specification.ExtensionMethods
{
    internal static class Icon
    {
        public static string GetFileName(this Profile.Icon icon)
        {
            switch (icon)
            {
                case Profile.Icon.Profile:
                    return "icon_profile.png";
                case Profile.Icon.Extension:
                    return "icon_extension_simple.png";
                case Profile.Icon.ModifierExtension:
                    return "icon_modifierExtension.png";
                case Profile.Icon.Element:
                    return "icon_element.gif";
                case Profile.Icon.Reference:
                    return "icon_reference.png";
                case Profile.Icon.Choice:
                    return "icon_choice.gif";
                case Profile.Icon.Reuse:
                    return "icon_reuse.png";
                case Profile.Icon.Primitive:
                    return "icon_primitive.png";
                case Profile.Icon.DataType:
                    return "icon_datatype.gif";
                case Profile.Icon.Resource:
                    return "icon_resource.png";
                default:
                    throw new InvalidEnumArgumentException($" {icon} is not a supported icon!");
            }
        }
    }
}