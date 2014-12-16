using System;
using System.Windows.Forms;

namespace Framework
{
	public partial class CredentialsPrompt : Form
	{
		public string UserName { get; set; }
		public string Password { get; set; }

		public CredentialsPrompt()
		{
			InitializeComponent();
			UserName = String.Empty;
			Password = String.Empty;
		}

		private void Prompt_Load(object sender, EventArgs e)
		{
			if (!String.IsNullOrEmpty(UserName))
				userNameTextBox.Text = UserName;
		}

		private void confirmationButton_Click(object sender, EventArgs e)
		{
			UserName = userNameTextBox.Text;
			Password = passwordTextBox.Text;
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
