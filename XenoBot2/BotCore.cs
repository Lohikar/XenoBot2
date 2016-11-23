using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using XenoBot2.Data;
using XenoBot2.Shared;

namespace XenoBot2
{
	internal class BotCore
	{
		private DiscordClient _client;

		private const char Prefix = '$';
		private string _key;

		public UserDataStore<ulong, ulong, UserFlag> UserFlags;
		public UserDataStore<string, ulong, CommandState> CommandStateData;
		public CommandStore Commands;

		public async Task Exit() => await _client.Disconnect();

		public void SetGame(string game) => _client.SetGame(game);

		public async Task Initialize()
		{
			Utilities.WriteLog($"XenoBot2 v{Utilities.GetVersion()} starting initialization...");
			// Initialize command storage
			Commands = new CommandStore();
			// load default commands
			Commands.AddMany(DefaultCommands.Content);
			CommandStateData = new UserDataStore<string, ulong, CommandState>(CommandState.None, 0);
			UserFlags = new UserDataStore<ulong, ulong, UserFlag>(UserFlag.User, 0);

			UserFlags.Add(174018252161286144, UserFlags.GlobalValue, UserFlag.BotAdministrator | UserFlag.BotDebug | UserFlag.Administrator | UserFlag.Moderator);

			Utilities.WriteLog("Loading API key from disk...");

			if (!File.Exists("apikey.txt"))
			{
				Utilities.WriteLog("Error: API key not found.\n" +
				                   "Please create a file named 'apikey.txt' in the same directory as the binary, " +
				                   "and put your Discord Bot API key into the file.\n" +
				                   "Press Enter to exit.");
				Console.ReadLine();
				throw new FileNotFoundException();
			}
			using (var keyfile = File.OpenText("apikey.txt"))
			{
				Utilities.WriteLog("Found API key.");
				_key = await keyfile.ReadToEndAsync();
			}


			Utilities.WriteLog("Setting up WhatTheCommit...");

			// TODO: Cache wtc info instead of re-downloading each time
			var wtc = await Shared.Utilities.GetStringAsync("https://www.lohikar.io/assets/xenobot/commits.txt");
			Strings.WhatTheCommit = wtc.Split('\n');

			var wtcNames = await Shared.Utilities.GetStringAsync("https://www.lohikar.io/assets/xenobot/names.txt");
			Strings.Names = wtcNames.Split('\n');

			// initialize client
			_client = new DiscordClient();

			_client.MessageReceived += async (s, e) =>
			{
				if (!e.Message.IsAuthor)
					await ParseMessage(e.User, e.Channel, e.Message.RawText);
			};

			_client.Ready += (s, e) =>
			{
				_client.SetGame($"v{Utilities.GetVersion()}");
			};

			Utilities.WriteLog("Done.");
		}

		/// <summary>
		///		Connects to discord. This call blocks until the client terminates.
		/// </summary>
		public void Connect()
		{
			_client.ExecuteAndWait(async () =>
			{
				await _client.Connect(_key, TokenType.Bot);
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
			if (cmd.State.HasFlag(CommandState.DoesNotExist) || cmd.Cmd == null) return;
			if ((cmd.Cmd.Flags.HasFlag(CommandFlag.NoPrivateChannel) && channel.IsPrivate) || cmd.Cmd.Flags.HasFlag(CommandFlag.NoPublicChannel) && !channel.IsPrivate)
			{
				await channel.SendMessage("That command cannot be used here.");
				return;
			}
			if (!cmd.Cmd.Flags.HasFlag(CommandFlag.UsableWhileIgnored) && Utilities.Permitted(UserFlag.Ignored, author))
				return;
			try
			{
				// execute command
				await cmd.Cmd.Definition(cmd, author, channel);
			}
			catch (InvalidCommandException ex)
			{
				Utilities.WriteLog($"Attempted use of invalid command '{ex.AttemptedCommand}'.");
				await channel.SendMessage("That command is not understood.");
			}
			catch (Exception ex)
			{
				Utilities.WriteLog($"An exception occurred during execution of command '{cmd.CommandText}'," +
				                   $" args '{string.Join(", ", cmd.Arguments)}'.\n" + ex.Message);
				await channel.SendMessage($"An internal error occurred running command '{cmd.CommandText}'.");
			}
		}
	}
}