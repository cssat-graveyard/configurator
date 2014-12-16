namespace Framework
{
	partial class CredentialsPrompt
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
			this.userNameTextBox = new System.Windows.Forms.TextBox();
			this.userNameLabel = new System.Windows.Forms.Label();
			this.passwordLabel = new System.Windows.Forms.Label();
			this.passwordTextBox = new System.Windows.Forms.TextBox();
			this.confirmationButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// userNameTextBox
			// 
			this.userNameTextBox.Location = new System.Drawing.Point(74, 12);
			this.userNameTextBox.Name = "userNameTextBox";
			this.userNameTextBox.Size = new System.Drawing.Size(134, 20);
			this.userNameTextBox.TabIndex = 0;
			// 
			// userNameLabel
			// 
			this.userNameLabel.AutoSize = true;
			this.userNameLabel.Location = new System.Drawing.Point(12, 15);
			this.userNameLabel.Name = "userNameLabel";
			this.userNameLabel.Size = new System.Drawing.Size(38, 13);
			this.userNameLabel.TabIndex = 1;
			this.userNameLabel.Text = "NetID:";
			// 
			// passwordLabel
			// 
			this.passwordLabel.AutoSize = true;
			this.passwordLabel.Location = new System.Drawing.Point(12, 44);
			this.passwordLabel.Name = "passwordLabel";
			this.passwordLabel.Size = new System.Drawing.Size(56, 13);
			this.passwordLabel.TabIndex = 2;
			this.passwordLabel.Text = "Password:";
			// 
			// passwordTextBox
			// 
			this.passwordTextBox.Location = new System.Drawing.Point(74, 41);
			this.passwordTextBox.Name = "passwordTextBox";
			this.passwordTextBox.PasswordChar = '*';
			this.passwordTextBox.Size = new System.Drawing.Size(134, 20);
			this.passwordTextBox.TabIndex = 3;
			// 
			// confirmationButton
			// 
			this.confirmationButton.Location = new System.Drawing.Point(34, 73);
			this.confirmationButton.Name = "confirmationButton";
			this.confirmationButton.Size = new System.Drawing.Size(75, 23);
			this.confirmationButton.TabIndex = 4;
			this.confirmationButton.Text = "OK";
			this.confirmationButton.UseVisualStyleBackColor = true;
			this.confirmationButton.Click += new System.EventHandler(this.confirmationButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(115, 73);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 5;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// Prompt
			// 
			this.AcceptButton = this.confirmationButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(220, 108);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.confirmationButton);
			this.Controls.Add(this.passwordTextBox);
			this.Controls.Add(this.passwordLabel);
			this.Controls.Add(this.userNameLabel);
			this.Controls.Add(this.userNameTextBox);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Prompt";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Provide Credentials";
			this.Load += new System.EventHandler(this.Prompt_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox userNameTextBox;
		private System.Windows.Forms.Label userNameLabel;
		private System.Windows.Forms.Label passwordLabel;
		private System.Windows.Forms.TextBox passwordTextBox;
		private System.Windows.Forms.Button confirmationButton;
		private System.Windows.Forms.Button cancelButton;
	}
}