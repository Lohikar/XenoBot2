using System;
using DiscordSharp;
using DiscordSharp.Objects;
using XenoBot2.Data;
using XenoBot2.Shared;

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
			var msg = Strings.WhatTheCommit.GetRandom().Trim()
				.Replace("XNAMEX", Strings.Names.GetRandom())
				.Replace("XUPPERNAMEX", Strings.Names.GetRandom().ToUpper())
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
				client.SendMessageToRoom($"{author.MakeMention()} asked: **{string.Join(" ", info.Arguments)}**\n" +
				                  $"The 8 Ball says... *{Strings.EightBall.GetRandom()}*", channel);
			}
			else
			{
				client.SendMessageToRoom($"*{Strings.EightBall.GetRandom()}*", channel);
			}
			
		}
	}
}
