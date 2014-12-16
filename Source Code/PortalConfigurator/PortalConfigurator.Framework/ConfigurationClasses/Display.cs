using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework
{
	public class Display : IConfigurationBase
	{
		public string Legend { get; set; }
		public DisplayColumn DisplayColumn { get; set; }
		public DisplayType DisplayType { get; set; }
		public bool? Multi { get; set; }
		public bool? ZeroLast { get; set; }
		public SortType SortType { get; set; }
		public bool? SortBoolean { get; set; }
		public SortFunction SortFunction { get; set; }
		public SelectedType SelectedType { get; set; }
		public List<string> SelectedList { get; set; }
		public string Alias { get; set; }
		public DisabledType DisabledType { get; set; }
		public List<string> DisabledList { get; set; }
		public QuarterDate QuarterDate { get; set; }
		public int? MonthStep { get; set; }
		public int? MonthLimit { get; set; }
		public bool? Visible { get; set; }
		public ResultUnavailable ResultUnavailable { get; set; }
		public Dictionary<string, string> Help { get; set; }
		public Dictionary<string, string> Format { get; set; }
		public bool IsEmpty { get { return CheckIfEmpty(); } }

		public Display()
			: this(String.Empty, DisplayColumn.NoColumn, DisplayType.NoDisplayType, (bool?)null, (bool?)null, SortType.NoSort, (bool?)null, String.Empty,
			SelectedType.NoSelected, new List<string>(), String.Empty, DisabledType.NoDisabled, new List<string>(), QuarterDate.NoQuarterDate, (int?)null,
			(int?)null, (bool?)null, ResultUnavailable.NoResultUnavailable, new Dictionary<string,string>(), new Dictionary<string,string>())
		{ }

		public Display(string legend, DisplayColumn displayColumn, DisplayType displayType, bool? multi, bool? zeroLast, SortType sortType, bool? sortBoolean,
			string sortFunction, SelectedType selectedType, List<string> selectedList, string alias, DisabledType disabledType, List<string> disabledList,
			QuarterDate quarterDate, int? monthStep, int? monthLimit, bool? visible, ResultUnavailable resultUnavailable, Dictionary<string, string> help,
			Dictionary<string, string> format)
		{
			this.Legend = legend;
			this.DisplayColumn = displayColumn;
			this.DisplayType = displayType;
			this.Multi = multi;
			this.ZeroLast = zeroLast;
			this.SortType = sortType;
			this.SortBoolean = sortBoolean;
			this.SortFunction = SortFunction;
			this.SelectedType = selectedType;
			this.SelectedList = selectedList;
			this.Alias = alias;
			this.DisabledType = disabledType;
			this.DisabledList = disabledList;
			this.QuarterDate = quarterDate;
			this.MonthStep = monthStep;
			this.MonthLimit = monthLimit;
			this.Visible = visible;
			this.ResultUnavailable = resultUnavailable;
			this.Help = help;
			this.Format = format;
		}

		private bool CheckIfEmpty()
		{
			return String.IsNullOrEmpty(Legend) && DisplayColumn == DisplayColumn.NoColumn && DisplayType == DisplayType.NoDisplayType && Multi == null &&
				ZeroLast == null && SortType == SortType.NoSort && SelectedType == SelectedType.NoSelected && String.IsNullOrEmpty(Alias) &&
				DisabledType == DisabledType.NoDisabled && QuarterDate == QuarterDate.NoQuarterDate && Visible == null &&
				ResultUnavailable == ResultUnavailable.NoResultUnavailable && Help.Count == 0 && Format.Count == 0;
		}

		public JObject CompileJson()
		{
			JObject myJson = new JObject();

			if (!String.IsNullOrEmpty(Legend))
				myJson.Add("legend", Legend);

			if (DisplayColumn != DisplayColumn.NoColumn)
				myJson.Add("column", Enums.GetString(DisplayColumn));

			if (DisplayType != DisplayType.NoDisplayType)
				myJson.Add("type", Enums.GetString(DisplayType));

			if (Multi != null)
				myJson.Add("multi", Multi);

			if (ZeroLast != null)
				myJson.Add("zeroLast", ZeroLast);


			if ((SortType == SortType.True || SortType == SortType.False) && SortBoolean != null)
				myJson.Add("sort", SortBoolean);
			else if (SortType == SortType.Function && SortFunction != SortFunction.NoSortFunction)
				myJson.Add("sort", Enums.GetString(SortFunction));

			AddSelectedJson(ref myJson);

			if (!String.IsNullOrEmpty(Alias))
				myJson.Add("alias", Alias);

			AddDisalbedJson(ref myJson);

			if (QuarterDate != QuarterDate.NoQuarterDate)
				myJson.Add("quarterDate", Enums.GetString(QuarterDate));

			if ((DisplayType == DisplayType.DateRange || DisplayType == DisplayType.DateSelect || DisplayType == DisplayType.DynamicDateRange) && MonthStep != null)
				myJson.Add("monthStep", MonthStep);

			if ((DisplayType == DisplayType.DateRange || DisplayType == DisplayType.DynamicDateRange) && MonthLimit != null)
				myJson.Add("monthLimit", MonthLimit);

			if (Visible != null)
				myJson.Add("visible", Visible);

			if (ResultUnavailable != ResultUnavailable.NoResultUnavailable)
				myJson.Add("resultUnavailable", Enums.GetString(ResultUnavailable));

			if (Help.Count == 1)
				myJson.Add("help", Help.ElementAt(0).Value);
			else if (Help.Count != 0)
			{
				JObject helpJson = new JObject();

				foreach (var helpItem in Help)
					helpJson.Add(helpItem.Key, helpItem.Value);

				myJson.Add("help", new JObject(helpJson));
			}
			
			if (DisplayType == DisplayType.DateType && Format.Count != 0)
			{
				JObject formatJson = new JObject();

				foreach (var formatItem in Format)
					formatJson.Add(formatItem.Key, formatItem.Value);

				myJson.Add("format", new JObject(formatJson));
			}

			return myJson;
		}

		public void AddSelectedJson(ref JObject myJson)
		{
			int intValue;

			if (SelectedType == SelectedType.Integer && SelectedList.Count == 1 && int.TryParse(SelectedList.ElementAt(0), out intValue))
				myJson.Add("selected", intValue);
			else if (SelectedType == SelectedType.String && SelectedList.Count == 1)
				myJson.Add("selected", SelectedList.ElementAt(0));
			else if (SelectedType == SelectedType.Integer && SelectedList.Count != 0)
			{
				List<int> intValues = new List<int>();

				foreach (var item in SelectedList)
					if (int.TryParse(item, out intValue))
						intValues.Add(intValue);

				if (SelectedList.Count == intValues.Count)
					myJson.Add("selected", new JArray(intValues));
			}
			else if (SelectedType == SelectedType.String && SelectedList.Count != 0)
				myJson.Add("selected", new JArray(SelectedList));
		}

		public void AddDisalbedJson(ref JObject myJson)
		{
			if (DisabledType == DisabledType.Integer)
			{
				int intValue;
				List<int> intValues = new List<int>();

				foreach (var item in DisabledList)
					if (int.TryParse(item, out intValue))
						intValues.Add(intValue);

				if (DisabledList.Count == intValues.Count)
					myJson.Add("disabled", new JArray(intValues));
			}
			else if (DisabledType == DisabledType.String)
				myJson.Add("disabled", new JArray(DisabledList));
		}

		public void ParseJson(JObject json)
		{
			int? intValue = 0;
			List<int> intList = new List<int>();
			SelectedList.Clear();
			DisabledList.Clear();
			Help.Clear();
			Format.Clear();

			foreach (var property in json.Properties())
			{
				if (property.Name == "legend")
					Legend = Json.Parse(Legend, property);
				else if (property.Name == "column")
					DisplayColumn = Enums.GetDisplayColumnEnum(Json.Parse(String.Empty, property));
				else if (property.Name == "type")
					DisplayType = Enums.GetDisplayTypeEnum(Json.Parse(String.Empty, property));
				else if (property.Name == "multi")
					Multi = Json.Parse(Multi, property);
				else if (property.Name == "zeroLast")
					ZeroLast = Json.Parse(ZeroLast, property);
				else if (property.Name == "sort")
				{
					SortBoolean = Json.Parse(SortBoolean, property);
					SortFunction = Enums.GetSortFunctionEnum(Json.Parse(String.Empty, property));

					if (SortBoolean != null)
						SortType = SortBoolean ?? false ? SortType.True : SortType.False;
					else if (SortFunction != SortFunction.NoSortFunction)
						SortType = SortType.Function;
				}
				else if (property.Name == "selected")
				{
					if (property.Value.Type == JTokenType.String)
					{
						SelectedList.Add(Json.Parse(String.Empty, property));
						SelectedType = SelectedType.String;
					}
					else if (property.Value.Type == JTokenType.Integer)
					{
						SelectedList.Add(Json.Parse(intValue, property).ToString());
						SelectedType = SelectedType.Integer;
					}
					else if (property.Value.Type == JTokenType.Array)
					{
						List<JToken> array = property.Values().ToList<JToken>();
						
						if (array.Values().All(p => p.Type == JTokenType.String))
						{
							SelectedList = Json.Parse(SelectedList, property);
							SelectedType = SelectedType.String;
						}
						else if (array.Values().All(p => p.Type == JTokenType.Integer))
						{
							intList.Clear();
							intList = Json.Parse(intList, property);

							foreach (var item in intList)
								SelectedList.Add(item.ToString());

							SelectedType = SelectedType.Integer;
						}
					}
					else
						SelectedType = SelectedType.NoSelected;
				}
				else if (property.Name == "alias")
					Alias = Json.Parse(Alias, property);
				else if (property.Name == "disabled")
				{
					List<JToken> array = property.Values().ToList<JToken>();

					if (array.Values().All(p => p.Type == JTokenType.String))
					{
						DisabledList = Json.Parse(DisabledList, property);
						DisabledType = DisabledType.String;
					}
					else if (array.Values().All(p => p.Type == JTokenType.Integer))
					{
						intList.Clear();
						intList = Json.Parse(intList, property);

						foreach (var item in intList)
							DisabledList.Add(item.ToString());

						DisabledType = DisabledType.Integer;
					}
					else
						DisabledType = DisabledType.NoDisabled;
				}
				else if (property.Name == "quarterDate")
					QuarterDate = Enums.GetQuarterDateEnum(Json.Parse(String.Empty, property));
				else if (property.Name == "monthStep")
					MonthStep = Json.Parse(MonthStep, property);
				else if (property.Name == "monthLimit")
					MonthLimit = Json.Parse(MonthLimit, property);
				else if (property.Name == "visible")
					Visible = Json.Parse(Visible, property);
				else if (property.Name == "resultUnavailable")
					ResultUnavailable = Enums.GetResultUnavailableEnum(Json.Parse(String.Empty, property));
				else if (property.Name == "help")
				{
					if (property.Value.Type == JTokenType.String)
						Help.Add("default", Json.Parse(String.Empty, property));
					else if (property.Value.Type == JTokenType.Object)
						Help = Json.Parse(Help, property);
				}
				else if (property.Name == "format")
					Format = Json.Parse(Format, property);
				else
					throw new UnknownJsonPropertyException(String.Format("The {0} property is not defined for a Display.", property.Name));
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !this.GetType().Equals(obj.GetType()))
				return false;
			else
			{
				Display d = (Display)obj;

				bool legendEqual = Legend == d.Legend;
				bool displayColumnEqual = DisplayColumn == d.DisplayColumn;
				bool displayTypeEqual = DisplayType == d.DisplayType;
				bool multiEqual = Multi == d.Multi;
				bool zeroLastEqual = ZeroLast == d.ZeroLast;
				bool sortTypeEqual = SortType == d.SortType;
				bool sortBooleanEqual = SortBoolean == d.SortBoolean;
				bool sortFunctionEqual = SortFunction == d.SortFunction;
				bool selectedTypeEqual = SelectedType == d.SelectedType;
				bool selectedListEqual = SelectedList.SequenceEqual(d.SelectedList);
				bool aliasEqual = Alias == d.Alias;
				bool disabledTypeEqual = DisabledType == d.DisabledType;
				bool disabledListEqual = DisabledList.SequenceEqual(d.DisabledList);
				bool quarterDateEqual = QuarterDate == d.QuarterDate;
				bool monthStepEqual = MonthStep == d.MonthStep;
				bool monthLimitEqual = MonthLimit == d.MonthLimit;
				bool visibleEqual = Visible == d.Visible;
				bool resultUnavailableEqual = ResultUnavailable == d.ResultUnavailable;
				bool helpEqual = Help.SequenceEqual(d.Help);
				bool formatEqual = Format.SequenceEqual(d.Format);

				return legendEqual && displayColumnEqual && displayTypeEqual && multiEqual && zeroLastEqual && sortTypeEqual && sortBooleanEqual &&
					sortFunctionEqual && selectedTypeEqual && selectedListEqual && aliasEqual && disabledTypeEqual && disabledListEqual &&
					quarterDateEqual && monthStepEqual && monthLimitEqual && visibleEqual && resultUnavailableEqual && helpEqual && formatEqual;
			}
		}

		public override int GetHashCode()
		{
			return Legend.GetHashCode() ^ DisplayColumn.GetHashCode() ^ DisplayType.GetHashCode() ^ Multi.GetHashCode() ^ ZeroLast.GetHashCode() ^
				SortType.GetHashCode() ^ SortBoolean.GetHashCode() ^ SortFunction.GetHashCode() ^ SelectedType.GetHashCode() ^
				SelectedList.GetHashCode() ^ Alias.GetHashCode() ^ DisabledType.GetHashCode() ^ DisabledList.GetHashCode() ^
				QuarterDate.GetHashCode() ^ MonthStep.GetHashCode() ^ MonthLimit.GetHashCode() ^ Visible.GetHashCode() ^
				ResultUnavailable.GetHashCode() ^ Help.GetHashCode() ^ Format.GetHashCode();
		}
	}
}
