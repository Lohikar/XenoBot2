using System;
using System.Threading.Tasks;
using Discord;
using XenoBot2.Data;
using XenoBot2.Shared;

namespace XenoBot2.Commands
{
	internal static class Fun
	{
		private static Random _rnd;

		internal static async Task WhatTheCommit(CommandInfo info, User author, Channel channel)
		{
			if (_rnd == null)
				_rnd = new Random();

			Utilities.WriteLog(author, "requested a WhatTheCommit message.");
			var msg = Strings.WhatTheCommit.GetRandom().Trim()
				.Replace("XNAMEX", Strings.Names.GetRandom())
				.Replace("XUPPERNAMEX", Strings.Names.GetRandom().ToUpper())
				.Replace("XNUMX", _rnd.Next(9000).ToString());
			await channel.SendMessage($"*{msg}*");
		}

		internal static async Task CatFact(CommandInfo info, User author, Channel channel)
		{
			Utilities.WriteLog(author, "requested a cat fact.");
			await channel.SendMessage($"Cat Fact: *{Shared.Utilities.GetStringFromWebService("https://cat-facts-as-a-service.appspot.com/fact")}*");
		}

		internal static async Task EightBall(CommandInfo info, User author, Channel channel)
		{
			Utilities.WriteLog(author, "consulted the 8 ball.");
			if (info.HasArguments)
			{
				await channel.SendMessage($"{author.MakeMention()} asked: **{string.Join(" ", info.Arguments)}**\n" +
				                  $"The 8 Ball says... *{Strings.EightBall.GetRandom()}*");
			}
			else
			{
				await channel.SendMessage($"*{Strings.EightBall.GetRandom()}*");
			}
			
		}
	}
}
