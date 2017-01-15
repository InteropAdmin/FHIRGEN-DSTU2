using System.Collections.Generic;
using Hl7.Fhir.Publication.Specification.Page;
using Hl7.Fhir.Publication.Specification.TableModel;

namespace Hl7.Fhir.Publication.Specification.HierarchicalTable
{
	internal class Factory
	{
        private readonly IImageGenerator _imageGenerator;

	    public Factory(IImageGenerator imageGenerator)
	    {
            _imageGenerator = imageGenerator;
        }

		public Table CreateFrom(TableModel.Model model)
		{
            ModelValidator.Validate(model);

            return new Table(CreateRows(model));
		}

		private IEnumerable<Rows.Row> CreateRows(TableModel.Model model)
		{
			var rows = new List<Rows.Row>();

			rows.Add(new Rows.TitleRow(model.Titles));

			foreach (Row row in model.Rows)
			{
				AddBodyRow(row, rows, new Stack<bool>());
			}

			return rows;
		}

		private void AddBodyRow(
			Row row, 
			ICollection<Rows.Row> rows,
			Stack<bool> indents)
		{
			rows.Add(new Rows.BodyRow(row, indents.ToArray(), _imageGenerator));

			List<Row> subRows = row.GetSubRows();
			int totalSubRows = subRows.Count;
			for(int i=0; i < totalSubRows; i++)
			{
				indents.Push(i == totalSubRows - 1);
				AddBodyRow(subRows[i], rows, indents);
				indents.Pop();
			}
		}
	}
}