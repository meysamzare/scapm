using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

public class INewMessage
{
	public string senderName { get; set; }

	public string messageText { get; set; }

	public string sendDate { get; set; }

	public int senderId { get; set; }
}

namespace SCMR_Api.Hubs
{
	[Authorize(AuthenticationSchemes = "Bearer")]
	public class ChatHub : Hub
	{
		private readonly static ConnectionMapping<string> _connections =
			new ConnectionMapping<string>();

		public void SendChatMessage(string who, INewMessage newMessage)
		{
			string name = Context.User.Identity.Name;
			var connectionId = _connections.GetConnections(who).FirstOrDefault();

			Clients.Client(connectionId).SendAsync("newChatRecive", newMessage);
		}

		public override async Task OnConnectedAsync()
		{
			string name = Context.User.Identity.Name;

			if (!_connections.GetConnections(name).Contains(Context.ConnectionId))
			{
				_connections.Add(name, Context.ConnectionId);
			}

			await base.OnConnectedAsync();
		}

		public override async Task OnDisconnectedAsync(Exception exception)
		{
			string name = Context.User.Identity.Name;

			_connections.Remove(name, Context.ConnectionId);

			await base.OnDisconnectedAsync(exception);
		}

		public Task ThrowException()
		{
			throw new HubException("This error will be sent to the client!");
		}
	}


	public class ConnectionMapping<T>
	{
		private readonly Dictionary<T, HashSet<string>> _connections =
			new Dictionary<T, HashSet<string>>();

		public int Count
		{
			get
			{
				return _connections.Count;
			}
		}

		public void Add(T key, string connectionId)
		{
			lock (_connections)
			{
				HashSet<string> connections;
				if (!_connections.TryGetValue(key, out connections))
				{
					connections = new HashSet<string>();
					_connections.Add(key, connections);
				}

				lock (connections)
				{
					connections.Add(connectionId);
				}
			}
		}

		public IEnumerable<string> GetConnections(T key)
		{
			HashSet<string> connections;
			if (_connections.TryGetValue(key, out connections))
			{
				return connections;
			}

			return Enumerable.Empty<string>();
		}

		public void Remove(T key, string connectionId)
		{
			lock (_connections)
			{
				HashSet<string> connections;
				if (!_connections.TryGetValue(key, out connections))
				{
					return;
				}

				lock (connections)
				{
					connections.Remove(connectionId);

					if (connections.Count == 0)
					{
						_connections.Remove(key);
					}
				}
			}
		}

		public void RemovebyKey(T key)
		{
			lock (_connections)
			{
				HashSet<string> connections;
				if (!_connections.TryGetValue(key, out connections))
				{
					return;
				}

				_connections.Remove(key);
			}
		}
	}
}
