using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
namespace Application.Hubs
{
    public abstract class DataUpdateHub : Hub
    {
        public abstract Task NotifyDataUpdate(string dataType);
    }
}