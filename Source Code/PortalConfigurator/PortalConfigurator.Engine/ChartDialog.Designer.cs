namespace PortalConfigurator
{
	partial class ChartDialog
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChartDialog));
			this.labelGroupBox = new System.Windows.Forms.GroupBox();
			this.yAxisFormatLabel = new System.Windows.Forms.Label();
			this.yAxisFormatComboBox = new System.Windows.Forms.ComboBox();
			this.yAxisMaxLabel = new System.Windows.Forms.Label();
			this.yAxisMaxNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.yAxisMinLabel = new System.Windows.Forms.Label();
			this.yAxisMinNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.yAxisLabelLabel = new System.Windows.Forms.Label();
			this.yAxisLabelTextBox = new System.Windows.Forms.TextBox();
			this.xAxisLabelLabel = new System.Windows.Forms.Label();
			this.xAxisLabelTextBox = new System.Windows.Forms.TextBox();
			this.chartIdTextBox = new System.Windows.Forms.TextBox();
			this.chartIdLabel = new System.Windows.Forms.Label();
			this.chartTypeComboBox = new System.Windows.Forms.ComboBox();
			this.chartTypeLabel = new System.Windows.Forms.Label();
			this.maxSetsNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.maxSetsLabel = new System.Windows.Forms.Label();
			this.chartWidthNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.chartWidthLabel = new System.Windows.Forms.Label();
			this.chartHeightNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.chartHeightLabel = new System.Windows.Forms.Label();
			this.multiAllowedCheckBox = new System.Windows.Forms.CheckBox();
			this.showColumnsDataGridView = new System.Windows.Forms.DataGridView();
			this.showColumnNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.showColumnColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.chartAreaGroupBox = new System.Windows.Forms.GroupBox();
			this.areaHeightLabel = new System.Windows.Forms.Label();
			this.areaHeightNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.areaWidthLabel = new System.Windows.Forms.Label();
			this.areaWidthNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.topLabel = new System.Windows.Forms.Label();
			this.topNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.leftLabel = new System.Windows.Forms.Label();
			this.leftNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.valueColumnsGroupBox = new System.Windows.Forms.GroupBox();
			this.doneButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.chartIdErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.warningProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.labelGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.yAxisMaxNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.yAxisMinNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.maxSetsNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.chartWidthNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.chartHeightNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.showColumnsDataGridView)).BeginInit();
			this.chartAreaGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.areaHeightNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.areaWidthNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.topNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.leftNumericUpDown)).BeginInit();
			this.valueColumnsGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.chartIdErrorProvider)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.warningProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// labelGroupBox
			// 
			this.labelGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelGroupBox.Controls.Add(this.yAxisFormatLabel);
			this.labelGroupBox.Controls.Add(this.yAxisFormatComboBox);
			this.labelGroupBox.Controls.Add(this.yAxisMaxLabel);
			this.labelGroupBox.Controls.Add(this.yAxisMaxNumericUpDown);
			this.labelGroupBox.Controls.Add(this.yAxisMinLabel);
			this.labelGroupBox.Controls.Add(this.yAxisMinNumericUpDown);
			this.labelGroupBox.Controls.Add(this.yAxisLabelLabel);
			this.labelGroupBox.Controls.Add(this.yAxisLabelTextBox);
			this.labelGroupBox.Controls.Add(this.xAxisLabelLabel);
			this.labelGroupBox.Controls.Add(this.xAxisLabelTextBox);
			this.labelGroupBox.Location = new System.Drawing.Point(218, 90);
			this.labelGroupBox.Name = "labelGroupBox";
			this.labelGroupBox.Size = new System.Drawing.Size(219, 176);
			this.labelGroupBox.TabIndex = 12;
			this.labelGroupBox.TabStop = false;
			this.labelGroupBox.Text = "Label";
			// 
			// yAxisFormatLabel
			// 
			this.yAxisFormatLabel.AutoSize = true;
			this.warningProvider.SetError(this.yAxisFormatLabel, "Test");
			this.yAxisFormatLabel.Location = new System.Drawing.Point(3, 152);
			this.yAxisFormatLabel.Name = "yAxisFormatLabel";
			this.yAxisFormatLabel.Size = new System.Drawing.Size(71, 13);
			this.yAxisFormatLabel.TabIndex = 9;
			this.yAxisFormatLabel.Text = "Y Axis Format";
			// 
			// yAxisFormatComboBox
			// 
			this.yAxisFormatComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.yAxisFormatComboBox.FormattingEnabled = true;
			this.yAxisFormatComboBox.Location = new System.Drawing.Point(95, 149);
			this.yAxisFormatComboBox.Name = "yAxisFormatComboBox";
			this.yAxisFormatComboBox.Size = new System.Drawing.Size(118, 21);
			this.yAxisFormatComboBox.TabIndex = 8;
			this.yAxisFormatComboBox.SelectedIndexChanged += new System.EventHandler(this.yAxisFormatComboBox_SelectedIndexChanged);
			// 
			// yAxisMaxLabel
			// 
			this.yAxisMaxLabel.AutoSize = true;
			this.warningProvider.SetError(this.yAxisMaxLabel, "Test");
			this.yAxisMaxLabel.Location = new System.Drawing.Point(3, 125);
			this.yAxisMaxLabel.Name = "yAxisMaxLabel";
			this.yAxisMaxLabel.Size = new System.Drawing.Size(83, 13);
			this.yAxisMaxLabel.TabIndex = 7;
			this.yAxisMaxLabel.Text = "Y Axis Maximum";
			// 
			// yAxisMaxNumericUpDown
			// 
			this.yAxisMaxNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.yAxisMaxNumericUpDown.DecimalPlaces = 1;
			this.yAxisMaxNumericUpDown.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.yAxisMaxNumericUpDown.Location = new System.Drawing.Point(107, 123);
			this.yAxisMaxNumericUpDown.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
			this.yAxisMaxNumericUpDown.Name = "yAxisMaxNumericUpDown";
			this.yAxisMaxNumericUpDown.Size = new System.Drawing.Size(106, 20);
			this.yAxisMaxNumericUpDown.TabIndex = 6;
			this.yAxisMaxNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.yAxisMaxNumericUpDown.ThousandsSeparator = true;
			this.yAxisMaxNumericUpDown.ValueChanged += new System.EventHandler(this.yAxisMaxNumericUpDown_ValueChanged);
			// 
			// yAxisMinLabel
			// 
			this.yAxisMinLabel.AutoSize = true;
			this.warningProvider.SetError(this.yAxisMinLabel, "Test");
			this.yAxisMinLabel.Location = new System.Drawing.Point(3, 100);
			this.yAxisMinLabel.Name = "yAxisMinLabel";
			this.yAxisMinLabel.Size = new System.Drawing.Size(80, 13);
			this.yAxisMinLabel.TabIndex = 5;
			this.yAxisMinLabel.Text = "Y Axis Minimum";
			// 
			// yAxisMinNumericUpDown
			// 
			this.yAxisMinNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.yAxisMinNumericUpDown.DecimalPlaces = 1;
			this.yAxisMinNumericUpDown.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.yAxisMinNumericUpDown.Location = new System.Drawing.Point(107, 97);
			this.yAxisMinNumericUpDown.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
			this.yAxisMinNumericUpDown.Name = "yAxisMinNumericUpDown";
			this.yAxisMinNumericUpDown.Size = new System.Drawing.Size(106, 20);
			this.yAxisMinNumericUpDown.TabIndex = 4;
			this.yAxisMinNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.yAxisMinNumericUpDown.ThousandsSeparator = true;
			this.yAxisMinNumericUpDown.ValueChanged += new System.EventHandler(this.yAxisMinNumericUpDown_ValueChanged);
			// 
			// yAxisLabelLabel
			// 
			this.yAxisLabelLabel.AutoSize = true;
			this.warningProvider.SetError(this.yAxisLabelLabel, "Test");
			this.yAxisLabelLabel.Location = new System.Drawing.Point(3, 55);
			this.yAxisLabelLabel.Name = "yAxisLabelLabel";
			this.yAxisLabelLabel.Size = new System.Drawing.Size(65, 13);
			this.yAxisLabelLabel.TabIndex = 3;
			this.yAxisLabelLabel.Text = "Y Axis Label";
			// 
			// yAxisLabelTextBox
			// 
			this.yAxisLabelTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.yAxisLabelTextBox.Location = new System.Drawing.Point(6, 71);
			this.yAxisLabelTextBox.Name = "yAxisLabelTextBox";
			this.yAxisLabelTextBox.Size = new System.Drawing.Size(207, 20);
			this.yAxisLabelTextBox.TabIndex = 2;
			this.yAxisLabelTextBox.TextChanged += new System.EventHandler(this.yAxisLabelTextBox_TextChanged);
			// 
			// xAxisLabelLabel
			// 
			this.xAxisLabelLabel.AutoSize = true;
			this.warningProvider.SetError(this.xAxisLabelLabel, "Test");
			this.xAxisLabelLabel.Location = new System.Drawing.Point(3, 16);
			this.xAxisLabelLabel.Name = "xAxisLabelLabel";
			this.xAxisLabelLabel.Size = new System.Drawing.Size(65, 13);
			this.xAxisLabelLabel.TabIndex = 1;
			this.xAxisLabelLabel.Text = "X Axis Label";
			// 
			// xAxisLabelTextBox
			// 
			this.xAxisLabelTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.xAxisLabelTextBox.Location = new System.Drawing.Point(6, 32);
			this.xAxisLabelTextBox.Name = "xAxisLabelTextBox";
			this.xAxisLabelTextBox.Size = new System.Drawing.Size(207, 20);
			this.xAxisLabelTextBox.TabIndex = 0;
			this.xAxisLabelTextBox.TextChanged += new System.EventHandler(this.xAxisLabelTextBox_TextChanged);
			// 
			// chartIdTextBox
			// 
			this.chartIdTextBox.Location = new System.Drawing.Point(12, 25);
			this.chartIdTextBox.Name = "chartIdTextBox";
			this.chartIdTextBox.Size = new System.Drawing.Size(121, 20);
			this.chartIdTextBox.TabIndex = 0;
			this.chartIdTextBox.TextChanged += new System.EventHandler(this.chartIdTextBox_TextChanged);
			// 
			// chartIdLabel
			// 
			this.chartIdLabel.AutoSize = true;
			this.chartIdErrorProvider.SetError(this.chartIdLabel, "Test");
			this.chartIdLabel.Location = new System.Drawing.Point(9, 9);
			this.chartIdLabel.Name = "chartIdLabel";
			this.chartIdLabel.Size = new System.Drawing.Size(46, 13);
			this.chartIdLabel.TabIndex = 1;
			this.chartIdLabel.Text = "Chart ID";
			// 
			// chartTypeComboBox
			// 
			this.chartTypeComboBox.FormattingEnabled = true;
			this.chartTypeComboBox.Location = new System.Drawing.Point(139, 25);
			this.chartTypeComboBox.Name = "chartTypeComboBox";
			this.chartTypeComboBox.Size = new System.Drawing.Size(121, 21);
			this.chartTypeComboBox.TabIndex = 2;
			this.chartTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.chartTypeComboBox_SelectedIndexChanged);
			// 
			// chartTypeLabel
			// 
			this.chartTypeLabel.AutoSize = true;
			this.warningProvider.SetError(this.chartTypeLabel, "Test");
			this.chartTypeLabel.Location = new System.Drawing.Point(136, 9);
			this.chartTypeLabel.Name = "chartTypeLabel";
			this.chartTypeLabel.Size = new System.Drawing.Size(31, 13);
			this.chartTypeLabel.TabIndex = 3;
			this.chartTypeLabel.Text = "Type";
			// 
			// maxSetsNumericUpDown
			// 
			this.maxSetsNumericUpDown.Location = new System.Drawing.Point(266, 25);
			this.maxSetsNumericUpDown.Name = "maxSetsNumericUpDown";
			this.maxSetsNumericUpDown.Size = new System.Drawing.Size(121, 20);
			this.maxSetsNumericUpDown.TabIndex = 4;
			this.maxSetsNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.maxSetsNumericUpDown.ValueChanged += new System.EventHandler(this.maxSetsNumericUpDown_ValueChanged);
			// 
			// maxSetsLabel
			// 
			this.maxSetsLabel.AutoSize = true;
			this.maxSetsLabel.Location = new System.Drawing.Point(263, 9);
			this.maxSetsLabel.Name = "maxSetsLabel";
			this.maxSetsLabel.Size = new System.Drawing.Size(75, 13);
			this.maxSetsLabel.TabIndex = 5;
			this.maxSetsLabel.Text = "Maximum Sets";
			// 
			// chartWidthNumericUpDown
			// 
			this.chartWidthNumericUpDown.Location = new System.Drawing.Point(12, 64);
			this.chartWidthNumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.chartWidthNumericUpDown.Name = "chartWidthNumericUpDown";
			this.chartWidthNumericUpDown.Size = new System.Drawing.Size(121, 20);
			this.chartWidthNumericUpDown.TabIndex = 6;
			this.chartWidthNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.chartWidthNumericUpDown.ValueChanged += new System.EventHandler(this.chartWidthNumericUpDown_ValueChanged);
			// 
			// chartWidthLabel
			// 
			this.chartWidthLabel.AutoSize = true;
			this.chartWidthLabel.Location = new System.Drawing.Point(9, 48);
			this.chartWidthLabel.Name = "chartWidthLabel";
			this.chartWidthLabel.Size = new System.Drawing.Size(63, 13);
			this.chartWidthLabel.TabIndex = 7;
			this.chartWidthLabel.Text = "Chart Width";
			// 
			// chartHeightNumericUpDown
			// 
			this.chartHeightNumericUpDown.Location = new System.Drawing.Point(139, 64);
			this.chartHeightNumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.chartHeightNumericUpDown.Name = "chartHeightNumericUpDown";
			this.chartHeightNumericUpDown.Size = new System.Drawing.Size(121, 20);
			this.chartHeightNumericUpDown.TabIndex = 8;
			this.chartHeightNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.chartHeightNumericUpDown.ValueChanged += new System.EventHandler(this.chartHeightNumericUpDown_ValueChanged);
			// 
			// chartHeightLabel
			// 
			this.chartHeightLabel.AutoSize = true;
			this.chartHeightLabel.Location = new System.Drawing.Point(136, 48);
			this.chartHeightLabel.Name = "chartHeightLabel";
			this.chartHeightLabel.Size = new System.Drawing.Size(66, 13);
			this.chartHeightLabel.TabIndex = 9;
			this.chartHeightLabel.Text = "Chart Height";
			// 
			// multiAllowedCheckBox
			// 
			this.multiAllowedCheckBox.AutoSize = true;
			this.multiAllowedCheckBox.Location = new System.Drawing.Point(266, 65);
			this.multiAllowedCheckBox.Name = "multiAllowedCheckBox";
			this.multiAllowedCheckBox.Size = new System.Drawing.Size(88, 17);
			this.multiAllowedCheckBox.TabIndex = 10;
			this.multiAllowedCheckBox.Text = "Multi Allowed";
			this.multiAllowedCheckBox.UseVisualStyleBackColor = true;
			this.multiAllowedCheckBox.CheckedChanged += new System.EventHandler(this.multiAllowedCheckBox_CheckedChanged);
			// 
			// showColumnsDataGridView
			// 
			this.showColumnsDataGridView.AllowUserToAddRows = false;
			this.showColumnsDataGridView.AllowUserToDeleteRows = false;
			this.showColumnsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.showColumnsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.showColumnsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.showColumnNameColumn,
            this.showColumnColumn});
			this.showColumnsDataGridView.Location = new System.Drawing.Point(6, 19);
			this.showColumnsDataGridView.Name = "showColumnsDataGridView";
			this.showColumnsDataGridView.RowHeadersVisible = false;
			this.showColumnsDataGridView.Size = new System.Drawing.Size(188, 148);
			this.showColumnsDataGridView.TabIndex = 45;
			this.showColumnsDataGridView.TabStop = false;
			this.showColumnsDataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.showColumnsDataGridView_CellValueChanged);
			this.showColumnsDataGridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.showColumnsDataGridView_CurrentCellDirtyStateChanged);
			// 
			// showColumnNameColumn
			// 
			this.showColumnNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.showColumnNameColumn.HeaderText = "Column";
			this.showColumnNameColumn.Name = "showColumnNameColumn";
			this.showColumnNameColumn.ReadOnly = true;
			this.showColumnNameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// showColumnColumn
			// 
			this.showColumnColumn.HeaderText = "Show";
			this.showColumnColumn.Name = "showColumnColumn";
			this.showColumnColumn.Width = 50;
			// 
			// chartAreaGroupBox
			// 
			this.chartAreaGroupBox.Controls.Add(this.areaHeightLabel);
			this.chartAreaGroupBox.Controls.Add(this.areaHeightNumericUpDown);
			this.chartAreaGroupBox.Controls.Add(this.areaWidthLabel);
			this.chartAreaGroupBox.Controls.Add(this.areaWidthNumericUpDown);
			this.chartAreaGroupBox.Controls.Add(this.topLabel);
			this.chartAreaGroupBox.Controls.Add(this.topNumericUpDown);
			this.chartAreaGroupBox.Controls.Add(this.leftLabel);
			this.chartAreaGroupBox.Controls.Add(this.leftNumericUpDown);
			this.chartAreaGroupBox.Location = new System.Drawing.Point(12, 272);
			this.chartAreaGroupBox.Name = "chartAreaGroupBox";
			this.chartAreaGroupBox.Size = new System.Drawing.Size(344, 73);
			this.chartAreaGroupBox.TabIndex = 13;
			this.chartAreaGroupBox.TabStop = false;
			this.chartAreaGroupBox.Text = "Chart Area";
			// 
			// areaHeightLabel
			// 
			this.areaHeightLabel.AutoSize = true;
			this.areaHeightLabel.Location = new System.Drawing.Point(185, 48);
			this.areaHeightLabel.Name = "areaHeightLabel";
			this.areaHeightLabel.Size = new System.Drawing.Size(49, 13);
			this.areaHeightLabel.TabIndex = 7;
			this.areaHeightLabel.Text = "Height %";
			// 
			// areaHeightNumericUpDown
			// 
			this.areaHeightNumericUpDown.Location = new System.Drawing.Point(240, 45);
			this.areaHeightNumericUpDown.Name = "areaHeightNumericUpDown";
			this.areaHeightNumericUpDown.Size = new System.Drawing.Size(98, 20);
			this.areaHeightNumericUpDown.TabIndex = 6;
			this.areaHeightNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.areaHeightNumericUpDown.ValueChanged += new System.EventHandler(this.areaHeightNumericUpDown_ValueChanged);
			// 
			// areaWidthLabel
			// 
			this.areaWidthLabel.AutoSize = true;
			this.areaWidthLabel.Location = new System.Drawing.Point(185, 21);
			this.areaWidthLabel.Name = "areaWidthLabel";
			this.areaWidthLabel.Size = new System.Drawing.Size(46, 13);
			this.areaWidthLabel.TabIndex = 5;
			this.areaWidthLabel.Text = "Width %";
			// 
			// areaWidthNumericUpDown
			// 
			this.areaWidthNumericUpDown.Location = new System.Drawing.Point(240, 19);
			this.areaWidthNumericUpDown.Name = "areaWidthNumericUpDown";
			this.areaWidthNumericUpDown.Size = new System.Drawing.Size(98, 20);
			this.areaWidthNumericUpDown.TabIndex = 4;
			this.areaWidthNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.areaWidthNumericUpDown.ValueChanged += new System.EventHandler(this.areaWidthNumericUpDown_ValueChanged);
			// 
			// topLabel
			// 
			this.topLabel.AutoSize = true;
			this.topLabel.Location = new System.Drawing.Point(6, 47);
			this.topLabel.Name = "topLabel";
			this.topLabel.Size = new System.Drawing.Size(68, 13);
			this.topLabel.TabIndex = 3;
			this.topLabel.Text = "Top Spacing";
			// 
			// topNumericUpDown
			// 
			this.topNumericUpDown.Location = new System.Drawing.Point(80, 45);
			this.topNumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.topNumericUpDown.Name = "topNumericUpDown";
			this.topNumericUpDown.Size = new System.Drawing.Size(99, 20);
			this.topNumericUpDown.TabIndex = 2;
			this.topNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.topNumericUpDown.ValueChanged += new System.EventHandler(this.topNumericUpDown_ValueChanged);
			// 
			// leftLabel
			// 
			this.leftLabel.AutoSize = true;
			this.leftLabel.Location = new System.Drawing.Point(7, 21);
			this.leftLabel.Name = "leftLabel";
			this.leftLabel.Size = new System.Drawing.Size(67, 13);
			this.leftLabel.TabIndex = 1;
			this.leftLabel.Text = "Left Spacing";
			// 
			// leftNumericUpDown
			// 
			this.leftNumericUpDown.Location = new System.Drawing.Point(80, 19);
			this.leftNumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.leftNumericUpDown.Name = "leftNumericUpDown";
			this.leftNumericUpDown.Size = new System.Drawing.Size(99, 20);
			this.leftNumericUpDown.TabIndex = 0;
			this.leftNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.leftNumericUpDown.ValueChanged += new System.EventHandler(this.leftNumericUpDown_ValueChanged);
			// 
			// valueColumnsGroupBox
			// 
			this.valueColumnsGroupBox.Controls.Add(this.showColumnsDataGridView);
			this.valueColumnsGroupBox.Location = new System.Drawing.Point(12, 90);
			this.valueColumnsGroupBox.Name = "valueColumnsGroupBox";
			this.valueColumnsGroupBox.Size = new System.Drawing.Size(200, 176);
			this.valueColumnsGroupBox.TabIndex = 11;
			this.valueColumnsGroupBox.TabStop = false;
			this.valueColumnsGroupBox.Text = "Value Columns";
			// 
			// doneButton
			// 
			this.doneButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.doneButton.Location = new System.Drawing.Point(362, 288);
			this.doneButton.Name = "doneButton";
			this.doneButton.Size = new System.Drawing.Size(75, 23);
			this.doneButton.TabIndex = 14;
			this.doneButton.Text = "Done";
			this.doneButton.UseVisualStyleBackColor = true;
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(362, 317);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 15;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// chartIdErrorProvider
			// 
			this.chartIdErrorProvider.ContainerControl = this;
			// 
			// warningProvider
			// 
			this.warningProvider.ContainerControl = this;
			this.warningProvider.Icon = ((System.Drawing.Icon)(resources.GetObject("warningProvider.Icon")));
			// 
			// ChartDialog
			// 
			this.AcceptButton = this.doneButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(449, 358);
			this.ControlBox = false;
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.doneButton);
			this.Controls.Add(this.valueColumnsGroupBox);
			this.Controls.Add(this.chartAreaGroupBox);
			this.Controls.Add(this.multiAllowedCheckBox);
			this.Controls.Add(this.chartHeightLabel);
			this.Controls.Add(this.chartHeightNumericUpDown);
			this.Controls.Add(this.chartWidthLabel);
			this.Controls.Add(this.chartWidthNumericUpDown);
			this.Controls.Add(this.maxSetsLabel);
			this.Controls.Add(this.maxSetsNumericUpDown);
			this.Controls.Add(this.chartTypeLabel);
			this.Controls.Add(this.chartTypeComboBox);
			this.Controls.Add(this.chartIdLabel);
			this.Controls.Add(this.chartIdTextBox);
			this.Controls.Add(this.labelGroupBox);
			this.MaximizeBox = false;
			this.Name = "ChartDialog";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Chart Options";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.ChartDialog_Load);
			this.labelGroupBox.ResumeLayout(false);
			this.labelGroupBox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.yAxisMaxNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.yAxisMinNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.maxSetsNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.chartWidthNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.chartHeightNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.showColumnsDataGridView)).EndInit();
			this.chartAreaGroupBox.ResumeLayout(false);
			this.chartAreaGroupBox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.areaHeightNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.areaWidthNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.topNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.leftNumericUpDown)).EndInit();
			this.valueColumnsGroupBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.chartIdErrorProvider)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.warningProvider)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox labelGroupBox;
		private System.Windows.Forms.Label yAxisFormatLabel;
		private System.Windows.Forms.ComboBox yAxisFormatComboBox;
		private System.Windows.Forms.Label yAxisMaxLabel;
		private System.Windows.Forms.NumericUpDown yAxisMaxNumericUpDown;
		private System.Windows.Forms.Label yAxisMinLabel;
		private System.Windows.Forms.NumericUpDown yAxisMinNumericUpDown;
		private System.Windows.Forms.Label yAxisLabelLabel;
		private System.Windows.Forms.TextBox yAxisLabelTextBox;
		private System.Windows.Forms.Label xAxisLabelLabel;
		private System.Windows.Forms.TextBox xAxisLabelTextBox;
		private System.Windows.Forms.TextBox chartIdTextBox;
		private System.Windows.Forms.Label chartIdLabel;
		private System.Windows.Forms.ComboBox chartTypeComboBox;
		private System.Windows.Forms.Label chartTypeLabel;
		private System.Windows.Forms.NumericUpDown maxSetsNumericUpDown;
		private System.Windows.Forms.Label maxSetsLabel;
		private System.Windows.Forms.NumericUpDown chartWidthNumericUpDown;
		private System.Windows.Forms.Label chartWidthLabel;
		private System.Windows.Forms.NumericUpDown chartHeightNumericUpDown;
		private System.Windows.Forms.Label chartHeightLabel;
		private System.Windows.Forms.CheckBox multiAllowedCheckBox;
		private System.Windows.Forms.DataGridView showColumnsDataGridView;
		private System.Windows.Forms.GroupBox chartAreaGroupBox;
		private System.Windows.Forms.Label areaHeightLabel;
		private System.Windows.Forms.NumericUpDown areaHeightNumericUpDown;
		private System.Windows.Forms.Label areaWidthLabel;
		private System.Windows.Forms.NumericUpDown areaWidthNumericUpDown;
		private System.Windows.Forms.Label topLabel;
		private System.Windows.Forms.NumericUpDown topNumericUpDown;
		private System.Windows.Forms.Label leftLabel;
		private System.Windows.Forms.NumericUpDown leftNumericUpDown;
		private System.Windows.Forms.GroupBox valueColumnsGroupBox;
		private System.Windows.Forms.Button doneButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.DataGridViewTextBoxColumn showColumnNameColumn;
		private System.Windows.Forms.DataGridViewCheckBoxColumn showColumnColumn;
		private System.Windows.Forms.ErrorProvider chartIdErrorProvider;
		private System.Windows.Forms.ErrorProvider warningProvider;
	}
}