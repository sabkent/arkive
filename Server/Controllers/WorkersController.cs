using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pws.Server.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using pws.Shared.Domain;
using pws.Shared.ReadModels;

namespace pws.Server.Controllers
{
    [Route("[controller]")]
    public class WorkersController : ControllerBase
    {
        private readonly WorkManagement _workManagement;

        public WorkersController(WorkManagement workManagement)
        {
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');

            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/')
            };

            var connString =  builder.ToString();
            
            NpgsqlConnection connect = new NpgsqlConnection(connString);

            connect.Open();
            _workManagement = workManagement;
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> Get()
        {
            var workers = await _workManagement.Workers.ToListAsync();
            return Ok(workers.Select(w => new V1.Worker {Name = w.Name}));
        }

        [HttpGet, Route("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            var worker = await _workManagement.Workers.FirstOrDefaultAsync(worker =>
                worker.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (worker == null)
                return NotFound(name);

            return Ok(worker);
        }

        [HttpPost, Route("")]
        public async Task<IActionResult> Post([FromBody] CreateWorkerRequest createWorkerRequest)
        {
            var worker = new Worker(createWorkerRequest.Name);
            await _workManagement.Workers.AddAsync(worker);
            await _workManagement.SaveChangesAsync();

            return Ok(worker);
        }
    }

    public class CreateWorkerRequest
    {
        public string Name { get; set; }
    }
}
