using System.Collections.Generic;
using System.Linq;
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
	}
}
