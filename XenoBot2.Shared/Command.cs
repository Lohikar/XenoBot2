using System.Threading.Tasks;
using Discord;

namespace XenoBot2.Shared
{
	/// <summary>
	///		Describes information about a command, such as help strings and permissions.
	/// </summary>
	public class Command
	{
		/// <summary>
		///		The short string that should be displayed in the help index.
		/// </summary>
		public string HelpText = string.Empty;

		/// <summary>
		///		The long string that should be displayed for command-specific help pages.
		/// </summary>
		public string LongHelpText = string.Empty;

		/// <summary>
		///		The commands arguments, if any.
		/// </summary>
		public string Arguments = string.Empty;

		/// <summary>
		///		The category this command should be shown under in help.
		/// </summary>
		public string HelpCategory = string.Empty;

		/// <summary>
		///		Defines if this command is an alias for another. All other members are ignored if this is set.
		/// </summary>
		public string AliasFor = null;

		/// <summary>
		///		The permissions required to use this command.
		/// </summary>
		public UserFlag Permission { get; set; } = UserFlag.User;

		/// <summary>
		///		General flags applied to this command.
		/// </summary>
		public CommandFlag Flags { get; set; } = CommandFlag.None;
		/// <summary>
		///		The function to execute when the command is called by a user.
		/// </summary>
		public RunnableCommand Definition { get; set; }
	}

	public delegate Task RunnableCommand(CommandInfo info, User member, Channel channel);
}
