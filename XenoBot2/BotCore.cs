using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Discord;
using XenoBot2.Data;
using XenoBot2.Shared;

namespace XenoBot2
{
	internal class BotCore
	{
		private DiscordClient _client;

		private string _key;
		private readonly string _configPath = Path.Combine("Data", "config.xml");

		public char Prefix { get; private set; }

		public ServerManager Manager;
		public TwoKeyDictionary<string, ulong, CommandState> CommandStateData;
		public CommandStore Commands;

		public async Task Exit() => await _client.Disconnect();

		public void SetGame(string game) => _client.SetGame(game);

		public async Task Initialize()
		{
			Utilities.WriteLog($"XenoBot2 v{Utilities.GetVersion()} starting initialization...");

			// Initialize command storage, needs to be done before config load
			Commands = new CommandStore();
			// load default commands
			Commands.AddMany(DefaultCommands.Content);
			CommandStateData = new TwoKeyDictionary<string, ulong, CommandState>(CommandState.None, 0);
			Manager = new ServerManager();

			var timer = new Stopwatch();
			
			Utilities.WriteLog("Looking for config...");
			timer.Start();

			await LoadConfig();
			
			timer.Stop();

			Utilities.WriteLog($"Config load took {timer.ElapsedMilliseconds} ms.");

			if (!Directory.Exists("Data"))
				Directory.CreateDirectory("Data");

			Utilities.WriteLog("Setting up command data...");
			timer.Restart();
			Strings.WhatTheCommit = (await Utilities.GetAsset("https://www.lohikar.io/assets/xenobot/commits.txt", "wtc.txt")).Split('\n');
			Strings.Names = (await Utilities.GetAsset("https://www.lohikar.io/assets/xenobot/names.txt", "wtc_names.txt")).Split('\n');
			timer.Stop();
			Utilities.WriteLog($"Done setting up command data; took {timer.ElapsedMilliseconds} ms.");

			// initialize client
			_client = new DiscordClient();
			_client.MessageReceived += MessageReceived;
			_client.Ready += OnReady;

			Utilities.WriteLog("Done initialization.");
		}

		private async Task LoadConfig()
		{
			var serializer = new XmlSerializer(typeof(Config));
			if (!File.Exists(_configPath))
			{
				Utilities.WriteLog("Config not found, creating.");
				using (var fs = new FileStream(_configPath, FileMode.CreateNew))
				{
					serializer.Serialize(fs, new Config());
				}

				Utilities.WriteLog(
					$"Please fill in the config file at \"{Path.Combine(Directory.GetCurrentDirectory(), _configPath)}\" and restart the bot.");
				throw new FileNotFoundException();
			}

			Utilities.WriteLog("Found config, loading.");
			using (var fs = File.OpenRead(_configPath))
			{
				var conf = serializer.Deserialize(fs) as Config;
				if (conf?.ConfigVersion != 1)
				{
					Utilities.WriteLog("WARNING: Unknown config version!");
				}
				Prefix = conf?.CommandPrefix ?? '$';
				Utilities.WriteLog($"Command prefix set to '{Prefix}'.");
				_key = conf?.ApiToken;
				if (string.IsNullOrWhiteSpace(_key))
				{
					Utilities.WriteLog("WARNING: No API Key in config! Attempting legacy load.");
					var path = Path.Combine("Data", "apikey.txt");
					await LoadApiKey(path);
				}

				foreach (var user in conf?.BotAdmins ?? new ulong[0])
				{
					Manager.AddGlobalFlag(user, UserFlag.BotAdministrate);
				}

				foreach (var user in conf?.BotDebuggers ?? new ulong[0])
				{
					Manager.AddGlobalFlag(user, UserFlag.Debug);
				}
			}
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
		///     Connects to discord. This call blocks until the client terminates.
		/// </summary>
		public void Connect()
		{
			if (_client == null) throw new InvalidOperationException("Object not initialized!");
			_client.ExecuteAndWait(async () =>
			{
				Utilities.WriteLog("Connecting to discord...");
				await _client.Connect(_key, TokenType.Bot);
			});
		}

		#region Event Handlers

		private static async void MessageReceived(object sender, MessageEventArgs e)
		{
			if (!e.Message.IsAuthor && !e.Message.User.IsBot)
				await CommandParser.ParseMessage(e.Message, e.Channel.IsPrivate);
		}

		private void OnReady(object sender, EventArgs e)
		{
			_client.SetGame($"v{Utilities.GetVersion()}-{Program.BuildTypeShort}");
			Utilities.WriteLog($"Logged into Discord as {_client.CurrentUser.GetFullUsername()}.");
			Console.Title = $"{_client.CurrentUser.GetFullUsername()} - XenoBot2 v{Utilities.GetVersion()} {Program.BuildType}";
		}

		#endregion
	}
}
