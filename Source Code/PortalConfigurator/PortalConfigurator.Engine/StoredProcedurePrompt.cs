using Framework;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PortalConfigurator
{
	public partial class StoredProcedurePrompt : Form
	{
		public List<string> StoredProcedureList { get; private set; }
		public string StoredProcedure { get; set; }
		private bool FormLoaded { get; set; }

		public StoredProcedurePrompt()
		{
			InitializeComponent();
			StoredProcedureList = new List<string>();
			StoredProcedure = String.Empty;
			FormLoaded = false;
		}

		private void StoredProcedurePrompt_Load(object sender, EventArgs e)
		{
			try
			{
				StoredProcedureList = Database.GetStoredProcedureList();
				storedProcedureComboBox.DataSource = StoredProcedureList;
				storedProcedureComboBox.SelectedIndex = StoredProcedureList.FindIndex(p => p == StoredProcedure);
				FormLoaded = true;
			}
			catch (DatabaseException exception)
			{
				string message = String.Format("There was an error when accessing the database:\n{0}", exception.Message);
				MessageBox.Show(message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				this.DialogResult = DialogResult.Cancel;
				this.Close();
			}
		}

		private void storedProcedureComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (FormLoaded)
				StoredProcedure = storedProcedureComboBox.SelectedItem.ToString();
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}
}
