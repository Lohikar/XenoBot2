using DiscordSharp;
using DiscordSharp.Objects;
using XenoBot2.Data;

namespace XenoBot2.Commands
{
	internal static class FunOffensive
	{
		internal static void Penis(DiscordClient client, CommandInfo info, DiscordMember author, DiscordChannelBase channel)
		{
			var length = 8;
			if (info.HasArguments && !int.TryParse(info.Arguments[0], out length))
			{
				client.SendMessageToRoom(Messages.InvalidIntError, channel);
				return;
			}
			if (length == 2)
			{
				client.SendMessageToRoom("8D", channel);
				Utilities.WriteLog(author, "is immature (zero units)");
			}
			else if (length < 2)
			{
				length = -length;
				if (length == 2)
				{
					client.SendMessageToRoom("c8", channel);
					Utilities.WriteLog(author, "is immature (zero units)");
				}
				else if (length < 2)
				{
					client.SendMessageToRoom("number too small", channel);
					Utilities.WriteLog(author, "is immature (invalid units)");
				}
				else
				{
					client.SendMessageToRoom($"c{"=".Repeat(length - 2)}8", channel);
					Utilities.WriteLog(author, $"is immature ({length} units)");
				}
			}
			else
			{
				client.SendMessageToRoom($"8{"=".Repeat(length - 2)}D", channel);
				Utilities.WriteLog(author, $"is immature ({length} units)");
			}
		}

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
