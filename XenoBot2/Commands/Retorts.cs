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

		internal static void WelcomeBack(DiscordClient client, CommandInfo info, DiscordMember author, DiscordChannelBase channel)
		{
			Utilities.WriteLog(author, "has been welcomed back.");
			client.SendMessageToRoom(string.Format(Strings.WelcomeBackMessages.GetRandom(), author.MakeMention()), channel);
		}

		internal static void Front(DiscordClient client, CommandInfo info, DiscordMember author, DiscordChannelBase channel)
		{
			Utilities.WriteLog(author, "thinks they're funny. (front)");
			client.SendMessageToRoom(string.Format(Strings.FrontResponses.GetRandom(), author.MakeMention()), channel);
		}

		internal static void Heart(DiscordClient client, CommandInfo info, DiscordMember author, DiscordChannelBase channel)
		{
			Utilities.WriteLog(author, "said heart.");
			client.SendMessageToRoom(Strings.HeartResponses.GetRandom(), channel);
		}
	}
}
