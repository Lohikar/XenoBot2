using System;
using System.Diagnostics;
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
			var timer = new Stopwatch();

			// Initialize command storage
			InitializeStorage();

			if (!Directory.Exists("Data"))
				Directory.CreateDirectory("Data");

			Utilities.WriteLog("Loading API key from disk...");
			var path = Path.Combine("Data", "apikey.txt");

			timer.Start();
			await LoadApiKey(path);
			timer.Stop();

			Utilities.WriteLog($"Done loading API key; took {timer.ElapsedMilliseconds} ms.");
			timer.Reset();

			Utilities.WriteLog("Setting up WhatTheCommit...");
			timer.Start();
			Strings.WhatTheCommit = (await GetAsset("https://www.lohikar.io/assets/xenobot/commits.txt", "wtc.txt")).Split('\n');
			Strings.Names = (await GetAsset("https://www.lohikar.io/assets/xenobot/names.txt", "wtc_names.txt")).Split('\n');
			timer.Stop();
			Utilities.WriteLog($"Done setting up WhatTheCommit; took {timer.ElapsedMilliseconds} ms.");

			// initialize client
			_client = new DiscordClient();
			_client.MessageReceived += MessageReceived;
			_client.Ready += (s, e) => _client.SetGame($"v{Utilities.GetVersion()}");

			Utilities.WriteLog("Done.");
		}

		private static async void MessageReceived(object sender, MessageEventArgs e)
		{
			if (!e.Message.IsAuthor)
				await ParseMessage(e.User, e.Channel, e.Message.RawText);
		}

		private void InitializeStorage()
		{
			Commands = new CommandStore();
			// load default commands
			Commands.AddMany(DefaultCommands.Content);
			CommandStateData = new UserDataStore<string, ulong, CommandState>(CommandState.None, 0);
			UserFlags = new UserDataStore<ulong, ulong, UserFlag>(UserFlag.User, 0);

			UserFlags.Add(174018252161286144, UserFlags.GlobalValue, UserFlag.BotAdministrator | UserFlag.BotDebug | UserFlag.Administrator | UserFlag.Moderator);
		}

		private async Task LoadApiKey(string path)
		{
			if (!File.Exists(path))
			{
				Utilities.WriteLog("Error: API key not found.\n" +
								   "Please create a file named 'apikey.txt' in the \"Data\" directory, " +
								   "and put your Discord Bot API key into the file.\n" +
								   "Press Enter to exit.");
				Console.ReadLine();
				throw new FileNotFoundException();
			}
			using (var keyfile = File.OpenText(path))
			{
				Utilities.WriteLog("Found API key.");
				_key = await keyfile.ReadToEndAsync();
			}
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
			if (string.IsNullOrWhiteSpace(messageText) || (messageText[0] != Prefix && !skipPrefix)) return;

			var text = skipPrefix ? messageText : messageText.Substring(1);

			// Process command & execute
			var cmd = CommandParser.ParseCommand(text, channel);
			if (CommandInvalid(cmd, author)) return;
			if ((cmd.BoundCommand.Flags.HasFlag(CommandFlag.NoPrivateChannel) && channel.IsPrivate) || cmd.BoundCommand.Flags.HasFlag(CommandFlag.NoPublicChannel) && !channel.IsPrivate)
			{
				await channel.SendMessage("That command cannot be used here.");
				return;
			}
			try
			{
				// execute command
				await cmd.BoundCommand.Definition(cmd, author, channel);
			}
			catch (Exception ex)
			{
				Utilities.WriteLog($"An exception occurred during execution of command '{cmd.CommandText}'," +
				                   $" args '{string.Join(", ", cmd.Arguments)}'.\n" + ex.Message);
				await channel.SendMessage($"An internal error occurred running command '{cmd.CommandText}'.");
			}
		}

		private static bool CommandInvalid(CommandInfo cmd, User author) =>
			 cmd.State.HasFlag(CommandState.DoesNotExist) || cmd.BoundCommand == null || !cmd.BoundCommand.Flags.HasFlag(CommandFlag.UsableWhileIgnored) && Utilities.Permitted(UserFlag.Ignored, author);

		private static async Task<string> GetAsset(string url, string cacheFileName)
		{
			var path = Path.Combine("Data", cacheFileName);

			if (File.Exists(path))
				using (var reader = File.OpenText(path))
					return await reader.ReadToEndAsync();

			var asset = await Shared.Utilities.GetStringAsync(url);
			using (var writer = new FileStream(path, FileMode.CreateNew))
			using (var text = new StreamWriter(writer))
				await text.WriteAsync(asset);
			
			return asset;
		}
	}
}