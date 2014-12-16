using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
	public class HeaderGridRow
	{
		private string _headerText;
		private HeaderType _headerType;
		private string _filterParameter;

		public int TableOrdinal { get; set; }
		public string ColumnName { get; set; }
		public bool IsReturnRow { get; set; }
		public bool IsReturnRowDate { get; set; }
		public bool IsReturnRowControl { get; set; }

		public string HeaderText
		{
			get { return _headerText; }
			set
			{
				_headerText = value;
				DecodeHeaderText();
			}
		}
		
		public HeaderType HeaderType
		{
			get { return _headerType; }
			set
			{
				_headerType = value;
				EncodeHeaderText();
			}
		}
		
		public string FilterParameter
		{
			get { return _filterParameter; }
			set
			{
				if (!String.IsNullOrEmpty(value) && (HeaderType == HeaderType.NotAHeader || HeaderType == HeaderType.ColumnName))
				{
					_filterParameter = value;
					HeaderType = HeaderType.FilterParameterName;
				}
				else if (!String.IsNullOrEmpty(value))
				{
					_filterParameter = value;
					EncodeHeaderText();
				}
				else if (String.IsNullOrEmpty(value) && (HeaderType == HeaderType.FilterParameterName || HeaderType == HeaderType.FilterParameterIndex))
				{
					if (TableOrdinal == -1)
						HeaderType = HeaderType.NotAHeader;
					else
						HeaderType = HeaderType.ColumnName;
				}
			}
		}

		public HeaderGridRow()
			: this(String.Empty)
		{ }

		public HeaderGridRow(string headerText)
			:this(-1, String.Empty, headerText)
		{ }

		public HeaderGridRow(int tableOrdinal, string columnName, string headerText)
		{
			this.TableOrdinal = tableOrdinal;
			this.HeaderText = headerText;
			this.ColumnName = columnName;
			this.IsReturnRow = false;
			this.IsReturnRowDate = false;
			this.IsReturnRowControl = false;
		}

		private void DecodeHeaderText()
		{
			if (String.IsNullOrEmpty(HeaderText))
			{
				ColumnName = String.Empty;
				_headerType = HeaderType.NotAHeader;
				_filterParameter = String.Empty;
			}
			else if (HeaderText.StartsWith("*"))
			{
				string name = HeaderText.Substring(1);
				ColumnName = name;
				_headerType = HeaderType.FilterParameterIndex;
				_filterParameter = name;
			}
			else if (HeaderText.Contains('*'))
			{
				int separator = HeaderText.IndexOf('*');
				ColumnName = HeaderText.Substring(0, separator);
				_headerType = HeaderType.FilterParameterIndex;
				_filterParameter = HeaderText.Substring(separator + 1);
			}
			else if (HeaderText.Contains('='))
			{
				int separator = HeaderText.IndexOf('=');
				ColumnName = HeaderText.Substring(0, separator);
				_headerType = HeaderType.FilterParameterName;
				_filterParameter = HeaderText.Substring(separator + 1);
			}
			else
			{
				ColumnName = HeaderText;
				_headerType = HeaderType.ColumnName;
				_filterParameter = String.Empty;
			}
		}

		private void EncodeHeaderText()
		{
			switch (HeaderType)
			{
				case HeaderType.NotAHeader:
					_headerText = String.Empty;
					_filterParameter = String.Empty;
					break;
				case HeaderType.ColumnName:
					_headerText = ColumnName;
					_filterParameter = String.Empty;
					break;
				case HeaderType.FilterParameterName:
					_headerText = String.Concat(ColumnName, "=", FilterParameter);
					break;
				case HeaderType.FilterParameterIndex:
					if (ColumnName == FilterParameter)
						_headerText = String.Concat("*", ColumnName);
					else
						_headerText = String.Concat(ColumnName, "*", FilterParameter);
					break;
				default:
					break;
			}
		}
	}
}
