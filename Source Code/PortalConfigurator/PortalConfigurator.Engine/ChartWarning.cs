using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PortalConfigurator
{
	public static class ChartWarning
	{
		public static bool IsDifferent(Object localValue, Object globalValue)
		{
			bool difference = false;

			if (localValue is ChartType && globalValue is ChartType)
			{
				if ((ChartType)localValue != ChartType.NoChartType && (ChartType)globalValue != ChartType.NoChartType)
					difference = (ChartType)localValue != (ChartType)globalValue;
			}
			else if (localValue is String && globalValue is String)
			{
				if (!String.IsNullOrEmpty(localValue as String) && !String.IsNullOrEmpty(globalValue as String))
					difference = localValue as String != globalValue as String;
			}
			else if (localValue is float? && globalValue is float?)
			{
				if (localValue as float? != null && globalValue as float? != null)
					difference = localValue as float? != globalValue as float?;
			}
			else if (localValue is AxisFormat && globalValue is AxisFormat)
			{
				if ((AxisFormat)localValue != AxisFormat.NoFormat && (AxisFormat)globalValue != AxisFormat.NoFormat)
					difference = (AxisFormat)localValue != (AxisFormat)globalValue;
			}

			return difference;
		}

		public static string GetWarning(ChartLocation location, String propertyName, ChartType chartType, bool isChartType, bool isDifferent)
		{
			string warningMessage = String.Empty;
			string localization = String.Empty;

			switch (location)
			{
				case ChartLocation.Global:
					localization = "in one or more of the charts";
					break;
				case ChartLocation.Local:
					localization = "globally in the measure";
					break;
				default:
					break;
			}

			if (!isChartType && chartType == ChartType.NoChartType)
				warningMessage = "No chart type is selected. Graph rendering errors may occur.";

			if (isDifferent)
				warningMessage = String.Format("A different {0} has been set {1}.\nThis setting may supercede the local chart setting.", propertyName, localization);

			return warningMessage;
		}
	}

	public enum ChartLocation
	{
		Global,
		Local
	}
}
