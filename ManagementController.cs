using System;

using Microsoft.AspNetCore.Mvc;

namespace heroku
{
    [Route("[controller]")]
    public class ManagementController : ControllerBase
    {
        [HttpGet, Route("")]
        public IActionResult Get()
        {
            return Ok("this has changed");
        }
    }
}
