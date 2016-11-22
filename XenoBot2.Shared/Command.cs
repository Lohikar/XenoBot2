using DiscordSharp;
using DiscordSharp.Objects;

namespace XenoBot2.Shared
{
	public enum CommandCategory
	{
		/// <summary>
		///		Unknown category.
		/// </summary>
		Unknown,

		/// <summary>
		///		Basic bot commands.
		/// </summary>
		Base,

		/// <summary>
		///		Utility commands.
		/// </summary>
		Utility,

		/// <summary>
		///		Bot administration commands.
		/// </summary>
		Administration,

		/// <summary>
		///		Fun commands.
		/// </summary>
		Fun,

		/// <summary>
		///		Offensive fun commands.
		/// </summary>
		FunOffensive,

		/// <summary>
		///		Fun messages in response to user chatter. 
		/// </summary>
		Retorts
	}

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
		///		Defines if this command is an alias for another. All other members are ignored if this is set.
		/// </summary>
		public string AliasFor = null;
		/// <summary>
		///		Defines if a command should be marked as AdminDisabled.
		/// </summary>
		public bool Disabled = false;
		/// <summary>
		///		The category this command belongs to. Shows in help index.
		/// </summary>
		public CommandCategory Category = CommandCategory.Unknown;

		/// <summary>
		///		The permissions required to use this command.
		/// </summary>
		public PermissionFlag Permission { get; set; } = PermissionFlag.User;

		public CommandFlag Flags { get; set; } = CommandFlag.None;
		/// <summary>
		///		The function to execute when the command is called by a user.
		/// </summary>
		public RunnableCommand Definition;
	}

	public delegate void RunnableCommand(
		DiscordClient client, CommandInfo info, DiscordMember member, DiscordChannelBase channel);
}
