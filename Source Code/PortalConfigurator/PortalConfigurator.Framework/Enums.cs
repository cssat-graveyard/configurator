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
		Area
	}

	public enum ControlType
	{
		Filter,
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

	public enum HeaderType
	{
		NotAHeader,
		ColumnName,
		FilterParameterName,
		FilterParameterIndex
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

	public enum SortFunction
	{
		NoSortFunction,
		RaceEthnicityCompare,
		QueryTypeCompare
	}

	public enum SortType
	{
		NoSort,
		True,
		False,
		Function
	}

	public enum TransformFunction
	{
		NoFunction,
		Rotate,
		Trim,
		DateRow
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
				default:
					break;
			}

			return enumString;
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

		public static string GetString(HeaderType value)
		{
			string enumValue = String.Empty;

			switch (value)
			{
				case HeaderType.NotAHeader:
					enumValue = "Not a Header";
					break;
				case HeaderType.ColumnName:
					enumValue = "Use Table Column";
					break;
				case HeaderType.FilterParameterName:
					enumValue = "(=) Use F/P Name";
					break;
				case HeaderType.FilterParameterIndex:
					enumValue = "(*) Use F/P Name & Values";
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

		public static string GetString(SelectedType value)
		{
			string enumString = String.Empty;

			switch (value)
			{
				case SelectedType.NoSelected:
					enumString = "None Selected";
					break;
				case SelectedType.Integer:
					enumString = "Integer";
					break;
				case SelectedType.String:
					enumString = "String";
					break;
				default:
					break;
			}

			return enumString;
		}

		public static string GetString(SortFunction value)
		{
			string enumString = String.Empty;

			switch (value)
			{
				case SortFunction.NoSortFunction:
					break;
				case SortFunction.RaceEthnicityCompare:
					enumString = "raceEthnicityCompare";
					break;
				case SortFunction.QueryTypeCompare:
					enumString = "qryTypeCompare";
					break;
				default:
					break;
			}

			return enumString;
		}

		public static string GetString(TransformFunction value)
		{
			string enumString = String.Empty;

			switch (value)
			{
				case TransformFunction.NoFunction:
					break;
				case TransformFunction.Rotate:
					enumString = "rotate";
					break;
				case TransformFunction.Trim:
					enumString = "trim";
					break;
				case TransformFunction.DateRow:
					enumString = "daterow";
					break;
				default:
					break;
			}

			return enumString;
		}

		public static string GetString(ValuesType value)
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
				case "##\'%":
					enumValue = AxisFormat.Percent;
					break;
				case "##.00":
					enumValue = AxisFormat.FixedDecimal;
					break;
				case "#,##0":
					enumValue = AxisFormat.Commas;
					break;
				default:
					break;
			}

			return enumValue;
		}

		public static ChartType GetChartTypeEnum(string value)
		{
			ChartType enumValue = ChartType.NoChartType;

			switch (value)
			{
				case "line":
					enumValue = ChartType.Line;
					break;
				case "column":
					enumValue = ChartType.Column;
					break;
				case "area":
					enumValue = ChartType.Area;
					break;
				default:
					break;
			}

			return enumValue;
		}

		public static ControlType GetControlTypeEnum(string value)
		{
			ControlType enumValue = ControlType.Filter;

			switch (value)
			{
				case "Filter":
					enumValue = ControlType.Filter;
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
				case "fieldsLeft":
					enumValue = DisplayColumn.FieldsLeft;
					break;
				case "fieldsRight":
					enumValue = DisplayColumn.FieldsRight;
					break;
				case "advancedLeft":
					enumValue = DisplayColumn.AdvancedLeft;
					break;
				case "advancedRight":
					enumValue = DisplayColumn.AdvancedRight;
					break;
				default:
					break;
			}

			return enumValue;
		}

		public static DisplayType GetDisplayTypeEnum(string value)
		{
			DisplayType enumValue = DisplayType.NoDisplayType;

			switch (value)
			{
				case "filter":
					enumValue = DisplayType.Filter;
					break;
				case "param":
					enumValue = DisplayType.Parameter;
					break;
				case "selector":
					enumValue = DisplayType.Selector;
					break;
				case "cohort":
					enumValue = DisplayType.Cohort;
					break;
				case "dateRange":
					enumValue = DisplayType.DateRange;
					break;
				case "dynamicDateRange":
					enumValue = DisplayType.DynamicDateRange;
					break;
				case "dateSelect":
					enumValue = DisplayType.DateSelect;
					break;
				case "dateType":
					enumValue = DisplayType.DateType;
					break;
				default:
					break;
			}

			return enumValue;
		}

		public static HeaderType GetHeaderTypeEnum(string value)
		{
			HeaderType enumValue = HeaderType.NotAHeader;

			switch (value)
			{
				case "Use Table Column":
					enumValue = HeaderType.ColumnName;
					break;
				case "(=) Use F/P Name":
					enumValue = HeaderType.FilterParameterName;
					break;
				case "(*) Use F/P Name & Values":
					enumValue = HeaderType.FilterParameterIndex;
					break;
				default:
					break;
			}

			return enumValue;
		}

		public static HideRow GetHideRowEnum(string value)
		{
			HideRow enumValue = HideRow.NoHideRow;

			switch (value)
			{
				case "anyEmpty":
					enumValue = HideRow.AnyEmpty;
					break;
				case "allEmpty":
					enumValue = HideRow.AllEmpty;
					break;
				case "ignoreEmpty":
					enumValue = HideRow.IgnoreEmpty;
					break;
				default:
					break;
			}

			return enumValue;
		}
		
		public static QuarterDate GetQuarterDateEnum(string value)
		{
			QuarterDate enumValue = QuarterDate.NoQuarterDate;

			switch (value)
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

		public static ResultUnavailable GetResultUnavailableEnum(string value)
		{
			ResultUnavailable enumValue = ResultUnavailable.NoResultUnavailable;

			switch (value)
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

		public static SortFunction GetSortFunctionEnum(string value)
		{
			SortFunction enumValue = SortFunction.NoSortFunction;

			switch (value)
			{
				case "raceEthnicityCompare":
					enumValue = SortFunction.RaceEthnicityCompare;
					break;
				case "qryTypeCompare":
					enumValue = SortFunction.QueryTypeCompare;
					break;
				default:
					break;
			}

			return enumValue;
		}

		public static TransformFunction GetTransformFunctionEnum(string value)
		{
			TransformFunction enumValue = TransformFunction.NoFunction;

			switch (value)
			{
				case "rotate":
					enumValue = TransformFunction.Rotate;
					break;
				case "trim":
					enumValue = TransformFunction.Trim;
					break;
				case "daterow":
					enumValue = TransformFunction.DateRow;
					break;
				default:
					break;
			}

			return enumValue;
		}
	}
}
