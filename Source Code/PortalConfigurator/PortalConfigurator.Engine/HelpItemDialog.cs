using mshtml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PortalConfigurator
{
	public partial class HelpItemDialog : Form
	{
		public string ItemName { get; set; }
		public string ItemContent { get; set; }
		private string OriginalName { get; set; }
		private string OriginalContent { get; set; }
		private List<string> ItemNames { get; set; }
		private bool Loaded { get; set; }
		private Color ChangedValueColor { get; set; }
		private string InitialName { get; set; }

		public HelpItemDialog()
			: this(String.Empty, String.Empty, String.Empty, String.Empty, Color.LemonChiffon, new List<string>())
		{ }

		public HelpItemDialog(string itemName, string itemContent, string originalName, string originalContent, Color changedValueColor, List<string> itemNames)
		{
			InitializeComponent();
			this.Loaded = false;
			this.ChangedValueColor = changedValueColor;
			this.ItemName = itemName;
			this.ItemContent = itemContent;
			this.OriginalName = originalName;
			this.OriginalContent = originalContent;
			this.ItemNames = itemNames;
			this.OriginalName = itemName;
			this.InitialName = itemName.ToString();
		}

		private void DictionaryItemDialog_Load(object sender, EventArgs e)
		{
			nameTextBox.Text = ItemName;
			errorProvider.SetError(nameLabel, String.Empty);
			contentWebBrowser.DocumentText = String.Format("<html><body>{0}</body></html>", ItemContent);
			Loaded = true;
		}

		private void nameTextBox_TextChanged(object sender, EventArgs e)
		{
			if (Loaded && nameTextBox.Text != ItemName)
			{
				bool invalidName = ItemNames.Contains(nameTextBox.Text) && nameTextBox.Text != InitialName;
				string errorMessage = invalidName ? "This help name is already used." : String.Empty;
				ItemName = nameTextBox.Text;
				errorProvider.SetError(nameLabel, errorMessage);
				doneButton.Enabled = !invalidName;
			}

			nameTextBox.BackColor = ItemName == OriginalName ? default(Color) : ChangedValueColor;
		}

		private void cutToolStripButton_Click(object sender, EventArgs e)
		{
			contentWebBrowser.Document.ExecCommand("Cut", false, null);
			UpdateHelpContent();
		}

		private void copyToolStripButton_Click(object sender, EventArgs e)
		{
			contentWebBrowser.Document.ExecCommand("Copy", false, null);
		}

		private void pasteToolStripButton_Click(object sender, EventArgs e)
		{
			contentWebBrowser.Document.ExecCommand("Paste", false, null);
			UpdateHelpContent();
		}

		private void boldToolStripButton_Click(object sender, EventArgs e)
		{
			contentWebBrowser.Document.ExecCommand("Bold", false, null);
			UpdateHelpContent();
		}

		private void underlineToolStripButton_Click(object sender, EventArgs e)
		{
			contentWebBrowser.Document.ExecCommand("Underline", false, null);
			UpdateHelpContent();
		}

		private void italicsToolStripButton_Click(object sender, EventArgs e)
		{
			contentWebBrowser.Document.ExecCommand("Italic", false, null);
			UpdateHelpContent();
		}

		private void createLinkToolStripButton_Click(object sender, EventArgs e)
		{
			contentWebBrowser.Document.ExecCommand("CreateLink", true, null);
			UpdateHelpContent();
		}

		private void contentWebBrowser_OnHtmlChanged(object source, EventArgs e)
		{
			UpdateHelpContent();
		}

		private void UpdateHelpContent()
		{
			if (Loaded)
			{
				string html = contentWebBrowser.Document.Body.InnerHtml.ToString();
				html = html.Replace('"', '\'');

				if (html != ItemContent)
					ItemContent = html;
			}

			//contentWebBrowser.Document.BackColor = ItemContent == OriginalContent ? Color.White : ChangedValueColor;
		}

		private void doneButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = doneButton.DialogResult;
			string html = contentWebBrowser.Document.Body.InnerHtml.ToString();
			ItemContent = html.Replace('"', '\''); ;
			this.Close();
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = cancelButton.DialogResult;
			this.Close();
		}
	}
}
