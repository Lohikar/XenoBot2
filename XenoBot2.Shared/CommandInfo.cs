using System.Collections.Generic;
using System.Linq;

namespace XenoBot2.Shared
{
	public enum CommandStatus
	{
		/// <summary>
		///     Something went wrong and the status of the command is not known.
		/// </summary>
		Unknown,

		/// <summary>
		///     Command exists and can be used in this context.
		/// </summary>
		Allowed,

		/// <summary>
		///     Command exists, but cannot be used in this context.
		/// </summary>
		Disabled,

		/// <summary>
		///     Command exists and can be used in this context, but has been disabled temporarily due to over-use.
		/// </summary>
		RateLimited,

		/// <summary>
		///     Command exists, but has been turned off by bot administrator.
		/// </summary>
		AdminDisabled
	}

	public class CommandInfo
	{
		public string CommandText { get; set; }
		public IList<string> Arguments { get; set; }
		public CommandStatus Status { get; set; }
		public CommandMetadata Meta { get; set; }
		public bool HasArguments => Arguments.Any();
	}
}