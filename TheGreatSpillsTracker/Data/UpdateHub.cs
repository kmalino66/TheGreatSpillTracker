using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheGreatSpillsTracker.Data
{
    public class UpdateHub : Hub
    {
        public void SendBroadcast()
        {
            Clients.All.SendAsync("broadcastMessage");
        }
    }
}
