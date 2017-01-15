namespace Hl7.Fhir.Publication.Specification.ExtensionMethods
{
    internal static class BindingStrength
    {
        public static string GetText(this Model.BindingStrength? bindingStrength)
        {
            if (bindingStrength == null)
                return "For codes, see ";

            switch (bindingStrength)
            {
                case Model.BindingStrength.Example:
                    return "For example codes, see ";
                case Model.BindingStrength.Preferred:
                    return "The codes SHOULD be taken from ";
                case Model.BindingStrength.Required:
                    return "The codes SHALL be taken from ";
                case Model.BindingStrength.Extensible:
                    return "The codes SHALL be taken from the following if appropriate, otherwise an alternate coding may be included instead. ";
                default:
                    throw new System.ComponentModel.InvalidEnumArgumentException();
            }
        }
    }
}