using Newtonsoft.Json.Linq;
using System;

namespace Framework
{
	public class NumberFormat : IConfigurationBase
	{
		public string DecimalSymbol { get; set; }
		public int? FractionDigits { get; set; }
		public string GroupingSymbol { get; set; }
		public string NegativeColor { get; set; }
		public bool? NegativeParens { get; set; }
		public string Pattern { get; set; }
		public string Prefix { get; set; }
		public string Suffix { get; set; }
		public bool IsEmpty { get { return CheckIfEmpty(); } }

		public NumberFormat()
			: this(String.Empty, null, String.Empty, String.Empty, null, String.Empty, String.Empty, String.Empty)
		{ }
		
		public NumberFormat(string decimalSymbol, int? fractionDigits, string groupingSymbol, string negativeColor, bool? negativeParens,
			string pattern, string prefix, string suffix)
		{
			this.DecimalSymbol = decimalSymbol;
			this.FractionDigits = fractionDigits;
			this.GroupingSymbol = groupingSymbol;
			this.NegativeColor = negativeColor;
			this.NegativeParens = negativeParens;
			this.Pattern = pattern;
			this.Prefix = prefix;
			this.Suffix = suffix;
		}

		private bool CheckIfEmpty()
		{
			return (DecimalSymbol == null && FractionDigits == null && GroupingSymbol == null && String.IsNullOrEmpty(NegativeColor) && NegativeParens == null &&
				String.IsNullOrEmpty(Pattern) && String.IsNullOrEmpty(Prefix) && String.IsNullOrEmpty(Suffix));
		}

		public JObject CompileJson()
		{
			JObject myJson = new JObject();

			if (!String.IsNullOrEmpty(DecimalSymbol))
				myJson.Add("decimalSymbol", DecimalSymbol);

			if (FractionDigits != null)
				myJson.Add("fractionDigits", FractionDigits);

			if (!String.IsNullOrEmpty(GroupingSymbol))
				myJson.Add("groupingSymbol", GroupingSymbol);

			if (!String.IsNullOrEmpty(NegativeColor))
				myJson.Add("negativeColor", NegativeColor);

			if (NegativeParens != null)
				myJson.Add("negativeParens", NegativeParens);

			if (!String.IsNullOrEmpty(Pattern))
				myJson.Add("pattern", Pattern);

			if (!String.IsNullOrEmpty(Prefix))
				myJson.Add("prefix", Prefix);

			if (!String.IsNullOrEmpty(Suffix))
				myJson.Add("suffix", Suffix);

			return myJson;
		}

		public void ParseJson(JObject json)
		{
			foreach (var property in json.Properties())
			{
				switch (property.Name)
				{
					case "decimalSymbol":
						DecimalSymbol = Json.Parse(DecimalSymbol, property);
						break;
					case "fractionDigits":
						FractionDigits = Json.Parse(FractionDigits, property);
						break;
					case "groupingSymbol":
						GroupingSymbol = Json.Parse(GroupingSymbol, property);
						break;
					case "negativeColor":
						NegativeColor = Json.Parse(NegativeColor, property);
						break;
					case "negativeParens":
						NegativeParens = Json.Parse(NegativeParens, property);
						break;
					case "pattern":
						Pattern = Json.Parse(Pattern, property);
						break;
					case "prefix":
						Prefix = Json.Parse(Prefix, property);
						break;
					case "suffix":
						Suffix = Json.Parse(Suffix, property);
						break;
					default:
						throw new UnknownJsonPropertyException(String.Format("The {0} property is not defined for a Number Format.", property.Name));
				}
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !this.GetType().Equals(obj.GetType()))
				return false;
			else
			{
				NumberFormat nf = (NumberFormat)obj;

				bool decimalSymbolEqual = DecimalSymbol == nf.DecimalSymbol;
				bool fractionDigitsEqual = FractionDigits == nf.FractionDigits;
				bool groupingSymbolEqual = GroupingSymbol == nf.GroupingSymbol;
				bool negativeColorEqual = NegativeColor == nf.NegativeColor;
				bool negativeParensEqual = NegativeParens == nf.NegativeParens;
				bool patternEqual = Pattern == nf.Pattern;
				bool prefixEqual = Prefix == nf.Prefix;
				bool suffixEqual = Suffix == nf.Suffix;

				return decimalSymbolEqual && fractionDigitsEqual && groupingSymbolEqual && negativeColorEqual && negativeColorEqual && patternEqual &&
					prefixEqual && suffixEqual;
			}
		}

		public override int GetHashCode()
		{
			return DecimalSymbol.GetHashCode() ^ FractionDigits.GetHashCode() ^ GroupingSymbol.GetHashCode() ^ NegativeColor.GetHashCode() ^
				NegativeParens.GetHashCode() ^ Pattern.GetHashCode() ^ Prefix.GetHashCode() ^ Suffix.GetHashCode();
		}
	}
}
