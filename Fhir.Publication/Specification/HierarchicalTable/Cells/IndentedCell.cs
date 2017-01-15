using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hl7.Fhir.Publication.Specification.TableModel;

namespace Hl7.Fhir.Publication.Specification.HierarchicalTable.Cells
{
	internal class IndentedCell : LeadingCell
	{
		private readonly bool _hasChildren;
		private readonly IReadOnlyList<bool> _indents;
        private readonly IImageGenerator _imageGenerator;
        
        public IndentedCell(
			IReadOnlyList<bool> indents, 
			bool hasChildren,
			string icon,
			string anchor,
			IEnumerable<Piece> pieces,
            IImageGenerator imageGenerator)
			: base(icon, anchor, pieces)
		{
			_hasChildren = hasChildren;
			_indents = indents;
            _imageGenerator = imageGenerator;
        }

        protected override string Style => string.Concat(
            base.Style,
            "; white-space: nowrap; background-image: url(",
            GenerateImage(_indents.ToArray(), _hasChildren),
            "); background-repeat: repeat-y; ");

	    protected override IEnumerable<Component.CellComponent> CreateCellComponents()
		{
			yield return new Component.Image("tbl_spacer.png");	

			for (int i = _indents.Count - 1; i >= 0; i--)
            {
                if (_indents[i] && i > 0 ) 
                {
                    yield return new Component.Image("tbl_blank.png");
                }
                else if (_indents[i])
                {
                    yield return new Component.Image("tbl_vjoin_end.png");
                }
               else if (i == 0) 
				{
                    yield return new Component.Image("tbl_vjoin.png");
                }
                else
				{
					yield return new Component.Image("tbl_vline.png");
				}
			}

			foreach (Component.CellComponent cellComponent in base.CreateCellComponents())
				yield return cellComponent;
		}

        private string GenerateImage(IEnumerable<bool> indents, bool hasChildren)
		{
			return _imageGenerator != null
                ? Path.Combine(
                    Profile.KnowledgeProvider.RelativeImagesPath,
                    _imageGenerator.Generate(hasChildren, indents.Reverse().ToArray()))
                : string.Empty;
		}
    }
}