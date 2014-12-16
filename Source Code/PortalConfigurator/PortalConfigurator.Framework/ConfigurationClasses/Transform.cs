using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework
{
	public class Transform : IConfigurationBase
	{
		private Dictionary<int, TransformFieldGridRow> _transformFieldGridRows;
		private Dictionary<int, string> _tableColumns;
		internal bool _transformFieldGridRowsAreUpdated;
		private bool _tableDataAreLoaded;
		
		public TransformFunction Function { get; set; }
		public string Table { get; set; }
		public string DateField { get; set; }
		public List<string> ValueFields { get; set; }
		public List<string> RemoveFields { get; set; }
		public bool IsEmpty { get { return CheckForEmpty(); } }

		public Dictionary<int, TransformFieldGridRow> TransformFieldGridRows
		{
			get
			{
				if (!_transformFieldGridRowsAreUpdated)
					RefreshMyTransformFieldGridRows();

				return _transformFieldGridRows;
			}
			set
			{
				_transformFieldGridRows = value;
				UpdatePropertiesFromTransformFieldGridRows();
			}
		}

		public Dictionary<int, string> TableColumns
		{
			get
			{
				if (_tableColumns.Count == 0 && !String.IsNullOrEmpty(Table))
					LoadTableFromDatabase();

				return _tableColumns;
			}
			internal set
			{
				_tableColumns = value;
			}
		}

		public bool TableDataAreLoaded
		{
			get { return _tableDataAreLoaded; }
			set
			{
				_tableDataAreLoaded = value;

				if (!value)
					_tableColumns.Clear();
			}
		}

		public Transform()
			: this(TransformFunction.NoFunction, String.Empty, String.Empty, new List<string>(), new List<string>())
		{ }

		public Transform(TransformFunction function, string table, string dateField, List<string> valueFields, List<string> removeFields)
		{
			this.Function = function;
			this.Table = table;
			this.DateField = dateField;
			this.ValueFields = valueFields;
			this.RemoveFields = removeFields;
			this._transformFieldGridRows = new Dictionary<int, TransformFieldGridRow>();
			this._tableColumns = new Dictionary<int, string>();
			this._transformFieldGridRowsAreUpdated = false;
			this._tableDataAreLoaded = false;
		}

		private bool CheckForEmpty()
		{
			return Function == TransformFunction.NoFunction && String.IsNullOrEmpty(Table) && String.IsNullOrEmpty(DateField) && ValueFields.Count == 0 && RemoveFields.Count == 0;
		}

		private void LoadTableFromDatabase()
		{
			bool tableDataWasUpdated = false;

			try
			{
				_tableColumns = Database.PopulateMeasure(Table);
				tableDataWasUpdated = true;
			}
			catch (Exception)
			{
				_tableColumns = new Dictionary<int, string>();
				throw;
			}
			finally
			{
				_tableDataAreLoaded = true;
				_transformFieldGridRowsAreUpdated = !tableDataWasUpdated;
			}
		}

		private void RefreshMyTransformFieldGridRows()
		{
			_transformFieldGridRows.Clear();

			foreach (var item in TableColumns)
			{
				TransformFieldGridRow row = new TransformFieldGridRow(item.Value);
				row.IsValueField = ValueFields.Contains(item.Value);
				row.IsRemovedField = RemoveFields.Contains(item.Value);
				_transformFieldGridRows.Add(item.Key, row);
			}

			_transformFieldGridRowsAreUpdated = true;
		}

		public void UpdatePropertiesFromTransformFieldGridRows()
		{
			ValueFields.Clear();
			RemoveFields.Clear();

			foreach (var item in _transformFieldGridRows)
			{
				if (item.Value.IsValueField)
					ValueFields.Add(item.Value.FieldName);

				if (item.Value.IsRemovedField)
					RemoveFields.Add(item.Value.FieldName);
			}
		}

		public JObject CompileJson()
		{
			JObject myJson = new JObject();

			if (Function != TransformFunction.NoFunction)
				myJson.Add("Function", Enums.GetString(Function));

			if (!String.IsNullOrEmpty(Table))
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
				if (property.Name == "Function")
					Function = Enums.GetTransformFunctionEnum(Json.Parse(String.Empty, property));
				else if (property.Name == "table")
					Table = Json.Parse(Table, property);
				else if (property.Name == "dateField")
					DateField = Json.Parse(DateField, property);
				else if (property.Name == "valueFields")
					ValueFields = Json.Parse(ValueFields, property);
				else if (property.Name == "removeFields")
					RemoveFields = Json.Parse(RemoveFields, property);
				else
					throw new UnknownJsonPropertyException(String.Format("The {0} property is not defined for a Transform.", property.Name));
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
}
