using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PortalConfigurator
{
	public partial class MultichartNameDialogue : Form
	{
		public string MultichartName { get; private set; }
		public List<string> ExistingNames { get; private set; }
		private string OriginalName { get; set; }
		private Color ChangedValueColor { get; set; }
		private bool Loaded { get; set; }

		public MultichartNameDialogue()
			: this(String.Empty, new List<string>(), Color.LemonChiffon)
		{ }

		public MultichartNameDialogue(string multichartName, List<string> existingNames, Color changedValueColor)
		{
			InitializeComponent();
			this.MultichartName = multichartName;
			this.ExistingNames = existingNames;
			this.OriginalName = multichartName;
			this.ChangedValueColor = changedValueColor;
			this.Loaded = false;
		}

		private void MultichartNameDialogue_Load(object sender, EventArgs e)
		{
			multichartNameTextBox.Text = MultichartName;
			duplicateNameErrorProvider.SetIconAlignment(multichartNameLabel, ErrorIconAlignment.MiddleRight);
			duplicateNameErrorProvider.SetIconPadding(multichartNameLabel, 2);
			SetDuplicateNameError();
			Loaded = true;
		}

		private void SetDuplicateNameError()
		{
			bool invalidName = ExistingNames.Contains(MultichartName) || String.IsNullOrEmpty(MultichartName);
			string errorMessage = invalidName ? "This multichart name is not valid." : String.Empty;
			duplicateNameErrorProvider.SetError(multichartNameLabel, errorMessage);
			doneButton.Enabled = !invalidName;
		}

		private void multichartNameTextBox_TextChanged(object sender, EventArgs e)
		{
			if (Loaded && MultichartName != multichartNameTextBox.Text)
			{
				MultichartName = multichartNameTextBox.Text;
				multichartNameTextBox.BackColor = MultichartName == OriginalName ? default(Color) : ChangedValueColor;
				SetDuplicateNameError();
			}
		}

		private void doneButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}
}
