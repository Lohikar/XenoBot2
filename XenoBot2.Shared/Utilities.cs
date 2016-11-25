using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace XenoBot2.Shared
{
	public static class Utilities
	{
		private static Random _random;

		/// <summary>
		///     Downloads a text file from a web service and returns the contents as a string.
		/// </summary>
		/// <param name="url">The URL of the file to download.</param>
		/// <returns></returns>
		[Obsolete("Use GetStringAsync if possible.")]
		public static string GetStringFromWebService(string url)
		{
			using (var webclient = new WebClient())
			{
				var data = webclient.DownloadData(url);
				return Encoding.ASCII.GetString(data);
			}
		}

		/// <summary>
		///     Downloads a text file from a web site and returns the contents as a string.
		/// </summary>
		/// <param name="url">The url of the resource to fetch.</param>
		/// <returns></returns>
		public static async Task<string> GetStringAsync(string url)
		{
			using (var client = new WebClient())
				return await client.DownloadStringTaskAsync(url);
		}

		/// <summary>
		///     Splits a string into an array of strings.
		/// </summary>
		/// <param name="source">The string to split.</param>
		/// <param name="predictedSplitLen">How large each string probably will be. Specify to theoretically improve performance.</param>
		/// <param name="splitValues">Array of chars to split on.</param>
		/// <returns></returns>
		public static IEnumerable<string> LazySplit(this string source, int predictedSplitLen, params char[] splitValues)
		{
			var builder = predictedSplitLen <= 0
				? new StringBuilder()
				: new StringBuilder(predictedSplitLen);
			foreach (var c in source)
			{
				if (splitValues.Contains(c))
				{
					// split off string
					var result = builder.ToString();
					builder.Clear();
					yield return result;
				}

				builder.Append(c);
			}
		}

		/// <summary>
		///     Repeats a string.
		/// </summary>
		/// <param name="source">The string to repeat.</param>
		/// <param name="times">How many times to repeat the string. Must be greater than 0.</param>
		/// <returns></returns>
		public static string Repeat(this string source, int times)
		{
			if (times <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(times));
			}
			if (source == null)
				throw new ArgumentNullException(nameof(source));
			if (times == 1)
				return source;
			var builder = new StringBuilder(source.Length*times);
			for (var i = 0; i < times; i++)
			{
				builder.Append(source);
			}
			return builder.ToString();
		}

		[Obsolete]
		public static string MakeMention(this User target) => $"<@{target.Id}>";

		public static string GetFullUsername(this User target) => $"{target.Name}#{target.Discriminator}";
		public static string GetFullUsername(this Profile target) => $"{target.Name}#{target.Discriminator}";

		/// <summary>
		///     Returns a random string from a list of strings.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static string GetRandom(this IReadOnlyList<string> source)
		{
			if (_random == null)
			{
				_random = new Random();
			}
			return source.Any() ? source?[_random.Next(source.Count)] : "";
		}

		/// <summary>
		///     Returns a random string from a list of strings.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static string GetRandom(this IList<string> source)
		{
			if (_random == null)
			{
				_random = new Random();
			}
			return source?.Any() ?? false ? source[_random.Next(source.Count)] : "";
		}

		/// <summary>
		///     Returns true if source has at least num elements.
		/// </summary>
		public static bool AtLeast<T>(this ICollection<T> source, int num) => source.Count >= num;

		/// <summary>
		///     Returns true if source has less than num elements.
		/// </summary>
		public static bool LessThan<T>(this ICollection<T> source, int num) => source.Count < num;

		[Obsolete]
		public static string GetName(this Channel channel) => channel.Name;

		public static User GetMemberFromMention(this string src, Channel channel)
			=> channel.GetUser(uint.Parse(src.Replace("<@!", "").Replace("<@", "").Replace(">", "")));
	}
}