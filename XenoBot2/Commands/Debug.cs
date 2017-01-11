using System.Threading.Tasks;
using Discord;
using XenoBot2.Shared;

namespace XenoBot2.Commands
{
	internal static class Debug
	{
		internal static async Task Cmdinfo(CommandInfo info, Message msg)
		{
			if (!info.HasArguments)
			{
				await msg.Channel.SendMessage("Not enough arguments.");
				return;
			}
			var cmdtxt = string.Join(" ", info.Arguments);
			Utilities.WriteLog(msg.User, $"requested cmdinfo for '{cmdtxt}'");
			var cmd = CommandParser.ParseCommand(cmdtxt, msg.Channel);
			if (cmd == null)
			{
				await msg.Channel.SendMessage("Error: Command is not defined");
				return;
			}
			var cmddata = Program.BotInstance.Commands[cmd.CommandText].ResolveCommand();
			await msg.Channel.SendMessage($"Input: {cmdtxt}\n" +
									 "```\n" +
									 $"CmdText: {cmd.CommandText}\n" +
									 $"Flags: {cmddata.Flags}\n" +
									 $"LocalState: {Program.BotInstance.CommandStateData[cmdtxt, msg.Channel.Id]}\n" +
									 $"GlobalState: {Program.BotInstance.CommandStateData[cmdtxt]}\n" +
									 $"PermissionFlags: {cmddata.Permission}\n" +
									 $"Category: {cmddata.HelpCategory}\n" +
									 "```");
		}

		internal static async Task GetChannelInfo(CommandInfo info, Message msg)
		{
			Utilities.WriteLog(msg.User, "requested channel info.");
			await msg.Channel.SendMessage("```\n" +
			                          $"ID: {msg.Channel.Id}\n" +
			                          $"Name: {msg.Channel.Name}\n" +
			                          $"Private: {msg.Channel.IsPrivate}\n" +
			                          $"Topic: {msg.Channel.Topic}\n" +
			                          "```");
		}
	}
}
