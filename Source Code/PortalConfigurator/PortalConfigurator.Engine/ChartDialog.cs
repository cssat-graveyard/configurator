using Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PortalConfigurator
{
	public partial class ChartDialog : Form
	{
		public ChartInfo Subject { get; private set; }
		private ChartInfo Original { get; set; }
		private MeasurementFile Measure { get; set; }
		private Color ChangedValueColor { get; set; }
		private List<string> ValueColumns { get; set; }
		private List<string> ChartIds { get; set; }
		private bool Loaded { get; set; }
		private string InitialChartId { get; set; }

		public ChartDialog()
			:this(new ChartInfo(), new ChartInfo(), new MeasurementFile(), Color.LemonChiffon, new List<string>())
		{ }

		public ChartDialog(ChartInfo subject, ChartInfo original, MeasurementFile measure, Color changedValueColor, List<string> chartIds)
		{
			InitializeComponent();
			this.Subject = subject;
			this.Original = original;
			this.Measure = measure;
			this.ChangedValueColor = changedValueColor;
			this.ValueColumns = measure.Transform.ValueFields;
			this.ChartIds = chartIds;
			this.Loaded = false;
			this.InitialChartId = subject.ChartId;
		}

		private void ChartDialog_Load(object sender, EventArgs e)
		{
			chartIdTextBox.Text = Subject.ChartId;
			chartTypeComboBox.Items.AddRange(Enums.GetFormattedChartTypeEnumNames());
			chartTypeComboBox.SelectedIndex = (int)Subject.ChartType;
			maxSetsNumericUpDown.Value = Subject.MaxSets ?? 0;
			chartWidthNumericUpDown.Value = Subject.BaseOptionSet.Width ?? 0;
			chartHeightNumericUpDown.Value = Subject.BaseOptionSet.Height ?? 0;
			multiAllowedCheckBox.Checked = Subject.AddInputClass == AddInputClass.MultiAllowed;
			Subject.HideColumns.RemoveAll(p => p >= ValueColumns.Count);

			if (ValueColumns.Count != 0)
				showColumnsDataGridView.Rows.Add(ValueColumns.Count);

			DataGridViewCell showColumnNameCell;
			DataGridViewCell showColumnCell;

			for (int i = 0; i < ValueColumns.Count; i++)
			{
				bool currentlyHidden = Subject.HideColumns.Contains(i);
				bool originallyHidden = Original.HideColumns.Contains(i);
				showColumnNameCell = showColumnsDataGridView[showColumnNameColumn.Index, i];
				showColumnCell = showColumnsDataGridView[showColumnColumn.Index, i];

				showColumnNameCell.Value = ValueColumns[i];
				showColumnCell.Value = !currentlyHidden;
				showColumnCell.Style.BackColor = currentlyHidden == originallyHidden ? default(Color) : ChangedValueColor;
			}

			xAxisLabelTextBox.Text = Subject.Label.XAxisLabel;
			yAxisLabelTextBox.Text = Subject.Label.YAxisLabel;
			yAxisMinNumericUpDown.Value = (decimal)(Subject.Label.YAxisMin ?? 0.0f);
			yAxisMaxNumericUpDown.Value = (decimal)(Subject.Label.YAxisMax ?? 0.0f);
			yAxisFormatComboBox.Items.AddRange(Enums.GetFormattedAxisFormatEnumNames());
			yAxisFormatComboBox.SelectedIndex = (int)Subject.Label.YAxisFormat;
			leftNumericUpDown.Value = Subject.BaseOptionSet.ChartArea.Left ?? 0;
			topNumericUpDown.Value = Subject.BaseOptionSet.ChartArea.Top ?? 0;

			int intValue;
			char[] percentChar = new char[] { '%' };
			string percentString = String.Empty;

			if (!String.IsNullOrEmpty(Subject.BaseOptionSet.ChartArea.Width))
			{
				percentString = Subject.BaseOptionSet.ChartArea.Width.TrimEnd(percentChar);
				areaWidthNumericUpDown.Value = int.TryParse(percentString, out intValue) ? intValue : 0;
			}
			else
				areaWidthNumericUpDown.Value = 0;

			if (!String.IsNullOrEmpty(Subject.BaseOptionSet.ChartArea.Height))
			{
				percentString = Subject.BaseOptionSet.ChartArea.Height.TrimEnd(new char[] { '%' });
				areaHeightNumericUpDown.Value = int.TryParse(percentString, out intValue) ? intValue : 0;
			}
			else
				areaHeightNumericUpDown.Value = 0;

			chartIdErrorProvider.SetError(chartIdLabel, String.Empty);
			warningProvider.SetError(chartTypeLabel, String.Empty);
			warningProvider.SetError(xAxisLabelLabel, String.Empty);
			warningProvider.SetError(yAxisLabelLabel, String.Empty);
			warningProvider.SetError(yAxisMinLabel, String.Empty);
			warningProvider.SetError(yAxisMaxLabel, String.Empty);
			warningProvider.SetError(yAxisFormatLabel, String.Empty);
			Loaded = true;
		}

		private void chartIdTextBox_TextChanged(object sender, EventArgs e)
		{
			if (Loaded && chartIdTextBox.Text != Subject.ChartId)
			{
				bool invalidName = ChartIds.Contains(chartIdTextBox.Text) && chartIdTextBox.Text != InitialChartId;
				string errorMessage = invalidName ? "This Chart ID is already used." : String.Empty;
				Subject.ChartId = chartIdTextBox.Text;
				chartIdErrorProvider.SetError(chartIdLabel, errorMessage);
				doneButton.Enabled = !invalidName;
			}

			chartIdTextBox.BackColor = Subject.ChartId == Original.ChartId ? default(Color) : ChangedValueColor;
		}

		private void chartTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (Loaded && chartTypeComboBox.SelectedIndex != (int)Subject.ChartType)
				Subject.ChartType = (ChartType)chartTypeComboBox.SelectedIndex;

			chartTypeComboBox.BackColor = Subject.ChartType == Original.ChartType ? default(Color) : ChangedValueColor;
			bool differentTypes = ChartWarning.IsDifferent(Subject.ChartType, Measure.ChartType);
			warningProvider.SetError(chartTypeLabel,
				ChartWarning.GetWarning(ChartLocation.Local, "chart type", Subject.ChartType, true, differentTypes, Subject.ChartType == ChartType.NoChartType));
		}

		private void maxSetsNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			int? value = maxSetsNumericUpDown.Value == 0 ? null : (int?)maxSetsNumericUpDown.Value;

			if (Loaded && value != Subject.MaxSets)
				Subject.MaxSets = value;

			maxSetsNumericUpDown.BackColor = Subject.MaxSets == Original.MaxSets ? default(Color) : ChangedValueColor;
		}

		private void chartWidthNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			int? value = chartWidthNumericUpDown.Value == 0 ? null : (int?)chartWidthNumericUpDown.Value;

			if (Loaded && value != Subject.BaseOptionSet.Width)
				Subject.BaseOptionSet.Width = value;

			chartWidthNumericUpDown.BackColor = Subject.BaseOptionSet.Width == Original.BaseOptionSet.Width ? default(Color) : ChangedValueColor;
		}

		private void chartHeightNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			int? value = chartHeightNumericUpDown.Value == 0 ? null : (int?)chartHeightNumericUpDown.Value;

			if (Loaded && value != Subject.BaseOptionSet.Height)
				Subject.BaseOptionSet.Height = value;

			chartHeightNumericUpDown.BackColor = Subject.BaseOptionSet.Height == Original.BaseOptionSet.Height ? default(Color) : ChangedValueColor;
		}

		private void multiAllowedCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			AddInputClass value = (AddInputClass)(multiAllowedCheckBox.Checked ? 1 : 0);

			if (Loaded && value != Subject.AddInputClass)
				Subject.AddInputClass = value;

			multiAllowedCheckBox.BackColor = Subject.AddInputClass == Original.AddInputClass ? default(Color) : ChangedValueColor;
		}

		private void showColumnsDataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			if (showColumnsDataGridView.CurrentCell.ColumnIndex == showColumnColumn.Index)
				showColumnsDataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}

		private void showColumnsDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex != -1 && e.ColumnIndex == showColumnColumn.Index)
			{
				DataGridViewCell showColumnCell = showColumnsDataGridView[e.ColumnIndex, e.RowIndex];
				bool showColumn = (bool)showColumnCell.Value;

				if (Subject.HideColumns.Contains(e.RowIndex) == showColumn)
				{
					if (Subject.HideColumns.Contains(e.RowIndex))
						Subject.HideColumns.Remove(e.RowIndex);
					else
					{
						Subject.HideColumns.Add(e.RowIndex);
						Subject.HideColumns.Sort();
					}

					showColumnCell.Style.BackColor = Subject.HideColumns.Contains(e.RowIndex) == Original.HideColumns.Contains(e.RowIndex) ? default(Color) : ChangedValueColor;
				}
			}
		}

		private void xAxisLabelTextBox_TextChanged(object sender, EventArgs e)
		{
			if (Loaded && xAxisLabelTextBox.Text != Subject.Label.XAxisLabel)
				Subject.Label.XAxisLabel = xAxisLabelTextBox.Text;

			xAxisLabelTextBox.BackColor = Subject.Label.XAxisLabel == Original.Label.XAxisLabel ? default(Color) : ChangedValueColor;
			bool differentLabel = ChartWarning.IsDifferent(Subject.Label.XAxisLabel, Measure.Label.XAxisLabel);
			warningProvider.SetError(xAxisLabelLabel,
				ChartWarning.GetWarning(ChartLocation.Local, "X axis label", Subject.ChartType, true, differentLabel, String.IsNullOrEmpty(Subject.Label.XAxisLabel)));
		}

		private void yAxisLabelTextBox_TextChanged(object sender, EventArgs e)
		{
			if (Loaded && yAxisLabelTextBox.Text != Subject.Label.YAxisLabel)
				Subject.Label.YAxisLabel = yAxisLabelTextBox.Text;
	
			yAxisLabelTextBox.BackColor = Subject.Label.YAxisLabel == Original.Label.YAxisLabel ? default(Color) : ChangedValueColor;
			bool differentLabel = ChartWarning.IsDifferent(Subject.Label.YAxisLabel, Measure.Label.YAxisLabel);
			warningProvider.SetError(yAxisLabelLabel,
				ChartWarning.GetWarning(ChartLocation.Local, "Y axis label", Subject.ChartType, true, differentLabel, String.IsNullOrEmpty(Subject.Label.YAxisLabel)));
		}

		private void yAxisMinNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			float? value = yAxisMinNumericUpDown.Value == 0 ? null : (float?)yAxisMinNumericUpDown.Value;

			if (Loaded && value != Subject.Label.YAxisMin)
				Subject.Label.YAxisMin = value;

			yAxisMinNumericUpDown.BackColor = Subject.Label.YAxisMin == Original.Label.YAxisMin ? default(Color) : ChangedValueColor;
			bool differentMinimum = ChartWarning.IsDifferent(Subject.Label.YAxisMin, Measure.Label.YAxisMin);
			warningProvider.SetError(yAxisMinLabel,
				ChartWarning.GetWarning(ChartLocation.Local, "Y axis minimum", Subject.ChartType, true, differentMinimum, Subject.Label.YAxisMin == null));
		}

		private void yAxisMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			float? value = yAxisMaxNumericUpDown.Value == 0 ? null : (float?)yAxisMaxNumericUpDown.Value;

			if (Loaded && value != Subject.Label.YAxisMax)
				Subject.Label.YAxisMax = value;

			yAxisMaxNumericUpDown.BackColor = Subject.Label.YAxisMax == Original.Label.YAxisMax ? default(Color) : ChangedValueColor;
			bool differentMaximum = ChartWarning.IsDifferent(Subject.Label.YAxisMax, Measure.Label.YAxisMax);
			warningProvider.SetError(yAxisMaxLabel,
				ChartWarning.GetWarning(ChartLocation.Local, "Y axis maximum", Subject.ChartType, true, differentMaximum, Subject.Label.YAxisMax == null));
		}

		private void yAxisFormatComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (Loaded && yAxisFormatComboBox.SelectedIndex != (int)Subject.Label.YAxisFormat)
				Subject.Label.YAxisFormat = (AxisFormat)yAxisFormatComboBox.SelectedIndex;

			yAxisFormatComboBox.BackColor = Subject.Label.YAxisFormat == Original.Label.YAxisFormat ? default(Color) : ChangedValueColor;
			bool differentFormat = ChartWarning.IsDifferent(Subject.Label.YAxisFormat, Measure.Label.YAxisFormat);
			warningProvider.SetError(yAxisFormatLabel,
				ChartWarning.GetWarning(ChartLocation.Local, "Y axis format", Subject.ChartType, true, differentFormat, Subject.Label.YAxisFormat == AxisFormat.NoFormat));
		}

		private void leftNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			int? value = leftNumericUpDown.Value == 0 ? null : (int?)leftNumericUpDown.Value;
			int? original = Original.BaseOptionSet.ChartArea.Left;

			if (Loaded && value != Subject.BaseOptionSet.ChartArea.Left)
				Subject.BaseOptionSet.ChartArea.Left = value;

			leftNumericUpDown.BackColor = Subject.BaseOptionSet.ChartArea.Left == original ? default(Color) : ChangedValueColor;
		}

		private void topNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			int? value = topNumericUpDown.Value == 0 ? null : (int?)topNumericUpDown.Value;
			int? original = Original.BaseOptionSet.ChartArea.Top;

			if (Loaded && value != Subject.BaseOptionSet.ChartArea.Top)
				Subject.BaseOptionSet.ChartArea.Top = value;

			topNumericUpDown.BackColor = Subject.BaseOptionSet.ChartArea.Top == original ? default(Color) : ChangedValueColor;
		}

		private void areaWidthNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			string value = areaWidthNumericUpDown.Value == 0 ? String.Empty : String.Concat(areaWidthNumericUpDown.Value.ToString(), "%");
			string original = Original.BaseOptionSet.ChartArea.Width;

			if (Loaded && value != Subject.BaseOptionSet.ChartArea.Width)
				Subject.BaseOptionSet.ChartArea.Width = value;

			areaWidthNumericUpDown.BackColor = Subject.BaseOptionSet.ChartArea.Width == original ? default(Color) : ChangedValueColor;
		}

		private void areaHeightNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			string value = areaHeightNumericUpDown.Value == 0 ? String.Empty : String.Concat(areaHeightNumericUpDown.Value.ToString(), "%");
			string original = Original.BaseOptionSet.ChartArea.Height;

			if (Loaded && value != Subject.BaseOptionSet.ChartArea.Height)
				Subject.BaseOptionSet.ChartArea.Height = value;

			areaHeightNumericUpDown.BackColor = Subject.BaseOptionSet.ChartArea.Height == original ? default(Color) : ChangedValueColor;
		}
	}
}
