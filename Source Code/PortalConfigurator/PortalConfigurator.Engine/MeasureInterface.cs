using Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PortalConfigurator
{
	public partial class PortalConfiguratorForm : Form
	{
		private MeasurementFile MyMeasurementFile { get; set; }
		private MeasurementFile OriginalMeasurementFile { get; set; }
		private string FilterParameterFileName { get; set; }
		private string MyFilterParameterDirectory { get; set; }
		private List<string> FilterParameterList { get; set; }
		private List<string> TableColumnList { get; set; }
		private int FilterParameterComboBoxEvents { get; set; }
		private int HeaderComboBoxEvents { get; set; }
		private bool SimpleReload { get; set; }

		public void InitializeMeasureInterface()
		{
			MyMeasurementFile = new MeasurementFile();
			OriginalMeasurementFile = new MeasurementFile();
			FilterParameterFileName = String.Empty;
			MyFilterParameterDirectory = FilterParameterDirectory;
			FilterParameterList = new List<string>();
			TableColumnList = new List<string>();
			FilterParameterComboBoxEvents = 0;
			SimpleReload = false;
		}

		public void LoadMeasureInterface(object sender, EventArgs e)
		{
			measureBreadcrumbLabel.Text = MyMeasurementFile.Breadcrumb;

			if (String.IsNullOrEmpty(FilterParameterFileName))
			{
				filterParameterFileButton.Text = "No Filter/Parameter File";
				filterParameterFileButton.ForeColor = UnknownValueColor;
			}
			else
			{
				filterParameterFileButton.Text = FilterParameterFileName;
				filterParameterFileButton.ForeColor = default(Color);
			}

			tableComboBox.Items.Add("No Stored Procedure");
			tableComboBox.Items.AddRange(StoredProcedures.Keys.ToArray<string>());
			tableComboBox.SelectedIndex = 0;
			hideRowComboBox.Items.AddRange(Enums.GetFormattedHideRowEnumNames());
			chartTypeComboBox.Items.AddRange(Enums.GetFormattedChartTypeEnumNames());
			controlTypeColumn.DataSource = Enum.GetNames(typeof(ControlType));
			functionComboBox.Items.AddRange(Enums.GetFormattedFunctionEnumNames());
			yAxisFormatComboBox.Items.AddRange(Enums.GetFormattedAxisFormatEnumNames());
			warningErrorProvider.SetError(chartTypeLabel, String.Empty);
			warningErrorProvider.SetError(xAxisLabelLabel, String.Empty);
			warningErrorProvider.SetError(yAxisLabelLabel, String.Empty);
			warningErrorProvider.SetError(yAxisMinLabel, String.Empty);
			warningErrorProvider.SetError(yAxisMaxLabel, String.Empty);
			warningErrorProvider.SetError(yAxisFormatLabel, String.Empty);
			warningErrorProvider.SetError(functionLabel, String.Empty);

			List<string> headerTypes = new List<string>();
			
			foreach (HeaderType item in Enum.GetValues(typeof(HeaderType)))
				headerTypes.Add(Enums.GetFormattedString(item));

			headerTypeColumn.DataSource = headerTypes;
		}

		private void newMeasurementFileToolStripButton_Click(object sender, EventArgs e)
		{
			EndGridCellEditMode();
			bool hasUnsavedChanges = !MyMeasurementFile.Equals(OriginalMeasurementFile);
			DialogResult proceed = DialogResult.Yes;

			if (hasUnsavedChanges)
			{
				string message = "You will lose unsaved changes if you continue.\nDo you wish to continue?";
				proceed = MessageBox.Show(message, "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
			}

			if (!hasUnsavedChanges || proceed == DialogResult.Yes)
			{
				ResetMeasureInterface();
			}
		}

		private void ResetMeasureInterface()
		{
			MyMeasurementFile = new MeasurementFile();
			OriginalMeasurementFile = MyMeasurementFile.Clone();

			measureBreadcrumbLabel.Text = MyMeasurementFile.Breadcrumb;
			measureDataGridView.Rows.Clear();
			numberFormatsDataGridView.Rows.Clear();
			chartsDataGridView.Rows.Clear();
			multichartsComboBox.SelectedIndex = -1;
			multichartsComboBox.Items.Clear();
			tableComboBox.SelectedIndex = 0;
			tableComboBox.BackColor = default(Color);
			titleTextBox.Text = MyMeasurementFile.Title;
			titleTextBox.BackColor = default(Color);
			subtitleTextBox.Text = MyMeasurementFile.Subtitle;
			subtitleTextBox.BackColor = default(Color);
			baseButton.Text = "No Base Measure File";
			baseButton.BackColor = default(Color);
			orderNumericUpDown.Value = MyMeasurementFile.Order ?? 0;
			orderNumericUpDown.BackColor = default(Color);
			hideRowComboBox.SelectedIndex = 0;
			hideRowComboBox.BackColor = default(Color);
			dropdownTextBox.Text = MyMeasurementFile.Dropdown;
			dropdownTextBox.BackColor = default(Color);
			filterTextBox.Text = MyMeasurementFile.Filter;
			filterTextBox.BackColor = default(Color);
			maxCheckedNumericUpDown.Value = MyMeasurementFile.MaxChecked ?? 0;
			maxCheckedNumericUpDown.BackColor = default(Color);
			chartTypeComboBox.SelectedIndex = (int)MyMeasurementFile.ChartType;
			chartTypeComboBox.BackColor = default(Color);
			showAllOthersCheckBox.CheckState = ToCheckState(MyMeasurementFile.ShowAllOthers);
			showAllOthersCheckBox.BackColor = default(Color);
			muteAllOthersCheckBox.CheckState = ToCheckState(MyMeasurementFile.MutexAllOthers);
			muteAllOthersCheckBox.BackColor = default(Color);
			summaryTextBox.Text = MyMeasurementFile.Summary;
			summaryTextBox.BackColor = default(Color);
			xAxisLabelTextBox.Text = MyMeasurementFile.Label.XAxisLabel;
			xAxisLabelTextBox.BackColor = default(Color);
			yAxisLabelTextBox.Text = MyMeasurementFile.Label.YAxisLabel;
			yAxisLabelTextBox.BackColor = default(Color);
			yAxisMinNumericUpDown.Value = (decimal)(MyMeasurementFile.Label.YAxisMin ?? 0.0f);
			yAxisMinNumericUpDown.BackColor = default(Color);
			yAxisMaxNumericUpDown.Value = (decimal)(MyMeasurementFile.Label.YAxisMax ?? 0.0f);
			yAxisMaxNumericUpDown.BackColor = default(Color);
			yAxisFormatComboBox.SelectedIndex = (int)MyMeasurementFile.Label.YAxisFormat;
			yAxisFormatComboBox.BackColor = default(Color);
			functionComboBox.SelectedIndex = (int)MyMeasurementFile.Transform.Function;
			functionComboBox.BackColor = default(Color);
			parametersCountLabel.Text = "0";
			parametersNeededLabel.Text = "0";
			parametersCountLabel.ForeColor = default(Color);
			chartListRadioButton.Checked = false;
			chartListRadioButton.BackColor = default(Color);
			multichartsRadioButton.Checked = false;
			multichartsRadioButton.BackColor = default(Color);
			multichartsComboBox.Enabled = false;
			multichartsComboBox.BackColor = default(Color);
			addMultichartsButton.Enabled = false;
			deleteMultichartsButton.Enabled = false;
			moveUpMultichartsButton.Enabled = false;
			moveDownMultichartsButton.Enabled = false;
			changeMultichartNameButton.Enabled = false;
		}

		private void openMeasureFileToolStripButton_Click(object sender, EventArgs e)
		{
			EndGridCellEditMode();
			bool hasUnsavedChanges = !MyMeasurementFile.Equals(OriginalMeasurementFile);
			DialogResult proceed = DialogResult.Yes;

			if (hasUnsavedChanges)
			{
				string message = "You will lose unsaved changes if you continue.\nDo you wish to continue?";
				proceed = MessageBox.Show(message, "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
			}

			if (!hasUnsavedChanges || proceed == DialogResult.Yes)
			{
				openFileDialog.Title = "Open Measurement File";

				if (!String.IsNullOrEmpty(MyMeasurementFile.FilePath))
				{
					FileInfo filePath = new FileInfo(MyMeasurementFile.FilePath);
					openFileDialog.InitialDirectory = filePath.DirectoryName;
					openFileDialog.FileName = filePath.Name;
				}
				else
				{
					openFileDialog.InitialDirectory = MeasureDirectory;
					openFileDialog.FileName = String.Empty;
				}

				if ((SimpleReload && !String.IsNullOrEmpty(openFileDialog.FileName))? true : openFileDialog.ShowDialog() == DialogResult.OK)
				{
					bool openFileError = false;
					ResetMeasureInterface();
					MyMeasurementFile = new MeasurementFile(openFileDialog.FileName);

					try
					{
						MyMeasurementFile.ParseJson();
						List<string> tableColumns = MyMeasurementFile.TableColumns;

						if (!String.IsNullOrEmpty(MyMeasurementFile.Table) && !StoredProcedures.ContainsKey(MyMeasurementFile.Table))
						{
							string message = String.Format("The \"{0}\" stored procedure was not found in the database. Transform and column header settings will be lost.", MyMeasurementFile.Table);
							MessageBox.Show(message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
							ChangeTable(String.Empty);
						}

						List<MeasureGridRow> measureGridRows = MyMeasurementFile.MeasureGridRows;
					}
					catch (JsonParseException parseException)
					{
						MyMeasurementFile = new MeasurementFile();
						openFileError = true;
						string message = String.Format("The following problem occurred while parsing the file:\n{0}", parseException.Message);
						MessageBox.Show(message, "Parse Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					catch (Exception exception)
					{
						MyMeasurementFile = new MeasurementFile();
						openFileError = true;
						string message = String.Format("The following problem occurred while opening the file:\n{0}", exception.Message);
						MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}

					OriginalMeasurementFile = MyMeasurementFile.Clone();
					MyMeasurementFile.RemoveUnfoundColumns();

					if (String.IsNullOrEmpty(FilterParameterFileName) && !openFileError)
					{
						MessageBox.Show("Please open a parameters file in the next window to continue.", "Information");
						LoadFilterParametersFile();

						if (String.IsNullOrEmpty(FilterParameterFileName))
						{
							filterParameterFileButton.ForeColor = UnknownValueColor;
							MyMeasurementFile = new MeasurementFile();
							return;
						}
						else
						{
							filterParameterFileButton.Text = FilterParameterFileName;
							filterParameterFileButton.ForeColor = default(Color);
						}
					}

					SetDateColumnWarning();

					measureBreadcrumbLabel.Text = MyMeasurementFile.Breadcrumb;
					tableComboBox.SelectedIndex = StoredProcedures.Keys.ToList<string>().FindIndex(p => p == MyMeasurementFile.Table) + 1;
					titleTextBox.Text = MyMeasurementFile.Title;
					subtitleTextBox.Text = MyMeasurementFile.Subtitle;
					baseButton.Text = String.IsNullOrEmpty(MyMeasurementFile.BaseMeasure) ? "No Base Measure File" : MyMeasurementFile.BaseMeasure;
					orderNumericUpDown.Value = MyMeasurementFile.Order ?? 0;
					hideRowComboBox.SelectedIndex = (int)MyMeasurementFile.HideRow;
					dropdownTextBox.Text = MyMeasurementFile.Dropdown;
					filterTextBox.Text = MyMeasurementFile.Filter;
					maxCheckedNumericUpDown.Value = MyMeasurementFile.MaxChecked ?? 0;
					chartTypeComboBox.SelectedIndex = (int)MyMeasurementFile.ChartType;
					showAllOthersCheckBox.CheckState = ToCheckState(MyMeasurementFile.ShowAllOthers);
					muteAllOthersCheckBox.CheckState = ToCheckState(MyMeasurementFile.MutexAllOthers);
					summaryTextBox.Text = MyMeasurementFile.Summary;
					xAxisLabelTextBox.Text = MyMeasurementFile.Label.XAxisLabel;
					yAxisLabelTextBox.Text = MyMeasurementFile.Label.YAxisLabel;
					yAxisMinNumericUpDown.Value = (decimal)(MyMeasurementFile.Label.YAxisMin ?? 0.0f);
					yAxisMaxNumericUpDown.Value = (decimal)(MyMeasurementFile.Label.YAxisMax ?? 0.0f);
					yAxisFormatComboBox.SelectedIndex = (int)MyMeasurementFile.Label.YAxisFormat;
					functionComboBox.SelectedIndex = (int)MyMeasurementFile.Transform.Function;
					parametersNeededLabel.Text = MyMeasurementFile.DatabaseParameterCount.ToString();

					RefreshNumberFormatsGrid();

					if (MyMeasurementFile.Charts.Count != 0 && MyMeasurementFile.Multicharts.IsEmpty)
						chartListRadioButton.Checked = true;
					else if (MyMeasurementFile.Charts.Count == 0 && !MyMeasurementFile.Multicharts.IsEmpty)
						multichartsRadioButton.Checked = true;
					else
					{
						multichartsComboBox.Enabled = false;
						multichartsComboBox.Items.Clear();
						multichartsComboBox.BackColor = default(Color);
						addMultichartsButton.Enabled = false;
						deleteMultichartsButton.Enabled = false;
						moveUpMultichartsButton.Enabled = false;
						moveDownMultichartsButton.Enabled = false;
						changeMultichartNameButton.Enabled = false;
					}

					RefreshMeasureGrid();
				}
			}

			SimpleReload = false;
		}

		private void saveMeasureFileToolStripButton_Click(object sender, EventArgs e)
		{
			EndGridCellEditMode();
			saveFileDialog.Title = "Save Measurement File";

			if (!String.IsNullOrEmpty(MyMeasurementFile.FilePath))
			{
				FileInfo filePath = new FileInfo(MyMeasurementFile.FilePath);
				saveFileDialog.InitialDirectory = filePath.DirectoryName;
				saveFileDialog.FileName = filePath.Name;

				switch (CheckForNewerFile(filePath, MyMeasurementFile.FileDate))
				{
					case FileSaveConflictDecision.Overwrite:
						break;
					case FileSaveConflictDecision.Reload:
						SimpleReload = true;
						openMeasureFileToolStripButton_Click(this, new EventArgs());
						return;
					case FileSaveConflictDecision.Cancel:
						return;
					default:
						break;
				}
			}
			else
			{
				saveFileDialog.InitialDirectory = MeasureDirectory;
				saveFileDialog.FileName = String.Empty;
			}

			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				MyMeasurementFile.FilePath = saveFileDialog.FileName;
				bool hiddenChartColumnsRequiredUpdate = false;

				foreach (var item in MyMeasurementFile.Charts)
				{
					if (item.HideColumns.Any(p => p >= MyMeasurementFile.Transform.ValueFields.Count))
					{
						item.HideColumns.RemoveAll(p => p >= MyMeasurementFile.Transform.ValueFields.Count);
						hiddenChartColumnsRequiredUpdate = true;
					}
				}

				foreach (var item in MyMeasurementFile.Multicharts.Charts)
				{
					foreach (var chart in item.Value)
					{
						if (chart.HideColumns.Any(p => p >= MyMeasurementFile.Transform.ValueFields.Count))
						{
							chart.HideColumns.RemoveAll(p => p >= MyMeasurementFile.Transform.ValueFields.Count);
							hiddenChartColumnsRequiredUpdate = true;
						}
					}
				}

				if (hiddenChartColumnsRequiredUpdate)
				{
					string message = "Some charts had hidden value columns that are no longer in the cluster,\nso the hidden column lists in the affected charts were updated";
					MessageBox.Show(message, "Automatic Chart Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}

				try
				{
					MyMeasurementFile.WriteFile();
					OriginalMeasurementFile = MyMeasurementFile.Clone();

					measureBreadcrumbLabel.Text = MyMeasurementFile.Breadcrumb;
					tableComboBox.BackColor = default(Color);
					titleTextBox.BackColor = default(Color);
					subtitleTextBox.BackColor = default(Color);
					baseButton.BackColor = default(Color);
					orderNumericUpDown.BackColor = default(Color);
					hideRowComboBox.BackColor = default(Color);
					dropdownTextBox.BackColor = default(Color);
					filterTextBox.BackColor = default(Color);
					maxCheckedNumericUpDown.BackColor = default(Color);
					chartTypeComboBox.BackColor = default(Color);
					showAllOthersCheckBox.BackColor = default(Color);
					muteAllOthersCheckBox.BackColor = default(Color);
					summaryTextBox.BackColor = default(Color);
					xAxisLabelTextBox.BackColor = default(Color);
					yAxisLabelTextBox.BackColor = default(Color);
					yAxisMinNumericUpDown.BackColor = default(Color);
					yAxisMaxNumericUpDown.BackColor = default(Color);
					yAxisFormatComboBox.BackColor = default(Color);
					functionComboBox.BackColor = default(Color);

					for (int c = 0; c < numberFormatsDataGridView.Columns.Count - 1; c++)
						for (int r = 0; r < numberFormatsDataGridView.Rows.Count; r++)
							numberFormatsDataGridView[c, r].Style.BackColor = default(Color);

					chartListRadioButton.BackColor = default(Color);
					multichartsRadioButton.BackColor = default(Color);
					multichartsComboBox.BackColor = default(Color);

					for (int c = 0; c < chartsDataGridView.Columns.Count - 1; c++)
						for (int r = 0; r < chartsDataGridView.Rows.Count; r++)
							chartsDataGridView[c, r].Style.BackColor = default(Color);

					for (int c = 0; c < measureDataGridView.Columns.Count; c++)
						for (int r = 0; r < measureDataGridView.Rows.Count; r++)
							measureDataGridView[c, r].Style.BackColor = default(Color);
				}
				catch (Exception exception)
				{
					string message = String.Format("The following problem occurred while writing the file:\n{0}", exception.Message);
					MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void LoadFilterParametersFile()
		{
			string[] filterParameterArray = FilterParameterList.ToArray();
			string fileName = FilterParameterFileName.ToString();
			openFileDialog.Title = "Open Filter/Parameter File";
			openFileDialog.InitialDirectory = MyFilterParameterDirectory;
			openFileDialog.FileName = FilterParameterFileName;

			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				try
				{
					FilterParameterList.Clear();
					FilterParameterFileName = openFileDialog.SafeFileName;
					FilterParameterFile filterParameters = new FilterParameterFile(openFileDialog.FileName);
					filterParameters.ParseJson();

					foreach (var item in filterParameters.FilterParameters)
						FilterParameterList.Add(item.FilterParameterName);

					FilterParameterList.Sort();
					controlParameterColumn.DataSource = FilterParameterList;
					List<RemovedParameter> removalEntries = new List<RemovedParameter>();

					foreach (var item in MyMeasurementFile.MeasureGridRows)
					{
						if (String.IsNullOrEmpty(item.FilterParameter) ? false : !FilterParameterList.Contains(item.FilterParameter))
							removalEntries.Add(new RemovedParameter(item.FilterParameter, ControlType.Parameter, item.IsDate, item.IsRequired, item.HeaderText));
					}

					if (removalEntries.Count != 0)
					{
						string message = "The following parameters were not present in the parameter file and were removed from the measure:\n\n";
						message = String.Concat(message, "Name\tType\tDate\tRequired\tHeader\n");

						foreach (var item in removalEntries)
						{
							MyMeasurementFile.MeasureGridRows.RemoveAll(p => p.FilterParameter == item.Name);
							string line = String.Format("{0}\t{1}\t{2}\t{3}\t{4}\n", item.Name, item.ControlType, item.IsDate, item.IsRequired, item.HeaderName);
							message = String.Concat(message, line);
						}

						MyMeasurementFile.MeasureGridRowsAreUpdated = false;
						RefreshMeasureGrid();
						message = String.Concat(message, "\nSave the measure file to make these changes permanent.");
						MessageBox.Show(message, "Measure Settings Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}

					MyFilterParameterDirectory = openFileDialog.FileName;
				}
				catch (JsonParseException error)
				{
					FilterParameterFileName = fileName;
					FilterParameterList = filterParameterArray.ToList();
					MessageBox.Show(error.Message, "Json File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void RefreshMeasureGrid()
		{
			measureDataGridView.Rows.Clear();
			columnNameColumn.DataSource = MyMeasurementFile.TableColumns;

			if (MyMeasurementFile.MeasureGridRows.Count != 0)
			{
				measureDataGridView.Rows.Add(MyMeasurementFile.MeasureGridRows.Count);
				int i = 0;

				foreach (var subject in MyMeasurementFile.MeasureGridRows)
				{
					RefreshMeasureGridRow(i, subject);
					i++;
				}
			}
		}

		private void RefreshMeasureGridRow(int row, MeasureGridRow subject)
		{
			if (MyMeasurementFile.MeasureGridRows.Count != 0)
			{
				MeasureGridRow original = OriginalMeasurementFile.MeasureGridRows.FirstOrDefault(p => p.FilterParameter == subject.FilterParameter &&
					p.HeaderType == subject.HeaderType && p.ColumnName == subject.ColumnName) ?? new MeasureGridRow(MyMeasurementFile.Transform.DateField); ;
				DataGridViewCell controlParameterCell = measureDataGridView[controlParameterColumn.Index, row];
				DataGridViewCell controlTypeCell = measureDataGridView[controlTypeColumn.Index, row];
				DataGridViewCell isDateCell = measureDataGridView[isDateColumn.Index, row];
				DataGridViewCell isRequiredCell = measureDataGridView[isRequiredColumn.Index, row];
				DataGridViewCell columnNameCell = measureDataGridView[columnNameColumn.Index, row];
				DataGridViewCell headerTypeCell = measureDataGridView[headerTypeColumn.Index, row];
				DataGridViewCell headerTextCell = measureDataGridView[headerTextColumn.Index, row];
				DataGridViewCell isReturnRowCell = measureDataGridView[isReturnRowColumn.Index, row];
				DataGridViewCell isReturnRowDateCell = measureDataGridView[isReturnRowDateColumn.Index, row];
				DataGridViewCell isReturnRowControlCell = measureDataGridView[isReturnRowControlColumn.Index, row];
				DataGridViewCell isValueFieldCell = measureDataGridView[isValueFieldColumn.Index, row];
				DataGridViewCell isRemoveFieldCell = measureDataGridView[isRemoveFieldColumn.Index, row];
				bool lockParamNameCell = subject.HeaderType == HeaderType.DateColumn || subject.IsValueField || subject.IsRemoveField;
				bool lockParamAttributeCells = lockParamNameCell || String.IsNullOrEmpty(subject.FilterParameter);
				bool lockHeaderCells = subject.IsValueField || subject.IsRemoveField;
				bool lockTransformCells = subject.ControlType != ControlType.Neither || subject.HeaderType != HeaderType.NotAHeader;
				bool lockRemoveFieldCell = MyMeasurementFile.Transform.Function == Function.NoTransform;

				controlParameterCell.Value = subject.FilterParameter;
				controlParameterCell.Style.BackColor = subject.FilterParameter == original.FilterParameter ? default(Color) : ChangedValueColor;
				controlParameterCell.ReadOnly = lockParamNameCell;
				controlTypeCell.Value = Enum.GetName(typeof(ControlType), subject.ControlType);
				controlTypeCell.Style.BackColor = subject.ControlType == original.ControlType ? default(Color) : ChangedValueColor;
				controlTypeCell.ReadOnly = lockParamAttributeCells;
				isDateCell.Value = subject.IsDate;
				isDateCell.Style.BackColor = subject.IsDate == original.IsDate ? default(Color) : ChangedValueColor;
				isDateCell.ReadOnly = lockParamAttributeCells;
				isRequiredCell.Value = subject.IsRequired;
				isRequiredCell.Style.BackColor = subject.IsRequired == original.IsRequired ? default(Color) : ChangedValueColor;
				isRequiredCell.ReadOnly = lockParamAttributeCells;
				columnNameCell.Value = subject.ColumnName;
				columnNameCell.Style.BackColor = subject.ColumnName == original.ColumnName ? default(Color) : ChangedValueColor;
				headerTypeCell.Value = Enums.GetFormattedString(subject.HeaderType);
				headerTypeCell.Style.BackColor = subject.HeaderType == original.HeaderType ? default(Color) : ChangedValueColor;
				headerTypeCell.ReadOnly = lockHeaderCells;
				headerTextCell.Value = subject.HeaderText;
				headerTextCell.Style.BackColor = subject.HeaderText == original.HeaderText ? default(Color) : ChangedValueColor;
				isReturnRowCell.Value = subject.IsReturnRow;
				isReturnRowCell.Style.BackColor = subject.IsReturnRow == original.IsReturnRow ? default(Color) : ChangedValueColor;
				isReturnRowCell.ReadOnly = lockHeaderCells;
				isReturnRowDateCell.Value = subject.IsReturnRowDate;
				isReturnRowDateCell.Style.BackColor = subject.IsReturnRowDate == original.IsReturnRowDate ? default(Color) : ChangedValueColor;
				isReturnRowDateCell.ReadOnly = lockHeaderCells;
				isReturnRowControlCell.Value = subject.IsReturnRowControl;
				isReturnRowControlCell.Style.BackColor = subject.IsReturnRowControl == original.IsReturnRowControl ? default(Color) : ChangedValueColor;
				isReturnRowControlCell.ReadOnly = lockHeaderCells;
				isValueFieldCell.Value = subject.IsValueField;
				isValueFieldCell.Style.BackColor = subject.IsValueField == original.IsValueField ? default(Color) : ChangedValueColor;
				isValueFieldCell.ReadOnly = lockTransformCells;
				isRemoveFieldCell.Value = subject.IsRemoveField;
				isRemoveFieldCell.Style.BackColor = subject.IsRemoveField == original.IsRemoveField ? default(Color) : ChangedValueColor;
				isRemoveFieldCell.ReadOnly = lockTransformCells || lockRemoveFieldCell;
			}

			parametersCountLabel.Text = MyMeasurementFile.Parameters.Count.ToString();
			parametersCountLabel.ForeColor = MyMeasurementFile.Parameters.Count == MyMeasurementFile.DatabaseParameterCount ? default(Color) : Color.Red;
		}

		private void RefreshNumberFormatsGrid()
		{
			numberFormatsDataGridView.Rows.Clear();

			if (MyMeasurementFile.NumberFormats.Count != 0)
			{
				NumberFormat format;
				NumberFormat original;
				DataGridViewCell prefixCell;
				DataGridViewCell groupingCell;
				DataGridViewCell patternCell;
				DataGridViewCell decimalCell;
				DataGridViewCell fractionDigitsCell;
				DataGridViewCell suffixCell;
				DataGridViewCell negativeParensCell;
				DataGridViewCell negativeColorButtonCell;

				numberFormatsDataGridView.Rows.Add(MyMeasurementFile.NumberFormats.Count);

				for (int i = 0; i < MyMeasurementFile.NumberFormats.Count; i++)
				{
					format = MyMeasurementFile.NumberFormats[i];
					original = OriginalMeasurementFile.NumberFormats.Count == 0 ? new NumberFormat() : OriginalMeasurementFile.NumberFormats[i];
					prefixCell = numberFormatsDataGridView[prefixColumn.Index, i];
					groupingCell = numberFormatsDataGridView[groupingColumn.Index, i];
					patternCell = numberFormatsDataGridView[patternColumn.Index, i];
					decimalCell = numberFormatsDataGridView[decimalColumn.Index, i];
					fractionDigitsCell = numberFormatsDataGridView[fractionDigitsColumn.Index, i];
					suffixCell = numberFormatsDataGridView[suffixColumn.Index, i];
					negativeParensCell = numberFormatsDataGridView[negativeParensColumn.Index, i];
					negativeColorButtonCell = numberFormatsDataGridView[negativeColorButtonColumn.Index, i];

					prefixCell.Value = format.Prefix;
					prefixCell.Style.BackColor = format.Prefix == original.Prefix ? default(Color) : ChangedValueColor;
					groupingCell.Value = format.GroupingSymbol;
					groupingCell.Style.BackColor = format.GroupingSymbol == original.GroupingSymbol ? default(Color) : ChangedValueColor;
					patternCell.Value = format.Pattern;
					patternCell.Style.BackColor = format.Pattern == original.Pattern ? default(Color) : ChangedValueColor;
					decimalCell.Value = format.DecimalSymbol;
					decimalCell.Style.BackColor = format.DecimalSymbol == original.DecimalSymbol ? default(Color) : ChangedValueColor;
					fractionDigitsCell.Value = format.FractionDigits;
					fractionDigitsCell.Style.BackColor = format.FractionDigits == original.FractionDigits ? default(Color) : ChangedValueColor;
					suffixCell.Value = format.Suffix;
					suffixCell.Style.BackColor = format.Suffix == original.Suffix ? default(Color) : ChangedValueColor;
					negativeParensCell.Value = format.NegativeParens;
					negativeParensCell.Style.BackColor = format.NegativeParens == original.NegativeParens ? default(Color) : ChangedValueColor;

					try
					{
						Color buttonColor = ColorTranslator.FromHtml(format.NegativeColor);
						negativeColorButtonCell.Value = buttonColor.Name == "0" ? "No Color" : buttonColor.Name;
					}
					catch (Exception)
					{
						negativeColorButtonCell.Value = "No Color";
					}
				}
			}
		}

		private void UpdateChartsGridRow(int m, int i, ChartInfo subject)
		{
			string multichartName = m == -1 ? String.Empty : MyMeasurementFile.Multicharts.Charts.ElementAt(m).Key;
			string chartType = subject.ChartType == ChartType.NoChartType ? String.Empty : Enum.GetName(typeof(ChartType), subject.ChartType);
			ChartInfo original = GetOriginalChartInfo(multichartName, subject);
			DataGridViewCell chartIdCell = chartsDataGridView[chartIdColumn.Index, i];
			DataGridViewCell chartTypeCell = chartsDataGridView[chartTypeColumn.Index, i];
			DataGridViewCell maxSetsCell = chartsDataGridView[maxSetsColumn.Index, i];
			DataGridViewCell widthCell = chartsDataGridView[widthColumn.Index, i];
			DataGridViewCell heightCell = chartsDataGridView [heightColumn.Index, i];
			DataGridViewCell additionalOptionsCell = chartsDataGridView[additionalOptionsColumn.Index, i];

			chartIdCell.Value = subject.ChartId;
			chartIdCell.Style.BackColor = subject.ChartId == original.ChartId ? default(Color) : ChangedValueColor;
			chartTypeCell.Value = chartType;
			chartTypeCell.Style.BackColor = subject.ChartType == original.ChartType ? default(Color) : ChangedValueColor;
			maxSetsCell.Value = subject.MaxSets;
			maxSetsCell.Style.BackColor = subject.MaxSets == original.MaxSets ? default(Color) : ChangedValueColor;
			widthCell.Value = subject.BaseOptionSet.Width;
			widthCell.Style.BackColor = subject.BaseOptionSet.Width == original.BaseOptionSet.Width ? default(Color) : ChangedValueColor;
			heightCell.Value = subject.BaseOptionSet.Height;
			heightCell.Style.BackColor = subject.BaseOptionSet.Height == original.BaseOptionSet.Height ? default(Color) : ChangedValueColor;
			additionalOptionsCell.Value = "Edit Chart";
		}

		private ChartInfo GetOriginalChartInfo(string multichartName, ChartInfo subject)
		{
			ChartInfo original = new ChartInfo();

			if (String.IsNullOrEmpty(multichartName) && OriginalMeasurementFile.Charts.FindIndex(p => p.ChartId == subject.ChartId) != -1)
				original = OriginalMeasurementFile.Charts.Find(p => p.ChartId == subject.ChartId);
			else if (OriginalMeasurementFile.Multicharts.Charts.ContainsKey(multichartName))
				if (OriginalMeasurementFile.Multicharts.Charts[multichartName].FindIndex(p => p.ChartId == subject.ChartId) != -1)
					original = MyMeasurementFile.Multicharts.Charts[multichartName].Find(p => p.ChartId == subject.ChartId);

			return original;
		}

		private void filterParameterFileButton_Click(object sender, EventArgs e)
		{
			EndGridCellEditMode();
			if (!String.IsNullOrEmpty(FilterParameterFileName))
			{
				string message = "By changing the Filter/Parameter file,\nsettings related to items not in the new file\nwill be lost.";
				MessageBox.Show(message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}

			LoadFilterParametersFile();
			filterParameterFileButton.Text = String.IsNullOrEmpty(FilterParameterFileName) ? "No Filter/Parameter File" : FilterParameterFileName;
			filterParameterFileButton.ForeColor = String.IsNullOrEmpty(FilterParameterFileName) ? UnknownValueColor : default(Color);

			if (!String.IsNullOrEmpty(MyMeasurementFile.Table))
				RefreshMeasureGrid();
		}

		private void tableComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			string selection = (tableComboBox.SelectedIndex == -1 || tableComboBox.SelectedIndex == 0) ? String.Empty : tableComboBox.SelectedItem.ToString();

			if (selection != MyMeasurementFile.Table)
			{
				DialogResult prompt = DialogResult.OK;

				if (!String.IsNullOrEmpty(MyMeasurementFile.Table))
				{
					string message = "By changing the refrenced stored procedure,\ntransform and column header settings will be lost.";
					prompt = MessageBox.Show(message, "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
				}

				if (prompt == DialogResult.OK)
				{
					try
					{
						ChangeTable(selection);
						RefreshTableAffectedSettings();
					}
					catch (Exception error)
					{
						MessageBox.Show(error.Message, "Process Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
		}

		private void ChangeTable(string selection)
		{
			MyMeasurementFile.Table = selection;
			MyMeasurementFile.Transform.DateField = String.Empty;
			MyMeasurementFile.Transform.ValueFields.Clear();
			MyMeasurementFile.Transform.RemoveFields.Clear();
			MyMeasurementFile.MeasureGridRows.Clear();
			MyMeasurementFile.UpdatePropertiesFromMeasureGridRows();

			foreach (var item in MyMeasurementFile.Charts.Where(p => p.HideColumns.Count != 0))
				item.HideColumns.Clear();

			foreach (var group in MyMeasurementFile.Multicharts.Charts)
				foreach (var item in group.Value.Where(p => p.HideColumns.Count != 0))
					item.HideColumns.Clear();
		}

		private void RefreshTableAffectedSettings()
		{
			parametersNeededLabel.Text = MyMeasurementFile.DatabaseParameterCount.ToString();
			parametersCountLabel.ForeColor = MyMeasurementFile.Parameters.Count == MyMeasurementFile.DatabaseParameterCount ? default(Color) : Color.Red;
			RefreshMeasureGrid();

			tableComboBox.BackColor = MyMeasurementFile.Table == OriginalMeasurementFile.Table ? default(Color) : ChangedValueColor;
			SetDateColumnWarning();

			if (chartListRadioButton.Checked)
			{
				for (int i = 0; i < MyMeasurementFile.Charts.Count; i++)
					UpdateChartsGridRow(-1, i, MyMeasurementFile.Charts.ElementAt(i));
			}
			else if (multichartsRadioButton.Checked && multichartsComboBox.SelectedIndex != -1)
			{
				string subject = multichartsComboBox.SelectedItem.ToString();

				for (int i = 0; i < MyMeasurementFile.Multicharts.Charts[subject].Count; i++)
					UpdateChartsGridRow(multichartsComboBox.SelectedIndex, i, MyMeasurementFile.Multicharts.Charts[subject][i]);
			}
		}

		private void titleTextBox_TextChanged(object sender, EventArgs e)
		{
			if (titleTextBox.Text != MyMeasurementFile.Title)
			{
				MyMeasurementFile.Title = titleTextBox.Text;
				titleTextBox.BackColor = MyMeasurementFile.Title == OriginalMeasurementFile.Title ? default(Color) : ChangedValueColor;
			}
		}

		private void subtitleTextBox_TextChanged(object sender, EventArgs e)
		{
			if (subtitleTextBox.Text != MyMeasurementFile.Subtitle)
			{
				MyMeasurementFile.Subtitle = subtitleTextBox.Text;
				subtitleTextBox.BackColor = MyMeasurementFile.Subtitle == OriginalMeasurementFile.Subtitle ? default(Color) : ChangedValueColor;
			}
		}

		private void baseButton_Click(object sender, EventArgs e)
		{
			EndGridCellEditMode();
			openFileDialog.Title = "Choose Base Measure File";
			openFileDialog.FileName = String.Empty;

			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				FileInfo baseFile = new FileInfo(openFileDialog.FileName);
				string baseName = baseFile.Name.TrimEnd(baseFile.Extension.ToCharArray());

				if (MyMeasurementFile.BaseMeasure != baseName)
				{
					MyMeasurementFile.BaseMeasure = baseName;
					baseButton.Text = baseName;
					baseButton.BackColor = MyMeasurementFile.BaseMeasure == OriginalMeasurementFile.BaseMeasure ? default(Color) : ChangedValueColor;
				}
			}
		}

		private void baseButton_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				MyMeasurementFile.BaseMeasure = String.Empty;
				baseButton.Text = "No Base Measure File";
				baseButton.BackColor = MyMeasurementFile.BaseMeasure == OriginalMeasurementFile.BaseMeasure ? default(Color) : ChangedValueColor;
			}
		}

		private void orderNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			if (MyMeasurementFile.Order != orderNumericUpDown.Value)
			{
				if (orderNumericUpDown.Value == 0)
					MyMeasurementFile.Order = null;
				else
					MyMeasurementFile.Order = (int)orderNumericUpDown.Value;
				
				orderNumericUpDown.BackColor = MyMeasurementFile.Order == OriginalMeasurementFile.Order ? default(Color) : ChangedValueColor;
			}
		}

		private void hideRowComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if ((int)MyMeasurementFile.HideRow != hideRowComboBox.SelectedIndex)
			{
				MyMeasurementFile.HideRow = (HideRow)hideRowComboBox.SelectedIndex;
				hideRowComboBox.BackColor = MyMeasurementFile.HideRow == OriginalMeasurementFile.HideRow ? default(Color) : ChangedValueColor;
			}
		}

		private void dropdownTextBox_TextChanged(object sender, EventArgs e)
		{
			if (MyMeasurementFile.Dropdown != dropdownTextBox.Text)
			{
				MyMeasurementFile.Dropdown = dropdownTextBox.Text;
				dropdownTextBox.BackColor = MyMeasurementFile.Dropdown == OriginalMeasurementFile.Dropdown ? default(Color) : ChangedValueColor;
			}
		}

		private void filterTextBox_TextChanged(object sender, EventArgs e)
		{
			if (MyMeasurementFile.Filter != filterTextBox.Text)
			{
				MyMeasurementFile.Filter = filterTextBox.Text;
				filterTextBox.BackColor = MyMeasurementFile.Filter == OriginalMeasurementFile.Filter ? default(Color) : ChangedValueColor;
			}
		}

		private void maxCheckedNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			if (MyMeasurementFile.MaxChecked != maxCheckedNumericUpDown.Value)
			{
				if (maxCheckedNumericUpDown.Value == 0)
					MyMeasurementFile.MaxChecked = null;
				else
					MyMeasurementFile.MaxChecked = (int)maxCheckedNumericUpDown.Value;

				maxCheckedNumericUpDown.BackColor = MyMeasurementFile.MaxChecked == OriginalMeasurementFile.MaxChecked ? default(Color) : ChangedValueColor;
			}
		}

		private void SetChartTypeWarning()
		{
			bool differentTypes = MyMeasurementFile.Charts.Any(p => ChartWarning.IsDifferent(p.ChartType, MyMeasurementFile.ChartType));
			differentTypes = differentTypes || MyMeasurementFile.Multicharts.Charts.Any(p => p.Value.Any(ip => ChartWarning.IsDifferent(ip.ChartType, MyMeasurementFile.ChartType)));
			warningErrorProvider.SetError(chartTypeLabel, ChartWarning.GetWarning(ChartLocation.Global, "chart type", MyMeasurementFile.ChartType, true, differentTypes));
		}

		private void chartTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if ((int)MyMeasurementFile.ChartType != chartTypeComboBox.SelectedIndex)
			{
				MyMeasurementFile.ChartType = (ChartType)chartTypeComboBox.SelectedIndex;
				chartTypeComboBox.BackColor = MyMeasurementFile.ChartType == OriginalMeasurementFile.ChartType ? default(Color) : ChangedValueColor;
			}

			SetChartTypeWarning();
		}

		private void showAllOthersCheckBox_CheckStateChanged(object sender, EventArgs e)
		{
			bool? value = ToNullableBool(showAllOthersCheckBox.CheckState);

			if (MyMeasurementFile.ShowAllOthers != value)
			{
				MyMeasurementFile.ShowAllOthers = value;
				showAllOthersCheckBox.BackColor = value == OriginalMeasurementFile.ShowAllOthers ? default(Color) : ChangedValueColor;
			}
		}

		private void muteAllOthersCheckBox_CheckStateChanged(object sender, EventArgs e)
		{
			bool? value = ToNullableBool(muteAllOthersCheckBox.CheckState);

			if (MyMeasurementFile.MutexAllOthers != value)
			{
				MyMeasurementFile.MutexAllOthers = value;
				muteAllOthersCheckBox.BackColor = value == OriginalMeasurementFile.MutexAllOthers ? default(Color) : ChangedValueColor;
			}
		}

		private void summaryTextBox_TextChanged(object sender, EventArgs e)
		{
			if (MyMeasurementFile.Summary != summaryTextBox.Text)
			{
				MyMeasurementFile.Summary = summaryTextBox.Text;
				summaryTextBox.BackColor = MyMeasurementFile.Summary == OriginalMeasurementFile.Summary ? default(Color) : ChangedValueColor;
			}
		}

		private void SetXAxisLabelWarning()
		{
			bool differentLabels = MyMeasurementFile.Charts.Any(p => ChartWarning.IsDifferent(p.Label.XAxisLabel, MyMeasurementFile.Label.XAxisLabel));
			differentLabels = differentLabels || MyMeasurementFile.Multicharts.Charts.Any(p => p.Value.Any(ip => ChartWarning.IsDifferent(ip.Label.XAxisLabel, MyMeasurementFile.Label.XAxisLabel)));
			warningErrorProvider.SetError(xAxisLabelLabel, ChartWarning.GetWarning(ChartLocation.Global, "X axis label", MyMeasurementFile.ChartType, false, differentLabels));
		}

		private void xAxisLabelTextBox_TextChanged(object sender, EventArgs e)
		{
			if (MyMeasurementFile.Label.XAxisLabel != xAxisLabelTextBox.Text)
			{
				MyMeasurementFile.Label.XAxisLabel = xAxisLabelTextBox.Text;
				xAxisLabelTextBox.BackColor = MyMeasurementFile.Label.XAxisLabel == OriginalMeasurementFile.Label.XAxisLabel ? default(Color) : ChangedValueColor;
			}

			SetXAxisLabelWarning();
		}

		private void SetYAxisLabelWarning()
		{
			bool differentLabels = MyMeasurementFile.Charts.Any(p => ChartWarning.IsDifferent(p.Label.YAxisLabel, MyMeasurementFile.Label.YAxisLabel));
			differentLabels = differentLabels || MyMeasurementFile.Multicharts.Charts.Any(p => p.Value.Any(ip => ChartWarning.IsDifferent(ip.Label.YAxisLabel, MyMeasurementFile.Label.YAxisLabel)));
			warningErrorProvider.SetError(yAxisLabelLabel, ChartWarning.GetWarning(ChartLocation.Global, "Y axis label", MyMeasurementFile.ChartType, false, differentLabels));
		}

		private void yAxisLabelTextBox_TextChanged(object sender, EventArgs e)
		{
			if (MyMeasurementFile.Label.YAxisLabel != yAxisLabelTextBox.Text)
			{
				MyMeasurementFile.Label.YAxisLabel = yAxisLabelTextBox.Text;
				yAxisLabelTextBox.BackColor = MyMeasurementFile.Label.YAxisLabel == OriginalMeasurementFile.Label.YAxisLabel ? default(Color) : ChangedValueColor;
			}

			SetYAxisLabelWarning();
		}

		private void SetYAxisMinWarning()
		{
			bool differentMinimums = MyMeasurementFile.Charts.Any(p => ChartWarning.IsDifferent(p.Label.YAxisMin, MyMeasurementFile.Label.YAxisMin));
			differentMinimums = differentMinimums || MyMeasurementFile.Multicharts.Charts.Any(p => p.Value.Any(ip => ChartWarning.IsDifferent(ip.Label.YAxisMin, MyMeasurementFile.Label.YAxisMin)));
			warningErrorProvider.SetError(yAxisMinLabel, ChartWarning.GetWarning(ChartLocation.Global, "Y axis minimum", MyMeasurementFile.ChartType, false, differentMinimums));
		}

		private void yAxisMinNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			if ((MyMeasurementFile.Label.YAxisMin ?? 0.0f) != (float)yAxisMinNumericUpDown.Value)
			{
				if (yAxisMinNumericUpDown.Value == (decimal)0.0)
					MyMeasurementFile.Label.YAxisMin = null;
				else
					MyMeasurementFile.Label.YAxisMin = (float)yAxisMinNumericUpDown.Value;

				yAxisMinNumericUpDown.BackColor = MyMeasurementFile.Label.YAxisMin == OriginalMeasurementFile.Label.YAxisMin ? default(Color) : ChangedValueColor;
			}

			SetYAxisMinWarning();
		}

		private void SetYAxisMaxWarning()
		{
			bool differentMaximums = MyMeasurementFile.Charts.Any(p => ChartWarning.IsDifferent(p.Label.YAxisMax, MyMeasurementFile.Label.YAxisMax));
			differentMaximums = differentMaximums || MyMeasurementFile.Multicharts.Charts.Any(p => p.Value.Any(ip => ChartWarning.IsDifferent(ip.Label.YAxisMax, MyMeasurementFile.Label.YAxisMax)));
			warningErrorProvider.SetError(yAxisMinLabel, ChartWarning.GetWarning(ChartLocation.Global, "Y axis maximum", MyMeasurementFile.ChartType, false, differentMaximums));
		}

		private void yAxisMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			if ((MyMeasurementFile.Label.YAxisMax ?? 0.0f) != (float)yAxisMaxNumericUpDown.Value)
			{
				if (yAxisMaxNumericUpDown.Value == (decimal)0.0)
					MyMeasurementFile.Label.YAxisMax = null;
				else
					MyMeasurementFile.Label.YAxisMax = (float)yAxisMaxNumericUpDown.Value;

				yAxisMaxNumericUpDown.BackColor = MyMeasurementFile.Label.YAxisMax == OriginalMeasurementFile.Label.YAxisMax ? default(Color) : ChangedValueColor;
			}

			SetYAxisMaxWarning();
		}

		private void SetYAxisFormatWarning()
		{
			bool differentFormats = MyMeasurementFile.Charts.Any(p => ChartWarning.IsDifferent(p.Label.YAxisFormat, MyMeasurementFile.Label.YAxisFormat));
			differentFormats = differentFormats || MyMeasurementFile.Multicharts.Charts.Any(p => p.Value.Any(ip => ChartWarning.IsDifferent(ip.Label.YAxisFormat, MyMeasurementFile.Label.YAxisFormat)));
			warningErrorProvider.SetError(yAxisFormatLabel, ChartWarning.GetWarning(ChartLocation.Global, "Y axis format", MyMeasurementFile.ChartType, false, differentFormats));
		}

		private void yAxisFormatComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if ((int)MyMeasurementFile.Label.YAxisFormat != yAxisFormatComboBox.SelectedIndex)
			{
				MyMeasurementFile.Label.YAxisFormat = (AxisFormat)yAxisFormatComboBox.SelectedIndex;
				yAxisFormatComboBox.BackColor = MyMeasurementFile.Label.YAxisFormat == OriginalMeasurementFile.Label.YAxisFormat ? default(Color) : ChangedValueColor;
			}
		}

		private void functionComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			bool requiresDateColumn = TransformRule.RequiresDateRow((Function)functionComboBox.SelectedIndex);

			if ((int)MyMeasurementFile.Transform.Function != functionComboBox.SelectedIndex)
			{
				MyMeasurementFile.Transform.Function = (Function)functionComboBox.SelectedIndex;
				functionComboBox.BackColor = MyMeasurementFile.Transform.Function == OriginalMeasurementFile.Transform.Function ? default(Color) : ChangedValueColor;

				if (MyMeasurementFile.Transform.Function == Function.NoTransform && MyMeasurementFile.MeasureGridRows.Any(p => p.IsRemoveField))
				{
					foreach (var item in MyMeasurementFile.MeasureGridRows.Where(p => p.IsRemoveField))
						item.IsRemoveField = false;

					MyMeasurementFile.UpdatePropertiesFromMeasureGridRows();
					RefreshMeasureGrid();
				}

				if (!requiresDateColumn)
				{
					MeasureGridRow dateColumn = MyMeasurementFile.MeasureGridRows.FirstOrDefault(p => p.HeaderType == HeaderType.DateColumn);

					if (dateColumn != null)
					{
						if (dateColumn.ControlType == ControlType.Neither && !dateColumn.IsReturnRow && !dateColumn.IsReturnRowDate && !dateColumn.IsReturnRowControl)
							MyMeasurementFile.MeasureGridRows.Remove(dateColumn);
						else if (dateColumn.ControlType == ControlType.Neither)
							dateColumn.HeaderType = HeaderType.ColumnName;
						else
							dateColumn.HeaderType = HeaderType.FilterParameterName;

						MyMeasurementFile.UpdatePropertiesFromMeasureGridRows();
						RefreshMeasureGrid();
					}
				}

				SetDateColumnWarning();
			}
		}

		private void SetDateColumnWarning()
		{
			bool requiresDateColumn = MyMeasurementFile.Transform.RequiresDateRow;
			bool isMissingDateColumn = MyMeasurementFile.MeasureGridRows.FindIndex(p => p.HeaderType == HeaderType.DateColumn) == -1;
			string warningMessage = requiresDateColumn && isMissingDateColumn ? "A column must be supplied as the date header." : String.Empty;
			warningErrorProvider.SetError(functionLabel, warningMessage);
		}

		private void numberFormatsDataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			int columnIndex = numberFormatsDataGridView.CurrentCell.ColumnIndex;

			if (columnIndex == negativeParensColumn.Index)
				numberFormatsDataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}

		private void numberFormatsDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex != -1 ? numberFormatsDataGridView.CurrentCell.IsInEditMode && numberFormatsDataGridView.CurrentCell.ColumnIndex == e.ColumnIndex : false)
			{
				NumberFormat format = MyMeasurementFile.NumberFormats[e.RowIndex];
				bool originalExists = OriginalMeasurementFile.NumberFormats.Count > e.RowIndex;
				bool changeDetected = false;
				NumberFormat original = originalExists ? OriginalMeasurementFile.NumberFormats[e.RowIndex] : new NumberFormat();
				DataGridViewCell currentCell = numberFormatsDataGridView[e.ColumnIndex, e.RowIndex];

				if (e.ColumnIndex == prefixColumn.Index)
				{
					string cellValue = (currentCell.Value ?? (object)String.Empty).ToString();

					if (format.Prefix != cellValue)
					{
						format.Prefix = cellValue;
						changeDetected = !originalExists || format.Prefix != original.Prefix;
					}
				}
				else if (e.ColumnIndex == groupingColumn.Index)
				{
					string cellValue = (currentCell.Value ?? (object)String.Empty).ToString();

					if (format.GroupingSymbol != cellValue)
					{
						format.GroupingSymbol = cellValue;
						changeDetected = !originalExists || format.GroupingSymbol != original.GroupingSymbol;
					}
				}
				else if (e.ColumnIndex == patternColumn.Index)
				{
					string cellValue = (currentCell.Value ?? (object)String.Empty).ToString();

					if (format.Pattern != cellValue)
					{
						format.Pattern = cellValue;
						changeDetected = !originalExists || format.Pattern != original.Pattern;
					}
				}
				else if (e.ColumnIndex == decimalColumn.Index)
				{
					string cellValue = (currentCell.Value ?? (object)String.Empty).ToString();

					if (format.DecimalSymbol != cellValue)
					{
						format.DecimalSymbol = cellValue;
						changeDetected = !originalExists || format.DecimalSymbol != original.DecimalSymbol;
					}
				}
				else if (e.ColumnIndex == fractionDigitsColumn.Index)
				{
					string cellValue = (currentCell.Value ?? (object)String.Empty).ToString();
					int intValue;

					if (int.TryParse(cellValue, out intValue) || String.IsNullOrEmpty(cellValue))
					{
						if (format.FractionDigits.HasValue && String.IsNullOrEmpty(cellValue))
							format.FractionDigits = null;
						else if (format.FractionDigits != (int?)intValue)
							format.FractionDigits = intValue;
					}
					else
						currentCell.Value = format.FractionDigits;

					changeDetected = !originalExists || format.FractionDigits != original.FractionDigits;
				}
				else if (e.ColumnIndex == suffixColumn.Index)
				{
					string cellValue = (currentCell.Value ?? (object)String.Empty).ToString();

					if (format.Suffix != cellValue)
					{
						format.Suffix = cellValue;
						changeDetected = !originalExists || format.Suffix != original.Suffix;
					}
				}
				else if (e.ColumnIndex == negativeParensColumn.Index)
				{
					bool? cellValue = ToNullableBool((CheckState)currentCell.Value);

					if (format.NegativeParens != cellValue)
					{
						format.NegativeParens = cellValue;
						changeDetected = !originalExists || format.NegativeParens != original.NegativeParens;
					}
				}

				currentCell.Style.BackColor = changeDetected ? ChangedValueColor : default(Color);
				numberFormatsDataGridView.EndEdit();
			}
		}

		private void numberFormatsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			EndGridCellEditMode();

			if (e.ColumnIndex == negativeColorButtonColumn.Index)
			{
				NumberFormat format = MyMeasurementFile.NumberFormats[e.RowIndex];
				NumberFormat original = OriginalMeasurementFile.NumberFormats.Count > e.RowIndex ? OriginalMeasurementFile.NumberFormats[e.RowIndex] : new NumberFormat();
				DataGridViewButtonCell negativeColorButtonCell = (DataGridViewButtonCell)numberFormatsDataGridView[negativeColorButtonColumn.Index, e.RowIndex];
				colorDialog.Color = negativeColorButtonCell.Style.BackColor;

				if (colorDialog.ShowDialog() == DialogResult.OK && negativeColorButtonCell.Style.BackColor != colorDialog.Color)
				{
					if (colorDialog.Color == Color.Black || colorDialog.Color.Name == "0")
						format.NegativeColor = String.Empty;
					else
						format.NegativeColor = ColorTranslator.ToHtml(colorDialog.Color);

					negativeColorButtonCell.Value = String.IsNullOrEmpty(format.NegativeColor) ? "No Color" : format.NegativeColor;
				}
			}
		}

		private void addNumberFormatButton_Click(object sender, EventArgs e)
		{
			EndGridCellEditMode();
			MyMeasurementFile.NumberFormats.Add(new NumberFormat());
			RefreshNumberFormatsGrid();
			numberFormatsDataGridView.CurrentCell = numberFormatsDataGridView[0, numberFormatsDataGridView.Rows.Count - 1];
		}

		private void deleteNumberFormatButton_Click(object sender, EventArgs e)
		{
			EndGridCellEditMode();
			int row = numberFormatsDataGridView.CurrentCell.RowIndex;

			if (MyMeasurementFile.NumberFormats.Count != 0 && row != -1)
			{
				MyMeasurementFile.NumberFormats.RemoveAt(row);
				RefreshNumberFormatsGrid();

				if (row - 1 >= 0)
					numberFormatsDataGridView.CurrentCell = numberFormatsDataGridView[0, row - 1];
			}
		}

		private void moveUpNumberFormatButton_Click(object sender, EventArgs e)
		{
			EndGridCellEditMode();
			int row = numberFormatsDataGridView.CurrentCell.RowIndex;

			if (row > 0)
			{
				int column = numberFormatsDataGridView.CurrentCell.ColumnIndex;
				NumberFormat subject = MyMeasurementFile.NumberFormats[row];
				MyMeasurementFile.NumberFormats.RemoveAt(row);
				MyMeasurementFile.NumberFormats.Insert(row - 1, subject);
				RefreshNumberFormatsGrid();
				numberFormatsDataGridView.CurrentCell = numberFormatsDataGridView[column, row - 1];
			}
		}

		private void moveDownNumberFormatButton_Click(object sender, EventArgs e)
		{
			EndGridCellEditMode();
			int row = numberFormatsDataGridView.CurrentCell.RowIndex;

			if (row < numberFormatsDataGridView.Rows.Count - 1 && row != -1)
			{
				int column = numberFormatsDataGridView.CurrentCell.ColumnIndex;
				NumberFormat subject = MyMeasurementFile.NumberFormats[row];
				MyMeasurementFile.NumberFormats.RemoveAt(row);
				MyMeasurementFile.NumberFormats.Insert(row + 1, subject);
				RefreshNumberFormatsGrid();
				numberFormatsDataGridView.CurrentCell = numberFormatsDataGridView[column, row + 1];
			}
		}

		private void SetChartSettingPropagationWarnings()
		{
			SetChartTypeWarning();
			SetXAxisLabelWarning();
			SetYAxisLabelWarning();
			SetYAxisMinWarning();
			SetYAxisMaxWarning();
			SetYAxisFormatWarning();
		}

		private void chartListRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			EndGridCellEditMode();

			if (chartListRadioButton.Checked && !MyMeasurementFile.Multicharts.IsEmpty)
			{
				if (ConfirmChangeChartSetting())
				{
					MyMeasurementFile.Multicharts = new Multicharts();
					chartsDataGridView.Rows.Clear();
					multichartsComboBox.SelectedIndex = -1;
					multichartsComboBox.Items.Clear();
					ChangeChartSetting();
				}
				else
					multichartsRadioButton.Checked = true;
			}
			else if (chartListRadioButton.Checked)
				ChangeChartSetting();

			SetChartSettingPropagationWarnings();
		}

		private void multichartsRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			EndGridCellEditMode();

			if (multichartsRadioButton.Checked && MyMeasurementFile.Charts.Count != 0)
			{
				if (ConfirmChangeChartSetting())
				{
					MyMeasurementFile.Charts.Clear();
					chartsDataGridView.Rows.Clear();
					ChangeChartSetting();
				}
				else
					chartListRadioButton.Checked = true;
			}
			else if (multichartsRadioButton.Checked)
				ChangeChartSetting();

			SetChartSettingPropagationWarnings();
		}

		private bool ConfirmChangeChartSetting()
		{
			string message = "By changing the charts setting,\nthe existing chart data will be lost.\n\nDo you want to continue?";
			return MessageBox.Show(message, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
		}

		private void ChangeChartSetting()
		{
			if (chartListRadioButton.Checked)
			{
				chartsDataGridView.Rows.Clear();
				multichartsComboBox.Enabled = false;
				multichartsComboBox.Items.Clear();
				multichartsComboBox.SelectedIndex = -1;
				addMultichartsButton.Enabled = false;
				deleteMultichartsButton.Enabled = false;
				moveUpMultichartsButton.Enabled = false;
				moveDownMultichartsButton.Enabled = false;
				changeMultichartNameButton.Enabled = false;

				if (MyMeasurementFile.Charts.Count != 0)
				{
					chartsDataGridView.Rows.Add(MyMeasurementFile.Charts.Count);

					for (int i = 0; i < MyMeasurementFile.Charts.Count; i++)
						UpdateChartsGridRow(-1, i, MyMeasurementFile.Charts[i]);
				}

				if (OriginalMeasurementFile.Charts.Count != 0 || OriginalMeasurementFile.Multicharts.Charts.Count != 0)
				{
					chartListRadioButton.BackColor = MyMeasurementFile.Charts.SequenceEqual(OriginalMeasurementFile.Charts) ? default(Color) : ChangedValueColor;
					multichartsRadioButton.BackColor = default(Color);
				}
			}
			else if (multichartsRadioButton.Checked)
			{
				multichartsComboBox.Enabled = true;
				addMultichartsButton.Enabled = true;
				deleteMultichartsButton.Enabled = true;
				moveUpMultichartsButton.Enabled = true;
				moveDownMultichartsButton.Enabled = true;
				changeMultichartNameButton.Enabled = true;

				if (!MyMeasurementFile.Multicharts.IsEmpty)
				{
					multichartsComboBox.Items.AddRange(MyMeasurementFile.Multicharts.Charts.Keys.ToArray<string>());
					multichartsComboBox.SelectedIndex = 0;
				}

				if (OriginalMeasurementFile.Charts.Count != 0 || OriginalMeasurementFile.Multicharts.Charts.Count != 0)
				{
					chartListRadioButton.BackColor = default(Color);
					multichartsRadioButton.BackColor = MyMeasurementFile.Multicharts.Equals(OriginalMeasurementFile.Multicharts) ? default(Color) : ChangedValueColor;
				}
			}
		}

		private void multichartsComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			EndGridCellEditMode();

			if (multichartsComboBox.SelectedIndex != -1)
			{
				chartsDataGridView.Rows.Clear();
				List<ChartInfo> charts = MyMeasurementFile.Multicharts.Charts.ElementAt(multichartsComboBox.SelectedIndex).Value;

				if (charts.Count != 0)
				{
					chartsDataGridView.Rows.Add(charts.Count);

					for (int i = 0; i < charts.Count; i++)
						UpdateChartsGridRow(multichartsComboBox.SelectedIndex, i, charts[i]);
				}
			}
		}

		private void addMultichartsButton_Click(object sender, EventArgs e)
		{
			EndGridCellEditMode();
			MultichartNameDialogue dialogue = new MultichartNameDialogue(String.Empty, MyMeasurementFile.Multicharts.Charts.Keys.ToList<string>(), ChangedValueColor);
			
			if (dialogue.ShowDialog() == DialogResult.OK)
			{
				string name = dialogue.MultichartName.Replace(' ', '-');
				MyMeasurementFile.Multicharts.Charts.Add(name, new List<ChartInfo>());
				multichartsComboBox.Items.Add(name);
				multichartsComboBox.SelectedIndex = multichartsComboBox.Items.Count - 1;
			}
		}

		private void deleteMultichartsButton_Click(object sender, EventArgs e)
		{
			EndGridCellEditMode();
			int selectedIndex = multichartsComboBox.SelectedIndex;

			if (selectedIndex != -1)
			{
				DialogResult proceed = DialogResult.Yes;
				string multichart = MyMeasurementFile.Multicharts.Charts.ElementAt(selectedIndex).Key;

				if (MyMeasurementFile.Multicharts.Charts[multichart].Count != 0)
				{
					string message = "By deleting the current multichart,\nall the associated charts will be deleted.\n\nDo you want to continue?";
					proceed = MessageBox.Show(message, "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
				}

				if (proceed == DialogResult.Yes)
				{
					Dictionary<string, List<ChartInfo>> newList = new Dictionary<string, List<ChartInfo>>();

					foreach (var item in MyMeasurementFile.Multicharts.Charts)
					{
						if (item.Key != multichart)
							newList.Add(item.Key, item.Value);
					}

					MyMeasurementFile.Multicharts.Charts = newList;
					multichartsComboBox.Items.RemoveAt(selectedIndex);
					multichartsComboBox.SelectedIndex = selectedIndex == 0 ? selectedIndex : selectedIndex - 1;
				}
			}

			SetChartSettingPropagationWarnings();
		}

		private void moveUpMultichartsButton_Click(object sender, EventArgs e)
		{
			EndGridCellEditMode();
			int selectedIndex = multichartsComboBox.SelectedIndex;

			if (selectedIndex > 0)
			{
				Dictionary<string, List<ChartInfo>> newList = new Dictionary<string, List<ChartInfo>>();

				for (int i = 0; i < MyMeasurementFile.Multicharts.Charts.Count; i++)
				{
					if (i == selectedIndex - 1)
						newList.Add(MyMeasurementFile.Multicharts.Charts.ElementAt(selectedIndex).Key, MyMeasurementFile.Multicharts.Charts.ElementAt(selectedIndex).Value);

					if (i != selectedIndex)
						newList.Add(MyMeasurementFile.Multicharts.Charts.ElementAt(i).Key, MyMeasurementFile.Multicharts.Charts.ElementAt(i).Value);
				}

				MyMeasurementFile.Multicharts.Charts = newList;
				multichartsComboBox.SelectedIndex = selectedIndex - 1;
			}
		}

		private void moveDownMultichartsButton_Click(object sender, EventArgs e)
		{
			EndGridCellEditMode();
			int selectedIndex = multichartsComboBox.SelectedIndex;

			if (selectedIndex < multichartsComboBox.Items.Count - 1 && selectedIndex != -1)
			{
				Dictionary<string, List<ChartInfo>> newList = new Dictionary<string, List<ChartInfo>>();

				for (int i = 0; i < MyMeasurementFile.Multicharts.Charts.Count; i++)
				{
					if (i != selectedIndex)
						newList.Add(MyMeasurementFile.Multicharts.Charts.ElementAt(i).Key, MyMeasurementFile.Multicharts.Charts.ElementAt(i).Value);

					if (i == selectedIndex + 1)
						newList.Add(MyMeasurementFile.Multicharts.Charts.ElementAt(selectedIndex).Key, MyMeasurementFile.Multicharts.Charts.ElementAt(selectedIndex).Value);
				}

				MyMeasurementFile.Multicharts.Charts = newList;
				multichartsComboBox.SelectedIndex = selectedIndex + 1;
			}
		}

		private void changeMultichartNameButton_Click(object sender, EventArgs e)
		{
			EndGridCellEditMode();
			int selectedIndex = multichartsComboBox.SelectedIndex;

			if (selectedIndex != -1)
			{
				string subject = MyMeasurementFile.Multicharts.Charts.ElementAt(selectedIndex).Key;
				List<string> otherNames = MyMeasurementFile.Multicharts.Charts.Keys.Where(p => p != subject).ToList<string>();
				MultichartNameDialogue dialogue = new MultichartNameDialogue(subject, otherNames, ChangedValueColor);

				if (dialogue.ShowDialog() == DialogResult.OK && dialogue.MultichartName != subject)
				{
					Dictionary<string, List<ChartInfo>> newList = new Dictionary<string, List<ChartInfo>>();

					for (int i = 0; i < MyMeasurementFile.Multicharts.Charts.Count; i++)
					{
						if (i == selectedIndex)
							newList.Add(dialogue.MultichartName, MyMeasurementFile.Multicharts.Charts.ElementAt(i).Value);
						else
							newList.Add(MyMeasurementFile.Multicharts.Charts.ElementAt(i).Key, MyMeasurementFile.Multicharts.Charts.ElementAt(i).Value);
					}

					MyMeasurementFile.Multicharts.Charts = newList;
					multichartsComboBox.Items[selectedIndex] = dialogue.MultichartName;
				}
			}
		}

		private void chartsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			EndGridCellEditMode();

			if (e.ColumnIndex == additionalOptionsColumn.Index)
			{
				int multichartsIndex = -1;
				string subjectMultichart = String.Empty;
				ChartInfo subjectChart = new ChartInfo();
				ChartInfo originalChart = new ChartInfo();
				List<string> chartIds = new List<string>();

				if (chartListRadioButton.Checked)
				{
					subjectChart = MyMeasurementFile.Charts[e.RowIndex];
					originalChart = GetOriginalChartInfo(String.Empty, subjectChart);
					chartIds = MyMeasurementFile.Charts.Select<ChartInfo, string>(p => p.ChartId).ToList();
				}
				else if (multichartsRadioButton.Checked)
				{
					subjectMultichart = multichartsComboBox.SelectedItem.ToString();
					multichartsIndex = multichartsComboBox.SelectedIndex;
					subjectChart = MyMeasurementFile.Multicharts.Charts[subjectMultichart][e.RowIndex];
					originalChart = GetOriginalChartInfo(subjectMultichart, subjectChart);
					chartIds = MyMeasurementFile.Multicharts.Charts[subjectMultichart].Select<ChartInfo, string>(p => p.ChartId).ToList();
				}

				ChartDialog dialog = new ChartDialog(subjectChart.Clone(), originalChart, MyMeasurementFile, ChangedValueColor, chartIds);

				if (multichartsRadioButton.Checked)
					dialog.Text = String.Format("{0} - {1}", dialog.Text, subjectMultichart);

				if (dialog.ShowDialog() == DialogResult.OK && !subjectChart.Equals(dialog.Subject))
				{
					subjectChart = dialog.Subject;

					if (chartListRadioButton.Checked)
						MyMeasurementFile.Charts[e.RowIndex] = subjectChart;
					else if (multichartsRadioButton.Checked)
						MyMeasurementFile.Multicharts.Charts[subjectMultichart][e.RowIndex] = subjectChart;

					UpdateChartsGridRow(multichartsIndex, e.RowIndex, subjectChart);
					SetChartSettingPropagationWarnings();
				}
			}
		}

		private void addChartButton_Click(object sender, EventArgs e)
		{
			EndGridCellEditMode();

			if (chartListRadioButton.Checked || (multichartsRadioButton.Checked && multichartsComboBox.SelectedIndex != -1))
			{
				int multichartsIndex = -1;
				int chartIdIndex = 0;
				string chartId = String.Empty;
				string subjectMultichart = String.Empty;
				List<ChartInfo> chartList = new List<ChartInfo>();
				ChartInfo subjectChart = new ChartInfo();
				ChartInfo originalChart = new ChartInfo();

				if (chartListRadioButton.Checked)
					chartList = MyMeasurementFile.Charts;
				else if (multichartsRadioButton.Checked)
				{
					subjectMultichart = multichartsComboBox.SelectedItem.ToString();
					multichartsIndex = multichartsComboBox.SelectedIndex;
					chartList = MyMeasurementFile.Multicharts.Charts[subjectMultichart];
				}

				do
				{
					chartId = String.Format("chart{0}", ++chartIdIndex);
				} while (chartList.FindIndex(p => p.ChartId == chartId) != -1);

				subjectChart.ChartId = chartId;
				chartList.Add(subjectChart);
				originalChart = GetOriginalChartInfo(subjectMultichart, subjectChart);
				chartsDataGridView.Rows.Add();
				UpdateChartsGridRow(multichartsIndex, chartList.Count - 1, subjectChart);
				chartsDataGridView_CellClick(sender, new DataGridViewCellEventArgs(additionalOptionsColumn.Index, chartList.Count - 1));
			}
		}

		private void deleteChartButton_Click(object sender, EventArgs e)
		{
			EndGridCellEditMode();

			if (chartsDataGridView.SelectedCells.Count != 0 && chartsDataGridView.Rows.Count != 0)
			{
				int selectedIndex = chartsDataGridView.CurrentCell.RowIndex;
				int multichartsIndex = -1;
				string subjectMultichart = String.Empty;
				List<ChartInfo> chartList = new List<ChartInfo>();

				if (chartListRadioButton.Checked)
				{
					chartList = MyMeasurementFile.Charts;
				}
				else if (multichartsRadioButton.Checked)
				{
					subjectMultichart = multichartsComboBox.SelectedItem.ToString();
					multichartsIndex = multichartsComboBox.SelectedIndex;
					chartList = MyMeasurementFile.Multicharts.Charts[subjectMultichart];
				}

				chartList.RemoveAt(selectedIndex);
				chartsDataGridView.Rows.RemoveAt(selectedIndex);
				SetChartSettingPropagationWarnings();
			}
		}

		private void moveUpChartButton_Click(object sender, EventArgs e)
		{
			EndGridCellEditMode();

			int selectedIndex = chartsDataGridView.CurrentCell.RowIndex;

			if (selectedIndex > 0)
			{
				int multichartsIndex = -1;
				string subjectMultichart = String.Empty;
				List<ChartInfo> chartList = new List<ChartInfo>();
				List<ChartInfo> newList = new List<ChartInfo>();

				if (chartListRadioButton.Checked)
				{
					chartList = MyMeasurementFile.Charts;
				}
				else if (multichartsRadioButton.Checked)
				{
					subjectMultichart = multichartsComboBox.SelectedItem.ToString();
					multichartsIndex = multichartsComboBox.SelectedIndex;
					chartList = MyMeasurementFile.Multicharts.Charts[subjectMultichart];
				}

				for (int i = 0; i < chartList.Count; i++)
				{
					if (i == selectedIndex - 1)
						newList.Add(chartList[selectedIndex]);

					if (i != selectedIndex)
						newList.Add(chartList[i]);
				}

				chartList = newList;

				UpdateChartsGridRow(multichartsIndex, selectedIndex - 1, chartList[selectedIndex - 1]);
				UpdateChartsGridRow(multichartsIndex, selectedIndex, chartList[selectedIndex]);
			}
		}

		private void moveDownChartButton_Click(object sender, EventArgs e)
		{
			EndGridCellEditMode();
			int selectedIndex = chartsDataGridView.CurrentCell.RowIndex;

			if (selectedIndex < chartsDataGridView.Rows.Count - 1 && selectedIndex != -1)
			{
				int multichartsIndex = -1;
				string subjectMultichart = String.Empty;
				List<ChartInfo> chartList = new List<ChartInfo>();
				List<ChartInfo> newList = new List<ChartInfo>();

				if (chartListRadioButton.Checked)
				{
					chartList = MyMeasurementFile.Charts;
				}
				else if (multichartsRadioButton.Checked)
				{
					subjectMultichart = multichartsComboBox.SelectedItem.ToString();
					multichartsIndex = multichartsComboBox.SelectedIndex;
					chartList = MyMeasurementFile.Multicharts.Charts[subjectMultichart];
				}

				for (int i = 0; i < chartList.Count; i++)
				{
					if (i != selectedIndex)
						newList.Add(chartList[i]);

					if (i == selectedIndex + 1)
						newList.Add(chartList[selectedIndex]);
				}

				chartList = newList;

				UpdateChartsGridRow(multichartsIndex, selectedIndex, chartList[selectedIndex]);
				UpdateChartsGridRow(multichartsIndex, selectedIndex + 1, chartList[selectedIndex + 1]);
			}
		}

		private void measureDataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			int columnIndex = measureDataGridView.CurrentCell.ColumnIndex;

			if (columnIndex == controlParameterColumn.Index || columnIndex == controlTypeColumn.Index || columnIndex == isDateColumn.Index ||
				columnIndex == isRequiredColumn.Index || columnIndex == columnNameColumn.Index || columnIndex == headerTypeColumn.Index ||
				columnIndex == isReturnRowColumn.Index || columnIndex == isReturnRowDateColumn.Index || columnIndex == isReturnRowControlColumn.Index ||
				columnIndex == isValueFieldColumn.Index || columnIndex == isRemoveFieldColumn.Index)
				measureDataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}

		private void measureDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex != -1 ? measureDataGridView.CurrentCell.IsInEditMode && measureDataGridView.CurrentCell.ColumnIndex == e.ColumnIndex : false)
			{
				bool changeOccurred = false;
				MeasureGridRow subject = MyMeasurementFile.MeasureGridRows[e.RowIndex];

				if (e.ColumnIndex == controlParameterColumn.Index)
				{
					string selection = measureDataGridView[e.ColumnIndex, e.RowIndex].Value.ToString();
					bool paramAlreadyUsed = MyMeasurementFile.MeasureGridRows.FindIndex(p => p.FilterParameter == selection) != -1 && !String.IsNullOrEmpty(selection);

					if (subject.FilterParameter != selection && !paramAlreadyUsed)
					{
						subject.FilterParameter = selection;

						if (subject.ControlType == ControlType.Neither)
							subject.ControlType = ControlType.Parameter;

						changeOccurred = true;
					}
					else if (subject.FilterParameter != selection && paramAlreadyUsed)
					{
						measureDataGridView.CancelEdit();
						measureDataGridView[e.ColumnIndex, e.RowIndex].Value = subject.FilterParameter;
					}
				}
				else if (e.ColumnIndex == controlTypeColumn.Index)
				{
					ControlType selection = Enums.GetControlTypeEnum(measureDataGridView[e.ColumnIndex, e.RowIndex].Value.ToString());

					if (subject.ControlType != selection)
					{
						subject.ControlType = selection;
						changeOccurred = true;
					}
				}
				else if (e.ColumnIndex == isDateColumn.Index)
				{
					bool selection = (bool)measureDataGridView[e.ColumnIndex, e.RowIndex].Value;

					if (subject.IsDate != selection)
					{
						subject.IsDate = selection;
						changeOccurred = true;
					}
				}
				else if (e.ColumnIndex == isRequiredColumn.Index)
				{
					bool selection = (bool)measureDataGridView[e.ColumnIndex, e.RowIndex].Value;

					if (subject.IsRequired != selection)
					{
						subject.IsRequired = selection;
						changeOccurred = true;
					}
				}
				else if (e.ColumnIndex == columnNameColumn.Index)
				{
					string selection = measureDataGridView[e.ColumnIndex, e.RowIndex].Value.ToString();
					bool columnAlreadyUsed = MyMeasurementFile.MeasureGridRows.FindIndex(p => p.ColumnName == selection) != -1 && !String.IsNullOrEmpty(selection);

					if (subject.ColumnName != selection && !columnAlreadyUsed)
					{
						subject.ColumnName = selection;
						subject.TableOrdinal = MyMeasurementFile.TableColumns.FindIndex(p => p == selection);

						if (subject.HeaderType == HeaderType.DateColumn)
						{
							subject.DateColumn = selection;
							MyMeasurementFile.ResetTableColumnOrdinals();
						}

						changeOccurred = true;
					}
					else if (subject.ColumnName != selection && columnAlreadyUsed)
					{
						measureDataGridView.CancelEdit();
						measureDataGridView[e.ColumnIndex, e.RowIndex].Value = subject.ColumnName;
					}
				}
				else if (e.ColumnIndex == headerTypeColumn.Index)
				{
					HeaderType selection = Enums.GetHeaderTypeEnum(measureDataGridView[e.ColumnIndex, e.RowIndex].Value.ToString());
					int dateColumnIndex = MyMeasurementFile.MeasureGridRows.FindIndex(p => p.HeaderType == HeaderType.DateColumn);
					bool missingRequiredDateColumn = MyMeasurementFile.Transform.RequiresDateRow && !MyMeasurementFile.Transform.HasDateRow;

					if (subject.HeaderType != selection && selection != HeaderType.DateColumn)
					{
						subject.HeaderType = selection;
						changeOccurred = true;
					}
					else if (subject.HeaderType != selection && selection == HeaderType.DateColumn && missingRequiredDateColumn)
					{
						MyMeasurementFile.Transform.DateField = subject.ColumnName;
						subject.HeaderType = selection;
						MyMeasurementFile.ResetTableColumnOrdinals();
						columnNameColumn.DataSource = MyMeasurementFile.TableColumns;
						changeOccurred = true;
					}
					else if (subject.HeaderType != selection && selection == HeaderType.DateColumn && !missingRequiredDateColumn)
					{
						measureDataGridView.CancelEdit();
						measureDataGridView[e.ColumnIndex, e.RowIndex].Value = Enums.GetFormattedString(subject.HeaderType);
					}

					SetDateColumnWarning();
				}
				else if (e.ColumnIndex == isReturnRowColumn.Index)
				{
					bool selection = (bool)measureDataGridView[e.ColumnIndex, e.RowIndex].Value;

					if (subject.IsReturnRow != selection)
					{
						subject.IsReturnRow = selection;
						changeOccurred = true;
					}
				}
				else if (e.ColumnIndex == isReturnRowDateColumn.Index)
				{
					bool selection = (bool)measureDataGridView[e.ColumnIndex, e.RowIndex].Value;

					if (subject.IsReturnRowDate != selection)
					{
						subject.IsReturnRowDate = selection;
						changeOccurred = true;
					}
				}
				else if (e.ColumnIndex == isReturnRowControlColumn.Index)
				{
					bool selection = (bool)measureDataGridView[e.ColumnIndex, e.RowIndex].Value;

					if (subject.IsReturnRowControl != selection)
					{
						subject.IsReturnRowControl = selection;
						changeOccurred = true;
					}
				}
				else if (e.ColumnIndex == isValueFieldColumn.Index)
				{
					bool selection = (bool)measureDataGridView[e.ColumnIndex, e.RowIndex].Value;

					if (subject.IsValueField != selection)
					{
						subject.IsValueField = selection;
						changeOccurred = true;
					}
				}
				else if (e.ColumnIndex == isRemoveFieldColumn.Index)
				{
					bool selection = (bool)measureDataGridView[e.ColumnIndex, e.RowIndex].Value;

					if (subject.IsRemoveField != selection)
					{
						subject.IsRemoveField = selection;
						MyMeasurementFile.Transform.RemoveFields.Add(subject.ColumnName);
						MyMeasurementFile.ResetTableColumnOrdinals();
						changeOccurred = true;
					}
				}

				if (changeOccurred)
				{
					MyMeasurementFile.UpdatePropertiesFromMeasureGridRows();
					RefreshMeasureGridRow(e.RowIndex, subject);
				}

				measureDataGridView.EndEdit();
			}
		}

		private void addMeasureGridRowButton_Click(object sender, EventArgs e)
		{
			EndGridCellEditMode();
			MeasureGridRow newRow = new MeasureGridRow(MyMeasurementFile.Transform.DateField);

			if (MyMeasurementFile.Parameters.Count >= MyMeasurementFile.DatabaseParameterCount)
				newRow.ControlType = ControlType.Control;

			MyMeasurementFile.MeasureGridRows.Add(newRow);
			int row = measureDataGridView.Rows.Count;
			measureDataGridView.Rows.Add();
			RefreshMeasureGridRow(row, newRow);
			measureDataGridView.CurrentCell = measureDataGridView[controlParameterColumn.Index, row];
		}

		private void deleteMeasureGridRowButton_Click(object sender, EventArgs e)
		{
			EndGridCellEditMode();

			if (measureDataGridView.RowCount != 0)
			{
				int row = measureDataGridView.CurrentCell.RowIndex;
				bool resetColumnOrdinals = MyMeasurementFile.MeasureGridRows[row].HeaderType == HeaderType.DateColumn ||
					MyMeasurementFile.MeasureGridRows[row].IsRemoveField;
				List<MeasureGridRow> newList = new List<MeasureGridRow>();

				if (MyMeasurementFile.MeasureGridRows[row].HeaderType == HeaderType.DateColumn)
					MyMeasurementFile.Transform.DateField = String.Empty;

				for (int i = 0; i < MyMeasurementFile.MeasureGridRows.Count; i++)
					if (i != row)
						newList.Add(MyMeasurementFile.MeasureGridRows[i]);

				MyMeasurementFile.MeasureGridRows = newList;

				if (resetColumnOrdinals)
					MyMeasurementFile.ResetTableColumnOrdinals();

				RefreshMeasureGrid();

				if (row - 1 >= 0)
					measureDataGridView.CurrentCell = measureDataGridView[0, row - 1];
			}

			SetDateColumnWarning();
		}

		private void moveUpMeasureGridRowButton_Click(object sender, EventArgs e)
		{
			EndGridCellEditMode();
			int row = measureDataGridView.CurrentCell.RowIndex;

			if (row > 0)
			{
				int destination = row - 1;
				List<MeasureGridRow> newList = new List<MeasureGridRow>();

				for (int i = 0; i < MyMeasurementFile.MeasureGridRows.Count; i++)
				{
					if (i == destination)
						newList.Add(MyMeasurementFile.MeasureGridRows[row]);

					if (i != row)
						newList.Add(MyMeasurementFile.MeasureGridRows[i]);
				}

				MyMeasurementFile.MeasureGridRows = newList;
				RefreshMeasureGrid();
				measureDataGridView.CurrentCell = measureDataGridView[0, destination];
			}
		}

		private void moveDownMeasureGridRowButton_Click(object sender, EventArgs e)
		{
			EndGridCellEditMode();
			int row = measureDataGridView.CurrentCell.RowIndex;

			if (row < measureDataGridView.RowCount - 1 && row != -1)
			{
				int destination = row + 1;
				List<MeasureGridRow> newList = new List<MeasureGridRow>();

				for (int i = 0; i < MyMeasurementFile.MeasureGridRows.Count; i++)
				{
					if (i != row)
						newList.Add(MyMeasurementFile.MeasureGridRows[i]);

					if (i == destination)
						newList.Add(MyMeasurementFile.MeasureGridRows[row]);
				}

				MyMeasurementFile.MeasureGridRows = newList;
				RefreshMeasureGrid();
				measureDataGridView.CurrentCell = measureDataGridView[0, destination];
			}
		}
	}
}
