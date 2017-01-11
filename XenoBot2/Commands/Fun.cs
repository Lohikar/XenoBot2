using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Discord;
using XenoBot2.Data;
using XenoBot2.Shared;
using Newtonsoft.Json.Linq;

namespace XenoBot2.Commands
{
	internal static class Fun
	{
		private static Random _rnd;

		internal static async Task WhatTheCommit(CommandInfo info, Message msg)
		{
			if (_rnd == null)
				_rnd = new Random();

			Utilities.WriteLog(msg.User, "requested a WhatTheCommit message.");
			// TODO: Not this
			var wtc = Strings.WhatTheCommit.GetRandom().Trim()
				.Replace("XNAMEX", Strings.Names.GetRandom())
				.Replace("XUPPERNAMEX", Strings.Names.GetRandom().ToUpper())
				.Replace("XNUMX", _rnd.Next(9000).ToString())
				.Replace("<br/>", "\n");
			await msg.Channel.SendMessage($"*{wtc}*");
		}

		internal static async Task CatFact(CommandInfo info, Message msg)
		{
			Utilities.WriteLog(msg.User, "requested a cat fact.");
			await msg.Channel.SendMessage($"Cat Fact: *{await Shared.Utilities.GetStringAsync("https://cat-facts-as-a-service.appspot.com/fact")}*");
		}

		internal static async Task EightBall(CommandInfo info, Message msg)
		{
			Utilities.WriteLog(msg.User, "consulted the 8 ball.");
			if (info.HasArguments)
			{
				await msg.Channel.SendMessage($"{msg.User.NicknameMention} asked: **{string.Join(" ", info.Arguments)}**\n" +
				                  $"The 8 Ball says... *{Strings.EightBall.GetRandom()}*");
			}
			else
			{
				await msg.Channel.SendMessage($"*{Strings.EightBall.GetRandom()}*");
			}
			
		}

		internal static async Task RandomCat(CommandInfo info, Message msg)
		{
			using (var client = new WebClient())
			{
				dynamic result = JObject.Parse(await client.DownloadStringTaskAsync("http://random.cat/meow"));
				Utilities.WriteLog(msg.User, "requested a random cat.");
				await msg.Channel.SendMessage($"{msg.User.NicknameMention}: {result.file}");
			}
		}

		internal static async Task RollDieEx(CommandInfo info, Message msg)
		{
			if (!info.HasArguments)
			{
				await RollDie(info, msg);
				return;
			}
			// acceptable format: XdY+Z, where X is die count, Y is sides/die, and Z is number to add to sum. Z is optional, X is assumed to be 1 if not supplied.

			// reassemble args
			var cmdtext = string.Join(" ", info.Arguments);
			int numDies = 0;
			int sidesPerDie = 0;
			int bonus = 0;

			var currTarget = 0;	// if 0, numDies; if 1, sidesPerDie; if 2, bonus.

			var b = new StringBuilder();

			foreach (char c in cmdtext)
			{
				if (char.IsDigit(c))
				{
					b.Append(c);
					continue;
				}
				if (c == 'd' && currTarget == 0)
				{
					if (!int.TryParse(b.ToString(), out numDies))
					{
						// error somehow
					}
					currTarget = 1;
					b.Clear();
				}
				if (c == '+')
				{
					if (!int.TryParse(b.ToString(), out sidesPerDie))
					{
						// error somehow
					}
					currTarget = 2;
					b.Clear();
				}
			}
			if (currTarget == 1 && !int.TryParse(b.ToString(), out sidesPerDie))
			{
				// error
			}
			else if (currTarget == 2 && !int.TryParse(b.ToString(), out bonus))
			{
				// error
			}

			// got the values, actually roll the die now.
			Utilities.WriteLog(msg.User, bonus == 0 ? $"rolled a {numDies}d{sidesPerDie}" : $"rolled a {numDies}d{sidesPerDie}+{bonus}");
			var resultVals = Roll(numDies, sidesPerDie);
			var values = resultVals as IList<int> ?? resultVals.ToList();
			var result = values.Sum() + bonus;
			var resultBuilder = new StringBuilder();
			resultBuilder.Append("Rolling a ");
			resultBuilder.Append($"{numDies}d{sidesPerDie}");
			if (bonus != 0)
				resultBuilder.Append($"+{bonus}");
			resultBuilder.AppendLine();
			if (bonus != 0)
				resultBuilder.Append("(");
			resultBuilder.Append(string.Join(" + ", values));
			if (bonus != 0)
				resultBuilder.Append($") + {bonus}");
			resultBuilder.Append($" = {result}");
			await msg.Channel.SendMessage(resultBuilder.ToString());
		}

		private static IEnumerable<int> Roll(int numDies, int numSides)
		{
			for (var i = 0; i < numDies; i++)
				yield return _rnd.Next(1, numSides);
		}
 
		internal static async Task RollDie(CommandInfo info, Message msg)
		{
			if (info.HasArguments)
			{
				// assume format XdX
				string[] arg;
				if (info.Arguments.Count == 1)
				{
					arg = info.Arguments.First().Split('d');
				}
				else if (info.Arguments.AtLeast(2))
				{
					arg = info.Arguments.ToArray();
				}
				else
				{
					await msg.Channel.SendMessage("Malformed arguments.");
					return;
				}
				int numDies;
				if (!int.TryParse(arg[0], out numDies))
				{
					await msg.Channel.SendMessage("Malformed arguments: unable to parse number of dies.");
					return;
				}
				if (numDies > 400)
				{
					await msg.Channel.SendMessage("Unable to roll: too many dice.");
					return;
				}
				int numSides;
				if (!int.TryParse(arg[1], out numSides))
				{
					await msg.Channel.SendMessage("Malformed arguments: unable to parse number of sides.");
					return;
				}
				Utilities.WriteLog(msg.User, $"rolled a {numDies}d{numSides}.");
				var results = new List<int>();
				for (var i = 0; i < numDies; i++)
				{
					results.Add(_rnd.Next(1, numSides));
				}

				var output = $"Rolled {numDies}d{numSides}\nResult: {string.Join(" + ", results)} = {results.Sum()}";
				if (output.Length >= 2000)
				{
					await msg.Channel.SendMessage("Unable to roll: resultant output is too long.");
				}
				await msg.Channel.SendMessage(output);
				return;
			}
			Utilities.WriteLog(msg.User, "Rolled a 1d6 (default).");
			await msg.Channel.SendMessage($"Rolled 1d6\nResult: {_rnd.Next(1, 6)}");
		}
	}
}
