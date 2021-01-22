namespace TreeTestApp
{
	partial class Form1
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
            this.testTreeForm1 = new TreeTestApp.TestTreeForm();
            this.SuspendLayout();
            // 
            // testTreeForm1
            // 
            this.testTreeForm1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.testTreeForm1.Location = new System.Drawing.Point(0, 0);
            this.testTreeForm1.Name = "testTreeForm1";
            this.testTreeForm1.Size = new System.Drawing.Size(610, 598);
            this.testTreeForm1.TabIndex = 3;
            this.testTreeForm1.Load += new System.EventHandler(this.testTreeForm1_Load);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 598);
            this.Controls.Add(this.testTreeForm1);
            this.Name = "Form1";
            this.Text = "ITR Viewer";
            this.ResumeLayout(false);

		}

		#endregion

		private TestTreeForm testTreeForm1;
	}
}

