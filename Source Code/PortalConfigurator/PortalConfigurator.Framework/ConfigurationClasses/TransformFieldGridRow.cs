using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
	public class TransformFieldGridRow
	{
		public string FieldName { get; set; }
		public bool IsValueField { get; set; }
		public bool IsRemovedField { get; set; }

		public TransformFieldGridRow()
			: this(String.Empty)
		{ }

		public TransformFieldGridRow(string fieldName)
			: this(fieldName, false, false)
		{ }

		public TransformFieldGridRow(string fieldName, bool isValueField, bool isRemovedField)
		{
			this.FieldName = fieldName;
			this.IsValueField = isValueField;
			this.IsRemovedField = isRemovedField;
		}
	}
}
