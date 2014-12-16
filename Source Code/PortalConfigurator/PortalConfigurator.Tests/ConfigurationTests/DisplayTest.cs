using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Framework;
using System;

namespace Test
{
	[TestClass]
	public class DisplayTest
	{
		[TestMethod]
		public void DisplayObjectTest()
		{
			string expected = String.Empty;
			string actual = String.Empty;
			Display display = new Display();

			display.Legend = "Graph Display";
			display.DisplayColumn = DisplayColumn.FieldsRight;
			display.DisplayType = DisplayType.DateSelect;
			display.Multi = true;
			display.ZeroLast = true;
			display.SortType = SortType.True;
			display.SortBoolean = true;
			display.SelectedType = SelectedType.Integer;
			display.SelectedList.Add("1");
			display.Alias = "cd_discharge_type";
			display.DisabledType = DisabledType.Integer;
			display.DisabledList.Add("1");
			display.QuarterDate = QuarterDate.Start;
			display.MonthStep = 1;
			display.Visible = true;
			display.ResultUnavailable = ResultUnavailable.Show;
			display.Help.Add("default", "This is a help message.");

			expected += "{\r\n";
			expected += "  \"legend\": \"Graph Display\",\r\n";
			expected += "  \"column\": \"fieldsRight\",\r\n";
			expected += "  \"type\": \"dateSelect\",\r\n";
			expected += "  \"multi\": true,\r\n";
			expected += "  \"zeroLast\": true,\r\n";
			expected += "  \"sort\": true,\r\n";
			expected += "  \"selected\": 1,\r\n";
			expected += "  \"alias\": \"cd_discharge_type\",\r\n";
			expected += "  \"disabled\": [\r\n";
			expected += "    1\r\n";
			expected += "  ],\r\n";
			expected += "  \"quarterDate\": \"start\",\r\n";
			expected += "  \"monthStep\": 1,\r\n";
			expected += "  \"visible\": true,\r\n";
			expected += "  \"resultUnavailable\": \"show\",\r\n";
			expected += "  \"help\": \"This is a help message.\"\r\n";
			expected += "}";

			actual = display.CompileJson().ToString();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void DisplayListTest()
		{
			string expected = String.Empty;
			string actual = String.Empty;
			JObject myJson = new JObject();
			JArray myDisplays = new JArray();
			Display display1 = new Display();
			Display display2 = new Display();
			Display display3 = new Display();
			Display display4 = new Display();

			display1.Legend = "Graph Display 1";
			display1.DisplayColumn = DisplayColumn.FieldsRight;
			display1.DisplayType = DisplayType.DateSelect;
			display1.Multi = true;
			display1.ZeroLast = true;
			display1.SortType = SortType.True;
			display1.SortBoolean = true;
			display1.SelectedType = SelectedType.Integer;
			display1.SelectedList.Add("1");
			display1.Alias = "cd_discharge_type";
			display1.DisabledType = DisabledType.Integer;
			display1.DisabledList.Add("1");
			display1.QuarterDate = QuarterDate.Start;
			display1.MonthStep = 1;
			display1.Visible = true;
			display1.ResultUnavailable = ResultUnavailable.Show;
			display1.Help.Add("default", "This is a help message.");
			myDisplays.Add(display1.CompileJson());

			display2.Legend = "Graph Display 2";
			display2.DisplayColumn = DisplayColumn.FieldsLeft;
			display2.DisplayType = DisplayType.DateRange;
			display2.Multi = false;
			display2.ZeroLast = false;
			display2.SortType = SortType.Function;
			display2.SortFunction = SortFunction.QueryTypeCompare;
			display2.SelectedType = SelectedType.String;
			display2.SelectedList.Add("Overview");
			display2.Alias = "measure_nbr";
			display2.DisabledType = DisabledType.String;
			display2.DisabledList.Add("me");
			display2.QuarterDate = QuarterDate.End;
			display2.MonthStep = 2;
			display2.MonthLimit = 6;
			display2.Visible = false;
			display2.ResultUnavailable = ResultUnavailable.Hide;
			display2.Help.Add("default", "This is a help message.");
			display2.Help.Add("secondary", "Secondary help message.");
			myDisplays.Add(display2.CompileJson());

			display3.Legend = "Graph Display 3";
			display3.DisplayColumn = DisplayColumn.AdvancedLeft;
			display3.DisplayType = DisplayType.Cohort;
			display3.SelectedType = SelectedType.Integer;
			display3.SelectedList.Add("1");
			display3.SelectedList.Add("2");
			display3.ResultUnavailable = ResultUnavailable.Disable;
			myDisplays.Add(display3.CompileJson());

			display4.Legend = "Graph Display 4";
			display4.DisplayColumn = DisplayColumn.AdvancedRight;
			display4.DisplayType = DisplayType.DateType;
			display4.SelectedType = SelectedType.String;
			display4.SelectedList.Add("first");
			display4.SelectedList.Add("second");
			display4.Format.Add("0", "\'MMM yyyy");
			display4.Format.Add("1", "yyyyQQ");
			display4.Format.Add("2", "yyyy");
			myDisplays.Add(display4.CompileJson());

			expected = "{\r\n";
			expected += "  \"display\": [\r\n";
			expected += "    {\r\n";
			expected += "      \"legend\": \"Graph Display 1\",\r\n";
			expected += "      \"column\": \"fieldsRight\",\r\n";
			expected += "      \"type\": \"dateSelect\",\r\n";
			expected += "      \"multi\": true,\r\n";
			expected += "      \"zeroLast\": true,\r\n";
			expected += "      \"sort\": true,\r\n";
			expected += "      \"selected\": 1,\r\n";
			expected += "      \"alias\": \"cd_discharge_type\",\r\n";
			expected += "      \"disabled\": [\r\n";
			expected += "        1\r\n";
			expected += "      ],\r\n";
			expected += "      \"quarterDate\": \"start\",\r\n";
			expected += "      \"monthStep\": 1,\r\n";
			expected += "      \"visible\": true,\r\n";
			expected += "      \"resultUnavailable\": \"show\",\r\n";
			expected += "      \"help\": \"This is a help message.\"\r\n";
			expected += "    },\r\n";
			expected += "    {\r\n";
			expected += "      \"legend\": \"Graph Display 2\",\r\n";
			expected += "      \"column\": \"fieldsLeft\",\r\n";
			expected += "      \"type\": \"dateRange\",\r\n";
			expected += "      \"multi\": false,\r\n";
			expected += "      \"zeroLast\": false,\r\n";
			expected += "      \"sort\": \"qryTypeCompare\",\r\n";
			expected += "      \"selected\": \"Overview\",\r\n";
			expected += "      \"alias\": \"measure_nbr\",\r\n";
			expected += "      \"disabled\": [\r\n";
			expected += "        \"me\"\r\n";
			expected += "      ],\r\n";
			expected += "      \"quarterDate\": \"end\",\r\n";
			expected += "      \"monthStep\": 2,\r\n";
			expected += "      \"monthLimit\": 6,\r\n";
			expected += "      \"visible\": false,\r\n";
			expected += "      \"resultUnavailable\": \"hide\",\r\n";
			expected += "      \"help\": {\r\n";
			expected += "        \"default\": \"This is a help message.\",\r\n";
			expected += "        \"secondary\": \"Secondary help message.\"\r\n";
			expected += "      }\r\n";
			expected += "    },\r\n";
			expected += "    {\r\n";
			expected += "      \"legend\": \"Graph Display 3\",\r\n";
			expected += "      \"column\": \"advancedLeft\",\r\n";
			expected += "      \"type\": \"cohort\",\r\n";
			expected += "      \"selected\": [\r\n";
			expected += "        1,\r\n";
			expected += "        2\r\n";
			expected += "      ],\r\n";
			expected += "      \"resultUnavailable\": \"disable\"\r\n";
			expected += "    },\r\n";
			expected += "    {\r\n";
			expected += "      \"legend\": \"Graph Display 4\",\r\n";
			expected += "      \"column\": \"advancedRight\",\r\n";
			expected += "      \"type\": \"dateType\",\r\n";
			expected += "      \"selected\": [\r\n";
			expected += "        \"first\",\r\n";
			expected += "        \"second\"\r\n";
			expected += "      ],\r\n";
			expected += "      \"format\": {\r\n";
			expected += "        \"0\": \"\'MMM yyyy\",\r\n";
			expected += "        \"1\": \"yyyyQQ\",\r\n";
			expected += "        \"2\": \"yyyy\"\r\n";
			expected += "      }\r\n";
			expected += "    }\r\n";
			expected += "  ]\r\n";
			expected += "}";

			myJson.Add("display", myDisplays);
			actual = myJson.ToString();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void DisplayParseTest()
		{
			Display expected = new Display();
			Display actual = new Display();
			JObject json = new JObject();
			JArray selectedJArray = new JArray();
			JArray disabledJArray = new JArray();
			JObject stringJObject = new JObject();

			expected.Legend = "Graph Display";
			expected.DisplayColumn = DisplayColumn.FieldsRight;
			expected.DisplayType = DisplayType.DateSelect;
			expected.Multi = true;
			expected.ZeroLast = true;
			expected.SortType = SortType.True;
			expected.SortBoolean = true;
			expected.SelectedType = SelectedType.Integer;
			expected.SelectedList.Add("1");
			expected.SelectedList.Add("2");
			expected.Alias = "cd_discharge_type";
			expected.DisabledType = DisabledType.Integer;
			expected.DisabledList.Add("1");
			expected.QuarterDate = QuarterDate.Start;
			expected.MonthStep = 1;
			expected.Visible = true;
			expected.ResultUnavailable = ResultUnavailable.Show;
			expected.Help.Add("default", "First help message.");
			expected.Help.Add("second", "Second help message.");

			json.Add("legend", "Graph Display");
			json.Add("column", "fieldsRight");
			json.Add("type", "dateSelect");
			json.Add("multi", true);
			json.Add("zeroLast", true);
			json.Add("sort", true);
			selectedJArray.Add(1);
			selectedJArray.Add(2);
			json.Add("selected", selectedJArray);
			json.Add("alias", "cd_discharge_type");
			disabledJArray.Add(1);
			json.Add("disabled", disabledJArray);
			json.Add("quarterDate", "start");
			json.Add("monthStep", 1);
			json.Add("visible", true);
			json.Add("resultUnavailable", "show");
			stringJObject.Add("default", "First help message.");
			stringJObject.Add("second", "Second help message.");
			json.Add("help", stringJObject);

			actual.ParseJson(json);

			Assert.AreEqual(expected, actual);
		}
	}
}
