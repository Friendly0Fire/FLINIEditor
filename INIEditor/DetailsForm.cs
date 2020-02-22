using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace INIEditor
{
	public partial class DetailsForm : Form
	{
		private INIBlock data;
		
		public DetailsForm()
		{
			InitializeComponent();
		}
		
		public INIBlock Data
		{
			get
			{
				return data;
			}
			set
			{
				data = value;
				this.Refresh();
			}
		}

		public override void Refresh()
		{
			base.Refresh();

			DisplayParameters();

			DisplayBlocks();

			DisplayReferenced();

			DisplayReferences();

			DisplayProblems();
			
			this.mainTab.SelectedIndex = 0;

			this.txtLocation.Text = data.Location;
			this.txtLine.Text = data.Line.ToString();

            this.Text = "Details for " + data.Name;
		}

		private void DisplayParameters()
		{
			BindingListView<INIParameter> blv = new BindingListView<INIParameter>(data.Parameters);
			viewProperties.DataSource = blv;

			GridViewColumns.ParameterColumns(viewProperties);
		}

		private void DisplayBlocks()
		{
			BindingListView<INIBlock> blv = new BindingListView<INIBlock>(data.Blocks, delegate(PropertyDescriptor property, ListSortDirection direction) { return new INIComparer(property, direction); });
			viewBlocks.DataSource = blv;

			GridViewColumns.BlockColumns(viewBlocks);
		}

		private void DisplayReferences()
		{
			BindingListView<INIBlock> blv = new BindingListView<INIBlock>(data.References.ToList(), delegate(PropertyDescriptor property, ListSortDirection direction) { return new INIComparer(property, direction); });
			viewReferences.DataSource = blv;

			GridViewColumns.BlockColumns(viewReferences);
		}

		private void DisplayReferenced()
		{
			BindingListView<INIBlock> blv = new BindingListView<INIBlock>(data.Referenced.ToList(), delegate(PropertyDescriptor property, ListSortDirection direction) { return new INIComparer(property, direction); });
			viewReferenced.DataSource = blv;

			GridViewColumns.BlockColumns(viewReferenced);
		}

		private void DisplayProblems()
		{
			BindingListView<ValidationError> blv = new BindingListView<ValidationError>(data.Errors);
			viewProblems.Columns.Clear();
			viewProblems.AutoGenerateColumns = false;
			viewProblems.DataSource = blv;

			DataGridViewTextBoxColumn message = new DataGridViewTextBoxColumn();
			message.DataPropertyName = "ErrorText";
			message.HeaderText = "Problem";
			viewProblems.Columns.Add(message);

			DataGridViewTextBoxColumn severity = new DataGridViewTextBoxColumn();
			severity.DataPropertyName = "SeverityText";
			severity.HeaderText = "Severity";
			viewProblems.Columns.Add(severity);
		}

		private void viewBlocks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex < 0 || e.RowIndex < 0) return;

			this.Data = data.Blocks[e.RowIndex];
		}
	}
}
