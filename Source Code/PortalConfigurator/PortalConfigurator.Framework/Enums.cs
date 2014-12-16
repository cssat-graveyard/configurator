using System;

namespace Framework
{
	public enum AddInputClass
	{
		NoAddInputClass,
		MultiAllowed
	}

	public enum AxisFormat
	{
		NoFormat,
		Percent,
		FixedDecimal,
		Commas
	}

	public enum ChartType
	{
		NoChartType,
		Line,
		Column,
		Area,
		SteppedArea,
		Scatter
	}

	public enum ControlType
	{
		Neither,
		Control,
		Parameter,
		Both
	}

	public enum DisabledType
	{
		NoDisabled,
		Integer,
		String
	}

	public enum DisplayColumn
	{
		NoColumn,
		FieldsLeft,
		FieldsRight,
		AdvancedLeft,
		AdvancedRight
	}

	public enum DisplayType
	{
		NoDisplayType,
		Filter,
		Parameter,
		Selector,
		Cohort,
		DateRange,
		DynamicDateRange,
		DateSelect,
		DateType
	}

	public enum FilterParameterType
	{
		Values,
		Table,
		Neither
	}

	public enum Function
	{
		NoFunction,
		Rotate,
		Trim,
		DateRow
	}

	public enum HeaderType
	{
		NotAHeader,
		ColumnName,
		FilterParameterName,
		FilterParameterIndex,
		DateColumn
	}

	public enum HideRow
	{
		NoHideRow,
		AnyEmpty,
		AllEmpty,
		IgnoreEmpty
	}

	public enum QuarterDate
	{
		NoQuarterDate,
		Start,
		End
	}

	public enum ResultUnavailable
	{
		NoResultUnavailable,
		Show,
		Hide,
		Disable
	}

	public enum SelectedType
	{
		NoSelected,
		Integer,
		String
	}

	public enum SortType
	{
		NoSortType,
		DefaultSort,
		NotSorted,
		RaceEthnicityCompare,
		QueryTypeCompare
	}

	public enum ValuesType
	{
		NoValues,
		ListIntegers,
		ListStrings,
		DictionaryIntegers,
		DictionaryStrings
	}

	public static class Enums
	{
		public static string GetString(AddInputClass value)
		{
			string enumString = String.Empty;

			switch (value)
			{
				case AddInputClass.NoAddInputClass:
					break;
				case AddInputClass.MultiAllowed:
					enumString = "multiAllowed";
					break;
				default:
					break;
			}

			return enumString;
		}

		public static string GetString(AxisFormat value)
		{
			string enumString = String.Empty;

			switch (value)
			{
				case AxisFormat.NoFormat:
					break;
				case AxisFormat.Percent:
					enumString = "##\'%";
					break;
				case AxisFormat.FixedDecimal:
					enumString = "##.00";
					break;
				case AxisFormat.Commas:
					enumString = "#,##0";
					break;
				default:
					break;
			}

			return enumString;
		}

		public static string GetFormattedString(AxisFormat value)
		{
			string enumString = String.Empty;

			switch (value)
			{
				case AxisFormat.NoFormat:
					break;
				case AxisFormat.FixedDecimal:
					enumString = "Fixed Decimal";
					break;
				case AxisFormat.Percent:
				case AxisFormat.Commas:
				default:
					enumString = Enum.GetName(typeof(AxisFormat), value);
					break;
			}

			return enumString;
		}

		public static string GetString(ChartType value)
		{
			string enumString = String.Empty;

			switch (value)
			{
				case ChartType.NoChartType:
					break;
				case ChartType.Line:
					enumString = "line";
					break;
				case ChartType.Column:
					enumString = "column";
					break;
				case ChartType.Area:
					enumString = "area";
					break;
				case ChartType.SteppedArea:
					enumString = "steppedArea";
					break;
				case ChartType.Scatter:
					enumString = "scatterChart";
					break;
				default:
					break;
			}

			return enumString;
		}

		public static string GetFormattedString(ChartType value)
		{
			string formattedString = String.Empty;

			switch (value)
			{
				case ChartType.NoChartType:
					break;
				case ChartType.SteppedArea:
					formattedString = "Stepped Area";
					break;
				case ChartType.Scatter:
					formattedString = "Scatter Chart";
					break;
				case ChartType.Line:
				case ChartType.Column:
				case ChartType.Area:
				default:
					formattedString = Enum.GetName(typeof(ChartType), value);
					break;
			}

			return formattedString;
		}

		public static string GetString(DisabledType value)
		{
			string enumString = String.Empty;

			switch (value)
			{
				case DisabledType.NoDisabled:
					enumString = "None Disabled";
					break;
				case DisabledType.Integer:
					enumString = "Integer List";
					break;
				case DisabledType.String:
					enumString = "String List";
					break;
				default:
					break;
			}

			return enumString;
		}

		public static string GetString(DisplayColumn value)
		{
			string enumString = String.Empty;

			switch (value)
			{
				case DisplayColumn.NoColumn:
					break;
				case DisplayColumn.FieldsLeft:
					enumString = "fieldsLeft";
					break;
				case DisplayColumn.FieldsRight:
					enumString = "fieldsRight";
					break;
				case DisplayColumn.AdvancedLeft:
					enumString = "advancedLeft";
					break;
				case DisplayColumn.AdvancedRight:
					enumString = "advancedRight";
					break;
				default:
					break;
			}

			return enumString;
		}

		public static string GetFormattedString(DisplayColumn value)
		{
			string formattedString = String.Empty;

			switch (value)
			{
				case DisplayColumn.NoColumn:
					break;
				case DisplayColumn.FieldsLeft:
					formattedString = "Fields Left";
					break;
				case DisplayColumn.FieldsRight:
					formattedString = "Fields Right";
					break;
				case DisplayColumn.AdvancedLeft:
					formattedString = "Advanced Left";
					break;
				case DisplayColumn.AdvancedRight:
					formattedString = "Advanced Right";
					break;
				default:
					break;
			}

			return formattedString;
		}

		public static string GetString(DisplayType value)
		{
			string enumString = String.Empty;

			switch (value)
			{
				case DisplayType.NoDisplayType:
					break;
				case DisplayType.Filter:
					enumString = "filter";
					break;
				case DisplayType.Parameter:
					enumString = "param";
					break;
				case DisplayType.Selector:
					enumString = "selector";
					break;
				case DisplayType.Cohort:
					enumString = "cohort";
					break;
				case DisplayType.DateRange:
					enumString = "dateRange";
					break;
				case DisplayType.DynamicDateRange:
					enumString = "dynamicDateRange";
					break;
				case DisplayType.DateSelect:
					enumString = "dateSelect";
					break;
				case DisplayType.DateType:
					enumString = "dateType";
					break;
				default:
					break;
			}

			return enumString;
		}

		public static string GetFormattedString(DisplayType value)
		{
			string formattedString = String.Empty;

			switch (value)
			{
				case DisplayType.NoDisplayType:
					break;
				case DisplayType.DateRange:
					formattedString = "Date Range";
					break;
				case DisplayType.DynamicDateRange:
					formattedString = "Dynamic Date Range";
					break;
				case DisplayType.DateSelect:
					formattedString = "Date Select";
					break;
				case DisplayType.DateType:
					formattedString = "Date Type";
					break;
				case DisplayType.Filter:
				case DisplayType.Parameter:
				case DisplayType.Selector:
				case DisplayType.Cohort:
				default:
					formattedString = Enum.GetName(typeof(DisplayType), value);
					break;
			}

			return formattedString;
		}

		public static string GetString(Function value)
		{
			string enumString = String.Empty;

			switch (value)
			{
				case Function.NoFunction:
					break;
				case Function.Rotate:
					enumString = "rotate";
					break;
				case Function.Trim:
					enumString = "trim";
					break;
				case Function.DateRow:
					enumString = "daterow";
					break;
				default:
					break;
			}

			return enumString;
		}

		public static string GetFormattedString(Function value)
		{
			string formattedString = String.Empty;

			switch (value)
			{
				case Function.NoFunction:
					break;
				case Function.DateRow:
					formattedString = "Date Row";
					break;
				case Function.Rotate:
				case Function.Trim:
				default:
					formattedString = Enum.GetName(typeof(Function), value);
					break;
			}

			return formattedString;
		}

		public static string GetFormattedString(HeaderType value)
		{
			string enumValue = String.Empty;

			switch (value)
			{
				case HeaderType.NotAHeader:
					enumValue = "Not a Header";
					break;
				case HeaderType.ColumnName:
					enumValue = "Use Table";
					break;
				case HeaderType.FilterParameterName:
					enumValue = "(=) Use Title";
					break;
				case HeaderType.FilterParameterIndex:
					enumValue = "(*) Use Labels";
					break;
				case HeaderType.DateColumn:
					enumValue = "Date Column";
					break;
				default:
					break;
			}

			return enumValue;
		}

		public static string GetString(HideRow value)
		{
			string enumString = String.Empty;

			switch (value)
			{
				case HideRow.NoHideRow:
					break;
				case HideRow.AnyEmpty:
					enumString = "anyEmpty";
					break;
				case HideRow.AllEmpty:
					enumString = "allEmpty";
					break;
				case HideRow.IgnoreEmpty:
					enumString = "ignoreEmpty";
					break;
				default:
					break;
			}

			return enumString;
		}

		public static string GetFormattedString(HideRow value)
		{
			string formattedString = String.Empty;

			switch (value)
			{
				case HideRow.NoHideRow:
					break;
				case HideRow.AnyEmpty:
					formattedString = "Any Empty";
					break;
				case HideRow.AllEmpty:
					formattedString = "All Empty";
					break;
				case HideRow.IgnoreEmpty:
					formattedString = "Ignore Empty";
					break;
				default:
					break;
			}

			return formattedString;
		}

		public static string GetString(QuarterDate value)
		{
			string enumString = String.Empty;

			switch (value)
			{
				case QuarterDate.NoQuarterDate:
					break;
				case QuarterDate.Start:
					enumString = "start";
					break;
				case QuarterDate.End:
					enumString = "end";
					break;
				default:
					break;
			}

			return enumString;
		}

		public static string GetString(ResultUnavailable value)
		{
			string enumString = String.Empty;

			switch (value)
			{
				case ResultUnavailable.NoResultUnavailable:
					break;
				case ResultUnavailable.Show:
					enumString = "show";
					break;
				case ResultUnavailable.Hide:
					enumString = "hide";
					break;
				case ResultUnavailable.Disable:
					enumString = "disable";
					break;
				default:
					break;
			}

			return enumString;
		}

		public static string GetFormattedString(QuarterDate value)
		{
			return value == QuarterDate.NoQuarterDate ? String.Empty : Enum.GetName(typeof(QuarterDate), value);
		}

		public static string GetFormattedString(ResultUnavailable value)
		{
			return value == ResultUnavailable.NoResultUnavailable ? String.Empty : Enum.GetName(typeof(ResultUnavailable), value);
		}

		public static string GetString(SelectedType value)
		{
			string enumString = String.Empty;

			switch (value)
			{
				case SelectedType.NoSelected:
					enumString = "None Selected";
					break;
				case SelectedType.Integer:
				case SelectedType.String:
				default:
					enumString = Enum.GetName(typeof(SelectedType), value);
					break;
			}

			return enumString;
		}

		public static string GetString(SortType value)
		{
			string enumString = String.Empty;

			switch (value)
			{
				case SortType.NoSortType:
					break;
				case SortType.DefaultSort:
					enumString = "true";
					break;
				case SortType.NotSorted:
					enumString = "false";
					break;
				case SortType.RaceEthnicityCompare:
					enumString = "raceEthnicityCompare";
					break;
				case SortType.QueryTypeCompare:
					enumString = "qryTypeCompare";
					break;
				default:
					break;
			}

			return enumString;
		}

		public static string GetFormattedString(SortType value)
		{
			string formattedString = String.Empty;

			switch (value)
			{
				case SortType.NoSortType:
					break;
				case SortType.DefaultSort:
					formattedString = "Default Sort";
					break;
				case SortType.NotSorted:
					formattedString = "Not Sorted";
					break;
				case SortType.RaceEthnicityCompare:
					formattedString = "Race/Ethnicity Compare";
					break;
				case SortType.QueryTypeCompare:
					formattedString = "Query Type Compare";
					break;
				default:
					break;
			}

			return formattedString;
		}

		public static string GetFormattedString(ValuesType value)
		{
			string enumString = String.Empty;

			switch (value)
			{
				case ValuesType.NoValues:
					enumString = "No Values";
					break;
				case ValuesType.ListIntegers:
					enumString = "Integer List";
					break;
				case ValuesType.ListStrings:
					enumString = "String List";
					break;
				case ValuesType.DictionaryIntegers:
					enumString = "Named Integer Set";
					break;
				case ValuesType.DictionaryStrings:
					enumString = "Named String Set";
					break;
				default:
					break;
			}

			return enumString;
		}

		public static AddInputClass GetAddInputClassEnum(string value)
		{
			AddInputClass enumValue = AddInputClass.NoAddInputClass;

			switch (value)
			{
				case "multiAllowed":
					enumValue = AddInputClass.MultiAllowed;
					break;
				default:
					break;
			}

			return enumValue;
		}

		public static AxisFormat GetAxisFormatEnum(string value)
		{
			AxisFormat enumValue = AxisFormat.NoFormat;

			switch (value)
			{
				case "Percent":
				case "##\'%":
					enumValue = AxisFormat.Percent;
					break;
				case "Fixed Decimal":
				case "##.00":
					enumValue = AxisFormat.FixedDecimal;
					break;
				case "Commas":
				case "#,##0":
					enumValue = AxisFormat.Commas;
					break;
				default:
					break;
			}

			return enumValue;
		}

		public static string[] GetFormattedAxisFormatEnumNames()
		{
			int length = Enum.GetNames(typeof(AxisFormat)).Length;
			string[] formatted = new string[length];

			for (int i = 0; i < length; i++)
				formatted[i] = GetFormattedString((AxisFormat)i);

			return formatted;
		}

		public static ChartType GetChartTypeEnum(string value)
		{
			ChartType enumValue = ChartType.NoChartType;

			switch (value)
			{
				case "Line":
				case "line":
					enumValue = ChartType.Line;
					break;
				case "Column":
				case "column":
					enumValue = ChartType.Column;
					break;
				case "Area":
				case "area":
					enumValue = ChartType.Area;
					break;
				case "Stepped Area":
				case "steppedArea":
					enumValue = ChartType.SteppedArea;
					break;
				case "Scatter Chart":
				case "scatterChart":
					enumValue = ChartType.Scatter;
					break;
				default:
					break;
			}

			return enumValue;
		}

		public static string[] GetFormattedChartTypeEnumNames()
		{
			int length = Enum.GetNames(typeof(ChartType)).Length;
			string[] formatted = new string[length];

			for (int i = 0; i < length; i++)
				formatted[i] = GetFormattedString((ChartType)i);

			return formatted;
		}

		public static ControlType GetControlTypeEnum(string value)
		{
			ControlType enumValue = ControlType.Neither;

			switch (value)
			{
				case "Neither":
					enumValue = ControlType.Neither;
					break;
				case "Control":
					enumValue = ControlType.Control;
					break;
				case "Parameter":
					enumValue = ControlType.Parameter;
					break;
				case "Both":
					enumValue = ControlType.Both;
					break;
				default:
					break;
			}

			return enumValue;
		}

		public static DisplayColumn GetDisplayColumnEnum(string value)
		{
			DisplayColumn enumValue = DisplayColumn.NoColumn;

			switch (value)
			{
				case "Fields Left":
				case "fieldsLeft":
					enumValue = DisplayColumn.FieldsLeft;
					break;
				case "Fields Right":
				case "fieldsRight":
					enumValue = DisplayColumn.FieldsRight;
					break;
				case "Advanced Left":
				case "advancedLeft":
					enumValue = DisplayColumn.AdvancedLeft;
					break;
				case "Advanced Right":
				case "advancedRight":
					enumValue = DisplayColumn.AdvancedRight;
					break;
				default:
					break;
			}

			return enumValue;
		}

		public static string[] GetFormattedDisplayColumnEnumNames()
		{
			int length = Enum.GetNames(typeof(DisplayColumn)).Length;
			string[] formatted = new string[length];

			for (int i = 0; i < length; i++)
				formatted[i] = GetFormattedString((DisplayColumn)i);

			return formatted;
		}

		public static DisplayType GetDisplayTypeEnum(string value)
		{
			DisplayType enumValue = DisplayType.NoDisplayType;

			switch (value)
			{
				case "Filter":
				case "filter":
					enumValue = DisplayType.Filter;
					break;
				case "Parameter":
				case "param":
					enumValue = DisplayType.Parameter;
					break;
				case "Selector":
				case "selector":
					enumValue = DisplayType.Selector;
					break;
				case "Cohort":
				case "cohort":
					enumValue = DisplayType.Cohort;
					break;
				case "Date Range":
				case "dateRange":
					enumValue = DisplayType.DateRange;
					break;
				case "Dynamic Date Range":
				case "dynamicDateRange":
					enumValue = DisplayType.DynamicDateRange;
					break;
				case "Date Select":
				case "dateSelect":
					enumValue = DisplayType.DateSelect;
					break;
				case "Date Type":
				case "dateType":
					enumValue = DisplayType.DateType;
					break;
				default:
					break;
			}

			return enumValue;
		}

		public static string[] GetFormattedDisplayTypeEnumNames()
		{
			int length = Enum.GetNames(typeof(DisplayType)).Length;
			string[] formatted = new string[length];

			for (int i = 0; i < length; i++)
				formatted[i] = GetFormattedString((DisplayType)i);

			return formatted;
		}

		public static Function GetFunctionEnum(string value)
		{
			Function enumValue = Function.NoFunction;

			switch (value)
			{
				case "Rotate":
				case "rotate":
					enumValue = Function.Rotate;
					break;
				case "Trim":
				case "trim":
					enumValue = Function.Trim;
					break;
				case "Date Row":
				case "daterow":
					enumValue = Function.DateRow;
					break;
				default:
					break;
			}

			return enumValue;
		}

		public static string[] GetFormattedFunctionEnumNames()
		{
			int length = Enum.GetNames(typeof(Function)).Length;
			string[] formatted = new string[length];

			for (int i = 0; i < length; i++)
				formatted[i] = GetFormattedString((Function)i);

			return formatted;
		}

		public static HeaderType GetHeaderTypeEnum(string value)
		{
			HeaderType enumValue = HeaderType.NotAHeader;

			switch (value)
			{
				case "Use Table":
					enumValue = HeaderType.ColumnName;
					break;
				case "(=) Use Title":
					enumValue = HeaderType.FilterParameterName;
					break;
				case "(*) Use Labels":
					enumValue = HeaderType.FilterParameterIndex;
					break;
				case "Date Column":
					enumValue = HeaderType.DateColumn;
					break;
				default:
					break;
			}

			return enumValue;
		}

		public static string[] GetFormattedHeaderTypeEnumNames()
		{
			int length = Enum.GetNames(typeof(HeaderType)).Length;
			string[] formatted = new string[length];

			for (int i = 0; i < length; i++)
				formatted[i] = GetFormattedString((HeaderType)i);

			return formatted;
		}

		public static HideRow GetHideRowEnum(string value)
		{
			HideRow enumValue = HideRow.NoHideRow;

			switch (value)
			{
				case "Any Empty":
				case "anyEmpty":
					enumValue = HideRow.AnyEmpty;
					break;
				case "All Empty":
				case "allEmpty":
					enumValue = HideRow.AllEmpty;
					break;
				case "Ignore Empty":
				case "ignoreEmpty":
					enumValue = HideRow.IgnoreEmpty;
					break;
				default:
					break;
			}

			return enumValue;
		}

		public static string[] GetFormattedHideRowEnumNames()
		{
			int length = Enum.GetNames(typeof(HideRow)).Length;
			string[] formatted = new string[length];

			for (int i = 0; i < length; i++)
				formatted[i] = GetFormattedString((HideRow)i);

			return formatted;
		}
		
		public static QuarterDate GetQuarterDateEnum(string value)
		{
			QuarterDate enumValue = QuarterDate.NoQuarterDate;

			switch (value.ToLower())
			{
				case "start":
					enumValue = QuarterDate.Start;
					break;
				case "end":
					enumValue = QuarterDate.End;
					break;
				default:
					break;
			}

			return enumValue;
		}

		public static string[] GetFormattedQuarterDateEnumNames()
		{
			int length = Enum.GetNames(typeof(QuarterDate)).Length;
			string[] formatted = new string[length];

			for (int i = 0; i < length; i++)
				formatted[i] = GetFormattedString((QuarterDate)i);

			return formatted;
		}

		public static ResultUnavailable GetResultUnavailableEnum(string value)
		{
			ResultUnavailable enumValue = ResultUnavailable.NoResultUnavailable;

			switch (value.ToLower())
			{
				case "show":
					enumValue = ResultUnavailable.Show;
					break;
				case "hide":
					enumValue = ResultUnavailable.Hide;
					break;
				case "disable":
					enumValue = ResultUnavailable.Disable;
					break;
				default:
					break;
			}

			return enumValue;
		}

		public static string[] GetFormattedResultUnavailableEnumNames()
		{
			int length = Enum.GetNames(typeof(ResultUnavailable)).Length;
			string[] formatted = new string[length];

			for (int i = 0; i < length; i++)
				formatted[i] = GetFormattedString((ResultUnavailable)i);

			return formatted;
		}

		public static SortType GetSortTypeEnum(string value)
		{
			SortType enumValue = SortType.NoSortType;

			switch (value)
			{
				case "Default Sort":
				case "True":
				case "true":
					enumValue = SortType.DefaultSort;
					break;
				case "Not Sorted":
				case "False":
				case "false":
					enumValue = SortType.NotSorted;
					break;
				case "Race/Ethnicity Compare":
				case "raceEthnicityCompare":
					enumValue = SortType.RaceEthnicityCompare;
					break;
				case "Query Type Compare":
				case "qryTypeCompare":
					enumValue = SortType.QueryTypeCompare;
					break;
				default:
					break;
			}

			return enumValue;
		}

		public static string[] GetFormattedSortTypeEnumNames()
		{
			int length = Enum.GetValues(typeof(SortType)).Length;
			string[] formatted = new string[length];

			for (int i = 0; i < length; i++)
				formatted[i] = GetFormattedString((SortType)i);

			return formatted;
		}

		public static ValuesType GetValuesTypeEnum(string value)
		{
			ValuesType enumValue = ValuesType.NoValues;

			switch (value)
			{
				case "No Values":
				case "NoValues":
					break;
				case "Integer List":
				case "ListIntegers":
					enumValue = ValuesType.ListIntegers;
					break;
				case "String List":
				case "ListStrings":
					enumValue = ValuesType.ListStrings;
					break;
				case "Named Integer Set":
				case "DictionaryIntegers":
					enumValue = ValuesType.DictionaryIntegers;
					break;
				case "Named String Set":
				case "DictionaryStrings":
					enumValue = ValuesType.DictionaryStrings;
					break;
				default:
					break;
			}

			return enumValue;
		}

		public static string[] GetFormattedValuesTypeEnumNames()
		{
			int length = Enum.GetValues(typeof(ValuesType)).Length;
			string[] formatted = new string[length];

			for (int i = 0; i < length; i++)
				formatted[i] = GetFormattedString((ValuesType)i);

			return formatted;
		}
	}
}
