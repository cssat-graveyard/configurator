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
		public bool? Multi { get; set; }
		public bool? ZeroLast { get; set; }
		public SortType SortType { get; set; }
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

		private DisplayType _displayType;
		public DisplayType DisplayType
		{
			get
			{
				return _displayType;
			}

			set
			{
				if (value == DisplayType.Parameter)
					Multi = null;

				_displayType = value;
			}
		}

		public Display()
			: this(String.Empty, DisplayColumn.NoColumn, DisplayType.NoDisplayType, (bool?)null, (bool?)null, SortType.NoSortType,
			SelectedType.NoSelected, new List<string>(), String.Empty, DisabledType.NoDisabled, new List<string>(), QuarterDate.NoQuarterDate, (int?)null,
			(int?)null, (bool?)null, ResultUnavailable.NoResultUnavailable, new Dictionary<string,string>(), new Dictionary<string,string>())
		{ }

		public Display(string legend, DisplayColumn displayColumn, DisplayType displayType, bool? multi, bool? zeroLast, SortType sortFunction,
			SelectedType selectedType, List<string> selectedList, string alias, DisabledType disabledType, List<string> disabledList, QuarterDate quarterDate,
			int? monthStep, int? monthLimit, bool? visible, ResultUnavailable resultUnavailable, Dictionary<string, string> help, Dictionary<string, string> format)
		{
			this.Legend = legend;
			this.DisplayColumn = displayColumn;
			this.Multi = multi;
			this.DisplayType = displayType;
			this.ZeroLast = zeroLast;
			this.SortType = sortFunction;
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
				ZeroLast == null && SortType == SortType.NoSortType && SelectedType == SelectedType.NoSelected && String.IsNullOrEmpty(Alias) &&
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

			if (SortType == SortType.DefaultSort || SortType == SortType.NotSorted)
				myJson.Add("sort", bool.Parse(Enums.GetString(SortType)));
			else if (SortType != SortType.NoSortType)
				myJson.Add("sort", Enums.GetString(SortType));

			if (SelectedList.Count != 0)
				AddSelectedJson(ref myJson);

			if (!String.IsNullOrEmpty(Alias))
				myJson.Add("alias", Alias);

			if (DisabledList.Count != 0)
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

			if (SelectedType == SelectedType.Integer && SelectedList.Count == 1 && int.TryParse(SelectedList[0], out intValue))
				myJson.Add("selected", intValue);
			else if (SelectedType == SelectedType.String && SelectedList.Count == 1)
				myJson.Add("selected", SelectedList[0]);
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
				switch (property.Name)
				{
					case "legend":
						Legend = Json.Parse(Legend, property);
						break;
					case "column":
						DisplayColumn = Enums.GetDisplayColumnEnum(Json.Parse(String.Empty, property));
						break;
					case "type":
						DisplayType = Enums.GetDisplayTypeEnum(Json.Parse(String.Empty, property));
						break;
					case "multi":
						Multi = Json.Parse(Multi, property);
						break;
					case "zeroLast":
						ZeroLast = Json.Parse(ZeroLast, property);
						break;
					case "sort":
						if (property.Value.Type == JTokenType.Boolean)
							SortType = Enums.GetSortTypeEnum(Json.Parse((bool?)null, property).ToString());
						else if (property.Value.Type == JTokenType.String)
							SortType = Enums.GetSortTypeEnum(Json.Parse(String.Empty, property));
						break;
					case "selected":
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

							if (SelectedList.Count == 0)
								SelectedType = SelectedType.NoSelected;
						}
						break;
					case "alias":
						Alias = Json.Parse(Alias, property);
						break;
					case "disabled":
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

							if (DisabledList.Count == 0)
								DisabledType = DisabledType.NoDisabled;
						}
						break;
					case "quarterDate":
						QuarterDate = Enums.GetQuarterDateEnum(Json.Parse(String.Empty, property));
						break;
					case "monthStep":
						MonthStep = Json.Parse(MonthStep, property);
						break;
					case "monthLimit":
						MonthLimit = Json.Parse(MonthLimit, property);
						break;
					case "visible":
						Visible = Json.Parse(Visible, property);
						break;
					case "resultUnavailable":
						ResultUnavailable = Enums.GetResultUnavailableEnum(Json.Parse(String.Empty, property));
						break;
					case "help":
						if (property.Value.Type == JTokenType.String)
							Help.Add("default", Json.Parse(String.Empty, property));
						else if (property.Value.Type == JTokenType.Object)
							Help = Json.Parse(Help, property);
						break;
					case "format":
						Format = Json.Parse(Format, property);
						break;
					default:
						throw new UnknownJsonPropertyException(String.Format("The {0} property is not defined for a Display.", property.Name));
				}
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

				return legendEqual && displayColumnEqual && displayTypeEqual && multiEqual && zeroLastEqual && sortTypeEqual && selectedTypeEqual &&
					selectedListEqual && aliasEqual && disabledTypeEqual && disabledListEqual && quarterDateEqual && monthStepEqual && monthLimitEqual &&
					visibleEqual && resultUnavailableEqual && helpEqual && formatEqual;
			}
		}

		public override int GetHashCode()
		{
			return Legend.GetHashCode() ^ DisplayColumn.GetHashCode() ^ DisplayType.GetHashCode() ^ Multi.GetHashCode() ^ ZeroLast.GetHashCode() ^
				SortType.GetHashCode() ^ SelectedType.GetHashCode() ^ SelectedList.GetHashCode() ^ Alias.GetHashCode() ^ DisabledType.GetHashCode() ^
				DisabledList.GetHashCode() ^ QuarterDate.GetHashCode() ^ MonthStep.GetHashCode() ^ MonthLimit.GetHashCode() ^ Visible.GetHashCode() ^
				ResultUnavailable.GetHashCode() ^ Help.GetHashCode() ^ Format.GetHashCode();
		}
	}
}
