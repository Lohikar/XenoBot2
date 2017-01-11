using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using XenoBot2.Data;
using XenoBot2.Shared;
using static XenoBot2.Utilities;

namespace XenoBot2.Commands
{
	/// <summary>
	///		Core bot commands. This group cannot be disabled.
	/// </summary>
	internal static class Base
	{
		internal static async Task Help(CommandInfo info, Message msg)
		{
			Func<string, Task> send = async s =>
			{
				if (msg.User.PrivateChannel == null)
					await msg.User.CreatePMChannel();
				await msg.User.PrivateChannel.SendMessage(s);
				Thread.Sleep(100);
			};
			if (!msg.Channel.IsPrivate)
				await msg.Channel.SendMessage($"{msg.User.NicknameMention}: Sending you a PM!");


			if (!info.HasArguments)
			{
				WriteLog(msg.User, "requested help index.");

				await send(Messages.HelpTextHeader);
				// show list of commands + shorthelp

				var iter = 0;

				var helpLines = from item in Program.BotInstance.Commands
								where item.Value != null
								let channelPermitted = Permitted(item.Value.Permission, msg.User, msg.Channel)
								let globalPermitted = Permitted(item.Value.Permission, msg.User)
								where !item.Value.Flags.HasFlag(CommandFlag.Hidden) && channelPermitted
								let num = iter++
								group GenerateHelpEntry(item.Value, item.Key, channelPermitted, globalPermitted) by num / 10 into items
								select items;

				var builder = new StringBuilder();

				foreach (var i in helpLines)
				{
					builder.Clear();
					i.Aggregate(builder, (b, s) => b.AppendLine(s));
					await send(builder.ToString());
				}

			}
			else
			{
				if (!Program.BotInstance.Commands.Contains(info.Arguments[0])) return;
				var cmd = Program.BotInstance.Commands[info.Arguments[0]].ResolveCommand();

				WriteLog(msg.User, $"requested help page '{info.Arguments[0]}'");
				var builder = new StringBuilder();
				builder.AppendLine("```");
				builder.AppendLine($"Help - {info.Arguments[0]}");
				builder.AppendLine("---");
				builder.AppendLine($"Category: {cmd.HelpCategory}");
				builder.AppendLine($"Authorization: {cmd.Permission}");
				builder.AppendLine(GeneratePermissionLine(cmd, msg.User, msg.Channel));
				builder.Append("Arguments: ");
				builder.AppendLine(string.IsNullOrWhiteSpace(cmd.Arguments) ? "{none}" : cmd.Arguments);
				if (!string.IsNullOrWhiteSpace(cmd.HelpText))
				{
					builder.AppendLine("Short Description:");
					builder.AppendLine($"\t{cmd.HelpText}");
				}
				if (!string.IsNullOrWhiteSpace(cmd.LongHelpText))
				{
					builder.AppendLine("\nDescription: ");
					builder.AppendLine("\t" + cmd.LongHelpText.Replace("\n", "\n\t").TrimEnd());
				}
					builder.AppendLine("```");
				await send(builder.ToString());
			
			}
		}

		private static string GeneratePermissionLine(Command cmd, User user, Channel channel)
		{
			var globalPermitted = Permitted(cmd.Permission, user);
			var channelPermitted = Permitted(cmd.Permission, user, channel);

			if (globalPermitted) return "You are globally permitted to use this command.";
			return channelPermitted ? $"You are permitted to use this command in channel '{channel.Name}'." : "You are not permitted to use this command.";
		}

		private static string GenerateHelpEntry(Command cmd, string cmdname, bool channelPermitted = false, bool globalPermitted = false)
		{
			var builder = new StringBuilder();

			var helptext = cmd.HelpText;
			if (cmd.AliasFor != null)
				helptext = cmd.ResolveCommand().HelpText;
			
			builder.Append($"{cmdname} - {cmd.HelpCategory}");

			if (cmd.AliasFor != null)
				builder.AppendLine($"(Alias For {cmd.AliasFor})");
			else
				builder.AppendLine();

			builder.AppendLine($"Required Permissions: {cmd.Permission}");

			if (string.IsNullOrWhiteSpace(helptext)) return builder.ToString();
			builder.Append(" -> ");
			builder.AppendLine(helptext);

			return builder.ToString();
		}

		internal static async Task Version(CommandInfo info, Message msg)
		{
			WriteLog(msg.User, "requested bot version.");
			await msg.Channel.SendMessage($"`XenoBot2 v{GetVersion()} {Program.BuildType}`");
		}
	}
}
