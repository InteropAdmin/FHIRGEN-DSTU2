using System.Xml.Linq;

namespace Hl7.Fhir.Publication.Specification.HierarchicalTable.Cells.Component
{
	internal abstract class CellComponent
	{
		public abstract XObject ToHtml();
	}
}