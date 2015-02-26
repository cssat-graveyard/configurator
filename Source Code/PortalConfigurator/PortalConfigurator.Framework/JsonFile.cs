using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace Framework
{
	public abstract class JsonFile
	{
		protected JObject MyJson { get; set; }

		public DateTime FileDate { get; private set; }

		private string _filePath;
		public string FilePath
		{
			get
			{
				return _filePath;
			}

			set
			{
				_filePath = value;

				if (!String.IsNullOrEmpty(_filePath))
				{
					try
					{
						FileInfo file = new FileInfo(_filePath);
						this.FileDate = file.LastWriteTime;
					}
					catch (PlatformNotSupportedException)
					{
						this.FileDate = DateTime.Now;
					}
					catch (Exception)
					{
						throw;
					}
				}
				else
					this.FileDate = DateTime.Now;
			}
		}

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
			FileInfo file = new FileInfo(FilePath);
			FileDate = file.LastWriteTime;
		}
	}
}
