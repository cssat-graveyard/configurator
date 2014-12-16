using Framework;
using System;

namespace PortalConfigurator
{
	public class RemovedParameter
	{
		public string Name { get; set; }
		public ControlType ControlType { get; set; }
		public bool IsDate { get; set; }
		public bool IsRequired { get; set; }
		public string HeaderName { get; set; }

		public RemovedParameter()
			: this(String.Empty, ControlType.Parameter)
		{ }

		public RemovedParameter(string name, ControlType controlType)
			: this(name, controlType, false)
		{ }

		public RemovedParameter(string name, ControlType controlType, bool isDate)
			: this(name, controlType, isDate, false)
		{ }

		public RemovedParameter(string name, ControlType controlType, bool isDate, bool isRequired)
			: this(name, controlType, isDate, isRequired, String.Empty)
		{ }

		public RemovedParameter(string name, ControlType controlType, bool isDate, bool isRequired, string headerName)
		{
			this.Name = name;
			this.ControlType = controlType;
			this.IsDate = isDate;
			this.IsRequired = isRequired;
			this.HeaderName = headerName;
		}
	}
}
