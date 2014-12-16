using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Framework
{
    public class MeasurementFile : JsonFile, IConfigurationBase
    {
		private Dictionary<string, FilterParameterGridRow> _filterParameterGridRows;
		private Dictionary<int, HeaderGridRow> _headerGridRows;
		private Dictionary<int, string> _tableColumns;
		private bool _filterParameterGridRowsAreUpdated;
		private bool _headerGridRowsAreUpdated;
		private bool _tableDataAreLoaded;
		private string _table;

		public string BaseMeasure { get; set; }
		public string Title { get; set; }
		public int? Order { get; set; }
		public string FullTitle { get; set; }
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

		public string Table
		{
			get
			{
				if (_table != Transform.Table)
					Transform.Table = _table;

				return _table;
			}
			set
			{
				_table = value;
				Transform.Table = value;
			}
		}
		public Dictionary<string, FilterParameterGridRow> FilterParameterGridRows
		{
			get
			{
				if (!_filterParameterGridRowsAreUpdated)
					RefreshFilterParameterGridRows();

				return _filterParameterGridRows;
			}
			set
			{
				_filterParameterGridRows = value;
				UpdatePropertiesFromFilterParameterGridRows();
			}
		}

		public Dictionary<int, HeaderGridRow> HeaderGridRows
		{
			get
			{
				if (!_headerGridRowsAreUpdated)
					RefreshHeaderGridRows();
				
				return _headerGridRows;
			}
			set
			{
				_headerGridRows = value;
				UpdatePropertiesFromHeaderGridRows();
			}
		}

		public bool FilterParameterGridRowsAreUpdated
		{
			get { return _filterParameterGridRowsAreUpdated; }
			set
			{
				_filterParameterGridRowsAreUpdated = value;

				if (!value)
					_filterParameterGridRows.Clear();
			}
		}

		public bool HeaderGridRowsAreUpdated
		{
			get { return _headerGridRowsAreUpdated; }
			set
			{
				_headerGridRowsAreUpdated = value;

				if (!value)
					_headerGridRows.Clear();
			}
		}

		public Dictionary<int, string> TableColumns
		{
			get
			{
				if (_tableColumns.Count == 0)
					LoadTableFromDatabase();

				return _tableColumns;
			}
			set
			{
				_tableColumns = value;
				Transform.TableColumns = value;
			}
		}

		public bool TableDataAreLoaded
		{
			get { return _tableDataAreLoaded; }
			set
			{
				_tableDataAreLoaded = value;
				Transform.TableDataAreLoaded = value;

				if (!value)
					_tableColumns.Clear();
			}
		}

		public MeasurementFile()
			: this(String.Empty)
		{ }

		public MeasurementFile(string filePath)
			: this(filePath, String.Empty, String.Empty, String.Empty, (int?)null, String.Empty, String.Empty, String.Empty, new Transform(), new List<string>(),
			new List<string>(), new List<string>(), new List<string>(), (bool?)null, (int?)null, (bool?)null, new List<int>(), new List<int>(), new List<int>(),
			new List<string>(), (int?)null, HideRow.NoHideRow, (int?)null, ChartType.NoChartType, new Label(), new List<NumberFormat>(), new List<ChartInfo>(),
			new Multicharts())
		{ }

		public MeasurementFile(string filePath, string table, string baseMeasure, string title, int? order, string fullTitle, string summary, string filter,
			Transform transform, List<string> controls, List<string> parameters, List<string> dateParameters, List<string> requiredParameters, bool? showAllOthers,
			int? maxChecked, bool? mutexAllOthers, List<int> returnRowHeaders, List<int> returnRowDateHeaders, List<int> returnRowControlHeaders,
			List<string> headerNames, int? returnRowStart, HideRow hideRow, int? columnClusterSize, ChartType chartType, Label label,
			List<NumberFormat> numberFormats, List<ChartInfo> charts, Multicharts multicharts)
			: base(filePath)
		{
			this.Transform = transform;
			this.Table = table;
			this.BaseMeasure = baseMeasure;
			this.Title = title;
			this.Order = order;
			this.FullTitle = fullTitle;
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
			this._filterParameterGridRows = new Dictionary<string, FilterParameterGridRow>();
			this._headerGridRows = new Dictionary<int, HeaderGridRow>();
			this._tableColumns = new Dictionary<int, string>();
			this._filterParameterGridRowsAreUpdated = false;
			this._headerGridRowsAreUpdated = false;
			this._tableDataAreLoaded = false;
		}

		private bool CheckForEmpty()
		{
			return (String.IsNullOrEmpty(Table) && String.IsNullOrEmpty(BaseMeasure) && String.IsNullOrEmpty(Title) && Order == null &&
				String.IsNullOrEmpty(FullTitle) && Order == null && String.IsNullOrEmpty(Summary) && String.IsNullOrEmpty(Filter) && Transform.IsEmpty &&
				Controls.Count == 0 && Parameters.Count == 0 && DateParameters.Count == 0 && RequiredParameters.Count == 0 && ShowAllOthers == null &&
				MaxChecked == null && MutexAllOthers == null && ReturnRowHeaders.Count == 0 && ReturnRowDateHeaders.Count == 0 &&
				ReturnRowControlHeaders.Count == 0 && HeaderNames.Count == 0 && ReturnRowStart == null && HideRow == HideRow.NoHideRow &&
				ColumnClusterSize == null && ChartType == ChartType.NoChartType && Label.IsEmpty && NumberFormats.Count(p => !p.IsEmpty) == 0 &&
				Charts.Count(p => !p.IsEmpty) == 0 && Multicharts.IsEmpty);
		}

		private void LoadTableFromDatabase()
		{
			bool tableDataWasUpdated = false;

			try
			{
				if (!String.IsNullOrEmpty(Table))
					_tableColumns = Database.PopulateMeasure(Table);

				Transform.TableColumns = _tableColumns;
				tableDataWasUpdated = true;
			}
			catch (Exception)
			{
				_tableColumns = new Dictionary<int, string>();
				Transform.TableColumns = _tableColumns;
				throw;
			}
			finally
			{
				_tableDataAreLoaded = true;
				Transform.TableDataAreLoaded = true;
				_headerGridRowsAreUpdated = !tableDataWasUpdated;
				Transform._transformFieldGridRowsAreUpdated = !tableDataWasUpdated;
			}
		}

		private void RefreshFilterParameterGridRows()
		{
			foreach (var item in Controls)
			{
				FilterParameterGridRow row = new FilterParameterGridRow();
				row.ControlType = ControlType.Filter;
				_filterParameterGridRows.Add(item, row);
			}

			foreach (var item in Parameters)
			{
				if (_filterParameterGridRows.ContainsKey(item))
					_filterParameterGridRows[item].ControlType = ControlType.Both;
				else
				{
					FilterParameterGridRow row = new FilterParameterGridRow();
					row.ControlType = ControlType.Parameter;
					_filterParameterGridRows.Add(item, row);
				}
			}

			foreach (var item in DateParameters)
			{
				if (_filterParameterGridRows.ContainsKey(item))
				{
					if (_filterParameterGridRows[item].ControlType == ControlType.Filter)
						_filterParameterGridRows[item].ControlType = ControlType.Both;

					_filterParameterGridRows[item].IsDate = true;
				}
				else
				{
					FilterParameterGridRow row = new FilterParameterGridRow();
					row.ControlType = ControlType.Parameter;
					row.IsDate = true;
					_filterParameterGridRows.Add(item, row);
				}
			}

			foreach (var item in RequiredParameters)
			{
				if (_filterParameterGridRows.ContainsKey(item))
				{
					if (_filterParameterGridRows[item].ControlType == ControlType.Filter)
						_filterParameterGridRows[item].ControlType = ControlType.Both;

					_filterParameterGridRows[item].IsRequired = true;
				}
				else
				{
					FilterParameterGridRow row = new FilterParameterGridRow();
					row.ControlType = ControlType.Parameter;
					row.IsRequired = true;
					_filterParameterGridRows.Add(item, row);
				}
			}

			_filterParameterGridRowsAreUpdated = true;
		}

		public void UpdatePropertiesFromFilterParameterGridRows()
		{
			Controls.Clear();
			Parameters.Clear();
			DateParameters.Clear();
			RequiredParameters.Clear();

			foreach (var item in _filterParameterGridRows)
			{
				if (item.Value.ControlType == ControlType.Filter || item.Value.ControlType == ControlType.Both)
					Controls.Add(item.Key);

				if (item.Value.ControlType == ControlType.Parameter || item.Value.ControlType == ControlType.Both)
					Parameters.Add(item.Key);

				if (item.Value.IsDate)
					DateParameters.Add(item.Key);

				if (item.Value.IsRequired)
					RequiredParameters.Add(item.Key);
			}
		}

		private void RefreshHeaderGridRows()
		{
			_headerGridRows.Clear();
			int adjustedTableOrdinal;
			int headerNamesIndex;
			string headerText;
			bool needsDateField;
			bool hasDateField;
			NeedsDateField(out needsDateField, out hasDateField);

			if (needsDateField && hasDateField)
			{
				headerText = "&nbsp;";
				HeaderGridRow nbspHeader = new HeaderGridRow(headerText);
				nbspHeader.IsReturnRow = ReturnRowHeaders.Contains(0);
				nbspHeader.IsReturnRowDate = ReturnRowDateHeaders.Contains(0);
				nbspHeader.IsReturnRowControl = ReturnRowControlHeaders.Contains(0);
				_headerGridRows.Add(0, nbspHeader);
			}

			foreach (var item in TableColumns)
			{
				adjustedTableOrdinal = GetAdjustedTableOrdinal(_headerGridRows.Count, item.Key);
				headerNamesIndex = HeaderNames.FindIndex(p => p.StartsWith(item.Value) || p.StartsWith(String.Concat('*', item.Value)));
				headerText = headerNamesIndex == -1 ? String.Empty : HeaderNames.ElementAt(headerNamesIndex);
				HeaderGridRow row = new HeaderGridRow(item.Key, item.Value, headerText);

				row.IsReturnRow = ReturnRowHeaders.Contains(adjustedTableOrdinal);
				row.IsReturnRowDate = ReturnRowDateHeaders.Contains(adjustedTableOrdinal);
				row.IsReturnRowControl = ReturnRowControlHeaders.Contains(adjustedTableOrdinal);
				
				_headerGridRows.Add(adjustedTableOrdinal, row);
			}

			_headerGridRowsAreUpdated = true;
		}

		public void UpdatePropertiesFromHeaderGridRows()
		{
			HeaderNames.Clear();
			ReturnRowHeaders.Clear();
			ReturnRowDateHeaders.Clear();
			ReturnRowControlHeaders.Clear();

			foreach (var item in _headerGridRows)
			{
				if (item.Value.HeaderType != HeaderType.NotAHeader)
					HeaderNames.Add(item.Value.HeaderText);

				if (item.Value.IsReturnRow)
					ReturnRowHeaders.Add(item.Key);

				if (item.Value.IsReturnRowDate)
					ReturnRowDateHeaders.Add(item.Key);

				if (item.Value.IsReturnRowControl)
					ReturnRowControlHeaders.Add(item.Key);
			}
		}

		private int GetAdjustedTableOrdinal(int headerGridRowOrdinal, int tableOrdinal)
		{
			bool needsDateField;
			bool hasDateField;
			NeedsDateField(out needsDateField, out hasDateField);
			int dateFieldOrdinal = needsDateField && hasDateField ? TableColumns.First(p => p.Value == Transform.DateField).Key : -1;

			if (!needsDateField || !hasDateField || tableOrdinal < dateFieldOrdinal)
				return headerGridRowOrdinal;
			else if (needsDateField && hasDateField && tableOrdinal > dateFieldOrdinal)
				return tableOrdinal;
			else
				return -1;
		}

		private void NeedsDateField(out bool needsDateField, out bool hasDateField)
		{
			needsDateField = Transform.Function == TransformFunction.Trim || Transform.Function == TransformFunction.DateRow;
			hasDateField = !String.IsNullOrEmpty(Transform.DateField) && TableColumns.ContainsValue(Transform.DateField);
		}

		public JObject CompileJson()
		{
			JObject myJson = new JObject();
			int? returnRowStart = null;
			int? columnClusterSize = null;
			string firstValueColumnName = Transform.ValueFields.FirstOrDefault() ?? String.Empty;

			if (Transform.ValueFields.Count != 0 && TableColumns.ContainsValue(firstValueColumnName))
			{
				returnRowStart = TableColumns.First(p => p.Value == firstValueColumnName).Key;
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

			if (Order != null)
				myJson.Add("measurementOrder", Order);

			if (!String.IsNullOrEmpty(FullTitle))
				myJson.Add("fullTitle", FullTitle);

			if (!String.IsNullOrEmpty(Summary))
				myJson.Add("summary", Summary);

			if (!String.IsNullOrEmpty(Filter))
				myJson.Add("filter", Filter);

			if (!Transform.IsEmpty)
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
				myJson.Add("chartType", Enums.GetString(ChartType));

			if (!Label.IsEmpty)
				myJson.Add("labels", Label.CompileJson());

			if (NumberFormats.Count == 1)
				myJson.Add("numberFormat", NumberFormats.ElementAt(0).CompileJson());
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
					if (property.Name == "table")
						Table = Json.Parse(Table, property);
					else if (property.Name == "Base")
						BaseMeasure = Json.Parse(BaseMeasure, property);
					else if (property.Name == "title")
						Title = Json.Parse(Title, property);
					else if (property.Name == "measurementOrder")
						Order = Json.Parse(Order, property);
					else if (property.Name == "fullTitle")
						FullTitle = Json.Parse(FullTitle, property);
					else if (property.Name == "summary")
						Summary = Json.Parse(Summary, property);
					else if (property.Name == "filter")
						Filter = Json.Parse(Filter, property);
					else if (property.Name == "transform")
						Transform.ParseJson((JObject)property.Value);
					else if (property.Name == "controls")
						Controls = Json.Parse(Controls, property);
					else if (property.Name == "params")
						Parameters = Json.Parse(Parameters, property);
					else if (property.Name == "dateParams")
						DateParameters = Json.Parse(DateParameters, property);
					else if (property.Name == "requiredParams")
						RequiredParameters = Json.Parse(RequiredParameters, property);
					else if (property.Name == "showAllOthers")
						ShowAllOthers = Json.Parse(ShowAllOthers, property);
					else if (property.Name == "maxChecked")
						MaxChecked = Json.Parse(MaxChecked, property);
					else if (property.Name == "mutexAllOthers")
						MutexAllOthers = Json.Parse(MutexAllOthers, property);
					else if (property.Name == "returnRowHeaders")
						ReturnRowHeaders = Json.Parse(ReturnRowHeaders, property);
					else if (property.Name == "returnRowDateHeaders")
						ReturnRowDateHeaders = Json.Parse(ReturnRowDateHeaders, property);
					else if (property.Name == "returnRowControlHeaders")
						ReturnRowControlHeaders = Json.Parse(ReturnRowControlHeaders, property);
					else if (property.Name == "headerNames")
						HeaderNames = Json.Parse(HeaderNames, property);
					else if (property.Name == "returnRowStart")
						ReturnRowStart = Json.Parse(ReturnRowStart, property);
					else if (property.Name == "hideRow")
						HideRow = Enums.GetHideRowEnum(Json.Parse(String.Empty, property));
					else if (property.Name == "columnClusterSize")
						ColumnClusterSize = Json.Parse(ColumnClusterSize, property);
					else if (property.Name == "chartType")
						ChartType = Enums.GetChartTypeEnum(Json.Parse(String.Empty, property));
					else if (property.Name == "labels")
						Label.ParseJson((JObject)property.Value);
					else if (property.Name == "numberFormat")
					{
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
					}
					else if (property.Name == "charts")
					{
						foreach (var token in property.Values())
						{
							ChartInfo chart = new ChartInfo();
							chart.ParseJson((JObject)token);
							Charts.Add(chart);
						}
					}
					else if (property.Name == "multicharts")
						Multicharts.ParseJson((JObject)property.Value);
					else
						throw new UnknownJsonPropertyException(String.Format("The {0} property is not defined for a Measurement file.", property.Name));
				}
			}
			catch (Exception)
			{
				Table = String.Empty;
				BaseMeasure = String.Empty;
				Title = String.Empty;
				Order = null;
				FullTitle = String.Empty;
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
				bool orderEqual = Order == mf.Order;
				bool fullTitleEqual = FullTitle == mf.FullTitle;
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

				return tableEqual && baseMeasureEqual && titleEqual && orderEqual && fullTitleEqual && summaryEqual && filterEqual && transformEqual &&
					controlsEqual && parametersEqual && dateParametersEqual && requiredParametersEqual && showAllOthersEqual && maxCheckedEqual &&
					mutexAllOthersEqual && returnRowHeadersEqual && returnRowDateHeadersEqual && returnRowControlHeadersEqual && headerNamesEqual &&
					returnRowStartEqual && hideRowEqual && columnClusterSizeEqual && chartTypeEqual && labelEqual && numberFormatsEqual && chartsEqual &&
					multichartsEqual;
			}
		}

		public override int GetHashCode()
		{
			return Table.GetHashCode() ^ BaseMeasure.GetHashCode() ^ Title.GetHashCode() ^ Order.GetHashCode() ^ FullTitle.GetHashCode() ^
				Summary.GetHashCode() ^ Filter.GetHashCode() ^ Transform.GetHashCode() ^ Controls.GetHashCode() ^ Parameters.GetHashCode() ^
				DateParameters.GetHashCode() ^ RequiredParameters.GetHashCode() ^ ShowAllOthers.GetHashCode() ^ MaxChecked.GetHashCode() ^
				MutexAllOthers.GetHashCode() ^ ReturnRowHeaders.GetHashCode() ^ ReturnRowDateHeaders.GetHashCode() ^ ReturnRowControlHeaders.GetHashCode() ^
				HeaderNames.GetHashCode() ^ ReturnRowStart.GetHashCode() ^ HideRow.GetHashCode() ^ ColumnClusterSize.GetHashCode() ^ ChartType.GetHashCode() ^
				Label.GetHashCode() ^ NumberFormats.GetHashCode() ^ Charts.GetHashCode() ^ Multicharts.GetHashCode();
		}
	}
}
