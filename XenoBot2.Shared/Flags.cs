using System;

namespace XenoBot2.Shared
{
	[Flags]
	public enum UserFlag
	{
		/// <summary>
		///     No permissions required.
		/// </summary>
		None = 0,

		/// <summary>
		///     User is ignored.
		/// </summary>
		Ignored = 1,

		/// <summary>
		///     Invoker must be able to speak in the affected channel.
		/// </summary>
		User = 2,

		/// <summary>
		///     Invoker must be a moderator for the affected channel.
		/// </summary>
		Moderator = 4,

		/// <summary>
		///     Invoker must be an administrator for the server of the affected channel.
		/// </summary>
		Administrator = 8,

		/// <summary>
		///     Invoker must be the bot administrator. Global permission.
		/// </summary>
		BotAdministrator = 16,

		/// <summary>
		///     Invoker must be allowed to debug the bot. Global permission.
		/// </summary>
		BotDebug = 32
	}

	[Flags]
	public enum CommandFlag
	{
		/// <summary>
		///     No flags set.
		/// </summary>
		None = 0,

		/// <summary>
		///     Command is disallowed in normal channels.
		/// </summary>
		NoPublicChannel = 1,

		/// <summary>
		///     Command is disallowed in private messaging.
		/// </summary>
		NoPrivateChannel = 2,

		/// <summary>
		///     Command can be used while user is ignored by bot.
		/// </summary>
		UsableWhileIgnored = 4,

		/// <summary>
		///     Command cannot be disabled.
		/// </summary>
		NonDisableable = 8,

		/// <summary>
		///     Command does not show in help index pages.
		/// </summary>
		Hidden = 16
	}

	[Flags]
	public enum CommandState
	{
		/// <summary>
		///     No special state is set.
		/// </summary>
		None = 0,

		/// <summary>
		///     The command is disabled in this context.
		/// </summary>
		Disabled = 1,

		/// <summary>
		///     The command is hidden from help indexes in this context.
		/// </summary>
		Hidden = 2,

		/// <summary>
		///     The command does not exist.
		/// </summary>
		DoesNotExist = 4
	}
}
