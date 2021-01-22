namespace TreeTestApp
{
	partial class Form2
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.distinctView = new System.Windows.Forms.ListView();
            this.module = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.calls = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // distinctView
            // 
            this.distinctView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.distinctView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.module,
            this.calls});
            this.distinctView.HideSelection = false;
            this.distinctView.Location = new System.Drawing.Point(13, 13);
            this.distinctView.Name = "distinctView";
            this.distinctView.Size = new System.Drawing.Size(195, 530);
            this.distinctView.TabIndex = 0;
            this.distinctView.UseCompatibleStateImageBehavior = false;
            this.distinctView.View = System.Windows.Forms.View.Details;
            // 
            // module
            // 
            this.module.Text = "Module";
            this.module.Width = 90;
            // 
            // calls
            // 
            this.calls.Text = "Calls";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(220, 555);
            this.Controls.Add(this.distinctView);
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Distinct modules";
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView distinctView;
		private System.Windows.Forms.ColumnHeader module;
		private System.Windows.Forms.ColumnHeader calls;
	}
}