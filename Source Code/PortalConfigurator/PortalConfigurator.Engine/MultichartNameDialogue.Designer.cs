namespace PortalConfigurator
{
	partial class MultichartNameDialogue
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
			this.components = new System.ComponentModel.Container();
			this.multichartNameTextBox = new System.Windows.Forms.TextBox();
			this.multichartNameLabel = new System.Windows.Forms.Label();
			this.doneButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.duplicateNameErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			((System.ComponentModel.ISupportInitialize)(this.duplicateNameErrorProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// multichartNameTextBox
			// 
			this.multichartNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.multichartNameTextBox.Location = new System.Drawing.Point(12, 25);
			this.multichartNameTextBox.Name = "multichartNameTextBox";
			this.multichartNameTextBox.Size = new System.Drawing.Size(324, 20);
			this.multichartNameTextBox.TabIndex = 0;
			this.multichartNameTextBox.TextChanged += new System.EventHandler(this.multichartNameTextBox_TextChanged);
			// 
			// multichartNameLabel
			// 
			this.multichartNameLabel.AutoSize = true;
			this.multichartNameLabel.Location = new System.Drawing.Point(9, 9);
			this.multichartNameLabel.Name = "multichartNameLabel";
			this.multichartNameLabel.Size = new System.Drawing.Size(84, 13);
			this.multichartNameLabel.TabIndex = 1;
			this.multichartNameLabel.Text = "Multichart Name";
			// 
			// doneButton
			// 
			this.doneButton.Location = new System.Drawing.Point(95, 51);
			this.doneButton.Name = "doneButton";
			this.doneButton.Size = new System.Drawing.Size(75, 23);
			this.doneButton.TabIndex = 2;
			this.doneButton.Text = "Done";
			this.doneButton.UseVisualStyleBackColor = true;
			this.doneButton.Click += new System.EventHandler(this.doneButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(176, 51);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 3;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// duplicateNameErrorProvider
			// 
			this.duplicateNameErrorProvider.ContainerControl = this;
			// 
			// MultichartNameDialogue
			// 
			this.AcceptButton = this.doneButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(348, 86);
			this.ControlBox = false;
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.doneButton);
			this.Controls.Add(this.multichartNameLabel);
			this.Controls.Add(this.multichartNameTextBox);
			this.Name = "MultichartNameDialogue";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Multichart Name";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.MultichartNameDialogue_Load);
			((System.ComponentModel.ISupportInitialize)(this.duplicateNameErrorProvider)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox multichartNameTextBox;
		private System.Windows.Forms.Label multichartNameLabel;
		private System.Windows.Forms.Button doneButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.ErrorProvider duplicateNameErrorProvider;
	}
}