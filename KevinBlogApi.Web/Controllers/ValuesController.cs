using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KevinBlogApi.Web.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace KevinBlogApi.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;
        private readonly IHubContext<ChatHub> _hubContext;

        public ValuesController(ILogger<ValuesController> logger,
            IHubContext<ChatHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            _logger.LogDebug("Someone's getting information");
            _hubContext.Clients.All.SendAsync("News", "kevin", "I got some text, haha.");
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
