using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Framework
{
    public class MeasurementFile : JsonFile, IConfigurationBase
    {
		public string BaseMeasure { get; set; }
		public string Title { get; set; }
		public string Dropdown { get; set; }
		public int? Order { get; set; }
		public string Summary { get; set; }
		public string Filter { get; set; }
		public Transform Transform { get; set; }
		public List<string> Controls { get; set; }
		public List<string> Parameters { get; set; }
		public List<string> DateParameters { get; set; }
		public List<string> RequiredParameters { get; set; }
		public bool? ShowAllOthers { get; set; }
		public int? MaxChecked { get; set; }
		public bool? MutexAllOthers { get; set; }
		public List<int> ReturnRowHeaders { get; set; }
		public List<int> ReturnRowDateHeaders { get; set; }
		public List<int> ReturnRowControlHeaders { get; set; }
		public List<string> HeaderNames { get; set; }
		public int? ReturnRowStart { get; set; }
		public HideRow HideRow { get; set; }
		public int? ColumnClusterSize { get; set; }
		public ChartType ChartType { get; set; }
		public Label Label { get; set; }
		public List<NumberFormat> NumberFormats { get; set; }
		public List<ChartInfo> Charts { get; set; }
		public Multicharts Multicharts { get; set; }
		public bool IsEmpty { get { return CheckForEmpty(); } }
		public List<string> TableColumns { get; private set; }
		public int DatabaseParameterCount { get; private set; }

		public string Breadcrumb
		{
			get
			{
				string breadcrumb = String.Empty;

				if (!String.IsNullOrEmpty(this.FilePath))
				{
					FileInfo file = new FileInfo(this.FilePath);
					DirectoryInfo dir = file.Directory;
					breadcrumb = file.Name;

					while (!dir.Name.Equals("Table", StringComparison.OrdinalIgnoreCase) && !dir.Name.Equals(dir.Root.Name, StringComparison.OrdinalIgnoreCase))
					{
						breadcrumb = String.Concat(dir.Name, "\\", breadcrumb);
						dir = dir.Parent;
					}
				}

				return breadcrumb;
			}
		}

		private string _table;
		public string Table
		{
			get
			{
				if (_table != Transform.Table)
				{
					Transform.Table = _table;
					Transform.SynchedWithMeasure = true;
				}

				return _table;
			}
			set
			{
				_table = value;
				Transform.Table = value;
				Transform.SynchedWithMeasure = true;
				_measureGridRowsAreUpdated = false;

				if (String.IsNullOrEmpty(_table))
				{
					TableColumns.Clear();
					Transform.TableColumns = TableColumns;
					DatabaseParameterCount = 0;
				}
				else
					ResetTableColumnOrdinals();
			}
		}

		private List<MeasureGridRow> _measureGridRows;
		public List<MeasureGridRow> MeasureGridRows
		{
			get
			{
				if (!_measureGridRowsAreUpdated)
					RefreshMeasureGridRows();

				return _measureGridRows;
			}
			set
			{
				_measureGridRows = value;
				UpdatePropertiesFromMeasureGridRows();
			}
		}

		private bool _measureGridRowsAreUpdated;
		public bool MeasureGridRowsAreUpdated
		{
			get { return _measureGridRowsAreUpdated; }
			set
			{
				_measureGridRowsAreUpdated = value;

				if (!value)
					_measureGridRows.Clear();
			}
		}

		public MeasurementFile()
			: this(String.Empty)
		{ }

		public MeasurementFile(string filePath)
			: this(filePath, String.Empty, String.Empty, String.Empty, String.Empty, (int?)null, String.Empty, String.Empty, new Transform(), new List<string>(),
			new List<string>(), new List<string>(), new List<string>(), (bool?)null, (int?)null, (bool?)null, new List<int>(), new List<int>(), new List<int>(),
			new List<string>(), (int?)null, HideRow.NoHideRow, (int?)null, ChartType.NoChartType, new Label(), new List<NumberFormat>(), new List<ChartInfo>(),
			new Multicharts())
		{ }

		public MeasurementFile(string filePath, string table, string baseMeasure, string title, string dropdown, int? order, string summary, string filter,
			Transform transform, List<string> controls, List<string> parameters, List<string> dateParameters, List<string> requiredParameters, bool? showAllOthers,
			int? maxChecked, bool? mutexAllOthers, List<int> returnRowHeaders, List<int> returnRowDateHeaders, List<int> returnRowControlHeaders,
			List<string> headerNames, int? returnRowStart, HideRow hideRow, int? columnClusterSize, ChartType chartType, Label label,
			List<NumberFormat> numberFormats, List<ChartInfo> charts, Multicharts multicharts)
			: base(filePath)
		{
			this._measureGridRows = new List<MeasureGridRow>();
			this.TableColumns = new List<string>();
			this._measureGridRowsAreUpdated = false;
			this.Transform = transform;
			this.Table = table;
			this.BaseMeasure = baseMeasure;
			this.Title = title;
			this.Dropdown = dropdown;
			this.Order = order;
			this.Summary = summary;
			this.Filter = filter;
			this.Controls = controls;
			this.Parameters = parameters;
			this.DateParameters = dateParameters;
			this.RequiredParameters = requiredParameters;
			this.ShowAllOthers = showAllOthers;
			this.MaxChecked = maxChecked;
			this.MutexAllOthers = mutexAllOthers;
			this.ReturnRowHeaders = returnRowHeaders;
			this.ReturnRowDateHeaders = returnRowDateHeaders;
			this.ReturnRowControlHeaders = returnRowControlHeaders;
			this.HeaderNames = headerNames;
			this.ReturnRowStart = returnRowStart;
			this.HideRow = hideRow;
			this.ColumnClusterSize = columnClusterSize;
			this.ChartType = chartType;
			this.Label = label;
			this.NumberFormats = numberFormats;
			this.Charts = charts;
			this.Multicharts = multicharts;
		}

		private bool CheckForEmpty()
		{
			return (String.IsNullOrEmpty(Table) && String.IsNullOrEmpty(BaseMeasure) && String.IsNullOrEmpty(Title) && Order == null &&
				String.IsNullOrEmpty(Dropdown) && Order == null && String.IsNullOrEmpty(Summary) && String.IsNullOrEmpty(Filter) && Transform.IsEmpty &&
				Controls.Count == 0 && Parameters.Count == 0 && DateParameters.Count == 0 && RequiredParameters.Count == 0 && ShowAllOthers == null &&
				MaxChecked == null && MutexAllOthers == null && ReturnRowHeaders.Count == 0 && ReturnRowDateHeaders.Count == 0 &&
				ReturnRowControlHeaders.Count == 0 && HeaderNames.Count == 0 && ReturnRowStart == null && HideRow == HideRow.NoHideRow &&
				ColumnClusterSize == null && ChartType == ChartType.NoChartType && Label.IsEmpty && NumberFormats.Count(p => !p.IsEmpty) == 0 &&
				Charts.Count(p => !p.IsEmpty) == 0 && Multicharts.IsEmpty);
		}

		private void RefreshMeasureGridRows()
		{
			if (Transform.IsEmpty && ReturnRowStart != null)
			{
				int returnColumnIndex = ReturnRowStart ?? (TableColumns.Count - 1);
				int returnClusterEndIndex = returnColumnIndex + (ColumnClusterSize ?? 1);

				for (int i = returnColumnIndex; i < returnClusterEndIndex; i++)
				{
					if (i >= TableColumns.Count)
					{
						ColumnClusterSize = i - returnColumnIndex < 2 ? (int?)null : i - returnColumnIndex;
						string message = String.Format("{0} value column(s) could not be found in the procedure and have been removed.", returnClusterEndIndex - i);
						MessageBox.Show(message);
						break;
					}

					Transform.ValueFields.Add(TableColumns[i]);
				}
			}

			foreach (var item in Transform.ValueFields)
			{
				MeasureGridRow row = new MeasureGridRow(Transform.DateField);
				row.IsValueField = true;
				row.ColumnName = item;
				row.TableOrdinal = TableColumns.FindIndex(p => p == item);
				_measureGridRows.Add(row);
			}

			foreach (var item in Transform.RemoveFields)
			{
				int gridRowOrdinal = _measureGridRows.FindIndex(p => p.ColumnName == item);

				if (gridRowOrdinal == -1)
				{
					MeasureGridRow row = new MeasureGridRow(Transform.DateField);
					row.IsRemoveField = true;
					row.ColumnName = item;
					row.TableOrdinal = TableColumns.FindIndex(p => p == item);
					_measureGridRows.Add(row);
				}
				else
					_measureGridRows[gridRowOrdinal].IsRemoveField = true;
			}

			if (Transform.HasDateRow)
			{
				MeasureGridRow row = new MeasureGridRow(Transform.DateField);
				row.HeaderType = HeaderType.DateColumn;
				row.TableOrdinal = TableColumns.FindIndex(p => p == row.ColumnName);
				_measureGridRows.Add(row);
			}

			if (!String.IsNullOrEmpty(Transform.DateField) || Transform.RemoveFields.Count != 0)
				ResetTableColumnOrdinals();

			if (Transform.HasDateRow)
			{
				MeasureGridRow row = _measureGridRows.FirstOrDefault(p => p.HeaderType == HeaderType.DateColumn) ?? new MeasureGridRow(Transform.DateField);
				row.IsReturnRow = ReturnRowHeaders.Contains(row.TableOrdinal);
				row.IsReturnRowDate = ReturnRowDateHeaders.Contains(row.TableOrdinal);
				row.IsReturnRowControl = ReturnRowControlHeaders.Contains(row.TableOrdinal);
			}

			_measureGridRows = _measureGridRows.OrderBy(s => s.TableOrdinal).ToList<MeasureGridRow>();

			foreach (var item in Parameters)
			{
				MeasureGridRow row = new MeasureGridRow(Transform.DateField);
				row.HeaderText = HeaderNames.FirstOrDefault(p => p.EndsWith(item)) ?? String.Empty;

				if (String.IsNullOrEmpty(row.HeaderText))
					row.FilterParameter = item;
				
				if (row.ControlType != ControlType.Parameter)
					row.ControlType = ControlType.Parameter;
				
				row.IsDate = DateParameters.Contains(item);
				row.IsRequired = RequiredParameters.Contains(item);
				row.TableOrdinal = TableColumns.FindIndex(p => p == row.ColumnName);
				row.IsReturnRow = ReturnRowHeaders.Contains(row.TableOrdinal);
				row.IsReturnRowDate = ReturnRowDateHeaders.Contains(row.TableOrdinal);
				row.IsReturnRowControl = ReturnRowControlHeaders.Contains(row.TableOrdinal);
				_measureGridRows.Add(row);
			}

			foreach (var item in Controls)
			{
				int existingRow = _measureGridRows.FindIndex(p => p.FilterParameter == item);

				if (existingRow != -1)
					_measureGridRows[existingRow].ControlType = ControlType.Both;
				else
				{
					MeasureGridRow row = new MeasureGridRow(Transform.DateField);
					row.HeaderText = HeaderNames.FirstOrDefault(p => p.EndsWith(item)) ?? String.Empty;

					if (String.IsNullOrEmpty(row.HeaderText))
						row.FilterParameter = item;
					
					if (row.ControlType != ControlType.Control)
						row.ControlType = ControlType.Control;

					row.IsDate = DateParameters.Contains(row.FilterParameter);
					row.IsRequired = DateParameters.Contains(row.FilterParameter);
					row.TableOrdinal = TableColumns.FindIndex(p => p == row.ColumnName);
					row.IsReturnRow = ReturnRowHeaders.Contains(row.TableOrdinal);
					row.IsReturnRowDate = ReturnRowDateHeaders.Contains(row.TableOrdinal);
					row.IsReturnRowControl = ReturnRowControlHeaders.Contains(row.TableOrdinal);
					_measureGridRows.Add(row);
				}
			}

			foreach (var item in HeaderNames.Where(p => !p.Contains('*') && !p.Contains('=') && p != "&nbsp;"))
			{
				MeasureGridRow row = new MeasureGridRow(Transform.DateField);
				row.HeaderText = item;
				row.TableOrdinal = TableColumns.FindIndex(p => p == item);
				row.IsReturnRow = ReturnRowHeaders.Contains(row.TableOrdinal);
				row.IsReturnRowDate = ReturnRowDateHeaders.Contains(row.TableOrdinal);
				row.IsReturnRowControl = ReturnRowControlHeaders.Contains(row.TableOrdinal);
				_measureGridRows.Add(row);
			}

			_measureGridRowsAreUpdated = true;
		}

		public void RemoveUnfoundColumns()
		{
			if (MeasureGridRows.Count(p => !String.IsNullOrEmpty(p.ColumnName) && p.TableOrdinal == -1) != 0)
			{
				string message = "The following columns were not found in the stored procedure output\nand have been removed from the measure:\n";

				foreach (var item in MeasureGridRows.Where(p => p.TableOrdinal == -1 && !String.IsNullOrEmpty(p.ColumnName)))
					message = String.Concat(message, item.ColumnName, "\n");

				MessageBox.Show(message.Trim(), "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

				MeasureGridRows.RemoveAll(m => m.TableOrdinal == -1 && !String.IsNullOrEmpty(m.ColumnName));
				UpdatePropertiesFromMeasureGridRows();
				ResetTableColumnOrdinals();
			}
		}

		public void ResetTableColumnOrdinals()
		{
			TableColumns = Database.StoredProcedures[_table].Columns.Values.ToList<string>();

			if ((Transform.HasDateRow && TableColumns.Contains(Transform.DateField)) || Transform.RemoveFields.Count != 0)
			{
				List<string> columns = new List<string>();

				if (Transform.HasDateRow && TableColumns.Contains(Transform.DateField))
					columns.Add(Transform.DateField);

				foreach (var item in TableColumns.Where(p => p != Transform.DateField && !Transform.RemoveFields.Contains(p)))
					columns.Add(item);

				foreach (var item in TableColumns.Where(p => Transform.RemoveFields.Contains(p)))
					columns.Add(item);

				TableColumns = columns;
			}

			if (Transform.HasDateRow && TableColumns.Contains(Transform.DateField) && _measureGridRows.FindIndex(p => p.HeaderType == HeaderType.DateColumn) == -1)
			{
				MeasureGridRow dateColumn = new MeasureGridRow(Transform.DateField);
				dateColumn.HeaderType = HeaderType.DateColumn;
				_measureGridRows.Add(dateColumn);
			}

			if (_measureGridRows.Any(r => !String.IsNullOrEmpty(r.ColumnName) && r.TableOrdinal != TableColumns.FindIndex(tc => tc == r.ColumnName)))
			{
				foreach (var item in _measureGridRows.Where(p => !String.IsNullOrEmpty(p.ColumnName)))
				{
					item.DateColumn = Transform.DateField;
					int ordinal = TableColumns.FindIndex(p => p == item.ColumnName);

					if (item.TableOrdinal != ordinal)
						item.TableOrdinal = ordinal;

					if ((item.ColumnName == Transform.DateField && item.HeaderType != HeaderType.DateColumn) || item.TableOrdinal == -1)
						item.HeaderType = HeaderType.NotAHeader;
				}

				if (_measureGridRowsAreUpdated)
					UpdatePropertiesFromMeasureGridRows();
			}

			Transform.TableColumns = TableColumns;
			DatabaseParameterCount = Database.StoredProcedures[_table].Parameters.Count;
		}

		public void UpdatePropertiesFromMeasureGridRows()
		{
			Controls.Clear();
			Parameters.Clear();
			DateParameters.Clear();
			RequiredParameters.Clear();
			HeaderNames.Clear();
			ReturnRowHeaders.Clear();
			ReturnRowDateHeaders.Clear();
			ReturnRowControlHeaders.Clear();
			Transform.ValueFields.Clear();
			Transform.RemoveFields.Clear();

			foreach (var item in _measureGridRows)
			{
				if (item.IsValueField)
					Transform.ValueFields.Add(item.ColumnName);

				if (item.IsRemoveField)
					Transform.RemoveFields.Add(item.ColumnName);

				if (item.ControlType == ControlType.Control || item.ControlType == ControlType.Both)
					Controls.Add(item.FilterParameter);

				if (item.ControlType == ControlType.Parameter || item.ControlType == ControlType.Both)
					Parameters.Add(item.FilterParameter);

				if (item.IsDate)
					DateParameters.Add(item.FilterParameter);

				if (item.IsRequired)
					RequiredParameters.Add(item.FilterParameter);

				if (item.HeaderType != HeaderType.NotAHeader)
					HeaderNames.Add(item.HeaderText);

				if (item.IsReturnRow)
					ReturnRowHeaders.Add(item.TableOrdinal);

				if (item.IsReturnRowDate)
					ReturnRowDateHeaders.Add(item.TableOrdinal);

				if (item.IsReturnRowControl)
					ReturnRowControlHeaders.Add(item.TableOrdinal);
			}

			Transform.DateField = (_measureGridRows.FirstOrDefault(p => p.HeaderType == HeaderType.DateColumn) ?? new MeasureGridRow(String.Empty)).DateColumn;
			Transform.ValueFields = Transform.ValueFields.OrderBy(s => (_measureGridRows.FirstOrDefault(p => p.ColumnName == s) ??
				new MeasureGridRow(Transform.DateField)).TableOrdinal).ToList<string>();
			Transform.RemoveFields = Transform.RemoveFields.OrderBy(s => (_measureGridRows.FirstOrDefault(p => p.ColumnName == s) ??
				new MeasureGridRow(Transform.DateField)).TableOrdinal).ToList<string>();
			HeaderNames = HeaderNames.OrderBy(s => (_measureGridRows.FirstOrDefault(p => p.HeaderText == s) ??
				new MeasureGridRow(Transform.DateField)).TableOrdinal).ToList<string>();
			ReturnRowHeaders.Sort();
			ReturnRowDateHeaders.Sort();
			ReturnRowControlHeaders.Sort();
		}

		public JObject CompileJson()
		{
			JObject myJson = new JObject();
			int? returnRowStart = null;
			int? columnClusterSize = null;
			string firstValueColumnName = Transform.ValueFields.FirstOrDefault() ?? String.Empty;

			if (Transform.ValueFields.Count != 0 && TableColumns.Contains(firstValueColumnName))
			{
				returnRowStart = TableColumns.FindIndex(p => p == firstValueColumnName);
				columnClusterSize = Transform.ValueFields.Count;
			}

			ReturnRowStart = returnRowStart;
			ColumnClusterSize = columnClusterSize == 1 ? null : columnClusterSize;

			if (!String.IsNullOrEmpty(Table))
				myJson.Add("table", Table);

			if (!String.IsNullOrEmpty(BaseMeasure))
				myJson.Add("Base", BaseMeasure);

			if (!String.IsNullOrEmpty(Title))
				myJson.Add("title", Title);

			if (!String.IsNullOrEmpty(Dropdown))
				myJson.Add("dropdown", Dropdown);

			if (Order != null)
				myJson.Add("measurementOrder", Order);

			if (!String.IsNullOrEmpty(Summary))
				myJson.Add("summary", Summary);

			if (!String.IsNullOrEmpty(Filter))
				myJson.Add("filter", Filter);

			if (!Transform.IsEmpty && Transform.Function != Function.NoTransform)
				myJson.Add("transform", Transform.CompileJson());

			myJson.Add("controls", new JArray(Controls));
			myJson.Add("params", new JArray(Parameters));
			myJson.Add("dateParams", new JArray(DateParameters));
			myJson.Add("requiredParams", new JArray(RequiredParameters));

			if (ShowAllOthers != null)
				myJson.Add("showAllOthers", ShowAllOthers);

			if (MaxChecked != null)
				myJson.Add("maxChecked", MaxChecked);

			if (MutexAllOthers != null)
				myJson.Add("mutexAllOthers", MutexAllOthers);

			myJson.Add("returnRowHeaders", new JArray(ReturnRowHeaders));
			myJson.Add("returnRowDateHeaders", new JArray(ReturnRowDateHeaders));
			myJson.Add("returnRowControlHeaders", new JArray(ReturnRowControlHeaders));
			myJson.Add("headerNames", new JArray(HeaderNames));

			if (ReturnRowStart != null)
				myJson.Add("returnRowStart", ReturnRowStart);

			if (HideRow != HideRow.NoHideRow)
				myJson.Add("hideRow", Enums.GetString(HideRow));

			if (ColumnClusterSize != null)
				myJson.Add("columnClusterSize", ColumnClusterSize);

			if (ChartType != ChartType.NoChartType)
			{
				myJson.Add("chartType", Enums.GetString(ChartType));
				myJson.Add("labels", Label.CompileJson());
			}

			if (NumberFormats.Count == 1)
				myJson.Add("numberFormat", NumberFormats[0].CompileJson());
			else if (NumberFormats.Count != 0)
			{
				JArray numberFormats = new JArray();

				foreach (var numberFormat in NumberFormats.TakeWhile(p => !p.IsEmpty))
					numberFormats.Add(numberFormat.CompileJson());

				myJson.Add("numberFormat", numberFormats);
			}

			if (Charts.Count != 0)
			{
				JArray charts = new JArray();

				foreach (var chart in Charts)
					charts.Add(chart.CompileJson());
				
				myJson.Add("charts", charts);
			}

			if(!Multicharts.IsEmpty)
				myJson.Add("multicharts", Multicharts.CompileJson());

			base.MyJson = myJson;

			return myJson;
		}

		public override void ParseJson()
		{
			base.ReadFile();
			ParseFromJson();
		}

		private void ParseFromJson()
		{
			try
			{
				foreach (var property in base.MyJson.Properties())
				{
					switch (property.Name)
					{
						case "table":
							_table = Json.Parse(_table, property);
							break;
						case "Base":
							BaseMeasure = Json.Parse(BaseMeasure, property);
							break;
						case "title":
							Title = Json.Parse(Title, property);
							break;
						case "measurementOrder":
							Order = Json.Parse(Order, property);
							break;
						case "dropdown":
							Dropdown = Json.Parse(Dropdown, property);
							break;
						case "summary":
							Summary = Json.Parse(Summary, property);
							break;
						case "filter":
							Filter = Json.Parse(Filter, property);
							break;
						case "transform":
							Transform.ParseJson((JObject)property.Value);
							break;
						case "controls":
							Controls = Json.Parse(Controls, property);
							break;
						case "params":
							Parameters = Json.Parse(Parameters, property);
							break;
						case "dateParams":
							DateParameters = Json.Parse(DateParameters, property);
							break;
						case "requiredParams":
							RequiredParameters = Json.Parse(RequiredParameters, property);
							break;
						case "showAllOthers":
							ShowAllOthers = Json.Parse(ShowAllOthers, property);
							break;
						case "maxChecked":
							MaxChecked = Json.Parse(MaxChecked, property);
							break;
						case "mutexAllOthers":
							MutexAllOthers = Json.Parse(MutexAllOthers, property);
							break;
						case "returnRowHeaders":
							ReturnRowHeaders = Json.Parse(ReturnRowHeaders, property);
							break;
						case "returnRowDateHeaders":
							ReturnRowDateHeaders = Json.Parse(ReturnRowDateHeaders, property);
							break;
						case "returnRowControlHeaders":
							ReturnRowControlHeaders = Json.Parse(ReturnRowControlHeaders, property);
							break;
						case "headerNames":
							HeaderNames = Json.Parse(HeaderNames, property);
							break;
						case "returnRowStart":
							ReturnRowStart = Json.Parse(ReturnRowStart, property);
							break;
						case "hideRow":
							HideRow = Enums.GetHideRowEnum(Json.Parse(String.Empty, property));
							break;
						case "columnClusterSize":
							ColumnClusterSize = Json.Parse(ColumnClusterSize, property);
							break;
						case "chartType":
							ChartType = Enums.GetChartTypeEnum(Json.Parse(String.Empty, property));
							break;
						case "labels":
							Label.ParseJson((JObject)property.Value);
							break;
						case "numberFormat":
							if (property.Value.Type == JTokenType.Object)
							{
								NumberFormat numberFormat = new NumberFormat();
								numberFormat.ParseJson((JObject)property.Value);
								NumberFormats.Add(numberFormat);
							}
							else if (property.Value.Type == JTokenType.Array)
							{
								foreach (var token in property.Values())
								{
									NumberFormat numberFormat = new NumberFormat();
									numberFormat.ParseJson((JObject)token);
									NumberFormats.Add(numberFormat);
								}
							}
							break;
						case "charts":
							foreach (var token in property.Values())
							{
								ChartInfo chart = new ChartInfo();
								chart.ParseJson((JObject)token);
								Charts.Add(chart);
							}
							break;
						case "multicharts":
							Multicharts.ParseJson((JObject)property.Value);
							break;
						default:
							throw new UnknownJsonPropertyException(String.Format("The {0} property is not defined for a Measurement file.", property.Name));
					}
				}
			}
			catch (Exception)
			{
				Table = String.Empty;
				BaseMeasure = String.Empty;
				Title = String.Empty;
				Order = null;
				Dropdown = String.Empty;
				Summary = String.Empty;
				Filter = String.Empty;
				Transform = new Transform();
				Controls = new List<string>();
				Parameters = new List<string>();
				DateParameters = new List<string>();
				RequiredParameters = new List<string>();
				ShowAllOthers = null;
				MaxChecked = null;
				MutexAllOthers = null;
				ReturnRowHeaders = new List<int>();
				ReturnRowDateHeaders = new List<int>();
				ReturnRowControlHeaders = new List<int>();
				HeaderNames = new List<string>();
				ReturnRowStart = null;
				HideRow = HideRow.NoHideRow;
				ColumnClusterSize = null;
				ChartType = ChartType.NoChartType;
				Label = new Label();
				NumberFormats = new List<NumberFormat>();
				Charts = new List<ChartInfo>();
				Multicharts = new Multicharts();

				throw;
			}
		}

		public override void WriteFile()
		{
			CompileJson();
			base.WriteFile();
		}

		public MeasurementFile Clone()
		{
			MeasurementFile myClone = new MeasurementFile();
			myClone.MyJson = CompileJson();
			myClone.ParseFromJson();
			return myClone;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !this.GetType().Equals(obj.GetType()))
				return false;
			else
			{
				MeasurementFile mf = (MeasurementFile)obj;

				bool tableEqual = Table == mf.Table;
				bool baseMeasureEqual = BaseMeasure == mf.BaseMeasure;
				bool titleEqual = Title == mf.Title;
				bool dropdownEqual = Dropdown == mf.Dropdown;
				bool orderEqual = Order == mf.Order;
				bool summaryEqual = Summary == mf.Summary;
				bool filterEqual = Filter == mf.Filter;
				bool transformEqual = Transform.Equals(mf.Transform);
				bool controlsEqual = Controls.SequenceEqual(mf.Controls);
				bool parametersEqual = Parameters.SequenceEqual(mf.Parameters);
				bool dateParametersEqual = DateParameters.SequenceEqual(mf.DateParameters);
				bool requiredParametersEqual = RequiredParameters.SequenceEqual(mf.RequiredParameters);
				bool showAllOthersEqual = ShowAllOthers == mf.ShowAllOthers;
				bool maxCheckedEqual = MaxChecked == mf.MaxChecked;
				bool mutexAllOthersEqual = MutexAllOthers == mf.MutexAllOthers;
				bool returnRowHeadersEqual = ReturnRowHeaders.SequenceEqual(mf.ReturnRowHeaders);
				bool returnRowDateHeadersEqual = ReturnRowDateHeaders.SequenceEqual(mf.ReturnRowDateHeaders);
				bool returnRowControlHeadersEqual = ReturnRowControlHeaders.SequenceEqual(mf.ReturnRowControlHeaders);
				bool headerNamesEqual = HeaderNames.SequenceEqual(mf.HeaderNames);
				bool returnRowStartEqual = ReturnRowStart == mf.ReturnRowStart;
				bool hideRowEqual = HideRow == mf.HideRow;
				bool columnClusterSizeEqual = ColumnClusterSize == mf.ColumnClusterSize;
				bool chartTypeEqual = ChartType == mf.ChartType;
				bool labelEqual = Label.Equals(mf.Label);
				bool numberFormatsEqual = NumberFormats.SequenceEqual(mf.NumberFormats);
				bool chartsEqual = Charts.SequenceEqual(mf.Charts);
				bool multichartsEqual = Multicharts.Equals(mf.Multicharts);

				return tableEqual && baseMeasureEqual && titleEqual && orderEqual && dropdownEqual && summaryEqual && filterEqual && transformEqual &&
					controlsEqual && parametersEqual && dateParametersEqual && requiredParametersEqual && showAllOthersEqual && maxCheckedEqual &&
					mutexAllOthersEqual && returnRowHeadersEqual && returnRowDateHeadersEqual && returnRowControlHeadersEqual && headerNamesEqual &&
					returnRowStartEqual && hideRowEqual && columnClusterSizeEqual && chartTypeEqual && labelEqual && numberFormatsEqual && chartsEqual &&
					multichartsEqual;
			}
		}

		public override int GetHashCode()
		{
			return Table.GetHashCode() ^ BaseMeasure.GetHashCode() ^ Title.GetHashCode() ^ Dropdown.GetHashCode() ^ Order.GetHashCode() ^
				Summary.GetHashCode() ^ Filter.GetHashCode() ^ Transform.GetHashCode() ^ Controls.GetHashCode() ^ Parameters.GetHashCode() ^
				DateParameters.GetHashCode() ^ RequiredParameters.GetHashCode() ^ ShowAllOthers.GetHashCode() ^ MaxChecked.GetHashCode() ^
				MutexAllOthers.GetHashCode() ^ ReturnRowHeaders.GetHashCode() ^ ReturnRowDateHeaders.GetHashCode() ^ ReturnRowControlHeaders.GetHashCode() ^
				HeaderNames.GetHashCode() ^ ReturnRowStart.GetHashCode() ^ HideRow.GetHashCode() ^ ColumnClusterSize.GetHashCode() ^ ChartType.GetHashCode() ^
				Label.GetHashCode() ^ NumberFormats.GetHashCode() ^ Charts.GetHashCode() ^ Multicharts.GetHashCode();
		}
	}
}
