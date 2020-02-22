namespace INIEditor
{
	partial class DetailsForm
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
            this.mainTab = new System.Windows.Forms.TabControl();
            this.tabProperties = new System.Windows.Forms.TabPage();
            this.viewProperties = new System.Windows.Forms.DataGridView();
            this.tabBlocks = new System.Windows.Forms.TabPage();
            this.viewBlocks = new System.Windows.Forms.DataGridView();
            this.tabReferenced = new System.Windows.Forms.TabPage();
            this.viewReferenced = new System.Windows.Forms.DataGridView();
            this.tabReferences = new System.Windows.Forms.TabPage();
            this.viewReferences = new System.Windows.Forms.DataGridView();
            this.tabProblems = new System.Windows.Forms.TabPage();
            this.viewProblems = new System.Windows.Forms.DataGridView();
            this.mainSplit = new System.Windows.Forms.SplitContainer();
            this.txtLine = new System.Windows.Forms.TextBox();
            this.lblLine = new System.Windows.Forms.Label();
            this.lblLocation = new System.Windows.Forms.Label();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.mainTab.SuspendLayout();
            this.tabProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.viewProperties)).BeginInit();
            this.tabBlocks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.viewBlocks)).BeginInit();
            this.tabReferenced.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.viewReferenced)).BeginInit();
            this.tabReferences.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.viewReferences)).BeginInit();
            this.tabProblems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.viewProblems)).BeginInit();
            this.mainSplit.Panel1.SuspendLayout();
            this.mainSplit.Panel2.SuspendLayout();
            this.mainSplit.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTab
            // 
            this.mainTab.Controls.Add(this.tabProperties);
            this.mainTab.Controls.Add(this.tabBlocks);
            this.mainTab.Controls.Add(this.tabReferenced);
            this.mainTab.Controls.Add(this.tabReferences);
            this.mainTab.Controls.Add(this.tabProblems);
            this.mainTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTab.Location = new System.Drawing.Point(0, 0);
            this.mainTab.Name = "mainTab";
            this.mainTab.SelectedIndex = 0;
            this.mainTab.Size = new System.Drawing.Size(605, 480);
            this.mainTab.TabIndex = 0;
            // 
            // tabProperties
            // 
            this.tabProperties.Controls.Add(this.viewProperties);
            this.tabProperties.Location = new System.Drawing.Point(4, 22);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.Size = new System.Drawing.Size(597, 454);
            this.tabProperties.TabIndex = 0;
            this.tabProperties.Text = "Properties";
            this.tabProperties.UseVisualStyleBackColor = true;
            // 
            // viewProperties
            // 
            this.viewProperties.AllowUserToAddRows = false;
            this.viewProperties.AllowUserToDeleteRows = false;
            this.viewProperties.AllowUserToOrderColumns = true;
            this.viewProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.viewProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewProperties.Location = new System.Drawing.Point(0, 0);
            this.viewProperties.Name = "viewProperties";
            this.viewProperties.ReadOnly = true;
            this.viewProperties.ShowEditingIcon = false;
            this.viewProperties.ShowRowErrors = false;
            this.viewProperties.Size = new System.Drawing.Size(597, 454);
            this.viewProperties.TabIndex = 2;
            // 
            // tabBlocks
            // 
            this.tabBlocks.Controls.Add(this.viewBlocks);
            this.tabBlocks.Location = new System.Drawing.Point(4, 22);
            this.tabBlocks.Name = "tabBlocks";
            this.tabBlocks.Size = new System.Drawing.Size(597, 454);
            this.tabBlocks.TabIndex = 2;
            this.tabBlocks.Text = "Blocks";
            this.tabBlocks.UseVisualStyleBackColor = true;
            // 
            // viewBlocks
            // 
            this.viewBlocks.AllowUserToAddRows = false;
            this.viewBlocks.AllowUserToDeleteRows = false;
            this.viewBlocks.AllowUserToOrderColumns = true;
            this.viewBlocks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.viewBlocks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewBlocks.Location = new System.Drawing.Point(0, 0);
            this.viewBlocks.Name = "viewBlocks";
            this.viewBlocks.ReadOnly = true;
            this.viewBlocks.ShowEditingIcon = false;
            this.viewBlocks.ShowRowErrors = false;
            this.viewBlocks.Size = new System.Drawing.Size(597, 454);
            this.viewBlocks.TabIndex = 3;
            this.viewBlocks.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.viewBlocks_CellDoubleClick);
            // 
            // tabReferenced
            // 
            this.tabReferenced.Controls.Add(this.viewReferenced);
            this.tabReferenced.Location = new System.Drawing.Point(4, 22);
            this.tabReferenced.Name = "tabReferenced";
            this.tabReferenced.Size = new System.Drawing.Size(597, 454);
            this.tabReferenced.TabIndex = 4;
            this.tabReferenced.Text = "Referenced";
            this.tabReferenced.UseVisualStyleBackColor = true;
            // 
            // viewReferenced
            // 
            this.viewReferenced.AllowUserToAddRows = false;
            this.viewReferenced.AllowUserToDeleteRows = false;
            this.viewReferenced.AllowUserToOrderColumns = true;
            this.viewReferenced.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.viewReferenced.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewReferenced.Location = new System.Drawing.Point(0, 0);
            this.viewReferenced.Name = "viewReferenced";
            this.viewReferenced.ReadOnly = true;
            this.viewReferenced.ShowEditingIcon = false;
            this.viewReferenced.ShowRowErrors = false;
            this.viewReferenced.Size = new System.Drawing.Size(597, 454);
            this.viewReferenced.TabIndex = 5;
            // 
            // tabReferences
            // 
            this.tabReferences.Controls.Add(this.viewReferences);
            this.tabReferences.Location = new System.Drawing.Point(4, 22);
            this.tabReferences.Name = "tabReferences";
            this.tabReferences.Size = new System.Drawing.Size(597, 454);
            this.tabReferences.TabIndex = 3;
            this.tabReferences.Text = "References";
            this.tabReferences.UseVisualStyleBackColor = true;
            // 
            // viewReferences
            // 
            this.viewReferences.AllowUserToAddRows = false;
            this.viewReferences.AllowUserToDeleteRows = false;
            this.viewReferences.AllowUserToOrderColumns = true;
            this.viewReferences.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.viewReferences.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewReferences.Location = new System.Drawing.Point(0, 0);
            this.viewReferences.Name = "viewReferences";
            this.viewReferences.ReadOnly = true;
            this.viewReferences.ShowEditingIcon = false;
            this.viewReferences.ShowRowErrors = false;
            this.viewReferences.Size = new System.Drawing.Size(597, 454);
            this.viewReferences.TabIndex = 4;
            // 
            // tabProblems
            // 
            this.tabProblems.Controls.Add(this.viewProblems);
            this.tabProblems.Location = new System.Drawing.Point(4, 22);
            this.tabProblems.Name = "tabProblems";
            this.tabProblems.Size = new System.Drawing.Size(597, 454);
            this.tabProblems.TabIndex = 1;
            this.tabProblems.Text = "Problems";
            this.tabProblems.UseVisualStyleBackColor = true;
            // 
            // viewProblems
            // 
            this.viewProblems.AllowUserToAddRows = false;
            this.viewProblems.AllowUserToDeleteRows = false;
            this.viewProblems.AllowUserToOrderColumns = true;
            this.viewProblems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.viewProblems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewProblems.Location = new System.Drawing.Point(0, 0);
            this.viewProblems.Margin = new System.Windows.Forms.Padding(0);
            this.viewProblems.Name = "viewProblems";
            this.viewProblems.ReadOnly = true;
            this.viewProblems.ShowEditingIcon = false;
            this.viewProblems.ShowRowErrors = false;
            this.viewProblems.Size = new System.Drawing.Size(597, 454);
            this.viewProblems.TabIndex = 2;
            // 
            // mainSplit
            // 
            this.mainSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.mainSplit.Location = new System.Drawing.Point(0, 0);
            this.mainSplit.Name = "mainSplit";
            this.mainSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // mainSplit.Panel1
            // 
            this.mainSplit.Panel1.Controls.Add(this.mainTab);
            this.mainSplit.Panel1MinSize = 100;
            // 
            // mainSplit.Panel2
            // 
            this.mainSplit.Panel2.Controls.Add(this.txtLine);
            this.mainSplit.Panel2.Controls.Add(this.lblLine);
            this.mainSplit.Panel2.Controls.Add(this.lblLocation);
            this.mainSplit.Panel2.Controls.Add(this.txtLocation);
            this.mainSplit.Size = new System.Drawing.Size(605, 523);
            this.mainSplit.SplitterDistance = 480;
            this.mainSplit.TabIndex = 1;
            // 
            // txtLine
            // 
            this.txtLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLine.Location = new System.Drawing.Point(538, 6);
            this.txtLine.Name = "txtLine";
            this.txtLine.ReadOnly = true;
            this.txtLine.Size = new System.Drawing.Size(55, 20);
            this.txtLine.TabIndex = 3;
            this.txtLine.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblLine
            // 
            this.lblLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLine.AutoSize = true;
            this.lblLine.Location = new System.Drawing.Point(502, 9);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(30, 13);
            this.lblLine.TabIndex = 2;
            this.lblLine.Text = "Line:";
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(12, 9);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(51, 13);
            this.lblLocation.TabIndex = 1;
            this.lblLocation.Text = "Location:";
            // 
            // txtLocation
            // 
            this.txtLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLocation.Location = new System.Drawing.Point(69, 6);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.ReadOnly = true;
            this.txtLocation.Size = new System.Drawing.Size(427, 20);
            this.txtLocation.TabIndex = 0;
            // 
            // DetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 523);
            this.Controls.Add(this.mainSplit);
            this.Name = "DetailsForm";
            this.Text = "Details";
            this.mainTab.ResumeLayout(false);
            this.tabProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.viewProperties)).EndInit();
            this.tabBlocks.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.viewBlocks)).EndInit();
            this.tabReferenced.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.viewReferenced)).EndInit();
            this.tabReferences.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.viewReferences)).EndInit();
            this.tabProblems.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.viewProblems)).EndInit();
            this.mainSplit.Panel1.ResumeLayout(false);
            this.mainSplit.Panel2.ResumeLayout(false);
            this.mainSplit.Panel2.PerformLayout();
            this.mainSplit.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl mainTab;
		private System.Windows.Forms.TabPage tabProperties;
		private System.Windows.Forms.TabPage tabProblems;
		private System.Windows.Forms.DataGridView viewProperties;
		private System.Windows.Forms.DataGridView viewProblems;
		private System.Windows.Forms.TabPage tabBlocks;
		private System.Windows.Forms.DataGridView viewBlocks;
		private System.Windows.Forms.TabPage tabReferences;
		private System.Windows.Forms.DataGridView viewReferences;
		private System.Windows.Forms.TabPage tabReferenced;
		private System.Windows.Forms.DataGridView viewReferenced;
		private System.Windows.Forms.SplitContainer mainSplit;
		private System.Windows.Forms.Label lblLocation;
		private System.Windows.Forms.TextBox txtLocation;
		private System.Windows.Forms.TextBox txtLine;
		private System.Windows.Forms.Label lblLine;
	}
}