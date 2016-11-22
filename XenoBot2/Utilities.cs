using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using DiscordSharp;
using DiscordSharp.Objects;
using XenoBot2.Shared;

namespace XenoBot2
{
	/// <summary>
	/// Miscellaneous utility functions.
	/// </summary>
	public static class Utilities
	{
		private static Random _random;
		/// <summary>
		///     Downloads a text file from a web service and returns the contents as a string.
		/// </summary>
		/// <param name="url">The URL of the file to download.</param>
		/// <returns></returns>
		public static string GetStringFromWebService(string url)
		{
			using (var webclient = new WebClient())
			{
				var data = webclient.DownloadData(url);
				return Encoding.ASCII.GetString(data);
			}
		}

		/// <summary>
		///     Splits a string into an array of strings.
		/// </summary>
		/// <param name="source">The string to split.</param>
		/// <param name="predictedSplitLen">How large each string probably will be. Specify to theoretically improve performance.</param>
		/// <param name="splitValues">Array of chars to split on.</param>
		/// <returns></returns>
		public static IEnumerable<string> LazySplit(this string source, int predictedSplitLen, params char[] splitValues)
		{
			var builder = predictedSplitLen <= 0
				? new StringBuilder()
				: new StringBuilder(predictedSplitLen);
			foreach (var c in source)
			{
				if (splitValues.Contains(c))
				{
					// split off string
					var result = builder.ToString();
					builder.Clear();
					yield return result;
				}

				builder.Append(c);
			}
		}

		/// <summary>
		///     Repeats a string.
		/// </summary>
		/// <param name="source">The string to repeat.</param>
		/// <param name="times">How many times to repeat the string. Must be greater than 0.</param>
		/// <returns></returns>
		public static string Repeat(this string source, int times)
		{
			if (times <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(times));
			}
			if (source == null)
				throw new ArgumentNullException(nameof(source));
			if (times == 1)
				return source;
			var builder = new StringBuilder(source.Length*times);
			for (var i = 0; i < times; i++)
			{
				builder.Append(source);
			}
			return builder.ToString();
		}

		public static Version GetVersion() => Assembly.GetExecutingAssembly().GetName().Version;

		/// <summary>
		///		Checks if a <see cref="Command"/> has an Alias, and resolves it to a <see cref="Command"/>.
		/// </summary>
		/// <param name="command">The command to resolve the alias for.</param>
		/// <returns>A resolved alias or the passed <see cref="Command"/> if there was no defined alias.</returns>
		internal static Command ResolveCommand(this Command command)
			=> command.AliasFor == null ? command : CommandStore.Commands[command.AliasFor];

		internal static string MakeMention(this DiscordMember target) => $"<@{target.ID}>";

		internal static string GetFullUsername(this DiscordMember target) => $"{target.Username}#{target.Discriminator}";

		/// <summary>
		/// Returns a random string from a list of strings.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		internal static string GetRandom(this IReadOnlyList<string> source)
		{
			if (_random == null)
			{
				_random = new Random();
			}
			return source.Any() ? source?[_random.Next(source.Count)] : "";
		}

		/// <summary>
		/// Returns a random string from a list of strings.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		internal static string GetRandom(this IList<string> source)
		{
			if (_random == null)
			{
				_random = new Random();
			}
			return source?.Any() ?? false ? source[_random.Next(source.Count)] : "";
		}

		/// <summary>
		/// Returns true if source has at least num elements.
		/// </summary>
		public static bool AtLeast<T>(this ICollection<T> source, int num) => source.Count >= num;

		/// <summary>
		/// Returns true if source has less than num elements.
		/// </summary>
		public static bool LessThan<T>(this ICollection<T> source, int num) => source.Count < num;

		public static string GetName(this DiscordChannelBase channel)
		{
			if (!channel.Private) return ((DiscordChannel)channel).Name;
			var e = (DiscordPrivateChannel)channel;
			return $"PM::{e.Recipient.Username}#{e.Recipient.Discriminator}";
		}

		///  <summary>
		/// 		Sends a message to the specified channel.
		///  </summary>
		/// <param name="client"></param>
		/// <param name="message">The message to send.</param>
		///  <param name="channel">The channel to send the message to.</param>
		internal static void SendMessageToRoom(this DiscordClient client, string message, DiscordChannelBase channel)
		{
			if (string.IsNullOrWhiteSpace(message))
			{
				WriteLog($"WARNING: Empty message addressed to '{channel.GetName()}' dropped.");
				return;
			}
			if (channel.Private)
				client.SendMessageToUser(message, ((DiscordPrivateChannel)channel).Recipient);
			else
				client.SendMessageToChannel(message, (DiscordChannel)channel);
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
		/// <param name="originClient">The <see cref="DiscordMember" /> that triggered this event.</param>
		/// <param name="data">The message to write to the log.</param>
		internal static void WriteLog(DiscordMember originClient, string data)
			=> WriteLog($"Client {originClient.GetFullUsername()} {data}");

		internal static DiscordMember GetMemberFromMention(this string src, DiscordClient client, DiscordChannelBase channel)
			=> client.GetMemberFromChannel(channel, src.Replace("<@!", "").Replace("<@", "").Replace(">", ""));

		internal static bool Permitted(UserFlag flag, DiscordMember member, DiscordChannelBase channel = null)
			=> (SharedData.UserFlags[member.ID, channel?.ID ?? "*"] & flag) == flag;
	}
}