using System;

namespace XenoBot2
{
	[Serializable]
	internal class InvalidCommandException : Exception
	{
		public string AttemptedCommand { get; set; }

		public InvalidCommandException(string command)
		{
			AttemptedCommand = command;
		}
	}
}
