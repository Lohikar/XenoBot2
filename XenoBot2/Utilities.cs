using System;
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
			=> command.AliasFor == null ? command : CommandStore.Commands[command.AliasFor];

		internal static bool Permitted(UserFlag flag, User member, Channel channel = null)
			=> Permitted(flag, member.Id, channel?.Id);

		internal static bool Permitted(UserFlag flag, ulong member, ulong? channel = null)
		{
			var channelData = channel != null ? SharedData.UserFlags[member, channel.Value] : UserFlag.None;
			var globalData = SharedData.UserFlags[member];

			return ((channelData | globalData) & flag) == flag;
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

		/////  <summary>
		///// 		Sends a message to the specified channel.
		/////  </summary>
		///// <param name="client"></param>
		///// <param name="message">The message to send.</param>
		/////  <param name="channel">The channel to send the message to.</param>
		//internal static void SendMessageToRoom(this DiscordClient client, string message, Channel channel)
		//{
		//	if (string.IsNullOrWhiteSpace(message))
		//	{
		//		WriteLog($"WARNING: Empty message addressed to '{channel.Name}' dropped.");
		//		return;
		//	}
		//	client.SendMessageToRoom(message, channel);
		//}

		/// <summary>
		///     Writes an entry to the log with a date stamp & event origin user.
		/// </summary>
		/// <param name="originClient">The <see cref="DiscordMember" /> that triggered this event.</param>
		/// <param name="data">The message to write to the log.</param>
		internal static void WriteLog(User originClient, string data)
			=> WriteLog($"Client {originClient.GetFullUsername()} {data}");

		public static Version GetVersion() => Assembly.GetExecutingAssembly().GetName().Version;
	}
}
