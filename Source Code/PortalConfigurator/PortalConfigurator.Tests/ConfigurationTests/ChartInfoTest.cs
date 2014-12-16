using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Framework;
using System;

namespace Test
{
	[TestClass]
	public class ChartInfoTest
	{
		[TestMethod]
		public void ChartInfoObjectTest()
		{
			string expected = String.Empty;
			string actual = String.Empty;
			ChartInfo chartInfo = new ChartInfo();

			chartInfo.ChartType = ChartType.Column;
			chartInfo.ChartId = "chart1";
			chartInfo.MaxSets = 1;
			chartInfo.BaseOptionSet.Width = 300;
			chartInfo.BaseOptionSet.Height = 200;
			chartInfo.BaseOptionSet.ChartArea.Left = 10;
			chartInfo.BaseOptionSet.ChartArea.Top = 15;
			chartInfo.BaseOptionSet.ChartArea.Width = "85%";
			chartInfo.BaseOptionSet.ChartArea.Height = "90%";
			chartInfo.Label.XAxisLabel = "X Axis";
			chartInfo.Label.YAxisLabel = "Y Axis";
			chartInfo.Label.YAxisMin = 1.0f;
			chartInfo.Label.YAxisMax = 1.0f;
			chartInfo.Label.YAxisFormat = AxisFormat.Commas;
			chartInfo.HideColumns.Add(1);
			chartInfo.HideColumns.Add(5);
			chartInfo.AddInputClass = AddInputClass.MultiAllowed;

			expected = "{\r\n";
			expected += "  \"chartType\": \"column\",\r\n";
			expected += "  \"chartId\": \"chart1\",\r\n";
			expected += "  \"style\": \"\",\r\n";
			expected += "  \"maxSets\": 1,\r\n";
			expected += "  \"baseOptions\": {\r\n";
			expected += "    \"width\": 300,\r\n";
			expected += "    \"height\": 200,\r\n";
			expected += "    \"chartArea\": {\r\n";
			expected += "      \"left\": 10,\r\n";
			expected += "      \"top\": 15,\r\n";
			expected += "      \"width\": \"85%\",\r\n";
			expected += "      \"height\": \"90%\"\r\n";
			expected += "    }\r\n";
			expected += "  },\r\n";
			expected += "  \"labels\": {\r\n";
			expected += "    \"x\": \"X Axis\",\r\n";
			expected += "    \"y\": \"Y Axis\",\r\n";
			expected += "    \"yMin\": 1.0,\r\n";
			expected += "    \"yMax\": 1.0,\r\n";
			expected += "    \"yFormat\": \"#,##0\"\r\n";
			expected += "  },\r\n";
			expected += "  \"hideColumns\": [\r\n";
			expected += "    1,\r\n";
			expected += "    5\r\n";
			expected += "  ],\r\n";
			expected += "  \"addInputClass\": \"multiAllowed\"\r\n";
			expected += "}";

			actual = chartInfo.CompileJson().ToString();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void ChartInfoParseTest()
		{
			ChartInfo expected = new ChartInfo();
			ChartInfo actual = new ChartInfo();
			JObject json = new JObject();
			JObject baseOptionSetJson = new JObject();
			JObject chartAreaJson = new JObject();
			JObject labelJson = new JObject();
			JArray hideColumnsJson = new JArray();

			expected.ChartType = ChartType.Column;
			expected.ChartId = "chart1";
			expected.MaxSets = 1;
			expected.BaseOptionSet.Width = 300;
			expected.BaseOptionSet.Height = 200;
			expected.BaseOptionSet.ChartArea.Left = 10;
			expected.BaseOptionSet.ChartArea.Top = 15;
			expected.BaseOptionSet.ChartArea.Width = "85%";
			expected.BaseOptionSet.ChartArea.Height = "90%";
			expected.Label.XAxisLabel = "X Axis";
			expected.Label.YAxisLabel = "Y Axis";
			expected.Label.YAxisMin = 1.0f;
			expected.Label.YAxisMax = 1.0f;
			expected.Label.YAxisFormat = AxisFormat.Commas;
			expected.HideColumns.Add(1);
			expected.HideColumns.Add(5);
			expected.AddInputClass = AddInputClass.MultiAllowed;

			json.Add("chartType", "column");
			json.Add("chartId", "chart1");
			json.Add("style", "");
			json.Add("maxSets", 1);
			baseOptionSetJson.Add("width", 300);
			baseOptionSetJson.Add("height", 200);
			chartAreaJson.Add("left", 10);
			chartAreaJson.Add("top", 15);
			chartAreaJson.Add("width", "85%");
			chartAreaJson.Add("height", "90%");
			baseOptionSetJson.Add("chartArea", chartAreaJson);
			json.Add("baseOptions", baseOptionSetJson);
			labelJson.Add("x", "X Axis");
			labelJson.Add("y", "Y Axis");
			labelJson.Add("yMin", 1.0);
			labelJson.Add("yMax", 1.0);
			labelJson.Add("yFormat", "#,##0");
			json.Add("labels", labelJson);
			hideColumnsJson.Add(1);
			hideColumnsJson.Add(5);
			json.Add("hideColumns", hideColumnsJson);
			json.Add("addInputClass", "multiAllowed");

			actual.ParseJson(json);

			Assert.AreEqual(expected, actual);
		}
	}
}
