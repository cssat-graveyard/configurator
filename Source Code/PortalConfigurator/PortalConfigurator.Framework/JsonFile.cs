using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
	public abstract class JsonFile
	{
		protected JObject MyJson { get; set; }

		public string FilePath { get; set; }

		public JsonFile()
			: this(String.Empty)
		{ }

		public JsonFile(string filePath)
		{
			this.FilePath = filePath;
		}

		protected void ReadFile()
		{
			using (StreamReader reader = File.OpenText(FilePath))
			{
				MyJson = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
			}
		}

		public abstract void ParseJson();

		public virtual void WriteFile()
		{
			File.WriteAllText(FilePath, MyJson.ToString());
		}
	}
}
