using Newtonsoft.Json.Linq;
using System;

namespace Framework
{
	public class BaseOptionSet : IConfigurationBase
	{
		public int? Width { get; set; }
		public int? Height { get; set; }
		public ChartArea ChartArea { get; set; }
		public bool IsEmpty { get { return CheckForEmpty(); } }

		public BaseOptionSet()
			: this(null, null, new ChartArea())
		{ }
		
		public BaseOptionSet(int? width, int? height, ChartArea chartArea)
		{
			this.Width = width;
			this.Height = height;
			this.ChartArea = chartArea;
		}

		private bool CheckForEmpty()
		{
			return Width == null && Height == null && ChartArea.IsEmpty;
		}

		public JObject CompileJson()
		{
			JObject myJson = new JObject();

			if (Width != null)
				myJson.Add("width", Width);

			if (Height != null)
				myJson.Add("height", Height);

			if (!ChartArea.IsEmpty)
				myJson.Add("chartArea", ChartArea.CompileJson());

			return myJson;
		}

		public void ParseJson(JObject json)
		{
			foreach (var property in json.Properties())
			{
				switch (property.Name)
				{
					case "width":
						Width = Json.Parse(Width, property);
						break;
					case "height":
						Height = Json.Parse(Height, property);
						break;
					case "chartArea":
						ChartArea.ParseJson((JObject)property.Value);
						break;
					default:
						throw new UnknownJsonPropertyException(String.Format("The {0} property is not defined for a Base Option Set.", property.Name));
				}
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !this.GetType().Equals(obj.GetType()))
				return false;
			else
			{
				BaseOptionSet bos = (BaseOptionSet)obj;

				bool widthEqual = Width == bos.Width;
				bool heightEqual = Height == bos.Height;
				bool chartAreaEqual = ChartArea.Equals(bos.ChartArea);

				return widthEqual && heightEqual && chartAreaEqual;
			}
		}

		public override int GetHashCode()
		{
			return Width.GetHashCode() ^ Height.GetHashCode() ^ ChartArea.GetHashCode();
		}
	}
}
