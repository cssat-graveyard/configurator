using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
	public class FilterParameterGridRow
	{
		public ControlType ControlType { get; set; }
		public bool IsDate { get; set; }
		public bool IsRequired { get; set; }

		public FilterParameterGridRow()
			: this(ControlType.Filter, false, false)
		{ }

		public FilterParameterGridRow(ControlType controlType, bool isDate, bool isRequired)
		{
			this.ControlType = controlType;
			this.IsDate = isDate;
			this.IsRequired = isRequired;
		}
	}
}
