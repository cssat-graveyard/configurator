namespace PortalConfigurator
{
	partial class ValuesTypeDialog
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
			this.typeListView = new System.Windows.Forms.ListView();
			this.valueTypeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.typeLabel = new System.Windows.Forms.Label();
			this.selectedTypeTextBox = new System.Windows.Forms.TextBox();
			this.selectedTypeLabel = new System.Windows.Forms.Label();
			this.disabledTypeTextBox = new System.Windows.Forms.TextBox();
			this.disabledTypeLabel = new System.Windows.Forms.Label();
			this.closeButton = new System.Windows.Forms.Button();
			this.exampleJsonTextBox = new System.Windows.Forms.TextBox();
			this.exampleJsonLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// typeListView
			// 
			this.typeListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.typeListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.valueTypeColumn});
			this.typeListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.typeListView.HideSelection = false;
			this.typeListView.Location = new System.Drawing.Point(12, 25);
			this.typeListView.MultiSelect = false;
			this.typeListView.Name = "typeListView";
			this.typeListView.Size = new System.Drawing.Size(121, 99);
			this.typeListView.TabIndex = 0;
			this.typeListView.UseCompatibleStateImageBehavior = false;
			this.typeListView.View = System.Windows.Forms.View.Details;
			this.typeListView.SelectedIndexChanged += new System.EventHandler(this.typeListView_SelectedIndexChanged);
			// 
			// typeLabel
			// 
			this.typeLabel.AutoSize = true;
			this.typeLabel.Location = new System.Drawing.Point(9, 9);
			this.typeLabel.Name = "typeLabel";
			this.typeLabel.Size = new System.Drawing.Size(66, 13);
			this.typeLabel.TabIndex = 1;
			this.typeLabel.Text = "Values Type";
			// 
			// selectedTypeTextBox
			// 
			this.selectedTypeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.selectedTypeTextBox.BackColor = System.Drawing.SystemColors.Window;
			this.selectedTypeTextBox.Location = new System.Drawing.Point(12, 143);
			this.selectedTypeTextBox.Name = "selectedTypeTextBox";
			this.selectedTypeTextBox.ReadOnly = true;
			this.selectedTypeTextBox.Size = new System.Drawing.Size(121, 20);
			this.selectedTypeTextBox.TabIndex = 2;
			this.selectedTypeTextBox.TabStop = false;
			// 
			// selectedTypeLabel
			// 
			this.selectedTypeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.selectedTypeLabel.AutoSize = true;
			this.selectedTypeLabel.Location = new System.Drawing.Point(9, 127);
			this.selectedTypeLabel.Name = "selectedTypeLabel";
			this.selectedTypeLabel.Size = new System.Drawing.Size(111, 13);
			this.selectedTypeLabel.TabIndex = 3;
			this.selectedTypeLabel.Text = "Selected Values Type";
			// 
			// disabledTypeTextBox
			// 
			this.disabledTypeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.disabledTypeTextBox.BackColor = System.Drawing.SystemColors.Window;
			this.disabledTypeTextBox.Location = new System.Drawing.Point(12, 185);
			this.disabledTypeTextBox.Name = "disabledTypeTextBox";
			this.disabledTypeTextBox.ReadOnly = true;
			this.disabledTypeTextBox.Size = new System.Drawing.Size(121, 20);
			this.disabledTypeTextBox.TabIndex = 4;
			this.disabledTypeTextBox.TabStop = false;
			// 
			// disabledTypeLabel
			// 
			this.disabledTypeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.disabledTypeLabel.AutoSize = true;
			this.disabledTypeLabel.Location = new System.Drawing.Point(9, 169);
			this.disabledTypeLabel.Name = "disabledTypeLabel";
			this.disabledTypeLabel.Size = new System.Drawing.Size(110, 13);
			this.disabledTypeLabel.TabIndex = 5;
			this.disabledTypeLabel.Text = "Disabled Values Type";
			// 
			// closeButton
			// 
			this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.closeButton.Location = new System.Drawing.Point(12, 211);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(75, 23);
			this.closeButton.TabIndex = 6;
			this.closeButton.Text = "Close";
			this.closeButton.UseVisualStyleBackColor = true;
			this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
			// 
			// exampleJsonTextBox
			// 
			this.exampleJsonTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.exampleJsonTextBox.BackColor = System.Drawing.SystemColors.Window;
			this.exampleJsonTextBox.Location = new System.Drawing.Point(139, 25);
			this.exampleJsonTextBox.Multiline = true;
			this.exampleJsonTextBox.Name = "exampleJsonTextBox";
			this.exampleJsonTextBox.ReadOnly = true;
			this.exampleJsonTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.exampleJsonTextBox.Size = new System.Drawing.Size(188, 209);
			this.exampleJsonTextBox.TabIndex = 7;
			this.exampleJsonTextBox.TabStop = false;
			this.exampleJsonTextBox.WordWrap = false;
			// 
			// exampleJsonLabel
			// 
			this.exampleJsonLabel.AutoSize = true;
			this.exampleJsonLabel.Location = new System.Drawing.Point(136, 9);
			this.exampleJsonLabel.Name = "exampleJsonLabel";
			this.exampleJsonLabel.Size = new System.Drawing.Size(72, 13);
			this.exampleJsonLabel.TabIndex = 8;
			this.exampleJsonLabel.Text = "Example Json";
			// 
			// ValuesTypeDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(339, 246);
			this.ControlBox = false;
			this.Controls.Add(this.typeListView);
			this.Controls.Add(this.exampleJsonLabel);
			this.Controls.Add(this.exampleJsonTextBox);
			this.Controls.Add(this.closeButton);
			this.Controls.Add(this.disabledTypeLabel);
			this.Controls.Add(this.disabledTypeTextBox);
			this.Controls.Add(this.selectedTypeLabel);
			this.Controls.Add(this.selectedTypeTextBox);
			this.Controls.Add(this.typeLabel);
			this.Name = "ValuesTypeDialog";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Select a Values Type";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.ValuesTypeInterface_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView typeListView;
		private System.Windows.Forms.ColumnHeader valueTypeColumn;
		private System.Windows.Forms.Label typeLabel;
		private System.Windows.Forms.TextBox selectedTypeTextBox;
		private System.Windows.Forms.Label selectedTypeLabel;
		private System.Windows.Forms.TextBox disabledTypeTextBox;
		private System.Windows.Forms.Label disabledTypeLabel;
		private System.Windows.Forms.Button closeButton;
		private System.Windows.Forms.TextBox exampleJsonTextBox;
		private System.Windows.Forms.Label exampleJsonLabel;
	}
}