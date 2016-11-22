using System.Collections.Generic;
using System.Linq;
using DiscordSharp.Objects;

namespace XenoBot2.DataManagement
{
	internal class ChannelManager
	{
		private IDictionary<string, DiscordChannelBase> _channels;
		private IDictionary<string, string> _channelIdMappings;

		public delegate void ChannelRegisteredDelegate(DiscordChannelBase channel);
		public delegate void ChannelDeregisteredDelegate(DiscordChannelBase channel);
		public delegate void ChannelShortIdAssignedDelegate(string channelId, string shortId);

		public event ChannelShortIdAssignedDelegate ChannelShortIdAssigned;
		public event ChannelRegisteredDelegate ChannelRegistered;
		public event ChannelDeregisteredDelegate ChannelDeregistered;

		public void Register(DiscordChannel channel)
		{
			if (_channels.ContainsKey(channel.ID))
			{
				// channel already registered
				Utilities.WriteLog($"Attempted to register channel '{channel.Name}', but it was already registered.");
			}
			else
			{
				Utilities.WriteLog($"Registered channel '{channel.Name}'.");
				_channels.Add(channel.ID, channel);
				ChannelRegistered?.Invoke(channel);
			}
		}

		public void Deregister(DiscordChannel channel)
		{
			if (_channelIdMappings.Any(mapping => mapping.Value == channel.ID))
			{
				// remove shortid if present
				_channelIdMappings.Remove(_channelIdMappings.First(mapping => mapping.Value == channel.ID));
			}
			if (_channels.ContainsKey(channel.ID))
			{
				_channels.Remove(channel.ID);
				ChannelDeregistered?.Invoke(channel);
			}
			Utilities.WriteLog($"Deregistered channel '{channel.ID}'");
		}

		public DiscordChannelBase this[string id]
		{
			get
			{
				if (_channelIdMappings.ContainsKey(id))
				{
					return _channels[_channelIdMappings[id]];
				}
				return _channels.ContainsKey(id) ? _channels[id] : null;
			}
		}
	}
}
