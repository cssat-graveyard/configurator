using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Framework;
using System;

namespace Test
{
	[TestClass]
	public class ChartAreaTest
	{
		[TestMethod]
		public void ChartAreaObjectTest()
		{
			string expected = String.Empty;
			string actual = String.Empty;
			ChartArea chartArea = new ChartArea();

			chartArea.Left = 10;
			chartArea.Top = 15;
			chartArea.Width = "85%";
			chartArea.Height = "90%";

			expected = "{\r\n";
			expected += "  \"left\": 10,\r\n";
			expected += "  \"top\": 15,\r\n";
			expected += "  \"width\": \"85%\",\r\n";
			expected += "  \"height\": \"90%\"\r\n";
			expected += "}";

			actual = chartArea.CompileJson().ToString();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void ChartAreaParseTest()
		{
			ChartArea expected = new ChartArea();
			ChartArea actual = new ChartArea();
			JObject json = new JObject();

			expected.Left = 10;
			expected.Top = 15;
			expected.Width = "85%";
			expected.Height = "90%";

			json.Add("left", 10);
			json.Add("top", 15);
			json.Add("width", "85%");
			json.Add("height", "90%");

			actual.ParseJson(json);

			Assert.AreEqual(expected, actual);
		}
	}
}
