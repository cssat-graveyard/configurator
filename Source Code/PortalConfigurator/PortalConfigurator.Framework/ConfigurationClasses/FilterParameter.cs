using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Framework
{
	public class FilterParameter : IConfigurationBase
	{
		private bool _keysGridRowsAreUpdated;
		
		public string FilterParameterName { get; set; }
		public FilterParameterType Type { get; set; }
		public Dictionary<string,string> Comments { get; set; }
		public Dictionary<string, string> ValuesDictionary { get; set; }
		public int? LagColumn { get; set; }
		public Dictionary<string,string> AddKeyValues { get; set; }
		public List<int> RemoveKeys { get; set; }
		public string OrderBy { get; set; }
		public bool? Date { get; set; }
		public Display Display { get; set; }
		public bool IsEmpty { get { return CheckIfEmpty(); } }

		private string _table;
		public string Table
		{
			get { return _table; }
			set
			{
				if (_table != value)
				{
					_table = value;
					OrderBy = String.Empty;
					_valueColumn = null;
					LagColumn = null;
					TableDataAreLoaded = false;
				}
			}
		}

		private ValuesType _valuesType;
		public ValuesType ValuesType
		{
			get { return _valuesType; }
			set
			{
				_valuesType = value;
				UpdatePropertiesFromKeysGridRows();
			}
		}

		private int? _valueColumn;
		public int? ValueColumn
		{
			get { return _valueColumn; }
			set
			{
				if (_valueColumn != value)
				{
					_valueColumn = value;
					TableDataAreLoaded = false;
					_keysGridRowsAreUpdated = false;
				}
			}
		}
		
		private Dictionary<string, ValuesGridRow> _valuesGridRows;
		public Dictionary<string, ValuesGridRow> ValuesGridRows
		{
			get
			{
				if (Type == FilterParameterType.Neither)
				{
					_valuesGridRows.Clear();
					_keysGridRowsAreUpdated = true;
				}

				if (!_keysGridRowsAreUpdated)
					RefreshKeysGridRows();

				return _valuesGridRows;
			}
			set
			{
				_valuesGridRows = value;
				UpdatePropertiesFromKeysGridRows();
			}
		}

		private Dictionary<int, string> _tableColumns;
		public Dictionary<int, string> TableColumns 
		{
			get 
			{
				LoadTableFromDatabase();
				return _tableColumns;
			}
		}

		private Dictionary<int, string> _tableKeys;
		private Dictionary<int, string> TableKeys
		{
			get
			{
				LoadTableFromDatabase();
				return _tableKeys;
			}
		}

		private bool _tableDataAreLoaded;
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
			this._valuesGridRows = new Dictionary<string, ValuesGridRow>();
			this._keysGridRowsAreUpdated = false;
			this._valuesType = valuesType;
			this.ValuesDictionary = valuesDictionary;
			this._table = table;
			this._tableColumns = new Dictionary<int, string>();
			this._tableKeys = new Dictionary<int, string>();
			this._tableDataAreLoaded = false;
			this._valueColumn = valueColumn;
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
			if ((_tableKeys.Count == 0 || _tableColumns.Count == 0) && Type == FilterParameterType.Table)
			{
				bool tableDataWasUpdated = false;

				try
				{
					if (!String.IsNullOrEmpty(Table) && !_tableDataAreLoaded && Database.Tables.ContainsKey(Table))
					{
						DataTableReader reader = Database.Tables[Table].Values.CreateDataReader();
						_tableColumns = Database.Tables[Table].Columns.ToDictionary(k => k.Key, v => v.Value);
						int valueColumn = (ValueColumn ?? 1) < _tableColumns.Count ? ValueColumn ?? 1 : 1;
						_tableKeys.Clear();

						while (reader.Read())
						{
							int key = reader.GetInt32(0);
							string value = reader.GetValue(valueColumn).ToString();
							_tableKeys.Add(key, value);
						}

						reader.Close();
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
			_valuesGridRows.Clear();

			if (Type == FilterParameterType.Values)
			{
				if (_valuesType == ValuesType.ListStrings || _valuesType == ValuesType.ListIntegers)
					foreach (var item in ValuesDictionary)
						_valuesGridRows.Add(item.Key, ParseToKeysGridRow(item.Key, item.Key, (bool?)null));
				else if (_valuesType == ValuesType.DictionaryStrings || _valuesType == ValuesType.DictionaryIntegers)
					foreach (var item in ValuesDictionary)
						_valuesGridRows.Add(item.Key, ParseToKeysGridRow(item.Key, item.Value, (bool?)null));
			}
			else if (Type == FilterParameterType.Table)
			{
				Display.SelectedType = Display.SelectedList.Count == 0 ? SelectedType.NoSelected : SelectedType.Integer;
				Display.DisabledType = Display.DisabledList.Count == 0 ? DisabledType.NoDisabled : DisabledType.Integer;

				foreach (var item in TableKeys)
					_valuesGridRows.Add(item.Key.ToString(), ParseToKeysGridRow(item.Key.ToString(), item.Value, true));

				foreach (var item in AddKeyValues.Where(p => !_valuesGridRows.ContainsKey(p.Key)))
					_valuesGridRows.Add(item.Key, ParseToKeysGridRow(item.Key, item.Value, false));
			}

			_keysGridRowsAreUpdated = true;
		}

		private ValuesGridRow ParseToKeysGridRow(string key, string value, bool? isFromTable)
		{
			ValuesGridRow row = new ValuesGridRow();
			int intValue;

			row.Name = value;

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
			if ((Type == FilterParameterType.Values || Type == FilterParameterType.Neither || _valuesGridRows.Count == 0) && ValuesDictionary.Count != 0)
				ValuesDictionary.Clear();
			
			if (Type == FilterParameterType.Table || Type == FilterParameterType.Neither || _valuesGridRows.Count == 0)
			{
				if (RemoveKeys.Count != 0)
					RemoveKeys.Clear();
				
				if (AddKeyValues.Count != 0)
					AddKeyValues.Clear();
			}

			if (Type == FilterParameterType.Values && _valuesGridRows.Count != 0 && _valuesType == ValuesType.NoValues)
				_valuesType = ValuesType.DictionaryStrings;
			else if (Type == FilterParameterType.Values && _valuesGridRows.Count == 0 && _valuesType != ValuesType.NoValues)
				_valuesType = ValuesType.NoValues;

			if (Display.SelectedList.Count != 0)
				Display.SelectedList.Clear();

			if (_valuesGridRows.Count(p => p.Value.IsSelected) == 0)
				Display.SelectedType = SelectedType.NoSelected;
			
			if (Display.DisabledList.Count != 0)
				Display.DisabledList.Clear();
			
			if (_valuesGridRows.Count(p => p.Value.IsDisabled) == 0)
				Display.DisabledType = DisabledType.NoDisabled;
			
			Display.Format.Clear();

			for (int i = 0; i < _valuesGridRows.Count; i++)
			{
				string key = _valuesGridRows.Keys.ElementAt(i);
				ValuesGridRow value = _valuesGridRows.Values.ElementAt(i);

				if (value.IsFromTable == null)
				{
					if (_valuesType == ValuesType.ListStrings || _valuesType == ValuesType.ListIntegers)
					{
						ValuesDictionary.Add(key, key);
						value.Name = key;
					}
					else if (_valuesType == ValuesType.DictionaryStrings || _valuesType == ValuesType.DictionaryIntegers)
						ValuesDictionary.Add(key, value.Name);
				}
				else if (value.IsFromTable == true && value.IsRemoved)
					RemoveKeys.Add(int.Parse(key));
				else if (value.IsFromTable == false)
					AddKeyValues.Add(key, value.Name);

				if (value.IsSelected)
				{
					Display.SelectedList.Add(key);

					if ((_valuesType == ValuesType.ListIntegers || _valuesType == ValuesType.DictionaryStrings || Type == FilterParameterType.Table) && 
						Display.SelectedType != SelectedType.Integer)
						Display.SelectedType = SelectedType.Integer;
					else if ((_valuesType == ValuesType.ListStrings || _valuesType == ValuesType.DictionaryIntegers) && Display.SelectedType != SelectedType.String)
						Display.SelectedType = SelectedType.String;
				}

				if (value.IsDisabled)
				{
					Display.DisabledList.Add(key);

					if ((_valuesType == ValuesType.ListIntegers || _valuesType == ValuesType.DictionaryStrings || Type == FilterParameterType.Table) &&
						Display.DisabledType != DisabledType.Integer)
						Display.DisabledType = DisabledType.Integer;
					else if ((_valuesType == ValuesType.ListStrings || _valuesType == ValuesType.DictionaryIntegers) && Display.DisabledType != DisabledType.String)
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

			if (FilterParameterName == "multichart")
			{
				Display.SelectedType = SelectedType.String;
				Display.SelectedList.Add("Overview");
			}

			myJson.Add("display", Display.CompileJson());

			if (FilterParameterName == "multichart")
			{
				Display.SelectedType = SelectedType.NoSelected;
				Display.SelectedList.Clear();
			}

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
				else
				{
					switch (property.Name)
					{
						case "values":
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

							if (ValuesDictionary.Count == 0)
								_valuesType = ValuesType.NoValues;
						}
							break;
						case "table":
							Table = Json.Parse(Table, property);
							break;
						case "valueColumn":
							ValueColumn = Json.Parse(ValueColumn, property);
							break;
						case "lagColumn":
							LagColumn = Json.Parse(LagColumn, property);
							break;
						case "addKeyValues":
							AddKeyValues = Json.Parse(AddKeyValues, property);
							break;
						case "removeKeys":
							RemoveKeys = Json.Parse(RemoveKeys, property);
							break;
						case "orderBy":
							OrderBy = Json.Parse(OrderBy, property);
							break;
						case "date":
							Date = Json.Parse(Date, property);
							break;
						case "display":
							if (property.Value.Type == JTokenType.Object)
								Display.ParseJson((JObject)property.Value);
							if (FilterParameterName == "multichart")
							{
								Display.SelectedType = SelectedType.NoSelected;
								Display.SelectedList.Clear();
							}
							break;
						default:
							throw new UnknownJsonPropertyException(String.Format("The {0} property is not defined for a Filter\\Parameter file.", property.Name));
					}
				}
			}

			if (_valuesType != ValuesType.NoValues && !String.IsNullOrEmpty(Table))
			{
				string message = String.Format("The {0} filter\\parameter cannot have both values and a table.", FilterParameterName);
				throw new JsonRuleViolationException(message);
			}
			else if (_valuesType != ValuesType.NoValues)
				Type = FilterParameterType.Values;
			else if (!String.IsNullOrEmpty(Table) || AddKeyValues.Count != 0)
				Type = FilterParameterType.Table;
			else
				Type = FilterParameterType.Neither;

			if (RemoveKeys.Count != 0)
				RemoveKeys.Sort();
		}

		public FilterParameter Clone()
		{
			FilterParameter myClone = new FilterParameter();
			JObject myJson = CompileJson();
			myClone.ParseJson(FilterParameterName, myJson);
			return myClone;
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
