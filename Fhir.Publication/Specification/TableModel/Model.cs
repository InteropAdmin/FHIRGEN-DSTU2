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

using System.Collections.Generic;

namespace Hl7.Fhir.Publication.Specification.TableModel
{
    internal class Model
    {
        public readonly List<Title> Titles = new List<Title>();
        public readonly List<Row> Rows = new List<Row>();

        public static Model GetGenericTable()
        {
            var model = new Model();

            model.Titles.Add(new Title(null, null, "Name", "The logical name of the element", null, 0));
            model.Titles.Add(new Title(null, null, "Flags", "Information about the use of the element", null, 0));
            model.Titles.Add(new Title(null, null, "Card.", "Minimum and maximum # of times the element can appear in the instance", null, 0));
            model.Titles.Add(new Title(null, null, "Type", "Reference to the type of the element", null, 100));
            model.Titles.Add(new Title(null, null, "Description & Constraints", null, null, 0));
            return model;
        }

        public static Model GetProfileIndexTable()
        {
            var model = new Model();

            model.Titles.Add(new Title(null, null, "Name", "The logical name of the element", null, 250));
            model.Titles.Add(new Title(null, null, "Type", "Reference to the type of the element", null, 100));
            model.Titles.Add(new Title(null, null, "Description & Constraints", "Additional information about the element", null, 600));
            return model;
        }

        public static Model GetOperationDefinitionTable()
        {
            var model = new Model();

            model.Titles.Add(new Title(null, null, "Name", "The logical name of the element", null, 200));
            model.Titles.Add(new Title(null, null, "Card.", "Minimum and maximum # of times the element can appear in the instance", null, 100));
            model.Titles.Add(new Title(null, null, "Type", "Reference to the type of the element", null, 150));
            model.Titles.Add(new Title(null, null, "Description", "Additional information about the element", null, 500));
            return model;
        }

        public static Model GetOperationDefinitionMetaTable()
        {
            var model = new Model();

            model.Titles.Add(new Title(null, null, "Name", "The logical name of the element", null, 100));
            model.Titles.Add(new Title(null, null, "Type", "Reference to the type of the element", null, 100));
            model.Titles.Add(new Title(null, null, "Value", "Additional information about the element", null, 290));

            return model;
        }

        public static Model GetBindingsTable()
        {
            var model = new Model();

            model.Titles.Add(new Title(null, null, "Path", "The path of the element", null, 100));
            model.Titles.Add(new Title(null, null, "Name", "The name of the valueSet", null, 100));
            model.Titles.Add(new Title(null, null, "Binding Strength", "How closely the valueSet should be followed.", null, 100));
            model.Titles.Add(new Title(null, null, "ValueSet", "A value set specifies a set of codes drawn from one or more code systems.", null, 100));

            return model;
        }
    }
}