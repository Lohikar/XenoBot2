using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Discord;
using XenoBot2.Shared;

namespace XenoBot2
{
	[Serializable]
	internal class ServerManager
	{
		private readonly IDictionary<ulong, ServerInfo> _servers;
		[NonSerialized]
		private readonly IDictionary<ulong, UserFlag> _globalPermissions;	// this should be short, maybe 1-2 users.

		public UserFlag GetPermissionFlag(ulong user, Channel channel)
		{
			var flag = UserFlag.None;
			if (_globalPermissions.ContainsKey(user))
				flag |= _globalPermissions[user];

			if (channel == null)
				return flag;	// only return global flags if no channel context supplied

			if (channel.IsPrivate)
				return flag | UserFlag.User;

			var server = channel.Server;
			if (!_servers.ContainsKey(server.Id))
			{
				_servers.Add(server.Id, new ServerInfo(server));
			}
			flag |= _servers[server.Id].GetPermission(user);

			// possibly slow, cache?
			if (channel.GetUser(user).ServerPermissions.Speak)
				flag |= UserFlag.User;

			return flag;
		}

		public UserFlag GetPermissionFlag(User user, Channel channel) => GetPermissionFlag(user.Id, channel);

		public ServerManager()
		{
			_servers = new ConcurrentDictionary<ulong, ServerInfo>();
			_globalPermissions = new ConcurrentDictionary<ulong, UserFlag>();
		}

		public void AddGlobalPermission(ulong target, UserFlag flag)
		{
			if (!_globalPermissions.ContainsKey(target))
			{
				_globalPermissions.Add(target, flag);
			}
			else
			{
				_globalPermissions[target] |= flag;
			}
		}

		public void AddGlobalPermission(User target, UserFlag flag) => AddGlobalPermission(target.Id, flag);

		public void RemoveGlobalPermission(ulong target, UserFlag flag)
		{
			if (!_globalPermissions.ContainsKey(target))
				return;

			var perm = _globalPermissions[target];
			perm ^= flag;
			if (perm == UserFlag.None)
				_globalPermissions.Remove(target);
			else
				_globalPermissions[target] = perm;
		}

		public void RemoveGlobalPermission(User target, UserFlag flag) => RemoveGlobalPermission(target.Id, flag);

		public ServerInfo this[ulong id] => _servers[id];
	}

	[Serializable]
	internal class ServerInfo
	{
		private readonly Server _target;
		private readonly IDictionary<ulong, UserFlag> _userPermissions;

		public ServerInfo(Server target)
		{
			_target = target;
			_userPermissions = new ConcurrentDictionary<ulong, UserFlag>();
			Utilities.WriteLog($"ServerManager: Registered new server {target.Name} ({target.Id})");
		}

		public UserFlag GetPermission(ulong user)
		{
			var flag = UserFlag.None;
			if (_target.Owner.Id == user)
				flag |= UserFlag.Owner;
			if (_userPermissions.ContainsKey(user))
				flag |= _userPermissions[user];
			return flag;
		}

		public void AddFlag(User target, UserFlag flag)
		{
			if (!_userPermissions.ContainsKey(target.Id))
			{
				_userPermissions.Add(target.Id, flag);
			}
			else
			{
				_userPermissions[target.Id] |= flag;
			}
		}

		public void RemoveFlag(User target, UserFlag flag)
		{
			if (!_userPermissions.ContainsKey(target.Id))
				return;

			var perm = _userPermissions[target.Id];
			perm ^= flag;
			if (perm == UserFlag.None)
				_userPermissions.Remove(target.Id);
			else
				_userPermissions[target.Id] = perm;
		}
	}
}
