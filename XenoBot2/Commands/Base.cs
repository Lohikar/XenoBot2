using System.Linq;
using System.Text;
using DiscordSharp;
using DiscordSharp.Objects;
using XenoBot2.Data;
using XenoBot2.Shared;

namespace XenoBot2.Commands
{
	/// <summary>
	///		Core bot commands. This group cannot be disabled.
	/// </summary>
	internal static class Base
	{
		internal static void Help(DiscordClient client, CommandInfo info, DiscordMember author, DiscordChannelBase channel)
		{
			if (!info.HasArguments)
			{
				var cb = new StringBuilder();
				cb.AppendLine(Messages.HelpTextHeader);
				cb.AppendLine("```");
				// show list of commands + shorthelp

				var isAdmin = author.ID == Ids.Admin;

				var helpLines = from item in CommandData.CommandList
					where isAdmin || !item.Value.Flags.HasFlag(CommandFlag.Hidden)
					select GenerateHelpEntry(item.Value, item.Key);

				cb.Append(string.Join("\n", helpLines));

				cb.AppendLine("```");
				Utilities.WriteLog(author, "requested help index.");
				client.SendMessageToUser(cb.ToString(), author);
			}
			else
			{
				if (!CommandData.CommandList.ContainsKey(info.Arguments[0])) return;
				var cmdmeta = CommandData.CommandList[info.Arguments[0]].ResolveCommand();
				if (string.IsNullOrEmpty(cmdmeta.LongHelpText))
				{
					Utilities.WriteLog(author, $"requested non-existent help page '{info.Arguments[0]}'");
					client.SendMessageToUser("That page does not exist.", author);
				}
				else
				{
					Utilities.WriteLog(author, $"requested help page '{info.Arguments[0]}'");
					var builder = new StringBuilder();
					builder.AppendLine($"Help - {info.Arguments[0]}\n```");
					builder.AppendLine(cmdmeta.LongHelpText);
					builder.AppendLine("```");
					client.SendMessageToUser(builder.ToString(), author);
				}
			}
		}

		private static string GenerateHelpEntry(Command cmd, string cmdname)
		{
			var helptext = cmd.HelpText;
			if (cmd.AliasFor != null && CommandData.CommandList.ContainsKey(cmd.AliasFor))
			{
				helptext = CommandData.CommandList[cmd.AliasFor].HelpText;
			}
			var firstline =
				$"{cmdname} - {cmd.GetCategoryString()}{(cmd.AliasFor != null ? $"(Alias For {cmd.AliasFor})" : "")}\nRequired Permissions: {cmd.Permission}";
			return string.IsNullOrWhiteSpace(helptext) ? $"{firstline}" : $"{firstline}\n -> {helptext}";
		}

		internal static void Version(DiscordClient client, CommandInfo info, DiscordMember author, DiscordChannelBase channel)
		{
			Utilities.WriteLog(author, "requested bot version.");
			client.SendMessageToRoom($"XenoBot v{Utilities.GetVersion()}", channel);
		}
	}
}
