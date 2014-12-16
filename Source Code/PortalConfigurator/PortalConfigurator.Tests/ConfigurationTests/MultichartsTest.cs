using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Framework;
using System;
using System.Collections.Generic;

namespace Test
{
	[TestClass]
	public class MultichartsTest
	{
		[TestMethod]
		public void MultichartsObjectTest()
		{
			string expected = String.Empty;
			string actual = String.Empty;
			JObject myJson = new JObject();
			Multicharts multicharts = new Multicharts();
			List<ChartInfo> chartList = new List<ChartInfo>();
			ChartInfo chart = new ChartInfo();

			chart.ChartType = ChartType.Line;
			chart.ChartId = "chart1";
			chart.Label.XAxisLabel = "Months Since Entering";
			chart.Label.YAxisLabel = "Percentage Achieving Outcome";
			chart.HideColumns.Add(0);
			chartList.Add(chart);
			multicharts.Charts.Add("Default", chartList);

			expected = "{\r\n";
			expected += "  \"multicharts\": {\r\n";
			expected += "    \"Default\": [\r\n";
			expected += "      {\r\n";
			expected += "        \"chartType\": \"line\",\r\n";
			expected += "        \"chartId\": \"chart1\",\r\n";
			expected += "        \"style\": \"\",\r\n";
			expected += "        \"labels\": {\r\n";
			expected += "          \"x\": \"Months Since Entering\",\r\n";
			expected += "          \"y\": \"Percentage Achieving Outcome\"\r\n";
			expected += "        },\r\n";
			expected += "        \"hideColumns\": [\r\n";
			expected += "          0\r\n";
			expected += "        ]\r\n";
			expected += "      }\r\n";
			expected += "    ]\r\n";
			expected += "  }\r\n";
			expected += "}";

			myJson.Add("multicharts", multicharts.CompileJson());
			actual = myJson.ToString();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void MultichartsListTest()
		{
			string expected = String.Empty;
			string actual = String.Empty;
			JObject myJson = new JObject();
			Multicharts multicharts = new Multicharts();
			List<ChartInfo> chartList1 = new List<ChartInfo>();
			List<ChartInfo> chartList2 = new List<ChartInfo>();
			ChartInfo chart1 = new ChartInfo();
			ChartInfo chart2 = new ChartInfo();
			ChartInfo chart3 = new ChartInfo();

			chart1.ChartType = ChartType.Column;
			chart1.ChartId = "chart1";
			chart1.BaseOptionSet.Width = 615;
			chart1.BaseOptionSet.Height = 215;
			chart1.BaseOptionSet.ChartArea.Left = 75;
			chart1.BaseOptionSet.ChartArea.Top = 10;
			chart1.BaseOptionSet.ChartArea.Width = "85%";
			chart1.BaseOptionSet.ChartArea.Height = "90%";
			chart1.HideColumns.Add(1);
			chart1.HideColumns.Add(2);
			chartList1.Add(chart1);

			chart2.ChartType = ChartType.Area;
			chart2.ChartId = "chart2";
			chart2.BaseOptionSet.Width = 615;
			chart2.BaseOptionSet.Height = 215;
			chart2.BaseOptionSet.ChartArea.Left = 75;
			chart2.BaseOptionSet.ChartArea.Top = 10;
			chart2.BaseOptionSet.ChartArea.Width = "85%";
			chart2.BaseOptionSet.ChartArea.Height = "75%";
			chart2.HideColumns.Add(0);
			chartList1.Add(chart2);
			multicharts.Charts.Add("Overview", chartList1);

			chart3.ChartType = ChartType.Column;
			chart3.ChartId = "chart1";
			chart3.HideColumns.Add(1);
			chart3.HideColumns.Add(2);
			chart3.AddInputClass = AddInputClass.MultiAllowed;
			chartList2.Add(chart3);
			multicharts.Charts.Add("All-PPS-Cases-_p1st-Day-Qrtr_sYr_P", chartList2);

			expected = "{\r\n";
			expected += "  \"multicharts\": {\r\n";
			expected += "    \"Overview\": [\r\n";
			expected += "      {\r\n";
			expected += "        \"chartType\": \"column\",\r\n";
			expected += "        \"chartId\": \"chart1\",\r\n";
			expected += "        \"style\": \"\",\r\n";
			expected += "        \"baseOptions\": {\r\n";
			expected += "          \"width\": 615,\r\n";
			expected += "          \"height\": 215,\r\n";
			expected += "          \"chartArea\": {\r\n";
			expected += "            \"left\": 75,\r\n";
			expected += "            \"top\": 10,\r\n";
			expected += "            \"width\": \"85%\",\r\n";
			expected += "            \"height\": \"90%\"\r\n";
			expected += "          }\r\n";
			expected += "        },\r\n";
			expected += "        \"hideColumns\": [\r\n";
			expected += "          1,\r\n";
			expected += "          2\r\n";
			expected += "        ]\r\n";
			expected += "      },\r\n";
			expected += "      {\r\n";
			expected += "        \"chartType\": \"area\",\r\n";
			expected += "        \"chartId\": \"chart2\",\r\n";
			expected += "        \"style\": \"\",\r\n";
			expected += "        \"baseOptions\": {\r\n";
			expected += "          \"width\": 615,\r\n";
			expected += "          \"height\": 215,\r\n";
			expected += "          \"chartArea\": {\r\n";
			expected += "            \"left\": 75,\r\n";
			expected += "            \"top\": 10,\r\n";
			expected += "            \"width\": \"85%\",\r\n";
			expected += "            \"height\": \"75%\"\r\n";
			expected += "          }\r\n";
			expected += "        },\r\n";
			expected += "        \"hideColumns\": [\r\n";
			expected += "          0\r\n";
			expected += "        ]\r\n";
			expected += "      }\r\n";
			expected += "    ],\r\n";
			expected += "    \"All-PPS-Cases-_p1st-Day-Qrtr_sYr_P\": [\r\n";
			expected += "      {\r\n";
			expected += "        \"chartType\": \"column\",\r\n";
			expected += "        \"chartId\": \"chart1\",\r\n";
			expected += "        \"style\": \"\",\r\n";
			expected += "        \"hideColumns\": [\r\n";
			expected += "          1,\r\n";
			expected += "          2\r\n";
			expected += "        ],\r\n";
			expected += "        \"addInputClass\": \"multiAllowed\"\r\n";
			expected += "      }\r\n";
			expected += "    ]\r\n";
			expected += "  }\r\n";
			expected += "}";

			myJson.Add("multicharts", multicharts.CompileJson());
			actual = myJson.ToString();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void MultichartsParseTest()
		{
			Multicharts expected = new Multicharts();
			Multicharts actual = new Multicharts();
			List<ChartInfo> chartList1 = new List<ChartInfo>();
			List<ChartInfo> chartList2 = new List<ChartInfo>();
			ChartInfo chart1 = new ChartInfo();
			ChartInfo chart2 = new ChartInfo();
			ChartInfo chart3 = new ChartInfo();
			JObject json = new JObject();
			JArray chartList1Json = new JArray();
			JArray chartList2Json = new JArray();
			JObject chart1Json = new JObject();
			JObject chart2Json = new JObject();
			JObject chart3Json = new JObject();
			JObject baseOptions1Json = new JObject();
			JObject baseOptions2Json = new JObject();
			JObject chartArea1Json = new JObject();
			JObject chartArea2Json = new JObject();
			JArray hideColumns1Json = new JArray();
			JArray hideColumns2Json = new JArray();
			JArray hideColumns3Json = new JArray();

			chart1.ChartType = ChartType.Column;
			chart1.ChartId = "chart1";
			chart1.BaseOptionSet.Width = 615;
			chart1.BaseOptionSet.Height = 215;
			chart1.BaseOptionSet.ChartArea.Left = 75;
			chart1.BaseOptionSet.ChartArea.Top = 10;
			chart1.BaseOptionSet.ChartArea.Width = "85%";
			chart1.BaseOptionSet.ChartArea.Height = "90%";
			chart1.HideColumns.Add(1);
			chart1.HideColumns.Add(2);
			chartList1.Add(chart1);

			chart2.ChartType = ChartType.Area;
			chart2.ChartId = "chart2";
			chart2.BaseOptionSet.Width = 615;
			chart2.BaseOptionSet.Height = 215;
			chart2.BaseOptionSet.ChartArea.Left = 75;
			chart2.BaseOptionSet.ChartArea.Top = 10;
			chart2.BaseOptionSet.ChartArea.Width = "85%";
			chart2.BaseOptionSet.ChartArea.Height = "75%";
			chart2.HideColumns.Add(0);
			chartList1.Add(chart2);
			expected.Charts.Add("Overview", chartList1);

			chart3.ChartType = ChartType.Column;
			chart3.ChartId = "chart1";
			chart3.HideColumns.Add(1);
			chart3.HideColumns.Add(2);
			chart3.AddInputClass = AddInputClass.MultiAllowed;
			chartList2.Add(chart3);
			expected.Charts.Add("All-PPS-Cases-_p1st-Day-Qrtr_sYr_P", chartList2);

			chart1Json.Add("chartType", "column");
			chart1Json.Add("chartId", "chart1");
			chart1Json.Add("style", "");
			baseOptions1Json.Add("width", 615);
			baseOptions1Json.Add("height", 215);
			chartArea1Json.Add("left", 75);
			chartArea1Json.Add("top", 10);
			chartArea1Json.Add("width", "85%");
			chartArea1Json.Add("height", "90%");
			baseOptions1Json.Add("chartArea", chartArea1Json);
			chart1Json.Add("baseOptions", baseOptions1Json);
			hideColumns1Json.Add(1);
			hideColumns1Json.Add(2);
			chart1Json.Add("hideColumns", hideColumns1Json);
			chartList1Json.Add(chart1Json);
			
			chart2Json.Add("chartType", "area");
			chart2Json.Add("chartId", "chart2");
			chart2Json.Add("style", "");
			baseOptions2Json.Add("width", 615);
			baseOptions2Json.Add("height", 215);
			chartArea2Json.Add("left", 75);
			chartArea2Json.Add("top", 10);
			chartArea2Json.Add("width", "85%");
			chartArea2Json.Add("height", "75%");
			baseOptions2Json.Add("chartArea", chartArea2Json);
			chart2Json.Add("baseOptions", baseOptions2Json);
			hideColumns2Json.Add(0);
			chart2Json.Add("hideColumns", hideColumns2Json);
			chartList1Json.Add(chart2Json);
			json.Add("Overview", chartList1Json);
			
			chart3Json.Add("chartType", "column");
			chart3Json.Add("chartId", "chart1");
			chart3Json.Add("style", "");
			hideColumns3Json.Add(1);
			hideColumns3Json.Add(2);
			chart3Json.Add("hideColumns", hideColumns3Json);
			chart3Json.Add("addInputClass", "multiAllowed");
			chartList2Json.Add(chart3Json);
			json.Add("All-PPS-Cases-_p1st-Day-Qrtr_sYr_P", chartList2Json);

			actual.ParseJson(json);

			Assert.AreEqual(expected, actual);
		}
	}
}
