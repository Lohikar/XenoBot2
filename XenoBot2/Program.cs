namespace XenoBot2
{
	internal static class Program
	{
		public static BotCore BotInstance { get; private set; }
		private static void Main(string[] args)
		{
			BotInstance = new BotCore();
			BotInstance.Initialize().Wait();
			BotInstance.Connect();
		}
	}
}
