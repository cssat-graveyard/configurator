using Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PortalConfigurator
{
	public partial class PortalConfiguratorForm : Form
	{
		private FilterParameterFile MyFilterParameterFile { get; set;}
		private FilterParameterFile OriginalFilterParameterFile { get; set; }
		private bool KeysGridNeedsRefresh { get; set; }

		private void InitializeFilterParameterInterface()
		{
			MyFilterParameterFile = new FilterParameterFile();
			OriginalFilterParameterFile = MyFilterParameterFile.Clone();
			KeysGridNeedsRefresh = false;
		}

		private void LoadFilterParameterInterface(object sender, EventArgs e)
		{
			filterParametersListView.Columns[0].Width = filterParametersListView.Width - SystemInformation.VerticalScrollBarWidth;
			neitherRadioButton.Checked = true;
			displayTypeComboBox.Items.AddRange(Enum.GetNames(typeof(DisplayType)));
			displayTypeComboBox.Items.RemoveAt(0);
			displayTypeComboBox.Items.Insert(0, String.Empty);
			displayColumnComboBox.Items.AddRange(Enum.GetNames(typeof(DisplayColumn)));
			displayColumnComboBox.Items.RemoveAt(0);
			displayColumnComboBox.Items.Insert(0, String.Empty);
			sortComboBox.Items.AddRange(Enum.GetNames(typeof(SortType)));
			sortComboBox.Items.RemoveAt(0);
			sortComboBox.Items.Insert(0, String.Empty);
			sortFunctionComboBox.Items.AddRange(Enum.GetNames(typeof(SortFunction)));
			sortFunctionComboBox.Items.RemoveAt(0);
			sortFunctionComboBox.Items.Insert(0, String.Empty);
			resultUnavailableComboBox.Items.AddRange(Enum.GetNames(typeof(ResultUnavailable)));
			resultUnavailableComboBox.Items.RemoveAt(0);
			resultUnavailableComboBox.Items.Insert(0, String.Empty);
			quarterDateComboBox.Items.AddRange(Enum.GetNames(typeof(QuarterDate)));
			quarterDateComboBox.Items.RemoveAt(0);
			quarterDateComboBox.Items.Insert(0, String.Empty);
		}

		private void newFilterParameterFileToolStripButton_Click(object sender, EventArgs e)
		{
			bool hasUnsavedChanges = !MyFilterParameterFile.Equals(OriginalFilterParameterFile);
			bool isOkayToContinue = true;

			if (hasUnsavedChanges)
			{
				string message = "You will lose unsaved changes if you continue.\nDo you wish to continue?";
				isOkayToContinue = MessageBox.Show(message, "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes;
			}

			if (!hasUnsavedChanges || isOkayToContinue)
			{
				MyFilterParameterFile = new FilterParameterFile();
				
				FilterParameter newFilterParameter = new FilterParameter();
				newFilterParameter.FilterParameterName = "new Filter/Parameter";

				MyFilterParameterFile.FilterParameters.Add(newFilterParameter);
				OriginalFilterParameterFile = MyFilterParameterFile.Clone();

				filterParametersListView.Items.Clear();
				filterParametersListView.Items.Add(newFilterParameter.FilterParameterName);
				filterParametersListView.SelectedIndices.Clear();
				filterParametersListView.SelectedIndices.Add(filterParametersListView.Items.Count - 1);
			}
		}

		private void openFilterParameterFileToolStripButton_Click(object sender, EventArgs e)
		{
			bool hasUnsavedChanges = !MyFilterParameterFile.Equals(OriginalFilterParameterFile);
			bool isOkayToContinue = true;

			if (hasUnsavedChanges)
			{
				string message = "You will lose unsaved changes if you continue.\nDo you wish to continue?";
				isOkayToContinue = MessageBox.Show(message, "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes;
			}

			if (!hasUnsavedChanges || isOkayToContinue)
			{
				openFileDialog.Title = "Open Filter/Parameter File";
#if DEBUG
				openFileDialog.InitialDirectory = Environment.CurrentDirectory.Replace(@"\bin\Debug", "");
#endif
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					MyFilterParameterFile = new FilterParameterFile(openFileDialog.FileName);

					try
					{
						MyFilterParameterFile.ParseJson();
					}
					catch (JsonParseException parseException)
					{
						MyFilterParameterFile = new FilterParameterFile(openFileDialog.FileName);
						string message = String.Format("The following problem occurred while parsing the file:\n{0}", parseException.Message);
						MessageBox.Show(message, "Parse Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					catch (Exception exception)
					{
						MyFilterParameterFile = new FilterParameterFile(openFileDialog.FileName);
						string message = String.Format("The following problem occurred while opening the file:\n{0}", exception.Message);
						MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}

					OriginalFilterParameterFile = MyFilterParameterFile.Clone();

					filterParametersListView.SelectedIndices.Clear();
					filterParametersListView.Items.Clear();

					foreach (var item in MyFilterParameterFile.FilterParameters)
						filterParametersListView.Items.Add(item.FilterParameterName);
				}
			}
		}

		private void saveFilterParameterFileToolStripButton_Click(object sender, EventArgs e)
		{
			saveFileDialog.Title = "Save Filter/Parameter File";

			if (!String.IsNullOrEmpty(MyFilterParameterFile.FilePath))
				saveFileDialog.InitialDirectory = MyFilterParameterFile.FilePath;

			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				MyFilterParameterFile.FilePath = saveFileDialog.FileName;

				try
				{
					MyFilterParameterFile.WriteFile();
					OriginalFilterParameterFile = MyFilterParameterFile.Clone();

					if (filterParametersListView.SelectedIndices.Count != 0)
						FilterParameterTypeSelectionChanged(filterParametersListView.SelectedIndices[0]);

					for (int i = 0; i < filterParametersListView.Items.Count; i++)
						SetListViewItemBackgroundColor(i);
				}
				catch (Exception exception)
				{
					string message = String.Format("The following problem occurred while writing the file:\n{0}", exception.Message);
					MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void copyFilterParameterToolStripButton_Click(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				FilterParameter newFilterParameter = MyFilterParameterFile.FilterParameters.ElementAt(filterParametersListView.SelectedIndices[0]);
				newFilterParameter.FilterParameterName += " - Copy";

				MyFilterParameterFile.FilterParameters.Add(newFilterParameter);
				filterParametersListView.Items.Add(newFilterParameter.FilterParameterName);
				filterParametersListView.SelectedIndices.Clear();
				filterParametersListView.SelectedIndices.Add(filterParametersListView.Items.Count - 1);
				filterParametersListView.Items[filterParametersListView.SelectedIndices[0]].BackColor = ChangedValueColor;
			}
		}

		private void addFilterParameterToolStripButton_Click(object sender, EventArgs e)
		{
			FilterParameter newFilterParameter = new FilterParameter();
			string baseName = "new Filter/Parameter";
			int newFilterParametersCount = MyFilterParameterFile.FilterParameters.Count(p => p.FilterParameterName.StartsWith(baseName));
			string newName = newFilterParametersCount == 0 ? baseName : String.Format("{0}{1}", baseName,
				newFilterParametersCount == 0 ? String.Empty : String.Format(" {0}", newFilterParametersCount));

			while (MyFilterParameterFile.FilterParameters.FindIndex(p => p.FilterParameterName == newName) != -1)
			{
				newFilterParametersCount++;
				newName = String.Format("{0} {1}", baseName, newFilterParametersCount);
			}

			newFilterParameter.FilterParameterName = newName;

			MyFilterParameterFile.FilterParameters.Add(newFilterParameter);
			filterParametersListView.Items.Add(newFilterParameter.FilterParameterName);
			filterParametersListView.SelectedIndices.Clear();
			filterParametersListView.SelectedIndices.Add(filterParametersListView.Items.Count - 1);
			filterParametersListView.Items[filterParametersListView.SelectedIndices[0]].BackColor = ChangedValueColor;
		}

		private void deleteFilterParameterToolStripButton_Click(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				MyFilterParameterFile.FilterParameters.RemoveAt(selectedIndex);
				filterParametersListView.Items.RemoveAt(selectedIndex);
				filterParametersListView.SelectedIndices.Clear();

				if (filterParametersListView.Items.Count - 1 >= selectedIndex - 1 && selectedIndex - 1 >= 0)
					filterParametersListView.SelectedIndices.Add(selectedIndex - 1);
				else if (filterParametersListView.Items.Count - 1 >= selectedIndex)
					filterParametersListView.SelectedIndices.Add(selectedIndex);
				else
				{
					filterParameterNameTextBox.Clear();
					commentsDataGridView.Rows.Clear();
					neitherRadioButton.Checked = true;
					dateCheckBox.CheckState = CheckState.Indeterminate;
					filterParameterTableNameTextBox.Clear();
					orderByTextBox.Clear();
					valueColumnComboBox.Items.Clear();
					valueColumnComboBox.SelectedIndex = -1;
					lagColumnComboBox.Items.Clear();
					lagColumnComboBox.SelectedIndex = -1;
					keysDataGridView.Rows.Clear();
					legendTextBox.Clear();
					aliasTextBox.Clear();
					displayTypeComboBox.SelectedIndex = -1;
					displayColumnComboBox.SelectedIndex = -1;
					sortComboBox.SelectedIndex = -1;
					resultUnavailableComboBox.SelectedIndex = -1;
					quarterDateComboBox.SelectedIndex = -1;
					monthStepNumericUpDown.Value = 0;
					monthLimitNumericUpDown.Value = 0;
					allMonthsCheckBox.Checked = false;
					multiCheckBox.CheckState = CheckState.Indeterminate;
					zeroLastCheckBox.CheckState = CheckState.Indeterminate;
					visibleCheckBox.CheckState = CheckState.Indeterminate;
					sortFunctionComboBox.SelectedIndex = -1;
					helpDataGridView.Rows.Clear();
				}
			}
		}

		private void moveUpFilterParameterToolStripButton_Click(object sender, EventArgs e)
		{
			int selectedIndex = filterParametersListView.SelectedIndices.Count == 0 ? -1 : filterParametersListView.SelectedIndices[0];

			if (selectedIndex > 0)
				MoveFilterParameterInList(selectedIndex, selectedIndex - 1);
		}

		private void moveDownFilterParameterToolStripButton_Click(object sender, EventArgs e)
		{
			int selectedIndex = filterParametersListView.SelectedIndices.Count == 0 ? 0 : filterParametersListView.SelectedIndices[0];

			if (selectedIndex < filterParametersListView.Items.Count - 1)
				MoveFilterParameterInList(selectedIndex, selectedIndex + 1);
		}

		private void MoveFilterParameterInList(int selectedIndex, int destinationIndex)
		{
			FilterParameter subject = new FilterParameter();
			Color backColor = filterParametersListView.Items[filterParametersListView.SelectedIndices[0]].BackColor;
			subject = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex);

			MyFilterParameterFile.FilterParameters.RemoveAt(selectedIndex);
			MyFilterParameterFile.FilterParameters.Insert(destinationIndex, subject);

			filterParametersListView.Items.RemoveAt(filterParametersListView.SelectedIndices[0]);
			filterParametersListView.Items.Insert(destinationIndex, subject.FilterParameterName);
			filterParametersListView.Items[destinationIndex].BackColor = backColor;
			filterParametersListView.SelectedIndices.Clear();
			filterParametersListView.SelectedIndices.Add(destinationIndex);
		}

		private void filterParametersListView_Resize(object sender, EventArgs e)
		{
			filterParametersListView.Columns[0].Width = filterParametersListView.Width;
		}

		private void filterParametersListView_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				FilterParameter selected = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex);
				FilterParameter original = GetOriginalFilterParameter(selectedIndex);
				Display display = selected.Display;
				KeysGridNeedsRefresh = true;

				filterParameterNameTextBox.Text = selected.FilterParameterName;
				dateCheckBox.CheckState = ToCheckState(selected.Date);

				commentsDataGridView.Rows.Clear();
				if (selected.Comments.Count != 0)
				{
					commentsDataGridView.Rows.Add(selected.Comments.Count);

					for (int i = 0; i < selected.Comments.Count; i++)
					{
						commentsDataGridView[0, i].Value = selected.Comments.ElementAt(i).Key;
						commentsDataGridView[1, i].Value = selected.Comments.ElementAt(i).Value;
					}

					SetGridCellBackgroundColor(ref commentsDataGridView, selected.Comments, original.Comments);
				}

				legendTextBox.Text = display.Legend;
				aliasTextBox.Text = display.Alias;
				displayTypeComboBox.SelectedIndex = (int)display.DisplayType;
				displayColumnComboBox.SelectedIndex = (int)display.DisplayColumn;
				sortComboBox.SelectedIndex = (int)display.SortType;
				sortFunctionComboBox.SelectedIndex = (int)display.SortFunction;
				resultUnavailableComboBox.SelectedIndex = (int)display.ResultUnavailable;
				quarterDateComboBox.SelectedIndex = (int)display.QuarterDate;
				monthStepNumericUpDown.Value = display.MonthStep ?? 0;
				monthStepNumericUpDown.Enabled = true;
				monthLimitNumericUpDown.Value = display.MonthLimit ?? 0;
				monthLimitNumericUpDown.Enabled = display.MonthLimit == 0;
				allMonthsCheckBox.Checked = display.MonthLimit == 0;
				allMonthsCheckBox.Enabled = true;
				multiCheckBox.CheckState = ToCheckState(display.Multi);
				zeroLastCheckBox.CheckState = ToCheckState(display.ZeroLast);
				visibleCheckBox.CheckState = ToCheckState(display.Visible);

				if (selected.Type == FilterParameterType.Values)
					valuesRadioButton.Checked = true;
				else if (selected.Type == FilterParameterType.Table)
					tableRadioButton.Checked = true;
				else if (selected.Type == FilterParameterType.Neither)
					neitherRadioButton.Checked = true;

				if (KeysGridNeedsRefresh)
					FilterParameterTypeSelectionChanged(selectedIndex);

				helpDataGridView.Rows.Clear();
				if (display.Help.Count != 0)
				{
					helpDataGridView.Rows.Add(display.Help.Count);

					for (int i = 0; i < display.Help.Count; i++)
					{
						helpDataGridView[0, i].Value = display.Help.ElementAt(i).Key;
						helpDataGridView[1, i].Value = display.Help.ElementAt(i).Value;
					}

					SetGridCellBackgroundColor(ref commentsDataGridView, selected.Comments, original.Comments);
				}
			}
			else
			{
				filterParameterNameTextBox.Clear();
				commentsDataGridView.Rows.Clear();
				neitherRadioButton.Checked = true;
				dateCheckBox.CheckState = CheckState.Indeterminate;
				legendTextBox.Clear();
				aliasTextBox.Clear();
				displayTypeComboBox.SelectedIndex = -1;
				displayColumnComboBox.SelectedIndex = -1;
				sortComboBox.SelectedIndex = -1;
				sortFunctionComboBox.SelectedIndex = -1;
				resultUnavailableComboBox.SelectedIndex = -1;
				quarterDateComboBox.SelectedIndex = -1;
				monthStepNumericUpDown.Value = 0;
				monthStepNumericUpDown.Enabled = false;
				monthLimitNumericUpDown.Value = 0;
				monthLimitNumericUpDown.Enabled = false;
				allMonthsCheckBox.Checked = false;
				allMonthsCheckBox.Enabled = false;
				multiCheckBox.CheckState = CheckState.Indeterminate;
				zeroLastCheckBox.CheckState = CheckState.Indeterminate;
				visibleCheckBox.CheckState = CheckState.Indeterminate;
				valuesTypeButton.Text = Enums.GetString(ValuesType.NoValues);
				valuesTypeButton.BackColor = default(Color);
				keysDataGridView.Rows.Clear();
				helpDataGridView.Rows.Clear();
			}
		}

		private void SetListViewItemBackgroundColor(int selectedIndex)
		{
			FilterParameter subject = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex);
			FilterParameter original = new FilterParameter();
			int originalIndex = OriginalFilterParameterFile.FilterParameters.FindIndex(p => p.FilterParameterName == subject.FilterParameterName);

			if (originalIndex != -1)
				original = OriginalFilterParameterFile.FilterParameters.ElementAt(originalIndex);

			filterParametersListView.Items[selectedIndex].BackColor = subject.Equals(original) ? default(Color) : ChangedValueColor;
		}

		private void addCommentButton_Click(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				Dictionary<string, string> subjectDictionary = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex).Comments;
				AddDictionaryItemToGrid(ref commentsDataGridView, ref subjectDictionary, GetOriginalFilterParameter(selectedIndex).Comments);
			}
		}

		private void deleteCommentButton_Click(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				Dictionary<string, string> subjectDictionary = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex).Comments;
				DeleteDictionaryItemFromGrid(ref commentsDataGridView, ref subjectDictionary, GetOriginalFilterParameter(selectedIndex).Comments);
			}
		}

		private void moveUpCommentButton_Click(object sender, EventArgs e)
		{
			int startingIndex = commentsDataGridView.CurrentCellAddress.Y;

			if (startingIndex > 0 && filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				Dictionary<string, string> subjectDictionary = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex).Comments;
				Dictionary<string, string> original = GetOriginalFilterParameter(selectedIndex).Comments;
				MoveDictionaryItemInGrid(startingIndex, startingIndex - 1, ref commentsDataGridView, ref subjectDictionary, original);
			}
		}

		private void moveDownCommentButton_Click(object sender, EventArgs e)
		{
			int startingIndex = commentsDataGridView.CurrentCellAddress.Y;

			if (startingIndex < commentsDataGridView.RowCount - 1 && filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				Dictionary<string, string> subjectDictionary = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex).Comments;
				Dictionary<string, string> original = GetOriginalFilterParameter(selectedIndex).Comments;
				MoveDictionaryItemInGrid(startingIndex, startingIndex + 1, ref commentsDataGridView, ref subjectDictionary, original);
			}
		}

		private void AddDictionaryItemToGrid(ref DataGridView subjectGrid, ref Dictionary<string, string> subjectDictionary,
			Dictionary<string, string> originalDictionary)
		{
			int newItemNumber = subjectDictionary.Count;
			string newKey = String.Empty;
			string baseKey = String.Empty;

			if (subjectGrid.Equals(commentsDataGridView))
				baseKey = "_comment";
			else if (subjectGrid.Equals(helpDataGridView))
				baseKey = subjectDictionary.Count == 0 ? "default" : "help";

			while (subjectDictionary.Keys.Contains(newKey))
			{
				newItemNumber++;
				newKey = String.Format("{0}{1}", baseKey, newItemNumber == 0 ? String.Empty : newItemNumber.ToString());
			}

			subjectGrid.Rows.Add();
			int newGridIndex = subjectGrid.RowCount - 1;
			subjectGrid[0, newGridIndex].Value = newKey;
			subjectGrid[1, newGridIndex].Value = String.Empty;

			subjectDictionary.Add(newKey, String.Empty);

			SetGridCellBackgroundColor(ref subjectGrid, subjectDictionary, originalDictionary);
		}

		private void DeleteDictionaryItemFromGrid(ref DataGridView subjectGrid, ref Dictionary<string, string> subjectDictionary,
			Dictionary<string, string> originalDictionary)
		{
			if (subjectGrid.RowCount != 0)
			{
				int selectedIndex = subjectGrid.CurrentCellAddress.Y;
				Dictionary<string, string> newDictionary = new Dictionary<string, string>();

				subjectGrid.Rows.RemoveAt(selectedIndex);

				for (int i = 0; i < subjectDictionary.Count; i++)
					if (i != selectedIndex)
						newDictionary.Add(subjectDictionary.Keys.ElementAt(i), subjectDictionary.Values.ElementAt(i));

				subjectDictionary.Clear();

				foreach (var item in newDictionary)
					subjectDictionary.Add(item.Key, item.Value);

				SetGridCellBackgroundColor(ref subjectGrid, subjectDictionary, originalDictionary);
			}
		}

		private void MoveDictionaryItemInGrid(int startingIndex, int destinationIndex, ref DataGridView subjectGrid,
			ref Dictionary<string, string> subjectDictionary, Dictionary<string, string> originalDictionary)
		{
			Dictionary<string, string> newDictionary = new Dictionary<string, string>();
			int i = 0;
			string key = subjectGrid[0, startingIndex].Value.ToString();
			string value = subjectGrid[1, startingIndex].Value.ToString();

			subjectGrid.Rows.RemoveAt(startingIndex);
			subjectGrid.Rows.Insert(destinationIndex, 1);
			subjectGrid[0, destinationIndex].Value = key;
			subjectGrid[1, destinationIndex].Value = value;
			subjectGrid.CurrentCell = subjectGrid[subjectGrid.CurrentCellAddress.X, destinationIndex];

			do
			{
				if (newDictionary.Count == destinationIndex)
					newDictionary.Add(key, value);

				if (i != startingIndex && i < subjectDictionary.Count)
					newDictionary.Add(subjectDictionary.Keys.ElementAt(i), subjectDictionary.Values.ElementAt(i));

				i++;
			} while (newDictionary.Count != subjectDictionary.Count);

			subjectDictionary.Clear();
			
			foreach (var item in newDictionary)
				subjectDictionary.Add(item.Key, item.Value);

			SetGridCellBackgroundColor(ref subjectGrid, subjectDictionary, originalDictionary);
		}

		private void SetGridCellBackgroundColor(ref DataGridView subjectGrid, Dictionary<string, string> subjectDictionary,
			Dictionary<string, string> originalDictionary)
		{
			string key;
			string value;
			bool changeDetected = false;
			int selectedIndex = filterParametersListView.SelectedIndices[0];

			for (int i = 0; i < subjectGrid.Rows.Count; i++)
			{
				key = subjectDictionary.ElementAt(i).Key;
				value = subjectDictionary.ElementAt(i).Value;

				if (originalDictionary.ContainsKey(key))
				{
					subjectGrid[0, i].Style.BackColor = default(Color);
					subjectGrid[1, i].Style.BackColor = value == originalDictionary[key] ? default(Color) : ChangedValueColor;

					if (!changeDetected)
						changeDetected = value == originalDictionary[key];
				}
				else
				{
					subjectGrid[0, i].Style.BackColor = ChangedValueColor;
					subjectGrid[1, i].Style.BackColor = ChangedValueColor;
					changeDetected = true;
				}
			}

			if (changeDetected && filterParametersListView.Items[selectedIndex].BackColor == default(Color))
				filterParametersListView.Items[selectedIndex].BackColor = ChangedValueColor;
			else if (!changeDetected && filterParametersListView.Items[selectedIndex].BackColor == ChangedValueColor)
				SetListViewItemBackgroundColor(selectedIndex);
		}

		private void addKeyButton_Click(object sender, EventArgs e)
		{
			if (!neitherRadioButton.Checked && filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				FilterParameter subject = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex);
				int key = subject.KeysGridRows.Count;

				while (subject.KeysGridRows.ContainsKey(key.ToString()))
					key++;

				KeysGridRow value = new KeysGridRow();
				value.Value = key.ToString();

				if (tableRadioButton.Checked)
					value.IsFromTable = false;
					
				subject.KeysGridRows.Add(key.ToString(), value);

				keysDataGridView.Rows.Add();

				int row = keysDataGridView.Rows.Count - 1;

				keysDataGridView[keysKeyColumn.Index, row].Value = key;
				keysDataGridView[keysValueColumn.Index, row].Value = value.Value;
				keysDataGridView[includeKeyColumn.Index, row].Value = !value.IsRemoved;
				keysDataGridView[selectKeyColumn.Index, row].Value = value.IsSelected;
				keysDataGridView[disableKeyColumn.Index, row].Value = value.IsDisabled;
				keysDataGridView[formatKeyColumn.Index, row].Value = value.Format;
				keysDataGridView[isTableSourceColumn.Index, row].Value = value.IsFromTable;
				keysDataGridView[sourceLabelColumn.Index, row].Value = GetKeySourceLabel(value.IsFromTable);

				SetKeysGridCellBackgroundColor(subject.KeysGridRows, GetOriginalFilterParameter(selectedIndex).KeysGridRows);
			}
		}

		private void deleteKeyButton_Click(object sender, EventArgs e)
		{
			if (keysDataGridView.RowCount > 0 && filterParametersListView.SelectedIndices.Count != 0)
			{
				int row = keysDataGridView.CurrentCellAddress.Y;
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				Dictionary<string, KeysGridRow> gridRows = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex).KeysGridRows;
				Dictionary<string, KeysGridRow> newDictionary = new Dictionary<string, KeysGridRow>();
				string key = gridRows.Keys.ElementAt(row);
				KeysGridRow value = gridRows.Values.ElementAt(row);

				if (value.IsFromTable != true)
				{
					keysDataGridView.Rows.RemoveAt(row);

					for (int i = 0; i < gridRows.Count; i++)
						if (i != row)
							newDictionary.Add(gridRows.Keys.ElementAt(i), gridRows.Values.ElementAt(i));

					gridRows.Clear();

					foreach (var item in newDictionary)
						gridRows.Add(item.Key, item.Value);
				}

				SetKeysGridCellBackgroundColor(gridRows, GetOriginalFilterParameter(selectedIndex).KeysGridRows);
			}
		}

		private void moveUpKeyButton_Click(object sender, EventArgs e)
		{
			int startingIndex = keysDataGridView.CurrentCellAddress.Y;

			if (startingIndex > 0 && filterParametersListView.SelectedIndices.Count != 0)
			{
				int minimumIndex = 0;
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				Dictionary<string, KeysGridRow> keysDictionary = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex).KeysGridRows;

				if (tableRadioButton.Checked)
					minimumIndex = keysDictionary.Values.ToList<KeysGridRow>().FindIndex(p => p.IsFromTable != true);

				if (startingIndex > minimumIndex && !neitherRadioButton.Checked)
					MoveKeyItemInGrid(startingIndex, startingIndex - 1, ref keysDictionary, GetOriginalFilterParameter(selectedIndex).KeysGridRows);
			}
		}

		private void moveDownKeyButton_Click(object sender, EventArgs e)
		{
			int startingIndex = keysDataGridView.CurrentCellAddress.Y;

			if (startingIndex < keysDataGridView.Rows.Count - 1 && filterParametersListView.SelectedIndices.Count != 0)
			{
				int minimumIndex = 0;
				int maximumIndex = keysDataGridView.RowCount - 1;
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				Dictionary<string, KeysGridRow> keysDictionary = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex).KeysGridRows;

				if (tableRadioButton.Checked)
				{
					minimumIndex = keysDictionary.Values.ToList<KeysGridRow>().FindIndex(p => p.IsFromTable != true);
					maximumIndex = keysDictionary.Values.ToList<KeysGridRow>().FindLastIndex(p => p.IsFromTable != true);
				}

				if (startingIndex >= minimumIndex && startingIndex < maximumIndex && !neitherRadioButton.Checked)
					MoveKeyItemInGrid(startingIndex, startingIndex + 1, ref keysDictionary, GetOriginalFilterParameter(selectedIndex).KeysGridRows);
			}
		}

		private void MoveKeyItemInGrid(int startingIndex, int destinationIndex, ref Dictionary<string, KeysGridRow> keysDictionary, Dictionary<string, KeysGridRow> originalDictionary)
		{
			Dictionary<string, KeysGridRow> newDictionary = new Dictionary<string, KeysGridRow>();
			int i = 0;
			string key = keysDictionary.ElementAt(startingIndex).Key;
			KeysGridRow value = keysDictionary[key];

			keysDataGridView.Rows.RemoveAt(startingIndex);
			keysDataGridView.Rows.Insert(destinationIndex, 1);
			keysDataGridView[keysKeyColumn.Index, destinationIndex].Value = key;
			keysDataGridView[keysValueColumn.Index, destinationIndex].Value = value.Value;
			keysDataGridView[includeKeyColumn.Index, destinationIndex].Value = !value.IsRemoved;
			keysDataGridView[selectKeyColumn.Index, destinationIndex].Value = value.IsSelected;
			keysDataGridView[disableKeyColumn.Index, destinationIndex].Value = value.IsDisabled;
			keysDataGridView[formatKeyColumn.Index, destinationIndex].Value = value.Format;
			keysDataGridView[isTableSourceColumn.Index, destinationIndex].Value = value.IsFromTable;
			keysDataGridView[sourceLabelColumn.Index, destinationIndex].Value = GetKeySourceLabel(value.IsFromTable);
			keysDataGridView.CurrentCell = keysDataGridView[keysDataGridView.CurrentCellAddress.X, destinationIndex];

			do
			{
				if (newDictionary.Count == destinationIndex)
					newDictionary.Add(key, value);

				if (i != startingIndex && i < keysDictionary.Count)
					newDictionary.Add(keysDictionary.Keys.ElementAt(i), keysDictionary.Values.ElementAt(i));

				i++;
			} while (newDictionary.Count != keysDictionary.Count);

			keysDictionary = newDictionary;
			SetKeysGridCellBackgroundColor(keysDictionary, originalDictionary);
		}

		private void SetKeysGridCellBackgroundColor(Dictionary<string, KeysGridRow> subjectDictionary, Dictionary<string, KeysGridRow> originalDictionary)
		{
			string key;
			KeysGridRow value;
			bool changeDetected = false;
			int selectedIndex = filterParametersListView.SelectedIndices[0];
			DataGridViewCell keysKeyCell;
			DataGridViewCell keysValueCell;
			DataGridViewCell includeKeyCell;
			DataGridViewCell selectKeyCell;
			DataGridViewCell disableKeyCell;
			DataGridViewCell formatKeyCell;
			DataGridViewCell sourceLabelCell;

			for (int i = 0; i < keysDataGridView.Rows.Count; i++)
			{
				key = subjectDictionary.ElementAt(i).Key;
				value = subjectDictionary.ElementAt(i).Value;
				keysKeyCell = keysDataGridView[keysKeyColumn.Index, i];
				keysValueCell = keysDataGridView[keysValueColumn.Index, i];
				includeKeyCell = keysDataGridView[includeKeyColumn.Index, i];
				selectKeyCell = keysDataGridView[selectKeyColumn.Index, i];
				disableKeyCell = keysDataGridView[disableKeyColumn.Index, i];
				formatKeyCell = keysDataGridView[formatKeyColumn.Index, i];
				sourceLabelCell = keysDataGridView[sourceLabelColumn.Index, i];

				if (originalDictionary.ContainsKey(key))
				{
					keysKeyCell.Style.BackColor = default(Color);
					keysValueCell.Style.BackColor = value.Value == originalDictionary[key].Value ? default(Color) : ChangedValueColor;
					includeKeyCell.Style.BackColor = value.IsRemoved == originalDictionary[key].IsRemoved ? default(Color) : ChangedValueColor;
					selectKeyCell.Style.BackColor = value.IsSelected == originalDictionary[key].IsSelected ? default(Color) : ChangedValueColor;
					disableKeyCell.Style.BackColor = value.IsDisabled == originalDictionary[key].IsDisabled ? default(Color) : ChangedValueColor;
					formatKeyCell.Style.BackColor = value.Format == originalDictionary[key].Format ? default(Color) : ChangedValueColor;
					sourceLabelCell.Style.BackColor = formatKeyCell.Style.BackColor;

					if (!changeDetected && (value.Value == originalDictionary[key].Value || value.IsRemoved == originalDictionary[key].IsRemoved ||
						value.IsSelected == originalDictionary[key].IsSelected || value.IsDisabled == originalDictionary[key].IsDisabled ||
						value.Format == originalDictionary[key].Format))
						changeDetected = true;
				}
				else
				{
					keysKeyCell.Style.BackColor = ChangedValueColor;
					keysValueCell.Style.BackColor = ChangedValueColor;
					includeKeyCell.Style.BackColor = ChangedValueColor;
					selectKeyCell.Style.BackColor = ChangedValueColor;
					disableKeyCell.Style.BackColor = ChangedValueColor;
					formatKeyCell.Style.BackColor = ChangedValueColor;
					sourceLabelCell.Style.BackColor = ChangedValueColor;
					changeDetected = true;
				}
			}

			if (changeDetected && filterParametersListView.Items[selectedIndex].BackColor == default(Color))
				filterParametersListView.Items[selectedIndex].BackColor = ChangedValueColor;
			else if (!changeDetected && filterParametersListView.Items[selectedIndex].BackColor == ChangedValueColor)
				SetListViewItemBackgroundColor(selectedIndex);

			FilterParameter subject = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex);
			FilterParameter original = GetOriginalFilterParameter(selectedIndex);
			SetValuesTypeButtonText(subject, original);
		}

		private void addHelpButton_Click(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				Dictionary<string, string> subjectDictionary = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex).Display.Help;
				AddDictionaryItemToGrid(ref helpDataGridView, ref subjectDictionary, GetOriginalFilterParameter(selectedIndex).Display.Help);
			}
		}

		private void deleteHelpButton_Click(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				Dictionary<string, string> subjectDictionary = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex).Display.Help;
				DeleteDictionaryItemFromGrid(ref helpDataGridView, ref subjectDictionary, GetOriginalFilterParameter(selectedIndex).Display.Help);
			}
		}

		private void moveUpHelpButton_Click(object sender, EventArgs e)
		{
			int startingIndex = helpDataGridView.CurrentCellAddress.Y;

			if (startingIndex > 0 && filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				Dictionary<string, string> subjectDictionary = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex).Display.Help;
				Dictionary<string, string> original = GetOriginalFilterParameter(selectedIndex).Display.Help;
				MoveDictionaryItemInGrid(startingIndex, startingIndex - 1, ref helpDataGridView, ref subjectDictionary, original);
			}
		}

		private void moveDownHelpButton_Click(object sender, EventArgs e)
		{
			int startingIndex = helpDataGridView.CurrentCellAddress.Y;

			if (startingIndex < helpDataGridView.RowCount - 1 && filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				Dictionary<string, string> subjectDictionary = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex).Display.Help;
				Dictionary<string, string> original = GetOriginalFilterParameter(selectedIndex).Display.Help;
				MoveDictionaryItemInGrid(startingIndex, startingIndex + 1, ref helpDataGridView, ref subjectDictionary, original);
			}
		}

		private void filterParameterNameTextBox_TextChanged(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				FilterParameter subject = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex);

				if (filterParameterNameTextBox.Text != subject.FilterParameterName)
				{
					subject.FilterParameterName = filterParameterNameTextBox.Text;
					FilterParameter original = GetOriginalFilterParameter(selectedIndex);
					filterParameterNameTextBox.BackColor = subject.FilterParameterName == original.FilterParameterName ? default(Color) : ChangedValueColor;

					SetGridCellBackgroundColor(ref commentsDataGridView, subject.Comments, original.Comments);

					if (valuesRadioButton.Checked)
					{
						valuesRadioButton.BackColor = subject.Type == original.Type ? default(Color) : ChangedValueColor;
						tableRadioButton.BackColor = default(Color);
						neitherRadioButton.BackColor = default(Color);
						SetKeysGridCellBackgroundColor(subject.KeysGridRows, original.KeysGridRows);
					}
					else if (tableRadioButton.Checked)
					{
						valuesRadioButton.BackColor = default(Color);
						tableRadioButton.BackColor = subject.Type == original.Type ? default(Color) : ChangedValueColor;
						neitherRadioButton.BackColor = default(Color);
						SetKeysGridCellBackgroundColor(subject.KeysGridRows, original.KeysGridRows);
					}
					else if (neitherRadioButton.Checked)
					{
						valuesRadioButton.BackColor = default(Color);
						tableRadioButton.BackColor = default(Color);
						neitherRadioButton.BackColor = subject.Type == original.Type ? default(Color) : ChangedValueColor;
					}

					filterParameterTableNameTextBox.BackColor = subject.Table == original.Table ? default(Color) : ChangedValueColor;
					orderByTextBox.BackColor = subject.OrderBy == original.OrderBy ? default(Color) : ChangedValueColor;
					valueColumnComboBox.BackColor = subject.ValueColumn == original.ValueColumn ? default(Color) : ChangedValueColor;
					lagColumnComboBox.BackColor = subject.LagColumn == original.LagColumn ? default(Color) : ChangedValueColor;
					dateCheckBox.BackColor = subject.Date == original.Date ? default(Color) : ChangedValueColor;
					legendTextBox.BackColor = subject.Display.Legend == original.Display.Legend ? default(Color) : ChangedValueColor;
					aliasTextBox.BackColor = subject.Display.Alias == original.Display.Alias ? default(Color) : ChangedValueColor;
					displayTypeComboBox.BackColor = subject.Display.DisplayType == original.Display.DisplayType ? default(Color) : ChangedValueColor;
					displayColumnComboBox.BackColor = subject.Display.DisplayColumn == original.Display.DisplayColumn ? default(Color) : ChangedValueColor;
					sortComboBox.BackColor = subject.Display.SortType == original.Display.SortType ? default(Color) : ChangedValueColor;
					resultUnavailableComboBox.BackColor = subject.Display.ResultUnavailable == original.Display.ResultUnavailable ? default(Color) : ChangedValueColor;
					quarterDateComboBox.BackColor = subject.Display.QuarterDate == original.Display.QuarterDate ? default(Color) : ChangedValueColor;
					monthStepNumericUpDown.BackColor = subject.Display.MonthStep == original.Display.MonthStep ? default(Color) : ChangedValueColor;
					monthLimitNumericUpDown.BackColor = subject.Display.MonthLimit == original.Display.MonthLimit ? default(Color) : ChangedValueColor;
					allMonthsCheckBox.BackColor = subject.Display.MonthLimit == 0 && original.Display.MonthLimit == 0 ? default(Color) : ChangedValueColor;
					multiCheckBox.BackColor = subject.Display.Multi == original.Display.Multi ? default(Color) : ChangedValueColor;
					zeroLastCheckBox.BackColor = subject.Display.ZeroLast == original.Display.ZeroLast ? default(Color) : ChangedValueColor;
					visibleCheckBox.BackColor = subject.Display.Visible == original.Display.Visible ? default(Color) : ChangedValueColor;

					if (subject.Display.SortType == SortType.Function)
						sortFunctionComboBox.BackColor = subject.Display.SortFunction == original.Display.SortFunction ? default(Color) : ChangedValueColor;

					SetGridCellBackgroundColor(ref helpDataGridView, subject.Display.Help, original.Display.Help);
					filterParametersListView.Items[selectedIndex].BackColor = subject.Equals(original) ? default(Color) : ChangedValueColor;
				}
			}
			else if (!String.IsNullOrEmpty(filterParameterNameTextBox.Text))
			{
				filterParameterNameTextBox.Clear();
				filterParameterNameTextBox.BackColor = default(Color);
			}
		}

		private void valuesRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				FilterParameterType type = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex).Type;

				if (valuesRadioButton.Checked && type != FilterParameterType.Values)
					FilterParameterTypeSelectionChanged(selectedIndex);
			}
			else if (neitherRadioButton.Checked == false)
			{
				neitherRadioButton.Checked = true;
				valuesRadioButton.BackColor = default(Color);
			}
		}

		private void tableRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				FilterParameterType type = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex).Type;

				if (tableRadioButton.Checked && type != FilterParameterType.Table)
					FilterParameterTypeSelectionChanged(selectedIndex);
			}
			else if (neitherRadioButton.Checked == false)
			{
				neitherRadioButton.Checked = true;
				tableRadioButton.BackColor = default(Color);
			}
		}

		private void neitherRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				FilterParameterType type = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex).Type;

				if (neitherRadioButton.Checked && type != FilterParameterType.Neither)
					FilterParameterTypeSelectionChanged(selectedIndex);
			}
			else if (neitherRadioButton.Checked == false)
			{
				neitherRadioButton.Checked = true;
				neitherRadioButton.BackColor = default(Color);
			}
		}

		private void FilterParameterTypeSelectionChanged(int selectedIndex)
		{
			FilterParameter selected = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex);
			FilterParameter original = GetOriginalFilterParameter(selectedIndex);
			Display display = selected.Display;
			bool enableFilterParameterToolStrip = false;
			bool enableFilterParametersListView = false;
			bool enableFilterParameterNameTextBox = false;
			bool enableCommentsGroupBox = false;
			bool enableTableSettingsGroupBox = false;
			bool enableValueColumnComboBox = false;
			bool enableLagColumnComboBox = false;
			bool enableTypeGroupBox = false;
			bool enableDisplayGroupBox = false;
			bool enableValuesTypeButton = false;
			bool enableKeysDataGridView = false;
			bool enableHelpGroupBox = false;

			FilterParameterType chosenType;

			if (valuesRadioButton.Checked)
				chosenType = FilterParameterType.Values;
			else if (tableRadioButton.Checked)
				chosenType = FilterParameterType.Table;
			else
				chosenType = FilterParameterType.Neither;

			if (chosenType != selected.Type)
			{
				if (MessageBox.Show("By changing the type, you will lose the current settings.\n\nDo you wish to continue?", "Change Warning",
					MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
				{
					if (chosenType == FilterParameterType.Values)
						valuesRadioButton.Checked = true;
					else if (chosenType == FilterParameterType.Table)
						tableRadioButton.Checked = true;
					else
						neitherRadioButton.Checked = true;

					return;
				}
			}

			try
			{
				filterParameterToolStrip.Enabled = enableFilterParameterToolStrip;
				filterParametersListView.Enabled = enableFilterParametersListView;
				filterParameterNameTextBox.Enabled = enableFilterParameterNameTextBox;
				commentsGroupBox.Enabled = enableCommentsGroupBox;
				typeGroupBox.Enabled = enableTypeGroupBox;
				tableSettingsGroupBox.Enabled = enableTableSettingsGroupBox;
				valueColumnComboBox.Enabled = enableValueColumnComboBox;
				lagColumnComboBox.Enabled = enableLagColumnComboBox;
				displayGroupBox.Enabled = enableDisplayGroupBox;
				valuesTypeButton.Enabled = enableValuesTypeButton;
				keysDataGridView.Enabled = enableKeysDataGridView;
				keysDataGridView.Rows.Clear();
				helpGroupBox.Enabled = enableHelpGroupBox;

				if (valuesRadioButton.Checked)
				{
					valuesRadioButton.BackColor = FilterParameterType.Values == original.Type ? default(Color) : ChangedValueColor;
					tableRadioButton.BackColor = default(Color);
					neitherRadioButton.BackColor = default(Color);
					selected.TableColumns.Clear();
					selected.TableDataAreLoaded = false;

					enableFilterParameterToolStrip = true;
					enableFilterParametersListView = true;
					enableFilterParameterNameTextBox = true;
					enableCommentsGroupBox = true;

					if (selected.Type == FilterParameterType.Table)
					{
						filterParameterTableNameTextBox.Text = String.Empty;
						orderByTextBox.Text = String.Empty;
						valueColumnComboBox.SelectedIndex = -1;
						lagColumnComboBox.SelectedIndex = -1;
					}

					enableTypeGroupBox = true;
					enableDisplayGroupBox = true;
					enableValuesTypeButton = true;
					SetValuesTypeButtonText(selected, original);
					enableHelpGroupBox = true;
				}
				else if (tableRadioButton.Checked)
				{
					valuesRadioButton.BackColor = default(Color);
					tableRadioButton.BackColor = FilterParameterType.Table == original.Type ? default(Color) : ChangedValueColor;
					neitherRadioButton.BackColor = default(Color);

					enableFilterParameterToolStrip = true;
					enableFilterParametersListView = true;
					enableFilterParameterNameTextBox = true;
					enableTableSettingsGroupBox = true;
					enableTableSettingsGroupBox = true;
					enableCommentsGroupBox = true;
					filterParameterTableNameTextBox.Text = selected.Table;
					orderByTextBox.Text = selected.OrderBy;
					valueColumnComboBox.Items.Clear();
					lagColumnComboBox.Items.Clear();
					enableTypeGroupBox = true;
					enableDisplayGroupBox = true;
					enableValuesTypeButton = false;
					SetValuesTypeButtonText(selected, original);
					keysDataGridView.Rows.Clear();
					enableHelpGroupBox = true;

					valueColumnComboBox.Items.AddRange(selected.TableColumns.Values.ToArray());
					valueColumnComboBox.SelectedIndex = (selected.ValueColumn ?? 0) - 1;
					enableValueColumnComboBox = true;

					lagColumnComboBox.Items.AddRange(selected.TableColumns.Values.ToArray());
					lagColumnComboBox.SelectedIndex = (selected.LagColumn ?? 0) - 1;
					enableLagColumnComboBox = true;
				}
				else if (neitherRadioButton.Checked)
				{
					valuesRadioButton.BackColor = default(Color);
					tableRadioButton.BackColor = default(Color);
					neitherRadioButton.BackColor = FilterParameterType.Neither == original.Type ? default(Color) : ChangedValueColor;
					selected.KeysGridRows.Clear();
					selected.TableDataAreLoaded = false;

					enableFilterParameterToolStrip = true;
					enableFilterParametersListView = true;
					enableFilterParameterNameTextBox = true;
					enableCommentsGroupBox = true;
					
					if (selected.Type == FilterParameterType.Table)
					{
						filterParameterTableNameTextBox.Text = String.Empty;
						orderByTextBox.Text = String.Empty;
						valueColumnComboBox.SelectedIndex = -1;
						lagColumnComboBox.SelectedIndex = -1;
					}
					
					enableTypeGroupBox = true;
					enableDisplayGroupBox = true;
					enableValuesTypeButton = false;
					SetValuesTypeButtonText(selected, original);
					enableHelpGroupBox = true;
				}

				if (selected.KeysGridRows.Count != 0)
				{
					keysDataGridView.Rows.Add(selected.KeysGridRows.Count);
					SetKeysGridCellBackgroundColor(selected.KeysGridRows, original.KeysGridRows);
				}

				for (int i = 0; i < selected.KeysGridRows.Count; i++)
				{
					string key = selected.KeysGridRows.ElementAt(i).Key;
					KeysGridRow value = selected.KeysGridRows.ElementAt(i).Value;

					keysDataGridView[keysKeyColumn.Index, i].Value = key;
					keysDataGridView[keysKeyColumn.Index, i].ReadOnly = value.IsFromTable ?? false;
					keysDataGridView[keysValueColumn.Index, i].Value = value.Value;
					keysDataGridView[keysValueColumn.Index, i].ReadOnly = value.IsFromTable ?? false;
					keysDataGridView[includeKeyColumn.Index, i].Value = !value.IsRemoved;
					keysDataGridView[includeKeyColumn.Index, i].ReadOnly = !(value.IsFromTable ?? false);
					keysDataGridView[selectKeyColumn.Index, i].Value = value.IsSelected;
					keysDataGridView[disableKeyColumn.Index, i].Value = value.IsDisabled;
					keysDataGridView[formatKeyColumn.Index, i].Value = value.Format;
					keysDataGridView[isTableSourceColumn.Index, i].Value = value.IsFromTable;
					keysDataGridView[sourceLabelColumn.Index, i].Value = GetKeySourceLabel(value.IsFromTable);
				}

				selected.Type = chosenType;

				enableKeysDataGridView = true;
			}
			catch (Exception e)
			{
				enableFilterParameterToolStrip = true;
				enableFilterParametersListView = true;
				enableCommentsGroupBox = true;
				enableTableSettingsGroupBox = true;
				enableValueColumnComboBox = false;
				valueColumnComboBox.SelectedIndex = -1;
				enableLagColumnComboBox = false;
				lagColumnComboBox.SelectedIndex = -1;
				enableTypeGroupBox = true;
				enableDisplayGroupBox = true;
				enableValuesTypeButton = false;
				valuesTypeButton.Text = Enums.GetString(ValuesType.NoValues);
				valuesTypeButton.BackColor = default(Color);
				enableKeysDataGridView = true;
				keysDataGridView.Rows.Clear();
				enableHelpGroupBox = true;

				MessageBox.Show(e.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			KeysGridNeedsRefresh = false;
			filterParameterTableNameTextBox.BackColor = selected.Table == original.Table ? default(Color) : ChangedValueColor;
			orderByTextBox.BackColor = selected.OrderBy == original.OrderBy ? default(Color) : ChangedValueColor;
			valueColumnComboBox.BackColor = selected.ValueColumn == original.ValueColumn ? default(Color) : ChangedValueColor;
			lagColumnComboBox.BackColor = selected.LagColumn == original.LagColumn ? default(Color) : ChangedValueColor;
			filterParametersListView.Items[selectedIndex].BackColor = selected.Equals(original) ? default(Color) : ChangedValueColor;

			filterParameterToolStrip.Enabled = enableFilterParameterToolStrip;
			filterParametersListView.Enabled = enableFilterParametersListView;
			filterParameterNameTextBox.Enabled = enableFilterParameterNameTextBox;
			commentsGroupBox.Enabled = enableCommentsGroupBox;
			typeGroupBox.Enabled = enableTypeGroupBox;
			tableSettingsGroupBox.Enabled = enableTableSettingsGroupBox;
			valueColumnComboBox.Enabled = enableValueColumnComboBox;
			lagColumnComboBox.Enabled = enableLagColumnComboBox;
			displayGroupBox.Enabled = enableDisplayGroupBox;
			valuesTypeButton.Enabled = enableValuesTypeButton;
			keysDataGridView.Enabled = enableKeysDataGridView;
			helpGroupBox.Enabled = enableHelpGroupBox;
		}

		private string GetKeySourceLabel(bool? isFromTable)
		{
			string sourceLabel = String.Empty;
			
			if (isFromTable == true)
				sourceLabel = "Table";
			else if (isFromTable == false)
				sourceLabel = "Added Keys";
			else if (isFromTable == null)
				sourceLabel = "Values List";

			return sourceLabel;
		}

		private void dateCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				FilterParameter subject = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex);
				FilterParameter original = GetOriginalFilterParameter(selectedIndex);

				if (ToNullableBool(dateCheckBox.CheckState) != subject.Date)
					subject.Date = ToNullableBool(dateCheckBox.CheckState);

				dateCheckBox.BackColor = subject.Date == original.Date ? default(Color) : ChangedValueColor;
				filterParametersListView.Items[selectedIndex].BackColor = subject.Equals(original) ? default(Color) : ChangedValueColor;
			}
			else if (dateCheckBox.CheckState != CheckState.Indeterminate)
			{
				dateCheckBox.CheckState = CheckState.Indeterminate;
				dateCheckBox.BackColor = default(Color);
			}
		}

		private void filterParameterTableNameTextBox_TextChanged(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				FilterParameter subject = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex);
				FilterParameter original = GetOriginalFilterParameter(selectedIndex);

				if (filterParameterTableNameTextBox.Text != subject.Table)
					subject.Table = filterParameterTableNameTextBox.Text;

				filterParameterTableNameTextBox.BackColor = subject.Table == original.Table ? default(Color) : ChangedValueColor;
				filterParametersListView.Items[selectedIndex].BackColor = subject.Equals(original) ? default(Color) : ChangedValueColor;
			}
			else if (!String.IsNullOrEmpty(filterParameterTableNameTextBox.Text))
			{
				filterParameterTableNameTextBox.Clear();
				filterParameterTableNameTextBox.BackColor = default(Color);
			}
		}

		private void loadFilterParameterTableButton_Click(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex).TableDataAreLoaded = false;
				FilterParameterTypeSelectionChanged(selectedIndex);
			}
		}

		private void orderByTextBox_TextChanged(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				FilterParameter subject = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex);
				FilterParameter original = GetOriginalFilterParameter(selectedIndex);

				if (orderByTextBox.Text != subject.OrderBy)
					subject.OrderBy = orderByTextBox.Text;

				orderByTextBox.BackColor = subject.OrderBy == original.OrderBy ? default(Color) : ChangedValueColor;
				filterParametersListView.Items[selectedIndex].BackColor = subject.Equals(original) ? default(Color) : ChangedValueColor;
			}
			else if (!String.IsNullOrEmpty(orderByTextBox.Text))
			{
				orderByTextBox.Clear();
				orderByTextBox.BackColor = default(Color);
			}
		}

		private void valueColumnComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				FilterParameter subject = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex);
				FilterParameter original = GetOriginalFilterParameter(selectedIndex);

				if (valueColumnComboBox.SelectedIndex == -1 && subject.ValueColumn != null)
					subject.ValueColumn = null;
				else if (valueColumnComboBox.SelectedIndex != (subject.ValueColumn ?? -1))
					subject.ValueColumn = valueColumnComboBox.SelectedIndex;

				valueColumnComboBox.BackColor = subject.ValueColumn == original.ValueColumn ? default(Color) : ChangedValueColor;
				filterParametersListView.Items[selectedIndex].BackColor = subject.Equals(original) ? default(Color) : ChangedValueColor;
			}
			else if (valueColumnComboBox.SelectedIndex != -1)
			{
				valueColumnComboBox.SelectedIndex = -1;
				valueColumnComboBox.BackColor = default(Color);
			}
		}

		private void lagColumnComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				FilterParameter subject = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex);
				FilterParameter original = GetOriginalFilterParameter(selectedIndex);

				if (lagColumnComboBox.SelectedIndex == -1 && subject.LagColumn != null)
					subject.LagColumn = null;
				else if (lagColumnComboBox.SelectedIndex != (subject.LagColumn ?? -1))
					subject.LagColumn = lagColumnComboBox.SelectedIndex;

				lagColumnComboBox.BackColor = subject.LagColumn == original.LagColumn ? default(Color) : ChangedValueColor;
				filterParametersListView.Items[selectedIndex].BackColor = subject.Equals(original) ? default(Color) : ChangedValueColor;
			}
			else if (lagColumnComboBox.SelectedIndex != -1)
			{
				lagColumnComboBox.SelectedIndex = -1;
				lagColumnComboBox.BackColor = default(Color);
			}
		}

		private void commentsDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			int selectedIndex = filterParametersListView.SelectedIndices[0];
			Dictionary<string, string> comments = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex).Comments;
			Dictionary<string, string> original = GetOriginalFilterParameter(selectedIndex).Comments;
			UpdateDictionaryItemInGrid(ref commentsDataGridView, ref comments, original);
		}

		private void UpdateDictionaryItemInGrid(ref DataGridView subjectGrid, ref Dictionary<string, string> subjectDictionary,
			Dictionary<string, string> originalDictionary)
		{
			int column = subjectGrid.CurrentCellAddress.X;
			int row = subjectGrid.CurrentCellAddress.Y;
			string cellValue = subjectGrid.CurrentCell.Value.ToString();
			string key = subjectDictionary.ElementAt(row).Key;

			if (column == 0)
			{
				if (subjectGrid.Equals(commentsDataGridView))
					if (!cellValue.StartsWith("_comment"))
							cellValue = String.Concat("_comment", cellValue);

				if (cellValue != key && !subjectDictionary.ContainsKey(cellValue))
				{
					Dictionary<string, string> newDictionary = new Dictionary<string, string>();

					for (int i = 0; i < subjectDictionary.Count; i++)
						newDictionary.Add(i == row ? cellValue : subjectDictionary.ElementAt(i).Key, subjectDictionary.ElementAt(i).Value);

					subjectDictionary.Clear();

					foreach (var item in newDictionary)
						subjectDictionary.Add(item.Key, item.Value);
				}
				else
					subjectGrid.CurrentCell.Value = subjectDictionary.ElementAt(row).Key;
			}
			else if (column == 1)
			{
				if (cellValue != subjectDictionary[key])
					subjectDictionary[key] = cellValue;
			}

			SetGridCellBackgroundColor(ref subjectGrid, subjectDictionary, originalDictionary);
		}

		private void legendTextBox_TextChanged(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				FilterParameter subject = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex);
				FilterParameter original = GetOriginalFilterParameter(selectedIndex);

				if (legendTextBox.Text != subject.Display.Legend)
					subject.Display.Legend = legendTextBox.Text;

				legendTextBox.BackColor = subject.Display.Legend == original.Display.Legend ? default(Color) : ChangedValueColor;
				filterParametersListView.Items[selectedIndex].BackColor = subject.Equals(original) ? default(Color) : ChangedValueColor;
			}
			else if (!String.IsNullOrEmpty(legendTextBox.Text))
			{
				legendTextBox.Clear();
				legendTextBox.BackColor = default(Color);
			}
		}

		private void aliasTextBox_TextChanged(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				FilterParameter subject = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex);
				FilterParameter original = GetOriginalFilterParameter(selectedIndex);

				if (aliasTextBox.Text != subject.Display.Alias)
					subject.Display.Alias = aliasTextBox.Text;

				aliasTextBox.BackColor = subject.Display.Alias == original.Display.Alias ? default(Color) : ChangedValueColor;
				filterParametersListView.Items[selectedIndex].BackColor = subject.Equals(original) ? default(Color) : ChangedValueColor;
			}
			else if (!String.IsNullOrEmpty(aliasTextBox.Text))
			{
				aliasTextBox.Clear();
				aliasTextBox.BackColor = default(Color);
			}
		}

		private void displayTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				FilterParameter subject = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex);
				FilterParameter original = GetOriginalFilterParameter(selectedIndex);

				if (displayTypeComboBox.SelectedIndex != (int)subject.Display.DisplayType)
					subject.Display.DisplayType = (DisplayType)displayTypeComboBox.SelectedIndex;

				displayTypeComboBox.BackColor = subject.Display.DisplayType == original.Display.DisplayType ? default(Color) : ChangedValueColor;
				filterParametersListView.Items[selectedIndex].BackColor = subject.Equals(original) ? default(Color) : ChangedValueColor;
			}
			else if (displayTypeComboBox.SelectedIndex != -1)
			{
				displayTypeComboBox.SelectedIndex = -1;
				displayTypeComboBox.BackColor = default(Color);
			}
		}

		private void displayColumnComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				FilterParameter subject = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex);
				FilterParameter original = GetOriginalFilterParameter(selectedIndex);

				if (displayColumnComboBox.SelectedIndex != (int)subject.Display.DisplayColumn)
					subject.Display.DisplayColumn = (DisplayColumn)displayColumnComboBox.SelectedIndex;

				displayColumnComboBox.BackColor = subject.Display.DisplayColumn == original.Display.DisplayColumn ? default(Color) : ChangedValueColor;
				filterParametersListView.Items[selectedIndex].BackColor = subject.Equals(original) ? default(Color) : ChangedValueColor;
			}
			else if (displayColumnComboBox.SelectedIndex != -1)
			{
				displayColumnComboBox.SelectedIndex = -1;
				displayColumnComboBox.BackColor = default(Color);
			}
		}
		
		private void sortComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				FilterParameter subject = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex);
				FilterParameter original = GetOriginalFilterParameter(selectedIndex);

				if (sortComboBox.SelectedIndex != (int)subject.Display.SortType)
				{
					if (sortComboBox.SelectedIndex != (int)SortType.Function)
					{
						if (sortComboBox.SelectedIndex == (int)SortType.NoSort)
						{
							subject.Display.SortType = SortType.NoSort;
							subject.Display.SortBoolean = null;
						}
						else if (sortComboBox.SelectedIndex == (int)SortType.True)
						{
							subject.Display.SortType = SortType.True;
							subject.Display.SortBoolean = true;
						}
						else if (sortComboBox.SelectedIndex == (int)SortType.False)
						{
							subject.Display.SortType = SortType.False;
							subject.Display.SortBoolean = false;
						}

						subject.Display.SortFunction = SortFunction.NoSortFunction;

						sortFunctionComboBox.Enabled = false;
						sortFunctionComboBox.SelectedIndex = (int)SortFunction.NoSortFunction;
					}
					else
					{
						subject.Display.SortType = SortType.Function;
						subject.Display.SortBoolean = null;

						sortFunctionComboBox.Enabled = true;
						sortFunctionComboBox.SelectedIndex = (int)subject.Display.SortFunction;
					}
				}

				sortComboBox.BackColor = subject.Display.SortType == original.Display.SortType ? default(Color) : ChangedValueColor;
				filterParametersListView.Items[selectedIndex].BackColor = subject.Equals(original) ? default(Color) : ChangedValueColor;
			}
			else if (sortComboBox.SelectedIndex != -1)
			{
				sortComboBox.SelectedIndex = -1;
				sortComboBox.BackColor = default(Color);
			}
		}

		private void sortFunctionComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				FilterParameter subject = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex);
				FilterParameter original = GetOriginalFilterParameter(selectedIndex);

				if (sortFunctionComboBox.SelectedIndex != (int)subject.Display.SortFunction)
					subject.Display.SortFunction = (SortFunction)sortFunctionComboBox.SelectedIndex;

				sortFunctionComboBox.BackColor = subject.Display.SortFunction == original.Display.SortFunction ? default(Color) : ChangedValueColor;
				filterParametersListView.Items[selectedIndex].BackColor = subject.Equals(original) ? default(Color) : ChangedValueColor;
			}
			else if (sortFunctionComboBox.SelectedIndex != -1)
			{
				sortFunctionComboBox.SelectedIndex = -1;
				sortFunctionComboBox.BackColor = default(Color);
			}
		}

		private void resultUnavailableComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				FilterParameter subject = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex);
				FilterParameter original = GetOriginalFilterParameter(selectedIndex);

				if (resultUnavailableComboBox.SelectedIndex != (int)subject.Display.ResultUnavailable)
					subject.Display.ResultUnavailable = (ResultUnavailable)resultUnavailableComboBox.SelectedIndex;

				resultUnavailableComboBox.BackColor = subject.Display.ResultUnavailable == original.Display.ResultUnavailable ? default(Color) : ChangedValueColor;
				filterParametersListView.Items[selectedIndex].BackColor = subject.Equals(original) ? default(Color) : ChangedValueColor;
			}
			else if (resultUnavailableComboBox.SelectedIndex != -1)
			{
				resultUnavailableComboBox.SelectedIndex = -1;
				resultUnavailableComboBox.BackColor = default(Color);
			}
		}

		private void quarterDateComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				FilterParameter subject = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex);
				FilterParameter original = GetOriginalFilterParameter(selectedIndex);

				if (quarterDateComboBox.SelectedIndex != (int)subject.Display.QuarterDate)
					subject.Display.QuarterDate = (QuarterDate)quarterDateComboBox.SelectedIndex;

				quarterDateComboBox.BackColor = subject.Display.QuarterDate == original.Display.QuarterDate ? default(Color) : ChangedValueColor;
				filterParametersListView.Items[selectedIndex].BackColor = subject.Equals(original) ? default(Color) : ChangedValueColor;
			}
			else if (quarterDateComboBox.SelectedIndex != -1)
			{
				quarterDateComboBox.SelectedIndex = -1;
				quarterDateComboBox.BackColor = default(Color);
			}
		}

		private void monthStepNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				FilterParameter subject = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex);
				FilterParameter original = GetOriginalFilterParameter(selectedIndex);
				int? value = monthStepNumericUpDown.Value == 0 ? null : (int?)monthStepNumericUpDown.Value;

				if (value != subject.Display.MonthStep)
					subject.Display.MonthStep = value;

				monthStepNumericUpDown.BackColor = subject.Display.MonthStep == original.Display.MonthStep ? default(Color) : ChangedValueColor;
				filterParametersListView.Items[selectedIndex].BackColor = subject.Equals(original) ? default(Color) : ChangedValueColor;
			}
			else if (monthStepNumericUpDown.Value != 0)
			{
				monthStepNumericUpDown.Value = 0;
				monthStepNumericUpDown.BackColor = default(Color);
			}
		}

		private void monthLimitNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				FilterParameter subject = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex);
				FilterParameter original = GetOriginalFilterParameter(selectedIndex);
				int? value = monthLimitNumericUpDown.Value == 0 && !allMonthsCheckBox.Checked ? null : (int?)monthLimitNumericUpDown.Value;

				if (value != subject.Display.MonthLimit)
					subject.Display.MonthLimit = value;

				monthLimitNumericUpDown.BackColor = subject.Display.MonthLimit == original.Display.MonthLimit ? default(Color) : ChangedValueColor;
				filterParametersListView.Items[selectedIndex].BackColor = subject.Equals(original) ? default(Color) : ChangedValueColor;
			}
			else if (monthLimitNumericUpDown.Value != 0)
			{
				monthLimitNumericUpDown.Value = 0;
				monthLimitNumericUpDown.BackColor = default(Color);
			}
		}

		private void allMonthsCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				FilterParameter subject = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex);
				FilterParameter original = GetOriginalFilterParameter(selectedIndex);
				int? value = monthLimitNumericUpDown.Value == 0 && !allMonthsCheckBox.Checked ? null : (int?)monthLimitNumericUpDown.Value;

				if (allMonthsCheckBox.Checked != (subject.Display.MonthLimit == 0))
				{
					subject.Display.MonthLimit = value;
					monthLimitNumericUpDown.Value = value ?? 0;
				}

				monthLimitNumericUpDown.Enabled = subject.Display.MonthLimit == 0;
				allMonthsCheckBox.BackColor = subject.Display.MonthLimit == 0 && original.Display.MonthLimit == 0 ? default(Color) : ChangedValueColor;
			}
		}

		private void multiCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				FilterParameter subject = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex);
				FilterParameter original = GetOriginalFilterParameter(selectedIndex);

				if (multiCheckBox.CheckState != ToCheckState(subject.Display.Multi))
					subject.Display.Multi = ToNullableBool(multiCheckBox.CheckState);

				multiCheckBox.BackColor = subject.Display.Multi == original.Display.Multi ? default(Color) : ChangedValueColor;
				filterParametersListView.Items[selectedIndex].BackColor = subject.Equals(original) ? default(Color) : ChangedValueColor;
			}
			else if (multiCheckBox.CheckState != CheckState.Indeterminate)
			{
				multiCheckBox.CheckState = CheckState.Indeterminate;
				multiCheckBox.BackColor = default(Color);
			}
		}

		private void zeroLastCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				FilterParameter subject = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex);
				FilterParameter original = GetOriginalFilterParameter(selectedIndex);

				if (zeroLastCheckBox.CheckState != ToCheckState(subject.Display.ZeroLast))
					subject.Display.ZeroLast = ToNullableBool(zeroLastCheckBox.CheckState);

				zeroLastCheckBox.BackColor = subject.Display.ZeroLast == original.Display.ZeroLast ? default(Color) : ChangedValueColor;
				filterParametersListView.Items[selectedIndex].BackColor = subject.Equals(original) ? default(Color) : ChangedValueColor;
			}
			else if (zeroLastCheckBox.CheckState != CheckState.Indeterminate)
			{
				zeroLastCheckBox.CheckState = CheckState.Indeterminate;
				zeroLastCheckBox.BackColor = default(Color);
			}
		}

		private void visibleCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				FilterParameter subject = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex);
				FilterParameter original = GetOriginalFilterParameter(selectedIndex);

				if (visibleCheckBox.CheckState != ToCheckState(subject.Display.Visible))
					subject.Display.Visible = ToNullableBool(visibleCheckBox.CheckState);

				visibleCheckBox.BackColor = subject.Display.Visible == original.Display.Visible ? default(Color) : ChangedValueColor;
				filterParametersListView.Items[selectedIndex].BackColor = subject.Equals(original) ? default(Color) : ChangedValueColor;
			}
			else if (visibleCheckBox.CheckState != CheckState.Indeterminate)
			{
				visibleCheckBox.CheckState = CheckState.Indeterminate;
				visibleCheckBox.BackColor = default(Color);
			}
		}

		private void valuesTypeButton_Click(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				FilterParameter subject = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex);

				if (subject.ValuesType != ValuesType.NoValues)
				{
					FilterParameter original = GetOriginalFilterParameter(selectedIndex);
					ValuesTypeDialog valuesTypeDialog = new ValuesTypeDialog(ref subject, ref original, ChangedValueColor);
					valuesTypeDialog.ShowDialog();

					SetValuesTypeButtonText(subject, original);
				}
				else
				{
					valuesTypeButton.Text = Enums.GetString(ValuesType.NoValues);
					valuesTypeButton.BackColor = default(Color);
				}
			}
			else
			{
				valuesTypeButton.Text = Enums.GetString(ValuesType.NoValues);
				valuesTypeButton.BackColor = default(Color);
			}
		}

		private void SetValuesTypeButtonText(FilterParameter subject, FilterParameter original)
		{
			valuesTypeButton.Text = Enums.GetString(subject.ValuesType);

			bool valuesTypesEqual = subject.ValuesType == original.ValuesType;
			bool selectedTypesEqual = subject.Display.SelectedType == original.Display.SelectedType;
			bool disabledTypesEqual = subject.Display.DisabledType == original.Display.DisabledType;

			valuesTypeButton.BackColor = valuesTypesEqual && selectedTypesEqual && disabledTypesEqual ? default(Color) : ChangedValueColor;
		}

		private void keysDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewCell selectedCell = keysDataGridView[e.ColumnIndex, e.RowIndex];
			int selectedIndex = filterParametersListView.SelectedIndices[0];
			FilterParameter subject = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex);
			Dictionary<string, KeysGridRow> keysGridRows = subject.KeysGridRows;
			string key = keysGridRows.ElementAt(e.RowIndex).Key;
			KeysGridRow original = GetOriginalFilterParameter(selectedIndex).KeysGridRows.FirstOrDefault(p => p.Key == key).Value ?? new KeysGridRow();

			if (e.ColumnIndex == keysKeyColumn.Index)
			{
				string cellValue = selectedCell.Value.ToString();
				KeysGridRow value = keysGridRows[key];

				if (value.IsFromTable == null && !keysGridRows.ContainsKey(cellValue))
				{
					Dictionary<string, KeysGridRow> newDictionary = new Dictionary<string, KeysGridRow>();

					for (int i = 0; i < keysGridRows.Count; i++)
						newDictionary.Add(i == e.RowIndex ? cellValue : keysGridRows.ElementAt(i).Key, keysGridRows.ElementAt(i).Value);

					subject.KeysGridRows = newDictionary;
				}
				else if (value.IsFromTable == true)
					selectedCell.Value = key;
				else if (value.IsFromTable == false)
				{
					if (keysGridRows.ContainsKey(cellValue) && keysGridRows.Keys.ToList().IndexOf(cellValue) != e.RowIndex)
					{
						MessageBox.Show("The key value is already present in the current set.", "Invalid Value",
							MessageBoxButtons.OK, MessageBoxIcon.Warning);
						selectedCell.Value = key;
					}
					else
					{
						int intValue;

						if (int.TryParse(cellValue, out intValue))
						{
							Dictionary<string, KeysGridRow> newDictionary = new Dictionary<string, KeysGridRow>();

							for (int i = 0; i < keysGridRows.Count; i++)
								newDictionary.Add(i == e.RowIndex ? intValue.ToString() : keysGridRows.ElementAt(i).Key, keysGridRows.ElementAt(i).Value);

							subject.KeysGridRows = newDictionary;
						}
						else
						{
							MessageBox.Show("The key value must be an integer when it is appended to values from the database.", "Invalid Value",
								MessageBoxButtons.OK, MessageBoxIcon.Warning);
							selectedCell.Value = key;
						}
					}
				}
				
				selectedCell.Style.BackColor = original == null ? ChangedValueColor : default(Color);
			}
			else if (e.ColumnIndex == keysValueColumn.Index && keysGridRows[key].Value != selectedCell.Value.ToString())
			{
				subject.KeysGridRows[key].Value = selectedCell.Value.ToString();
				subject.UpdatePropertiesFromKeysGridRows();
				selectedCell.Style.BackColor = subject.KeysGridRows[key].Value == original.Value ? default(Color) : ChangedValueColor;
			}
			else if (e.ColumnIndex == formatKeyColumn.Index && keysGridRows[key].Format != selectedCell.Value.ToString())
			{
				subject.KeysGridRows[key].Format = selectedCell.Value.ToString();
				subject.UpdatePropertiesFromKeysGridRows();
				selectedCell.Style.BackColor = subject.KeysGridRows[key].Format == original.Format ? default(Color) : ChangedValueColor;
			}
		}

		private void keysDataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			int columnIndex = keysDataGridView.CurrentCell.ColumnIndex;

			if (columnIndex == includeKeyColumn.Index || columnIndex == selectKeyColumn.Index || columnIndex == disableKeyColumn.Index)
				keysDataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}

		private void keysDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex != -1)
			{
				DataGridViewCell selectedCell = keysDataGridView[e.ColumnIndex, e.RowIndex];
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				FilterParameter subject = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex);
				string key = subject.KeysGridRows.ElementAt(e.RowIndex).Key;
				KeysGridRow original = GetOriginalFilterParameter(selectedIndex).KeysGridRows.FirstOrDefault(p => p.Key == key).Value ?? new KeysGridRow();

				if (e.ColumnIndex == includeKeyColumn.Index && subject.KeysGridRows[key].IsRemoved != !(bool)selectedCell.Value)
				{
					subject.KeysGridRows[key].IsRemoved = !(bool)selectedCell.Value;
					subject.UpdatePropertiesFromKeysGridRows();
					selectedCell.Style.BackColor = subject.KeysGridRows[key].IsRemoved == original.IsRemoved ? default(Color) : ChangedValueColor;
				}
				else if (e.ColumnIndex == selectKeyColumn.Index && subject.KeysGridRows[key].IsRemoved != (bool)selectedCell.Value)
				{
					subject.KeysGridRows[key].IsSelected = (bool)selectedCell.Value;
					subject.UpdatePropertiesFromKeysGridRows();
					selectedCell.Style.BackColor = subject.KeysGridRows[key].IsSelected == original.IsSelected ? default(Color) : ChangedValueColor;
				}
				else if (e.ColumnIndex == disableKeyColumn.Index && subject.KeysGridRows[key].IsDisabled != (bool)selectedCell.Value)
				{
					subject.KeysGridRows[key].IsDisabled = (bool)selectedCell.Value;
					subject.UpdatePropertiesFromKeysGridRows();
					selectedCell.Style.BackColor = subject.KeysGridRows[key].IsDisabled == original.IsDisabled ? default(Color) : ChangedValueColor;
				}
			}
		}

		private void helpDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			int selectedIndex = filterParametersListView.SelectedIndices[0];
			Dictionary<string, string> comments = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex).Display.Help;
			Dictionary<string, string> original = GetOriginalFilterParameter(selectedIndex).Display.Help;
			UpdateDictionaryItemInGrid(ref helpDataGridView, ref comments, original);
		}

		private FilterParameter GetOriginalFilterParameter(int selectedIndex)
		{
			FilterParameter original = new FilterParameter();
			FilterParameter subject = MyFilterParameterFile.FilterParameters.ElementAt(selectedIndex);
			int originalIndex = OriginalFilterParameterFile.FilterParameters.FindIndex(p => p.FilterParameterName == subject.FilterParameterName);

			if (originalIndex != -1)
				original = OriginalFilterParameterFile.FilterParameters.ElementAt(originalIndex);

			return original;
		}
	}
}
