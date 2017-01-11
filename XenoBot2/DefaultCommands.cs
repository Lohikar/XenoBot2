﻿using System.Collections.Generic;
using XenoBot2.Commands;
using XenoBot2.Shared;

namespace XenoBot2
{
	internal static class DefaultCommands
	{
		public static readonly IDictionary<string, Command> Content = new Dictionary<string, Command>
		{
			{
				"wtc", new Command
				{
					AliasFor = "commit"
				}
			},
			{
				"date", new Command
				{
					HelpText = "Shows the current date in UTC.",
					HelpCategory = "Utility",
					Definition = Utility.Date
				}
			},
			{
				"time", new Command
				{
					HelpText = "Shows the current time in UTC",
					HelpCategory = "Utility",
					Definition = Utility.Time
				}
			},
			{
				"enable", new Command
				{
					HelpText = "Enables a disabled command on the current channel.",
					Permission = UserFlag.Administrate,
					HelpCategory = "Administration",
					Flags = CommandFlag.NoPrivateChannel | CommandFlag.NonDisableable,
					Definition = ChannelAdministration.Enable,
					Arguments = "command"
				}
			},
			{
				"disable", new Command
				{
					HelpText = "Disables an enabled command on the current channel.",
					Permission = UserFlag.Administrate,
					HelpCategory = "Administration",
					Flags = CommandFlag.NoPrivateChannel | CommandFlag.NonDisableable,
					Definition = ChannelAdministration.Disable,
					Arguments = "command"
				}
			},
			{
				"test", new Command
				{
					AliasFor = "echo"
				}
			},
			{
				"catfact", new Command
				{
					HelpText = "I would like to unsubscribe from cat facts.",
					HelpCategory = "Fun",
					Definition = Fun.CatFact
				}
			},
			{
				"commit", new Command
				{
					HelpText = "Shows a humorous commit message from WhatTheCommit.",
					HelpCategory = "Fun",
					Definition = Fun.WhatTheCommit
				}
			},
			{
				"echo", new Command
				{
					HelpText = "Echos the command arguments.",
					HelpCategory = "Utility",
					Definition = Utility.Echo,
					Arguments = "text"
				}
			},
			{
				"me", new Command
				{
					HelpText = "Prints some basic profile info about you.",
					HelpCategory = "Utility",
					Definition = Utility.Me,
					Flags = CommandFlag.UsableWhileIgnored
				}
			},
			{
				"numeral", new Command
				{
					HelpText = "Converts an integer into another format.",
					Arguments = "{roman|words|wordord|metric} number",
					LongHelpText = "Converts an integer into another format.\n" +
					               "Available formats:\n" +
					               "* roman   - Roman Numerals, i.e. \"XIV\"\n" +
					               "* words   - Words, i.e. \"twenty seven\"\n" +
					               "* wordord - Words (ordinal), i.e. \"twenty seventh\"\n" +
					               "* metric  - Metric prefixed, i.e. \"200K\"",
					HelpCategory = "Utility",
					Definition = Utility.ConvertNumber
				}
			},
			{
				"num", new Command
				{
					AliasFor = "numeral"
				}
			},
			{
				"!halt", new Command
				{
					HelpText = "Shuts down the bot.",
					Permission = UserFlag.BotAdministrate,
					HelpCategory = "Administration",
					Definition = BotAdministration.HaltBot,
					Flags = CommandFlag.NonDisableable | CommandFlag.UsableWhileIgnored,
					LongHelpText = "Terminates the bot's process. The bot cannot respond to commands after this command is run!"
				}
			},
			{
				"help", new Command
				{
					HelpText = "Displays this highly informative help text. Use \"$help command\" to get help for command.",
					HelpCategory = "Core",
					Definition = Base.Help,
					Flags = CommandFlag.UsableWhileIgnored | CommandFlag.NonDisableable,
					Arguments = "[topic]"
				}
			},
			{
				"version", new Command
				{
					HelpText = "Prints the bot's current version number.",
					HelpCategory = "Core",
					Definition = Base.Version
				}
			},
			{
				"user", new Command
				{
					HelpCategory = "Utility",
					HelpText = "Gets some information about a user.",
					Definition = Utility.UserInfo,
					Arguments = "mention"
				}
			},
			{
				"8", new Command
				{
					HelpCategory = "Fun",
					Definition = Fun.EightBall,
					HelpText = "Consult the 8 Ball.",
					Arguments = "[question]"
				}
			},
			{
				"ping", new Command
				{
					Definition = Utility.Ping,
					HelpCategory = "Utility",
					Flags = CommandFlag.Hidden
				}
			},
			{
				"avatar", new Command
				{
					HelpCategory = "Utility",
					HelpText = "Gets the avatar for the given user",
					Definition = Utility.Avatar,
					Arguments = "[mention]"
				}
			},
			{
				"ignore", new Command
				{
					HelpCategory = "Core",
					Permission = UserFlag.Moderate,
					Flags = CommandFlag.UsableWhileIgnored | CommandFlag.NoPrivateChannel,
					Definition = ChannelAdministration.IgnoreUser,
					HelpText = "Toggles command ignore for a user.",
					Arguments = "mention"
				}
			},
			{
				"globalignore", new Command
				{
					HelpCategory = "Core",
					Permission = UserFlag.BotAdministrate,
					Flags = CommandFlag.NonDisableable | CommandFlag.UsableWhileIgnored,
					Definition = BotAdministration.GlobalIgnoreUser,
					HelpText = "Toggles command ignore for a user on all channels.",
					Arguments = "mention"
				}
			},
			{
				"!cmdinfo", new Command
				{
					HelpCategory = "Debug",
					Permission = UserFlag.Debug,
					Flags = CommandFlag.NonDisableable,
					Definition = Debug.Cmdinfo,
					Arguments = "command"
				}
			},
			{
				"!chinfo", new Command
				{
					HelpCategory = "Debug",
					Permission = UserFlag.Debug,
					Flags = CommandFlag.NonDisableable,
					Definition = Debug.GetChannelInfo
				}
			},
			{
				"cat", new Command
				{
					HelpCategory = "Fun",
					Definition = Fun.RandomCat,
					HelpText = "Meow."
				}
			},
			{
				"die", new Command
				{
					HelpCategory = "Fun",
					Definition = Fun.RollDie,
					HelpText = "Rolls a die.",
					Arguments = "([dies]d[sides]|[dies] [sides])"
				}
			}
		};
	}
}