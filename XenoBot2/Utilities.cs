﻿using System;
using System.Reflection;
using Discord;
using XenoBot2.Shared;

namespace XenoBot2
{
	/// <summary>
	///     Miscellaneous utility functions.
	/// </summary>
	public static class Utilities
	{
		/// <summary>
		///     Checks if a <see cref="Command" /> has an Alias, and resolves it to a <see cref="Command" />.
		/// </summary>
		/// <param name="command">The command to resolve the alias for.</param>
		/// <returns>A resolved alias or the passed <see cref="Command" /> if there was no defined alias.</returns>
		internal static Command ResolveCommand(this Command command)
			=> command.AliasFor == null ? command : Program.BotInstance.Commands[command.AliasFor];

		internal static bool Permitted(UserFlag flag, User member, Channel channel = null)
			=> Permitted(flag, member.Id, channel);

		internal static bool Permitted(UserFlag flag, ulong member, Channel channel = null)
		{
			var data = Program.BotInstance.Manager.GetFlag(member, channel);

			return data.HasFlag(flag);
		}

		internal static bool HasState(CommandState flag, string command, ulong? channel = null)
		{
			var channelData = channel != null ? Program.BotInstance.CommandStateData[command, channel.Value] : CommandState.None;
			var globalData = Program.BotInstance.CommandStateData[command];

			return (channelData | globalData).HasFlag(flag);
		}

		/// <summary>
		///     Writes an entry to the log with a date stamp.
		/// </summary>
		/// <param name="data">The message to write to the log.</param>
		internal static void WriteLog(string data)
		{
			var message = $"[{DateTime.Now}] {data}";
			NonBlockingConsole.WriteLine(message);
		}

		/// <summary>
		///     Writes an entry to the log with a date stamp & event origin user.
		/// </summary>
		/// <param name="originClient">The <see cref="User" /> that triggered this event.</param>
		/// <param name="data">The message to write to the log.</param>
		internal static void WriteLog(User originClient, string data)
		{
			var desc = Permitted(UserFlag.BotAdministrate, originClient) ? "Admin" : "Client";
			WriteLog($"{desc} {originClient.GetFullUsername()} {data}");
		}

		public static Version GetVersion() => Assembly.GetExecutingAssembly().GetName().Version;

		/// <summary>
		///     Toggles the ignore state for a user, globally or per-channel.
		/// </summary>
		/// <param name="user">The user to toggle ignore for.</param>
		/// <param name="context">The channel to ignore on. Omit for global.</param>
		/// <returns>True if user is now ignored, false if not.</returns>
		public static bool ToggleIgnore(User user, Channel context)
		{
			var ignoreState = Program.BotInstance.Manager[context.Server.Id].GetFlag(user.Id).HasFlag(UserFlag.Ignored);
			if (ignoreState)
			{
				Program.BotInstance.Manager[context.Server.Id].RemoveFlag(user, UserFlag.Ignored);
			}
			else
			{
				Program.BotInstance.Manager[context.Server.Id].AddFlag(user, UserFlag.Ignored);
			}
			return ignoreState;
		}

		/// <summary>
		///     Toggles the ignore state for a user, globally or per-channel.
		/// </summary>
		/// <param name="user">The user to toggle ignore for.</param>
		/// <param name="context">The channel to ignore on. Omit for global.</param>
		/// <returns>True if user is now ignored, false if not.</returns>
		public static bool ToggleIgnoreGlobal(User user)
		{
			var ignoreState = Program.BotInstance.Manager.GetFlag(user, null).HasFlag(UserFlag.Ignored);
			if (ignoreState)
			{
				Program.BotInstance.Manager.RemoveGlobalFlag(user, UserFlag.Ignored);
			}
			else
			{
				Program.BotInstance.Manager.AddGlobalFlag(user, UserFlag.Ignored);
			}
			return ignoreState;
		}
	}
}
