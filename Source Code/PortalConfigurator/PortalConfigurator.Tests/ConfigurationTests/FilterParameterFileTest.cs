using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Framework;
using System;
using System.Linq;

namespace Test
{
	[TestClass]
	public class FilterParameterFileTest
	{
		[TestMethod]
		public void FilterParameterFileObjectTest()
		{
			string expected = String.Empty;
			string actual = String.Empty;
			FilterParameterFile filterParameterFile = new FilterParameterFile();
			FilterParameter filterParameter1 = new FilterParameter();
			FilterParameter filterParameter2 = new FilterParameter();

			filterParameter1.FilterParameterName = "multichart";
			filterParameter1.Type = FilterParameterType.Neither;
			filterParameter1.Comments.Add("_comment1", "This filter MUST be first and must use this name");
			filterParameter1.Comments.Add("_comment2", "The values are computed by the client.");
			filterParameter1.Display.Legend = "Graph Display";
			filterParameter1.Display.DisplayColumn = DisplayColumn.FieldsRight;
			filterParameter1.Display.DisplayType = DisplayType.Filter;
			filterParameter1.Display.Visible = false;
			filterParameter1.Display.Help.Add("default", "Default help message.");
			filterParameter1.Display.Help.Add("second", "Second help message.");
			filterParameterFile.FilterParameters.Add(filterParameter1);

			filterParameter2.FilterParameterName = "age";
			filterParameter2.Type = FilterParameterType.Table;
			filterParameter2.Table = "ref_lookup_age";
			filterParameter2.Display.Legend = "Age at Time of Placement";
			filterParameter2.Display.DisplayColumn = DisplayColumn.FieldsRight;
			filterParameter2.Display.DisplayType = DisplayType.Parameter;
			filterParameter2.Display.Help.Add("default", "Default help message.");
			filterParameterFile.FilterParameters.Add(filterParameter2);

			expected = "{\r\n";
			expected += "  \"multichart\": {\r\n";
			expected += "    \"_comment1\": \"This filter MUST be first and must use this name\",\r\n";
			expected += "    \"_comment2\": \"The values are computed by the client.\",\r\n";
			expected += "    \"display\": {\r\n";
			expected += "      \"legend\": \"Graph Display\",\r\n";
			expected += "      \"column\": \"fieldsRight\",\r\n";
			expected += "      \"type\": \"filter\",\r\n";
			expected += "      \"visible\": false,\r\n";
			expected += "      \"help\": {\r\n";
			expected += "        \"default\": \"Default help message.\",\r\n";
			expected += "        \"second\": \"Second help message.\"\r\n";
			expected += "      }\r\n";
			expected += "    }\r\n";
			expected += "  },\r\n";
			expected += "  \"age\": {\r\n";
			expected += "    \"table\": \"ref_lookup_age\",\r\n";
			expected += "    \"display\": {\r\n";
			expected += "      \"legend\": \"Age at Time of Placement\",\r\n";
			expected += "      \"column\": \"fieldsRight\",\r\n";
			expected += "      \"type\": \"param\",\r\n";
			expected += "      \"help\": \"Default help message.\"\r\n";
			expected += "    }\r\n";
			expected += "  }\r\n";
			expected += "}";

			actual = filterParameterFile.CompileJson().ToString();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void FilterParameterFileParseTest()
		{
			FilterParameterFile expected = new FilterParameterFile();
			FilterParameterFile actual = new FilterParameterFile();
			JObject json = new JObject();
			FilterParameter filterParameter1 = new FilterParameter();
			FilterParameter filterParameter2 = new FilterParameter();

			filterParameter1.FilterParameterName = "multichart";
			filterParameter1.Type = FilterParameterType.Neither;
			filterParameter1.Comments.Add("_comment1", "This filter MUST be first and must use this name");
			filterParameter1.Comments.Add("_comment2", "The values are computed by the client.");
			filterParameter1.Display.Legend = "Graph Display";
			filterParameter1.Display.DisplayColumn = DisplayColumn.FieldsRight;
			filterParameter1.Display.DisplayType = DisplayType.Filter;
			filterParameter1.Display.Visible = false;
			filterParameter1.Display.Help.Add("default", "Default help message.");
			filterParameter1.Display.Help.Add("second", "Second help message.");
			expected.FilterParameters.Add(filterParameter1);

			filterParameter2.FilterParameterName = "age";
			filterParameter2.Type = FilterParameterType.Table;
			filterParameter2.Table = "ref_lookup_age";
			filterParameter2.Display.Legend = "Age at Time of Placement";
			filterParameter2.Display.DisplayColumn = DisplayColumn.FieldsRight;
			filterParameter2.Display.DisplayType = DisplayType.Parameter;
			filterParameter2.Display.Help.Add("default", "Default help message.");
			expected.FilterParameters.Add(filterParameter2);

			actual.FilePath = Environment.CurrentDirectory.Replace(@"\bin\Debug", @"\") + "FilterParameterFileTest.Json";
			actual.ParseJson();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void FilterParameterFileWriteTest()
		{
			string expected = String.Empty;
			string actual = String.Empty;
			FilterParameterFile filterParameterFile = new FilterParameterFile();

			filterParameterFile.FilePath = Environment.CurrentDirectory.Replace(@"\bin\Debug", @"\") + "FilterParameterFileTest.Json";
			filterParameterFile.ParseJson();
			expected = filterParameterFile.CompileJson().ToString();
			
			filterParameterFile.FilePath = Environment.CurrentDirectory.Replace(@"\bin\Debug", @"\") + "FilterParameterFileWriteTest.Json";
			filterParameterFile.WriteFile();
			filterParameterFile = new FilterParameterFile(filterParameterFile.FilePath);
			filterParameterFile.ParseJson();
			actual = filterParameterFile.CompileJson().ToString();

			Assert.AreEqual(expected, actual);
		}
	}
}
