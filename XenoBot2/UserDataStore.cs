using System;
using System.Collections.Generic;

namespace XenoBot2
{
	internal class UserDataStore<T>
	{
		private readonly Dictionary<Tuple<string, string>, T> _data;

		public UserDataStore(T defaultValue)
		{
			DefaultValue = defaultValue;
			_data = new Dictionary<Tuple<string, string>, T>();
		}

		public T DefaultValue { get; }

		public T this[string firstKey, string secondKey]
		{
			get
			{
				var id = MakeId(firstKey, secondKey);
				return _data.ContainsKey(id) ? _data[id] : DefaultValue;
			}
			set
			{
				var id = MakeId(firstKey, secondKey);
				if (!_data.ContainsKey(id))
					_data.Add(id, value);
				else
					_data[id] = value;

				// no point storing values that are default
				if (EqualityComparer<T>.Default.Equals(value, DefaultValue))
					_data.Remove(id);
			}
		}

		public T this[string firstKey] => this[firstKey, "*"];

		public void Add(string firstKey, string secondKey, T value)
		{
			var id = MakeId(firstKey, secondKey);
			if (_data.ContainsKey(id))
			{
				throw new ArgumentException("That key already exists!");
			}
			_data.Add(id, value);
		}

		public void Remove(string firstKey, string secondKey)
		{
			_data.Remove(MakeId(firstKey, secondKey));
		}

		private static Tuple<string, string> MakeId(string memberId, string channelId)
			=> new Tuple<string, string>(memberId, channelId);
	}
}