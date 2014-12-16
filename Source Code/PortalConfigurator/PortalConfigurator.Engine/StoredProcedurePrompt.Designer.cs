namespace PortalConfigurator
{
	partial class StoredProcedurePrompt
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
			this.storedProcedureComboBox = new System.Windows.Forms.ComboBox();
			this.storedProdedureLabel = new System.Windows.Forms.Label();
			this.okButton = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// storedProcedureComboBox
			// 
			this.storedProcedureComboBox.FormattingEnabled = true;
			this.storedProcedureComboBox.Location = new System.Drawing.Point(13, 25);
			this.storedProcedureComboBox.Name = "storedProcedureComboBox";
			this.storedProcedureComboBox.Size = new System.Drawing.Size(259, 21);
			this.storedProcedureComboBox.TabIndex = 0;
			this.storedProcedureComboBox.SelectedIndexChanged += new System.EventHandler(this.storedProcedureComboBox_SelectedIndexChanged);
			// 
			// storedProdedureLabel
			// 
			this.storedProdedureLabel.AutoSize = true;
			this.storedProdedureLabel.Location = new System.Drawing.Point(12, 9);
			this.storedProdedureLabel.Name = "storedProdedureLabel";
			this.storedProdedureLabel.Size = new System.Drawing.Size(135, 13);
			this.storedProdedureLabel.TabIndex = 1;
			this.storedProdedureLabel.Text = "Select a Stored Procedure:";
			// 
			// okButton
			// 
			this.okButton.Location = new System.Drawing.Point(64, 52);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 2;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(145, 52);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 3;
			this.button1.Text = "Cancel";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// StoredProcedurePrompt
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 87);
			this.ControlBox = false;
			this.Controls.Add(this.button1);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.storedProdedureLabel);
			this.Controls.Add(this.storedProcedureComboBox);
			this.Name = "StoredProcedurePrompt";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Stored Procedure";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.StoredProcedurePrompt_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox storedProcedureComboBox;
		private System.Windows.Forms.Label storedProdedureLabel;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button button1;
	}
}