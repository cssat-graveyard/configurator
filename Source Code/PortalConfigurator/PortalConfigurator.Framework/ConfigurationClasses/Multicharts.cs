using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Framework
{
	public class Multicharts : IConfigurationBase
	{
		public Dictionary<string, List<ChartInfo>> Charts { get; set; }
		public bool IsEmpty { get { return CheckForEmpty(); } }

		public Multicharts()
			: this(new Dictionary<string, List<ChartInfo>>())
		{ }

		public Multicharts(Dictionary<string, List<ChartInfo>> charts)
		{
			this.Charts = charts;
		}

		private bool CheckForEmpty()
		{
			return Charts.Count == 0;
		}

		public JObject CompileJson()
		{
			JObject myJson = new JObject();

			foreach (var chart in Charts)
			{
				JArray charts = new JArray();

				foreach (var chartInfo in chart.Value)
					charts.Add(chartInfo.CompileJson());

				myJson.Add(chart.Key, charts);
			}

			return myJson;
		}

		public void ParseJson(JObject json)
		{
			foreach (var property in json.Properties())
			{
				List<ChartInfo> chartInfoList = new List<ChartInfo>();
				foreach (var token in property.Values())
				{
					ChartInfo chartInfo = new ChartInfo();
					chartInfo.ParseJson((JObject)token);
					chartInfoList.Add(chartInfo);
				}
				Charts.Add(property.Name, chartInfoList);
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !this.GetType().Equals(obj.GetType()))
				return false;
			else
			{
				Multicharts m = (Multicharts)obj;
				bool chartsEqual = Charts.Count == m.Charts.Count;

				if (chartsEqual)
				{
					for (int i = 0; i < Charts.Count; i++)
					{
						chartsEqual = Charts.ElementAt(i).Value.SequenceEqual(m.Charts.ElementAt(i).Value);
						chartsEqual = chartsEqual && Charts.ElementAt(i).Key == m.Charts.ElementAt(i).Key;

						if (!chartsEqual)
							break;
					}
				}

				return chartsEqual;
			}
		}

		public override int GetHashCode()
		{
			return Charts.GetHashCode();
		}
	}
}
