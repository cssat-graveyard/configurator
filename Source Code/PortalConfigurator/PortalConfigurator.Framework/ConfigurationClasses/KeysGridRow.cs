using System;

namespace Framework
{
	public class ValuesGridRow
	{
		public string Name { get; set; }
		public bool IsRemoved { get; set; }
		public bool IsSelected { get; set; }
		public bool IsDisabled { get; set; }
		public string Format { get; set; }
		public bool? IsFromTable { get; set; }

		public ValuesGridRow()
			: this(String.Empty, (bool?)null)
		{ }

		public ValuesGridRow(string name, bool? isFromTable)
			: this(name, false, false, false, String.Empty, isFromTable)
		{ }

		public ValuesGridRow(string name, bool isRemoved, bool isSelected, bool isDisabled, string format, bool? isFromTable)
		{
			this.Name = name;
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
				ValuesGridRow kgr = (ValuesGridRow)obj;

				bool valueEqual = Name == kgr.Name;
				bool isRemovedEqual = IsRemoved == kgr.IsRemoved;
				bool isSelectedEqual = IsSelected == kgr.IsSelected;
				bool isDisabledEqual = IsDisabled == kgr.IsDisabled;
				bool formatEqual = Format == kgr.Format;

				return valueEqual && isRemovedEqual && isSelectedEqual && isDisabledEqual && formatEqual;
			}
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode() ^ IsRemoved.GetHashCode() ^ IsSelected.GetHashCode() ^ IsDisabled.GetHashCode() ^ Format.GetHashCode();
		}
	}
}
