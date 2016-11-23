using System;
using System.Collections.Generic;

namespace XenoBot2
{
	internal class UserDataStore<TK1, TK2, TV>
	{
		private readonly Dictionary<Tuple<TK1, TK2>, TV> _data;

		public UserDataStore(TV defaultValue, TK2 globalValue)
		{
			DefaultValue = defaultValue;
			GlobalValue = globalValue;
			_data = new Dictionary<Tuple<TK1, TK2>, TV>();
		}

		public TV DefaultValue { get; }
		public TK2 GlobalValue { get; }

		public TV this[TK1 firstKey, TK2 secondKey]
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
				if (EqualityComparer<TV>.Default.Equals(value, DefaultValue))
					_data.Remove(id);
			}
		}

		public TV this[TK1 firstKey] => this[firstKey, GlobalValue];

		public void Add(TK1 firstKey, TK2 secondKey, TV value)
		{
			var id = MakeId(firstKey, secondKey);
			if (_data.ContainsKey(id))
			{
				throw new ArgumentException("That key already exists!");
			}
			_data.Add(id, value);
		}

		public void Remove(TK1 firstKey, TK2 secondKey)
		{
			_data.Remove(MakeId(firstKey, secondKey));
		}

		private static Tuple<TK1, TK2> MakeId(TK1 memberId, TK2 channelId)
			=> new Tuple<TK1, TK2>(memberId, channelId);
	}
}