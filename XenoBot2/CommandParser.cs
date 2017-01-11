using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using XenoBot2.Shared;

namespace XenoBot2
{
	internal static class CommandParser
	{
		public static CommandInfo ParseCommand(string commandline, Channel channelContext)
		{
			if (string.IsNullOrEmpty(commandline) || commandline.Length > 200)
				return null;

			var chunks = commandline.Split(' ');

			var cmdinfo = new CommandInfo();

			var cmd = chunks.First();

			cmdinfo.CommandText = cmd;

			cmdinfo.Arguments = chunks.AtLeast(2) ? chunks.Skip(1).ToList() : new List<string>();

			var state = Program.BotInstance.CommandStateData[cmd, channelContext.Id];
			if (!Program.BotInstance.Commands.Contains(cmd))
				state |= CommandState.DoesNotExist;
			cmdinfo.State = state;
			if (state.HasFlag(CommandState.Disabled) || state.HasFlag(CommandState.DoesNotExist))
				return cmdinfo;

			cmdinfo.BoundCommand = Program.BotInstance.Commands[cmd].ResolveCommand();
			return cmdinfo;
		}

		public static async Task ParseMessage(Message msg, bool skipPrefix = false)
		{
			// Ignore empty or non-prefixed messages, unless prefix checking has been disabled.
			if (string.IsNullOrWhiteSpace(msg.RawText) || (msg.RawText[0] != Program.BotInstance.Prefix && !skipPrefix)) return;

			var text = skipPrefix ? msg.RawText : msg.RawText.Substring(1);

			// Process command & execute
			var cmd = ParseCommand(text, msg.Channel);

			if (cmd == null) return;

			if (cmd.State.HasFlag(CommandState.DoesNotExist))
			{
				await msg.Channel.SendMessage("That command was not recognised.");
				return;
			}

			if (cmd.BoundCommand == null)
			{
				Utilities.WriteLog("WARNING: ParseCommand() returned null BoundCommand!");
				return;
			}

			var bound = cmd.BoundCommand;

			// Check ignore state
			if (!bound.Flags.HasFlag(CommandFlag.UsableWhileIgnored) && Utilities.Permitted(UserFlag.Ignored, msg.User, msg.Channel))
				return;

			if (!Utilities.Permitted(bound.Permission, msg.User, msg.Channel))
			{
				await msg.Channel.SendMessage("You are not authorized to run that command here.");
				Utilities.WriteLog(msg.User, $"was denied access to command '{cmd.CommandText}'");
				return;
			}

			if ((bound.Flags.HasFlag(CommandFlag.NoPrivateChannel) && msg.Channel.IsPrivate) ||
				bound.Flags.HasFlag(CommandFlag.NoPublicChannel) && !msg.Channel.IsPrivate)
			{
				await msg.Channel.SendMessage("That command cannot be used here.");
				return;
			}
			try
			{
				// execute command
				await cmd.BoundCommand.Definition(cmd, msg);
			}
			catch (Exception ex)
			{
				Utilities.WriteLog($"An exception occurred during execution of command '{cmd.CommandText}'," +
								   $" args '{string.Join(", ", cmd.Arguments)}'.\n" + ex.Message);
				await msg.Channel.SendMessage($"An internal error occurred running command '{cmd.CommandText}'.");
			}
		}
	}
}
