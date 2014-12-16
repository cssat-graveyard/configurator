using System;
using System.Linq;

namespace Framework
{
	public class MeasureGridRow
	{
		public string DateColumn { get; set; }
		public int TableOrdinal { get; set; }
		public bool IsDate { get; set; }
		public bool IsRequired { get; set; }
		public bool IsReturnRow { get; set; }
		public bool IsReturnRowDate { get; set; }
		public bool IsReturnRowControl { get; set; }

		private string _filterParameter;
		public string FilterParameter
		{
			get { return _filterParameter; }
			set
			{
				_filterParameter = value;

				if (!String.IsNullOrEmpty(value) && HeaderType == HeaderType.ColumnName)
					HeaderType = HeaderType.FilterParameterName;
				else if (!String.IsNullOrEmpty(value) && String.IsNullOrEmpty(ColumnName))
					HeaderType = HeaderType.NotAHeader;
				else if (String.IsNullOrEmpty(value) && (HeaderType == HeaderType.FilterParameterName || HeaderType == HeaderType.FilterParameterIndex))
					HeaderType = (IsValueField || IsRemoveField) ? HeaderType.NotAHeader : HeaderType.ColumnName;
				else
					EncodeHeaderText();

				if (!String.IsNullOrEmpty(value) && _controlType == ControlType.Neither)
					_controlType = ControlType.Parameter;
				else if (String.IsNullOrEmpty(value) && _controlType != ControlType.Neither)
				{
					_controlType = ControlType.Neither;
					IsDate = false;
					IsRequired = false;
				}
			}
		}

		private ControlType _controlType { get; set; }
		public ControlType ControlType
		{
			get { return _controlType; }
			set
			{
				if (!String.IsNullOrEmpty(FilterParameter) || value == ControlType.Neither)
				{
					switch (value)
					{
						case ControlType.Neither:
							_filterParameter = String.Empty;
							IsDate = false;
							IsRequired = false;
							IsReturnRow = (_headerType != HeaderType.NotAHeader && IsReturnRowControl) || IsReturnRow;
							IsReturnRowControl = false;
							break;
						case ControlType.Parameter:
							if (_controlType == ControlType.Neither || _controlType == ControlType.Control)
								IsReturnRow = _headerType != HeaderType.NotAHeader;
							break;
						case ControlType.Control:
							if (_controlType == ControlType.Neither || _controlType == ControlType.Parameter)
								IsReturnRowControl = _headerType != HeaderType.NotAHeader;
							break;
						case ControlType.Both:
							if (_controlType == ControlType.Neither || _controlType == ControlType.Control)
								IsReturnRow = _headerType != HeaderType.NotAHeader;
							if (_controlType == ControlType.Neither || _controlType == ControlType.Parameter)
								IsReturnRowControl = _headerType != HeaderType.NotAHeader;
							break;
						default:
							break;
					}

					_controlType = value;
				}
			}
		}

		private string _columnName { get; set; }
		public string ColumnName
		{
			get { return _columnName; }
			set
			{
				_columnName = value;

				if (String.IsNullOrEmpty(value) && HeaderType != HeaderType.NotAHeader)
					HeaderType = HeaderType.NotAHeader;
				else if (!String.IsNullOrEmpty(value) && String.IsNullOrEmpty(FilterParameter) && (HeaderType != HeaderType.DateColumn && HeaderType != HeaderType.NotAHeader))
					HeaderType = HeaderType.NotAHeader;
				else if (!String.IsNullOrEmpty(value) && !String.IsNullOrEmpty(FilterParameter) && HeaderType == HeaderType.NotAHeader)
					HeaderType = HeaderType.FilterParameterName;
				else
					EncodeHeaderText();
			}
		}

		private HeaderType _headerType;
		public HeaderType HeaderType
		{
			get { return _headerType; }
			set
			{
				if (_headerType == HeaderType.NotAHeader)
				{
					IsReturnRow = _controlType == ControlType.Parameter || _controlType == ControlType.Both;
					IsReturnRowControl = _controlType == ControlType.Control || _controlType == ControlType.Both;
				}
				else if (value == HeaderType.NotAHeader)
				{
					IsReturnRow = false;
					IsReturnRowDate = false;
					IsReturnRowControl = false;
				}

				_headerType = value;
				EncodeHeaderText();
			}
		}

		private string _headerText;
		public string HeaderText
		{
			get { return _headerText; }
			set
			{
				_headerText = value;
				DecodeHeaderText();
			}
		}

		private bool _isValueField { get; set; }
		public bool IsValueField
		{
			get { return _isValueField; }
			set
			{
				_isValueField = value;

				if (value)
					ControlType = ControlType.Neither;
			}
		}

		private bool _isRemoveField { get; set; }
		public bool IsRemoveField
		{
			get { return _isRemoveField; }
			set
			{
				_isRemoveField = value;

				if (value)
					ControlType = ControlType.Neither;
			}
		}

		public MeasureGridRow(string dateColumn)
			:this(dateColumn, String.Empty)
		{ }

		public MeasureGridRow(string dateColumn, string headerText)
		{
			this.DateColumn = dateColumn;
			this.HeaderText = headerText;
			this.TableOrdinal = -1;
			this.IsReturnRow = false;
			this.IsReturnRowDate = false;
			this.IsReturnRowControl = false;
			this.ControlType = ControlType.Neither;
			this.IsDate = false;
			this.IsRequired = false;
			this._isValueField = false;
			this._isRemoveField = false;
		}

		private void DecodeHeaderText()
		{
			if (String.IsNullOrEmpty(HeaderText))
			{
				_columnName = String.Empty;
				_headerType = HeaderType.NotAHeader;
				_filterParameter = String.Empty;
				_controlType = ControlType.Neither;
			}
			else if (HeaderText == "&nbsp;")
			{
				_columnName = DateColumn;
				_headerType = HeaderType.DateColumn;
				_filterParameter = String.Empty;
				_controlType = ControlType.Neither;
			}
			else if (HeaderText.IndexOf('*') == 0)
			{
				string name = HeaderText.Substring(1);
				_columnName = name;
				_headerType = HeaderType.FilterParameterIndex;
				_filterParameter = name;
				_controlType = ControlType.Parameter;
			}
			else if (HeaderText.Contains('*'))
			{
				int separator = HeaderText.IndexOf('*');
				_columnName = HeaderText.Substring(0, separator);
				_headerType = HeaderType.FilterParameterIndex;
				_filterParameter = HeaderText.Substring(separator + 1);
				_controlType = ControlType.Parameter;
			}
			else if (HeaderText.Contains('='))
			{
				int separator = HeaderText.IndexOf('=');
				_columnName = HeaderText.Substring(0, separator);
				_headerType = HeaderType.FilterParameterName;
				_filterParameter = HeaderText.Substring(separator + 1);
				_controlType = ControlType.Parameter;
			}
			else
			{
				_columnName = HeaderText;
				_headerType = HeaderType.ColumnName;
				_filterParameter = String.Empty;
				_controlType = ControlType.Neither;
			}
		}

		private void EncodeHeaderText()
		{
			switch (HeaderType)
			{
				case HeaderType.NotAHeader:
					_headerText = String.Empty;
					break;
				case HeaderType.ColumnName:
					_headerText = ColumnName;
					break;
				case HeaderType.FilterParameterName:
					_headerText = String.Concat(ColumnName, "=", FilterParameter);
					break;
				case HeaderType.FilterParameterIndex:
					if (_columnName == _filterParameter)
						_headerText = String.Concat("*", ColumnName);
					else
						_headerText = String.Concat(ColumnName, "*", FilterParameter);
					break;
				case HeaderType.DateColumn:
					_headerText = "&nbsp;";
					_filterParameter = String.Empty;
					_controlType = ControlType.Neither;
					if (_columnName != DateColumn && !String.IsNullOrEmpty(DateColumn))
						_columnName = DateColumn;
					else if (_columnName != DateColumn && !String.IsNullOrEmpty(_columnName))
						DateColumn = _columnName;
					break;
				default:
					break;
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !this.GetType().Equals(obj.GetType()))
				return false;
			else
			{
				MeasureGridRow mgr = (MeasureGridRow)obj;

				bool filterParameterEqual = _filterParameter == mgr._filterParameter;
				bool controlTypeEqual = _controlType == mgr._controlType;
				bool isDateEqual = IsDate == mgr.IsDate;
				bool isRequiredEqual = IsRequired == mgr.IsRequired;
				bool columnNameEqual = _columnName == mgr._columnName;
				bool headerTypeEqual = _headerType == mgr._headerType;
				bool headerTextEqual = _headerText == mgr._headerText;
				bool isReturnRowEqual = IsReturnRow == mgr.IsReturnRow;
				bool isReturnRowDateEqual = IsReturnRowDate == mgr.IsReturnRowDate;
				bool isReturnRowControlEqual = IsReturnRowControl == mgr.IsReturnRowControl;
				bool isValueFieldEqual = _isValueField == mgr._isValueField;
				bool isRemoveFieldEqual = _isRemoveField == mgr._isRemoveField;

				return filterParameterEqual && controlTypeEqual && isDateEqual && isRequiredEqual && columnNameEqual && headerTypeEqual && headerTextEqual &&
					isReturnRowEqual && isReturnRowDateEqual && isReturnRowControlEqual && isValueFieldEqual && isRemoveFieldEqual;
			}
		}

		public override int GetHashCode()
		{
			return _filterParameter.GetHashCode() ^ _controlType.GetHashCode() ^ IsDate.GetHashCode() ^ IsRequired.GetHashCode() ^ _columnName.GetHashCode() ^
				_headerType.GetHashCode() ^ _headerText.GetHashCode() ^ IsReturnRow.GetHashCode() ^ IsReturnRowDate.GetHashCode() ^ IsReturnRowControl.GetHashCode() ^
				_isValueField.GetHashCode() ^ _isRemoveField.GetHashCode();
		}
	}
}
