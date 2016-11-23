using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Humanizer;
using XenoBot2.Data;
using XenoBot2.Shared;

namespace XenoBot2.Commands
{
	/// <summary>
	///		Bot administration commands. Commands in this group can only be executed by the bot administrator.
	/// </summary>
	internal static class Administration
	{
		internal static async Task HaltBot(CommandInfo info, User author, Channel channel)
		{
			Utilities.WriteLog(author, "is shutting down the bot.");
			await channel.SendMessage("Bot shutting down.");
			await Program.BotInstance.Exit();
		}

		internal static async Task Enable(CommandInfo info, User member, Channel channel)
		{
			if (!info.HasArguments)
			{
				await channel.SendMessage("You must specify the command to enable.");
				return;
			}
			//if (Program.CombinedChannelCommandMgr.EnableCommand(info.Arguments.First(), channel))
			//	client.SendMessageToRoom($"Enabled command '{info.Arguments.First()}'", channel);
		}

		internal static async Task Disable(CommandInfo info, User member, Channel channel)
		{
			if (!info.HasArguments)
			{
				await channel.SendMessage("You must specify the command to disable.");
				return;
			}
			//if (Program.CombinedChannelCommandMgr.DisableCommand(info.Arguments.First(), channel))
			//	client.SendMessageToRoom($"Disabled command '{info.Arguments.First()}'", channel);
		}

		internal static async Task BotDebug(CommandInfo info, User member, Channel channel)
		{
			if (!Utilities.Permitted(UserFlag.BotDebug, member))
			{
				Utilities.WriteLog($"WARN: non-admin client '{member.GetFullUsername()}' ran $debug.");
				return;
			}

			if (!info.HasArguments)
			{
				await channel.SendMessage("Argument required.");
				return;
			}

			switch (info.Arguments[0])
			{
				case "except":	// throws an exception 
					throw new InvalidOperationException("$debug::except");
				//case "cmsg":    // send message to channel
				//	if (info.Arguments.LessThan(2))
				//	{
				//		await channel.SendMessage("not enough arguments");
				//		break;
				//	}
				//	var id = long.Parse(info.Arguments[1]);
				//	var msg = string.Join(" ", info.Arguments.Skip(2));
				//	.SendMessageToChannel(msg, client.GetChannelByID(id));
				//	client.SendMessageToUser("Sent message to channel.", member);
				//	Utilities.WriteLog(member, $"sent message '{msg}' to channel '{client.GetChannelByID(id).GetName()}'");
				//	break;
				case "cid":		// get channel ID
					await channel.SendMessage($"cid is {channel.Id}");
					Utilities.WriteLog(member, $"requested channelID (CID) for channel {channel.Name}");
					break;
				case "cmdinfo":
					if (!info.HasArguments)
					{
						await channel.SendMessage("not enough arguments");
						break;
					}
					var cmdtxt = string.Join(" ", info.Arguments.Skip(1));
					Utilities.WriteLog(member, $"requested cmdinfo for '{cmdtxt}'");
					var cmd = CommandParser.ParseCommand(cmdtxt, channel);
					if (cmd == null)
					{
						await channel.SendMessage("Error: Command is not defined");
						return;
					}
					var cmddata = CommandStore.Commands[cmd.Item1.CommandText].ResolveCommand();
					await channel.SendMessage($"Input: {cmdtxt}\n" +
					                         "```\n" +
					                         $"CmdText: {cmd.Item1.CommandText}\n" +
					                         $"Flags: {cmddata.Flags}\n" +
											 $"PermissionFlags: {cmddata.Permission}\n" +
					                         $"Category: {cmddata.HelpCategory}\n" +
					                         "```");
					break;
				//case "setgame":
				//	if (!info.HasArguments)
				//	{
				//		await channel.SendMessage("not enough arguments");
				//	}
				//	var gametext = string.Join(" ", info.Arguments.Skip(1));
				//	Utilities.WriteLog(member, $"set game to '{gametext}'");
				//	client.UpdateCurrentGame(gametext);
				//	break;
				default:
					await channel.SendMessage("That is not a valid sub-command.");
					Utilities.WriteLog(member, "issued invalid debug command.");
					break;
			}
		}

		internal static async Task IgnoreUser(CommandInfo info, User member, Channel channel)
		{
			if (!info.HasArguments)
			{
				Utilities.WriteLog(member, "tried to ignore, but forgot to say who to ignore.");
				await channel.SendMessage("You must specify who to ignore.");
				return;
			}

			var user = info.Arguments.First().GetMemberFromMention(channel);

			if (ToggleIgnore(member.Id, channel.Id))
			{
				Utilities.WriteLog(member, $"ignored {user.GetFullUsername()} on channel {channel.Name}");
				await channel.SendMessage($"Now ignoring {user.NicknameMention} on this channel.");
			}
			else
			{
				Utilities.WriteLog(member, $"unignored {user.GetFullUsername()} on channel {channel.Name}");
				await channel.SendMessage($"No longer ignoring {user.NicknameMention} on this channel.");
			}
		}

		internal static async Task GlobalIgnoreUser(CommandInfo info, User member, Channel channel)
		{
			if (!info.HasArguments)
			{
				Utilities.WriteLog(member, "tried to ignore, but forgot to say who to ignore.");
				await channel.SendMessage("You must specify who to ignore.");
				return;
			}

			var user = info.Arguments.First().GetMemberFromMention(channel);

			if (ToggleIgnore(member.Id))
			{
				Utilities.WriteLog(member, $"ignored {user.GetFullUsername()} globally.");
				await channel.SendMessage($"Now ignoring {user.NicknameMention} everywhere.");
			}
			else
			{
				Utilities.WriteLog(member, $"unignored {user.GetFullUsername()} globally.");
				await channel.SendMessage($"No longer ignoring {user.NicknameMention} everywhere (unless specifically ignored elsewhere).");
			}
		}

		/// <summary>
		///		Toggles the ignore state for a user, globally or per-channel.
		/// </summary>
		/// <param name="uid">The user ID to toggle ignore for.</param>
		/// <param name="chid">The channel ID to ignore on. Omit for global.</param>
		/// <returns>True if user is now ignored, false if not.</returns>
		private static bool ToggleIgnore(ulong uid, ulong chid = 0)
		{
			var ignored = Utilities.Permitted(UserFlag.Ignored, uid);
			if (ignored)
				Program.BotInstance.UserFlags[uid, chid] ^= UserFlag.Ignored;
			else
				Program.BotInstance.UserFlags[uid, chid] |= UserFlag.Ignored;

			return !ignored;
		}
	}
}
