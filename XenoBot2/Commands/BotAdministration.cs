using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Humanizer;
using XenoBot2.Shared;

namespace XenoBot2.Commands
{
	/// <summary>
	///		Bot administration commands. Commands in this group can only be executed by the bot administrator.
	/// </summary>
	internal static class BotAdministration
	{
		/// <summary>
		///		Terminates the bot.
		/// </summary>
		internal static async Task HaltBot(CommandInfo info, Message msg)
		{
			if (!Utilities.Permitted(UserFlag.BotAdministrate, msg.User))
			{
				Utilities.WriteLog("WARNING: PERMISSION CHECK FAILED ON HALT!");
				return;
			}
			Utilities.WriteLog(msg.User, "is shutting down the bot.");
			await msg.Channel.SendMessage("Bot shutting down.");
			Thread.Sleep(1.Seconds());
			await Program.BotInstance.Exit();
		}

		/// <summary>
		///		Toggles the global ignore status for a user.
		/// </summary>
		internal static async Task GlobalIgnoreUser(CommandInfo info, Message msg)
		{
			if (!info.HasArguments)
			{
				Utilities.WriteLog(msg.User, "tried to ignore, but forgot to say who to ignore.");
				await msg.Channel.SendMessage("You must specify who to ignore.");
				return;
			}

			var user = info.Arguments.First().GetMemberFromMention(msg.Channel);

			if (Utilities.ToggleIgnoreGlobal(user))
			{
				Utilities.WriteLog(msg.User, $"ignored {user.GetFullUsername()} globally.");
				await msg.Channel.SendMessage($"Now ignoring {user.NicknameMention} everywhere.");
			}
			else
			{
				Utilities.WriteLog(msg.User, $"unignored {user.GetFullUsername()} globally.");
				await msg.Channel.SendMessage($"No longer ignoring {user.NicknameMention} everywhere (unless specifically ignored elsewhere).");
			}
		}

		
	}
}
