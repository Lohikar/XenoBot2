using System;
using DiscordSharp;
using DiscordSharp.Objects;
using XenoBot2.Data;

namespace XenoBot2.Commands
{
	internal static class Fun
	{
		private static Random _rnd;

		internal static void WhatTheCommit(DiscordClient client, CommandInfo info, DiscordMember author, DiscordChannelBase channel)
		{
			if (_rnd == null)
				_rnd = new Random();

			Utilities.WriteLog(author, "requested a WhatTheCommit message.");
			var msg = CommandData.WhatTheCommit.GetRandom().Trim()
				.Replace("XNAMEX", CommandData.Names.GetRandom())
				.Replace("XUPPERNAMEX", CommandData.Names.GetRandom().ToUpper())
				.Replace("XNUMX", _rnd.Next(9000).ToString());
			client.SendMessageToRoom($"*{msg}*", channel);
		}

		internal static void CatFact(DiscordClient client, CommandInfo info, DiscordMember author, DiscordChannelBase channel)
		{
			Utilities.WriteLog(author, "requested a cat fact.");
			client.SendMessageToRoom($"Cat Fact: *{Utilities.GetStringFromWebService("https://cat-facts-as-a-service.appspot.com/fact")}*", channel);
		}

		internal static void EightBall(DiscordClient client, CommandInfo info, DiscordMember author, DiscordChannelBase channel)
		{
			Utilities.WriteLog(author, "consulted the 8 ball.");
			if (info.HasArguments)
			{
				client.SendMessageToRoom($"{author.GetMention()} asked: **{string.Join(" ", info.Arguments)}**\n" +
				                  $"The 8 Ball says... *{CommandData.EightBall.GetRandom()}*", channel);
			}
			else
			{
				client.SendMessageToRoom($"*{CommandData.EightBall.GetRandom()}*", channel);
			}
			
		}

		internal static void PrefixMeme(DiscordClient client, CommandInfo info, DiscordMember author,
			DiscordChannelBase channel)
		{
			if (!info.HasArguments)
			{
				client.SendMessageToRoom("Not enough arguments.", channel);
				Utilities.WriteLog(author, "tried to get a meme, but forgot to specify which one.");
				return;
			}
			Utilities.WriteLog(author, $"fetched meme '{info.Arguments[0]}'");
			client.SendMessageToRoom($"https://www.lohikar.io/i/m/{info.Arguments[0]}", channel);
		}
	}
}
