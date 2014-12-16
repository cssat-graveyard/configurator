using Newtonsoft.Json.Linq;
using System;

namespace Framework
{
	public class ChartArea : IConfigurationBase
	{
		public int? Left { get; set; }
		public int? Top { get; set; }
		public string Width { get; set; }
		public string Height { get; set; }
		public bool IsEmpty { get { return CheckForEmpty(); } }

		public ChartArea()
			: this(null, null, String.Empty, String.Empty)
		{ }

		public ChartArea(int? left, int? top, string width, string height)
		{
			this.Left = left;
			this.Top = top;
			this.Width = width;
			this.Height = height;
		}

		private bool CheckForEmpty()
		{
			return Left == null && Top == null && String.IsNullOrEmpty(Width) && String.IsNullOrEmpty(Height);
		}

		public JObject CompileJson()
		{
			JObject myJson = new JObject();

			if (Left != null)
				myJson.Add("left", (JToken)Left);

			if (Top != null)
				myJson.Add("top", (JToken)Top);

			if (!String.IsNullOrEmpty(Width))
				myJson.Add("width", (JToken)Width);
			
			if (!String.IsNullOrEmpty(Height))
				myJson.Add("height", (JToken)Height);

			return myJson;
		}

		public void ParseJson(JObject json)
		{
			foreach (var property in json.Properties())
			{
				switch (property.Name)
				{ 
					case "left":
						Left = Json.Parse(Left, property);
						break;
					case "top":
						Top = Json.Parse(Top, property);
						break;
					case "width":
						Width = Json.Parse(Width, property);
						break;
					case "height":
						Height = Json.Parse(Height, property);
						break;
					default:
						throw new UnknownJsonPropertyException(String.Format("The {0} property is not defined for a Chart Area.", property.Name));
				}
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !this.GetType().Equals(obj.GetType()))
				return false;
			else
			{
				ChartArea ca = (ChartArea)obj;

				bool leftEqual = Left == ca.Left;
				bool topEqual = Top == ca.Top;
				bool widthEqual = Width == ca.Width;
				bool heightEqual = Height == ca.Height;

				return leftEqual && topEqual && widthEqual && heightEqual;
			}
		}

		public override int GetHashCode()
		{
			return Left.GetHashCode() ^ Top.GetHashCode() ^ Width.GetHashCode() ^ Height.GetHashCode();
		}
	}
}
