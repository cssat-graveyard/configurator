using System;

namespace Framework
{
	public class KeysGridRow
	{
		public string Value { get; set; }
		public bool IsRemoved { get; set; }
		public bool IsSelected { get; set; }
		public bool IsDisabled { get; set; }
		public string Format { get; set; }
		public bool? IsFromTable { get; set; }

		public KeysGridRow()
			: this(String.Empty, (bool?)null)
		{ }

		public KeysGridRow(string value, bool? isFromTable)
			: this(value, false, false, false, String.Empty, isFromTable)
		{ }

		public KeysGridRow(string value, bool isRemoved, bool isSelected, bool isDisabled, string format, bool? isFromTable)
		{
			this.Value = value;
			this.IsRemoved = isRemoved;
			this.IsSelected = isSelected;
			this.IsDisabled = isDisabled;
			this.Format = format;
			this.IsFromTable = isFromTable;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !this.GetType().Equals(obj.GetType()))
				return false;
			else
			{
				KeysGridRow kgr = (KeysGridRow)obj;

				bool valueEqual = Value == kgr.Value;
				bool isRemovedEqual = IsRemoved == kgr.IsRemoved;
				bool isSelectedEqual = IsSelected == kgr.IsSelected;
				bool isDisabledEqual = IsDisabled == kgr.IsDisabled;
				bool formatEqual = Format == kgr.Format;

				return valueEqual && isRemovedEqual && isSelectedEqual && isDisabledEqual && formatEqual;
			}
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode() ^ IsRemoved.GetHashCode() ^ IsSelected.GetHashCode() ^ IsDisabled.GetHashCode() ^ Format.GetHashCode();
		}
	}
}
