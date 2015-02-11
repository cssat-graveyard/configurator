using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Framework;
using System;

namespace Test
{
	[TestClass]
	public class TransformTest
	{
		[TestMethod]
		public void TransformObjectTest()
		{
			string expected = String.Empty;
			string actual = String.Empty;
			Transform transform = new Transform();

			transform.Function = Function.DateRow;
			transform.Table = "sp_table";
			transform.DateField = "Date Field";
			transform.ValueFields.Add("Value Field 1");
			transform.ValueFields.Add("Value Field 2");
			transform.RemoveFields.Add("Remove Field 1");
			transform.RemoveFields.Add("Remove Field 2");

			expected = "{\r\n";
			expected += "  \"Function\": \"daterow\",\r\n";
			expected += "  \"table\": \"sp_table\",\r\n";
			expected += "  \"dateField\": \"Date Field\",\r\n";
			expected += "  \"valueFields\": [\r\n";
			expected += "    \"Value Field 1\",\r\n";
			expected += "    \"Value Field 2\"\r\n";
			expected += "  ],\r\n";
			expected += "  \"removeFields\": [\r\n";
			expected += "    \"Remove Field 1\",\r\n";
			expected += "    \"Remove Field 2\"\r\n";
			expected += "  ]\r\n";
			expected += "}";

			actual = transform.CompileJson().ToString();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TransformParseTest()
		{
			Transform expected = new Transform();
			Transform actual = new Transform();
			JObject json = new JObject();
			JArray valueFieldsJson = new JArray();
			JArray removeFieldsJson = new JArray();

			expected.Function = Function.DateRow;
			expected.Table = "sp_table";
			expected.DateField = "Date Field";
			expected.ValueFields.Add("Value Field 1");
			expected.ValueFields.Add("Value Field 2");
			expected.RemoveFields.Add("Remove Field 1");
			expected.RemoveFields.Add("Remove Field 2");

			json.Add("Function", "daterow");
			json.Add("table", "sp_table");
			json.Add("dateField", "Date Field");
			valueFieldsJson.Add("Value Field 1");
			valueFieldsJson.Add("Value Field 2");
			json.Add("valueFields", valueFieldsJson);
			removeFieldsJson.Add("Remove Field 1");
			removeFieldsJson.Add("Remove Field 2");
			json.Add("removeFields", removeFieldsJson);

			actual.ParseJson(json);

			Assert.AreEqual(expected, actual);
		}
	}
}
