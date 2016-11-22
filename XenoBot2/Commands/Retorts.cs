using DiscordSharp;
using DiscordSharp.Objects;
using XenoBot2.Data;

namespace XenoBot2.Commands
{
	internal static class Retorts
	{
		internal static void Sudo(DiscordClient client, CommandInfo info, DiscordMember author, DiscordChannelBase channel)
		{
			Utilities.WriteLog(author, "attempted sudo.");

			var fmt = string.Format(author.ID == Ids.Admin 
				? CommandData.SudoAdminMessages.GetRandom() 
				: CommandData.SudoMessages.GetRandom(), author.GetMention());

			client.SendMessageToRoom(fmt, channel);
		}

		internal static void WelcomeBack(DiscordClient client, CommandInfo info, DiscordMember author, DiscordChannelBase channel)
		{
			Utilities.WriteLog(author, "has been welcomed back.");
			client.SendMessageToRoom(string.Format(CommandData.WelcomeBackMessages.GetRandom(), author.GetMention()), channel);
		}

		internal static void Front(DiscordClient client, CommandInfo info, DiscordMember author, DiscordChannelBase channel)
		{
			Utilities.WriteLog(author, "thinks they're funny. (front)");
			client.SendMessageToRoom(string.Format(CommandData.FrontResponses.GetRandom(), author.GetMention()), channel);
		}

		internal static void Heart(DiscordClient client, CommandInfo info, DiscordMember author, DiscordChannelBase channel)
		{
			Utilities.WriteLog(author, "said heart.");
			client.SendMessageToRoom(CommandData.HeartResponses.GetRandom(), channel);
		}
	}
}
