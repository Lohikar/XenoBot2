using System;

namespace XenoBot2.Shared
{
	[Flags]
	public enum UserFlag
	{
		/// <summary>
		///     No permissions present or required.
		/// </summary>
		None = 0,

		/// <summary>
		///     User is ignored.
		/// </summary>
		Ignored = 1 << 0,

		/// <summary>
		///     Invoker must be able to speak in the affected channel. Present on any user able to speak in the channel.
		/// </summary>
		User = 1 << 1,

		/// <summary>
		///     Invoker must be a moderator for the affected channel. Set by server owner.
		/// </summary>
		Moderator = 1 << 2,

		/// <summary>
		///     Invoker must be an administrator for the server of the affected channel. Set by server owner.
		/// </summary>
		Administrator = 1 << 3,

		/// <summary>
		///		Invoker must be the owner of the server. Automatically assigned and cannot be reassigned.
		/// </summary>
		Owner = 1 << 4,

		/// <summary>
		///     Invoker must be the bot administrator. Global permission. Set in bot configuration.
		/// </summary>
		BotAdministrator = 1 << 5,

		/// <summary>
		///     Invoker must be allowed to debug the bot. Global permission. Set in bot configuration.
		/// </summary>
		BotDebug = 1 << 6
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
