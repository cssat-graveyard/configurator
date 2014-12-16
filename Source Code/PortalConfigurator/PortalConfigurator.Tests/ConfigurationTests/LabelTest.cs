using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Framework;
using System;

namespace Test
{
	[TestClass]
	public class LabelTest
	{
		[TestMethod]
		public void LabelObjectTest()
		{
			string expected = String.Empty;
			string actual = String.Empty;
			Label label = new Label();

			label.XAxisLabel = "X Axis";
			label.YAxisLabel = "Y Axis";
			label.YAxisMin = 0.0f;
			label.YAxisMax = 0.0f;
			label.YAxisFormat = AxisFormat.Commas;

			expected = "{\r\n";
			expected += "  \"x\": \"X Axis\",\r\n";
			expected += "  \"y\": \"Y Axis\",\r\n";
			expected += "  \"yMin\": 0.0,\r\n";
			expected += "  \"yMax\": 0.0,\r\n";
			expected += "  \"yFormat\": \"#,##0\"\r\n";
			expected += "}";

			actual = label.CompileJson().ToString();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void LabelParseTest()
		{
			Label expected = new Label();
			Label actual = new Label();
			JObject json = new JObject();

			expected.XAxisLabel = "X Axis";
			expected.YAxisLabel = "Y Axis";
			expected.YAxisMin = 0.0f;
			expected.YAxisMax = 0.0f;
			expected.YAxisFormat = AxisFormat.Commas;

			json.Add("x", "X Axis");
			json.Add("y", "Y Axis");
			json.Add("yMin", 0.0);
			json.Add("yMax", 0.0);
			json.Add("yFormat", "#,##0");

			actual.ParseJson(json);

			Assert.AreEqual(expected, actual);
		}
	}
}
