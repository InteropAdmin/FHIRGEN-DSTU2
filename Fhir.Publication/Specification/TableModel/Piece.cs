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

namespace Hl7.Fhir.Publication.Specification.TableModel
{
    internal class Piece
    {
        private readonly string _tag;
        private readonly string _label;
        private readonly string _text;
        private readonly string _hint;
        private readonly string _reference;
        private string _style;

        public Piece(string tag)
        {
            _tag = tag;
        }

        public Piece(string label, string text)
        {
            _label = label;
            _text = text;
        }

        public Piece(string reference, string text, string hint)
        {
            _reference = reference;
            _text = text;
            _hint = hint;
        }

        public string GetReference()
        {
            return _reference;
        }

        public string GetText()
        {
            return _text;
        }

        public string GetHint()
        {
            return _hint;
        }

        public string GetTag()
        {
            return _tag;
        }

        public string GetStyle()
        {
            return _style;
        }

        public string GetLabel()
        {
            return _label;
        }

        public Piece AddStyle(string style)
        {
            if (_style != null)
                _style = _style + ": " + style;
            else
                _style = style;
            return this;
        }
    }
}
