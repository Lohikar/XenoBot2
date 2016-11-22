using System;
using System.Collections.Generic;
using System.Linq;
using DiscordSharp.Objects;
using XenoBot2.Shared;

namespace XenoBot2
{
	internal static class CommandParser
	{
		public static Tuple<CommandInfo, Command> ParseCommand(string commandline, DiscordChannelBase channelContext)
		{
			var chunks = commandline.Split(' ');
			var cmdinfo = new CommandInfo();

			if (chunks.LessThan(1))
				return null;

			var cmd = chunks.First();
			cmdinfo.CommandText = cmd;

			cmdinfo.Arguments = chunks.AtLeast(2) ? chunks.Skip(1).ToList() : new List<string>();

			var state = SharedData.CommandState[cmd, channelContext.ID];
			if (state.HasFlag(CommandStateFlag.Disabled))
				return new Tuple<CommandInfo, Command>(cmdinfo, null);

			return new Tuple<CommandInfo, Command>(cmdinfo, CommandStore.Commands[cmd].ResolveCommand());
		}
	}
}
