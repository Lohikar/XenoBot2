using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using XenoBot2.Data;
using XenoBot2.Shared;

namespace XenoBot2
{
	internal static class Program
	{
		private static DiscordClient _client;

		public const char Prefix = '$';

		private static void Main(string[] args)
		{
			Utilities.WriteLog($"XenoBot2 v{Utilities.GetVersion()} starting initialization...");
			// Initialize command storage
			CommandStore.Commands = new CommandStore();
			// load default commands
			CommandStore.Commands.AddMany(DefaultCommands.Content);
			SharedData.CommandState = new UserDataStore<string, ulong, CommandState>(CommandState.None, 0);
			SharedData.UserFlags = new UserDataStore<ulong, ulong, UserFlag>(UserFlag.User, 0);

			SharedData.UserFlags.Add(174018252161286144, SharedData.UserFlags.GlobalValue, UserFlag.BotAdministrator);

			Utilities.WriteLog("Loading API key from disk...");
			StreamReader apifile;
			if (File.Exists("apikey.txt"))
			{
				apifile = File.OpenText("apikey.txt");
				Utilities.WriteLog("Found API key, starting client...");
			}
			else
			{
				Utilities.WriteLog("Error: API key not found.\n" +
				                   "Please create a file named 'apikey.txt' in the same directory as the binary, " +
				                   "and put your Discord Bot API key into the file.\n" +
				                   "Press Enter to exit.");
				Console.ReadLine();
				return;
			}

			// initialize client
			_client = new DiscordClient();

			_client.MessageReceived += async (s, e) =>
			{
				if (!e.Message.IsAuthor)
					await ParseMessage(e.User, e.Channel, e.Message.RawText);
			};


			Utilities.WriteLog("Setting up WhatTheCommit...");

			// TODO: Cache wtc info instead of re-downloading each time
			var wtc =
				Shared.Utilities.GetStringFromWebService("https://www.lohikar.io/assets/xenobot/commits.txt");
			Strings.WhatTheCommit = wtc.Split('\n');

			Strings.Names = Shared.Utilities.GetStringFromWebService("https://www.lohikar.io/assets/xenobot/names.txt").Split('\n');

			Utilities.WriteLog("Done.");

			_client.ExecuteAndWait(async () =>
			{
				await _client.Connect(await apifile.ReadToEndAsync(), TokenType.Bot);
			});
		}

		private static async Task ParseMessage(User author, Channel channel, string messageText, bool skipPrefix = false)
		{
			// silently ignore our own messages & messages from other bots
			if (string.IsNullOrWhiteSpace(messageText) ||
			    (messageText[0] != Prefix && !skipPrefix)) return;

			var text = skipPrefix ? messageText : messageText.Substring(1);

			// Process command & execute
			var cmd = CommandParser.ParseCommand(text, channel);
			if (cmd.Item1.State.HasFlag(CommandState.DoesNotExist))
				return;
			if (cmd.Item2 == null)
			{
				Utilities.WriteLog(string.Format(Messages.CommandNotDefined, cmd.Item1));
				SendMessageToRoom($"The command '{cmd.Item1}' seems to be broken right now.", channel);
				return;
			}
			if ((cmd.Item2.Flags.HasFlag(CommandFlag.NoPrivateChannel) && channel.IsPrivate) ||
			    cmd.Item2.Flags.HasFlag(CommandFlag.NoPublicChannel) && !channel.IsPrivate)
			{
				SendMessageToRoom("That command cannot be used here.", channel);
				return;
			}
			if (!cmd.Item2.Flags.HasFlag(CommandFlag.UsableWhileIgnored) && Utilities.Permitted(UserFlag.Ignored, author))
				return;
			try
			{
				// execute command
				await cmd.Item2.Definition(cmd.Item1, author, channel);
			}
			catch (InvalidCommandException ex)
			{
				Utilities.WriteLog($"Attempted use of invalid command '{ex.AttemptedCommand}'.");
				SendMessageToRoom("That command is not understood.", channel);
			}
			catch (Exception ex)
			{
				Utilities.WriteLog($"An exception occurred during execution of command '{cmd.Item1.CommandText}'," +
				                   $" args '{string.Join(", ", cmd.Item1.Arguments)}'.\n" + ex.Message);
				SendMessageToRoom($"An internal error occurred running command '{cmd.Item1.CommandText}'.", channel);
			}
		}

		private static void SendMessageToRoom(string data, Channel channel) => channel.SendMessage(data);
	}
}
