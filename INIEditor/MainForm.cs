using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace INIEditor
{
	public partial class MainForm : Form
	{
		BindingListView<INIBlock> blv;
		private DetailsForm details;
		
		XMLParser xParser;
		INIParser iParser;
		
		public MainForm()
		{
			InitializeComponent();
			
			folderBrowser.SelectedPath = (string) Properties.Settings.Default["DataPath"];
			
			Program.DefinitionsPath = Path.Combine(Application.StartupPath, "definitions.xml");
			if (File.Exists(Program.DefinitionsPath))
				xParser = new XMLParser(Program.DefinitionsPath);
		}
		
		private void DisplayBlocks()
		{
			blv = new BindingListView<INIBlock>(iParser.Blocks, delegate(PropertyDescriptor property, ListSortDirection direction) { return new INIComparer(property, direction); });
			
			view.DataSource = blv;
			
			GridViewColumns.BlockColumns(view);

            exportToCSVToolStripMenuItem.Enabled = true;
		}

		private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(folderBrowser.ShowDialog() == DialogResult.OK)
			{
				if (workerParse.IsBusy)
					workerParse.CancelAsync();
				
				
				Program.DataPath = folderBrowser.SelectedPath;
				
				progress.Visible = true;
				workerParse.RunWorkerAsync();
			}
		}

		private void workerParse_DoWork(object sender, DoWorkEventArgs e)
		{
			iParser = new INIParser(Program.DataPath);
			
			iParser.MatchBlocks(xParser);

            iParser.INIChanged += new EventHandler(iParser_INIChanged);
            iParser.INIChangedStart += new EventHandler(iParser_INIChangedStart);
        }

        string preChangeSelectedName;
        string preChangeSelectedType;

        string preChangeDetailsName;
        string preChangeDetailsType;

        void iParser_INIChangedStart(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                progress.Visible = true;
                if (view.SelectedRows.Count > 0)
                {
                    INIBlock b = blv[view.SelectedRows[0].Index];
                    preChangeSelectedType = b.Type.ToLower();
                    preChangeSelectedName = b.Name.ToLower();
                }
                else
                    preChangeSelectedName = preChangeSelectedType = "";

                if (details != null)
                {
                    INIBlock b = details.Data;
                    preChangeDetailsType = b.Type.ToLower();
                    preChangeDetailsName = b.Name.ToLower();
                }
                else
                    preChangeDetailsName = preChangeDetailsType = "";
            });
        }

        void iParser_INIChanged(object sender, EventArgs e)
        {
            iParser.MatchBlocks(xParser);

            this.Invoke((MethodInvoker)delegate {
                DisplayBlocks();
                blv.Filter = filterTextStrip.Text;
                progress.Visible = false;

                if (preChangeSelectedName != "")
                {
                    foreach (INIBlock b in blv)
                    {
                        if (b.Name.ToLower() == preChangeSelectedName && b.Type.ToLower() == preChangeSelectedType)
                        {
                            int i = blv.IndexOf(b);
                            view.Rows[i].Selected = true;
                            break;
                        }
                    }
                }

                if (preChangeDetailsName != "" && details != null)
                {
                    bool found = false;

                    foreach (INIBlock b in blv)
                    {
                        if (b.Name.ToLower() == preChangeDetailsName && b.Type.ToLower() == preChangeDetailsType)
                        {
                            details.Data = b;
                            details.Focus();
                            found = true;
                            break;
                        }
                    }

                    if(!found)
                        details.Close();
                }
            });
        }

		private void workerParse_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if(e.Error == null)
				DisplayBlocks();
			else
			{
				MessageBox.Show("An error occured during parsing: " + e.Error.Message, "Parsing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				if(iParser != null) iParser = null;
            }
			
			progress.Visible = false;
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if(e.CloseReason == CloseReason.UserClosing)
			{
				e.Cancel = true;
				Hide();
			}
		}

		private void notify_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			Show();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Show();
		}

		private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void view_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if(e.ColumnIndex < 0 || e.RowIndex < 0) return;

            if (view.Columns[e.ColumnIndex].DataPropertyName == "Location")
            {
                INIBlock b = blv[e.RowIndex];
                string command = "\"" + b.Location + "\" -n" + b.Line;

                System.Diagnostics.Process.Start("notepad++", command);
            }
            else
            {
                if (details == null)
                {
                    details = new DetailsForm();
                    details.Show();
                    details.FormClosed += new FormClosedEventHandler(details_FormClosed);
                }
                else
                    details.Focus();

                details.Data = blv[e.RowIndex];
            }
		}

		void details_FormClosed(object sender, FormClosedEventArgs e)
		{
			details = null;
		}

		private void view_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (!blv[e.RowIndex].Valid)
				view.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightYellow;
		}

		private void filterToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if(blv == null) return;
			
			blv.Filter = filterTextStrip.Text;
		}

		private void filterTextStrip_KeyUp(object sender, KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter && blv != null)
			{
				blv.Filter = filterTextStrip.Text;
				e.Handled = true;
			}
		}

		private void filterTextStrip_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter && blv != null)
			{
				e.SuppressKeyPress = true;
			}
		}

		private void view_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			MessageBox.Show("Exception during parsing!" + Environment.NewLine + e.Exception.Message + Environment.NewLine + e.Exception.StackTrace);
			
			Exception er = e.Exception.InnerException;
			while(er != null)
			{
				MessageBox.Show("Inner exception:" + Environment.NewLine + er.Message + Environment.NewLine + er.StackTrace);
				er = er.InnerException;
			}
		}

        private void exportToCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string export;
            if (exportCSVDialog.ShowDialog() == DialogResult.OK)
                export = exportCSVDialog.FileName;
            else
                return;

            System.Collections.IEnumerable bc = (System.Collections.IEnumerable)view.Rows;
            if (view.SelectedRows.Count > 1)
                bc = (System.Collections.IEnumerable)view.SelectedRows;

            string firstType = null;

            // Get all possible parameters

            List<string> columns = new List<string>();

            foreach (DataGridViewRow row in bc)
            {
                INIBlock b = blv[row.Index];
                if (firstType == null)
                    firstType = b.Type;
                else if (b.Type != firstType)
                {
                    MessageBox.Show("Selected rows are of different block types; cannot generate consistent CSV file.", "Mismatched block types!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                foreach (INIParameter p in b.Parameters)
                    if (!columns.Contains(p.Name))
                        columns.Add(p.Name);
            }

            // Create the CSV file
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(String.Join(",", columns.ToArray()));

            foreach (DataGridViewRow row in bc)
            {
                INIBlock b = blv[row.Index];

                string[] cols = new string[columns.Count];

                foreach (INIParameter p in b.Parameters)
                {
                    string vals = "";
                    foreach (INIValue v in p)
                    {
                        vals += ", " + v.Value;
                    }
                    int colindex = columns.IndexOf(p.Name);
                    if (cols[colindex] == null)
                        cols[colindex] = vals.Substring(2);
                    else
                        cols[colindex] += Environment.NewLine + vals.Substring(2);
                }

                for (int a = 0; a < cols.Length; a++)
                    if (cols[a] == null) cols[a] = " ";
                    else cols[a] = "\"" + cols[a] + "\"";

                sb.AppendLine(String.Join(",", cols));
            }

            File.WriteAllText(export, sb.ToString());
        }

        struct LogData
        {
            public string Message;
            public string File;
            public int Line;
        };

        private class LogComparer : IComparer<LogData>
        {
            public int Compare(LogData d1, LogData d2)
            {
                int c1 = d1.File.CompareTo(d2.File);
                if (c1 != 0) return c1;

                return d1.Line.CompareTo(d2.Line);
            }
        };

        private void exportAllErrorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string export;
            if (errorsFileDialog.ShowDialog() == DialogResult.OK)
                export = errorsFileDialog.FileName;
            else
                return;

            List<LogData> logs = new List<LogData>();

            foreach (INIBlock b in blv)
            {
                if (!b.Valid)
                {
                    LogData log = new LogData();

                    log.Message = "\t[" + b.Type.ToUpper() + "] (" + b.Line + ") - " + b.Name + Environment.NewLine;

                    foreach (ValidationError err in b.Errors.OrderByDescending(x => (int)x.Severity))
                        log.Message += "\t\t[" + err.SeverityText.ToUpper() + "] - " + err.ErrorText + Environment.NewLine;

                    log.Message = log.Message.Trim();

                    log.File = b.Location;
                    log.Line = b.Line;

                    logs.Add(log);
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("FILE GENERATED ON " + DateTime.Now.ToLongTimeString());
            string currentFile = "";

            foreach (LogData l in logs.OrderBy(x => x, new LogComparer()))
            {
                if (l.File != currentFile)
                {
                    sb.AppendLine();
                    sb.AppendLine(new String('-', l.File.Length));
                    sb.AppendLine(l.File);
                    sb.AppendLine(new String('-', l.File.Length));
                    sb.AppendLine();
                }

                currentFile = l.File;

                sb.AppendLine(l.Message);
            }

            File.WriteAllText(export, sb.ToString());
        }

        private void view_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void exportAllCRCsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string export;
            if (errorsFileDialog.ShowDialog() == DialogResult.OK)
                export = errorsFileDialog.FileName;
            else
                return;

            Dictionary<uint, string> crcs = new Dictionary<uint, string>();
            Dictionary<uint, string> hashes = new Dictionary<uint, string>();

            CRC c = CRC.GetInstance();

            foreach (INIBlock b in blv)
            {
                /*if (!b.Valid)
                {
                    LogData log = new LogData();

                    log.Message = "\t[" + b.Type.ToUpper() + "] (" + b.Line + ") - " + b.Name + Environment.NewLine;

                    foreach (ValidationError err in b.Errors.OrderByDescending(x => (int)x.Severity))
                        log.Message += "\t\t[" + err.SeverityText.ToUpper() + "] - " + err.ErrorText + Environment.NewLine;

                    log.Message = log.Message.Trim();

                    log.File = b.Location;
                    log.Line = b.Line;

                    logs.Add(log);
                }*/

                foreach(INIParameter p in b.Parameters)
                {
                    foreach(INIValue v in p)
                    {
                        uint crc = c.ComputeCRC(v.Value);
                        uint hash = c.ComputeHash(v.Value);

                        crcs[crc] = v.Value;
                        hashes[hash] = v.Value;
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("FILE GENERATED ON " + DateTime.Now.ToLongTimeString());

            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("------------------");
            sb.AppendLine("------------------");
            sb.AppendLine("List of CRC Values");
            sb.AppendLine("------------------");
            sb.AppendLine("------------------");

            foreach (KeyValuePair<uint, string> kvp in crcs)
            {
                sb.AppendLine(kvp.Key + "(0x" + kvp.Key.ToString("X") + ") = " + kvp.Value);
            }

            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("-------------------");
            sb.AppendLine("-------------------");
            sb.AppendLine("List of Hash Values");
            sb.AppendLine("-------------------");
            sb.AppendLine("-------------------");

            foreach (KeyValuePair<uint, string> kvp in hashes)
            {
                sb.AppendLine(kvp.Key + "(0x" + kvp.Key.ToString("X") + ") = " + kvp.Value);
            }

            File.WriteAllText(export, sb.ToString());
        }
	}
}
