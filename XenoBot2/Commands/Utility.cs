using System;
using System.Globalization;
using System.Linq;
using DiscordSharp;
using DiscordSharp.Objects;
using Humanizer;
using XenoBot2.Data;
using XenoBot2.Shared;

namespace XenoBot2.Commands
{
	internal static class Utility
	{
		internal static void Echo(DiscordClient client, CommandInfo info, DiscordMember author, DiscordChannelBase channel)
		{
			Utilities.WriteLog(author, "requested an echo.");
			client.SendMessageToRoom($"{author.Username} said: {string.Join(" ", info.Arguments)}", channel);
		}

		internal static void Time(DiscordClient client, CommandInfo info, DiscordMember author, DiscordChannelBase channel)
		{
			Utilities.WriteLog(author, "requested the time.");
			client.SendMessageToRoom($"The time is {DateTime.UtcNow.ToLongTimeString()} (UTC).", channel);
		}

		internal static void Date(DiscordClient client, CommandInfo info, DiscordMember author, DiscordChannelBase channel)
		{
			Utilities.WriteLog(author, "requested the date.");
			client.SendMessageToRoom($"It is {DateTime.UtcNow.ToLongDateString()} (UTC).", channel);
		}

		internal static void Me(DiscordClient client, CommandInfo info, DiscordMember origin, DiscordChannelBase channel)
		{
			Utilities.WriteLog(origin, "requested info about themselves.");
			client.SendMessageToRoom(GetInfoString(origin, channel), channel);
		}

		private static string GetInfoString(DiscordMember author, DiscordChannelBase channel)
			=> $"Information about: **{author.GetFullUsername()}**\n" +
			   "```\n" +
			   $"ID:                   {author.ID}\n" +
			   $"Bot:                  {author.IsBot}\n" +
			   $"Avatar:               {author.GetAvatarURL()}\n" +
			   $"User Flags (Channel): {SharedData.UserFlags[author.ID, channel.ID]}\n" +
			   $"User Flags (Global):  {SharedData.UserFlags[author.ID]}" +
			   "```";

		internal static void ConvertNumber(DiscordClient client, CommandInfo info, DiscordMember author, DiscordChannelBase channel)
		{
			if (info.Arguments.LessThan(2))
			{
				client.SendMessageToRoom(Messages.CommandNotEnoughArguments, channel);
				return;
			}
			int number;
			if (!int.TryParse(info.Arguments[1], out number))
			{
				client.SendMessageToRoom($"{info.Arguments[1]} is not a valid integer.", channel);
				return;
			}
			CultureInfo culture = null;
			if (info.Arguments.AtLeast(3))
			{
				try
				{
					culture = new CultureInfo(info.Arguments[2]);
				}
				catch (CultureNotFoundException)
				{
					client.SendMessageToRoom("Unknown culture.", channel);
					return;
				}
			}
			try
			{
				switch (info.Arguments[0])
				{
					case "roman":
						client.SendMessageToRoom($"{number} in roman numerals is {number.ToRoman()}.", channel);
						break;

					case "words":
						client.SendMessageToRoom($"{number} in words is {number.ToWords(culture)}.", channel);
						break;

					case "metric":
						client.SendMessageToRoom($"{number} in metric is {number.ToMetric()}.", channel);
						break;

					case "wordord":
						client.SendMessageToRoom($"{number} is {number.ToOrdinalWords(culture)}.", channel);
						break;

					default:
						client.SendMessageToRoom($"Unknown conversion type *{info.Arguments[0]}*.", channel);
						break;
				}
			}
			catch (ArgumentOutOfRangeException)
			{
				client.SendMessageToRoom("Error: number out of range.", channel);
			}
			catch
			{
				client.SendMessageToRoom("This shouldn't happen; something broke.", channel);
			}
		}

		internal static void UserInfo(DiscordClient client, CommandInfo info, DiscordMember member, DiscordChannelBase channel)
		{
			var author = !info.HasArguments 
				? member 
				: info.Arguments.First().GetMemberFromMention(client, channel);
			if (author == null)
			{
				client.SendMessageToRoom("User does not exist.", channel);
				return;
			}
			client.SendMessageToRoom(GetInfoString(author, channel), channel);
		}

		internal static void Avatar(DiscordClient client, CommandInfo info, DiscordMember member, DiscordChannelBase channel)
		{
			var author = !info.HasArguments
				? member
				: info.Arguments[0].GetMemberFromMention(client, channel);
			if (author == null)
			{
				client.SendMessageToRoom("User does not exist.", channel);
				return;
			}

			client.SendMessageToRoom(
				string.IsNullOrWhiteSpace(author.Avatar)
					? "https://www.lohikar.io/i/xb/avatar_missing.jpg"
					: author.GetAvatarURL().ToString(), channel);
		}

		internal static void Ping(DiscordClient client, CommandInfo info, DiscordMember member, DiscordChannelBase channel)
		{
			Utilities.WriteLog(member, "requested a ping.");
			client.SendMessageToRoom(info.CommandText == "ping" ? "pong" : $"{info.CommandText} response", channel);
		}
	}
}
