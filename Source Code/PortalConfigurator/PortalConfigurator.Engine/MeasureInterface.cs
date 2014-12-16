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
		private List<string> FilterParameterList { get; set; }
		private List<string> HeaderParameterList { get; set; }
		private int FilterParameterComboBoxEvents { get; set; }
		private int HeaderComboBoxEvents { get; set; }

		public void InitializeMeasureInterface()
		{
			MyMeasurementFile = new MeasurementFile();
			OriginalMeasurementFile = new MeasurementFile();
			FilterParameterFileName = String.Empty;
			FilterParameterList = new List<string>();
			HeaderParameterList = new List<string>();
			FilterParameterComboBoxEvents = 0;
			HeaderComboBoxEvents = 0;
		}

		public void LoadMeasureInterface(object sender, EventArgs e)
		{
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

			hideRowComboBox.Items.AddRange(Enum.GetNames(typeof(HideRow)));
			hideRowComboBox.Items.RemoveAt(0);
			hideRowComboBox.Items.Insert(0, String.Empty);
			chartTypeComboBox.Items.AddRange(Enum.GetNames(typeof(ChartType)));
			chartTypeComboBox.Items.RemoveAt(0);
			chartTypeComboBox.Items.Insert(0, String.Empty);
			controlTypeColumn.DataSource = Enum.GetNames(typeof(ControlType));
			functionComboBox.Items.AddRange(Enum.GetNames(typeof(TransformFunction)));
			functionComboBox.Items.RemoveAt(0);
			functionComboBox.Items.Insert(0, String.Empty);
			yAxisFormatComboBox.Items.AddRange(Enum.GetNames(typeof(AxisFormat)));
			yAxisFormatComboBox.Items.RemoveAt(0);
			yAxisFormatComboBox.Items.Insert(0, String.Empty);

			List<string> headerTypes = new List<string>();
			
			foreach (HeaderType item in Enum.GetValues(typeof(HeaderType)))
				headerTypes.Add(Enums.GetString(item));

			headerTypeColumn.DataSource = headerTypes;
		}

		private void newMeasurementFileToolStripButton_Click(object sender, EventArgs e)
		{
			bool hasUnsavedChanges = !MyMeasurementFile.Equals(OriginalMeasurementFile);
			DialogResult proceed = DialogResult.Yes;

			if (hasUnsavedChanges)
			{
				string message = "You will lose unsaved changes if you continue.\nDo you wish to continue?";
				proceed = MessageBox.Show(message, "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
			}

			if (!hasUnsavedChanges || proceed == DialogResult.Yes)
			{
				MyMeasurementFile = new MeasurementFile();
				OriginalMeasurementFile = MyMeasurementFile.Clone();

				transformFieldsDataGridView.Rows.Clear();
				filterParametersDataGridView.Rows.Clear();
				headersDataGridView.Rows.Clear();
				numberFormatsDataGridView.Rows.Clear();
				chartsDataGridView.Rows.Clear();
				multichartsComboBox.Items.Clear();
				measureTableButton.Text = "No Stored Procedure";
				measureTableButton.BackColor = default(Color);
				titleTextBox.Text = MyMeasurementFile.Title;
				titleTextBox.BackColor = default(Color);
				baseButton.Text = "No Base Measure File";
				baseButton.BackColor = default(Color);
				orderNumericUpDown.Value = MyMeasurementFile.Order ?? 0;
				orderNumericUpDown.BackColor = default(Color);
				hideRowComboBox.SelectedIndex = -1;
				hideRowComboBox.BackColor = default(Color);
				fullTitleTextBox.Text = MyMeasurementFile.FullTitle;
				fullTitleTextBox.BackColor = default(Color);
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
				functionComboBox.SelectedIndex = (int)MyMeasurementFile.Transform.Function;
				functionComboBox.BackColor = default(Color);
				dateFieldComboBox.Items.Clear();
				dateFieldComboBox.BackColor = default(Color);
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
		}

		private void openMeasureFileToolStripButton_Click(object sender, EventArgs e)
		{
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
#if DEBUG
				openFileDialog.InitialDirectory = Environment.CurrentDirectory.Replace(@"\bin\Debug", "");
#endif
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					MyMeasurementFile = new MeasurementFile(openFileDialog.FileName);

					try
					{
						MyMeasurementFile.ParseJson();
						Dictionary<int, string> tableColumns = MyMeasurementFile.TableColumns;
					}
					catch (JsonParseException parseException)
					{
						MyMeasurementFile = new MeasurementFile();
						string message = String.Format("The following problem occurred while parsing the file:\n{0}", parseException.Message);
						MessageBox.Show(message, "Parse Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					catch(DatabaseException databaseException)
					{
						MyMeasurementFile = new MeasurementFile();
						string message = String.Format("The following problem occurred while synching with the database:\n{0}", databaseException.Message);
						MessageBox.Show(message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					catch (Exception exception)
					{
						MyMeasurementFile = new MeasurementFile();
						string message = String.Format("The following problem occurred while opening the file:\n{0}", exception.Message);
						MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}

					if (String.IsNullOrEmpty(FilterParameterFileName))
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
					else
						RefreshFilterParametersGrid();

					OriginalMeasurementFile = MyMeasurementFile.Clone();

					transformFieldsDataGridView.Rows.Clear();
					filterParametersDataGridView.Rows.Clear();
					headersDataGridView.Rows.Clear();
					numberFormatsDataGridView.Rows.Clear();
					chartsDataGridView.Rows.Clear();
					multichartsComboBox.SelectedIndex = -1;
					multichartsComboBox.Items.Clear();

					measureTableButton.Text = MyMeasurementFile.Table;
					titleTextBox.Text = MyMeasurementFile.Title;
					baseButton.Text = String.IsNullOrEmpty(MyMeasurementFile.BaseMeasure) ? "No Base Measure File" : MyMeasurementFile.BaseMeasure;
					orderNumericUpDown.Value = MyMeasurementFile.Order ?? 0;
					hideRowComboBox.SelectedIndex = (int)MyMeasurementFile.HideRow;
					fullTitleTextBox.Text = MyMeasurementFile.FullTitle;
					filterTextBox.Text = MyMeasurementFile.Filter;
					maxCheckedNumericUpDown.Value = MyMeasurementFile.MaxChecked ?? 0;
					chartTypeComboBox.SelectedIndex = (int)MyMeasurementFile.ChartType;
					showAllOthersCheckBox.CheckState = ToCheckState(MyMeasurementFile.ShowAllOthers);
					muteAllOthersCheckBox.CheckState = ToCheckState(MyMeasurementFile.MutexAllOthers);
					summaryTextBox.Text = MyMeasurementFile.Summary;
					functionComboBox.SelectedIndex = (int)MyMeasurementFile.Transform.Function;

					RefreshFilterParametersGrid();
					RefreshDateFieldComboBox();
					RefreshTransformGrid();

					xAxisLabelTextBox.Text = MyMeasurementFile.Label.XAxisLabel;
					yAxisLabelTextBox.Text = MyMeasurementFile.Label.YAxisLabel;
					yAxisMinNumericUpDown.Value = (decimal)(MyMeasurementFile.Label.YAxisMin ?? 0.0f);
					yAxisMaxNumericUpDown.Value = (decimal)(MyMeasurementFile.Label.YAxisMax ?? 0.0f);
					yAxisFormatComboBox.SelectedIndex = (int)MyMeasurementFile.Label.YAxisFormat;

					RefreshHeaderParameterList();
					RefreshNumberFormatsGrid();

					if (MyMeasurementFile.Charts.Count != 0 && MyMeasurementFile.Multicharts.IsEmpty)
						chartListRadioButton.Checked = true;
					else if (MyMeasurementFile.Charts.Count == 0 && !MyMeasurementFile.Multicharts.IsEmpty)
						multichartsRadioButton.Checked = true;
					else
					{
						chartListRadioButton.Checked = false;
						multichartsRadioButton.Checked = false;
						multichartsComboBox.Enabled = false;
						multichartsComboBox.Items.Clear();
						multichartsComboBox.BackColor = default(Color);
						addMultichartsButton.Enabled = false;
						deleteMultichartsButton.Enabled = false;
						moveUpMultichartsButton.Enabled = false;
						moveDownMultichartsButton.Enabled = false;
						changeMultichartNameButton.Enabled = false;
					}
				}
			}
		}

		private void saveMeasureFileToolStripButton_Click(object sender, EventArgs e)
		{
			saveFileDialog.Title = "Save Measurement File";

			if (!String.IsNullOrEmpty(MyMeasurementFile.FilePath))
			{
				FileInfo filePath = new FileInfo(MyMeasurementFile.FilePath);
				saveFileDialog.InitialDirectory = filePath.DirectoryName;
				saveFileDialog.FileName = filePath.Name;
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

					measureTableButton.BackColor = default(Color);
					titleTextBox.BackColor = default(Color);
					baseButton.BackColor = default(Color);
					orderNumericUpDown.BackColor = default(Color);
					hideRowComboBox.BackColor = default(Color);
					fullTitleTextBox.BackColor = default(Color);
					filterTextBox.BackColor = default(Color);
					maxCheckedNumericUpDown.BackColor = default(Color);
					chartTypeComboBox.BackColor = default(Color);
					showAllOthersCheckBox.BackColor = default(Color);
					muteAllOthersCheckBox.BackColor = default(Color);
					summaryTextBox.BackColor = default(Color);

					for (int c = 0; c < filterParametersDataGridView.Columns.Count; c++)
						for (int r = 0; r < filterParametersDataGridView.Rows.Count; r++)
							filterParametersDataGridView[c, r].Style.BackColor = default(Color);

					functionComboBox.BackColor = default(Color);
					dateFieldComboBox.BackColor = default(Color);

					for (int c = 1; c < transformFieldsDataGridView.Columns.Count; c++)
						for (int r = 0; r < transformFieldsDataGridView.Rows.Count; r++)
							transformFieldsDataGridView[c, r].Style.BackColor = default(Color);

					xAxisLabelTextBox.BackColor = default(Color);
					yAxisLabelTextBox.BackColor = default(Color);
					yAxisMinNumericUpDown.BackColor = default(Color);
					yAxisMaxNumericUpDown.BackColor = default(Color);
					yAxisFormatComboBox.BackColor = default(Color);

					for (int c = 0; c < numberFormatsDataGridView.Columns.Count - 1; c++)
						for (int r = 0; r < numberFormatsDataGridView.Rows.Count; r++)
							numberFormatsDataGridView[c, r].Style.BackColor = default(Color);

					for (int c = 1; c < headersDataGridView.Columns.Count; c++)
						for (int r = 0; r < headersDataGridView.Rows.Count; r++)
							headersDataGridView[c, r].Style.BackColor = default(Color);

					chartListRadioButton.BackColor = default(Color);
					multichartsRadioButton.BackColor = default(Color);
					multichartsComboBox.BackColor = default(Color);

					for (int c = 0; c < chartsDataGridView.Columns.Count - 1; c++)
						for (int r = 0; r < chartsDataGridView.Rows.Count; r++)
							chartsDataGridView[c, r].Style.BackColor = default(Color);
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
			openFileDialog.FileName = String.Empty;

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
					filterParameterNameColumn.DataSource = FilterParameterList;
					List<RemovedParameter> removalEntries = new List<RemovedParameter>();

					for (int i = 0; i < MyMeasurementFile.Parameters.Count; i++)
					{
						string item = MyMeasurementFile.Parameters.ElementAt(i);

						if (!FilterParameterList.Contains(item))
						{
							removalEntries.Add(new RemovedParameter(item, ControlType.Parameter));
							MyMeasurementFile.Parameters.Remove(item);
							i--;
						}
					}

					for (int i = 0; i < MyMeasurementFile.Controls.Count; i++)
					{
						string item = MyMeasurementFile.Controls.ElementAt(i);

						if (!FilterParameterList.Contains(item))
						{
							int removedIndex = removalEntries.FindIndex(p => p.Name == item);

							if (removedIndex == -1)
								removalEntries.Add(new RemovedParameter(item, ControlType.Filter));
							else
								removalEntries.ElementAt(i).ControlType = ControlType.Both;

							MyMeasurementFile.Controls.Remove(item);
							i--;
						}
					}

					for (int i = 0; i < MyMeasurementFile.DateParameters.Count; i++)
					{
						string item = MyMeasurementFile.DateParameters.ElementAt(i);

						if (!FilterParameterList.Contains(item))
						{
							int removedIndex = removalEntries.FindIndex(p => p.Name == item);

							if (removedIndex == -1)
								removalEntries.Add(new RemovedParameter(item, ControlType.Parameter, true));
							else
								removalEntries.ElementAt(removedIndex).IsDate = true;

							MyMeasurementFile.DateParameters.Remove(item);
							i--;
						}
					}

					for (int i = 0; i < MyMeasurementFile.RequiredParameters.Count; i++)
					{
						string item = MyMeasurementFile.RequiredParameters.ElementAt(i);

						if (!FilterParameterList.Contains(item))
						{
							int removalIndex = removalEntries.FindIndex(p => p.Name == item);

							if (removalIndex == -1)
								removalEntries.Add(new RemovedParameter(item, ControlType.Parameter, false, true));
							else
								removalEntries.ElementAt(removalIndex).IsRequired = true;

							MyMeasurementFile.RequiredParameters.Remove(item);
							i--;
						}
					}

					foreach (var item in MyMeasurementFile.HeaderGridRows)
					{
						if (String.IsNullOrEmpty(item.Value.FilterParameter) ? false : !FilterParameterList.Contains(item.Value.FilterParameter))
						{
							int removalIndex = removalEntries.FindIndex(p => p.Name == item.Value.FilterParameter);

							if (removalIndex == -1)
								removalEntries.Add(new RemovedParameter(item.Value.FilterParameter, ControlType.Parameter, false, false, item.Value.ColumnName));
							else
								removalEntries.ElementAt(removalIndex).HeaderName = item.Value.ColumnName;

							item.Value.FilterParameter = String.Empty;
						}
					}

					if (removalEntries.Count != 0)
					{
						string message = "The following parameters were not present in the parameter file and were removed from the measure:\n\n";
						message = String.Concat(message, "Name\tType\tDate\tRequired\tHeader\n");

						foreach (var item in removalEntries)
						{
							string line = String.Format("{0}\t{1}\t{2}\t{3}\t{4}\n", item.Name, item.ControlType, item.IsDate, item.IsRequired, item.HeaderName);
							message = String.Concat(message, line);
						}

						message = String.Concat(message, "\nSave the measure file to make these changes permanent.");
						MessageBox.Show(message, "Measure Settings Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				}
				catch (JsonParseException error)
				{
					FilterParameterFileName = fileName;
					FilterParameterList = filterParameterArray.ToList();
					MessageBox.Show(error.Message, "Json File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void RefreshFilterParametersGrid()
		{
			string subjectKey;
			int originalIndex;
			string originalKey;
			FilterParameterGridRow subjectValue;
			FilterParameterGridRow originalValue;
			filterParametersDataGridView.Rows.Clear();

			if (MyMeasurementFile.FilterParameterGridRows.Count != 0)
			{
				filterParametersDataGridView.Rows.Add(MyMeasurementFile.FilterParameterGridRows.Count);
				DataGridViewCell filterParameterNameCell;
				DataGridViewCell controlTypeCell;
				DataGridViewCell isDateCell;
				DataGridViewCell isRequiredCell;

				for (int i = 0; i < MyMeasurementFile.FilterParameterGridRows.Count; i++)
				{
					subjectKey = MyMeasurementFile.FilterParameterGridRows.ElementAt(i).Key;
					subjectValue = MyMeasurementFile.FilterParameterGridRows.ElementAt(i).Value;
					originalIndex = OriginalMeasurementFile.FilterParameterGridRows.Keys.ToList<string>().FindIndex(p => p == subjectKey);
					originalKey = originalIndex == -1 ? String.Empty : OriginalMeasurementFile.FilterParameterGridRows.ElementAt(originalIndex).Key;
					originalValue = originalIndex == -1 ? new FilterParameterGridRow() : OriginalMeasurementFile.FilterParameterGridRows.ElementAt(originalIndex).Value;
					filterParameterNameCell = filterParametersDataGridView[filterParameterNameColumn.Index, i];
					controlTypeCell = filterParametersDataGridView[controlTypeColumn.Index, i];
					isDateCell = filterParametersDataGridView[isDateColumn.Index, i];
					isRequiredCell = filterParametersDataGridView[isRequiredColumn.Index, i];

					filterParameterNameCell.Value = subjectKey;
					filterParameterNameCell.Style.BackColor = subjectKey == originalKey ? default(Color) : ChangedValueColor;
					controlTypeCell.Value = Enum.GetName(typeof(ControlType), subjectValue.ControlType);
					controlTypeCell.Style.BackColor = subjectValue.ControlType == originalValue.ControlType ? default(Color) : ChangedValueColor;
					isDateCell.Value = subjectValue.IsDate;
					isDateCell.Style.BackColor = subjectValue.IsDate == originalValue.IsDate ? default(Color) : ChangedValueColor;
					isRequiredCell.Value = subjectValue.IsRequired;
					isRequiredCell.Style.BackColor = subjectValue.IsRequired == originalValue.IsRequired ? default(Color) : ChangedValueColor;
				}
			}
		}

		private void RefreshDateFieldComboBox()
		{
			dateFieldComboBox.Items.Clear();
			dateFieldComboBox.Items.Add(String.Empty);
			dateFieldComboBox.Items.AddRange(MyMeasurementFile.Transform.TableColumns.Values.ToArray<string>());
			string dateField = MyMeasurementFile.Transform.DateField;

			if (MyMeasurementFile.Transform.TableColumns.ContainsValue(dateField))
				dateFieldComboBox.SelectedIndex = MyMeasurementFile.Transform.TableColumns.First(p => p.Value == dateField).Key + 1;
			else
				dateFieldComboBox.SelectedIndex = -1;

			dateFieldComboBox.BackColor = MyMeasurementFile.Transform.DateField == OriginalMeasurementFile.Transform.DateField ? default(Color) : ChangedValueColor;
		}

		private void RefreshTransformGrid()
		{
			transformFieldsDataGridView.Rows.Clear();

			if (MyMeasurementFile.Transform.TransformFieldGridRows.Count != 0)
			{
				transformFieldsDataGridView.Rows.Add(MyMeasurementFile.Transform.TransformFieldGridRows.Count);
				DataGridViewCell fieldNameCell;
				DataGridViewCell isValueFieldCell;
				DataGridViewCell removeFieldCell;

				foreach (var item in MyMeasurementFile.Transform.TransformFieldGridRows)
				{
					bool isOriginalValueField = OriginalMeasurementFile.Transform.ValueFields.Contains(item.Value.FieldName);
					bool isOriginalRemoveField = OriginalMeasurementFile.Transform.RemoveFields.Contains(item.Value.FieldName);
					fieldNameCell = transformFieldsDataGridView[fieldNameColumn.Index, item.Key];
					isValueFieldCell = transformFieldsDataGridView[isValueFieldColumn.Index, item.Key];
					removeFieldCell = transformFieldsDataGridView[removeFieldColumn.Index, item.Key];

					fieldNameCell.Value = item.Value.FieldName;
					isValueFieldCell.Value = item.Value.IsValueField;
					isValueFieldCell.Style.BackColor = item.Value.IsValueField == isOriginalValueField ? default(Color) : ChangedValueColor;
					removeFieldCell.Value = item.Value.IsRemovedField;
					removeFieldCell.Style.BackColor = item.Value.IsRemovedField == isOriginalRemoveField ? default(Color) : ChangedValueColor;
				}
			}
		}

		private void RefreshHeaderParameterList()
		{
			HeaderParameterList.Clear();

			foreach (var item in MyMeasurementFile.FilterParameterGridRows)
				HeaderParameterList.Add(item.Key);

			foreach (var item in MyMeasurementFile.HeaderGridRows)
				if (!HeaderParameterList.Contains(item.Value.FilterParameter))
					item.Value.FilterParameter = String.Empty;

			headerFilterParameterColumn.DataSource = HeaderParameterList;
			RefreshHeadersGrid();
		}

		private void RefreshHeadersGrid()
		{
			headersDataGridView.Rows.Clear();

			if (MyMeasurementFile.HeaderGridRows.Count != 0)
			{
				headersDataGridView.Rows.Add(MyMeasurementFile.HeaderGridRows.Count);

				for (int i = 0; i < MyMeasurementFile.HeaderGridRows.Count; i++)
					RefreshHeadersGridRow(i);
			}
		}

		private void RefreshHeadersGridRow(int row)
		{
			int headerKey = MyMeasurementFile.HeaderGridRows.ElementAt(row).Key;
			HeaderGridRow header = MyMeasurementFile.HeaderGridRows.ElementAt(row).Value;
			string name = header.TableOrdinal == -1 ? header.HeaderText : header.ColumnName;
			int originalIndex = OriginalMeasurementFile.HeaderGridRows.Values.ToList<HeaderGridRow>().FindIndex(p => p.ColumnName == header.ColumnName);
			HeaderGridRow original = originalIndex == -1 ? new HeaderGridRow() : OriginalMeasurementFile.HeaderGridRows.ElementAt(originalIndex).Value;
			string originalName = original.TableOrdinal == -1 ? original.HeaderText : original.ColumnName;
			DataGridViewCell headerKeyCell = headersDataGridView[headerKeyColumn.Index, row];
			DataGridViewCell headerColumnNameCell = headersDataGridView[columnNameColumn.Index, row];
			DataGridViewCell headerTypeCell = headersDataGridView[headerTypeColumn.Index, row];
			DataGridViewCell headerFilterParameterCell = headersDataGridView[headerFilterParameterColumn.Index, row];
			DataGridViewCell isReturnRowCell = headersDataGridView[isReturnRowColumn.Index, row];
			DataGridViewCell isReturnRowDateCell = headersDataGridView[isReturnRowDateColumn.Index, row];
			DataGridViewCell isReturnRowControlCell = headersDataGridView[isReturnRowControlColumn.Index, row];

			headerKeyCell.Value = headerKey;
			headerColumnNameCell.Value = name;
			headerColumnNameCell.Style.BackColor = name == originalName ? default(Color) : ChangedValueColor;
			headerTypeCell.Value = Enums.GetString(header.HeaderType);
			headerTypeCell.Style.BackColor = header.HeaderType == original.HeaderType ? default(Color) : ChangedValueColor;
			headerFilterParameterCell.Value = header.FilterParameter;
			headerFilterParameterCell.Style.BackColor = header.FilterParameter == original.FilterParameter ? default(Color) : ChangedValueColor;
			isReturnRowCell.Value = header.IsReturnRow;
			isReturnRowCell.Style.BackColor = header.IsReturnRow == original.IsReturnRow ? default(Color) : ChangedValueColor;
			isReturnRowDateCell.Value = header.IsReturnRowDate;
			isReturnRowDateCell.Style.BackColor = header.IsReturnRowDate == original.IsReturnRowDate ? default(Color) : ChangedValueColor;
			isReturnRowControlCell.Value = header.IsReturnRowControl;
			isReturnRowControlCell.Style.BackColor = header.IsReturnRowControl == original.IsReturnRowControl ? default(Color) : ChangedValueColor;
			headersDataGridView.Rows[row].ReadOnly = header.ColumnName == MyMeasurementFile.Transform.DateField;
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
					format = MyMeasurementFile.NumberFormats.ElementAt(i);
					original = OriginalMeasurementFile.NumberFormats.Count == 0 ? new NumberFormat() : OriginalMeasurementFile.NumberFormats.ElementAt(i);
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
			if (!String.IsNullOrEmpty(FilterParameterFileName))
			{
				string message = "By changing the Filter/Parameter file,\nsettings related to items not in the new file\nwill be lost.";
				MessageBox.Show(message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}

			LoadFilterParametersFile();
			filterParameterFileButton.Text = String.IsNullOrEmpty(FilterParameterFileName) ? "No Filter/Parameter File" : FilterParameterFileName;
			filterParameterFileButton.ForeColor = String.IsNullOrEmpty(FilterParameterFileName) ? UnknownValueColor : default(Color);

			if (!String.IsNullOrEmpty(MyMeasurementFile.Table))
			{
				RefreshFilterParametersGrid();
				RefreshHeaderParameterList();
			}
		}

		private void measureTableButton_Click(object sender, EventArgs e)
		{
			if (!String.IsNullOrEmpty(MyMeasurementFile.Table))
			{
				string message = "By changing the refrenced stored procedure,\ntransform and column header settings will be lost.";
				MessageBox.Show(message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			
			StoredProcedurePrompt prompt = new StoredProcedurePrompt();
			prompt.StoredProcedure = MyMeasurementFile.Table;
			prompt.ShowDialog();

			if (prompt.DialogResult == DialogResult.OK)
			{
				if (prompt.StoredProcedure != MyMeasurementFile.Table)
				{
					try
					{
						MyMeasurementFile.Table = prompt.StoredProcedure;
						MyMeasurementFile.TableDataAreLoaded = false;
						MyMeasurementFile.Transform.DateField = String.Empty;
						MyMeasurementFile.Transform.ValueFields.Clear();
						MyMeasurementFile.Transform.RemoveFields.Clear();
						MyMeasurementFile.HeaderNames.Clear();
						MyMeasurementFile.ReturnRowHeaders.Clear();
						MyMeasurementFile.ReturnRowDateHeaders.Clear();
						MyMeasurementFile.ReturnRowControlHeaders.Clear();
						Dictionary<int, string> tableColumns = MyMeasurementFile.TableColumns;

						RefreshDateFieldComboBox();
						RefreshTransformGrid();
						RefreshHeadersGrid();

						measureTableButton.Text = MyMeasurementFile.Table;
						measureTableButton.BackColor = MyMeasurementFile.Table == OriginalMeasurementFile.Table ? default(Color) : ChangedValueColor;
					}
					catch (Exception error)
					{
						MessageBox.Show(error.Message, "Process Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
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

		private void baseButton_Click(object sender, EventArgs e)
		{
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

		private void fullTitleTextBox_TextChanged(object sender, EventArgs e)
		{
			if (MyMeasurementFile.FullTitle != fullTitleTextBox.Text)
			{
				MyMeasurementFile.FullTitle = fullTitleTextBox.Text;
				fullTitleTextBox.BackColor = MyMeasurementFile.FullTitle == OriginalMeasurementFile.FullTitle ? default(Color) : ChangedValueColor;
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

		private void chartTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if ((int)MyMeasurementFile.ChartType != chartTypeComboBox.SelectedIndex)
			{
				MyMeasurementFile.ChartType = (ChartType)chartTypeComboBox.SelectedIndex;
				chartTypeComboBox.BackColor = MyMeasurementFile.ChartType == OriginalMeasurementFile.ChartType ? default(Color) : ChangedValueColor;
			}
		}

		private void showAllOthersCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			bool? value = ToNullableBool(showAllOthersCheckBox.CheckState);

			if (MyMeasurementFile.ShowAllOthers != value)
			{
				MyMeasurementFile.ShowAllOthers = value;
				showAllOthersCheckBox.BackColor = value == OriginalMeasurementFile.ShowAllOthers ? default(Color) : ChangedValueColor;
			}
		}

		private void muteAllOthersCheckBox_CheckedChanged(object sender, EventArgs e)
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

		private void filterParametersDataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			int columnIndex = filterParametersDataGridView.CurrentCell.ColumnIndex;

			if (columnIndex == filterParameterNameColumn.Index || columnIndex == controlTypeColumn.Index || columnIndex == isDateColumn.Index ||
				columnIndex == isRequiredColumn.Index)
				filterParametersDataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}

		private void filterParametersDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex != -1)
			{
				bool changeOccurred = false;
				string key = MyMeasurementFile.FilterParameterGridRows.ElementAt(e.RowIndex).Key;

				if (e.ColumnIndex == filterParameterNameColumn.Index)
				{
					string selection = filterParametersDataGridView[e.ColumnIndex, e.RowIndex].Value.ToString();

					if (key != selection)
					{
						if (!MyMeasurementFile.FilterParameterGridRows.ContainsKey(selection))
						{
							Dictionary<string, FilterParameterGridRow> newRows = new Dictionary<string, FilterParameterGridRow>();

							foreach (var item in MyMeasurementFile.FilterParameterGridRows)
								newRows.Add(item.Key == key ? selection : item.Key, item.Value);

							MyMeasurementFile.FilterParameterGridRows = newRows;
							key = selection;

							foreach (var item in MyMeasurementFile.HeaderGridRows)
								if (item.Value.FilterParameter == selection)
									item.Value.FilterParameter = String.Empty;

							RefreshHeaderParameterList();
							changeOccurred = true;
						}
						else
						{
							string message = String.Format("Parameter {0} is already in this file and cannot be added again.", selection);
							MessageBox.Show(message, "Dulplicate Situation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
							filterParametersDataGridView[e.ColumnIndex, e.RowIndex].Value = key;
						}
					}
				}
				else if (e.ColumnIndex == controlTypeColumn.Index)
				{
					ControlType selection = Enums.GetControlTypeEnum(filterParametersDataGridView[e.ColumnIndex, e.RowIndex].Value.ToString());

					if (MyMeasurementFile.FilterParameterGridRows[key].ControlType != selection)
					{
						MyMeasurementFile.FilterParameterGridRows[key].ControlType = selection;
						MyMeasurementFile.UpdatePropertiesFromFilterParameterGridRows();
						changeOccurred = true;
					}
				}
				else if (e.ColumnIndex == isDateColumn.Index)
				{
					bool selection = (bool)filterParametersDataGridView[e.ColumnIndex, e.RowIndex].Value;

					if (MyMeasurementFile.FilterParameterGridRows[key].IsDate != selection)
					{
						MyMeasurementFile.FilterParameterGridRows[key].IsDate = selection;
						MyMeasurementFile.UpdatePropertiesFromFilterParameterGridRows();
						changeOccurred = true;
					}
				}
				else if (e.ColumnIndex == isRequiredColumn.Index)
				{
					bool selection = (bool)filterParametersDataGridView[e.ColumnIndex, e.RowIndex].Value;

					if (MyMeasurementFile.FilterParameterGridRows[key].IsRequired != selection)
					{
						MyMeasurementFile.FilterParameterGridRows[key].IsRequired = selection;
						MyMeasurementFile.UpdatePropertiesFromFilterParameterGridRows();
						changeOccurred = true;
					}
				}

				if (changeOccurred)
				{
					FilterParameterGridRow subjectValue = MyMeasurementFile.FilterParameterGridRows[key];
					int originalIndex = OriginalMeasurementFile.FilterParameterGridRows.Keys.ToList<string>().FindIndex(p => p == key);
					string originalKey = originalIndex == -1 ? String.Empty : OriginalMeasurementFile.FilterParameterGridRows.ElementAt(originalIndex).Key;
					FilterParameterGridRow originalValue = originalIndex == -1 ? new FilterParameterGridRow() : OriginalMeasurementFile.FilterParameterGridRows[originalKey];
					DataGridViewCell filterParameterNameCell = filterParametersDataGridView[filterParameterNameColumn.Index, e.RowIndex]; ;
					DataGridViewCell controlTypeCell = filterParametersDataGridView[controlTypeColumn.Index, e.RowIndex];
					DataGridViewCell isDateCell = filterParametersDataGridView[isDateColumn.Index, e.RowIndex];
					DataGridViewCell isRequiredCell = filterParametersDataGridView[isRequiredColumn.Index, e.RowIndex];

					filterParameterNameCell.Style.BackColor = key == originalKey ? default(Color) : ChangedValueColor;
					controlTypeCell.Style.BackColor = subjectValue.ControlType == originalValue.ControlType ? default(Color) : ChangedValueColor;
					isDateCell.Style.BackColor = subjectValue.IsDate == originalValue.IsDate ? default(Color) : ChangedValueColor;
					isRequiredCell.Style.BackColor = subjectValue.IsRequired == originalValue.IsRequired ? default(Color) : ChangedValueColor;
				}
			}
		}

		private void addFilterParameterButton_Click(object sender, EventArgs e)
		{
			foreach (var item in FilterParameterList)
			{
				if (!MyMeasurementFile.FilterParameterGridRows.ContainsKey(item))
				{
					MyMeasurementFile.FilterParameterGridRows.Add(item, new FilterParameterGridRow());
					RefreshFilterParametersGrid();
					RefreshHeaderParameterList();
					filterParametersDataGridView.CurrentCell = filterParametersDataGridView[0, filterParametersDataGridView.Rows.Count - 1];
					return;
				}
			}
		}

		private void deleteFilterParameterButton_Click(object sender, EventArgs e)
		{
			if (filterParametersDataGridView.RowCount != 0)
			{
				int row = filterParametersDataGridView.CurrentCellAddress.Y;
				Dictionary<string, FilterParameterGridRow> newList = new Dictionary<string, FilterParameterGridRow>();

				for (int i = 0; i < MyMeasurementFile.FilterParameterGridRows.Count; i++)
					if (i != row)
						newList.Add(MyMeasurementFile.FilterParameterGridRows.ElementAt(i).Key, MyMeasurementFile.FilterParameterGridRows.ElementAt(i).Value);

				foreach (var item in MyMeasurementFile.HeaderGridRows)
				{
					if (item.Value.FilterParameter == MyMeasurementFile.FilterParameterGridRows.ElementAt(row).Key)
						item.Value.FilterParameter = String.Empty;
				}

				MyMeasurementFile.FilterParameterGridRows.Clear();
				MyMeasurementFile.FilterParameterGridRows = newList;
				RefreshFilterParametersGrid();
				RefreshHeaderParameterList();

				if (row - 1 >= 0)
					filterParametersDataGridView.CurrentCell = filterParametersDataGridView[0, row - 1];
			}
		}

		private void moveUpFilterParameterButton_Click(object sender, EventArgs e)
		{
			int row = filterParametersDataGridView.CurrentCellAddress.Y;

			if (row > 0)
			{
				int destination = row - 1;
				Dictionary<string, FilterParameterGridRow> newList = new Dictionary<string, FilterParameterGridRow>();

				for (int i = 0; i < MyMeasurementFile.FilterParameterGridRows.Count; i++)
				{
					if (i == destination)
						newList.Add(MyMeasurementFile.FilterParameterGridRows.ElementAt(row).Key, MyMeasurementFile.FilterParameterGridRows.ElementAt(row).Value);

					if (i != row)
						newList.Add(MyMeasurementFile.FilterParameterGridRows.ElementAt(i).Key, MyMeasurementFile.FilterParameterGridRows.ElementAt(i).Value);
				}

				MyMeasurementFile.FilterParameterGridRows.Clear();
				MyMeasurementFile.FilterParameterGridRows = newList;
				RefreshFilterParametersGrid();
				filterParametersDataGridView.CurrentCell = filterParametersDataGridView[0, destination];
			}
		}

		private void moveDownFilterParameterButton_Click(object sender, EventArgs e)
		{
			int row = filterParametersDataGridView.CurrentCellAddress.Y;

			if (row < filterParametersDataGridView.RowCount - 1 && row != -1)
			{
				int destination = row + 1;
				Dictionary<string, FilterParameterGridRow> newList = new Dictionary<string, FilterParameterGridRow>();

				for (int i = 0; i < MyMeasurementFile.FilterParameterGridRows.Count; i++)
				{
					if (i != row)
						newList.Add(MyMeasurementFile.FilterParameterGridRows.ElementAt(i).Key, MyMeasurementFile.FilterParameterGridRows.ElementAt(i).Value);

					if (i == destination)
						newList.Add(MyMeasurementFile.FilterParameterGridRows.ElementAt(row).Key, MyMeasurementFile.FilterParameterGridRows.ElementAt(row).Value);
				}

				MyMeasurementFile.FilterParameterGridRows.Clear();
				MyMeasurementFile.FilterParameterGridRows = newList;
				RefreshFilterParametersGrid();
				filterParametersDataGridView.CurrentCell = filterParametersDataGridView[0, destination];
			}
		}

		private void functionComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if ((int)MyMeasurementFile.Transform.Function != functionComboBox.SelectedIndex)
			{
				MyMeasurementFile.Transform.Function = (TransformFunction)functionComboBox.SelectedIndex;
				functionComboBox.BackColor = MyMeasurementFile.Transform.Function == OriginalMeasurementFile.Transform.Function ? default(Color) : ChangedValueColor;
			}
		}

		private void dateFieldComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (dateFieldComboBox.SelectedItem.ToString() != MyMeasurementFile.Transform.DateField)
			{
				MyMeasurementFile.Transform.DateField = dateFieldComboBox.SelectedItem.ToString();
				dateFieldComboBox.BackColor = MyMeasurementFile.Transform.DateField == OriginalMeasurementFile.Transform.DateField ? default(Color) : ChangedValueColor;

				if (dateFieldComboBox.SelectedIndex != -1)
				{
					MyMeasurementFile.HeaderGridRowsAreUpdated = false;
					RefreshHeadersGrid();
				}
			}
		}

		private void transformFieldsDataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			int columnIndex = transformFieldsDataGridView.CurrentCell.ColumnIndex;

			if (columnIndex == isValueFieldColumn.Index || columnIndex == removeFieldColumn.Index)
				transformFieldsDataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}

		public void transformFieldsDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex != -1)
			{
				DataGridViewCell checkBoxCell = transformFieldsDataGridView[e.ColumnIndex, e.RowIndex];
				TransformFieldGridRow selection = MyMeasurementFile.Transform.TransformFieldGridRows[e.RowIndex];
				TransformFieldGridRow original = OriginalMeasurementFile.Transform.TransformFieldGridRows.FirstOrDefault(p => p.Value.FieldName == selection.FieldName).Value ?? new TransformFieldGridRow();

				if (e.ColumnIndex == isValueFieldColumn.Index && selection.IsValueField != (bool)checkBoxCell.Value)
				{
					MyMeasurementFile.Transform.TransformFieldGridRows[e.RowIndex].IsValueField = (bool)checkBoxCell.Value;
					MyMeasurementFile.Transform.UpdatePropertiesFromTransformFieldGridRows();
					checkBoxCell.Style.BackColor = (bool)checkBoxCell.Value == original.IsValueField ? default(Color) : ChangedValueColor;
				}
				else if (e.ColumnIndex == removeFieldColumn.Index && selection.IsRemovedField != (bool)checkBoxCell.Value)
				{
					MyMeasurementFile.Transform.TransformFieldGridRows[e.RowIndex].IsRemovedField = (bool)checkBoxCell.Value;
					MyMeasurementFile.Transform.UpdatePropertiesFromTransformFieldGridRows();
					checkBoxCell.Style.BackColor = (bool)checkBoxCell.Value == original.IsRemovedField ? default(Color) : ChangedValueColor;
				}
			}
		}

		private void xAxisLabelTextBox_TextChanged(object sender, EventArgs e)
		{
			if (MyMeasurementFile.Label.XAxisLabel != xAxisLabelTextBox.Text)
			{
				MyMeasurementFile.Label.XAxisLabel = xAxisLabelTextBox.Text;
				xAxisLabelTextBox.BackColor = MyMeasurementFile.Label.XAxisLabel == OriginalMeasurementFile.Label.XAxisLabel ? default(Color) : ChangedValueColor;
			}
		}

		private void yAxisLabelTextBox_TextChanged(object sender, EventArgs e)
		{
			if (MyMeasurementFile.Label.YAxisLabel != yAxisLabelTextBox.Text)
			{
				MyMeasurementFile.Label.YAxisLabel = yAxisLabelTextBox.Text;
				yAxisLabelTextBox.BackColor = MyMeasurementFile.Label.YAxisLabel == OriginalMeasurementFile.Label.YAxisLabel ? default(Color) : ChangedValueColor;
			}
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
		}

		private void yAxisFormatComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if ((int)MyMeasurementFile.Label.YAxisFormat != yAxisFormatComboBox.SelectedIndex)
			{
				MyMeasurementFile.Label.YAxisFormat = (AxisFormat)yAxisFormatComboBox.SelectedIndex;
				yAxisFormatComboBox.BackColor = MyMeasurementFile.Label.YAxisFormat == OriginalMeasurementFile.Label.YAxisFormat ? default(Color) : ChangedValueColor;
			}
		}

		private void numberFormatsDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			NumberFormat format = MyMeasurementFile.NumberFormats.ElementAt(e.RowIndex);
			NumberFormat original = OriginalMeasurementFile.NumberFormats.Count > e.RowIndex ? OriginalMeasurementFile.NumberFormats.ElementAt(e.RowIndex) : new NumberFormat();

			if (e.ColumnIndex == prefixColumn.Index)
			{
				DataGridViewCell prefixCell = numberFormatsDataGridView[prefixColumn.Index, e.RowIndex];
				string cellValue = (prefixCell.Value ?? (object)String.Empty).ToString();

				if (format.Prefix != cellValue)
				{
					format.Prefix = cellValue;
					prefixCell.Style.BackColor = format.Prefix == original.Prefix ? default(Color) : ChangedValueColor;
				}
			}
			else if (e.ColumnIndex == groupingColumn.Index)
			{
				DataGridViewCell groupingCell = numberFormatsDataGridView[groupingColumn.Index, e.RowIndex];
				string cellValue = (groupingCell.Value ?? (object)String.Empty).ToString();

				if (format.GroupingSymbol != cellValue)
				{
					format.GroupingSymbol = cellValue;
					groupingCell.Style.BackColor = format.GroupingSymbol == original.GroupingSymbol ? default(Color) : ChangedValueColor;
				}
			}
			else if (e.ColumnIndex == patternColumn.Index)
			{
				DataGridViewCell patternCell = numberFormatsDataGridView[patternColumn.Index, e.RowIndex];
				string cellValue = (patternCell.Value ?? (object)String.Empty).ToString();

				if (format.Pattern != cellValue)
				{
					format.Pattern = cellValue;
					patternCell.Style.BackColor = format.Pattern == original.Pattern ? default(Color) : ChangedValueColor;
				}
			}
			else if (e.ColumnIndex == decimalColumn.Index)
			{
				DataGridViewCell decimalCell = numberFormatsDataGridView[decimalColumn.Index, e.RowIndex];
				string cellValue = (decimalCell.Value ?? (object)String.Empty).ToString();

				if (format.DecimalSymbol != cellValue)
				{
					format.DecimalSymbol = cellValue;
					decimalCell.Style.BackColor = format.DecimalSymbol == original.DecimalSymbol ? default(Color) : ChangedValueColor;
				}
			}
			else if (e.ColumnIndex == fractionDigitsColumn.Index)
			{
				DataGridViewCell fractionDigitsCell = numberFormatsDataGridView[fractionDigitsColumn.Index, e.RowIndex];
				string cellValue = (fractionDigitsCell.Value ?? (object)String.Empty).ToString();
				int intValue;

				if (int.TryParse(cellValue, out intValue) || String.IsNullOrEmpty(cellValue))
				{
					if (format.FractionDigits.HasValue && String.IsNullOrEmpty(cellValue))
						format.FractionDigits = null;
					else if ((format.FractionDigits ?? 0) != intValue)
						format.FractionDigits = intValue;
				}
				else
					fractionDigitsCell.Value = format.FractionDigits;

				fractionDigitsCell.Style.BackColor = format.FractionDigits == original.FractionDigits ? default(Color) : ChangedValueColor;
			}
			else if (e.ColumnIndex == suffixColumn.Index)
			{
				DataGridViewCell suffixCell = numberFormatsDataGridView[suffixColumn.Index, e.RowIndex];
				string cellValue = (suffixCell.Value ?? (object)String.Empty).ToString();

				if (format.Suffix != cellValue)
				{
					format.Suffix = cellValue;
					suffixCell.Style.BackColor = format.Suffix == original.Suffix ? default(Color) : ChangedValueColor;
				}
			}
		}

		private void numberFormatsDataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			int columnIndex = numberFormatsDataGridView.CurrentCell.ColumnIndex;

			if (columnIndex == negativeParensColumn.Index)
				numberFormatsDataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}

		private void numberFormatsDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex != -1 && e.ColumnIndex == negativeParensColumn.Index)
			{
				NumberFormat format = MyMeasurementFile.NumberFormats.ElementAt(e.RowIndex);
				NumberFormat original = OriginalMeasurementFile.NumberFormats.Count > e.RowIndex ? OriginalMeasurementFile.NumberFormats.ElementAt(e.RowIndex) : new NumberFormat();
				DataGridViewCell negativeParensCell = numberFormatsDataGridView[negativeParensColumn.Index, e.RowIndex];
				bool? cellValue = ToNullableBool((CheckState)negativeParensCell.Value);

				if (format.NegativeParens != cellValue)
				{
					format.NegativeParens = cellValue;
					negativeParensCell.Style.BackColor = format.NegativeParens == original.NegativeParens ? default(Color) : ChangedValueColor;
				}
			}
		}

		private void numberFormatsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == negativeColorButtonColumn.Index)
			{
				NumberFormat format = MyMeasurementFile.NumberFormats.ElementAt(e.RowIndex);
				NumberFormat original = OriginalMeasurementFile.NumberFormats.Count > e.RowIndex ? OriginalMeasurementFile.NumberFormats.ElementAt(e.RowIndex) : new NumberFormat();
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
			MyMeasurementFile.NumberFormats.Add(new NumberFormat());
			RefreshNumberFormatsGrid();
			numberFormatsDataGridView.CurrentCell = numberFormatsDataGridView[0, numberFormatsDataGridView.Rows.Count - 1];
		}

		private void deleteNumberFormatButton_Click(object sender, EventArgs e)
		{
			int row = numberFormatsDataGridView.CurrentCellAddress.Y;

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
			int row = numberFormatsDataGridView.CurrentCellAddress.Y;

			if (row > 0)
			{
				int column = numberFormatsDataGridView.CurrentCellAddress.X;
				NumberFormat subject = MyMeasurementFile.NumberFormats.ElementAt(row);
				MyMeasurementFile.NumberFormats.RemoveAt(row);
				MyMeasurementFile.NumberFormats.Insert(row - 1, subject);
				RefreshNumberFormatsGrid();
				numberFormatsDataGridView.CurrentCell = numberFormatsDataGridView[column, row - 1];
			}
		}

		private void moveDownNumberFormatButton_Click(object sender, EventArgs e)
		{
			int row = numberFormatsDataGridView.CurrentCellAddress.Y;

			if (row < numberFormatsDataGridView.Rows.Count - 1 && row != -1)
			{
				int column = numberFormatsDataGridView.CurrentCellAddress.X;
				NumberFormat subject = MyMeasurementFile.NumberFormats.ElementAt(row);
				MyMeasurementFile.NumberFormats.RemoveAt(row);
				MyMeasurementFile.NumberFormats.Insert(row + 1, subject);
				RefreshNumberFormatsGrid();
				numberFormatsDataGridView.CurrentCell = numberFormatsDataGridView[column, row + 1];
			}
		}

		private void headersDataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			int columnIndex = headersDataGridView.CurrentCell.ColumnIndex;

			if (columnIndex == headerTypeColumn.Index || columnIndex == headerFilterParameterColumn.Index || columnIndex == isReturnRowColumn.Index ||
				columnIndex == isDateColumn.Index || columnIndex == isReturnRowColumn.Index)
				headersDataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}

		private void headersDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex != -1)
			{
				int headerKey = (int)headersDataGridView[headerKeyColumn.Index, e.RowIndex].Value;
				DataGridViewCell subject = headersDataGridView[e.ColumnIndex, e.RowIndex];
				bool changeOccurred = false;

				if (e.ColumnIndex == headerTypeColumn.Index)
				{
					HeaderType selection = Enums.GetHeaderTypeEnum(subject.Value.ToString()); ;

					if (MyMeasurementFile.HeaderGridRows[headerKey].HeaderType != selection)
					{
						MyMeasurementFile.HeaderGridRows[headerKey].HeaderType = selection;
						MyMeasurementFile.UpdatePropertiesFromHeaderGridRows();
						changeOccurred = true;
					}
				}
				else if (e.ColumnIndex == headerFilterParameterColumn.Index)
				{
					string selection = subject.Value.ToString();

					if (MyMeasurementFile.HeaderGridRows[headerKey].FilterParameter != selection)
					{
						MyMeasurementFile.HeaderGridRows[headerKey].FilterParameter = selection;
						MyMeasurementFile.UpdatePropertiesFromHeaderGridRows();
						changeOccurred = true;
					}
				}
				else if (e.ColumnIndex == isReturnRowColumn.Index)
				{
					if (MyMeasurementFile.HeaderGridRows[headerKey].IsReturnRow != (bool)subject.Value)
					{
						MyMeasurementFile.HeaderGridRows[headerKey].IsReturnRow = (bool)subject.Value;
						MyMeasurementFile.UpdatePropertiesFromHeaderGridRows();
						changeOccurred = true;
					}
				}
				else if (e.ColumnIndex == isReturnRowDateColumn.Index)
				{
					if (MyMeasurementFile.HeaderGridRows[headerKey].IsReturnRowDate != (bool)subject.Value)
					{
						MyMeasurementFile.HeaderGridRows[headerKey].IsReturnRowDate = (bool)subject.Value;
						MyMeasurementFile.UpdatePropertiesFromHeaderGridRows();
						changeOccurred = true;
					}
				}
				else if (e.ColumnIndex == isReturnRowControlColumn.Index)
				{
					if (MyMeasurementFile.HeaderGridRows[headerKey].IsReturnRowControl != (bool)subject.Value)
					{
						MyMeasurementFile.HeaderGridRows[headerKey].IsReturnRowControl = (bool)subject.Value;
						MyMeasurementFile.UpdatePropertiesFromHeaderGridRows();
						changeOccurred = true;
					}
				}

				if (changeOccurred)
					RefreshHeadersGridRow(e.RowIndex);
			}
		}

		private void chartListRadioButton_CheckedChanged(object sender, EventArgs e)
		{
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
		}

		private void multichartsRadioButton_CheckedChanged(object sender, EventArgs e)
		{
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
						UpdateChartsGridRow(-1, i, MyMeasurementFile.Charts.ElementAt(i));
				}

				if (OriginalMeasurementFile.Multicharts.Charts.Count != 0)
				{
					chartListRadioButton.BackColor = ChangedValueColor;
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

				if (OriginalMeasurementFile.Charts.Count != 0)
				{
					chartListRadioButton.BackColor = default(Color);
					multichartsRadioButton.BackColor = ChangedValueColor;
				}
			}
		}

		private void multichartsComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (multichartsComboBox.SelectedIndex != -1)
			{
				chartsDataGridView.Rows.Clear();
				List<ChartInfo> charts = MyMeasurementFile.Multicharts.Charts.ElementAt(multichartsComboBox.SelectedIndex).Value;

				if (charts.Count != 0)
				{
					chartsDataGridView.Rows.Add(charts.Count);

					for (int i = 0; i < charts.Count; i++)
						UpdateChartsGridRow(multichartsComboBox.SelectedIndex, i, charts.ElementAt(i));
				}
			}
		}

		private void addMultichartsButton_Click(object sender, EventArgs e)
		{
			string key = "New Multichart";
			int genericMultichartsCount = MyMeasurementFile.Multicharts.Charts.Keys.Count(p => p.StartsWith(key));
			key = String.Concat(key, genericMultichartsCount == 0 ? String.Empty : String.Format(" {0}", genericMultichartsCount));
			MyMeasurementFile.Multicharts.Charts.Add(key, new List<ChartInfo>());
			multichartsComboBox.Items.Add(key);
			multichartsComboBox.SelectedIndex = multichartsComboBox.Items.Count - 1;
		}

		private void deleteMultichartsButton_Click(object sender, EventArgs e)
		{
			int selectedIndex = multichartsComboBox.SelectedIndex;

			if (selectedIndex != -1)
			{
				DialogResult proceed = DialogResult.Yes;
				string multichart = MyMeasurementFile.Multicharts.Charts.ElementAt(selectedIndex).Key;

				if (MyMeasurementFile.Multicharts.Charts[multichart].Count != 0)
				{
					string message = "By deleting the current multichart,\nall the assoicated charts will be deleted.\n\nDo you want to continue?";
					proceed = MessageBox.Show(message, "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
				}

				if (proceed == DialogResult.Yes)
				{
					Dictionary<string, List<ChartInfo>> newList = new Dictionary<string, List<ChartInfo>>();

					foreach(var item in MyMeasurementFile.Multicharts.Charts)
						if (item.Key != multichart)
							newList.Add(item.Key, item.Value);

					MyMeasurementFile.Multicharts.Charts = newList;
					multichartsComboBox.SelectedIndex = selectedIndex == 0 ? selectedIndex : selectedIndex - 1;
				}
			}
		}

		private void moveUpMultichartsButton_Click(object sender, EventArgs e)
		{
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
			int selectedIndex = multichartsComboBox.SelectedIndex;

			if (selectedIndex != -1)
			{
				string subject = MyMeasurementFile.Multicharts.Charts.ElementAt(selectedIndex).Key;
				MultichartNameDialogue dialogue = new MultichartNameDialogue(subject, ChangedValueColor);

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

				ChartDialog dialog = new ChartDialog(subjectChart.Clone(), originalChart, ChangedValueColor, MyMeasurementFile.Transform.ValueFields, chartIds);

				if (dialog.ShowDialog() == DialogResult.OK && !subjectChart.Equals(dialog.Subject))
				{
					subjectChart = dialog.Subject;

					if (chartListRadioButton.Checked)
						MyMeasurementFile.Charts[e.RowIndex] = subjectChart;
					else if (multichartsRadioButton.Checked)
						MyMeasurementFile.Multicharts.Charts[subjectMultichart][e.RowIndex] = subjectChart;

					UpdateChartsGridRow(multichartsIndex, e.RowIndex, subjectChart);
				}
			}
		}

		private void addChartButton_Click(object sender, EventArgs e)
		{
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
			}
		}

		private void deleteChartButton_Click(object sender, EventArgs e)
		{
			if (chartsDataGridView.SelectedCells.Count != 0 && chartsDataGridView.Rows.Count != 0)
			{
				int selectedIndex = chartsDataGridView.CurrentCellAddress.Y;
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
			}
		}

		private void moveUpChartButton_Click(object sender, EventArgs e)
		{
			int selectedIndex = chartsDataGridView.CurrentCellAddress.Y;

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
						newList.Add(chartList.ElementAt(selectedIndex));

					if (i != selectedIndex)
						newList.Add(chartList.ElementAt(i));
				}

				chartList = newList;

				UpdateChartsGridRow(multichartsIndex, selectedIndex - 1, chartList.ElementAt(selectedIndex - 1));
				UpdateChartsGridRow(multichartsIndex, selectedIndex, chartList.ElementAt(selectedIndex));
			}
		}

		private void moveDownChartButton_Click(object sender, EventArgs e)
		{
			int selectedIndex = chartsDataGridView.CurrentCellAddress.Y;

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
						newList.Add(chartList.ElementAt(i));

					if (i == selectedIndex + 1)
						newList.Add(chartList.ElementAt(selectedIndex));
				}

				chartList = newList;

				UpdateChartsGridRow(multichartsIndex, selectedIndex, chartList.ElementAt(selectedIndex));
				UpdateChartsGridRow(multichartsIndex, selectedIndex + 1, chartList.ElementAt(selectedIndex + 1));
			}
		}
	}
}
