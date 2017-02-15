/*
Copyright (c) 2011+, HL7, Inc
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, 
are permitted provided that the following conditions are met:

 * Redistributions of source code must retain the above copyright notice, this 
   list of conditions and the following disclaimer.
 * Redistributions in binary form must reproduce the above copyright notice, 
   this list of conditions and the following disclaimer in the documentation 
   and/or other materials provided with the distribution.
 * Neither the name of HL7 nor the names of its contributors may be used to 
   endorse or promote products derived from this software without specific 
   prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR 
PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
POSSIBILITY OF SUCH DAMAGE.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Support;
using System.Diagnostics;

namespace Hl7.Fhir.Publication.Specification.Profile.Structure.Dictionary
{
    internal class HtmlGenerator
    {
        private readonly KnowledgeProvider _knowledgeProvider;
        private StringBuilder _xhtml;
        private ImplementationGuide.ResourceStore _resources;
        private string _package;
        
        public HtmlGenerator(KnowledgeProvider knowledgeProvider)
        {
            if (knowledgeProvider == null)
                throw new ArgumentNullException(
                    nameof(knowledgeProvider));

            _knowledgeProvider = knowledgeProvider;
            _xhtml = new StringBuilder();
        }

        public XElement Generate(StructureDefinition structure, ImplementationGuide.ResourceStore resources, string package)
        {
            _resources = resources;
            _package = package;

            _xhtml = new StringBuilder();
            Write(string.Concat("<div xmlns=\"", XmlNs.XHTML, "\">"));

            GenerateStructure(structure);

            Write("</div>");

            return XElement.Parse(_xhtml.ToString());
        }

        private void Write(string item)
        {
            _xhtml.Append(item);
        }

        private void GenerateStructure(StructureDefinition structureDefinition)
        {
            Debug.Print(structureDefinition.Name);
            Write(string.Concat("<p><b>", structureDefinition.Name, "</b></p>\r\n"));
            Write("<table class=\"dict\">\r\n");

            ElementDefinition[] differential = structureDefinition.Differential.Element.ToArray();

            string slicedElementPath = string.Empty;

            foreach (ElementDefinition snapshotElement in structureDefinition.Snapshot.Element)
            {
                Debug.Print("GenerateStructure - "+snapshotElement.Path);

                var ig01 = snapshotElement.Representation.Any();

                if (snapshotElement.Representation != null && !snapshotElement.Representation.Any())
                {
                    if (snapshotElement.Slicing != null)
                        slicedElementPath = snapshotElement.Path;
                    else if (!snapshotElement.Path.StartsWith(slicedElementPath) || slicedElementPath == snapshotElement.Path)
                        slicedElementPath = string.Empty;

                    ElementDefinition differentialElement = Array.Find(
                        differential,
                        element =>
                            element.Path == snapshotElement.Path
                            &&
                            element.Name == snapshotElement.Name);

                    if (!HasZeroCardinality(snapshotElement, differential)
                        &&
                        slicedElementPath == string.Empty)
                        GenerateElement(
                            differentialElement ?? snapshotElement,
                            structureDefinition.Name);
                }
                else
                {
                    Debug.Print("Ignore Representation element");
                }     
            }

            Write("</table>\r\n");
        }

        private static bool HasZeroCardinality(ElementDefinition snapshotElement, IEnumerable<ElementDefinition> differentialElements)
        {
            return differentialElements
                .Any(differentialElement =>
                    snapshotElement.Path.StartsWith(differentialElement.Path)
                    &&
                    differentialElement.Max == "0");
        }

        private void GenerateElement(ElementDefinition elementDefinition,  string resourceName)
        {          
            var name = KnowledgeProvider.MakeElementDictAnchor(elementDefinition);

            var title = string.Concat(
                elementDefinition.Path,
                (elementDefinition.Name == null
                    ? string.Empty
                    : string.Concat("(", elementDefinition.Name, ")")));

            Write(string.Concat("  <tr><td colspan=\"2\" class=\"structure\"><a name=\"", name, "\"> </a><b>", title, "</b></td></tr>\r\n"));

            Write(
                new ElementTable(_resources, elementDefinition, resourceName, _package, _knowledgeProvider)
                    .Generate());

        }
    }
}