/*
//Copyright (c) 2011+, HL7, Inc
//All rights reserved.

//Redistribution and use in source and binary forms, with or without modification, 
//are permitted provided that the following conditions are met:

// * Redistributions of source code must retain the above copyright notice, this 
//   list of conditions and the following disclaimer.
// * Redistributions in binary form must reproduce the above copyright notice, 
//   this list of conditions and the following disclaimer in the documentation 
//   and/or other materials provided with the distribution.
// * Neither the name of HL7 nor the names of its contributors may be used to 
//   endorse or promote products derived from this software without specific 
//   prior written permission.

//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
//ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
//WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
//IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
//INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
//NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR 
//PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
//WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
//ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
//POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;
using Hl7.Fhir.Publication.Specification.ExtensionMethods;
using Hl7.Fhir.Support;
using OperationDefinition = Hl7.Fhir.Model.OperationDefinition;
using StructureDefinition = Hl7.Fhir.Model.StructureDefinition;

namespace Hl7.Fhir.Publication.Specification.Profile.Example.Index
{
    internal class Factory
    {
        private readonly Log _log;
        private readonly IDirectoryCreator _directoryCreator;
        private XElement _xhtml;
        private ImplementationGuide.Base _baseResource;
        private string _packageName;

        public Factory(
            KnowledgeProvider profileKnowledgeProvider, 
            Log log, 
            IDirectoryCreator directoryCreator)
        {
            if (profileKnowledgeProvider == null)
                throw new ArgumentNullException(
                    nameof(profileKnowledgeProvider));

            if (log == null)
                throw new ArgumentNullException(
                    nameof(log));

            if (directoryCreator == null)
                throw new ArgumentNullException(
                    nameof(directoryCreator));
            _log = log;
            _directoryCreator = directoryCreator;
        }

        private XElement Package => new XElement(
                XmlNs.XHTMLNS + "div",
                new XAttribute("class", "package"),
                new XElement(XmlNs.XHTMLNS + "br"),
                new XElement(XmlNs.XHTMLNS + "h3", _packageName));

        public XElement Generate(
            string targetDirectory, 
            ImplementationGuide.Package package, 
            ImplementationGuide.Base baseResource)
        {
            _baseResource = baseResource;
            _packageName = package.Name.Split('.').Last();

            var generator = new ResourceGenerator(_directoryCreator, baseResource.ExamplesXml, baseResource.ExamplesJson, _log);

            _xhtml = Package;

            GetOperationExamples(package, generator, targetDirectory);

            GetStructureExamples(package, generator, targetDirectory);

            return _xhtml;
        }

        private void GetOperationExamples(ImplementationGuide.Package package, ResourceGenerator generator, string dir)
        {
            foreach (OperationDefinition operationDefintion in package.OperationDefinitions.OrderBy(GetOrder))
            {
                XElement table = Table.ToHtml(operationDefintion.Name, package.Name, _baseResource, operationDefintion.Meta?.Tag);

                if (table != null)
                    _xhtml.Add(table);

                IEnumerable<Coding> items = GetMetaData(operationDefintion.Meta?.Tag);

                if (items != null)
                    foreach (Coding item in items)
                    {
                        string source = Path.Combine(dir, Page.Content.Example.GetPath(), package.Name);

                        generator.Generate(item.Code, source);
                    }
            }
        }

        private void GetStructureExamples(ImplementationGuide.Package package, ResourceGenerator generator, string dir)
        {
            foreach (StructureDefinition structureDefinition in package.StructureDefinitions.OrderBy(GetOrder))
            {
                XElement table = Table.ToHtml(structureDefinition.Name, package.Name, _baseResource, structureDefinition.Meta?.Tag);

                if (table != null)
                    _xhtml.Add(table);

                IEnumerable<Coding> items = GetMetaData(structureDefinition.Meta?.Tag);

                if (items != null)
                    foreach (Coding item in items)
                    {
                        string source = Path.Combine(dir, Page.Content.Example.GetPath(), package.Name);

                        generator.Generate(item.Code, source);
                    }
            }
        }

        private static int GetOrder(Resource profile)
        {
            int order = 0;
            bool result = false;

            if (profile.Meta != null)
            {
                result = int.TryParse(
                profile.Meta.Tag.SingleOrDefault(
                    coding =>
                        coding.System == Urn.PublishOrder.GetUrnString())?.Code,
                out order);
            }

            return result ? order : 0;
        }

        private static IEnumerable<Coding> GetMetaData(IEnumerable<Coding> metaTags)
        {
            return
                metaTags.Where(
                    item =>
                        item.System == Urn.Example.GetUrnString())
                    .ToList();
        }
    }
}