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
using System.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Publication.Framework;
using Hl7.Fhir.Publication.Framework.ExtensionMethods;
using Hl7.Fhir.Model;
using Hl7.Fhir.Publication.Specification.Profile.Structure.Rows;
using Hl7.Fhir.Specification.Navigation;
using StructureDefinition = Hl7.Fhir.Model.StructureDefinition;


namespace Hl7.Fhir.Publication.Specification.Profile.Structure
{
    internal class Factory
    {
        private const string _extension = "extension";
        private const string _modifierExtension = "modifierExtension";
        private readonly KnowledgeProvider _knowledgeProvider;
        private readonly ImplementationGuide.Package _package;
        private readonly Log _log;
        private readonly IDirectoryCreator _directoryCreator;
        private bool _hasShortDescription;
        private string _elementName;

        internal Factory(
            KnowledgeProvider profileKnowledgeProvider,
            ImplementationGuide.Package package,
            Log log,
            IDirectoryCreator directoryCreator)
        {
            if (profileKnowledgeProvider == null)
                throw new ArgumentNullException(
                    nameof(profileKnowledgeProvider));

            if (package == null)
                throw new ArgumentNullException(
                    nameof(package));

            if (log == null)
                throw new ArgumentNullException(
                    nameof(log));

            if (directoryCreator == null)
                throw new ArgumentNullException(
                    nameof(directoryCreator));

            _knowledgeProvider = profileKnowledgeProvider;
            _package = package;
            _log = log;
            _directoryCreator = directoryCreator;
        }

        public XElement GenerateStructure(StructureDefinition structure, bool isDifferential)
        {
            _log.Info(string.Concat("Creating structure for ", structure.Name));

            TableModel.Model model = TableModel.Model.GetGenericTable();

            var navigator = new ElementNavigator(structure.Snapshot.Element);

            navigator.MoveToFirstChild();

            GetElementRow(model.Rows, navigator, structure, isDifferential);

            var factory = new HierarchicalTable.Factory(new HierarchicalTable.ImageGenerator(_directoryCreator));

            return factory.CreateFrom(model).ToHtml();
        }

        private void GetElementRow(
            ICollection<TableModel.Row> rows,
            ElementNavigator navigator,
            StructureDefinition profile,
            bool showMissing)
        {
            ElementDefinition elementDefinition = navigator.Current;

            var path = elementDefinition.Path.Split('.').Last();
            var filtered = IsFiltered(elementDefinition);

            if (!filtered)
            {
                if (path == _extension
                    || path == _modifierExtension
                    || !Cardinality.IsZeroCardinality(elementDefinition.MaxElement.ToString()))
                {
                    CreateRow(elementDefinition, profile, showMissing, rows, navigator);
                }
            }
        }



        private void CreateRow(
            ElementDefinition elementDefinition,
            StructureDefinition profile,
            bool showMissing,
            ICollection<TableModel.Row> rows,
            ElementNavigator navigator)
        {
            var row = new TableModel.Row();

            var root = elementDefinition.Path.Split('.').First();

            _log.Info(string.Concat("CreateRow " + elementDefinition.Path + "( " +root + " )"));

            row.SetAnchor(elementDefinition.Path);

            _elementName = elementDefinition.GetNameFromPath();

            _hasShortDescription = elementDefinition.Short != null;

            if (_elementName == _extension)
                SetElementNameIfModifer(elementDefinition);

            row = SetIcon(_elementName, row, elementDefinition.Type, elementDefinition.NameReferenceElement);

            var dictionaryLink = KnowledgeProvider.GetLinkForElementInDictionary(profile.Name, elementDefinition);

            if (IsExtension(_elementName))
            {
                if (elementDefinition.Name != null)
                {
                    if (root == "Extension")
                    {
                        var elementRow = new ElementRow(
                                        _knowledgeProvider,
                                        elementDefinition,
                                        dictionaryLink,
                                        row,
                                        _package,
                                        _elementName,
                                        profile.Name,
                                        profile.ConstrainedType);

                        rows.Add(elementRow.Value);

                        bool isSlice = elementDefinition.Slicing != null;

                        if (!isSlice)
                        {
                            if (navigator.MoveToFirstChild())
                            {
                                do
                                {
                                    GetElementRow(row.GetSubRows(), navigator, profile, showMissing);
                                }
                                while (navigator.MoveToNext());

                                navigator.MoveToParent();
                            }
                        }
                    }
                    else
                    {
                        var extensionRow = new ExtensionRow(
                            _package,
                            _knowledgeProvider,
                            row,
                            dictionaryLink,
                            elementDefinition,
                            profile.Name);

                        rows.Add(extensionRow.Value);
                    }
                }

            }
            else if (IsReferenceToParent(elementDefinition.NameReferenceElement))
            {
                var referenceRow = new ReferenceToParentRow(
                        _elementName,
                        _package,
                        _knowledgeProvider,
                        row,
                        dictionaryLink,
                        elementDefinition);

                rows.Add(referenceRow.Value);
            }
            else
            {
                var elementRow = new ElementRow(
                    _knowledgeProvider,
                    elementDefinition,
                    dictionaryLink,
                    row,
                    _package,
                    _elementName,
                    profile.Name,
                    profile.ConstrainedType);

                rows.Add(elementRow.Value);

                bool isSlice = elementDefinition.Slicing != null;

                if (!isSlice)
                {
                    if (navigator.MoveToFirstChild())
                    {
                        do
                        {
                            GetElementRow(row.GetSubRows(), navigator, profile, showMissing);
                        }
                        while (navigator.MoveToNext());

                        navigator.MoveToParent();
                    }
                }

            }
            //_log.Info(string.Concat("CreateRow - END - " + elementDefinition.Path));
        }

        private void SetElementNameIfModifer(ElementDefinition elementDefinition)
        {
            var extensionUrl = elementDefinition.Type.SingleOrDefault()?.Profile.SingleOrDefault();

            if (extensionUrl != null)
            {
                StructureDefinition resource = _package.ResourceStore.GetStructureDefinitionByUrl(extensionUrl, _package.Name);

                if (resource != null && resource.IsModifierExtension())
                    _elementName = _modifierExtension;
            }
        }

        private static bool IsFiltered(ElementDefinition elementDefinition)
        {
            bool result = false;

            if ((elementDefinition.RepresentationElement != null) && (elementDefinition.RepresentationElement.Count > 0) )
            {
                var attr = ( elementDefinition.RepresentationElement[0].Value == ElementDefinition.PropertyRepresentation.XmlAttr);
                var last = elementDefinition.Path.Split('.').Last();

                if (attr && last=="id")
                {
                    result = true;
                }
            }

            return result;
        }

        private static bool IsExtension(string elementName)
        {
            return elementName == _extension || elementName == _modifierExtension;
        }

        private static bool IsReferenceToParent(FhirString nameReference)
        {
            return nameReference?.Value != null;
        }

        private TableModel.Row SetIcon(
            string elementName,
            TableModel.Row row,
            IReadOnlyList<ElementDefinition.TypeRefComponent> elementDefinitionType,
            FhirString nameReference)
        {
            if (elementDefinitionType.Count > 0 || IsExtension(elementName) || nameReference.Value != null)
            {
                FHIRDefinedType? typeCode = null;
                bool isReferenceToParentType = false;

                if (nameReference?.Value != null)
                    isReferenceToParentType = true;
                else if (elementDefinitionType.Count > 0)
                    typeCode = elementDefinitionType[0].Code;

                row.SetIcon(IconSetter.GetIcon(typeCode, elementName, _hasShortDescription, elementDefinitionType, isReferenceToParentType, _knowledgeProvider));
            }
            return row;
        }
    }
}