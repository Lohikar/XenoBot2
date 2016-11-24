namespace XenoBot2
{
	public class Config
	{
		public int ConfigVersion { get; set; } = 1;
		public char CommandPrefix { get; set; } = '$';
		public string ApiToken { get; set; } = string.Empty;
		public ulong[] BotAdmins { get; set; } = {0};
		public ulong[] BotDebuggers { get; set; } = {0};
	}
}
