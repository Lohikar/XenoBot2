using System;
using System.Collections.Concurrent;
using System.Threading;

namespace XenoBot2
{
	// from https://stackoverflow.com/questions/3670057/does-console-writeline-block

	/// <summary>
	/// A console that queues console writes and executes them on a different thread.
	/// </summary>
	public static class NonBlockingConsole
	{
		private static readonly BlockingCollection<string> _mQueue = new BlockingCollection<string>();

		static NonBlockingConsole()
		{
			var thread = new Thread(
				() => { while (true) Console.WriteLine(_mQueue.Take()); }) {IsBackground = true};
			thread.Start();
		}

		public static void WriteLine(string value)
		{
			_mQueue.Add(value);
		}
	}
}
