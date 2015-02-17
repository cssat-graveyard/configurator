using Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PortalConfigurator
{
	public enum StringDictionary
	{
		Comments,
		Help
	}

	public partial class PortalConfiguratorForm : Form
	{
		private FilterParameterFile MyFilterParameterFile { get; set;}
		private FilterParameterFile OriginalFilterParameterFile { get; set; }
		private FilterParameter Subject { get; set; }
		private FilterParameter Original { get; set; }
		private int SubjectIndex { get; set; }
		private bool KeysGridNeedsRefresh { get; set; }
		private bool ScreenRefresh { get; set; }

		private void InitializeFilterParameterInterface()
		{
			MyFilterParameterFile = new FilterParameterFile();
			OriginalFilterParameterFile = MyFilterParameterFile.Clone();
			Subject = new FilterParameter();
			Original = new FilterParameter();
			SubjectIndex = -1;
			KeysGridNeedsRefresh = false;
			ScreenRefresh = false;
		}

		private void LoadFilterParameterInterface(object sender, EventArgs e)
		{
			filterParameterBreadcrumbLabel.Text = MyFilterParameterFile.Breadcrumb;
			filterParametersListView.Columns[0].Width = filterParametersListView.Width - SystemInformation.VerticalScrollBarWidth;
			filterParameterNameTextBox.Enabled = false;
			filterParameterTableNameComboBox.Items.AddRange(Tables.Keys.ToArray<string>());
			neitherRadioButton.Checked = true;
			typeGroupBox.Enabled = false;
			tableSettingsGroupBox.Enabled = false;
			commentsGroupBox.Enabled = false;
			displayTypeComboBox.Items.AddRange(Enums.GetFormattedDisplayTypeEnumNames());
			displayColumnComboBox.Items.AddRange(Enums.GetFormattedDisplayColumnEnumNames());
			sortTypeComboBox.Items.AddRange(Enums.GetFormattedSortTypeEnumNames());
			resultUnavailableComboBox.Items.AddRange(Enums.GetFormattedResultUnavailableEnumNames());
			quarterDateComboBox.Items.AddRange(Enums.GetFormattedQuarterDateEnumNames());
			valuesTypeErrorProvider.SetIconAlignment(valuesTypeLabel, ErrorIconAlignment.MiddleRight);
			valuesTypeErrorProvider.SetIconPadding(valuesTypeLabel, 2);
			displayGroupBox.Enabled = false;
		}

		private void newFilterParameterFileToolStripButton_Click(object sender, EventArgs e)
		{
			EndGridCellEditMode();
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
				valuesTypeErrorProvider.SetError(valuesTypeLabel, String.Empty);
			}
		}

		private void openFilterParameterFileToolStripButton_Click(object sender, EventArgs e)
		{
			EndGridCellEditMode();
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

				if (!String.IsNullOrEmpty(MyFilterParameterFile.FilePath))
				{
					FileInfo currentFile = new FileInfo(MyFilterParameterFile.FilePath);
					openFileDialog.InitialDirectory = currentFile.DirectoryName;
					openFileDialog.FileName = currentFile.Name;
				}
				else
				{
					openFileDialog.InitialDirectory = FilterParameterDirectory;
					openFileDialog.FileName = String.Empty;
				}

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					filterParametersListView.Items.Clear();
					filterParametersListView.SelectedIndices.Clear();
					ResetFilterParameterInterface();
					MyFilterParameterFile = new FilterParameterFile(openFileDialog.FileName);

					try
					{
						MyFilterParameterFile.ParseJson();
						OriginalFilterParameterFile = MyFilterParameterFile.Clone();
					}
					catch (JsonParseException parseException)
					{
						MyFilterParameterFile = new FilterParameterFile(openFileDialog.FileName);
						OriginalFilterParameterFile = new FilterParameterFile(openFileDialog.FileName);
						string message = String.Format("The following problem occurred while parsing the file:\n{0}", parseException.Message);
						MessageBox.Show(message, "Parse Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					catch (Exception exception)
					{
						MyFilterParameterFile = new FilterParameterFile(openFileDialog.FileName);
						OriginalFilterParameterFile = new FilterParameterFile(openFileDialog.FileName);
						string message = String.Format("The following problem occurred while opening the file:\n{0}", exception.Message);
						MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}

					filterParameterBreadcrumbLabel.Text = MyFilterParameterFile.Breadcrumb;

					foreach (var item in MyFilterParameterFile.FilterParameters)
						filterParametersListView.Items.Add(item.FilterParameterName);
				}
			}
		}

		private void saveFilterParameterFileToolStripButton_Click(object sender, EventArgs e)
		{
			EndGridCellEditMode();
			saveFileDialog.Title = "Save Filter/Parameter File";

			if (!String.IsNullOrEmpty(MyFilterParameterFile.FilePath))
			{
				FileInfo currentFile = new FileInfo(MyFilterParameterFile.FilePath);
				saveFileDialog.InitialDirectory = currentFile.DirectoryName;
				saveFileDialog.FileName = currentFile.Name;

				switch (CheckForNewerFile(currentFile, MyFilterParameterFile.FileDate))
				{
					case FileSaveConflictDecision.Overwrite:
						break;
					case FileSaveConflictDecision.Reload:
						openFilterParameterFileToolStripButton_Click(this, new EventArgs());
						return;
					case FileSaveConflictDecision.Cancel:
						return;
					default:
						break;
				}
			}
			else
			{
				saveFileDialog.InitialDirectory = FilterParameterDirectory;
				saveFileDialog.FileName = String.Empty;
			}

			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				MyFilterParameterFile.FilePath = saveFileDialog.FileName;

				try
				{
					MyFilterParameterFile.WriteFile();
					OriginalFilterParameterFile = MyFilterParameterFile.Clone();
					GetOriginalFilterParameter();
					filterParameterBreadcrumbLabel.Text = MyFilterParameterFile.Breadcrumb;

					foreach (ListViewItem item in filterParametersListView.Items)
						item.BackColor = default(Color);

					if (SubjectIndex != -1)
					{
						ScreenRefresh = true;
						filterParameterNameTextBox.BackColor = default(Color);
						SetGridCellsBackgroundColor(StringDictionary.Comments);
						valuesRadioButton.BackColor = default(Color);
						tableRadioButton.BackColor = default(Color);
						neitherRadioButton.BackColor = default(Color);
						dateCheckBox.BackColor = default(Color);
						filterParameterTableNameComboBox.BackColor = default(Color);
						orderByTextBox.BackColor = default(Color);
						valueColumnComboBox.BackColor = default(Color);
						lagColumnComboBox.BackColor = default(Color);
						legendTextBox.BackColor = default(Color);
						aliasTextBox.BackColor = default(Color);
						displayTypeComboBox.BackColor = default(Color);
						displayColumnComboBox.BackColor = default(Color);
						sortTypeComboBox.BackColor = default(Color);
						resultUnavailableComboBox.BackColor = default(Color);
						quarterDateComboBox.BackColor = default(Color);
						monthStepNumericUpDown.BackColor = default(Color);
						monthLimitNumericUpDown.BackColor = default(Color);
						monthLimitCheckBox.BackColor = default(Color);
						multiCheckBox.BackColor = default(Color);
						zeroLastCheckBox.BackColor = default(Color);
						visibleCheckBox.BackColor = default(Color);
						valuesTypeButton.BackColor = default(Color);
						valuesTypeErrorProvider.SetError(valuesTypeLabel, String.Empty);
						SetKeysGridCellsBackgroundColor();
						SetGridCellsBackgroundColor(StringDictionary.Help);
						ScreenRefresh = false;
					}
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
			if (SubjectIndex != -1)
			{
				EndGridCellEditMode();
				FilterParameter newFilterParameter = Subject.Clone();
				newFilterParameter.FilterParameterName += " - Copy";

				MyFilterParameterFile.FilterParameters.Add(newFilterParameter);
				filterParametersListView.Items.Add(newFilterParameter.FilterParameterName);
				filterParametersListView.SelectedIndices.Clear();
				filterParametersListView.SelectedIndices.Add(filterParametersListView.Items.Count - 1);
				SubjectIndex = filterParametersListView.SelectedIndices[0];
				filterParametersListView.EnsureVisible(SubjectIndex);
				filterParametersListView.Items[SubjectIndex].BackColor = ChangedValueColor;
				string message = Subject.GetPossibleValuesTypes().Contains(Subject.ValuesType) ? String.Empty :
					"The current format is invalid.\nPlease select an allowed format.";
				valuesTypeErrorProvider.SetError(valuesTypeLabel, message);
			}
		}

		private void addFilterParameterToolStripButton_Click(object sender, EventArgs e)
		{
			EndGridCellEditMode();
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
			SubjectIndex = filterParametersListView.SelectedIndices[0];
			filterParametersListView.EnsureVisible(SubjectIndex);
			filterParametersListView.Items[SubjectIndex].BackColor = ChangedValueColor;
			valuesTypeErrorProvider.SetError(valuesTypeLabel, String.Empty);
			filterParameterNameTextBox.Focus();
		}

		private void deleteFilterParameterToolStripButton_Click(object sender, EventArgs e)
		{
			if (SubjectIndex != -1)
			{
				EndGridCellEditMode();
				int selectedIndex = filterParametersListView.SelectedIndices[0];
				MyFilterParameterFile.FilterParameters.RemoveAt(selectedIndex);
				filterParametersListView.Items.RemoveAt(selectedIndex);
				filterParametersListView.SelectedIndices.Clear();

				if (filterParametersListView.Items.Count - 1 >= selectedIndex - 1 && selectedIndex - 1 >= 0)
				{
					filterParametersListView.SelectedIndices.Add(selectedIndex - 1);
					SubjectIndex = filterParametersListView.SelectedIndices[0];
					filterParametersListView.EnsureVisible(SubjectIndex);
					valuesTypeErrorProvider.SetError(valuesTypeLabel, String.Empty);
				}
				else if (filterParametersListView.Items.Count - 1 >= selectedIndex)
				{
					filterParametersListView.SelectedIndices.Add(selectedIndex);
					SubjectIndex = filterParametersListView.SelectedIndices[0];
					filterParametersListView.EnsureVisible(SubjectIndex);
					valuesTypeErrorProvider.SetError(valuesTypeLabel, String.Empty);
				}
				else
					ResetFilterParameterInterface();
			}
		}

		private void moveUpFilterParameterToolStripButton_Click(object sender, EventArgs e)
		{
			if (SubjectIndex > 0)
				MoveFilterParameterInList(SubjectIndex, SubjectIndex - 1, Subject.Clone());
		}

		private void moveDownFilterParameterToolStripButton_Click(object sender, EventArgs e)
		{
			if (SubjectIndex < filterParametersListView.Items.Count - 1)
				MoveFilterParameterInList(SubjectIndex, SubjectIndex + 1, Subject.Clone());
		}

		private void MoveFilterParameterInList(int selectedIndex, int destinationIndex, FilterParameter selectedItem)
		{
			EndGridCellEditMode();
			Color backColor = filterParametersListView.Items[selectedIndex].BackColor;

			MyFilterParameterFile.FilterParameters.RemoveAt(selectedIndex);
			MyFilterParameterFile.FilterParameters.Insert(destinationIndex, selectedItem);

			filterParametersListView.SelectedIndices.Clear();
			filterParametersListView.Items.RemoveAt(selectedIndex);
			filterParametersListView.Items.Insert(destinationIndex, selectedItem.FilterParameterName);
			filterParametersListView.Items[destinationIndex].BackColor = backColor;
			filterParametersListView.SelectedIndices.Add(destinationIndex);
			SubjectIndex = filterParametersListView.SelectedIndices[0];
			filterParametersListView.EnsureVisible(SubjectIndex);
		}

		private void filterParametersListView_Resize(object sender, EventArgs e)
		{
			filterParametersListView.Columns[0].Width = filterParametersListView.Width;
		}

		private void filterParametersListView_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (filterParametersListView.SelectedIndices.Count != 0)
			{
				filterParameterNameTextBox.Enabled = true;
				typeGroupBox.Enabled = true;
				commentsGroupBox.Enabled = true;
				displayGroupBox.Enabled = true;
				SubjectIndex = filterParametersListView.SelectedIndices[0];
				Subject = MyFilterParameterFile.FilterParameters[SubjectIndex];
				GetOriginalFilterParameter();
				KeysGridNeedsRefresh = true;
				ScreenRefresh = true;

				filterParameterNameTextBox.Text = Subject.FilterParameterName;
				dateCheckBox.CheckState = ToCheckState(Subject.Date);

				commentsDataGridView.Rows.Clear();
				if (Subject.Comments.Count != 0)
				{
					commentsDataGridView.Rows.Add(Subject.Comments.Count);

					for (int i = 0; i < Subject.Comments.Count; i++)
					{
						commentsDataGridView[0, i].Value = Subject.Comments.ElementAt(i).Key;
						commentsDataGridView[1, i].Value = Subject.Comments.ElementAt(i).Value;
					}

					SetGridCellsBackgroundColor(StringDictionary.Comments);
				}

				legendTextBox.Text = Subject.Display.Legend;
				aliasTextBox.Text = Subject.Display.Alias;
				displayTypeComboBox.SelectedIndex = (int)Subject.Display.DisplayType;
				displayColumnComboBox.SelectedIndex = (int)Subject.Display.DisplayColumn;
				sortTypeComboBox.SelectedIndex = (int)Subject.Display.SortType;
				resultUnavailableComboBox.SelectedIndex = (int)Subject.Display.ResultUnavailable;
				quarterDateComboBox.SelectedIndex = (int)Subject.Display.QuarterDate;
				monthStepNumericUpDown.Value = Subject.Display.MonthStep ?? 0;
				monthStepNumericUpDown.Enabled = true;
				monthLimitCheckBox.Checked = Subject.Display.MonthLimit != null;
				monthLimitNumericUpDown.Value = Subject.Display.MonthLimit ?? 0;
				monthLimitNumericUpDown.Enabled = Subject.Display.MonthLimit != null;
				multiCheckBox.CheckState = ToCheckState(Subject.Display.Multi);
				multiCheckBox.Enabled = Subject.Display.DisplayType != DisplayType.Parameter;
				zeroLastCheckBox.CheckState = ToCheckState(Subject.Display.ZeroLast);
				visibleCheckBox.CheckState = ToCheckState(Subject.Display.Visible);

				if (Subject.Type == FilterParameterType.Values)
					valuesRadioButton.Checked = true;
				else if (Subject.Type == FilterParameterType.Table)
					tableRadioButton.Checked = true;
				else if (Subject.Type == FilterParameterType.Neither)
					neitherRadioButton.Checked = true;

				if (KeysGridNeedsRefresh)
					FilterParameterTypeSelectionChanged();

				helpDataGridView.Rows.Clear();
				if (Subject.Display.Help.Count != 0)
				{
					helpDataGridView.Rows.Add(Subject.Display.Help.Count);

					for (int i = 0; i < Subject.Display.Help.Count; i++)
					{
						helpDataGridView[0, i].Value = Subject.Display.Help.ElementAt(i).Key;
						helpDataGridView[1, i].Value = Subject.Display.Help.ElementAt(i).Value;
					}

					SetGridCellsBackgroundColor(StringDictionary.Help);
				}
			}
			else
				ResetFilterParameterInterface();

			ScreenRefresh = false;
		}

		private void ResetFilterParameterInterface()
		{
			SubjectIndex = -1;
			Subject = new FilterParameter();
			Original = new FilterParameter();
			filterParameterNameTextBox.Clear();
			filterParameterNameTextBox.BackColor = default(Color);
			filterParameterNameTextBox.Enabled = false;
			commentsDataGridView.Rows.Clear();
			commentsGroupBox.Enabled = false;
			valuesRadioButton.BackColor = default(Color);
			tableRadioButton.BackColor = default(Color);
			neitherRadioButton.Checked = true;
			neitherRadioButton.BackColor = default(Color);
			dateCheckBox.CheckState = CheckState.Indeterminate;
			dateCheckBox.BackColor = default(Color);
			typeGroupBox.Enabled = false;
			filterParameterTableNameComboBox.SelectedIndex = -1;
			filterParameterTableNameComboBox.BackColor = default(Color);
			orderByTextBox.Clear();
			orderByTextBox.BackColor = default(Color);
			valueColumnComboBox.SelectedIndex = -1;
			valueColumnComboBox.Items.Clear();
			valueColumnComboBox.BackColor = default(Color);
			lagColumnComboBox.SelectedIndex = -1;
			lagColumnComboBox.Items.Clear();
			lagColumnComboBox.BackColor = default(Color);
			legendTextBox.Clear();
			legendTextBox.BackColor = default(Color);
			aliasTextBox.Clear();
			aliasTextBox.BackColor = default(Color);
			displayTypeComboBox.SelectedIndex = -1;
			displayTypeComboBox.BackColor = default(Color);
			displayColumnComboBox.SelectedIndex = -1;
			displayColumnComboBox.BackColor = default(Color);
			sortTypeComboBox.SelectedIndex = -1;
			sortTypeComboBox.BackColor = default(Color);
			resultUnavailableComboBox.SelectedIndex = -1;
			resultUnavailableComboBox.BackColor = default(Color);
			quarterDateComboBox.SelectedIndex = -1;
			quarterDateComboBox.BackColor = default(Color);
			monthStepNumericUpDown.Value = 0;
			monthStepNumericUpDown.Enabled = false;
			monthStepNumericUpDown.BackColor = default(Color);
			monthLimitCheckBox.Checked = false;
			monthLimitNumericUpDown.Value = 0;
			monthLimitNumericUpDown.Enabled = false;
			monthLimitNumericUpDown.BackColor = default(Color);
			multiCheckBox.CheckState = CheckState.Indeterminate;
			multiCheckBox.Enabled = false;
			multiCheckBox.BackColor = default(Color);
			zeroLastCheckBox.CheckState = CheckState.Indeterminate;
			zeroLastCheckBox.BackColor = default(Color);
			visibleCheckBox.CheckState = CheckState.Indeterminate;
			visibleCheckBox.BackColor = default(Color);
			valuesTypeButton.Text = Enums.GetFormattedString(ValuesType.NoValues);
			valuesTypeButton.BackColor = default(Color);
			valuesTypeErrorProvider.SetError(valuesTypeLabel, String.Empty);
			valuesDataGridView.Rows.Clear();
			helpDataGridView.Rows.Clear();
			displayGroupBox.Enabled = false;
		}

		private void SetListViewItemBackgroundColor(bool changeDetected, int selectedIndex)
		{
			if (!ScreenRefresh && selectedIndex != -1)
			{
				Color currentBackColor = filterParametersListView.Items[selectedIndex].BackColor;

				if (changeDetected && currentBackColor != ChangedValueColor)
					filterParametersListView.Items[selectedIndex].BackColor = ChangedValueColor;
				else if (!changeDetected && currentBackColor == ChangedValueColor)
				{
					if (selectedIndex == SubjectIndex)
						filterParametersListView.Items[selectedIndex].BackColor = Subject.Equals(Original) ? default(Color) : ChangedValueColor;
					else
					{
						FilterParameter subject = MyFilterParameterFile.FilterParameters[selectedIndex];
						FilterParameter original = GetOriginalFilterParameter(subject);
						filterParametersListView.Items[selectedIndex].BackColor = subject.Equals(original) ? default(Color) : ChangedValueColor;
					}
				}
			}
		}

		private void addCommentButton_Click(object sender, EventArgs e)
		{
			if (SubjectIndex != -1)
				AddDictionaryItemToGrid(StringDictionary.Comments);
		}

		private void deleteCommentButton_Click(object sender, EventArgs e)
		{
			if (SubjectIndex != -1)
				DeleteDictionaryItemFromGrid(StringDictionary.Comments);
		}

		private void moveUpCommentButton_Click(object sender, EventArgs e)
		{
			int startingIndex = commentsDataGridView.CurrentCell.RowIndex;

			if (startingIndex > 0 && filterParametersListView.SelectedIndices.Count != 0)
				MoveDictionaryItemInGrid(startingIndex, startingIndex - 1, StringDictionary.Comments);
		}

		private void moveDownCommentButton_Click(object sender, EventArgs e)
		{
			int startingIndex = commentsDataGridView.CurrentCell.RowIndex;

			if (startingIndex < commentsDataGridView.RowCount - 1 && filterParametersListView.SelectedIndices.Count != 0)
				MoveDictionaryItemInGrid(startingIndex, startingIndex + 1, StringDictionary.Comments);
		}

		private void AddDictionaryItemToGrid(StringDictionary source)
		{
			EndGridCellEditMode();
			DataGridView subjectGrid = new DataGridView();
			Dictionary<string, string> subjectDictionary = new Dictionary<string,string>();
			int keyColumnIndex = 0;
			int valueColumnIndex = 1;

			if (source == StringDictionary.Comments)
			{
				subjectGrid = commentsDataGridView;
				subjectDictionary = Subject.Comments;
				keyColumnIndex = commentName.Index;
				valueColumnIndex = commentContent.Index;
			}
			else if (source == StringDictionary.Help)
			{
				subjectGrid = helpDataGridView;
				subjectDictionary = Subject.Display.Help;
				keyColumnIndex = helpName.Index;
				valueColumnIndex = helpContent.Index;
			}

			int newItemNumber = subjectDictionary.Count;
			string newKey = String.Empty;
			string baseKey = String.Empty;

			if (subjectGrid.Equals(commentsDataGridView))
				baseKey = "_comment";
			else if (subjectGrid.Equals(helpDataGridView))
				baseKey = subjectDictionary.Count == 0 ? "default" : "help";

			do
			{
				newKey = String.Format("{0}{1}", baseKey, newItemNumber == 0 ? String.Empty : newItemNumber.ToString());
				newItemNumber++;
			} while (subjectDictionary.ContainsKey(newKey));

			int newRowIndex = subjectGrid.RowCount;
			subjectGrid.Rows.Add();
			subjectGrid[keyColumnIndex, newRowIndex].Value = newKey;
			subjectGrid[valueColumnIndex, newRowIndex].Value = String.Empty;

			subjectDictionary.Add(newKey, String.Empty);

			SetGridCellsBackgroundColor(source, newRowIndex);

			if (subjectGrid.RowCount == 1)
				subjectGrid.CurrentCell = subjectGrid[valueColumnIndex, newRowIndex];
			else if (subjectGrid.RowCount > 1)
				subjectGrid.CurrentCell = subjectGrid[keyColumnIndex, newRowIndex];

			subjectGrid.BeginEdit(true);
		}

		private void DeleteDictionaryItemFromGrid(StringDictionary source)
		{
			EndGridCellEditMode();
			DataGridView subjectGrid = new DataGridView();
			Dictionary<string, string> subjectDictionary = new Dictionary<string, string>();

			if (source == StringDictionary.Comments)
			{
				subjectGrid = commentsDataGridView;
				subjectDictionary = Subject.Comments;
			}
			else if (source == StringDictionary.Help)
			{
				subjectGrid = helpDataGridView;
				subjectDictionary = Subject.Display.Help;
			}

			if (subjectGrid.RowCount != 0)
			{
				int selectedIndex = subjectGrid.CurrentCell.RowIndex;
				Dictionary<string, string> newDictionary = new Dictionary<string, string>();

				subjectGrid.Rows.RemoveAt(selectedIndex);

				for (int i = 0; i < subjectDictionary.Count; i++)
					if (i != selectedIndex)
						newDictionary.Add(subjectDictionary.Keys.ElementAt(i), subjectDictionary.Values.ElementAt(i));

				subjectDictionary.Clear();

				foreach (var item in newDictionary)
					subjectDictionary.Add(item.Key, item.Value);
			}
		}

		private void MoveDictionaryItemInGrid(int startingRowIndex, int destinationRowIndex, StringDictionary source)
		{
			EndGridCellEditMode();
			DataGridView subjectGrid = new DataGridView();
			int keyColumnIndex = 0;
			int valueColumnIndex = 1;
			Dictionary<string, string> subjectDictionary = new Dictionary<string, string>();
			Dictionary<string, string> originalDictionar = new Dictionary<string, string>();
			Dictionary<string, string> newDictionary = new Dictionary<string, string>();
			int i = 0;

			if (source == StringDictionary.Comments)
			{
				subjectGrid = commentsDataGridView;
				keyColumnIndex = commentName.Index;
				valueColumnIndex = commentContent.Index;
			}
			else if (source == StringDictionary.Help)
			{
				subjectGrid = helpDataGridView;
				keyColumnIndex = helpName.Index;
				valueColumnIndex = helpContent.Index;
			}

			string key = subjectGrid[keyColumnIndex, startingRowIndex].Value.ToString();
			string value = subjectGrid[valueColumnIndex, startingRowIndex].Value.ToString();

			subjectGrid.Rows.RemoveAt(startingRowIndex);
			subjectGrid.Rows.Insert(destinationRowIndex, 1);
			subjectGrid[keyColumnIndex, destinationRowIndex].Value = key;
			subjectGrid[valueColumnIndex, destinationRowIndex].Value = value;
			subjectGrid.CurrentCell = subjectGrid[subjectGrid.CurrentCell.ColumnIndex, destinationRowIndex];

			do
			{
				if (newDictionary.Count == destinationRowIndex)
					newDictionary.Add(key, value);

				if (i != startingRowIndex && i < subjectDictionary.Count)
					newDictionary.Add(subjectDictionary.Keys.ElementAt(i), subjectDictionary.Values.ElementAt(i));

				i++;
			} while (newDictionary.Count != subjectDictionary.Count);

			subjectDictionary.Clear();
			
			foreach (var item in newDictionary)
				subjectDictionary.Add(item.Key, item.Value);

			SetGridCellsBackgroundColor(source, destinationRowIndex);
		}

		private void SetGridCellsBackgroundColor(StringDictionary source)
		{
			DataGridView subjectGrid = new DataGridView();
			bool changeDetected = false;
			bool insideScreenRefresh = ScreenRefresh;
			ScreenRefresh = true;

			if (source == StringDictionary.Comments)
				subjectGrid = commentsDataGridView;
			else if (source == StringDictionary.Help)
				subjectGrid = helpDataGridView;

			for (int i = 0; i < subjectGrid.Rows.Count; i++)
			{
				if (SetGridCellsBackgroundColor(source, i) && !changeDetected)
					changeDetected = true;
			}

			ScreenRefresh = insideScreenRefresh;
			SetListViewItemBackgroundColor(changeDetected, SubjectIndex);
		}

		private bool SetGridCellsBackgroundColor(StringDictionary source, int row)
		{
			string key = String.Empty;
			string value = String.Empty;
			string original = String.Empty;
			bool changeDetected = false;
			bool originalExists = false;
			DataGridView subjectGrid = new DataGridView();
			int keyColumnIndex = 0;
			int valueColumnIndex = 1;

			if (source == StringDictionary.Comments)
			{
				subjectGrid = commentsDataGridView;
				keyColumnIndex = commentName.Index;
				valueColumnIndex = commentContent.Index;
				key = Subject.Comments.ElementAt(row).Key;
				value = Subject.Comments.ElementAt(row).Value;
				originalExists = Original.Comments.ContainsKey(key);
				original = originalExists ? Original.Comments[key] : String.Empty;
			}
			else if (source == StringDictionary.Help)
			{
				subjectGrid = helpDataGridView;
				keyColumnIndex = helpName.Index;
				valueColumnIndex = helpContent.Index;
				key = Subject.Display.Help.ElementAt(row).Key;
				value = Subject.Display.Help.ElementAt(row).Value;
				originalExists = Original.Display.Help.ContainsKey(key);
				original = originalExists ? Original.Display.Help[key] : string.Empty;
			}

			subjectGrid[keyColumnIndex, row].Style.BackColor = !originalExists ? ChangedValueColor : default(Color);
			subjectGrid[valueColumnIndex, row].Style.BackColor = !originalExists || value != original ? ChangedValueColor : default(Color);

			SetListViewItemBackgroundColor(changeDetected, SubjectIndex);
			return changeDetected;
		}

		private void filterParameterNameTextBox_TextChanged(object sender, EventArgs e)
		{
			if (SubjectIndex != -1 && filterParameterNameTextBox.Text != Subject.FilterParameterName)
			{
				Subject.FilterParameterName = filterParameterNameTextBox.Text;
				filterParametersListView.Items[SubjectIndex].Text = Subject.FilterParameterName;
			}

			bool changeDetected = Subject.FilterParameterName != Original.FilterParameterName;
			filterParameterNameTextBox.BackColor = changeDetected ? ChangedValueColor : default(Color);
			SetListViewItemBackgroundColor(changeDetected, SubjectIndex);
		}

		private void valuesRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (SubjectIndex != -1)
			{
				if (valuesRadioButton.Checked && Subject.Type != FilterParameterType.Values)
					FilterParameterTypeSelectionChanged();
			}
			else
				valuesRadioButton.BackColor = default(Color);
		}

		private void tableRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (SubjectIndex != -1)
			{
				if (tableRadioButton.Checked && Subject.Type != FilterParameterType.Table)
					FilterParameterTypeSelectionChanged();
			}
			else
				tableRadioButton.BackColor = default(Color);
		}

		private void neitherRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (SubjectIndex != -1)
			{
				if (neitherRadioButton.Checked && Subject.Type != FilterParameterType.Neither)
					FilterParameterTypeSelectionChanged();
			}
			else
				neitherRadioButton.BackColor = default(Color);
		}

		private void FilterParameterTypeSelectionChanged()
		{
			Display display = Subject.Display;
			bool enableTableSettingsGroupBox = false;
			bool enableValueColumnComboBox = false;
			bool enableLagColumnComboBox = false;
			bool enableValuesTypeButton = false;
			bool changeDetected = false;
			bool alreadyScreenRefresh = ScreenRefresh;
			ScreenRefresh = true;

			FilterParameterType chosenType;

			if (valuesRadioButton.Checked)
				chosenType = FilterParameterType.Values;
			else if (tableRadioButton.Checked)
				chosenType = FilterParameterType.Table;
			else
				chosenType = FilterParameterType.Neither;

			if (chosenType != Subject.Type && Subject.Type != FilterParameterType.Neither)
			{
				string message = "By changing the type, you will lose the current settings.\n\nDo you wish to continue?";

				if (MessageBox.Show(message, "Change Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
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
				if (valuesRadioButton.Checked)
				{
					Subject.TableColumns.Clear();
					Subject.TableDataAreLoaded = false;

					if (Subject.Type == FilterParameterType.Table)
					{
						filterParameterTableNameComboBox.SelectedIndex = -1;
						orderByTextBox.Text = String.Empty;
						valueColumnComboBox.SelectedIndex = -1;
						lagColumnComboBox.SelectedIndex = -1;
					}

					enableValuesTypeButton = true;
					SetValuesTypeButtonText();
				}
				else if (tableRadioButton.Checked)
				{
					enableTableSettingsGroupBox = true;
					enableTableSettingsGroupBox = true;

					if (Tables.ContainsKey(Subject.Table))
					{
						filterParameterTableNameComboBox.SelectedIndex = Tables.Keys.ToList<string>().FindIndex(p => p == Subject.Table);
						orderByTextBox.Text = Subject.OrderBy;
						valueColumnComboBox.Items.Clear();
						valueColumnComboBox.Items.AddRange(Subject.TableColumns.Values.ToArray());

						if ((Subject.ValueColumn ?? 1) < Subject.TableColumns.Count)
							valueColumnComboBox.SelectedIndex = Subject.ValueColumn ?? 1;
						else
						{
							string message = String.Format("The value column at position {0} was not in the table.", Subject.ValueColumn ?? 1);
							Subject.ValueColumn = null;
							valueColumnComboBox.Enabled = false;
							valueColumnComboBox.BackColor = Subject.ValueColumn == Original.ValueColumn ? default(Color) : ChangedValueColor;
							MessageBox.Show(message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						}
						
						lagColumnComboBox.Items.Clear();
						lagColumnComboBox.Items.Add(String.Empty);
						lagColumnComboBox.Items.AddRange(Subject.TableColumns.Values.ToArray());

						if ((Subject.LagColumn ?? -1) < Subject.TableColumns.Count)
							lagColumnComboBox.SelectedIndex = (Subject.LagColumn ?? -2) + 1;
						else
						{
							string message = String.Format("The lag column at position {0} was not in the table.", Subject.LagColumn ?? 0);
							Subject.LagColumn = null;
							lagColumnComboBox.Enabled = false;
							lagColumnComboBox.BackColor = Subject.LagColumn == Original.LagColumn ? default(Color) : ChangedValueColor;
							MessageBox.Show(message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						}
					}
					else if (!String.IsNullOrEmpty(Subject.Table))
					{
						string message = String.Format("The \"{0}\" table was not found in the database.", Subject.Table);
						Subject.Table = String.Empty;
						filterParameterTableNameComboBox.BackColor = Subject.Table == Original.Table ? default(Color) : ChangedValueColor;
						orderByTextBox.BackColor = Subject.OrderBy == Original.OrderBy ? default(Color) : ChangedValueColor;
						valueColumnComboBox.Enabled = false;
						valueColumnComboBox.BackColor = Subject.ValueColumn == Original.ValueColumn ? default(Color) : ChangedValueColor;
						lagColumnComboBox.Enabled = false;
						lagColumnComboBox.BackColor = Subject.LagColumn == Original.LagColumn ? default(Color) : ChangedValueColor;
						MessageBox.Show(message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}

					enableValuesTypeButton = false;
					SetValuesTypeButtonText();
					enableValueColumnComboBox = true;
					enableLagColumnComboBox = true;
				}
				else if (neitherRadioButton.Checked)
				{
					Subject.ValuesGridRows.Clear();
					Subject.TableDataAreLoaded = false;
					
					if (Subject.Type == FilterParameterType.Table)
					{
						filterParameterTableNameComboBox.SelectedIndex = -1;
						orderByTextBox.Text = String.Empty;
						valueColumnComboBox.SelectedIndex = -1;
						valueColumnComboBox.Items.Clear();
						lagColumnComboBox.SelectedIndex = -1;
						lagColumnComboBox.Items.Clear();
					}
					
					enableValuesTypeButton = false;
					SetValuesTypeButtonText();
				}

				RefreshKeysDataGridView();
				Subject.Type = chosenType;
			}
			catch (TableNotFoundException e)
			{
				enableTableSettingsGroupBox = true;
				enableValueColumnComboBox = false;
				enableLagColumnComboBox = false;
				enableValuesTypeButton = false;
				valuesTypeButton.Text = Enums.GetFormattedString(ValuesType.NoValues);
				valuesTypeButton.BackColor = default(Color);
				valuesDataGridView.Rows.Clear();

				MessageBox.Show(e.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (ColumnNotFoundException e)
			{
				enableTableSettingsGroupBox = true;
				enableValueColumnComboBox = true;
				enableLagColumnComboBox = true;
				enableValuesTypeButton = false;
				valuesTypeButton.Text = Enums.GetFormattedString(ValuesType.NoValues);
				valuesTypeButton.BackColor = default(Color);
				valuesDataGridView.Rows.Clear();

				MessageBox.Show(e.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (Exception e)
			{
				enableTableSettingsGroupBox = true;
				enableValueColumnComboBox = false;
				valueColumnComboBox.SelectedIndex = -1;

				if (valueColumnComboBox.SelectedIndex != (Subject.ValueColumn ?? 1))
					valueColumnComboBox_SelectedIndexChanged(valueColumnComboBox, new EventArgs());
				
				enableLagColumnComboBox = false;
				lagColumnComboBox.SelectedIndex = -1;

				if (lagColumnComboBox.SelectedIndex != (Subject.LagColumn ?? 0))
					lagColumnComboBox_SelectedIndexChanged(valueColumnComboBox, new EventArgs());

				enableValuesTypeButton = false;
				valuesTypeButton.Text = Enums.GetFormattedString(ValuesType.NoValues);
				valuesTypeButton.BackColor = default(Color);
				valuesDataGridView.Rows.Clear();

				MessageBox.Show(e.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			changeDetected = Subject.Type != Original.Type;
			valuesRadioButton.BackColor = Subject.Type == FilterParameterType.Values && changeDetected ? ChangedValueColor : default(Color);
			tableRadioButton.BackColor = Subject.Type == FilterParameterType.Table && changeDetected ? ChangedValueColor : default(Color);
			neitherRadioButton.BackColor = Subject.Type == FilterParameterType.Neither && changeDetected ? ChangedValueColor : default(Color);

			KeysGridNeedsRefresh = false;
			Subject.UpdatePropertiesFromKeysGridRows();
			ScreenRefresh = alreadyScreenRefresh;
			SetListViewItemBackgroundColor(changeDetected, SubjectIndex);
			tableSettingsGroupBox.Enabled = enableTableSettingsGroupBox;
			valueColumnComboBox.Enabled = enableValueColumnComboBox;
			lagColumnComboBox.Enabled = enableLagColumnComboBox;
			valuesTypeButton.Enabled = enableValuesTypeButton;
		}

		private void dateCheckBox_CheckStateChanged(object sender, EventArgs e)
		{
			if (SubjectIndex != -1)
			{
				if (ToNullableBool(dateCheckBox.CheckState) != Subject.Date)
					Subject.Date = ToNullableBool(dateCheckBox.CheckState);

				filterParametersListView.Items[SubjectIndex].BackColor = Subject.Equals(Original) ? default(Color) : ChangedValueColor;
			}

			bool changeDetected = Subject.Date != Original.Date;
			dateCheckBox.BackColor = changeDetected ? ChangedValueColor : default(Color);
			SetListViewItemBackgroundColor(changeDetected, SubjectIndex);
		}

		private void filterParameterTableNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (SubjectIndex != -1)
			{
				if ((filterParameterTableNameComboBox.SelectedItem ?? (object)String.Empty).ToString() != Subject.Table)
				{
					Subject.Table = (filterParameterTableNameComboBox.SelectedItem ?? (object)String.Empty).ToString();

					ScreenRefresh = true;
					orderByTextBox.Text = Subject.OrderBy;
					orderByTextBox.Enabled = !String.IsNullOrEmpty(Subject.Table);

					if (String.IsNullOrEmpty(Subject.Table))
					{
						valueColumnComboBox.SelectedIndex = -1;
						lagColumnComboBox.SelectedIndex = -1;
						valueColumnComboBox.Items.Clear();
						lagColumnComboBox.Items.Clear();
						valuesDataGridView.Rows.Clear();
					}
					else
					{
						valueColumnComboBox.Items.Clear();
						valueColumnComboBox.Items.AddRange(Subject.TableColumns.Values.ToArray<string>());
						valueColumnComboBox.SelectedIndex = Subject.ValueColumn ?? 1;
						lagColumnComboBox.Items.Clear();
						lagColumnComboBox.Items.Add(String.Empty);
						lagColumnComboBox.Items.AddRange(Subject.TableColumns.Values.ToArray<string>());
						lagColumnComboBox.SelectedIndex = Subject.LagColumn ?? -1;
						RefreshKeysDataGridView();
					}

					valueColumnComboBox.Enabled = !String.IsNullOrEmpty(Subject.Table);
					valueColumnComboBox.Enabled = !String.IsNullOrEmpty(Subject.Table);
					ScreenRefresh = false;
				}
			}

			bool changeDetected = Subject.Table != Original.Table;
			filterParameterTableNameComboBox.BackColor = changeDetected ? ChangedValueColor : default(Color);
			SetListViewItemBackgroundColor(changeDetected, SubjectIndex);
		}

		private void orderByTextBox_TextChanged(object sender, EventArgs e)
		{
			if (SubjectIndex != -1)
			{
				if (orderByTextBox.Text != Subject.OrderBy)
					Subject.OrderBy = orderByTextBox.Text;
			}

			bool changeDetected = Subject.OrderBy != Original.OrderBy;
			orderByTextBox.BackColor = changeDetected ? ChangedValueColor : default(Color);
			SetListViewItemBackgroundColor(changeDetected, SubjectIndex);
		}

		private void valueColumnComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (SubjectIndex != -1)
			{
				int? valueColumnIndex = valueColumnComboBox.SelectedIndex == -1 || valueColumnComboBox.SelectedIndex == 1 ? null : (int?)valueColumnComboBox.SelectedIndex;

				if (valueColumnIndex != Subject.ValueColumn)
				{
					Subject.ValueColumn = valueColumnIndex;
					ScreenRefresh = true;
					RefreshKeysDataGridView();
					ScreenRefresh = false;
				}
			}

			bool changeDetected = Subject.ValueColumn != Original.ValueColumn;
			valueColumnComboBox.BackColor = changeDetected ? ChangedValueColor : default(Color);
			SetListViewItemBackgroundColor(changeDetected, SubjectIndex);
		}

		private void lagColumnComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (SubjectIndex != -1)
			{
				int? lagColumnIndex = lagColumnComboBox.SelectedIndex == -1 || lagColumnComboBox.SelectedIndex == 0 ? null : (int?)lagColumnComboBox.SelectedIndex - 1;

				if (lagColumnIndex != Subject.LagColumn)
					Subject.LagColumn = lagColumnIndex;
			}

			bool changeDetected = Subject.LagColumn != Original.LagColumn;
			lagColumnComboBox.BackColor = changeDetected ? ChangedValueColor : default(Color);
			SetListViewItemBackgroundColor(changeDetected, SubjectIndex);
		}

		private void commentsDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex != -1 ? commentsDataGridView.CurrentCell.IsInEditMode && commentsDataGridView.CurrentCell.ColumnIndex == e.ColumnIndex : false)
				UpdateDictionaryItemInGrid(StringDictionary.Comments);
		}

		private void UpdateDictionaryItemInGrid(StringDictionary source)
		{
			DataGridView subjectGrid = new DataGridView();
			Dictionary<string, string> subjectDictionary = new Dictionary<string, string>();
			int keyColumn = 0;
			int valueColumn = 1;

			if (source == StringDictionary.Comments)
			{
				subjectGrid = commentsDataGridView;
				subjectDictionary = Subject.Comments;
				keyColumn = commentName.Index;
				valueColumn = commentContent.Index;
			}
			else if (source == StringDictionary.Help)
			{
				subjectGrid = helpDataGridView;
				subjectDictionary = Subject.Display.Help;
				keyColumn = helpName.Index;
				valueColumn = helpContent.Index;
			}

			int row = subjectGrid.CurrentCell.RowIndex;
			string cellValue = (subjectGrid.CurrentCell.Value ?? (object)String.Empty).ToString();
			string key = subjectDictionary.ElementAt(row).Key;

			if (subjectGrid.CurrentCell.ColumnIndex == keyColumn)
			{
				if (source == StringDictionary.Comments)
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
			else if (subjectGrid.CurrentCell.ColumnIndex == valueColumn)
			{
				if (cellValue != subjectDictionary[key])
					subjectDictionary[key] = cellValue;
			}

			if (subjectGrid.CurrentCell.IsInEditMode)
				subjectGrid.EndEdit(DataGridViewDataErrorContexts.Commit);

			SetGridCellsBackgroundColor(source, row);
		}

		private void legendTextBox_TextChanged(object sender, EventArgs e)
		{
			if (SubjectIndex != -1)
			{
				if (legendTextBox.Text != Subject.Display.Legend)
					Subject.Display.Legend = legendTextBox.Text;
			}

			bool changeDetected = Subject.Display.Legend != Original.Display.Legend;
			legendTextBox.BackColor = changeDetected ? ChangedValueColor : default(Color);
			SetListViewItemBackgroundColor(changeDetected, SubjectIndex);
		}

		private void aliasTextBox_TextChanged(object sender, EventArgs e)
		{
			if (SubjectIndex != -1)
			{
				if (aliasTextBox.Text != Subject.Display.Alias)
					Subject.Display.Alias = aliasTextBox.Text;
			}

			bool changeDetected = Subject.Display.Alias != Original.Display.Alias;
			aliasTextBox.BackColor = changeDetected ? ChangedValueColor : default(Color);
			SetListViewItemBackgroundColor(changeDetected, SubjectIndex);
		}

		private void displayTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (SubjectIndex != -1)
			{
				if (displayTypeComboBox.SelectedIndex != (int)Subject.Display.DisplayType)
					Subject.Display.DisplayType = (DisplayType)displayTypeComboBox.SelectedIndex;
			}

			bool changeDetected = Subject.Display.DisplayType != Original.Display.DisplayType;
			displayTypeComboBox.BackColor = changeDetected ? ChangedValueColor : default(Color);
			multiCheckBox.Enabled = Subject.Display.DisplayType != DisplayType.Parameter;

			if (Subject.Display.DisplayType == DisplayType.Parameter)
				multiCheckBox.CheckState = ToCheckState(Subject.Display.Multi);

			SetListViewItemBackgroundColor(changeDetected, SubjectIndex);
		}

		private void displayColumnComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (SubjectIndex != -1)
			{
				if (displayColumnComboBox.SelectedIndex != (int)Subject.Display.DisplayColumn)
					Subject.Display.DisplayColumn = (DisplayColumn)displayColumnComboBox.SelectedIndex;
			}

			bool changeDetected = Subject.Display.DisplayColumn != Original.Display.DisplayColumn;
			displayColumnComboBox.BackColor = changeDetected ? ChangedValueColor : default(Color);
			SetListViewItemBackgroundColor(changeDetected, SubjectIndex);
		}
		
		private void sortTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (SubjectIndex != -1)
			{
				if (sortTypeComboBox.SelectedIndex != (int)Subject.Display.SortType)
					Subject.Display.SortType = (SortType)sortTypeComboBox.SelectedIndex;
			}

			bool changeDetected = Subject.Display.SortType != Original.Display.SortType;
			sortTypeComboBox.BackColor = changeDetected ? ChangedValueColor : default(Color);
			SetListViewItemBackgroundColor(changeDetected, SubjectIndex);
		}

		private void resultUnavailableComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (SubjectIndex != -1)
			{
				if (resultUnavailableComboBox.SelectedIndex != (int)Subject.Display.ResultUnavailable)
					Subject.Display.ResultUnavailable = (ResultUnavailable)resultUnavailableComboBox.SelectedIndex;
			}

			bool changeDetected = Subject.Display.ResultUnavailable != Original.Display.ResultUnavailable;
			resultUnavailableComboBox.BackColor = changeDetected ? ChangedValueColor : default(Color);
			SetListViewItemBackgroundColor(changeDetected, SubjectIndex);
		}

		private void quarterDateComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (SubjectIndex != -1)
			{
				if (quarterDateComboBox.SelectedIndex != (int)Subject.Display.QuarterDate)
					Subject.Display.QuarterDate = (QuarterDate)quarterDateComboBox.SelectedIndex;
			}

			bool changeDetected = Subject.Display.QuarterDate != Original.Display.QuarterDate;
			quarterDateComboBox.BackColor = changeDetected ? ChangedValueColor : default(Color);
			SetListViewItemBackgroundColor(changeDetected, SubjectIndex);
		}

		private void monthStepNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			if (SubjectIndex != -1)
			{
				int? value = monthStepNumericUpDown.Value == 0 ? null : (int?)monthStepNumericUpDown.Value;

				if (value != Subject.Display.MonthStep)
					Subject.Display.MonthStep = value;
			}

			bool changeDetected = Subject.Display.MonthStep != Original.Display.MonthStep;
			monthStepNumericUpDown.BackColor = changeDetected ? ChangedValueColor : default(Color);
			SetListViewItemBackgroundColor(changeDetected, SubjectIndex);
		}

		private void monthLimitCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			bool alreadyScreenRefresh = ScreenRefresh;
			ScreenRefresh = true;

			if (SubjectIndex != -1)
			{
				if (monthLimitCheckBox.Checked && Subject.Display.MonthLimit == null)
				{
					Subject.Display.MonthLimit = 0;
					monthLimitNumericUpDown.Value = 0;
				}
				else if (monthLimitCheckBox.Checked && Subject.Display.MonthLimit != null)
				{
					monthLimitNumericUpDown.Value = Subject.Display.MonthLimit ?? 0;
				}
				else
				{
					Subject.Display.MonthLimit = null;
					monthLimitNumericUpDown.Value = 0;
				}
			}

			ScreenRefresh = alreadyScreenRefresh;
			bool changeDetected = Subject.Display.MonthLimit != Original.Display.MonthLimit;
			monthLimitNumericUpDown.Enabled = monthLimitCheckBox.Checked;
			monthLimitNumericUpDown.BackColor = changeDetected ? ChangedValueColor : default(Color);
			SetListViewItemBackgroundColor(changeDetected, SubjectIndex);
		}

		private void monthLimitNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			if (SubjectIndex != -1)
			{
				int? value = monthLimitCheckBox.Checked ? (int?)monthLimitNumericUpDown.Value : null;

				if (value != Subject.Display.MonthLimit)
					Subject.Display.MonthLimit = value;
			}

			bool changeDetected = Subject.Display.MonthLimit != Original.Display.MonthLimit;
			monthLimitNumericUpDown.BackColor = changeDetected ? ChangedValueColor : default(Color);
			SetListViewItemBackgroundColor(changeDetected, SubjectIndex);
		}

		private void multiCheckBox_CheckStateChanged(object sender, EventArgs e)
		{
			if (SubjectIndex != -1)
			{
				if (multiCheckBox.CheckState != ToCheckState(Subject.Display.Multi))
					Subject.Display.Multi = ToNullableBool(multiCheckBox.CheckState);
			}

			bool changeDetected = Subject.Display.Multi != Original.Display.Multi;
			multiCheckBox.BackColor = changeDetected ? ChangedValueColor : default(Color);
			SetListViewItemBackgroundColor(changeDetected, SubjectIndex);
		}

		private void zeroLastCheckBox_CheckStateChanged(object sender, EventArgs e)
		{
			if (SubjectIndex != -1)
			{
				if (zeroLastCheckBox.CheckState != ToCheckState(Subject.Display.ZeroLast))
					Subject.Display.ZeroLast = ToNullableBool(zeroLastCheckBox.CheckState);
			}

			bool changeDetected = Subject.Display.ZeroLast != Original.Display.ZeroLast;
			zeroLastCheckBox.BackColor = changeDetected ? ChangedValueColor : default(Color);
			SetListViewItemBackgroundColor(changeDetected, SubjectIndex);
		}

		private void visibleCheckBox_CheckStateChanged(object sender, EventArgs e)
		{
			if (SubjectIndex != -1)
			{
				if (visibleCheckBox.CheckState != ToCheckState(Subject.Display.Visible))
					Subject.Display.Visible = ToNullableBool(visibleCheckBox.CheckState);
			}

			bool changeDetected = Subject.Display.Visible != Original.Display.Visible;
			visibleCheckBox.BackColor = changeDetected ? ChangedValueColor : default(Color);
			SetListViewItemBackgroundColor(changeDetected, SubjectIndex);
		}

		private void valuesTypeButton_Click(object sender, EventArgs e)
		{
			if (SubjectIndex != -1)
			{
				EndGridCellEditMode();
				if (Subject.Type == FilterParameterType.Values && Subject.ValuesGridRows.Count != 0)
				{
					FilterParameter subject = Subject;
					FilterParameter original = Original;
					ValuesType currentType = Subject.ValuesType;
					ValuesTypeDialog valuesTypeDialog = new ValuesTypeDialog(ref subject, ref original, ChangedValueColor);
					valuesTypeDialog.ShowDialog();

					if (Subject.ValuesType != currentType)
					{
						ScreenRefresh = true;
						RefreshKeysDataGridView();
						ScreenRefresh = false;
					}

					string message = Subject.GetPossibleValuesTypes().Contains(Subject.ValuesType) ? String.Empty :
						"The current format is invalid.\nPlease select an allowed format.";
					valuesTypeErrorProvider.SetError(valuesTypeLabel, message);
				}
			}

			SetValuesTypeButtonText();
			SetListViewItemBackgroundColor(Subject.ValuesType != Original.ValuesType, SubjectIndex);
		}

		private void SetValuesTypeButtonText()
		{
			valuesTypeButton.Text = Enums.GetFormattedString(Subject.ValuesType);

			bool valuesTypesEqual = Subject.ValuesType == Original.ValuesType;
			bool selectedTypesEqual = Subject.Display.SelectedType == Original.Display.SelectedType;
			bool disabledTypesEqual = Subject.Display.DisabledType == Original.Display.DisabledType;

			valuesTypeButton.BackColor = valuesTypesEqual && selectedTypesEqual && disabledTypesEqual ? default(Color) : ChangedValueColor;
		}

		private void addKeyButton_Click(object sender, EventArgs e)
		{
			if (!neitherRadioButton.Checked && SubjectIndex != -1)
			{
				EndGridCellEditMode();
				int key = Subject.ValuesGridRows.Count;

				if (Subject.Type == FilterParameterType.Values && key == 0)
					valuesTypeErrorProvider.SetError(valuesTypeLabel, "Please ensure that the correct format for the JSON output is chosen.");

				while (Subject.ValuesGridRows.ContainsKey(key.ToString()))
					key++;

				ValuesGridRow value = new ValuesGridRow();
				value.Name = key.ToString();

				if (tableRadioButton.Checked)
					value.IsFromTable = false;

				int newRowIndex = valuesDataGridView.Rows.Count;
				Subject.ValuesGridRows.Add(key.ToString(), value);
				Subject.UpdatePropertiesFromKeysGridRows();
				valuesDataGridView.Rows.Add();
				ScreenRefresh = true;
				RefreshKeysDataGridRow(valuesDataGridView.Rows.Count - 1);
				ScreenRefresh = false;
				valuesDataGridView.CurrentCell = valuesDataGridView[valuesValueColumn.Index, newRowIndex];
				valuesDataGridView.BeginEdit(true);
				SetValuesTypeButtonText();
				SetListViewItemBackgroundColor(Subject.ValuesGridRows.SequenceEqual(Original.ValuesGridRows), SubjectIndex);
			}
		}

		private void deleteKeyButton_Click(object sender, EventArgs e)
		{
			if (valuesDataGridView.RowCount > 0 && SubjectIndex != -1)
			{
				EndGridCellEditMode();
				int row = valuesDataGridView.CurrentCell.RowIndex;
				Dictionary<string, ValuesGridRow> newDictionary = new Dictionary<string, ValuesGridRow>();
				ValuesGridRow value = Subject.ValuesGridRows.Values.ElementAt(row);

				if (value.IsFromTable != true)
				{
					valuesDataGridView.Rows.RemoveAt(row);

					for (int i = 0; i < Subject.ValuesGridRows.Count; i++)
						if (i != row)
							newDictionary.Add(Subject.ValuesGridRows.Keys.ElementAt(i), Subject.ValuesGridRows.Values.ElementAt(i));

					Subject.ValuesGridRows = newDictionary;
				}

				SetListViewItemBackgroundColor(Subject.ValuesGridRows.SequenceEqual(Original.ValuesGridRows), SubjectIndex);
			}
		}

		private void moveUpKeyButton_Click(object sender, EventArgs e)
		{
			int startingIndex = valuesDataGridView.CurrentCell.RowIndex;

			if (startingIndex > 0 && SubjectIndex != -1)
			{
				int minimumIndex = 0;
				int maximumIndex = valuesDataGridView.RowCount - 1;

				if (tableRadioButton.Checked)
				{
					minimumIndex = Subject.ValuesGridRows.Values.ToList<ValuesGridRow>().FindIndex(p => p.IsFromTable != true);
					maximumIndex = Subject.ValuesGridRows.Values.ToList<ValuesGridRow>().FindLastIndex(p => p.IsFromTable != true);
				}

				if (startingIndex > minimumIndex && startingIndex <= maximumIndex && !neitherRadioButton.Checked)
					MoveKeyItemInGrid(startingIndex, startingIndex - 1);
			}
		}

		private void moveDownKeyButton_Click(object sender, EventArgs e)
		{
			int startingIndex = valuesDataGridView.CurrentCell.RowIndex;

			if (startingIndex < valuesDataGridView.Rows.Count - 1 && SubjectIndex != -1)
			{
				int minimumIndex = 0;
				int maximumIndex = valuesDataGridView.RowCount - 1;

				if (tableRadioButton.Checked)
				{
					minimumIndex = Subject.ValuesGridRows.Values.ToList<ValuesGridRow>().FindIndex(p => p.IsFromTable != true);
					maximumIndex = Subject.ValuesGridRows.Values.ToList<ValuesGridRow>().FindLastIndex(p => p.IsFromTable != true);
				}

				if (startingIndex >= minimumIndex && startingIndex < maximumIndex && !neitherRadioButton.Checked)
					MoveKeyItemInGrid(startingIndex, startingIndex + 1);
			}
		}

		private void MoveKeyItemInGrid(int startingIndex, int destinationIndex)
		{
			EndGridCellEditMode();
			Dictionary<string, ValuesGridRow> newDictionary = new Dictionary<string, ValuesGridRow>();
			int i = 0;
			string key = Subject.ValuesGridRows.ElementAt(startingIndex).Key;
			ValuesGridRow value = Subject.ValuesGridRows[key];

			valuesDataGridView.Rows.RemoveAt(startingIndex);
			valuesDataGridView.Rows.Insert(destinationIndex, 1);

			do
			{
				if (newDictionary.Count == destinationIndex)
					newDictionary.Add(key, value);

				if (i != startingIndex && i < Subject.ValuesGridRows.Count)
					newDictionary.Add(Subject.ValuesGridRows.Keys.ElementAt(i), Subject.ValuesGridRows.Values.ElementAt(i));

				i++;
			} while (newDictionary.Count != Subject.ValuesGridRows.Count);

			Subject.ValuesGridRows = newDictionary;
			ScreenRefresh = true;
			RefreshKeysDataGridRow(destinationIndex);
			ScreenRefresh = false;
			SetListViewItemBackgroundColor(Subject.ValuesGridRows.SequenceEqual(Original.ValuesGridRows), SubjectIndex);
			valuesDataGridView.CurrentCell = valuesDataGridView[valuesDataGridView.CurrentCell.ColumnIndex, destinationIndex];
		}

		private void SetKeysGridCellsBackgroundColor()
		{
			bool changeDetected = false;

			for (int i = 0; i < valuesDataGridView.Rows.Count; i++)
			{
				if (SetKeysGridCellsBackgroundColor(i))
					changeDetected = true;
			}

			SetListViewItemBackgroundColor(changeDetected, SubjectIndex);
			SetValuesTypeButtonText();
		}

		private bool SetKeysGridCellsBackgroundColor(int row)
		{
			string key = Subject.ValuesGridRows.ElementAt(row).Key;
			ValuesGridRow value = Subject.ValuesGridRows.ElementAt(row).Value;
			ValuesGridRow original = Original.ValuesGridRows.FirstOrDefault(p => p.Key == key).Value ?? new ValuesGridRow();
			bool originalExists = Original.ValuesGridRows.ContainsKey(key);
			DataGridViewCell keysValueCell = valuesDataGridView[valuesValueColumn.Index, row];
			DataGridViewCell keysNameCell = valuesDataGridView[valuesNameColumn.Index, row];
			DataGridViewCell includeKeyCell = valuesDataGridView[includeValueColumn.Index, row];
			DataGridViewCell selectKeyCell = valuesDataGridView[selectValueColumn.Index, row];
			DataGridViewCell disableKeyCell = valuesDataGridView[disableValueColumn.Index, row];
			DataGridViewCell formatKeyCell = valuesDataGridView[formatValueColumn.Index, row];
			DataGridViewCell sourceLabelCell = valuesDataGridView[sourceLabelColumn.Index, row];
			
			keysValueCell.Style.BackColor = !originalExists ? ChangedValueColor : default(Color);
			keysNameCell.Style.BackColor = !originalExists || value.Name != original.Name ? ChangedValueColor : default(Color);
			includeKeyCell.Style.BackColor = !originalExists || value.IsRemoved != original.IsRemoved ? ChangedValueColor : default(Color);
			selectKeyCell.Style.BackColor = !originalExists || value.IsSelected != original.IsSelected ? ChangedValueColor : default(Color);
			disableKeyCell.Style.BackColor = !originalExists || value.IsDisabled != original.IsDisabled ? ChangedValueColor : default(Color);
			formatKeyCell.Style.BackColor = !originalExists || value.Format != original.Format ? ChangedValueColor : default(Color);
			sourceLabelCell.Style.BackColor = formatKeyCell.Style.BackColor;
			return !originalExists ? true : !value.Equals(original);
		}

		private void keysDataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			int columnIndex = valuesDataGridView.CurrentCell.ColumnIndex;

			if (columnIndex == includeValueColumn.Index || columnIndex == selectValueColumn.Index || columnIndex == disableValueColumn.Index)
				valuesDataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}

		private void keysDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex != -1 ? valuesDataGridView.CurrentCell.IsInEditMode && valuesDataGridView.CurrentCell.ColumnIndex == e.ColumnIndex : false)
			{
				DataGridViewCell selectedCell = valuesDataGridView[e.ColumnIndex, e.RowIndex];
				string key = Subject.ValuesGridRows.ElementAt(e.RowIndex).Key;
				ValuesGridRow original = Original.ValuesGridRows.FirstOrDefault(p => p.Key == key).Value ?? new ValuesGridRow();
				bool originalExists = Original.ValuesGridRows.ContainsKey(key);
				bool changeDetected = false;

				if (e.ColumnIndex == valuesValueColumn.Index)
				{
					string cellValue = selectedCell.Value.ToString();
					ValuesGridRow value = Subject.ValuesGridRows[key];

					if (value.IsFromTable == null && !Subject.ValuesGridRows.ContainsKey(cellValue))
					{
						Dictionary<string, ValuesGridRow> newDictionary = new Dictionary<string, ValuesGridRow>();

						for (int i = 0; i < Subject.ValuesGridRows.Count; i++)
							newDictionary.Add(i == e.RowIndex ? cellValue : Subject.ValuesGridRows.ElementAt(i).Key, Subject.ValuesGridRows.ElementAt(i).Value);

						Subject.ValuesGridRows = newDictionary;
					}
					else if (value.IsFromTable == true)
						selectedCell.Value = key;
					else if (value.IsFromTable == false)
					{
						if (Subject.ValuesGridRows.ContainsKey(cellValue) && Subject.ValuesGridRows.Keys.ToList().IndexOf(cellValue) != e.RowIndex)
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
								Dictionary<string, ValuesGridRow> newDictionary = new Dictionary<string, ValuesGridRow>();

								for (int i = 0; i < Subject.ValuesGridRows.Count; i++)
									newDictionary.Add(i == e.RowIndex ? intValue.ToString() : Subject.ValuesGridRows.ElementAt(i).Key, Subject.ValuesGridRows.ElementAt(i).Value);

								Subject.ValuesGridRows = newDictionary;
							}
							else
							{
								MessageBox.Show("The key value must be an integer when it is appended to values from the database.", "Invalid Value",
									MessageBoxButtons.OK, MessageBoxIcon.Warning);
								selectedCell.Value = key;
							}
						}
					}

					changeDetected = !originalExists;
				}
				else if (e.ColumnIndex == valuesNameColumn.Index && Subject.ValuesGridRows[key].Name != selectedCell.Value.ToString())
				{
					Subject.ValuesGridRows[key].Name = selectedCell.Value.ToString();
					changeDetected = !originalExists || Subject.ValuesGridRows[key].Name != original.Name;
				}
				else if (e.ColumnIndex == includeValueColumn.Index && Subject.ValuesGridRows[key].IsRemoved != !(bool)selectedCell.Value)
				{
					Subject.ValuesGridRows[key].IsRemoved = !(bool)selectedCell.Value;
					changeDetected = !originalExists || Subject.ValuesGridRows[key].IsRemoved != original.IsRemoved;
				}
				else if (e.ColumnIndex == selectValueColumn.Index && Subject.ValuesGridRows[key].IsSelected != (bool)selectedCell.Value)
				{
					Subject.ValuesGridRows[key].IsSelected = (bool)selectedCell.Value;
					changeDetected = !originalExists || Subject.ValuesGridRows[key].IsSelected != original.IsSelected;
				}
				else if (e.ColumnIndex == disableValueColumn.Index && Subject.ValuesGridRows[key].IsDisabled != (bool)selectedCell.Value)
				{
					Subject.ValuesGridRows[key].IsDisabled = (bool)selectedCell.Value;
					changeDetected = !originalExists || Subject.ValuesGridRows[key].IsDisabled != original.IsDisabled;
				}
				else if (e.ColumnIndex == formatValueColumn.Index && Subject.ValuesGridRows[key].Format != selectedCell.Value.ToString())
				{
					Subject.ValuesGridRows[key].Format = selectedCell.Value.ToString();
					changeDetected = !originalExists || Subject.ValuesGridRows[key].Format != original.Format;
				}

				if (changeDetected)
				{
					Subject.UpdatePropertiesFromKeysGridRows();
					RefreshKeysDataGridRow(e.RowIndex);

					if ((e.ColumnIndex == valuesValueColumn.Index || e.ColumnIndex == valuesNameColumn.Index) && !Subject.GetPossibleValuesTypes().Contains(Subject.ValuesType))
					{
						string message = Subject.GetPossibleValuesTypes().Contains(Subject.ValuesType) ? String.Empty :
							String.Format("A change to the values list caused the {0} format to become invalid.\nTo prevent an error, the format was changed to {1}.",
							Enums.GetFormattedString(Subject.ValuesType), Enums.GetFormattedString(ValuesType.DictionaryStrings));
						valuesTypeErrorProvider.SetError(valuesTypeLabel, message);
						Subject.ValuesType = ValuesType.DictionaryStrings;
						SetValuesTypeButtonText();
					}
				}

				selectedCell.Style.BackColor = changeDetected ? ChangedValueColor : default(Color);
				SetListViewItemBackgroundColor(changeDetected, SubjectIndex);
			}
		}

		private void RefreshKeysDataGridView()
		{
			valuesDataGridView.Rows.Clear();

			if (Subject.ValuesGridRows.Count != 0)
			{
				for (int i = 0; i < Subject.ValuesGridRows.Count; i++)
				{
					valuesDataGridView.Rows.Add();
					RefreshKeysDataGridRow(i);
				}

				string message = String.Empty;

				if (Subject.AddKeyValues.Count != Subject.ValuesGridRows.Count(p => p.Value.IsFromTable == false))
				{
					message = String.Concat(message, "The following added keys are no longer valid and have been removed:\n");
					List<string> addedKeys = Subject.ValuesGridRows.Where(p => p.Value.IsFromTable == false).Select(s => s.Key).ToList<string>();

					foreach (var item in Subject.AddKeyValues.Where(p => !addedKeys.Contains(p.Key)))
						message = String.Concat(message, String.Format("\n\t{0}:\t{1}", item.Key, item.Value));
				}

				if (Subject.RemoveKeys.Count != Subject.ValuesGridRows.Count(p => p.Value.IsRemoved))
				{
					message = String.Concat(message, String.Format("{0}The following removed keys are no longer valid and have been eliminated:\n",
						!String.IsNullOrEmpty(message) ? "\n\n" : String.Empty));

					List<string> removedKeys = Subject.ValuesGridRows.Where(p => p.Value.IsRemoved).Select(s => s.Key).ToList<string>();

					foreach (var item in Subject.RemoveKeys.Where(p => !removedKeys.Contains(p.ToString())))
						message = String.Concat(message, String.Format("\n\t{0}", item));
				}

				if (Subject.Display.SelectedList.Count != Subject.ValuesGridRows.Count(p => p.Value.IsSelected))
				{
					message = String.Concat(message, String.Format("{0}The following selected keys are no longer valid and have been removed:\n",
						!String.IsNullOrEmpty(message) ? "\n\n" : String.Empty));

					List<string> selectedKeys = Subject.ValuesGridRows.Where(p => p.Value.IsSelected).Select(s => s.Key).ToList<string>();

					foreach (var item in Subject.Display.SelectedList.Where(p => !selectedKeys.Contains(p)))
						message = String.Concat(message, String.Format("\n\t{0}", item));
				}

				if (Subject.Display.DisabledList.Count != Subject.ValuesGridRows.Count(p => p.Value.IsDisabled))
				{
					message = String.Concat(message, String.Format("{0}The following disabled keys are no longer valid and have been removed:\n",
						!String.IsNullOrEmpty(message) ? "\n\n" : String.Empty));

					List<string> disabledKeys = Subject.ValuesGridRows.Where(p => p.Value.IsDisabled).Select(s => s.Key).ToList<string>();

					foreach (var item in Subject.Display.DisabledList.Where(p => !disabledKeys.Contains(p)))
						message = String.Concat(message, String.Format("\n\t{0}", item));
				}

				if (!String.IsNullOrEmpty(message))
				{
					bool alreadyScreenRefresh = ScreenRefresh;
					ScreenRefresh = false;
					SetListViewItemBackgroundColor(true, SubjectIndex);
					ScreenRefresh = alreadyScreenRefresh;
					MessageBox.Show(message, "Invalid Settings Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
		}

		private void RefreshKeysDataGridRow(int row)
		{
			string key = Subject.ValuesGridRows.ElementAt(row).Key;
			ValuesGridRow value = Subject.ValuesGridRows.ElementAt(row).Value;
			DataGridViewCell valueCell = valuesDataGridView[valuesValueColumn.Index, row];
			DataGridViewCell nameCell = valuesDataGridView[valuesNameColumn.Index, row];
			DataGridViewCell includedCell = valuesDataGridView[includeValueColumn.Index, row];
			DataGridViewCell selectedCell = valuesDataGridView[selectValueColumn.Index, row];
			DataGridViewCell disabledCell = valuesDataGridView[disableValueColumn.Index, row];
			DataGridViewCell formatCell = valuesDataGridView[formatValueColumn.Index, row];
			DataGridViewCell isTableSourceCell = valuesDataGridView[isTableSourceColumn.Index, row];
			DataGridViewCell sourceLabelCell = valuesDataGridView[sourceLabelColumn.Index, row];

			if (valueCell.Value == null ? true : valueCell.Value.ToString() != key)
				valueCell.Value = key;
			
			valueCell.ReadOnly = value.IsFromTable ?? false;
			
			if (nameCell.Value == null ? true : nameCell.Value.ToString() != value.Name)
				nameCell.Value = value.Name;

			nameCell.ReadOnly = value.IsFromTable ?? false || Subject.ValuesType == ValuesType.ListIntegers || Subject.ValuesType == ValuesType.ListStrings;

			if (includedCell.Value == null ? true : (bool)includedCell.Value != !value.IsRemoved)
				includedCell.Value = !value.IsRemoved;
			
			includedCell.ReadOnly = !(value.IsFromTable ?? false);
			
			if(selectedCell.Value == null ? true : (bool)selectedCell.Value != value.IsSelected)
				selectedCell.Value = value.IsSelected;

			if (disabledCell.Value == null ? true : (bool)disabledCell.Value != value.IsDisabled)
				disabledCell.Value = value.IsDisabled;
			
			if (formatCell.Value == null ? true : formatCell.Value.ToString() != value.Format)
				formatCell.Value = value.Format;

			if ((bool?)isTableSourceCell.Value != value.IsFromTable)
			{
				isTableSourceCell.Value = value.IsFromTable;
				sourceLabelCell.Value = GetKeySourceLabel(value.IsFromTable);
			}

			SetListViewItemBackgroundColor(SetKeysGridCellsBackgroundColor(row), SubjectIndex);
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

		private void addHelpButton_Click(object sender, EventArgs e)
		{
			if (SubjectIndex != -1)
				AddDictionaryItemToGrid(StringDictionary.Help);
		}

		private void deleteHelpButton_Click(object sender, EventArgs e)
		{
			if (SubjectIndex != -1)
				DeleteDictionaryItemFromGrid(StringDictionary.Help);
		}

		private void moveUpHelpButton_Click(object sender, EventArgs e)
		{
			int startingIndex = helpDataGridView.CurrentCell.RowIndex;

			if (startingIndex > 0 && SubjectIndex != -1)
			{
				Dictionary<string, string> subjectDictionary = Subject.Display.Help;
				Dictionary<string, string> original = Original.Display.Help;
				MoveDictionaryItemInGrid(startingIndex, startingIndex - 1, StringDictionary.Help);
			}
		}

		private void moveDownHelpButton_Click(object sender, EventArgs e)
		{
			int startingIndex = helpDataGridView.CurrentCell.RowIndex;

			if (startingIndex < helpDataGridView.RowCount - 1 && SubjectIndex != -1)
			{
				Dictionary<string, string> subjectDictionary = Subject.Display.Help;
				Dictionary<string, string> original = Original.Display.Help;
				MoveDictionaryItemInGrid(startingIndex, startingIndex + 1, StringDictionary.Help);
			}
		}

		private void helpDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex != -1 ? helpDataGridView.CurrentCell.IsInEditMode && helpDataGridView.CurrentCell.ColumnIndex == e.ColumnIndex : false)
				UpdateDictionaryItemInGrid(StringDictionary.Help);
		}

		private void GetOriginalFilterParameter()
		{
			Original = GetOriginalFilterParameter(Subject);
		}

		private FilterParameter GetOriginalFilterParameter(FilterParameter subject)
		{
			FilterParameter original = new FilterParameter();
			int originalIndex = OriginalFilterParameterFile.FilterParameters.FindIndex(p => p.FilterParameterName == subject.FilterParameterName);

			if (originalIndex != -1)
				original = OriginalFilterParameterFile.FilterParameters[originalIndex];

			return original;
		}
	}
}
