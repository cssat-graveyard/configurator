using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework
{
	public class FilterParameter : IConfigurationBase
	{
		private ValuesType _valuesType;
		private Dictionary<string, KeysGridRow> _keysGridRows;
		private bool _keysGridRowsAreUpdated;
		private Dictionary<int, string> _tableColumns;
		private Dictionary<int, string> _tableKeys;
		private bool _tableDataAreLoaded;
		
		public string FilterParameterName { get; set; }
		public FilterParameterType Type { get; set; }
		public Dictionary<string,string> Comments { get; set; }
		public Dictionary<string, string> ValuesDictionary { get; set; }
		public string Table { get; set; }
		public int? ValueColumn { get; set; }
		public int? LagColumn { get; set; }
		public Dictionary<string,string> AddKeyValues { get; set; }
		public List<int> RemoveKeys { get; set; }
		public string OrderBy { get; set; }
		public bool? Date { get; set; }
		public Display Display { get; set; }
		public bool IsEmpty { get { return CheckIfEmpty(); } }

		public ValuesType ValuesType
		{
			get { return _valuesType; }
			set
			{
				_valuesType = value;
				UpdatePropertiesFromKeysGridRows();
			}
		}

		public Dictionary<string, KeysGridRow> KeysGridRows
		{
			get
			{
				if (Type == FilterParameterType.Neither)
				{
					_keysGridRows.Clear();
					_keysGridRowsAreUpdated = true;
				}

				if (!_keysGridRowsAreUpdated)
					RefreshKeysGridRows();

				return _keysGridRows;
			}
			set
			{
				_keysGridRows = value;
				UpdatePropertiesFromKeysGridRows();
			}
		}

		public Dictionary<int, string> TableColumns 
		{
			get 
			{
				LoadTableFromDatabase();
				return _tableColumns;
			}
		}

		private Dictionary<int, string> TableKeys
		{
			get
			{
				LoadTableFromDatabase();
				return _tableKeys;
			}
		}

		public bool TableDataAreLoaded
		{
			get { return _tableDataAreLoaded; }
			set
			{
				_tableDataAreLoaded = value;

				if (!value)
				{
					_tableColumns.Clear();
					_tableKeys.Clear();
				}
			}
		}

		public FilterParameter()
			: this(String.Empty, FilterParameterType.Neither, new Dictionary<string, string>(), ValuesType.NoValues, new Dictionary<string, string>(),
			String.Empty, (int?)null, (int?)null, new Dictionary<string, string>(), new List<int>(), String.Empty, (bool?)null, new Display())
		{ }

		public FilterParameter(string filterParameterName, FilterParameterType type, Dictionary<string,string> comments, ValuesType valuesType,
			Dictionary<string,string> valuesDictionary, string table, int? valueColumn, int? lagColumn, Dictionary<string,string> addKeyValues,
			List<int> removeKeys, string orderBy, bool? date, Display display)
		{
			this.FilterParameterName = filterParameterName;
			this.Type = type;
			this.Comments = comments;
			this._keysGridRows = new Dictionary<string, KeysGridRow>();
			this._keysGridRowsAreUpdated = false;
			this._valuesType = valuesType;
			this.ValuesDictionary = valuesDictionary;
			this.Table = table;
			this._tableColumns = new Dictionary<int, string>();
			this._tableKeys = new Dictionary<int, string>();
			this._tableDataAreLoaded = false;
			this.ValueColumn = valueColumn;
			this.LagColumn = lagColumn;
			this.AddKeyValues = addKeyValues;
			this.RemoveKeys = removeKeys;
			this.OrderBy = orderBy;
			this.Date = date;
			this.Display = display;
		}

		private bool CheckIfEmpty()
		{
			return (String.IsNullOrEmpty(FilterParameterName) && Type == FilterParameterType.Neither && Comments.Count == 0 && _valuesType == ValuesType.NoValues &&
				String.IsNullOrEmpty(Table) && ValueColumn == null && LagColumn == null && AddKeyValues.Count == 0 && RemoveKeys.Count == 0 &&
				String.IsNullOrEmpty(OrderBy) && Date == null && Display.IsEmpty);
		}

		private void LoadTableFromDatabase()
		{
			if ((_tableKeys.Count == 0 || _tableColumns.Count == 0))
			{
				bool tableDataWasUpdated = false;

				try
				{
					if (!String.IsNullOrEmpty(Table) && !_tableDataAreLoaded)
					{
						Dictionary<int, string> columns = new Dictionary<int, string>();
						Dictionary<int, string> keys = new Dictionary<int, string>();

						Database.PopulateFilterParameter(Table, ref columns, ref keys);

						_tableColumns = columns;
						_tableKeys = keys;
					}

					tableDataWasUpdated = true;
				}
				catch (Exception)
				{
					_tableColumns = new Dictionary<int, string>();
					_tableKeys = new Dictionary<int, string>();
					throw;
				}
				finally
				{
					_tableDataAreLoaded = true;
					_keysGridRowsAreUpdated = !tableDataWasUpdated;
				}
			}
		}

		private void RefreshKeysGridRows()
		{
			_keysGridRows.Clear();

			if (Type == FilterParameterType.Values)
			{
				if (_valuesType == ValuesType.ListStrings || _valuesType == ValuesType.ListIntegers)
					foreach (var item in ValuesDictionary)
						_keysGridRows.Add(item.Key, ParseToKeysGridRow(item.Key, item.Key, (bool?)null));
				else if (_valuesType == ValuesType.DictionaryStrings || _valuesType == ValuesType.DictionaryIntegers)
					foreach (var item in ValuesDictionary)
						_keysGridRows.Add(item.Key, ParseToKeysGridRow(item.Key, item.Value, (bool?)null));
			}
			else if (Type == FilterParameterType.Table)
			{
				Display.SelectedType = SelectedType.Integer;
				Display.DisabledType = DisabledType.Integer;

				foreach (var item in TableKeys)
					_keysGridRows.Add(item.Key.ToString(), ParseToKeysGridRow(item.Key.ToString(), item.Value, true));

				foreach (var item in AddKeyValues)
					_keysGridRows.Add(item.Key, ParseToKeysGridRow(item.Key, item.Value, false));
			}

			_keysGridRowsAreUpdated = true;
		}

		private KeysGridRow ParseToKeysGridRow(string key, string value, bool? isFromTable)
		{
			KeysGridRow row = new KeysGridRow();
			int intValue;

			row.Value = value;

			if (int.TryParse(key, out intValue))
				row.IsRemoved = RemoveKeys.Contains(intValue);

			row.IsSelected = Display.SelectedList.Contains(key);
			row.IsDisabled = Display.DisabledList.Contains(key);

			if (Display.Format.ContainsKey(key))
				row.Format = Display.Format[key];

			row.IsFromTable = isFromTable;

			return row;
		}

		public void UpdatePropertiesFromKeysGridRows()
		{
			if (Type == FilterParameterType.Values || Type == FilterParameterType.Neither || _keysGridRows.Count == 0)
				ValuesDictionary.Clear();
			
			if (Type == FilterParameterType.Table || Type == FilterParameterType.Neither || _keysGridRows.Count == 0)
			{
				RemoveKeys.Clear();
				AddKeyValues.Clear();
			}

			Display.SelectedList.Clear();
			Display.SelectedType = SelectedType.NoSelected;
			Display.DisabledList.Clear();
			Display.DisabledType = DisabledType.NoDisabled;
			Display.Format.Clear();

			for (int i = 0; i < _keysGridRows.Count; i++)
			{
				string key = _keysGridRows.Keys.ElementAt(i);
				KeysGridRow value = _keysGridRows.Values.ElementAt(i);

				if (value.IsFromTable == null)
				{
					if (_valuesType == ValuesType.ListStrings || _valuesType == ValuesType.ListIntegers)
						ValuesDictionary.Add(key, key);
					else if (_valuesType == ValuesType.DictionaryStrings || _valuesType == ValuesType.DictionaryIntegers)
						ValuesDictionary.Add(key, value.Value);
				}
				else if (value.IsFromTable == true && value.IsRemoved)
					RemoveKeys.Add(int.Parse(key));
				else if (value.IsFromTable == false)
					AddKeyValues.Add(key, value.Value);

				if (value.IsSelected)
				{
					Display.SelectedList.Add(key);

					if (_valuesType == ValuesType.ListIntegers || _valuesType == ValuesType.DictionaryStrings || Type == FilterParameterType.Table)
						Display.SelectedType = SelectedType.Integer;
					else if (_valuesType == ValuesType.ListStrings || _valuesType == ValuesType.DictionaryIntegers)
						Display.SelectedType = SelectedType.String;
				}

				if (value.IsDisabled)
				{
					Display.DisabledList.Add(key);

					if (_valuesType == ValuesType.ListIntegers || _valuesType == ValuesType.DictionaryStrings || Type == FilterParameterType.Table)
						Display.DisabledType = DisabledType.Integer;
					else if (_valuesType == ValuesType.ListStrings || _valuesType == ValuesType.DictionaryIntegers)
						Display.DisabledType = DisabledType.String;
				}

				if (!String.IsNullOrEmpty(value.Format))
					Display.Format.Add(key, value.Format);
			}
		}

		public List<ValuesType> GetPossibleValuesTypes()
		{
			List<ValuesType> possibleValuesTypes = new List<ValuesType>();
			List<int> intValues = new List<int>();
			int intValue;

			foreach (var item in ValuesDictionary)
				if (int.TryParse(item.Value, out intValue))
					intValues.Add(intValue);

			if (intValues.Count == ValuesDictionary.Count && ValuesDictionary.All(p => p.Key == p.Value))
				possibleValuesTypes.Add(ValuesType.ListIntegers);

			if (ValuesDictionary.All(p => p.Key == p.Value))
				possibleValuesTypes.Add(ValuesType.ListStrings);

			if (intValues.Count == ValuesDictionary.Count)
				possibleValuesTypes.Add(ValuesType.DictionaryIntegers);

			possibleValuesTypes.Add(ValuesType.DictionaryStrings);

			return possibleValuesTypes;
		}

		public JObject CompileJson()
		{
			JObject myJson = new JObject();

			if (Comments.Count != 0)
				foreach (var item in Comments)
					myJson.Add(item.Key, item.Value);

			if (Type == FilterParameterType.Values)
			{
				AddValuesJson(ref myJson);
			}
			else if (Type == FilterParameterType.Table)
			{
				if (!String.IsNullOrEmpty(Table))
					myJson.Add("table", Table);

				if (ValueColumn != null)
					myJson.Add("valueColumn", ValueColumn);

				if (LagColumn != null)
					myJson.Add("lagColumn", LagColumn);

				if (AddKeyValues.Count != 0)
				{
					JObject addKeyValuesProperties = new JObject();

					foreach (var item in AddKeyValues)
						addKeyValuesProperties.Add(item.Key, item.Value);

					myJson.Add("addKeyValues", addKeyValuesProperties);
				}

				if (RemoveKeys.Count != 0)
					myJson.Add("removeKeys", new JArray(RemoveKeys));

				if (!String.IsNullOrEmpty(OrderBy))
				{
					myJson.Add("orderBy", OrderBy);

					if (Display.ZeroLast != null)
						Display.ZeroLast = null;
				}
			}

			if (Date != null)
				myJson.Add("date", Date);

			if (!Display.IsEmpty)
				myJson.Add("display", Display.CompileJson());

			return myJson;
		}

		public void AddValuesJson(ref JObject myJson)
		{
			int intValue;
			Dictionary<string, int> intDictionary = new Dictionary<string, int>();

			foreach (var item in ValuesDictionary)
				if (int.TryParse(item.Value, out intValue))
					intDictionary.Add(item.Key, intValue);

			if (_valuesType == ValuesType.ListIntegers)
			{
				if (intDictionary.Count == ValuesDictionary.Count)
					myJson.Add("values", new JArray(intDictionary.Values.ToList<int>()));
				else
				{
					string message = "Not all values are valid integers.";
					throw new JsonRuleViolationException(message);
				}
			}
			else if (_valuesType == ValuesType.ListStrings)
				myJson.Add("values", new JArray(ValuesDictionary.Keys.ToList<string>()));
			else if (_valuesType == ValuesType.DictionaryIntegers)
			{
				if (intDictionary.Count == ValuesDictionary.Count)
				{
					JObject dictionaryValues = new JObject();

					foreach (var item in intDictionary)
						dictionaryValues.Add(item.Key, item.Value);

					myJson.Add("values", dictionaryValues);
				}
				else
				{
					string message = "Not all values are valid integers.";
					throw new JsonRuleViolationException(message);
				}
			}
			else if (_valuesType == ValuesType.DictionaryStrings)
			{
				JObject dictionaryValues = new JObject();

				foreach (var item in ValuesDictionary)
					dictionaryValues.Add(item.Key, item.Value);

				myJson.Add("values", dictionaryValues);
			}
		}

		public void ParseJson(string name, JObject json)
		{
			FilterParameterName = name;

			foreach (var property in json.Properties())
			{
				if (property.Name.StartsWith("_comment"))
					Comments.Add(property.Name, Json.Parse(String.Empty, property));
				else if (property.Name == "values")
				{
					if (property.Value.Type == JTokenType.Array)
					{
						List<JToken> tokens = property.Values().ToList<JToken>();

						if (tokens.Values().All(p => p.Type == JTokenType.String))
						{
							List<string> values = new List<string>();
							values = Json.Parse(values, property);

							foreach (var item in values)
								ValuesDictionary.Add(item, item);

							_valuesType = ValuesType.ListStrings;
						}
						else if (tokens.Values().All(p => p.Type == JTokenType.Integer))
						{
							List<int> values = new List<int>();
							values = Json.Parse(values, property);

							foreach (var item in values)
								ValuesDictionary.Add(item.ToString(), item.ToString());

							_valuesType = ValuesType.ListIntegers;
						}
					}
					else if (property.Value.Type == JTokenType.Object)
					{
						JObject valuesObject = (JObject)property.Value;

						if (valuesObject.Properties().All(p => p.Value.Type == JTokenType.String))
						{
							ValuesDictionary = Json.Parse(ValuesDictionary, property);
							_valuesType = ValuesType.DictionaryStrings;
						}
						else if (valuesObject.Properties().All(p => p.Value.Type == JTokenType.Integer))
						{
							Dictionary<string, int> values = new Dictionary<string, int>();
							values = Json.Parse(values, property);

							foreach (var item in values)
								ValuesDictionary.Add(item.Key, item.Value.ToString());

							_valuesType = ValuesType.DictionaryIntegers;
						}
					}
				}
				else if (property.Name == "table")
					Table = Json.Parse(Table, property);
				else if (property.Name == "valueColumn")
					ValueColumn = Json.Parse(ValueColumn, property);
				else if (property.Name == "lagColumn")
					LagColumn = Json.Parse(LagColumn, property);
				else if (property.Name == "addKeyValues")
					AddKeyValues = Json.Parse(AddKeyValues, property);
				else if (property.Name == "removeKeys")
					RemoveKeys = Json.Parse(RemoveKeys, property);
				else if (property.Name == "orderBy")
					OrderBy = Json.Parse(OrderBy, property);
				else if (property.Name == "date")
					Date = Json.Parse(Date, property);
				else if (property.Name == "display" && property.Value.Type == JTokenType.Object)
					Display.ParseJson((JObject)property.Value);
				else
				{
					string message = String.Format("The {0} property is not defined for a Filter\\Parameter file.", property.Name);
					throw new UnknownJsonPropertyException(message);
				}
			}

			if (_valuesType != ValuesType.NoValues && !String.IsNullOrEmpty(Table))
			{
				string message = String.Format("The {0} filter\\parameter cannot have both values and a table.", FilterParameterName);
				throw new JsonRuleViolationException(message);
			}
			else if (_valuesType != ValuesType.NoValues)
				Type = FilterParameterType.Values;
			else if (!String.IsNullOrEmpty(Table))
				Type = FilterParameterType.Table;
			else
				Type = FilterParameterType.Neither;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !this.GetType().Equals(obj.GetType()))
				return false;
			else
			{
				FilterParameter fp = (FilterParameter)obj;

				bool filterParameterNameEqual = FilterParameterName == fp.FilterParameterName;
				bool typeEqual = Type.Equals(fp.Type);
				bool commentsEqual = Comments.SequenceEqual(fp.Comments);
				bool valuesTypeEqual = _valuesType == fp.ValuesType;
				bool valuesDictionaryEqual = ValuesDictionary.SequenceEqual(fp.ValuesDictionary);
				bool tableEqual = Table == fp.Table;
				bool valueColumnEqual = ValueColumn == fp.ValueColumn;
				bool lagColumnEqual = LagColumn == fp.LagColumn;
				bool addKeyValuesEqual = AddKeyValues.SequenceEqual(fp.AddKeyValues);
				bool removeKeysEqual = RemoveKeys.SequenceEqual(fp.RemoveKeys);
				bool orderByEqual = OrderBy == fp.OrderBy;
				bool dateEqual = Date == fp.Date;
				bool displayEqual = Display.Equals(fp.Display);

				return filterParameterNameEqual && typeEqual && commentsEqual && valuesTypeEqual && valuesDictionaryEqual && tableEqual && valueColumnEqual &&
					lagColumnEqual && addKeyValuesEqual && removeKeysEqual && orderByEqual && dateEqual && displayEqual;
			}
		}

		public override int GetHashCode()
		{
			return FilterParameterName.GetHashCode() ^ Type.GetHashCode() ^ Comments.GetHashCode() ^ _valuesType.GetHashCode() ^ ValuesDictionary.GetHashCode() ^
				Table.GetHashCode() ^ ValueColumn.GetHashCode() ^ LagColumn.GetHashCode() ^ AddKeyValues.GetHashCode() ^ RemoveKeys.GetHashCode() ^
				OrderBy.GetHashCode() ^ Date.GetHashCode() ^ Display.GetHashCode();
		}
	}
}
