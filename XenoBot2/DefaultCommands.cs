using System.Collections.Generic;
using XenoBot2.Shared;

namespace XenoBot2
{
	internal static class DefaultCommands
	{
		public static readonly IDictionary<string, Command> Content = new Dictionary<string, Command>
		{
			{
				"$wtc", new Command
				{
					AliasFor = "$commit"
				}
			},
			{
				"$date", new Command
				{
					HelpText = "Shows the current date in UTC.",
					HelpCategory = "Utility",
					Definition = Commands.Utility.Date
				}
			},
			{
				"$time", new Command
				{
					HelpText = "Shows the current time in UTC",
					HelpCategory = "Utility",
					Definition = Commands.Utility.Time
				}
			},
			{
				"$enable", new Command
				{
					HelpText = "Enables a disabled command on the current channel.",
					Permission = Permission.Administrator,
					HelpCategory = "Administration",
					Flags = CommandFlag.NoPrivateChannel | CommandFlag.NonDisableable,
					Definition = Commands.Administration.Enable
				}
			},
			{
				"$disable", new Command
				{
					HelpText = "Disables an enabled command on the current channel.",
					Permission = Permission.Administrator,
					HelpCategory = "Administration",
					Flags = CommandFlag.NoPrivateChannel | CommandFlag.NonDisableable,
					Definition = Commands.Administration.Disable
				}
			},
			{
				"$test", new Command
				{
					AliasFor = "$echo"
				}
			},
			{
				"$cat", new Command
				{
					HelpText = "I would like to unsubscribe from cat facts.",
					HelpCategory = "Fun",
					Definition = Commands.Fun.CatFact
				}
			},
			{
				"$commit", new Command
				{
					HelpText = "Shows a humorous commit message from WhatTheCommit.",
					HelpCategory = "Fun",
					Definition = Commands.Fun.WhatTheCommit
				}
			},
			{
				"$echo", new Command
				{
					HelpText = "Echos the command arguments.",
					HelpCategory = "Utility",
					Definition = Commands.Utility.Echo
				}
			},
			{
				"$me", new Command
				{
					HelpText = "Prints some basic profile info about you.",
					HelpCategory = "Utility",
					Definition = Commands.Utility.Me
				}
			},
			{
				"$numeral", new Command
				{
					HelpText = "Converts an integer into another format. Available formats: roman, words, wordord, metric.",
					LongHelpText = "Arguments: format number [culture]\n" +
								   "Converts an integer into another format.\n" +
								   "Available formats:\n" +
								   "* roman - Roman Numerals, i.e. \"XIV\"\n" +
								   "* words - Words, i.e. \"twenty seven\"\n" +
								   "* wordord - Words (ordinal), i.e. \"twenty seventh\"\n" +
								   "* metric - Metric prefixed, i.e. \"200K\"\n" +
								   "Optionally, a 2-letter culture code can be specified to get output in that language's format.",
					HelpCategory = "Utility",
					Definition = Commands.Utility.ConvertNumber
				}
			},
			{
				"$num", new Command
				{
					AliasFor = "$numeral"
				}
			},
			{
				"$!halt", new Command
				{
					HelpText = "Shuts down the bot.",
					Permission = Permission.BotAdministrator,
					HelpCategory = "Administration",
					Definition = Commands.Administration.HaltBot,
					Flags = CommandFlag.NonDisableable
				}
			},
			{
				"$help", new Command
				{
					HelpText = "Displays this highly informative help text. Use \"$help command\" to get help for command.",
					HelpCategory = "CommandCategory.Base",
					Definition = Commands.Base.Help,
					Flags = CommandFlag.UsableWhileIgnored | CommandFlag.NonDisableable
				}
			},
			{
				"$trump", new Command
				{
					HelpText = "MAKE HELP GREAT AGAIN!",
					LongHelpText = "Prints a message in the form \"Make $THING great again!\"\n" +
								   "If an argument is passed, $THING is replaced with the argument.\"\n" +
								   "If no arguments are passed, 'bots' is used instead.",
					HelpCategory = "Fun (Offensive)",
					Definition = Commands.FunOffensive.Trump
				}
			},
			{
				"$version", new Command
				{
					HelpText = "Prints the bot's current version number.",
					HelpCategory = "Core",
					Definition = Commands.Base.Version
				}
			},
			{
				"sudo", new Command
				{
					Flags = CommandFlag.Hidden,
					Definition = Commands.Retorts.Sudo
				}
			},
			{
				"$user", new Command
				{
					HelpCategory = "Utility",
					HelpText = "Gets some information about a user.",
					Definition = Commands.Utility.UserInfo
				}
			},
			{
				"$8", new Command
				{
					HelpCategory = "Fun",
					Definition = Commands.Fun.EightBall,
					HelpText = "Consult the 8 Ball."
				}
			},
			{
				"test", new Command
				{
					AliasFor = "ping"
				}
			},
			{
				"ping", new Command
				{
					Definition = Commands.Utility.Ping,
					HelpCategory = "Utility",
					Flags = CommandFlag.Hidden
				}
			},
			{
				"$avatar", new Command
				{
					HelpCategory = "Utility",
					HelpText = "Gets the avatar for the given user",
					Definition = Commands.Utility.Avatar
				}
			},
			{
				"$!debug", new Command
				{
					HelpCategory = "Core",
					Permission = Permission.BotAdministrator,
					Definition = Commands.Administration.BotDebug,
					Flags = CommandFlag.NonDisableable | CommandFlag.Hidden,
					HelpText = "Miscellaneous debugging commands."
				}
			},
			{
				"$ignore", new Command
				{
					HelpCategory = "Core",
					Permission = Permission.Moderator,
					Definition = Commands.Administration.IgnoreUser,
					HelpText = "Toggles command ignore for a user."
				}
			}
		};
	}
}
