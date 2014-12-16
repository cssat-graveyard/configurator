using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
	[Serializable]
	public abstract class JsonParseException : Exception
	{
		public JsonParseException()
		{ }

		public JsonParseException(string message)
			: base(message)
		{ }

		public JsonParseException(string message, Exception inner)
			: base(message, inner)
		{ }
	}

	[Serializable]
	public class UnknownJsonPropertyException : JsonParseException
	{
		public UnknownJsonPropertyException()
		{ }

		public UnknownJsonPropertyException(string message)
			: base(message)
		{ }

		public UnknownJsonPropertyException(string message, Exception inner)
			: base(message, inner)
		{ }
	}

	[Serializable]
	public class JsonPropertyParseException : JsonParseException
	{
		public JsonPropertyParseException()
		{ }

		public JsonPropertyParseException(string message)
			: base(message)
		{ }

		public JsonPropertyParseException(string message, Exception inner)
			: base(message, inner)
		{ }
	}

	[Serializable]
	public class JsonRuleViolationException : JsonParseException
	{
		public JsonRuleViolationException()
		{ }

		public JsonRuleViolationException(string message)
			: base(message)
		{ }

		public JsonRuleViolationException(string message, Exception inner)
			: base(message, inner)
		{ }
	}

	[Serializable]
	public class JsonFileStructureException : JsonParseException
	{
		public JsonFileStructureException()
		{ }

		public JsonFileStructureException(string message)
			: base(message)
		{ }

		public JsonFileStructureException(string message, Exception inner)
			: base(message, inner)
		{ }
	}
}
