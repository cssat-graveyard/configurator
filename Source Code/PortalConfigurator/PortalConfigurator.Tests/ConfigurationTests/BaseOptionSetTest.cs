using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Framework;
using System;

namespace Test
{
	[TestClass]
	public class BaseOptionSetTest
	{
		[TestMethod]
		public void BaseOptionSetObjectTest()
		{
			string expected = String.Empty;
			string actual = String.Empty;
			BaseOptionSet baseOptionSet = new BaseOptionSet();

			baseOptionSet.Width = 300;
			baseOptionSet.Height = 200;
			baseOptionSet.ChartArea.Left = 10;
			baseOptionSet.ChartArea.Top = 15;
			baseOptionSet.ChartArea.Width = "85%";
			baseOptionSet.ChartArea.Height = "90%";

			expected = "{\r\n";
			expected += "  \"width\": 300,\r\n";
			expected += "  \"height\": 200,\r\n";
			expected += "  \"chartArea\": {\r\n";
			expected += "    \"left\": 10,\r\n";
			expected += "    \"top\": 15,\r\n";
			expected += "    \"width\": \"85%\",\r\n";
			expected += "    \"height\": \"90%\"\r\n";
			expected += "  }\r\n";
			expected += "}";

			actual = baseOptionSet.CompileJson().ToString();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void BaseOptionSetParseTest()
		{
			BaseOptionSet expected = new BaseOptionSet();
			BaseOptionSet actual = new BaseOptionSet();
			JObject json = new JObject();
			JObject chartAreaJson = new JObject();

			expected.Width = 300;
			expected.Height = 200;
			expected.ChartArea.Left = 10;
			expected.ChartArea.Top = 15;
			expected.ChartArea.Width = "85%";
			expected.ChartArea.Height = "90%";

			json.Add("width", 300);
			json.Add("height", 200);
			chartAreaJson.Add("left", 10);
			chartAreaJson.Add("top", 15);
			chartAreaJson.Add("width", "85%");
			chartAreaJson.Add("height", "90%");
			json.Add("chartArea", chartAreaJson);

			actual.ParseJson(json);

			Assert.AreEqual(expected, actual);
		}
	}
}
