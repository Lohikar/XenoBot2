using System;
using DiscordSharp.Objects;

namespace XenoBot2
{
	public struct CommandMetadata
	{
		public bool Equals(CommandMetadata other)
		{
			return string.Equals(CommandText, other.CommandText);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is CommandMetadata && Equals((CommandMetadata) obj);
		}

		public override int GetHashCode()
		{
			return CommandText?.GetHashCode() ?? 0;
		}

		public string CommandText { get; }
		public DateTime LastUseTime { get; set; }
		public bool IsEnabled { get; set; }
		public DiscordMember DisablerMember { get; set; }
		public DiscordMember LastExecutor { get; set; }
		public TimeSpan MinimumTimeBetweenInvokes { get; set; }

		public CommandMetadata(string command)
		{
			CommandText = command;
			LastUseTime = DateTime.MinValue;
			IsEnabled = true;
			DisablerMember = null;
			LastExecutor = null;
			MinimumTimeBetweenInvokes = TimeSpan.Zero;
		}

		public CommandStatus GetStatus()
		{
			if (IsEnabled &&
			    (MinimumTimeBetweenInvokes == TimeSpan.Zero || DateTime.Now - LastUseTime > MinimumTimeBetweenInvokes))
			{
				return CommandStatus.Allowed;
			}
			if (!IsEnabled)
			{
				return CommandStatus.Disabled;
			}
			return IsEnabled ? CommandStatus.RateLimited : CommandStatus.Unknown;
		}

		public static bool operator ==(CommandMetadata cmd1, string command) => cmd1.CommandText == command;

		public static bool operator !=(CommandMetadata cmd1, string command) => !(cmd1 == command);
	}
}