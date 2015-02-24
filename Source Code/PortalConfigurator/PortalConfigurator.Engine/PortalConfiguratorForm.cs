using Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace PortalConfigurator
{
	public partial class PortalConfiguratorForm : Form
	{
		private Color ChangedValueColor = Color.LemonChiffon;
		private Color UnknownValueColor = Color.Maroon;
		private Dictionary<string, Table> Tables;
		private Dictionary<string, StoredProcedure> StoredProcedures;
		private string MeasureDirectory = String.Format("{0}\\GitHub\\annie-config\\test\\table", Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
		private string FilterParameterDirectory = String.Format("{0}\\GitHub\\annie-config\\test", Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

		public PortalConfiguratorForm()
		{
			Tables = new Dictionary<string, Table>();
			StoredProcedures = new Dictionary<string, StoredProcedure>();
			InitializeComponent();
			InitializeMeasureInterface();
			InitializeFilterParameterInterface();
		}

		private void PortalConfiguratorForm_Load(object sender, EventArgs e)
		{
			bool abort = false;
			SplashScreen splash = new SplashScreen();
			splash.StartPosition = FormStartPosition.Manual;
			splash.Location = new Point(this.Location.X + (this.Width - splash.Width) / 2, this.Location.Y + (this.Height - splash.Height) / 2);
			splash.Show(this);

			do
			{
				try
				{
					Database.RefreshFromDatabase();
					Tables = Database.Tables.ToDictionary(k => k.Key, v => v.Value);
					StoredProcedures = Database.StoredProcedures.ToDictionary(k => k.Key, v => v.Value);
					LoadMeasureInterface(sender, e);
					LoadFilterParameterInterface(sender, e);
					break;
				}
				catch (CancelException)
				{
					abort = true;
				}
				catch (Exception exception)
				{
					string message = String.Format("{0}\n\nYou may retry or click Cancel to close the application.", exception.Message);
					DialogResult result = MessageBox.Show(message, "Database Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Asterisk,
						MessageBoxDefaultButton.Button1);
					abort = result == DialogResult.Cancel;
				}
			} while (!abort);

			splash.Close();

			if (abort)
				this.Close();
		}

		private void reloadFromDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EndGridCellEditMode();
			bool abort = false;

			if (!MyMeasurementFile.Equals(OriginalMeasurementFile))
			{
				string message = "The measurement file contains unsaved changes that may be lost.\nDo you wish to continue?";
				abort = MessageBox.Show(message, "Unsaved Changes", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel;
			}

			if (!MyFilterParameterFile.Equals(OriginalFilterParameterFile) && !abort)
			{
				string message = "The filter/parameter file contains unsaved changes that may be lost.\nDo you wish to continue?";
				abort = MessageBox.Show(message, "Unsaved Changes", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel;
			}

			this.Enabled = false;
			SplashScreen splash = new SplashScreen();
			splash.StartPosition = FormStartPosition.Manual;
			splash.Location = new Point(this.Location.X + (this.Width - splash.Width) / 2, this.Location.Y + (this.Height - splash.Height) / 2);
			splash.Show(this);

			while (!abort)
			{
				try
				{
					Database.RefreshFromDatabase();
					Tables = Database.Tables.ToDictionary(k => k.Key, v => v.Value);
					StoredProcedures = Database.StoredProcedures.ToDictionary(k => k.Key, v => v.Value);
					break;
				}
				catch (Exception exception)
				{
					string message = String.Format("{0}\n\nYou may retry or click Cancel to close the application.", exception.Message);
					DialogResult result = MessageBox.Show(message, "Database Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Asterisk,
						MessageBoxDefaultButton.Button1);
					abort = result == DialogResult.Cancel;
				}
			}

			splash.Close();
			this.Enabled = true;
			this.BringToFront();

			if (!abort)
			{
				filterParameterTableNameComboBox.Items.Clear();
				filterParameterTableNameComboBox.Items.AddRange(Tables.Keys.ToArray());

				if (SubjectIndex != -1)
					FilterParameterTypeSelectionChanged();

				tableComboBox.Items.Clear();
				tableComboBox.Items.Add("No Stored Procedure");
				tableComboBox.Items.AddRange(StoredProcedures.Keys.ToArray());
				string selection = (tableComboBox.SelectedIndex == -1 || tableComboBox.SelectedIndex == 0) ? String.Empty : tableComboBox.SelectedItem.ToString();
				
				if (selection != MyMeasurementFile.Table)
				{
					string message = String.Format("The \"{0}\" stored procedure was not found in the database. Transform and column header settings will be lost.", MyMeasurementFile.Table);
					MessageBox.Show(message, "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
					ChangeTable(selection);
					RefreshTableAffectedSettings();
				}
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EndGridCellEditMode();
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
			EndGridCellEditMode();
			bool measureFileHasUnsavedChanges = !MyMeasurementFile.Equals(OriginalMeasurementFile);
			bool filterParameterFileHasUnsavedChanges = !MyFilterParameterFile.Equals(OriginalFilterParameterFile);

			if (measureFileHasUnsavedChanges || filterParameterFileHasUnsavedChanges)
			{
				DialogResult result = MessageBox.Show("Unsaved changes will be lost.\nAre you sure you want to close now?",
					"Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

				e.Cancel = result != DialogResult.Yes;
			}
		}

		private void EndGridCellEditMode()
		{
			bool chartsUncommitted = chartsDataGridView.CurrentCell == null ? false : chartsDataGridView.CurrentCell.IsInEditMode;
			bool numberFormatsUncommitted = numberFormatsDataGridView.CurrentCell == null ? false : numberFormatsDataGridView.CurrentCell.IsInEditMode;
			bool measureUncommitted = measureDataGridView.CurrentCell == null ? false : measureDataGridView.CurrentCell.IsInEditMode;
			bool commentsUncommitted = commentsDataGridView.CurrentCell == null ? false : commentsDataGridView.CurrentCell.IsInEditMode;
			bool keysUncommitted = valuesDataGridView.CurrentCell == null ? false : valuesDataGridView.CurrentCell.IsInEditMode;
			bool helpUncommitted = helpDataGridView.CurrentCell == null ? false : helpDataGridView.CurrentCell.IsInEditMode;

			if (chartsUncommitted || numberFormatsUncommitted || measureUncommitted || commentsUncommitted || keysUncommitted || helpUncommitted)
				legendTextBox.Focus();
		}

		private void configTabControl_Click(object sender, EventArgs e)
		{
			EndGridCellEditMode();
		}

		private FileSaveConflictDecision CheckForNewerFile(FileInfo file, DateTime originalDate)
		{
			DateTime fileDate;
			FileSaveConflictDecision decision = FileSaveConflictDecision.Overwrite;

			try
			{
				fileDate = file.LastWriteTime;
			}
			catch (PlatformNotSupportedException)
			{
				fileDate = file.CreationTime;
			}
			catch (Exception)
			{
				throw;
			}

			if (fileDate.CompareTo(originalDate) > 0)
			{
				NewerFileDialog dialog = new NewerFileDialog();
				DialogResult result = dialog.ShowDialog();

				switch (result)
				{
					case DialogResult.Cancel:
						decision = FileSaveConflictDecision.Cancel;
						break;
					case DialogResult.Ignore:
						decision = FileSaveConflictDecision.Overwrite;
						break;
					case DialogResult.Retry:
						decision = FileSaveConflictDecision.Reload;
						break;
					default:
						break;
				}
			}

			return decision;
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Version version = Assembly.GetExecutingAssembly().GetName().Version;
			string message = String.Format("{0}\n\n{1} version {2}", Application.CompanyName, Application.ProductName, version);
			MessageBox.Show(message, "About", MessageBoxButtons.OK, MessageBoxIcon.None);
		}
	}
}
