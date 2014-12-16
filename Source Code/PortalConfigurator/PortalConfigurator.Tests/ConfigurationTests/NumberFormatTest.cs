using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Framework;
using System;

namespace Test
{
	[TestClass]
	public class NumberFormatTest
	{
		[TestMethod]
		public void NumberFormatObjectTest()
		{
			string expected = String.Empty;
			string actual = String.Empty;
			NumberFormat numberFormat = new NumberFormat();

			numberFormat.DecimalSymbol = ".";
			numberFormat.FractionDigits = 0;
			numberFormat.GroupingSymbol = ",";
			numberFormat.NegativeColor = "red";
			numberFormat.NegativeParens = true;
			numberFormat.Pattern = "#,##0";
			numberFormat.Prefix = "$";
			numberFormat.Suffix = "@";

			expected = "{\r\n";
			expected += "  \"decimalSymbol\": \".\",\r\n";
			expected += "  \"fractionDigits\": 0,\r\n";
			expected += "  \"groupingSymbol\": \",\",\r\n";
			expected += "  \"negativeColor\": \"red\",\r\n";
			expected += "  \"negativeParens\": true,\r\n";
			expected += "  \"pattern\": \"#,##0\",\r\n";
			expected += "  \"prefix\": \"$\",\r\n";
			expected += "  \"suffix\": \"@\"\r\n";
			expected += "}";

			actual = numberFormat.CompileJson().ToString();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void NumberFormatParseTest()
		{
			NumberFormat expected = new NumberFormat();
			NumberFormat actual = new NumberFormat();
			JObject json = new JObject();

			expected.DecimalSymbol = ".";
			expected.FractionDigits = 0;
			expected.GroupingSymbol = ",";
			expected.NegativeColor = "red";
			expected.NegativeParens = true;
			expected.Pattern = "#,##0";
			expected.Prefix = "$";
			expected.Suffix = "@";

			json.Add("decimalSymbol", ".");
			json.Add("fractionDigits", 0);
			json.Add("groupingSymbol", ",");
			json.Add("negativeColor", "red");
			json.Add("negativeParens", true);
			json.Add("pattern", "#,##0");
			json.Add("prefix", "$");
			json.Add("suffix", "@");

			actual.ParseJson(json);

			Assert.AreEqual(expected, actual);
		}
	}
}
