using System.Threading.Tasks;
using Discord;
using XenoBot2.Shared;

namespace XenoBot2.Commands
{
	internal static class FunOffensive
	{
		internal static async Task Trump(CommandInfo info, User author, Channel channel)
		{
			var subject = "bots";
			if (info.HasArguments)
			{
				subject = string.Join(" ", info.Arguments);
			}
			Utilities.WriteLog(author, $"made '{subject}' great again.");
			await channel.SendMessage($"Make {subject} great again!");
		}
	}
}
