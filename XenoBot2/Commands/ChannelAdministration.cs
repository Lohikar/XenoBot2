using System.Linq;
using System.Threading.Tasks;
using Discord;
using XenoBot2.Shared;

namespace XenoBot2.Commands
{
	internal static class ChannelAdministration
	{
		/// <summary>
		///		Enables a disabled command for this channel.
		/// </summary>
		internal static async Task Enable(CommandInfo info, Message msg)
		{
			if (!info.HasArguments)
			{
				await msg.Channel.SendMessage("You must specify the command to enable.");
				return;
			}
			if (Program.BotInstance.Commands[info.Arguments.First()].ResolveCommand().Flags.HasFlag(CommandFlag.NonDisableable))
			{
				await msg.Channel.SendMessage("That command cannot be disabled.");
				return;
			}
			if (!Utilities.HasState(CommandState.Disabled, info.Arguments.First()))
			{
				await msg.Channel.SendMessage("That command is already enabled.");
				return;
			}
			await msg.Channel.SendMessage($"Enabled command '{info.Arguments.First()}' on this channel.");
			Program.BotInstance.CommandStateData[info.Arguments.First(), msg.Channel.Id] ^= CommandState.Disabled;
		}

		/// <summary>
		///		Disables an enabled command for this channel.
		/// </summary>
		internal static async Task Disable(CommandInfo info, Message msg)
		{
			if (!info.HasArguments)
			{
				await msg.Channel.SendMessage("You must specify the command to disable.");
				return;
			}
			if (Program.BotInstance.Commands[info.Arguments.First()].ResolveCommand().Flags.HasFlag(CommandFlag.NonDisableable))
			{
				await msg.Channel.SendMessage("That command cannot be disabled.");
				return;
			}
			if (Utilities.HasState(CommandState.Disabled, info.Arguments.First()))
			{
				await msg.Channel.SendMessage("That command is already disabled.");
				return;
			}
			await msg.Channel.SendMessage($"Disabled command '{info.Arguments.First()}' on this channel.");
			Program.BotInstance.CommandStateData[info.Arguments.First(), msg.Channel.Id] |= CommandState.Disabled;
		}

		/// <summary>
		///		Toggles the ignore state for a user for this channel.
		/// </summary>
		internal static async Task IgnoreUser(CommandInfo info, Message msg)
		{
			if (!info.HasArguments)
			{
				Utilities.WriteLog(msg.User, "tried to ignore, but forgot to say who to ignore.");
				await msg.Channel.SendMessage("You must specify who to ignore.");
				return;
			}

			var user = info.Arguments.First().GetMemberFromMention(msg.Channel);

			if (Utilities.ToggleIgnore(user, msg.Channel))
			{
				Utilities.WriteLog(msg.User, $"ignored {user.GetFullUsername()} on channel {msg.Channel.Name}");
				await msg.Channel.SendMessage($"Now ignoring {user.NicknameMention} on this channel.");
			}
			else
			{
				Utilities.WriteLog(msg.User, $"unignored {user.GetFullUsername()} on channel {msg.Channel.Name}");
				await msg.Channel.SendMessage($"No longer ignoring {user.NicknameMention} on this channel.");
			}
		}
	}
}
