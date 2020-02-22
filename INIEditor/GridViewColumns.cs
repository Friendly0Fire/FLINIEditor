using System.Windows.Forms;

namespace INIEditor
{
	public class GridViewColumns
	{
		public static void BlockColumns(DataGridView view)
		{
			view.Columns.Clear();
			view.AutoGenerateColumns = false;
			
			DataGridViewTextBoxColumn type = new DataGridViewTextBoxColumn();
			type.DataPropertyName = "Type";
			type.HeaderText = "Type";
            view.Columns.Add(type);

            DataGridViewTextBoxColumn name = new DataGridViewTextBoxColumn();
            name.DataPropertyName = "Name";
            name.HeaderText = "Name";
            view.Columns.Add(name);

            DataGridViewTextBoxColumn hash = new DataGridViewTextBoxColumn();
            hash.DataPropertyName = "Hash";
            hash.HeaderText = "Hash";
            view.Columns.Add(hash);

            DataGridViewTextBoxColumn crc = new DataGridViewTextBoxColumn();
            crc.DataPropertyName = "Crc";
            crc.HeaderText = "Crc";
            view.Columns.Add(crc);

			DataGridViewTextBoxColumn refs = new DataGridViewTextBoxColumn();
			refs.DataPropertyName = "ReferencesCount";
			refs.HeaderText = "References";
			refs.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			refs.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
			view.Columns.Add(refs);

			DataGridViewTextBoxColumn refd = new DataGridViewTextBoxColumn();
			refd.DataPropertyName = "ReferencedCount";
			refd.HeaderText = "Referenced";
			refd.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			refd.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
			view.Columns.Add(refd);

			DataGridViewCheckBoxColumn matched = new DataGridViewCheckBoxColumn();
			matched.DataPropertyName = "Matched";
			matched.HeaderText = "Matched";
			matched.SortMode = DataGridViewColumnSortMode.Automatic;
			matched.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			view.Columns.Add(matched);

			DataGridViewCheckBoxColumn valid = new DataGridViewCheckBoxColumn();
			valid.DataPropertyName = "Valid";
			valid.HeaderText = "Valid";
			valid.SortMode = DataGridViewColumnSortMode.Automatic;
			valid.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			view.Columns.Add(valid);

			DataGridViewTextBoxColumn line = new DataGridViewTextBoxColumn();
			line.DataPropertyName = "Line";
			line.HeaderText = "Line Number";
			line.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			line.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
			view.Columns.Add(line);

			DataGridViewTextBoxColumn path = new DataGridViewTextBoxColumn();
			path.DataPropertyName = "Location";
			path.HeaderText = "File";
			view.Columns.Add(path);
		}
		
		public static void ParameterColumns(DataGridView view)
		{
			view.Columns.Clear();
			view.AutoGenerateColumns = false;
			
			DataGridViewTextBoxColumn name = new DataGridViewTextBoxColumn();
			name.DataPropertyName = "Name";
			name.HeaderText = "Name";
			view.Columns.Add(name);

			DataGridViewTextBoxColumn values = new DataGridViewTextBoxColumn();
			values.DataPropertyName = "Values";
			values.HeaderText = "Values";
			view.Columns.Add(values);

			DataGridViewCheckBoxColumn matched = new DataGridViewCheckBoxColumn();
			matched.DataPropertyName = "Matched";
			matched.HeaderText = "Matched";
			matched.SortMode = DataGridViewColumnSortMode.Automatic;
			matched.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			view.Columns.Add(matched);

			DataGridViewCheckBoxColumn valid = new DataGridViewCheckBoxColumn();
			valid.DataPropertyName = "Valid";
			valid.HeaderText = "Valid";
			valid.SortMode = DataGridViewColumnSortMode.Automatic;
			valid.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			view.Columns.Add(valid);

			DataGridViewTextBoxColumn xvalues = new DataGridViewTextBoxColumn();
			xvalues.DataPropertyName = "XValues";
			xvalues.HeaderText = "Matched Patterns";
			view.Columns.Add(xvalues);
		}
	}
}