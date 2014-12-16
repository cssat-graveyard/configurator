using Newtonsoft.Json.Linq;
using System;

namespace Framework
{
	public class Label : IConfigurationBase
	{
		public string XAxisLabel { get; set; }
		public string YAxisLabel { get; set; }
		public float? YAxisMin { get; set; }
		public float? YAxisMax { get; set; }
		public AxisFormat YAxisFormat { get; set; }
		public bool IsEmpty { get { return CheckIfEmpty(); } }

		public Label()
			: this(String.Empty, String.Empty, null, null, AxisFormat.NoFormat)
		{ }

		public Label(string xAxisLabel, string yAxisLabel, float? yAxisMin, float? yAxisMax, AxisFormat yAxisFormat)
		{
			this.XAxisLabel = xAxisLabel;
			this.YAxisLabel = yAxisLabel;
			this.YAxisMin = yAxisMin;
			this.YAxisMax = yAxisMax;
			this.YAxisFormat = yAxisFormat;
		}

		private bool CheckIfEmpty()
		{
			return String.IsNullOrEmpty(XAxisLabel) && String.IsNullOrEmpty(YAxisLabel) && YAxisMin == null && YAxisMax == null && YAxisFormat == AxisFormat.NoFormat;
		}

		public JObject CompileJson()
		{
			JObject myJson = new JObject();

			myJson.Add("x", XAxisLabel);
			myJson.Add("y", YAxisLabel);

			if (YAxisMin != null)
				myJson.Add("yMin", YAxisMin);

			if (YAxisMax != null)
				myJson.Add("yMax", YAxisMax);

			if (YAxisFormat != AxisFormat.NoFormat)
				myJson.Add("yFormat", Enums.GetString(YAxisFormat));

			return myJson;
		}

		public void ParseJson(JObject json)
		{
			foreach (var property in json.Properties())
			{
				switch (property.Name)
				{
					case "x":
						XAxisLabel = Json.Parse(XAxisLabel, property);
						break;
					case "y":
						YAxisLabel = Json.Parse(YAxisLabel, property);
						break;
					case "yMin":
						YAxisMin = Json.Parse(YAxisMin, property);
						break;
					case "yMax":
						YAxisMax = Json.Parse(YAxisMax, property);
						break;
					case "yFormat":
						YAxisFormat = Enums.GetAxisFormatEnum(Json.Parse(String.Empty, property));
						break;
					default:
						throw new UnknownJsonPropertyException(String.Format("The {0} property is not defined for a Label.", property.Name));
				}
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !this.GetType().Equals(obj.GetType()))
				return false;
			else
			{
				Label l = (Label)obj;

				bool xAxisLabelEqual = XAxisLabel == l.XAxisLabel;
				bool yAxisLabelEqual = YAxisLabel == l.YAxisLabel;
				bool yAxisMinEqual = YAxisMin == l.YAxisMin;
				bool yAxisMaxEqual = YAxisMax == l.YAxisMax;
				bool yAxisFormatEqual = YAxisFormat == l.YAxisFormat;

				return xAxisLabelEqual && yAxisLabelEqual && yAxisMinEqual && yAxisMaxEqual && yAxisFormatEqual;
			}
		}

		public override int GetHashCode()
		{
			return XAxisLabel.GetHashCode() ^ YAxisLabel.GetHashCode() ^ YAxisMin.GetHashCode() ^ YAxisMax.GetHashCode() ^ YAxisFormat.GetHashCode();
		}
	}
}
