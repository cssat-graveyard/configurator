using Framework;
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
	public partial class PortalConfiguratorForm : Form
	{
		private Color ChangedValueColor = Color.LemonChiffon;
		private Color UnknownValueColor = Color.Maroon;

		public PortalConfiguratorForm()
		{
			InitializeComponent();
			InitializeMeasureInterface();
			InitializeFilterParameterInterface();
		}

		private void PortalConfiguratorForm_Load(object sender, EventArgs e)
		{
			LoadMeasureInterface(sender, e);
			LoadFilterParameterInterface(sender, e);
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private CheckState ToCheckState(bool? value)
		{
			CheckState state = CheckState.Indeterminate;

			switch (value)
			{
				case true:
					state = CheckState.Checked;
					break;
				case null:
					break;
				case false:
					state = CheckState.Unchecked;
					break;
				default:
					break;
			}

			return state;
		}

		private bool? ToNullableBool(CheckState state)
		{
			bool? value = null;

			switch (state)
			{
				case CheckState.Checked:
					value = true;
					break;
				case CheckState.Indeterminate:
					break;
				case CheckState.Unchecked:
					value = false;
					break;
				default:
					break;
			}

			return value;
		}

		private void PortalConfiguratorForm_FormClosing(object sender, CancelEventArgs e)
		{
			bool measureFileHasUnsavedChanges = !MyMeasurementFile.Equals(OriginalMeasurementFile);
			bool filterParameterFileHasUnsavedChanges = !MyFilterParameterFile.Equals(OriginalFilterParameterFile);

			if (measureFileHasUnsavedChanges || filterParameterFileHasUnsavedChanges)
			{
				DialogResult result = MessageBox.Show("Unsaved changes will be lost.\nAre you sure you want to close now?",
					"Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

				e.Cancel = result != DialogResult.Yes;
			}
		}
	}
}
