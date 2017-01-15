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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using Hl7.Fhir.Publication.Framework;

namespace Hl7.Fhir.Publication.Specification.HierarchicalTable
{
    internal class ImageGenerator : IImageGenerator
    {
        private const int _width = 400;
        private const int _height = 2;
        private const string _imageName = "tbl_bck";
        private readonly IDirectoryCreator _directoryCreator;

        public ImageGenerator(IDirectoryCreator directoryCreator)
        {
            if (directoryCreator == null)
                throw new ArgumentNullException(
                    nameof(directoryCreator));

            _directoryCreator = directoryCreator;
        }

        public string Generate(bool hasChildren, IReadOnlyList<bool> indents)
        {
            SeparatorImage image = GenerateSeparatorImage(hasChildren, indents);

            var relativeImagePath = Path.Combine(Profile.KnowledgeProvider.RelativeGeneratedImagesPath, image.Filename);

            if (!_directoryCreator.DirectoryExists(Profile.KnowledgeProvider.RelativeGeneratedImagesPath))
                _directoryCreator.CreateDirectory(Profile.KnowledgeProvider.RelativeGeneratedImagesPath);

            if (!_directoryCreator.FileExists(relativeImagePath))
            {
                using (FileStream stream = _directoryCreator.GetFileStream(relativeImagePath, FileMode.Create))
                {
                    image.Bitmap.Save(stream, ImageFormat.Png);
                }
            }

            return image.Filename;
        }

        private static SeparatorImage GenerateSeparatorImage(bool hasChildren, IReadOnlyList<bool> indents)
        {
            var stringBuilder = new StringBuilder(_imageName);

            var bitmap = new Bitmap(_width, _height);

            Graphics graphics = Graphics.FromImage(bitmap);

            graphics.DrawRectangle(
                pen: new Pen(Color.White),
                x: 0,
                y: 0,
                width: _width,
                height: _height);

            for (int i = 0; i < indents.Count; i++)
            {
                stringBuilder.Append(indents[i] ? "0" : "1");

                if (!indents[i])
                    bitmap.SetPixel(
                        x: 12 + (i * 16),
                        y: 0,
                        color: Color.Black);
            }

            if (hasChildren)
            {
                bitmap.SetPixel(
                    x: 12 + (indents.Count * 16),
                    y: 0,
                    color: Color.Black);

                stringBuilder.Append("1");
            }
            else
            {
                stringBuilder = new StringBuilder(RemoveTrailingZeros(stringBuilder.ToString()));
            }

            stringBuilder.Append(".png");

            return new SeparatorImage(bitmap, stringBuilder.ToString());      
        }

        private static string RemoveTrailingZeros(string fileName)
        {
            int lastZero = fileName.LastIndexOf("1", StringComparison.Ordinal);
            if (lastZero == -1)
                lastZero = 6;

            return lastZero < fileName.Length - 1 ? new StringBuilder(fileName.Substring(0, lastZero + 1)).ToString() : fileName;
        }
    }
}