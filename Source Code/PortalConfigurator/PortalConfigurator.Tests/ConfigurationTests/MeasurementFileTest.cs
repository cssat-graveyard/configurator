﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace Test
{
	[TestClass]
	public class MeasurementFileTest
	{
		[TestMethod]
		public void MeasurementFileObjectTest()
		{
			string expected = String.Empty;
			string actual = String.Empty;
			MeasurementFile measurementFile = new MeasurementFile();
			NumberFormat numberFormat = new NumberFormat();
			List<ChartInfo> chartInfoList = new List<ChartInfo>();
			ChartInfo chartInfo = new ChartInfo();
			ChartInfo multichartsChartInfo = new ChartInfo();
			Dictionary<int, string> tableColumns = new Dictionary<int, string>();

			measurementFile.Table = "myTable";
			measurementFile.BaseMeasure = "myBaseMeasure";
			measurementFile.Order = 1;
			measurementFile.Title = "myTitle";
			measurementFile.FullTitle = "myFullTitle";
			measurementFile.Summary = "mySummary";
			measurementFile.Filter = "myFilter";
			measurementFile.Transform.Function = TransformFunction.DateRow;
			measurementFile.Transform.Table = "myTable";
			measurementFile.Transform.DateField = "myDateField";
			measurementFile.Transform.ValueFields.Add("column3");
			measurementFile.Transform.ValueFields.Add("column4");
			measurementFile.Controls.Add("control 1");
			measurementFile.Controls.Add("control 2");
			measurementFile.Parameters.Add("parameter 1");
			measurementFile.Parameters.Add("parameter 2");
			measurementFile.DateParameters.Add("date parameter 1");
			measurementFile.DateParameters.Add("date parameter 2");
			measurementFile.RequiredParameters.Add("required parameter 1");
			measurementFile.RequiredParameters.Add("required parameter 2");
			measurementFile.ShowAllOthers = true;
			measurementFile.MaxChecked = 3;
			measurementFile.MutexAllOthers = true;
			measurementFile.ReturnRowHeaders.Add(0);
			measurementFile.ReturnRowHeaders.Add(1);
			measurementFile.ReturnRowDateHeaders.Add(10);
			measurementFile.ReturnRowDateHeaders.Add(11);
			measurementFile.ReturnRowControlHeaders.Add(20);
			measurementFile.ReturnRowControlHeaders.Add(21);
			measurementFile.HeaderNames.Add("header 1");
			measurementFile.HeaderNames.Add("header 2");
			measurementFile.ReturnRowStart = 2;
			measurementFile.HideRow = HideRow.AllEmpty;
			measurementFile.ColumnClusterSize = 2;
			measurementFile.ChartType = ChartType.Column;
			measurementFile.Label.XAxisLabel = "X Axis";
			measurementFile.Label.YAxisLabel = "Y Axis";
			
			numberFormat.FractionDigits = 0;
			measurementFile.NumberFormats.Add(numberFormat);
			
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
			measurementFile.Charts.Add(chartInfo);

			multichartsChartInfo.ChartType = ChartType.Line;
			multichartsChartInfo.ChartId = "chart1";
			multichartsChartInfo.Label.XAxisLabel = "Months Since Entering";
			multichartsChartInfo.Label.YAxisLabel = "Percentage Achieving Outcome";
			multichartsChartInfo.HideColumns.Add(0);
			chartInfoList.Add(multichartsChartInfo);
			measurementFile.Multicharts.Charts.Add("Default", chartInfoList);

			tableColumns.Add(0, "column1");
			tableColumns.Add(1, "column2");
			tableColumns.Add(2, "column3");
			tableColumns.Add(3, "column4");
			measurementFile.TableColumns = tableColumns;
			measurementFile.TableDataAreLoaded = true;

			expected = "{\r\n";
			expected += "  \"table\": \"myTable\",\r\n";
			expected += "  \"Base\": \"myBaseMeasure\",\r\n";
			expected += "  \"title\": \"myTitle\",\r\n";
			expected += "  \"measurementOrder\": 1,\r\n";
			expected += "  \"fullTitle\": \"myFullTitle\",\r\n";
			expected += "  \"summary\": \"mySummary\",\r\n";
			expected += "  \"filter\": \"myFilter\",\r\n";
			expected += "  \"transform\": {\r\n";
			expected += "    \"Function\": \"daterow\",\r\n";
			expected += "    \"table\": \"myTable\",\r\n";
			expected += "    \"dateField\": \"myDateField\",\r\n";
			expected += "    \"valueFields\": [\r\n";
			expected += "      \"column3\",\r\n";
			expected += "      \"column4\"\r\n";
			expected += "    ],\r\n";
			expected += "    \"removeFields\": []\r\n";
			expected += "  },\r\n";
			expected += "  \"controls\": [\r\n";
			expected += "    \"control 1\",\r\n";
			expected += "    \"control 2\"\r\n";
			expected += "  ],\r\n";
			expected += "  \"params\": [\r\n";
			expected += "    \"parameter 1\",\r\n";
			expected += "    \"parameter 2\"\r\n";
			expected += "  ],\r\n";
			expected += "  \"dateParams\": [\r\n";
			expected += "    \"date parameter 1\",\r\n";
			expected += "    \"date parameter 2\"\r\n";
			expected += "  ],\r\n";
			expected += "  \"requiredParams\": [\r\n";
			expected += "    \"required parameter 1\",\r\n";
			expected += "    \"required parameter 2\"\r\n";
			expected += "  ],\r\n";
			expected += "  \"showAllOthers\": true,\r\n";
			expected += "  \"maxChecked\": 3,\r\n";
			expected += "  \"mutexAllOthers\": true,\r\n";
			expected += "  \"returnRowHeaders\": [\r\n";
			expected += "    0,\r\n";
			expected += "    1\r\n";
			expected += "  ],\r\n";
			expected += "  \"returnRowDateHeaders\": [\r\n";
			expected += "    10,\r\n";
			expected += "    11\r\n";
			expected += "  ],\r\n";
			expected += "  \"returnRowControlHeaders\": [\r\n";
			expected += "    20,\r\n";
			expected += "    21\r\n";
			expected += "  ],\r\n";
			expected += "  \"headerNames\": [\r\n";
			expected += "    \"header 1\",\r\n";
			expected += "    \"header 2\"\r\n";
			expected += "  ],\r\n";
			expected += "  \"returnRowStart\": 2,\r\n";
			expected += "  \"hideRow\": \"allEmpty\",\r\n";
			expected += "  \"columnClusterSize\": 2,\r\n";
			expected += "  \"chartType\": \"column\",\r\n";
			expected += "  \"labels\": {\r\n";
			expected += "    \"x\": \"X Axis\",\r\n";
			expected += "    \"y\": \"Y Axis\"\r\n";
			expected += "  },\r\n";
			expected += "  \"numberFormat\": {\r\n";
			expected += "    \"fractionDigits\": 0\r\n";
			expected += "  },\r\n";
			expected += "  \"charts\": [\r\n";
			expected += "    {\r\n";
			expected += "      \"chartType\": \"column\",\r\n";
			expected += "      \"chartId\": \"chart1\",\r\n";
			expected += "      \"style\": \"\",\r\n";
			expected += "      \"maxSets\": 1,\r\n";
			expected += "      \"baseOptions\": {\r\n";
			expected += "        \"width\": 300,\r\n";
			expected += "        \"height\": 200,\r\n";
			expected += "        \"chartArea\": {\r\n";
			expected += "          \"left\": 10,\r\n";
			expected += "          \"top\": 15,\r\n";
			expected += "          \"width\": \"85%\",\r\n";
			expected += "          \"height\": \"90%\"\r\n";
			expected += "        }\r\n";
			expected += "      },\r\n";
			expected += "      \"labels\": {\r\n";
			expected += "        \"x\": \"X Axis\",\r\n";
			expected += "        \"y\": \"Y Axis\",\r\n";
			expected += "        \"yMin\": 1.0,\r\n";
			expected += "        \"yMax\": 1.0,\r\n";
			expected += "        \"yFormat\": \"#,##0\"\r\n";
			expected += "      },\r\n";
			expected += "      \"hideColumns\": [\r\n";
			expected += "        1,\r\n";
			expected += "        5\r\n";
			expected += "      ],\r\n";
			expected += "      \"addInputClass\": \"multiAllowed\"\r\n";
			expected += "    }\r\n";
			expected += "  ],\r\n";
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

			actual = measurementFile.CompileJson().ToString();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void MeasurementFileParseTest()
		{
			MeasurementFile expected = new MeasurementFile();
			MeasurementFile actual = new MeasurementFile();
			NumberFormat numberFormat = new NumberFormat();
			List<ChartInfo> chartInfoList = new List<ChartInfo>();
			ChartInfo chartInfo = new ChartInfo();
			ChartInfo multichartsChartInfo = new ChartInfo();

			expected.Table = "myTable";
			expected.BaseMeasure = "myBaseMeasure";
			expected.Title = "myTitle";
			expected.FullTitle = "myFullTitle";
			expected.Summary = "mySummary";
			expected.Filter = "myFilter";
			expected.Transform.Function = TransformFunction.DateRow;
			expected.Transform.Table = "myTable";
			expected.Transform.DateField = "myDateField";
			expected.Controls.Add("control 1");
			expected.Controls.Add("control 2");
			expected.Parameters.Add("parameter 1");
			expected.Parameters.Add("parameter 2");
			expected.DateParameters.Add("date parameter 1");
			expected.DateParameters.Add("date parameter 2");
			expected.RequiredParameters.Add("required parameter 1");
			expected.RequiredParameters.Add("required parameter 2");
			expected.ShowAllOthers = true;
			expected.MaxChecked = 3;
			expected.MutexAllOthers = true;
			expected.ReturnRowHeaders.Add(0);
			expected.ReturnRowHeaders.Add(1);
			expected.ReturnRowDateHeaders.Add(10);
			expected.ReturnRowDateHeaders.Add(11);
			expected.ReturnRowControlHeaders.Add(20);
			expected.ReturnRowControlHeaders.Add(21);
			expected.HeaderNames.Add("header 1");
			expected.HeaderNames.Add("header 2");
			expected.ReturnRowStart = 0;
			expected.HideRow = HideRow.AllEmpty;
			expected.ColumnClusterSize = 3;
			expected.ChartType = ChartType.Column;
			expected.Label.XAxisLabel = "X Axis";
			expected.Label.YAxisLabel = "Y Axis";

			numberFormat.FractionDigits = 0;
			expected.NumberFormats.Add(numberFormat);

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
			expected.Charts.Add(chartInfo);

			multichartsChartInfo.ChartType = ChartType.Line;
			multichartsChartInfo.ChartId = "chart1";
			multichartsChartInfo.Label.XAxisLabel = "Months Since Entering";
			multichartsChartInfo.Label.YAxisLabel = "Percentage Achieving Outcome";
			multichartsChartInfo.HideColumns.Add(0);
			chartInfoList.Add(multichartsChartInfo);
			expected.Multicharts.Charts.Add("Default", chartInfoList);

			JObject json = new JObject();
			json.Add("table", "myTable");
			json.Add("Base", "myBaseMeasure");
			json.Add("title", "myTitle");
			json.Add("fullTitle", "myFullTitle");
			json.Add("summary", "mySummary");
			json.Add("filter", "myFilter");
			JObject transformJson = new JObject();
			transformJson.Add("Function", "daterow");
			transformJson.Add("table", "myTable");
			transformJson.Add("dateField", "myDateField");
			transformJson.Add("valueFields", new JArray());
			transformJson.Add("removeFields", new JArray());
			json.Add("transform", transformJson);
			JArray controlsJson = new JArray();
			controlsJson.Add("control 1");
			controlsJson.Add("control 2");
			json.Add("controls", controlsJson);
			JArray parametersJson = new JArray();
			parametersJson.Add("parameter 1");
			parametersJson.Add("parameter 2");
			json.Add("params", parametersJson);
			JArray dateParamsJson = new JArray();
			dateParamsJson.Add("date parameter 1");
			dateParamsJson.Add("date parameter 2");
			json.Add("dateParams", dateParamsJson);
			JArray requiredParamsJson = new JArray();
			requiredParamsJson.Add("required parameter 1");
			requiredParamsJson.Add("required parameter 2");
			json.Add("requiredParams", requiredParamsJson);
			json.Add("showAllOthers", true);
			json.Add("maxChecked", 3);
			json.Add("mutexAllOthers", true);
			JArray returnRowHeadersJson = new JArray();
			returnRowHeadersJson.Add(0);
			returnRowHeadersJson.Add(1);
			json.Add("returnRowHeaders", returnRowHeadersJson);
			JArray returnRowDateHeadersJson = new JArray();
			returnRowDateHeadersJson.Add(10);
			returnRowDateHeadersJson.Add(11);
			json.Add("returnRowDateHeaders", returnRowDateHeadersJson);
			JArray returnRowControlHeadersJson = new JArray();
			returnRowControlHeadersJson.Add(20);
			returnRowControlHeadersJson.Add(21);
			json.Add("returnRowControlHeaders", returnRowControlHeadersJson);
			JArray headerNamesJson = new JArray();
			headerNamesJson.Add("header 1");
			headerNamesJson.Add("header 2");
			json.Add("headerNames", headerNamesJson);
			json.Add("returnRowStart", 0);
			json.Add("hideRow", "allEmpty");
			json.Add("columnClusterSize", 3);
			json.Add("chartType", "column");
			JObject labels1Json = new JObject();
			labels1Json.Add("x", "X Axis");
			labels1Json.Add("y", "Y Axis");
			json.Add("labels", labels1Json);
			JObject numberFormatJson = new JObject();
			numberFormatJson.Add("fractionDigits", 0);
			json.Add("numberFormat", numberFormatJson);
			JArray chartsJson = new JArray();
			JObject chart1Json = new JObject();
			chart1Json.Add("chartType", "column");
			chart1Json.Add("chartId", "chart1");
			chart1Json.Add("style", "");
			chart1Json.Add("maxSets", 1);
			JObject baseOptionsJson = new JObject();
			baseOptionsJson.Add("width", 300);
			baseOptionsJson.Add("height", 200);
			JObject chartAreaJson = new JObject();
			chartAreaJson.Add("left", 10);
			chartAreaJson.Add("top", 15);
			chartAreaJson.Add("width", "85%");
			chartAreaJson.Add("height", "90%");
			baseOptionsJson.Add("chartArea", chartAreaJson);
			chart1Json.Add("baseOptions", baseOptionsJson);
			JObject labels2Json = new JObject();
			labels2Json.Add("x", "X Axis");
			labels2Json.Add("y", "Y Axis");
			labels2Json.Add("yMin", 1.0);
			labels2Json.Add("yMax", 1.0);
			labels2Json.Add("yFormat", "#,##0");
			chart1Json.Add("labels", labels2Json);
			JArray hideColumns1Json = new JArray();
			hideColumns1Json.Add(1);
			hideColumns1Json.Add(5);
			chart1Json.Add("hideColumns", hideColumns1Json);
			chart1Json.Add("addInputClass", "multiAllowed");
			chartsJson.Add(chart1Json);
			json.Add("charts", chartsJson);
			JObject multichartsJson = new JObject();
			JArray multichartsChartsJson = new JArray();
			JObject chart2Json = new JObject();
			chart2Json.Add("chartType", "line");
			chart2Json.Add("chartId", "chart1");
			chart2Json.Add("style", "");
			JObject labels3Json = new JObject();
			labels3Json.Add("x", "Months Since Entering");
			labels3Json.Add("y", "Percentage Achieving Outcome");
			chart2Json.Add("labels", labels3Json);
			JArray hideColumns2Json = new JArray();
			hideColumns2Json.Add(0);
			chart2Json.Add("hideColumns", hideColumns2Json);
			multichartsChartsJson.Add(chart2Json);
			multichartsJson.Add("Default", multichartsChartsJson);
			json.Add("multicharts", multichartsJson);

			actual.FilePath = Environment.CurrentDirectory.Replace(@"\bin\Debug", @"\") + "MeasurementFileTest.Json";
			actual.ParseJson();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void MeasurementFileWriteTest()
		{
			string expected = String.Empty;
			string actual = String.Empty;
			MeasurementFile measurementFile = new MeasurementFile();


			measurementFile.FilePath = Environment.CurrentDirectory.Replace(@"\bin\Debug", @"\") + "MeasurementFileTest.Json";
			measurementFile.ParseJson();
			expected = measurementFile.CompileJson().ToString();
			
			measurementFile.FilePath = Environment.CurrentDirectory.Replace(@"\bin\Debug", @"\") + "MeasurementFileWriteTest.Json";
			measurementFile.WriteFile();
			measurementFile = new MeasurementFile(measurementFile.FilePath);
			measurementFile.ParseJson();
			actual = measurementFile.CompileJson().ToString();

			Assert.AreEqual(actual, expected);
		}
	}
}