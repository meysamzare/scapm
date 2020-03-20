using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace SCMR_Api.Hubs
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UserHub : Hub
    {

        public static readonly ConnectionMapping<string> userConnections =
            new ConnectionMapping<string>();

        public void SendChatMessage(string who, string message)
        {
            string name = Context.User.Identity.Name;
        }

        public async Task removeOldUser(string name)
        {
            try
            {
                var oldConnectionId = userConnections.GetConnections(name).FirstOrDefault();

                userConnections.RemovebyKey(name);

                userConnections.Add(name, Context.ConnectionId);

                await Clients.Client(oldConnectionId).SendAsync("closeConnection");
            }
            catch
            {
            }

        }

        public override async Task OnConnectedAsync()
        {
            string name = Context.User.Identity.Name;

            if (!userConnections.GetConnections(name).Any())
            {
                userConnections.Add(name, Context.ConnectionId);
            }
            else if (userConnections.GetConnections(name).Contains(Context.ConnectionId))
            {
                // Do Nothing
            }
            else
            {
                // await Clients.Client(Context.ConnectionId).SendAsync("anotherUserLoginAlert");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string name = Context.User.Identity.Name;

            userConnections.Remove(name, Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }

        public Task ThrowException()
        {
            throw new HubException("This error will be sent to the client!");
        }
    }
}