using System;

namespace XenoBot2
{
	[Flags]
	public enum PermissionFlag
	{
		None = 0,
		User = 1,
		Moderator = 2,
		Administrator = 4,
		BotAdministrator = 8
	}

	[Flags]
	public enum CommandFlag
	{
		None = 0,
		NoPublicChannel = 1,
		NoPrivateChannel = 2,
		UsableWhileIgnored = 4,
		NonDisableable = 8,
		Hidden = 16,
	}
}
