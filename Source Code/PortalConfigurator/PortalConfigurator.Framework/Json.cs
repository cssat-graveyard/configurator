using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
	public static class Json
	{
		public static string Parse(string field, JProperty property)
		{
			if (property.Value.Type == JTokenType.String)
				field = property.Value.ToString();

			return field;
		}

		public static bool? Parse(bool? field, JProperty property)
		{
			if (property.Value.Type == JTokenType.Boolean)
			{
				try
				{
					field = bool.Parse(property.Value.ToString());
				}
				catch (Exception e)
				{
					string message = String.Format("Property {0} has value \"{1}\" that could not be parsed to type Boolean.",
						property.Name, property.Value);
					throw new JsonPropertyParseException(message, e);
				}
			}

			return field;
		}

		public static int? Parse(int? field, JProperty property)
		{
			if (property.Value.Type == JTokenType.Integer)
			{
				try
				{
					field = int.Parse(property.Value.ToString());
				}
				catch (Exception e)
				{
					string message = String.Format("Property {0} has value \"{1}\" that could not be parsed to type Integer.",
						property.Name, property.Value);
					throw new JsonPropertyParseException(message, e);
				}
			}

			return field;
		}

		public static float? Parse(float? field, JProperty property)
		{
			if (property.Value.Type == JTokenType.Float)
			{
				try
				{
					field = float.Parse(property.Value.ToString());
				}
				catch (Exception e)
				{
					string message = String.Format("Property {0} has value \"{1}\" that could not be parsed to type Float.",
						property.Name, property.Value);
					throw new JsonPropertyParseException(message, e);
				}
			}

			return field;
		}

		public static List<string> Parse(List<string> field, JProperty property)
		{
			if (property.Value.Type == JTokenType.Array)
			{
				List<JToken> valueList = property.Value.ToList();

				if (valueList.Values().All(p => p.Type == JTokenType.String))
					foreach (var value in valueList)
						field.Add(value.ToString());
			}

			return field;
		}

		public static List<int> Parse(List<int> field, JProperty property)
		{
			if (property.Value.Type == JTokenType.Array)
			{
				List<JToken> valueList = property.Value.ToList();

				if (valueList.Values().All(p => p.Type == JTokenType.Integer))
					foreach (var value in valueList)
					{
						try
						{
							field.Add(int.Parse(value.ToString()));
						}
						catch (Exception e)
						{
							string message = String.Format("Property {0} has value \"{1}\" that could not be parsed to type Integer.",
								property.Name, property.Value);
							throw new JsonPropertyParseException(message, e);
						}
					}
			}

			return field;
		}

		public static Dictionary<string,string> Parse(Dictionary<string,string> field, JProperty property)
		{
			if (property.Value.Type == JTokenType.Object)
			{
				JObject dictionaryObject = (JObject)property.Value;
				List<JProperty> propertyList = dictionaryObject.Properties().ToList();

				if (propertyList.Values().All(p => p.Type == JTokenType.String))
					foreach (var subProperty in propertyList)
						field.Add(subProperty.Name, subProperty.Value.ToString());
			}

			return field;
		}

		public static Dictionary<string,int> Parse(Dictionary<string,int> field, JProperty property)
		{
			if (property.Value.Type == JTokenType.Object)
			{
				JObject dictionaryObject = (JObject)property.Value;
				List<JProperty> propertyList = dictionaryObject.Properties().ToList();

				if (propertyList.Values().All(p => p.Type == JTokenType.Integer))
					foreach (var subProperty in propertyList)
					{
						try
						{
							field.Add(subProperty.Name, int.Parse(subProperty.Value.ToString()));
						}
						catch (Exception e)
						{
							string message = String.Format("Property {0}:{1} has value \"{2}\" that could not be parsed to type Integer.",
								property.Name, subProperty.Name, subProperty.Value);
							throw new JsonPropertyParseException(message, e);
						}
					}
			}

			return field;
		}
	}
}
