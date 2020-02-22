namespace INIEditor
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param type="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportAllErrorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filterTextStrip = new INIEditor.ToolStripStretchTextBox();
            this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.openDatabaseDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveDatabaseDialog = new System.Windows.Forms.SaveFileDialog();
            this.view = new System.Windows.Forms.DataGridView();
            this.notify = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.restoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.workerParse = new System.ComponentModel.BackgroundWorker();
            this.exportCSVDialog = new System.Windows.Forms.SaveFileDialog();
            this.errorsFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.exportAllCRCsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.view)).BeginInit();
            this.notifyMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.filterToolStripMenuItem,
            this.filterTextStrip});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 26);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToCSVToolStripMenuItem,
            this.exportAllErrorsToolStripMenuItem,
            this.exportAllCRCsToolStripMenuItem,
            this.openFolderToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 22);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exportToCSVToolStripMenuItem
            // 
            this.exportToCSVToolStripMenuItem.Enabled = false;
            this.exportToCSVToolStripMenuItem.Name = "exportToCSVToolStripMenuItem";
            this.exportToCSVToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.exportToCSVToolStripMenuItem.Text = "Export to CSV...";
            this.exportToCSVToolStripMenuItem.Click += new System.EventHandler(this.exportToCSVToolStripMenuItem_Click);
            // 
            // exportAllErrorsToolStripMenuItem
            // 
            this.exportAllErrorsToolStripMenuItem.Name = "exportAllErrorsToolStripMenuItem";
            this.exportAllErrorsToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.exportAllErrorsToolStripMenuItem.Text = "Export All Errors...";
            this.exportAllErrorsToolStripMenuItem.Click += new System.EventHandler(this.exportAllErrorsToolStripMenuItem_Click);
            // 
            // openFolderToolStripMenuItem
            // 
            this.openFolderToolStripMenuItem.Name = "openFolderToolStripMenuItem";
            this.openFolderToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.openFolderToolStripMenuItem.Text = "Open Folder...";
            this.openFolderToolStripMenuItem.Click += new System.EventHandler(this.openFolderToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(163, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // filterToolStripMenuItem
            // 
            this.filterToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.filterToolStripMenuItem.Name = "filterToolStripMenuItem";
            this.filterToolStripMenuItem.Size = new System.Drawing.Size(45, 22);
            this.filterToolStripMenuItem.Text = "Filter";
            this.filterToolStripMenuItem.Click += new System.EventHandler(this.filterToolStripMenuItem_Click);
            // 
            // filterTextStrip
            // 
            this.filterTextStrip.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.filterTextStrip.Name = "filterTextStrip";
            this.filterTextStrip.Size = new System.Drawing.Size(100, 22);
            this.filterTextStrip.KeyDown += new System.Windows.Forms.KeyEventHandler(this.filterTextStrip_KeyDown);
            this.filterTextStrip.KeyUp += new System.Windows.Forms.KeyEventHandler(this.filterTextStrip_KeyUp);
            // 
            // folderBrowser
            // 
            this.folderBrowser.Description = "Please select your DATA folder.";
            this.folderBrowser.ShowNewFolderButton = false;
            // 
            // view
            // 
            this.view.AllowUserToAddRows = false;
            this.view.AllowUserToDeleteRows = false;
            this.view.AllowUserToOrderColumns = true;
            this.view.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.view.Dock = System.Windows.Forms.DockStyle.Fill;
            this.view.Location = new System.Drawing.Point(0, 26);
            this.view.Name = "view";
            this.view.ReadOnly = true;
            this.view.ShowEditingIcon = false;
            this.view.ShowRowErrors = false;
            this.view.Size = new System.Drawing.Size(784, 536);
            this.view.TabIndex = 1;
            this.view.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.view_CellContentClick);
            this.view.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.view_CellDoubleClick);
            this.view.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.view_CellFormatting);
            this.view.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.view_DataError);
            // 
            // notify
            // 
            this.notify.ContextMenuStrip = this.notifyMenu;
            this.notify.Icon = ((System.Drawing.Icon)(resources.GetObject("notify.Icon")));
            this.notify.Text = "INI Analyzer";
            this.notify.Visible = true;
            this.notify.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notify_MouseDoubleClick);
            // 
            // notifyMenu
            // 
            this.notifyMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.restoreToolStripMenuItem,
            this.exitToolStripMenuItem1});
            this.notifyMenu.Name = "notifyMenu";
            this.notifyMenu.Size = new System.Drawing.Size(114, 48);
            // 
            // restoreToolStripMenuItem
            // 
            this.restoreToolStripMenuItem.Name = "restoreToolStripMenuItem";
            this.restoreToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.restoreToolStripMenuItem.Text = "Restore";
            this.restoreToolStripMenuItem.Click += new System.EventHandler(this.restoreToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(113, 22);
            this.exitToolStripMenuItem1.Text = "Exit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitToolStripMenuItem1_Click);
            // 
            // progress
            // 
            this.progress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progress.Location = new System.Drawing.Point(0, 539);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(784, 23);
            this.progress.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progress.TabIndex = 2;
            this.progress.Visible = false;
            // 
            // workerParse
            // 
            this.workerParse.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workerParse_DoWork);
            this.workerParse.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workerParse_RunWorkerCompleted);
            // 
            // exportCSVDialog
            // 
            this.exportCSVDialog.DefaultExt = "csv";
            this.exportCSVDialog.Filter = "CSV files|*.csv|All files|*.*";
            // 
            // errorsFileDialog
            // 
            this.errorsFileDialog.DefaultExt = "log";
            this.errorsFileDialog.FileName = "errors.log";
            this.errorsFileDialog.Filter = "Log files|*.log|All files|*.*";
            // 
            // exportAllCRCsToolStripMenuItem
            // 
            this.exportAllCRCsToolStripMenuItem.Name = "exportAllCRCsToolStripMenuItem";
            this.exportAllCRCsToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.exportAllCRCsToolStripMenuItem.Text = "Export All CRCs...";
            this.exportAllCRCsToolStripMenuItem.Click += new System.EventHandler(this.exportAllCRCsToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.progress);
            this.Controls.Add(this.view);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "INI Analyzer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.view)).EndInit();
            this.notifyMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openFolderToolStripMenuItem;
		private System.Windows.Forms.FolderBrowserDialog folderBrowser;
		private System.Windows.Forms.OpenFileDialog openDatabaseDialog;
		private System.Windows.Forms.SaveFileDialog saveDatabaseDialog;
		private System.Windows.Forms.DataGridView view;
		private System.Windows.Forms.NotifyIcon notify;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip notifyMenu;
		private System.Windows.Forms.ToolStripMenuItem restoreToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
		private System.Windows.Forms.ProgressBar progress;
		private System.ComponentModel.BackgroundWorker workerParse;
		private System.Windows.Forms.ToolStripMenuItem filterToolStripMenuItem;
		private ToolStripStretchTextBox filterTextStrip;
        private System.Windows.Forms.ToolStripMenuItem exportToCSVToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog exportCSVDialog;
        private System.Windows.Forms.ToolStripMenuItem exportAllErrorsToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog errorsFileDialog;
        private System.Windows.Forms.ToolStripMenuItem exportAllCRCsToolStripMenuItem;
	}
}

