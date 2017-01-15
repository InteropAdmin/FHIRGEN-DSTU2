namespace Hl7.Fhir.Publication.ImplementationGuide
{
    internal interface IResource
    {
        string Name { get; }
        string Description { get; }
        string Url { get;  }
        Model.Meta Meta { get; } 
        Model.ResourceType Type {get;}
    }
}