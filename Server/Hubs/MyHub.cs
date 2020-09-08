using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using pws.Shared.Hubs;

namespace pws.Server.Hubs
{
    public class MyHub : Hub<IMyHub>
    {
        public async Task SendMessage(string content)
        {
            await Clients.All.SpreadAsync(content);
        }
    }
}
