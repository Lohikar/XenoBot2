using System.Threading.Tasks;
using Discord;
using XenoBot2.Shared;

namespace XenoBot2.Commands
{
	internal static class Debug
	{
		internal static async Task Cmdinfo(CommandInfo info, User author, Channel channel)
		{
			if (!info.HasArguments)
			{
				await channel.SendMessage("Not enough arguments.");
				return;
			}
			var cmdtxt = string.Join(" ", info.Arguments);
			Utilities.WriteLog(author, $"requested cmdinfo for '{cmdtxt}'");
			var cmd = CommandParser.ParseCommand(cmdtxt, channel);
			if (cmd == null)
			{
				await channel.SendMessage("Error: Command is not defined");
				return;
			}
			var cmddata = Program.BotInstance.Commands[cmd.Item1.CommandText].ResolveCommand();
			await channel.SendMessage($"Input: {cmdtxt}\n" +
									 "```\n" +
									 $"CmdText: {cmd.Item1.CommandText}\n" +
									 $"Flags: {cmddata.Flags}\n" +
									 $"LocalState: {Program.BotInstance.CommandStateData[cmdtxt, channel.Id]}\n" +
									 $"GlobalState: {Program.BotInstance.CommandStateData[cmdtxt]}\n" +
									 $"PermissionFlags: {cmddata.Permission}\n" +
									 $"Category: {cmddata.HelpCategory}\n" +
									 "```");
		}

		internal static async Task GetChannelInfo(CommandInfo info, User author, Channel channel)
		{
			Utilities.WriteLog(author, "requested channel info.");
			await channel.SendMessage("```\n" +
			                          $"ID: {channel.Id}\n" +
			                          $"Name{channel.Name}" +
			                          $"Private: {channel.IsPrivate}\n" +
			                          $"Topic: {channel.Topic}\n" +
			                          "```");
		}
	}
}
