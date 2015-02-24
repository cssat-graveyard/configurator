namespace PortalConfigurator
{
	partial class NewerFileDialog
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
			this.dialogLabel = new System.Windows.Forms.Label();
			this.decisionLabel = new System.Windows.Forms.Label();
			this.overwriteButton = new System.Windows.Forms.Button();
			this.reloadButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// dialogLabel
			// 
			this.dialogLabel.AutoSize = true;
			this.dialogLabel.Location = new System.Drawing.Point(12, 21);
			this.dialogLabel.MaximumSize = new System.Drawing.Size(450, 0);
			this.dialogLabel.Name = "dialogLabel";
			this.dialogLabel.Size = new System.Drawing.Size(262, 13);
			this.dialogLabel.TabIndex = 0;
			this.dialogLabel.Text = "The source file has been modified since it was loaded.";
			// 
			// decisionLabel
			// 
			this.decisionLabel.AutoSize = true;
			this.decisionLabel.Location = new System.Drawing.Point(12, 51);
			this.decisionLabel.MaximumSize = new System.Drawing.Size(265, 0);
			this.decisionLabel.Name = "decisionLabel";
			this.decisionLabel.Size = new System.Drawing.Size(265, 26);
			this.decisionLabel.TabIndex = 1;
			this.decisionLabel.Text = "Do you wish to continue overwriting the file, reload the file and lose current ch" +
    "anges, or cancel the save?";
			// 
			// overwriteButton
			// 
			this.overwriteButton.DialogResult = System.Windows.Forms.DialogResult.Ignore;
			this.overwriteButton.Location = new System.Drawing.Point(25, 93);
			this.overwriteButton.Name = "overwriteButton";
			this.overwriteButton.Size = new System.Drawing.Size(75, 23);
			this.overwriteButton.TabIndex = 2;
			this.overwriteButton.Text = "Overwrite";
			this.overwriteButton.UseVisualStyleBackColor = true;
			// 
			// reloadButton
			// 
			this.reloadButton.DialogResult = System.Windows.Forms.DialogResult.Retry;
			this.reloadButton.Location = new System.Drawing.Point(107, 93);
			this.reloadButton.Name = "reloadButton";
			this.reloadButton.Size = new System.Drawing.Size(75, 23);
			this.reloadButton.TabIndex = 3;
			this.reloadButton.Text = "Reload";
			this.reloadButton.UseVisualStyleBackColor = true;
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(189, 93);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 4;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// NewerFileDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(290, 128);
			this.ControlBox = false;
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.reloadButton);
			this.Controls.Add(this.overwriteButton);
			this.Controls.Add(this.decisionLabel);
			this.Controls.Add(this.dialogLabel);
			this.Name = "NewerFileDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Newer File Conflict";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label dialogLabel;
		private System.Windows.Forms.Label decisionLabel;
		private System.Windows.Forms.Button overwriteButton;
		private System.Windows.Forms.Button reloadButton;
		private System.Windows.Forms.Button cancelButton;
	}
}