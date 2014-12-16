using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework
{
	public class Transform : IConfigurationBase
	{
		public Function Function { get; set; }
		public string Table { get; set; }
		public string DateField { get; set; }
		public List<string> ValueFields { get; set; }
		public List<string> RemoveFields { get; set; }
		public bool IsEmpty { get { return CheckForEmpty(); } }
		public List<string> TableColumns { get; internal set; }
		internal bool SynchedWithMeasure { get; set; }

		public bool RequiresDateRow
		{
			get { return TransformRule.RequiresDateRow(Function); }
		}

		public bool HasDateRow
		{
			get { return TransformRule.RequiresDateRow(Function) && !String.IsNullOrEmpty(DateField); }
		}

		public Transform()
			: this(Function.NoFunction, String.Empty, String.Empty, new List<string>(), new List<string>())
		{ }

		public Transform(Function function, string table, string dateField, List<string> valueFields, List<string> removeFields)
		{
			this.TableColumns = new List<string>();
			this.Function = function;
			this.Table = table;
			this.DateField = dateField;
			this.ValueFields = valueFields;
			this.RemoveFields = removeFields;
			this.SynchedWithMeasure = false;
		}

		private bool CheckForEmpty()
		{
			return Function == Function.NoFunction && String.IsNullOrEmpty(Table) && String.IsNullOrEmpty(DateField) && ValueFields.Count == 0 && RemoveFields.Count == 0;
		}

		public JObject CompileJson()
		{
			JObject myJson = new JObject();

			if (Function != Function.NoFunction)
				myJson.Add("Function", Enums.GetString(Function));

			if (!String.IsNullOrEmpty(Table) && !SynchedWithMeasure)
				myJson.Add("table", Table);

			if (!String.IsNullOrEmpty(DateField))
				myJson.Add("dateField", DateField);

			myJson.Add("valueFields", new JArray(ValueFields));
			myJson.Add("removeFields", new JArray(RemoveFields));

			return myJson;
		}

		public void ParseJson(JObject json)
		{
			foreach (var property in json.Properties())
			{
				switch (property.Name)
				{
					case "Function":
						Function = Enums.GetFunctionEnum(Json.Parse(String.Empty, property));
						break;
					case "table":
						Table = Json.Parse(Table, property);
						break;
					case "dateField":
						DateField = Json.Parse(DateField, property);
						break;
					case "valueFields":
						ValueFields = Json.Parse(ValueFields, property);
						break;
					case "removeFields":
						RemoveFields = Json.Parse(RemoveFields, property);
						break;
					default:
						throw new UnknownJsonPropertyException(String.Format("The {0} property is not defined for a Transform.", property.Name));
				}
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !this.GetType().Equals(obj.GetType()))
				return false;
			else
			{
				Transform t = (Transform)obj;

				bool functionEqual = Function == t.Function;
				bool tableEqual = Table == t.Table;
				bool dateFieldEqual = DateField == t.DateField;
				bool valueFieldsEqual = ValueFields.SequenceEqual(t.ValueFields);
				bool removeFieldsEqual = RemoveFields.SequenceEqual(t.RemoveFields);

				return functionEqual && tableEqual && dateFieldEqual && valueFieldsEqual && removeFieldsEqual;
			}
		}

		public override int GetHashCode()
		{
			return Function.GetHashCode() ^ Table.GetHashCode() ^ DateField.GetHashCode() ^ ValueFields.GetHashCode() ^ RemoveFields.GetHashCode();
		}
	}

	public static class TransformRule
	{
		public static bool RequiresDateRow(Function function)
		{
			return function == Function.DateRow || function == Function.Trim;
		}
	}
}
