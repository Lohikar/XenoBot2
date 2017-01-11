using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Humanizer;
using XenoBot2.Data;
using XenoBot2.Shared;

namespace XenoBot2.Commands
{
	internal static class Utility
	{
		internal static async Task Echo(CommandInfo info, Message msg)
		{
			Utilities.WriteLog(msg.User, "requested an echo.");
			await msg.Channel.SendMessage($"{msg.Channel.Name} said: {string.Join(" ", info.Arguments)}");
		}

		internal static async Task Time(CommandInfo info, Message msg)
		{
			Utilities.WriteLog(msg.User, "requested the time.");
			await msg.Channel.SendMessage($"The time is {DateTime.UtcNow.ToLongTimeString()} (UTC).");
		}

		internal static async Task Date(CommandInfo info, Message msg)
		{
			Utilities.WriteLog(msg.User, "requested the date.");
			await msg.Channel.SendMessage($"It is {DateTime.UtcNow.ToLongDateString()} (UTC).");
		}

		internal static async Task Me(CommandInfo info, Message msg)
		{
			Utilities.WriteLog(msg.User, "requested info about themselves.");
			await msg.Channel.SendMessage(GetInfoString(msg.User, msg.Channel));
		}

		private static string GetInfoString(User author, Channel channel)
			=> $"Information about: **{author.GetFullUsername()}**\n" +
			   "```\n" +
			   $"ID:				{author.Id}\n" +
			   $"Bot:				{author.IsBot}\n" +
			   $"Avatar:			{author.AvatarUrl}\n" +
			   $"Roles:				{string.Join(", ", author.Roles.Where(item => !item.IsEveryone).Select(item => item.Name))}\n" +
			   $"JoinedAt:			{author.JoinedAt}\n" +
			   $"LastActivity:		{author.LastActivityAt}\n" +
			   $"LastOnline:		{author.LastOnlineAt}\n" +
			   $"User Flags (Ag):	{Program.BotInstance.Manager.GetFlag(author, channel)}\n" +
			   $"User Flags (G):	{Program.BotInstance.Manager.GetFlag(author, null)}\n" + 
			   "```";

		internal static async Task ConvertNumber(CommandInfo info, Message msg)
		{
			if (info.Arguments.LessThan(2))
			{
				await msg.Channel.SendMessage(Messages.CommandNotEnoughArguments);
				return;
			}
			int number;
			if (!int.TryParse(info.Arguments[1], out number))
			{
				await msg.Channel.SendMessage($"{info.Arguments[1]} is not a valid integer.");
				return;
			}
			try
			{
				switch (info.Arguments[0])
				{
					case "roman":
						await msg.Channel.SendMessage($"{number} in roman numerals is {number.ToRoman()}.");
						break;

					case "words":
						await msg.Channel.SendMessage($"{number} in words is {number.ToWords()}.");
						break;

					case "metric":
						await msg.Channel.SendMessage($"{number} in metric is {number.ToMetric()}.");
						break;

					case "wordord":
						await msg.Channel.SendMessage($"{number} is {number.ToOrdinalWords()}.");
						break;

					default:
						await msg.Channel.SendMessage($"Unknown conversion type *{info.Arguments[0]}*.");
						break;
				}
			}
			catch (ArgumentOutOfRangeException)
			{
				await msg.Channel.SendMessage("Error: number out of range.");
			}
		}

		internal static async Task UserInfo(CommandInfo info, Message msg)
		{
			var author = !info.HasArguments 
				? msg.User
				: info.Arguments.First().GetMemberFromMention(msg.Channel);
			if (author == null)
			{
				Utilities.WriteLog(msg.User, "requested information on a user, but the user did not exist.");
				await msg.Channel.SendMessage("User does not exist.");
				return;
			}
			Utilities.WriteLog(msg.User, $"requested information on user {author.GetFullUsername()}.");
			await msg.Channel.SendMessage(GetInfoString(author, msg.Channel));
		}

		internal static async Task Avatar(CommandInfo info, Message msg)
		{
			var author = !info.HasArguments
				? msg.User
				: info.Arguments[0].GetMemberFromMention(msg.Channel);
			if (author == null)
			{
				await msg.Channel.SendMessage("User does not exist.");
				Utilities.WriteLog(msg.User, "requested  the avatar of a user, but the user did not exist.");
				return;
			}

			Utilities.WriteLog(msg.User, $"requested the avatar of user {author.GetFullUsername()}.");
			await msg.Channel.SendMessage(
				string.IsNullOrWhiteSpace(author.AvatarUrl)
					? "https://www.lohikar.io/i/xb/avatar_missing.jpg"
					: author.AvatarUrl);
		}

		internal static async Task Ping(CommandInfo info, Message msg)
		{
			Utilities.WriteLog(msg.User, "requested a ping.");
			await msg.Channel.SendMessage(info.CommandText == "ping" ? "pong" : $"{info.CommandText} response");
		}
	}
}
