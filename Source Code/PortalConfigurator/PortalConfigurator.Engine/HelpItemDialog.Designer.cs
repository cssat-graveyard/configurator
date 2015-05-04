using CustomControls;

namespace PortalConfigurator
{
	partial class HelpItemDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpItemDialog));
			this.nameLabel = new System.Windows.Forms.Label();
			this.nameTextBox = new System.Windows.Forms.TextBox();
			this.contentLabel = new System.Windows.Forms.Label();
			this.doneButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.cutToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.copyToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.pasteToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.boldToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.underlineToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.italicsToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.createLinkToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.contentWebBrowser = new CustomControls.WebEditor();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
			this.toolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// nameLabel
			// 
			this.nameLabel.AutoSize = true;
			this.errorProvider.SetError(this.nameLabel, "Test");
			this.nameLabel.Location = new System.Drawing.Point(9, 9);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(35, 13);
			this.nameLabel.TabIndex = 0;
			this.nameLabel.Text = "Name";
			// 
			// nameTextBox
			// 
			this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.nameTextBox.Location = new System.Drawing.Point(12, 25);
			this.nameTextBox.Name = "nameTextBox";
			this.nameTextBox.Size = new System.Drawing.Size(629, 20);
			this.nameTextBox.TabIndex = 1;
			this.nameTextBox.TextChanged += new System.EventHandler(this.nameTextBox_TextChanged);
			// 
			// contentLabel
			// 
			this.contentLabel.AutoSize = true;
			this.contentLabel.Location = new System.Drawing.Point(9, 48);
			this.contentLabel.Name = "contentLabel";
			this.contentLabel.Size = new System.Drawing.Size(44, 13);
			this.contentLabel.TabIndex = 2;
			this.contentLabel.Text = "Content";
			// 
			// doneButton
			// 
			this.doneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.doneButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.doneButton.Location = new System.Drawing.Point(485, 373);
			this.doneButton.Name = "doneButton";
			this.doneButton.Size = new System.Drawing.Size(75, 23);
			this.doneButton.TabIndex = 4;
			this.doneButton.Text = "Done";
			this.doneButton.UseVisualStyleBackColor = true;
			this.doneButton.Click += new System.EventHandler(this.doneButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(566, 373);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 5;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			// 
			// toolStrip
			// 
			this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripButton,
            this.copyToolStripButton,
            this.pasteToolStripButton,
            this.toolStripSeparator,
            this.boldToolStripButton,
            this.underlineToolStripButton,
            this.italicsToolStripButton,
            this.createLinkToolStripButton});
			this.toolStrip.Location = new System.Drawing.Point(12, 67);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(170, 25);
			this.toolStrip.TabIndex = 7;
			this.toolStrip.Text = "toolStrip1";
			// 
			// cutToolStripButton
			// 
			this.cutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.cutToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripButton.Image")));
			this.cutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cutToolStripButton.Name = "cutToolStripButton";
			this.cutToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.cutToolStripButton.Text = "C&ut";
			this.cutToolStripButton.Click += new System.EventHandler(this.cutToolStripButton_Click);
			// 
			// copyToolStripButton
			// 
			this.copyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.copyToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripButton.Image")));
			this.copyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.copyToolStripButton.Name = "copyToolStripButton";
			this.copyToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.copyToolStripButton.Text = "&Copy";
			this.copyToolStripButton.Click += new System.EventHandler(this.copyToolStripButton_Click);
			// 
			// pasteToolStripButton
			// 
			this.pasteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.pasteToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripButton.Image")));
			this.pasteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.pasteToolStripButton.Name = "pasteToolStripButton";
			this.pasteToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.pasteToolStripButton.Text = "&Paste";
			this.pasteToolStripButton.Click += new System.EventHandler(this.pasteToolStripButton_Click);
			// 
			// toolStripSeparator
			// 
			this.toolStripSeparator.Name = "toolStripSeparator";
			this.toolStripSeparator.Size = new System.Drawing.Size(6, 25);
			// 
			// boldToolStripButton
			// 
			this.boldToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.boldToolStripButton.Image = global::PortalConfigurator.Properties.Resources.Bold_11689_32;
			this.boldToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.boldToolStripButton.Name = "boldToolStripButton";
			this.boldToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.boldToolStripButton.Text = "Bold";
			this.boldToolStripButton.Click += new System.EventHandler(this.boldToolStripButton_Click);
			// 
			// underlineToolStripButton
			// 
			this.underlineToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.underlineToolStripButton.Image = global::PortalConfigurator.Properties.Resources.Underline_11700_32;
			this.underlineToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.underlineToolStripButton.Name = "underlineToolStripButton";
			this.underlineToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.underlineToolStripButton.Text = "Underline";
			this.underlineToolStripButton.Click += new System.EventHandler(this.underlineToolStripButton_Click);
			// 
			// italicsToolStripButton
			// 
			this.italicsToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.italicsToolStripButton.Image = global::PortalConfigurator.Properties.Resources.Italic_11693_32;
			this.italicsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.italicsToolStripButton.Name = "italicsToolStripButton";
			this.italicsToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.italicsToolStripButton.Text = "Italics";
			this.italicsToolStripButton.Click += new System.EventHandler(this.italicsToolStripButton_Click);
			// 
			// createLinkToolStripButton
			// 
			this.createLinkToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.createLinkToolStripButton.Image = global::PortalConfigurator.Properties.Resources.HyperLInk_733_24;
			this.createLinkToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.createLinkToolStripButton.Name = "createLinkToolStripButton";
			this.createLinkToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.createLinkToolStripButton.Text = "&CreateLink";
			this.createLinkToolStripButton.ToolTipText = "Create Link";
			this.createLinkToolStripButton.Click += new System.EventHandler(this.createLinkToolStripButton_Click);
			// 
			// contentWebBrowser
			// 
			this.contentWebBrowser.AllowNavigation = false;
			this.contentWebBrowser.AllowWebBrowserDrop = false;
			this.contentWebBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.contentWebBrowser.IsWebBrowserContextMenuEnabled = false;
			this.contentWebBrowser.Location = new System.Drawing.Point(11, 98);
			this.contentWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
			this.contentWebBrowser.Name = "contentWebBrowser";
			this.contentWebBrowser.ScriptErrorsSuppressed = true;
			this.contentWebBrowser.Size = new System.Drawing.Size(629, 269);
			this.contentWebBrowser.TabIndex = 6;
			this.contentWebBrowser.OnHtmlChanged += new CustomControls.WebEditorHtmlChanged(this.contentWebBrowser_OnHtmlChanged);
			// 
			// HelpItemDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(653, 408);
			this.ControlBox = false;
			this.Controls.Add(this.toolStrip);
			this.Controls.Add(this.contentWebBrowser);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.doneButton);
			this.Controls.Add(this.contentLabel);
			this.Controls.Add(this.nameTextBox);
			this.Controls.Add(this.nameLabel);
			this.Name = "HelpItemDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Help Item Editor";
			this.Load += new System.EventHandler(this.DictionaryItemDialog_Load);
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.TextBox nameTextBox;
		private System.Windows.Forms.Label contentLabel;
		private System.Windows.Forms.Button doneButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.ErrorProvider errorProvider;
		private CustomControls.WebEditor contentWebBrowser;
		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.ToolStripButton cutToolStripButton;
		private System.Windows.Forms.ToolStripButton copyToolStripButton;
		private System.Windows.Forms.ToolStripButton pasteToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
		private System.Windows.Forms.ToolStripButton createLinkToolStripButton;
		private System.Windows.Forms.ToolStripButton italicsToolStripButton;
		private System.Windows.Forms.ToolStripButton boldToolStripButton;
		private System.Windows.Forms.ToolStripButton underlineToolStripButton;
	}
}