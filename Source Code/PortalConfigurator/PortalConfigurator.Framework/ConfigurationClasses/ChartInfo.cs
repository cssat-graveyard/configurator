using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework
{
	public class ChartInfo : IConfigurationBase
	{
		public ChartType ChartType { get; set; }
		public string ChartId { get; set; }
		public string ChartStyle { get; set; } // The notes say this attribute was discontinued.
		public int? MaxSets { get; set; }
		public BaseOptionSet BaseOptionSet { get; set; }
		public Label Label { get; set; }
		public List<int> HideColumns { get; set; }
		public AddInputClass AddInputClass { get; set; }
		public bool IsEmpty { get { return CheckForEmpty(); } }

		public ChartInfo()
			: this(ChartType.NoChartType, String.Empty, String.Empty, null, new BaseOptionSet(), new Label(), new List<int>(), AddInputClass.NoAddInputClass)
		{ }

		public ChartInfo(ChartType chartType, string chartId, string chartStyle, int? maxSets, BaseOptionSet baseOptionSet, 
			Label label, List<int> hideColumns, AddInputClass addInputClass)
		{
			this.ChartType = chartType;
			this.ChartId = chartId;
			this.ChartStyle = chartStyle;
			this.MaxSets = maxSets;
			this.BaseOptionSet = baseOptionSet;
			this.Label = label;
			this.HideColumns = hideColumns;
			this.AddInputClass = addInputClass;
		}

		private bool CheckForEmpty()
		{
			return (ChartType == ChartType.NoChartType && String.IsNullOrEmpty(ChartId) && String.IsNullOrEmpty(ChartStyle) && MaxSets == null &&
				BaseOptionSet.IsEmpty && Label.IsEmpty && HideColumns.Count == 0 && AddInputClass == AddInputClass.NoAddInputClass);
		}

		public JObject CompileJson()
		{
			JObject myJson = new JObject();

			if (ChartType != ChartType.NoChartType)
				myJson.Add("chartType", Enums.GetString(ChartType));

			if (!String.IsNullOrEmpty(ChartId))
				myJson.Add("chartId", ChartId);

			myJson.Add("style", ChartStyle);

			if (MaxSets != null)
				myJson.Add("maxSets", MaxSets);

			if (!BaseOptionSet.IsEmpty)
				myJson.Add("baseOptions", BaseOptionSet.CompileJson());

			if (!Label.IsEmpty)
				myJson.Add("labels", Label.CompileJson());

			if (HideColumns.Count != 0)
				myJson.Add("hideColumns", new JArray(HideColumns));

			if (AddInputClass != AddInputClass.NoAddInputClass)
				myJson.Add("addInputClass", Enums.GetString(AddInputClass));

			return myJson;
		}

		public void ParseJson(JObject json)
		{
			foreach (var property in json.Properties())
			{
				if (property.Name == "chartType")
					ChartType = Enums.GetChartTypeEnum(Json.Parse(String.Empty, property));
				else if (property.Name == "chartId")
					ChartId = Json.Parse(ChartId, property);
				else if (property.Name == "style")
					ChartStyle = Json.Parse(ChartStyle, property);
				else if (property.Name == "maxSets")
					MaxSets = Json.Parse(MaxSets, property);
				else if (property.Name == "baseOptions")
					BaseOptionSet.ParseJson((JObject)property.Value);
				else if (property.Name == "labels")
					Label.ParseJson((JObject)property.Value);
				else if (property.Name == "hideColumns")
					HideColumns = Json.Parse(HideColumns, property);
				else if (property.Name == "addInputClass")
					AddInputClass = Enums.GetAddInputClassEnum(Json.Parse(String.Empty, property));
				else
					throw new UnknownJsonPropertyException(String.Format("The {0} property is not defined for a Chart Info.", property.Name));
			}
		}

		public ChartInfo Clone()
		{
			ChartInfo clone = new ChartInfo();
			clone.ParseJson(this.CompileJson());
			return clone;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !this.GetType().Equals(obj.GetType()))
				return false;
			else
			{
				ChartInfo ci = (ChartInfo)obj;

				bool chartTypeEqual = ChartType == ci.ChartType;
				bool chartIdEqual = ChartId == ci.ChartId;
				bool chartStyleEqual = ChartStyle == ci.ChartStyle;
				bool maxSetsEqual = MaxSets == ci.MaxSets;
				bool baseOptionSetEqual = BaseOptionSet.Equals(ci.BaseOptionSet);
				bool labelEqual = Label.Equals(ci.Label);
				bool hideColumnsEqual = HideColumns.SequenceEqual(ci.HideColumns);
				bool addInputClassEqual = AddInputClass == ci.AddInputClass;

				return chartTypeEqual && chartIdEqual && chartStyleEqual && maxSetsEqual && baseOptionSetEqual && labelEqual && hideColumnsEqual &&
					addInputClassEqual;
			}
		}

		public override int GetHashCode()
		{
			return ChartType.GetHashCode() ^ ChartId.GetHashCode() ^ ChartStyle.GetHashCode() ^ MaxSets.GetHashCode() ^ BaseOptionSet.GetHashCode() ^
				Label.GetHashCode() ^ HideColumns.GetHashCode() ^ AddInputClass.GetHashCode();
		}
	}
}
