using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
namespace API.Hubs
{
  public class DataUpdateHub : Application.Hubs.DataUpdateHub
    {
        public override async Task NotifyDataUpdate(string dataType)
        {
            await Clients.All.SendAsync("DataUpdated", dataType);
        }
    }
}