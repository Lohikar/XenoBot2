using DiscordSharp;
using DiscordSharp.Objects;
using XenoBot2.Shared;

namespace XenoBot2.Commands
{
	internal static class FunOffensive
	{
		internal static void Trump(DiscordClient client, CommandInfo info, DiscordMember author, DiscordChannelBase channel)
		{
			var subject = "bots";
			if (info.HasArguments)
			{
				subject = string.Join(" ", info.Arguments);
			}
			client.SendMessageToRoom($"Make {subject} great again!", channel);
			Utilities.WriteLog(author, $"made '{subject}' great again.");
		}
	}
}
