namespace TreeTestApp
{
	partial class TestTreeForm
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            CommonTools.TreeListColumn treeListColumn1 = new CommonTools.TreeListColumn("name", "Name");
            CommonTools.TreeListColumn treeListColumn2 = new CommonTools.TreeListColumn("level", "Level");
            CommonTools.TreeListColumn treeListColumn3 = new CommonTools.TreeListColumn("entry", "Entry");
            CommonTools.TreeListColumn treeListColumn4 = new CommonTools.TreeListColumn("instructions", "Instructions");
            CommonTools.TreeListColumn treeListColumn5 = new CommonTools.TreeListColumn("allInstr", "All instructions");
            CommonTools.TreeListColumn treeListColumn6 = new CommonTools.TreeListColumn("visibleCount", "Visible Count");
            this.m_fill = new System.Windows.Forms.Button();
            this.m_tree = new TreeTestApp.TestTree();
            this.searchBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.m_tree)).BeginInit();
            this.SuspendLayout();
            // 
            // m_fill
            // 
            this.m_fill.Location = new System.Drawing.Point(3, 4);
            this.m_fill.Name = "m_fill";
            this.m_fill.Size = new System.Drawing.Size(75, 23);
            this.m_fill.TabIndex = 1;
            this.m_fill.Text = "Open";
            this.m_fill.UseVisualStyleBackColor = true;
            this.m_fill.Click += new System.EventHandler(this.m_tree.OnFill);
            // 
            // m_tree
            // 
            this.m_tree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            treeListColumn1.AutoSize = true;
            treeListColumn1.AutoSizeMinSize = 140;
            treeListColumn1.Width = 150;
            treeListColumn2.AutoSizeMinSize = 100;
            treeListColumn2.CellFormat.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            treeListColumn2.HeaderFormat.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            treeListColumn2.Width = 50;
            treeListColumn3.AutoSizeMinSize = 0;
            treeListColumn3.CellFormat.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            treeListColumn3.HeaderFormat.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            treeListColumn3.Width = 70;
            treeListColumn4.AutoSizeMinSize = 0;
            treeListColumn4.CellFormat.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            treeListColumn4.HeaderFormat.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            treeListColumn4.Width = 70;
            treeListColumn5.AutoSizeMinSize = 0;
            treeListColumn5.CellFormat.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            treeListColumn5.HeaderFormat.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            treeListColumn5.Width = 100;
            treeListColumn6.AutoSizeMinSize = 0;
            treeListColumn6.CellFormat.ForeColor = System.Drawing.Color.DarkGreen;
            treeListColumn6.CellFormat.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            treeListColumn6.HeaderFormat.ForeColor = System.Drawing.Color.Red;
            treeListColumn6.HeaderFormat.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            treeListColumn6.Width = 100;
            this.m_tree.Columns.AddRange(new CommonTools.TreeListColumn[] {
            treeListColumn1,
            treeListColumn2,
            treeListColumn3,
            treeListColumn4,
            treeListColumn5,
            treeListColumn6});
            this.m_tree.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.m_tree.Images = null;
            this.m_tree.Location = new System.Drawing.Point(0, 33);
            this.m_tree.Name = "m_tree";
            this.m_tree.Size = new System.Drawing.Size(616, 267);
            this.m_tree.TabIndex = 0;
            this.m_tree.ViewOptions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            // 
            // searchBox
            // 
            this.searchBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchBox.Location = new System.Drawing.Point(513, 7);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(100, 20);
            this.searchBox.TabIndex = 2;
            this.searchBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.m_tree.OnSearch);
            // 
            // TestTreeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.searchBox);
            this.Controls.Add(this.m_fill);
            this.Controls.Add(this.m_tree);
            this.MinimumSize = new System.Drawing.Size(0, 300);
            this.Name = "TestTreeForm";
            this.Size = new System.Drawing.Size(616, 300);
            ((System.ComponentModel.ISupportInitialize)(this.m_tree)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
		}

        #endregion

        private TestTree m_tree;
		private System.Windows.Forms.Button m_fill;
        private System.Windows.Forms.TextBox searchBox;
    }
}
