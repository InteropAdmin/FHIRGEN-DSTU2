using System;
using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;
using Hl7.Fhir.Model;

namespace Hl7.Fhir.Publication.Specification.Profile
{
    internal class KnowledgeProvider
    {
        private const string _html = @".html";
        private const string _dictionaryExtension = "-dict.html";
        const string _primitive = "-primitive";
        private readonly XNamespace _xmlXs = @"http://www.w3.org/2001/XMLSchema";
        private readonly Log _log;
        private readonly XDocument _xDoc;

        public KnowledgeProvider(Log log)
        {
            if (log == null)
                throw new ArgumentNullException(
                    nameof(log));

            _log = log;

            _xDoc = XDocument.Parse(Resources.fhir_base);
        }

        public static string RelativeImagesPath => "../dist/images/";
        public static string RelativeGeneratedImagesPath => "../Generated/dist/images/";
        public static string ExampleLinkPath => @"\examples\";
        public static string ResourceIndex => string.Concat(Url.Dstu.GetUrlString(), "resourcelist.html");
        public static string OpenTypeElement => string.Concat(Url.Dstu.GetUrlString(), "datatypes.html#open");
        public static string OperationDefintionKindPath => string.Concat(Url.Dstu.GetUrlString(), "valueset-operation-kind.html");
        public static string ReferenceResourcePath => string.Concat(Url.Dstu.GetUrlString(), "references.html");

        private static string BackboneElementPath => string.Concat(Url.Dstu.GetUrlString(),"backboneelement.html");
        private static string OperationDefintionPath => string.Concat(Url.Dstu.GetUrlString(), "operationdefinition.html");
        private static string NarrativeTypePath => string.Concat(Url.Dstu.GetUrlString(), "narrative.html#Narrative");
        private static string XhtmlTypePath => string.Concat(Url.Dstu.GetUrlString(), "narrative.html#xhtml");
        private static string TerminologiesPath => string.Concat(Url.Dstu.GetUrlString(), "terminologies.html");
        
        public static string ExamplesPageLink(string resourceName)
        {
            return $@"<a href=""Examples.html#{resourceName}"">Examples</a>";
        }
        
        public string GetLinkForTypeDocument(FHIRDefinedType typeName)
        {
            if (typeName == FHIRDefinedType.BackboneElement)
                return BackboneElementPath;
            else if (typeName == FHIRDefinedType.Narrative)
                return NarrativeTypePath;
            else if (typeName == FHIRDefinedType.Xhtml)
                return XhtmlTypePath;
            if (IsComplexDataType(typeName) || IsPrimitive(typeName))
                return string.Concat(Url.Dstu.GetUrlString(), "datatypes.html#", typeName.ToString().ToLower());
            else if (typeName == FHIRDefinedType.OperationDefinition)
                return OperationDefintionPath;
            else if (ModelInfo.IsKnownResource(typeName.ToString()))
                return string.Concat(Url.Dstu.GetUrlString(), typeName.ToString().ToLower(), _html);
            else if (typeName == FHIRDefinedType.Extension)
                return string.Concat(Url.Dstu.GetUrlString(), "extensibility.html#Extension");
            else if (typeName == FHIRDefinedType.Resource)
                return string.Concat(Url.Dstu.GetUrlString(), "#");
            else
                throw new InvalidOperationException(string.Concat("Don't know how to link to specification page for type ", typeName));
        }

        public bool HasLinkForTypeDocu(FHIRDefinedType typename)
        {
            string type = typename.ToString();
            return
                type == "*" 
                || IsComplexDataType(typename)
                || IsPrimitive(typename) 
                || type == FHIRDefinedType.Extension.ToString()
                || ModelInfo.IsKnownResource(type);
        }

        public bool IsComplexDataType(FHIRDefinedType dataType)
        {
            return _xDoc.Descendants(_xmlXs + "complexType")
                  .Select(
                      element =>
                        element.Attribute("name")?.Value.ToLower())
                  .ToList()
                  .Contains(dataType.ToString().ToLower());
        }

        public static bool IsReference(FHIRDefinedType type)
        {
            return type == FHIRDefinedType.Reference;
        }

        public bool IsPrimitive(FHIRDefinedType dataType)
        {
            return _xDoc.Descendants(_xmlXs + "simpleType")
                .Where(
                    element =>
                        element.Attribute("name")
                        ?.Value
                        .Contains(_primitive) ?? false)
                    .Select(
                        typeName =>
                            RemoveSuffix(typeName.Attribute("name").Value))
                    .ToList().Contains(dataType.ToString().ToLower());
        }

        private static string RemoveSuffix(string typeName)
        {
            return
                typeName.IndexOf(_primitive, StringComparison.Ordinal) > 0
                ? typeName.Substring(0, typeName.IndexOf(_primitive, StringComparison.Ordinal))
                : typeName;
        }

        public string GetLinkForProfileReference(string profileName, string resourceName)
        {
            if (resourceName.StartsWith(Url.Hl7StructureDefintion.GetUrlString()))
            {
                string name = resourceName.Substring(Url.Hl7StructureDefintion.GetUrlString().Length);
                var type = (FHIRDefinedType)Enum.Parse(typeof(FHIRDefinedType), name);

                return GetLinkForTypeDocument(type);
            }
                
            if (resourceName.StartsWith(Url.FhirSystemPrefix.GetUrlString()))
                return GetLinkForLocalResource(profileName);

            return
                resourceName.StartsWith("#")
                    ? GetLinkForLocalResource(profileName)
                    : resourceName;
        }

        public static string GetLabelForProfileReference(string referenceName)
        {
            string prefixRemoved = referenceName;

            if (referenceName.StartsWith(Url.Hl7StructureDefintion.GetUrlString()))
                return referenceName.Split('/').Last();

            if (referenceName.StartsWith(Url.FhirSystemPrefix.GetUrlString()))
                return referenceName.TrimStart(Url.FhirSystemPrefix.GetUrlString().ToCharArray());

            if (referenceName.StartsWith(Url.FhirStructureDefintion.GetUrlString()))
                return referenceName.TrimStart(Url.FhirStructureDefintion.GetUrlString().ToCharArray());

            return referenceName.StartsWith(Url.FhirPrefix.GetUrlString()) 
                ? referenceName.TrimStart(Url.FhirPrefix.GetUrlString().ToCharArray()) 
                : prefixRemoved;
        }

        public static string GetLinkForElementInDictionary(string profileName, ElementDefinition element)
        {
            return string.Concat(GetLinkForProfileDict(profileName), "#",  MakeElementDictAnchor(element));
        }

        public static string GetLinkForLocalResource(string name)
        {
            return string.Concat(TokenizeName(name).ToLower(), _html);
        }

        public static string GetLinkForProfileDict(string profileName)
        {
            return string.Concat(TokenizeName(profileName).ToLower(), _dictionaryExtension);
        }

        public static string MakeElementDictAnchor(ElementDefinition element)
        {
            if (element.Name == null)
                return element.Path;

            return !element.Path.Contains(".") 
                ? element.Name 
                : string.Concat(element.Path.Substring(0, element.Path.LastIndexOf(".", StringComparison.Ordinal)), ".", element.Name);
        }

        public static string TokenizeName(string name)
        {
            var builder = new System.Text.StringBuilder();

            foreach (char c in name)
            {
                if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == '-' || c == '_')
                    builder.Append(c);
                else if (c != ' ')
                    builder.Append(string.Concat(".", c));
            }

            return builder.ToString();
        }

        public string GetReferenceForBinding(ElementDefinition.BindingComponent binding)
        {
            if (binding.ValueSet == null)
                return null;

            var reference = binding.ValueSet.TypeName == FHIRDefinedType.Reference.ToString()
                ? ((ResourceReference)binding.ValueSet).Reference 
                : ((FhirUri)binding.ValueSet).Value;

            reference = reference.Trim();

            _log.Info(string.Concat(FHIRDefinedType.Reference.ToString(), ": ",reference));
            return reference;
        }

        public static string GetSpecLink(string version)
        {
            return string.Concat(Url.Dstu.GetUrlString(), version);
        }

        public static string RemovePrefix(string typeName)
        {
            if (!typeName.StartsWith(Url.FhirPrefix.GetUrlString())
                && !typeName.StartsWith(Url.Hl7StructureDefintion.GetUrlString()))
                throw new InvalidOperationException(
                    $" {typeName} does not begin {Url.FhirPrefix.GetUrlString()} or {Url.Hl7StructureDefintion.GetUrlString()}!");
            else
                return typeName.Split('/').Last();
        }

        public static string GetBindingStrengthLink(BindingStrength strength)
        {
            return string.Concat(TerminologiesPath, "#", strength.ToString().ToLower());
        }

        public static bool IsRelativeLink(string url)
        {
            return url.StartsWith(Url.FhirValueSet.GetUrlString());
        }           
    }
}