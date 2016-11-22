using DiscordSharp;
using DiscordSharp.Objects;
using XenoBot2.Data;
using XenoBot2.Shared;

namespace XenoBot2.Commands
{
	internal static class Retorts
	{
		internal static void Sudo(DiscordClient client, CommandInfo info, DiscordMember author, DiscordChannelBase channel)
		{
			Utilities.WriteLog(author, "attempted sudo.");

			var fmt = string.Format(author.ID == Ids.Admin 
				? Strings.SudoAdminMessages.GetRandom() 
				: Strings.SudoMessages.GetRandom(), author.MakeMention());

			client.SendMessageToRoom(fmt, channel);
		}
	}
}
