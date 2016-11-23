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
	internal static class Administration
	{
		internal static async Task HaltBot(CommandInfo info, User author, Channel channel)
		{
			Utilities.WriteLog(author, "is shutting down the bot.");
			await channel.SendMessage("Bot shutting down.");
			Thread.Sleep(1.Seconds());
			await Program.BotInstance.Exit();
		}

		internal static async Task Enable(CommandInfo info, User member, Channel channel)
		{
			if (!info.HasArguments)
			{
				await channel.SendMessage("You must specify the command to enable.");
				return;
			}
			if (Program.BotInstance.Commands[info.Arguments.First()].ResolveCommand().Flags.HasFlag(CommandFlag.NonDisableable))
			{
				await channel.SendMessage("That command cannot be disabled.");
				return;
			}
			if (!Utilities.HasState(CommandState.Disabled, info.Arguments.First()))
			{
				await channel.SendMessage("That command is already enabled.");
				return;
			}
			await channel.SendMessage($"Enabled command '{info.Arguments.First()}' on this channel.");
			Program.BotInstance.CommandStateData[info.Arguments.First(), channel.Id] ^= CommandState.Disabled;
		}

		internal static async Task Disable(CommandInfo info, User member, Channel channel)
		{
			if (!info.HasArguments)
			{
				await channel.SendMessage("You must specify the command to disable.");
				return;
			}
			if (Program.BotInstance.Commands[info.Arguments.First()].ResolveCommand().Flags.HasFlag(CommandFlag.NonDisableable))
			{
				await channel.SendMessage("That command cannot be disabled.");
				return;
			}
			if (Utilities.HasState(CommandState.Disabled, info.Arguments.First()))
			{
				await channel.SendMessage("That command is already disabled.");
				return;
			}
			await channel.SendMessage($"Disabled command '{info.Arguments.First()}' on this channel.");
			Program.BotInstance.CommandStateData[info.Arguments.First(), channel.Id] |= CommandState.Disabled;
		}

		internal static async Task IgnoreUser(CommandInfo info, User member, Channel channel)
		{
			if (!info.HasArguments)
			{
				Utilities.WriteLog(member, "tried to ignore, but forgot to say who to ignore.");
				await channel.SendMessage("You must specify who to ignore.");
				return;
			}

			var user = info.Arguments.First().GetMemberFromMention(channel);

			if (ToggleIgnore(member.Id, channel.Id))
			{
				Utilities.WriteLog(member, $"ignored {user.GetFullUsername()} on channel {channel.Name}");
				await channel.SendMessage($"Now ignoring {user.NicknameMention} on this channel.");
			}
			else
			{
				Utilities.WriteLog(member, $"unignored {user.GetFullUsername()} on channel {channel.Name}");
				await channel.SendMessage($"No longer ignoring {user.NicknameMention} on this channel.");
			}
		}

		internal static async Task GlobalIgnoreUser(CommandInfo info, User member, Channel channel)
		{
			if (!info.HasArguments)
			{
				Utilities.WriteLog(member, "tried to ignore, but forgot to say who to ignore.");
				await channel.SendMessage("You must specify who to ignore.");
				return;
			}

			var user = info.Arguments.First().GetMemberFromMention(channel);

			if (ToggleIgnore(member.Id))
			{
				Utilities.WriteLog(member, $"ignored {user.GetFullUsername()} globally.");
				await channel.SendMessage($"Now ignoring {user.NicknameMention} everywhere.");
			}
			else
			{
				Utilities.WriteLog(member, $"unignored {user.GetFullUsername()} globally.");
				await channel.SendMessage($"No longer ignoring {user.NicknameMention} everywhere (unless specifically ignored elsewhere).");
			}
		}

		/// <summary>
		///		Toggles the ignore state for a user, globally or per-channel.
		/// </summary>
		/// <param name="uid">The user ID to toggle ignore for.</param>
		/// <param name="chid">The channel ID to ignore on. Omit for global.</param>
		/// <returns>True if user is now ignored, false if not.</returns>
		private static bool ToggleIgnore(ulong uid, ulong chid = 0)
		{
			var ignored = Utilities.Permitted(UserFlag.Ignored, uid);
			if (ignored)
				Program.BotInstance.UserFlags[uid, chid] ^= UserFlag.Ignored;
			else
				Program.BotInstance.UserFlags[uid, chid] |= UserFlag.Ignored;

			return !ignored;
		}
	}
}
