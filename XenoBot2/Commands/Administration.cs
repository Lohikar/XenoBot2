using System;
using System.Linq;
using DiscordSharp;
using DiscordSharp.Objects;
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
		internal static void HaltBot(DiscordClient client, CommandInfo info, DiscordMember author, DiscordChannelBase channel)
		{
			if (author.ID == Ids.Admin)
			{
				Utilities.WriteLog(author, "is shutting down the bot; is admin.");
				client.SendMessageToRoom("Bot shutting down.", channel);
				Program.Exit();
			}
			else
			{
				Utilities.WriteLog(author, "attempted to shutdown bot, but is not admin.");
				client.SendMessageToRoom("You are not the bot administrator.", channel);
			}
		}

		internal static void Enable(DiscordClient client, CommandInfo info, DiscordMember member, DiscordChannelBase channel)
		{
			if (!info.HasArguments)
			{
				client.SendMessageToRoom("You must specify the command to enable.", channel);
				return;
			}
			//if (Program.CombinedChannelCommandMgr.EnableCommand(info.Arguments.First(), channel))
			//	client.SendMessageToRoom($"Enabled command '{info.Arguments.First()}'", channel);
		}

		internal static void Disable(DiscordClient client, CommandInfo info, DiscordMember member, DiscordChannelBase channel)
		{
			if (!info.HasArguments)
			{
				client.SendMessageToRoom("You must specify the command to disable.", channel);
				return;
			}
			//if (Program.CombinedChannelCommandMgr.DisableCommand(info.Arguments.First(), channel))
			//	client.SendMessageToRoom($"Disabled command '{info.Arguments.First()}'", channel);
		}

		internal static void BotDebug(DiscordClient client, CommandInfo info, DiscordMember member, DiscordChannelBase channel)
		{
			if (member.ID != Ids.Admin)
			{
				Utilities.WriteLog($"WARN: non-admin client '{member.GetFullUsername()}' ran $debug.");
			}

			if (!info.HasArguments)
			{
				client.SendMessageToRoom("Argument required.", channel);
				return;
			}

			switch (info.Arguments[0])
			{
				case "except":	// throws an exception 
					throw new InvalidOperationException("$debug::except");
				case "cmsg":	// send message to channel
					if (info.Arguments.LessThan(2))
					{
						client.SendMessageToRoom("not enough arguments", channel);
						break;
					}
					var id = long.Parse(info.Arguments[1]);
					var msg = string.Join(" ", info.Arguments.Skip(2));
					client.SendMessageToChannel(msg, client.GetChannelByID(id));
					client.SendMessageToUser("Sent message to channel.", member);
					Utilities.WriteLog(member, $"sent message '{msg}' to channel '{client.GetChannelByID(id).GetName()}'");
					break;
				case "cid":		// get channel ID
					client.SendMessageToRoom($"cid is {channel.ID}", channel);
					Utilities.WriteLog(member, $"requested channelID (CID) for channel {channel.GetName()}");
					break;
				case "cmdinfo":
					if (!info.HasArguments)
					{
						client.SendMessageToRoom("not enough arguments", channel);
						break;
					}
					var cmdtxt = string.Join(" ", info.Arguments.Skip(1));
					Utilities.WriteLog(member, $"requested cmdinfo for '{cmdtxt}'");
					var cmd = CommandParser.ParseCommand(cmdtxt, channel);
					if (cmd == null)
					{
						client.SendMessageToRoom("Error: Command is not defined", channel);
						return;
					}
					var cmddata = CommandStore.Commands[cmd.Item1.CommandText].ResolveCommand();
					client.SendMessageToRoom($"Input: {cmdtxt}\n" +
					                         "```\n" +
					                         $"CmdText: {cmd.Item1.CommandText}\n" +
					                         $"Flags: {cmddata.Flags}\n" +
											 $"PermissionFlags: {cmddata.Permission}\n" +
					                         $"Category: {cmddata.HelpCategory}\n" +
					                         "```", channel);
					break;
				case "setgame":
					if (!info.HasArguments)
					{
						client.SendMessageToRoom("not enough arguments", channel);
					}
					var gametext = string.Join(" ", info.Arguments.Skip(1));
					Utilities.WriteLog(member, $"set game to '{gametext}'");
					client.UpdateCurrentGame(gametext);
					break;
				default:
					client.SendMessageToRoom("That is not a valid sub-command.", channel);
					Utilities.WriteLog(member, "issued invalid debug command.");
					break;
			}
		}

		internal static void IgnoreUser(DiscordClient client, CommandInfo info, DiscordMember member,
			DiscordChannelBase channel)
		{
			if (!info.HasArguments)
			{
				Utilities.WriteLog(member, "tried to ignore, but forgot to say who to ignore.");
				client.SendMessageToRoom("You must specify who to ignore.", channel);
				return;
			}

			var user = info.Arguments.First().GetMemberFromMention(client, channel);

			if (Ids.Ignored.Contains(user.ID))
			{
				Utilities.WriteLog(member, $"unignored {user.GetFullUsername()}");
				Ids.Ignored.Remove(user.ID);
				client.SendMessageToRoom($"Unignored {user.MakeMention()}.", channel);
			}
			else
			{
				Utilities.WriteLog(member, $"ignored {user.GetFullUsername()}");
				Ids.Ignored.Add(user.ID);
				client.SendMessageToRoom($"Ignored {user.MakeMention()}.", channel);
			}
		}
	}
}
