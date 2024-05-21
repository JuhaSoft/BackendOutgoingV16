using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendUpdateDataNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveUpdateDataNotification", message);
        }
    }
}