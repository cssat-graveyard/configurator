using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Framework
{
	public interface IConfigurationBase
	{
		bool IsEmpty { get; }

		JObject CompileJson();
	}
}
