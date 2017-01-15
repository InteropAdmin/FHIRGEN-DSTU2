//*
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
//*

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Specification.ExtensionMethods;

namespace Hl7.Fhir.Publication.Specification.Profile
{
    internal class TableFactory
    {
        private readonly KnowledgeProvider _knowledgeProvider;

        public TableFactory(KnowledgeProvider knowledgeProvider)
        {
            if (knowledgeProvider == null)
                throw new ArgumentNullException(
                    nameof(knowledgeProvider));

            _knowledgeProvider = knowledgeProvider;
        }

        private static HierarchicalTable.IImageGenerator CreateImageGenerator(Framework.IDirectoryCreator directoryCreator)
        {
            return new HierarchicalTable.ImageGenerator(directoryCreator);
        }

        public XElement GenerateProfile(
            IEnumerable<StructureDefinition> resources,
            string packageName, 
            Icon icon,
            Framework.IDirectoryCreator directoryCreator)
        {
            TableModel.Model model = TableModel.Model.GetProfileIndexTable();

            GenerateStructureDefinitionsInProfile(model.Rows, resources.ToArray(), icon);

            if (model.Rows.Count == 0)
                throw new InvalidOperationException($"{packageName} failed to generated any rows!");

            var factory = new HierarchicalTable.Factory(CreateImageGenerator(directoryCreator));

            return
                factory.CreateFrom(model).ToHtml();
        }

        public XElement GenerateProfile(
            IEnumerable<OperationDefinition> resources, 
            string packageName, 
            Icon icon,
             Framework.IDirectoryCreator directoryCreator)
        {
            TableModel.Model model = TableModel.Model.GetProfileIndexTable();

            GenerateOperationDefinitionsInProfile(model.Rows, resources.ToArray(), icon);

            if (model.Rows.Count == 0)
                throw new InvalidOperationException($"{packageName} failed to generated any rows!");

            var factory = new HierarchicalTable.Factory(CreateImageGenerator(directoryCreator));
            return
               factory.CreateFrom(model).ToHtml();
        }

        private void GenerateStructureDefinitionsInProfile(
            ICollection<TableModel.Row> rows,
            IEnumerable<StructureDefinition> resources,
            Icon icon)
        {
            foreach (StructureDefinition structureDefinition in resources)
            {
                string structureType = GetBaseType(structureDefinition);

                var type = (FHIRDefinedType)Enum.Parse(typeof(FHIRDefinedType), structureType);

                var resource = new ImplementationGuide.Resource(
                      structureDefinition.Name,
                      structureDefinition.Description,
                      structureDefinition.Url,
                      structureDefinition.Meta,
                      ResourceType.StructureDefinition);

                rows.Add(Row.Create(icon.GetFileName(), resource, structureType, _knowledgeProvider.GetLinkForTypeDocument(type)));
            }
        }

        private static string GetBaseType(StructureDefinition structureDefinition)
        {
            return structureDefinition.Snapshot.Element.First().Path;
        }

        private void GenerateOperationDefinitionsInProfile(
            ICollection<TableModel.Row> rows,
            IEnumerable<OperationDefinition> resources,
            Icon icon)
        {
            foreach (OperationDefinition operationDefinition in resources)
            {
                string operationType = GetBaseType(operationDefinition);

                var resource = new ImplementationGuide.Resource(
                     operationDefinition.Name,
                     operationDefinition.Description,
                     operationDefinition.Url,
                     operationDefinition.Meta,
                     ResourceType.OperationDefinition);

                rows.Add(Row.Create(icon.GetFileName(), resource, operationType, _knowledgeProvider.GetLinkForTypeDocument(FHIRDefinedType.OperationDefinition)));
            }
        }

        private static string GetBaseType(OperationDefinition operationDefinition)
        {
            return operationDefinition.Kind?.ToString();
        }
    }
}