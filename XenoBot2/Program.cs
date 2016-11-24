using System;
using System.IO;

namespace XenoBot2
{
	internal static class Program
	{
		public static BotCore BotInstance { get; private set; }

#if DEBUG
		public const string BuildType = "DEBUG";
#else
		public const string BuildType = "RELEASE";
#endif

		private static void Main(string[] args)
		{
			BotInstance = new BotCore();
			try
			{
				BotInstance.Initialize().Wait();
				BotInstance.Connect();
			}
			catch (AggregateException ex)
			{
				if (!(ex.GetBaseException() is FileNotFoundException))
					throw ex.GetBaseException();

				NonBlockingConsole.WriteLine();
				NonBlockingConsole.WriteLine("Press enter to exit.");
				Console.ReadLine();
			}
		}
	}
}
