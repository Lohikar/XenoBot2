using XenoBot2.Shared;

namespace XenoBot2
{
	internal static class SharedData
	{
		public static UserDataStore<ulong, ulong, UserFlag> UserFlags;
		public static UserDataStore<string, ulong, CommandState> CommandState;
	}
}
