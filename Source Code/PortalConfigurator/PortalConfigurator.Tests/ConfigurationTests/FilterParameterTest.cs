using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Framework;
using System;

namespace Test
{
	[TestClass]
	public class FilterParameterTest
	{
		[TestMethod]
		public void FilterParameterObjectTest()
		{
			string expected = String.Empty;
			string actual = String.Empty;
			FilterParameter filterParameter = new FilterParameter();

			filterParameter.FilterParameterName = "multichart";
			filterParameter.Type = FilterParameterType.Neither;
			filterParameter.Comments.Add("_comment1", "This filter MUST be first and must use this name");
			filterParameter.Comments.Add("_comment2", "The values are computed by the client.");
			filterParameter.Display.Legend = "Graph Display";
			filterParameter.Display.DisplayColumn = DisplayColumn.FieldsRight;
			filterParameter.Display.DisplayType = DisplayType.Filter;
			filterParameter.Display.Visible = false;
			filterParameter.Display.Help.Add("default", "Default help message.");
			filterParameter.Display.Help.Add("secondary", "Secondary help message.");

			expected = "{\r\n";
			expected += "  \"_comment1\": \"This filter MUST be first and must use this name\",\r\n";
			expected += "  \"_comment2\": \"The values are computed by the client.\",\r\n";
			expected += "  \"display\": {\r\n";
			expected += "    \"legend\": \"Graph Display\",\r\n";
			expected += "    \"column\": \"fieldsRight\",\r\n";
			expected += "    \"type\": \"filter\",\r\n";
			expected += "    \"visible\": false,\r\n";
			expected += "    \"help\": {\r\n";
			expected += "      \"default\": \"Default help message.\",\r\n";
			expected += "      \"secondary\": \"Secondary help message.\"\r\n";
			expected += "    }\r\n";
			expected += "  }\r\n";
			expected += "}";

			actual = filterParameter.CompileJson().ToString();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void FilterParameterListTest()
		{
			string expected = String.Empty;
			string actual = String.Empty;
			JObject myJson = new JObject();
			FilterParameter filterParameter1 = new FilterParameter();
			FilterParameter filterParameter2 = new FilterParameter();
			FilterParameter filterParameter3 = new FilterParameter();
			FilterParameter filterParameter4 = new FilterParameter();
			FilterParameter filterParameter5 = new FilterParameter();

			filterParameter1.FilterParameterName = "age";
			filterParameter1.Type = FilterParameterType.Table;
			filterParameter1.Table = "ref_lookup_age";
			filterParameter1.ValueColumn = 2;
			filterParameter1.LagColumn = 0;
			filterParameter1.AddKeyValues.Add("0", "All");
			filterParameter1.RemoveKeys.Add(-99);
			filterParameter1.RemoveKeys.Add(-1);
			filterParameter1.OrderBy = "ordering";
			filterParameter1.Display.Legend = "Age at Time of Placement";
			filterParameter1.Display.DisplayColumn = DisplayColumn.FieldsRight;
			filterParameter1.Display.DisplayType = DisplayType.Parameter;
			filterParameter1.Display.Help.Add("default", "Default help message.");
			myJson.Add(filterParameter1.FilterParameterName, filterParameter1.CompileJson());

			filterParameter2.FilterParameterName = "cohort_year_int";
			filterParameter2.Type = FilterParameterType.Values;
			filterParameter2.ValuesType = ValuesType.ListIntegers;
			filterParameter2.ValuesDictionary.Add("2011", "2011");
			filterParameter2.ValuesDictionary.Add("2012", "2012");
			filterParameter2.Display.Legend = "Entry Cohort Year";
			filterParameter2.Display.DisplayColumn = DisplayColumn.FieldsLeft;
			filterParameter2.Display.DisplayType = DisplayType.Filter;
			filterParameter2.Display.Multi = true;
			filterParameter2.Display.ZeroLast = false;
			filterParameter2.Display.SelectedType = SelectedType.Integer;
			filterParameter2.Display.SelectedList.Add("2011");
			filterParameter2.Display.Visible = false;
			filterParameter2.Display.Help.Add("default", "Default help message.");
			myJson.Add(filterParameter2.FilterParameterName, filterParameter2.CompileJson());

			filterParameter3.FilterParameterName = "nbrSiblings";
			filterParameter3.Type = FilterParameterType.Values;
			filterParameter3.ValuesType = ValuesType.ListStrings;
			filterParameter3.ValuesDictionary.Add("All Others", "All Others");
			filterParameter3.ValuesDictionary.Add("1", "1");
			filterParameter3.ValuesDictionary.Add("2", "2");
			filterParameter3.Display.Legend = "Number of Siblings";
			filterParameter3.Display.DisplayColumn = DisplayColumn.AdvancedLeft;
			filterParameter3.Display.DisplayType = DisplayType.Parameter;
			filterParameter3.Display.Visible = false;
			myJson.Add(filterParameter3.FilterParameterName, filterParameter3.CompileJson());

			filterParameter4.FilterParameterName = "yearPIT";
			filterParameter4.Type = FilterParameterType.Values;
			filterParameter4.ValuesType = ValuesType.DictionaryIntegers;
			filterParameter4.ValuesDictionary.Add("2012", "2012");
			filterParameter4.ValuesDictionary.Add("2013", "2013");
			filterParameter4.Date = true;
			filterParameter4.Display.Legend = "First Day of Year";
			filterParameter4.Display.DisplayColumn = DisplayColumn.FieldsLeft;
			filterParameter4.Display.DisplayType = DisplayType.Filter;
			filterParameter4.Display.Multi = true;
			filterParameter4.Display.SelectedType = SelectedType.String;
			filterParameter4.Display.SelectedList.Add("2013");
			filterParameter4.Display.ResultUnavailable = ResultUnavailable.Disable;
			myJson.Add(filterParameter4.FilterParameterName, filterParameter4.CompileJson());

			filterParameter5.FilterParameterName = "discharge_type_safety";
			filterParameter5.Type = FilterParameterType.Values;
			filterParameter5.ValuesType = ValuesType.DictionaryStrings;
			filterParameter5.ValuesDictionary.Add("1", "Reunification");
			filterParameter5.ValuesDictionary.Add("3", "Adoption");
			filterParameter5.ValuesDictionary.Add("4", "Guardianship");
			filterParameter5.Display.Legend = "Re-Entry by Exit Type";
			filterParameter5.Display.DisplayColumn = DisplayColumn.FieldsRight;
			filterParameter5.Display.DisplayType = DisplayType.Filter;
			filterParameter5.Display.Multi = true;
			filterParameter5.Display.ZeroLast = false;
			filterParameter5.Display.SelectedType = SelectedType.Integer;
			filterParameter5.Display.SelectedList.Add("1");
			filterParameter5.Display.SelectedList.Add("3");
			filterParameter5.Display.SelectedList.Add("4");
			filterParameter5.Display.Alias = "cd_discharge_type";
			filterParameter5.Display.Visible = false;
			filterParameter5.Display.Help.Add("default", "Default help message.");
			myJson.Add(filterParameter5.FilterParameterName, filterParameter5.CompileJson());

			expected = "{\r\n";
			expected += "  \"age\": {\r\n";
			expected += "    \"table\": \"ref_lookup_age\",\r\n";
			expected += "    \"valueColumn\": 2,\r\n";
			expected += "    \"lagColumn\": 0,\r\n";
			expected += "    \"addKeyValues\": {\r\n";
			expected += "      \"0\": \"All\"\r\n";
			expected += "    },\r\n";
			expected += "    \"removeKeys\": [\r\n";
			expected += "      -99,\r\n";
			expected += "      -1\r\n";
			expected += "    ],\r\n";
			expected += "    \"orderBy\": \"ordering\",\r\n";
			expected += "    \"display\": {\r\n";
			expected += "      \"legend\": \"Age at Time of Placement\",\r\n";
			expected += "      \"column\": \"fieldsRight\",\r\n";
			expected += "      \"type\": \"param\",\r\n";
			expected += "      \"help\": \"Default help message.\"\r\n";
			expected += "    }\r\n";
			expected += "  },\r\n";
			expected += "  \"cohort_year_int\": {\r\n";
			expected += "    \"values\": [\r\n";
			expected += "      2011,\r\n";
			expected += "      2012\r\n";
			expected += "    ],\r\n";
			expected += "    \"display\": {\r\n";
			expected += "      \"legend\": \"Entry Cohort Year\",\r\n";
			expected += "      \"column\": \"fieldsLeft\",\r\n";
			expected += "      \"type\": \"filter\",\r\n";
			expected += "      \"multi\": true,\r\n";
			expected += "      \"zeroLast\": false,\r\n";
			expected += "      \"selected\": 2011,\r\n";
			expected += "      \"visible\": false,\r\n";
			expected += "      \"help\": \"Default help message.\"\r\n";
			expected += "    }\r\n";
			expected += "  },\r\n";
			expected += "  \"nbrSiblings\": {\r\n";
			expected += "    \"values\": [\r\n";
			expected += "      \"All Others\",\r\n";
			expected += "      \"1\",\r\n";
			expected += "      \"2\"\r\n";
			expected += "    ],\r\n";
			expected += "    \"display\": {\r\n";
			expected += "      \"legend\": \"Number of Siblings\",\r\n";
			expected += "      \"column\": \"advancedLeft\",\r\n";
			expected += "      \"type\": \"param\",\r\n";
			expected += "      \"visible\": false\r\n";
			expected += "    }\r\n";
			expected += "  },\r\n";
			expected += "  \"yearPIT\": {\r\n";
			expected += "    \"values\": {\r\n";
			expected += "      \"2012\": 2012,\r\n";
			expected += "      \"2013\": 2013\r\n";
			expected += "    },\r\n";
			expected += "    \"date\": true,\r\n";
			expected += "    \"display\": {\r\n";
			expected += "      \"legend\": \"First Day of Year\",\r\n";
			expected += "      \"column\": \"fieldsLeft\",\r\n";
			expected += "      \"type\": \"filter\",\r\n";
			expected += "      \"multi\": true,\r\n";
			expected += "      \"selected\": \"2013\",\r\n";
			expected += "      \"resultUnavailable\": \"disable\"\r\n";
			expected += "    }\r\n";
			expected += "  },\r\n";
			expected += "  \"discharge_type_safety\": {\r\n";
			expected += "    \"values\": {\r\n";
			expected += "      \"1\": \"Reunification\",\r\n";
			expected += "      \"3\": \"Adoption\",\r\n";
			expected += "      \"4\": \"Guardianship\"\r\n";
			expected += "    },\r\n";
			expected += "    \"display\": {\r\n";
			expected += "      \"legend\": \"Re-Entry by Exit Type\",\r\n";
			expected += "      \"column\": \"fieldsRight\",\r\n";
			expected += "      \"type\": \"filter\",\r\n";
			expected += "      \"multi\": true,\r\n";
			expected += "      \"zeroLast\": false,\r\n";
			expected += "      \"selected\": [\r\n";
			expected += "        1,\r\n";
			expected += "        3,\r\n";
			expected += "        4\r\n";
			expected += "      ],\r\n";
			expected += "      \"alias\": \"cd_discharge_type\",\r\n";
			expected += "      \"visible\": false,\r\n";
			expected += "      \"help\": \"Default help message.\"\r\n";
			expected += "    }\r\n";
			expected += "  }\r\n";
			expected += "}";

			actual = myJson.ToString();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void FilterParameterParseTest()
		{
			FilterParameter expected = new FilterParameter();
			FilterParameter actual = new FilterParameter();
			JObject json = new JObject();
			JObject displayJson = new JObject();
			JObject helpJson = new JObject();

			expected.FilterParameterName = "multichart";
			expected.Type = FilterParameterType.Neither;
			expected.Comments.Add("_comment1", "This filter MUST be first and must use this name");
			expected.Comments.Add("_comment2", "The values are computed by the client.");
			expected.Date = true;
			expected.Display.Legend = "Graph Display";
			expected.Display.DisplayColumn = DisplayColumn.FieldsRight;
			expected.Display.DisplayType = DisplayType.Filter;
			expected.Display.Visible = false;
			expected.Display.Help.Add("default", "Default help message.");
			expected.Display.Help.Add("secondary", "Secondary help message.");

			json.Add("_comment1", "This filter MUST be first and must use this name");
			json.Add("_comment2", "The values are computed by the client.");
			json.Add("date", true);
			displayJson.Add("legend", "Graph Display");
			displayJson.Add("column", "fieldsRight");
			displayJson.Add("type", "filter");
			displayJson.Add("visible", false);
			helpJson.Add("default", "Default help message.");
			helpJson.Add("secondary", "Secondary help message.");
			displayJson.Add("help", helpJson);
			json.Add("display", displayJson);

			actual.ParseJson("multichart", json);

			Assert.AreEqual(expected, actual);
		}
	}
}
