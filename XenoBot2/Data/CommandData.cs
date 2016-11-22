using System.Collections.Generic;

namespace XenoBot2.Data
{
	/// <summary>
	///		Strings & other data used by commands.
	/// </summary>
	internal static class CommandData
	{
		public static readonly IReadOnlyList<string> SudoMessages = new List<string>
		{
			"You can't tell me what to do!",
			"You're not the boss of me!",
			"I can't do that.",
			"Go `fsck` yourself.",
			"I'm sorry, but I can't do that {0}.",
			"*HISS*",
			"No.",
			"Do it yourself."
		};

		public static readonly IReadOnlyList<string> SudoAdminMessages = new List<string>
		{
			"Alright.",
			"Right away.",
			"Sure.",
			"Why not?",
			"I guess."
		};

		public static readonly IReadOnlyList<string> WelcomeBackMessages = new List<string>
		{
			"Welcome Back, {0}.",
			"wb {0}",
			"weba {0}"
		};

		public static readonly IReadOnlyList<string> FrontResponses = new List<string>
		{
			"side",
			"Very funny, {0}.",
			"Ha.",
			"up"
		};

		public static readonly IReadOnlyList<string> HeartResponses = new List<string>
		{
			"<2"
		};

		public static IList<string> Names;
		public static IList<string> WhatTheCommit;

		public static readonly IReadOnlyList<string> EightBall = new List<string>
		{
			"Signs point to yes.",
			"Yes.",
			"Reply hazy, try again.",
			"Without a doubt.",
			"My sources say no.",
			"As I see it, yes.",
			"Concentrate and ask again.",
			"Outlook not so good.",
			"It is decidedly not so.",
			"Better not tell you now.",
			"Very doubtful.",
			"Yes - definitely.",
			"It is certain.",
			"Cannot predict now.",
			"Most likely.",
			"Ask again later.",
			"My reply is no.",
			"Outlook good.",
			"Don't count on it.",
			"Yes, in due time.",
			"You will have to wait.",
			"Outlook so so.",
			"I have my doubts.",
			"Who knows?",
			"Looking good!",
			"Go for it!",
			"Are you kidding?",
			"Don't bet on it.",
			"Forget about it.",
			"Probably."
		};
	}
}