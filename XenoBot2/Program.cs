using System;
using System.IO;
using System.Threading;
using DiscordSharp;
using DiscordSharp.Events;
using DiscordSharp.Objects;
using Humanizer;
using XenoBot2.Data;
using XenoBot2.Shared;

namespace XenoBot2
{
	internal static class Program
	{
		private static DiscordClient _client;
		internal static CombinedChannelCommandManager CombinedChannelCommandMgr;
		private static bool _exit;
		private static Thread _clientThread;

		internal static void Exit() => _exit = true;

		private static void Main(string[] args)
		{
			Utilities.WriteLog($"XenoBot2 v{Utilities.GetVersion()} starting initialization...");
			// Initialize command storage
			CommandStore.Commands = new CommandStore();
			// load default commands
			CommandStore.Commands.AddMany(DefaultCommands.Content);

			// initialize CombinedChannelCommandManager
			CombinedChannelCommandMgr = new CombinedChannelCommandManager();
			CombinedChannelCommandMgr.ChannelRegistered += e =>
			{
				Utilities.WriteLog($"Channel '{e.GetName()}' registered with CommandManager.");
			};
			CombinedChannelCommandMgr.CommandRegistered +=
				(command, channel) =>
				{
					Utilities.WriteLog($"Command '{command}' registered on channel '{channel.GetName()}'");
				};

			Utilities.WriteLog("Loaded CombinedChannelCommandManager...");

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
			_client = new DiscordClient(isBotAccount: true, tokenOverride: apifile.ReadToEnd());

			Utilities.WriteLog("Started client.");

			// register events
			_client.Connected += (sender, e) =>
			{
				Utilities.WriteLog($"Logged in to Discord as {e.User.GetFullUsername()}");
				Console.Title = $"{e.User.GetFullUsername()} - XenoBot";
				_client.UpdateCurrentGame("Hiss");
			};
			_client.MessageReceived += ParseMessageEventWrapper;
			_client.PrivateMessageReceived += ParsePrivateMessage;
			_client.MentionReceived += ClientMentioned;
			//_client.UserAddedToServer += UserJoin;
			//_client.UserRemovedFromServer += UserLeave;
			_client.SocketClosed += (sender, e) =>
			{
				// something went wrong and the socket got closed, attempt reconnect
				// TODO: More intelligent reconnection
				NonBlockingConsole.WriteLine("ERROR: !! DISCONNECTED FROM DISCORD !!");
				NonBlockingConsole.WriteLine("Waiting 10 seconds and reconnecting.");
				Thread.Sleep(10.Seconds());
				Connect(out _clientThread, _client);
			};
			
			Connect(out _clientThread, _client);

			Utilities.WriteLog("Setting up WhatTheCommit...");

			// TODO: Cache wtc info instead of re-downloading each time
			var wtc =
				Utilities.GetStringFromWebService("https://www.lohikar.io/assets/xenobot/commits.txt");
			Strings.WhatTheCommit = wtc.Split('\n');

			Strings.Names = Utilities.GetStringFromWebService("https://www.lohikar.io/assets/xenobot/names.txt").Split('\n');

			Utilities.WriteLog("Done.");

			// Sleep main thread until time to exit
			while (true)
			{
				Thread.Sleep(1.Seconds());
				if (_exit)
					break;
			}
		}

		private static void Connect(out Thread clientThread, DiscordClient client)
		{
			client.SendLoginRequest();

			clientThread = new Thread(client.Connect);

			clientThread.Start();
		}

		private static void UserLeave(object sender, DiscordGuildMemberRemovedEventArgs e)
		{
			Utilities.WriteLog(e.MemberRemoved, $"has left {e.Server.Name}");
			_client.SendMessageToUser($"**{e.MemberRemoved.Username}** has left {e.Server.Name}.", e.Server.Owner);
		}

		private static void UserJoin(object sender, DiscordGuildMemberAddEventArgs e)
		{
			Utilities.WriteLog(e.AddedMember, $"has joined {e.Guild.Name}");
			_client.SendMessageToUser($"**{e.AddedMember.Username}** has joined {e.Guild.Name}.", e.Guild.Owner);
		}

		private static void ClientMentioned(object sender, DiscordMessageEventArgs e)
		{
			if (CheckMemberIgnore(e.Author)) return;
			Utilities.WriteLog(e.Author, Messages.MentionedMe);
			_client.SendMessageToChannel(Messages.XenomorphHiss, e.Channel);
		}

		private static void ParsePrivateMessage(object sender, DiscordPrivateMessageEventArgs e) =>
			ParseMessage(e.Author, e.Channel, e.Message.Trim());
		

		private static void ParseMessageEventWrapper(object sender, DiscordMessageEventArgs e) =>
			ParseMessage(e.Author, e.Channel, e.MessageText);
		

		private static void ParseMessage(DiscordMember author, DiscordChannelBase channel, string messageText)
		{
			// silently ignore our own messages & messages from other bots
			if (CheckMemberIgnore(author)) return;

			if (string.IsNullOrWhiteSpace(messageText)) return;

			var cmdinfo = GetInfo(messageText, channel, author);
			if (cmdinfo?.CommandText == null) return;	// command not found

			// Process command & execute
			var cmd = CommandStore.Commands[cmdinfo.CommandText].ResolveCommand();
			if (cmd?.Definition == null)
			{
				Utilities.WriteLog(string.Format(Messages.CommandNotDefined, cmdinfo.CommandText));
				SendMessageToRoom($"The command '{cmdinfo.CommandText}' seems to be broken right now.", channel);
				return;
			}
			if ((cmd.Flags.HasFlag(CommandFlag.NoPrivateChannel) && channel.Private) || 
				cmd.Flags.HasFlag(CommandFlag.NoPublicChannel) && !channel.Private)
			{
				SendMessageToRoom("That command cannot be used here.", channel);
				return;
			}
			try
			{
				// execute command
				cmd.Definition(_client, cmdinfo, author, channel);
			}
			catch (InvalidCommandException ex)
			{
				Utilities.WriteLog($"Attempted use of invalid command '{ex.AttemptedCommand}'.");
				SendMessageToRoom("That command is not understood.", channel);
			}
			catch (Exception ex)
			{
				Utilities.WriteLog($"An exception occurred during execution of command '{cmdinfo.CommandText}'," +
				                   $" args '{string.Join(", ", cmdinfo.Arguments)}'.\n" + ex.Message);
				SendMessageToRoom($"An internal error occurred running command '{cmdinfo.CommandText}'.", channel);
			}
		}

		private static bool CheckMemberIgnore(DiscordMember member) => member == _client.Me || member.IsBot || Ids.Ignored.Contains(member.ID);

		private static void SendMessageToRoom(string data, DiscordChannelBase channel) => _client.SendMessageToRoom(data, channel);

		private static CommandInfo GetInfo(string messageText, DiscordChannelBase channel, DiscordMember originMember)
		{
			var cmd = CombinedChannelCommandMgr.ParseCommand(messageText.Trim(), channel);

			switch (cmd.Status)
			{
				case CommandStatus.RateLimited:
					Utilities.WriteLog(originMember, $"tried to use rate-limited command '{cmd.CommandText}'");
					return null;

				case CommandStatus.Disabled:
					Utilities.WriteLog(originMember, $"tried to use disabled command '{cmd.CommandText}'");
					return null;
				case CommandStatus.AdminDisabled:
					Utilities.WriteLog(originMember, $"tried to use admin-disabled command '{cmd.CommandText}'");
					return null;

				default:
					return cmd;
			}
		}
	}
}
