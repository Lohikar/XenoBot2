using System.Collections.Generic;
using DiscordSharp.Objects;

namespace XenoBot2.DataManagement
{
	internal class UserManager
	{
		private Dictionary<string, UserInfo> _users;

		public void Register(DiscordMember member)
		{
			if (_users.ContainsKey(member.ID))
			{
				// user already registered
			}
		}

		public bool IsAllowed(DiscordMember user, string command, DiscordChannelBase channel)
		{
			Register(user);
			return false;
		}

		public void CommandUsed(DiscordMember user, string command, DiscordChannelBase channel)
		{
			
		}
	}

	internal struct UserInfo
	{
		public DiscordMember User;
		// this can't be efficient
		public IDictionary<string, int> CommandUsage;
		public IDictionary<string, PerChannelUserInfo> ChannelUserInfo;
	}

	internal struct PerChannelUserInfo
	{
		public UserClass Class;
	}

	internal enum UserClass
	{
		/// <summary>
		/// 
		/// </summary>
		Unknown,
		/// <summary>
		///		A regular user with no special permissions.
		/// </summary>
		Regular,
		/// <summary>
		///		A user with permission to run moderation commands on a channel.
		/// </summary>
		Moderator,
		/// <summary>
		///		The owner of a channel, with unlimited permissions for channel-specific permissions.
		/// </summary>
		ChannelOwner,
		/// <summary>
		///		A bot administrator, with unlimited permissions, globally.
		/// </summary>
		BotAdministrator
	}
}
