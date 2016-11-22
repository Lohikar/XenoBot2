using System;
using System.Collections.Generic;

namespace XenoBot2
{
	internal class UserDataStore<T>
	{
		private readonly Dictionary<Tuple<string, string>, T> _permissionDict;

		public UserDataStore(T defaultValue)
		{
			DefaultValue = defaultValue;
			_permissionDict = new Dictionary<Tuple<string, string>, T>();
		}

		public T DefaultValue { get; }

		public T this[string firstKey, string secondKey]
		{
			get
			{
				var id = MakeId(firstKey, secondKey);
				return _permissionDict.ContainsKey(id) ? _permissionDict[id] : DefaultValue;
			}
			set
			{
				var id = MakeId(firstKey, secondKey);
				if (!_permissionDict.ContainsKey(id))
					_permissionDict.Add(id, value);
				else
					_permissionDict[id] = value;
			}
		}

		public T this[string firstKey] => this[firstKey, "*"];

		public void Add(string firstKey, string secondKey, T value)
		{
			var id = MakeId(firstKey, secondKey);
			if (_permissionDict.ContainsKey(id))
			{
				throw new ArgumentException("That key already exists!");
			}
			_permissionDict.Add(id, value);
		}

		public void Remove(string firstKey, string secondKey)
		{
			_permissionDict.Remove(MakeId(firstKey, secondKey));
		}

		private static Tuple<string, string> MakeId(string memberId, string channelId)
			=> new Tuple<string, string>(memberId, channelId);
	}
}