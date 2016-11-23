using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using XenoBot2.Shared;

namespace XenoBot2
{
	internal static class CommandParser
	{
		public static Tuple<CommandInfo, Command> ParseCommand(string commandline, Channel channelContext)
		{
			var chunks = commandline.Split(' ');
			var cmdinfo = new CommandInfo();

			if (chunks.LessThan(1))
				return null;

			var cmd = chunks.First();

			cmdinfo.CommandText = cmd;

			cmdinfo.Arguments = chunks.AtLeast(2) ? chunks.Skip(1).ToList() : new List<string>();

			var state = Program.BotInstance.CommandStateData[cmd, channelContext.Id];
			if (!Program.BotInstance.Commands.Contains(cmd))
				state |= CommandState.DoesNotExist;
			cmdinfo.State = state;
			if (state.HasFlag(CommandState.Disabled) || state.HasFlag(CommandState.DoesNotExist))
				return new Tuple<CommandInfo, Command>(cmdinfo, null);

			return new Tuple<CommandInfo, Command>(cmdinfo, Program.BotInstance.Commands[cmd].ResolveCommand());
		}
	}
}
