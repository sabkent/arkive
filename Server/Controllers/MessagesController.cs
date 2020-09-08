using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using pws.Server.Hubs;
using pws.Shared.Hubs;

namespace pws.Server.Controllers
{
    [Route("[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IHubContext<MyHub, IMyHub> _hubContext;

        public MessagesController(IHubContext<MyHub, IMyHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await _hubContext.Clients.All.SpreadAsync("Some message");
            return Ok("your mom");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Command command)
        {
            await _hubContext.Clients.All.SpreadAsync(command.Message);
            return Accepted();
        }
    }

    public class Command
    {
        public string Message { get; set; }
    }
}
