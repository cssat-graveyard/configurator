using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Framework
{
	public class FilterParameterFile : JsonFile, IConfigurationBase
	{
		public List<FilterParameter> FilterParameters { get; set; }
		public bool IsEmpty { get { return CheckForEmpty(); } }

		public string Breadcrumb
		{
			get
			{
				string breadcrumb = String.Empty;

				if (!String.IsNullOrEmpty(this.FilePath))
				{
					FileInfo file = new FileInfo(this.FilePath);
					DirectoryInfo dir = file.Directory;
					breadcrumb = file.Name;

					while (!dir.Name.Equals("test", StringComparison.OrdinalIgnoreCase) && !dir.Name.Equals(dir.Root.Name, StringComparison.OrdinalIgnoreCase))
					{
						breadcrumb = String.Concat(dir.Name, "\\", breadcrumb);
						dir = dir.Parent;
					}
				}

				return breadcrumb;
			}
		}

		public FilterParameterFile()
			: this(String.Empty)
		{ }

		public FilterParameterFile(string filePath)
			: this(filePath, new List<FilterParameter>())
		{ }

		public FilterParameterFile(string filePath, List<FilterParameter> filterParameters)
			: base(filePath)
		{
			this.FilterParameters = filterParameters;
		}

		private bool CheckForEmpty()
		{
			return FilterParameters.Count(p => !p.IsEmpty) == 0;
		}

		public JObject CompileJson()
		{
			JObject myJson = new JObject();

			if (FilterParameters.Count != 0)
				foreach (var item in FilterParameters)
					myJson.Add(item.FilterParameterName, item.CompileJson());

			base.MyJson = myJson;

			return myJson;
		}

		public override void ParseJson()
		{
			base.ReadFile();
			ParseFromJson();
		}

		private void ParseFromJson()
		{
			try
			{
				if (FilterParameters.Count != 0)
					FilterParameters.Clear();

				foreach (var property in base.MyJson.Properties())
				{
					FilterParameter filterParameter = new FilterParameter();
					filterParameter.ParseJson(property.Name, (JObject)property.Value);
					FilterParameters.Add(filterParameter);
				}
			}
			catch (InvalidCastException e)
			{
				string message = "The file does not follow the format of a Filter/Parameter file.";
				throw new JsonFileStructureException(message, e);
			}
		}

		public override void WriteFile()
		{
			int multichartPosition = FilterParameters.FindIndex(p => p.FilterParameterName == "multichart");

			if (multichartPosition == 0 || multichartPosition == -1)
			{
				CompileJson();
				base.WriteFile();
			}
			else
			{
				string message = "The \"multichart\" item must be first in the file.";
				throw new JsonRuleViolationException(message);
			}
		}

		public FilterParameterFile Clone()
		{
			FilterParameterFile myClone = new FilterParameterFile(base.FilePath);
			myClone.MyJson = CompileJson();
			myClone.ParseFromJson();
			return myClone;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !this.GetType().Equals(obj.GetType()))
				return false;
			else
			{
				FilterParameterFile fpf = (FilterParameterFile)obj;

				return FilterParameters.SequenceEqual(fpf.FilterParameters);
			}
		}

		public override int GetHashCode()
		{
			return FilterParameters.GetHashCode();
		}
	}
}
