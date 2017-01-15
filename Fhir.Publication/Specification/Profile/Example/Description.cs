namespace Hl7.Fhir.Publication.Specification.Profile.Example
{
    internal class Description
    {
        public Description(string name, string display)
        {
            Name = name;
            Display = display;
        }

        public string Name { get; private set; }

        public string Display { get; private set; }
    }
}