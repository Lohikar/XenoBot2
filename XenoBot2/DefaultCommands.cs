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
					Category = CommandCategory.Utility,
					Definition = Commands.Utility.Date
				}
			},
			{
				"$time", new Command
				{
					HelpText = "Shows the current time in UTC",
					Category = CommandCategory.Utility,
					Definition = Commands.Utility.Time
				}
			},
			{
				"$enable", new Command
				{
					HelpText = "Enables a disabled command on the current channel.",
					Permission = PermissionFlag.Administrator,
					Category = CommandCategory.Administration,
					Flags = CommandFlag.NoPrivateChannel | CommandFlag.NonDisableable,
					Definition = Commands.Administration.Enable
				}
			},
			{
				"$disable", new Command
				{
					HelpText = "Disables an enabled command on the current channel.",
					Permission = PermissionFlag.Administrator,
					Category = CommandCategory.Administration,
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
					Category = CommandCategory.Fun,
					Definition = Commands.Fun.CatFact
				}
			},
			{
				"$commit", new Command
				{
					HelpText = "Shows a humorous commit message from WhatTheCommit.",
					Category = CommandCategory.Fun,
					Definition = Commands.Fun.WhatTheCommit
				}
			},
			{
				"$echo", new Command
				{
					HelpText = "Echos the command arguments.",
					Category = CommandCategory.Utility,
					Definition = Commands.Utility.Echo
				}
			},
			{
				"$me", new Command
				{
					HelpText = "Prints some basic profile info about you.",
					Category = CommandCategory.Utility,
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
					Category = CommandCategory.Utility,
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
				"$!HALT", new Command
				{
					HelpText = "Shuts down the bot.",
					Permission = PermissionFlag.BotAdministrator,
					Category = CommandCategory.Administration,
					Definition = Commands.Administration.HaltBot,
					Flags = CommandFlag.NonDisableable
				}
			},
			{
				"$help", new Command
				{
					HelpText = "Displays this highly informative help text. Use \"$help command\" to get help for command.",
					Category = CommandCategory.Base,
					Definition = Commands.Base.Help,
					Flags = CommandFlag.UsableWhileIgnored | CommandFlag.NonDisableable
				}
			},
			{
				"$penis", new Command
				{
					Flags = CommandFlag.Hidden,
					Category = CommandCategory.FunOffensive,
					Definition = Commands.FunOffensive.Penis,
					HelpText = "Prints an arbitrary length ASCII penis."
				}
			},
			{
				"$trump", new Command
				{
					HelpText = "MAKE HELP GREAT AGAIN!",
					LongHelpText = "Prints a message in the form \"Make $THING great again!\"\n" +
								   "If an argument is passed, $THING is replaced with the argument.\"\n" +
								   "If no arguments are passed, 'bots' is used instead.",
					Category = CommandCategory.FunOffensive,
					Definition = Commands.FunOffensive.Trump
				}
			},
			{
				"$version", new Command
				{
					HelpText = "Prints the bot's current version number.",
					Category = CommandCategory.Base,
					Definition = Commands.Base.Version
				}
			},
			{
				"sudo", new Command
				{
					Flags = CommandFlag.Hidden,
					Category = CommandCategory.Retorts,
					Definition = Commands.Retorts.Sudo
				}
			},
			{
				"back", new Command
				{
					Category = CommandCategory.Retorts,
					Definition = Commands.Retorts.WelcomeBack
				}
			},
			{
				"$user", new Command
				{
					Category = CommandCategory.Utility,
					HelpText = "Gets some information about a user.",
					Definition = Commands.Utility.UserInfo
				}
			},
			{
				"front", new Command
				{
					Category = CommandCategory.Retorts,
					Definition = Commands.Retorts.Front
				}
			},
			{
				"<3", new Command
				{
					Category = CommandCategory.Retorts,
					Definition = Commands.Retorts.Heart
				}
			},
			{
				"$8", new Command
				{
					Category = CommandCategory.Fun,
					Definition = Commands.Fun.EightBall,
					HelpText = "Consult the 8 Ball."
				}
			},
			{
				"Back", new Command
				{
					AliasFor = "back"
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
					Category = CommandCategory.Utility,
					Flags = CommandFlag.Hidden
				}
			},
			{
				".", new Command
				{
					AliasFor = "ping"
				}
			},
			{
				"$avatar", new Command
				{
					Category = CommandCategory.Utility,
					HelpText = "Gets the avatar for the given user",
					Definition = Commands.Utility.Avatar
				}
			},
			{
				"$m", new Command
				{
					Category = CommandCategory.Fun,
					HelpText = "Fetches a meme from lohikar.io.",
					Definition = Commands.Fun.PrefixMeme
				}
			},
			{
				"$!debug", new Command
				{
					Category = CommandCategory.Administration,
					Permission = PermissionFlag.BotAdministrator,
					Definition = Commands.Administration.BotDebug,
					Flags = CommandFlag.NonDisableable | CommandFlag.Hidden,
					HelpText = "Miscellaneous debugging commands."
				}
			},
			{
				"$ignore", new Command
				{
					Category = CommandCategory.Administration,
					Permission = PermissionFlag.Moderator,
					Definition = Commands.Administration.IgnoreUser,
					HelpText = "Toggles command ignore for a user."
				}
			}
		};
	}
}
