using System.Collections.Generic;
using System.Linq;

namespace XenoBot2.Shared
{
	public class CommandInfo
	{
		/// <summary>
		///		The string used to invoke this command.
		/// </summary>
		public string CommandText { get; set; }
		/// <summary>
		///		Arguments (if any) specified when the command was invoked.
		/// </summary>
		public IList<string> Arguments { get; set; }
		/// <summary>
		///		If there's any arguments.
		/// </summary>
		public bool HasArguments => Arguments.Any();
		/// <summary>
		///		The current state of this <see cref="CommandInfo"/>'s associated <see cref="Command"/>.
		/// </summary>
		public CommandState State { get; set; }
	}
}