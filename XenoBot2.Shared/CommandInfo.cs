using System.Collections.Generic;
using System.Linq;

namespace XenoBot2.Shared
{
	public class CommandInfo
	{
		public string CommandText { get; set; }
		public IList<string> Arguments { get; set; }
		public bool HasArguments => Arguments.Any();
	}
}