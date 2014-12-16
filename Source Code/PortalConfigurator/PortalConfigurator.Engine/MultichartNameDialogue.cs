using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PortalConfigurator
{
	public partial class MultichartNameDialogue : Form
	{
		public string MultichartName { get; private set; }
		private string OriginalName { get; set; }
		private Color ChangedValueColor { get; set; }
		private bool Loaded { get; set; }

		public MultichartNameDialogue()
			: this(String.Empty, Color.LemonChiffon)
		{ }

		public MultichartNameDialogue(string multichartName, Color changedValueColor)
		{
			InitializeComponent();
			this.MultichartName = multichartName;
			this.OriginalName = multichartName;
			this.ChangedValueColor = changedValueColor;
			this.Loaded = false;
		}

		private void MultichartNameDialogue_Load(object sender, EventArgs e)
		{
			multichartNameTextBox.Text = MultichartName;
			Loaded = true;
		}

		private void multichartNameTextBox_TextChanged(object sender, EventArgs e)
		{
			if (Loaded & MultichartName != multichartNameTextBox.Text)
			{
				MultichartName = multichartNameTextBox.Text;
				multichartNameTextBox.BackColor = MultichartName == OriginalName ? default(Color) : ChangedValueColor;
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
