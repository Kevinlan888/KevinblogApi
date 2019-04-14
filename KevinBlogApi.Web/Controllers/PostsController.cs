using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KevinBlogApi.Core.Model;
using KevinBlogApi.Core.Model.Interface;
using KevinBlogApi.Web.Hubs;
using KevinBlogApi.Web.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace KevinBlogApi.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private IPostRepository _post;
        private ILogger<PostsController> _logger;
        private readonly IHubContext<ChatHub> _hubContext;
        public PostsController(IPostRepository post, ILogger<PostsController> logger,
            IHubContext<ChatHub> hubContext)
        {
            _post = post;
            _logger = logger;
            _hubContext = hubContext;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{slug}")]
        public async Task<ActionResult<string>> Get(string slug)
        {
            try
            {
                var post = await _post.GetPost(s => s.Slug == slug);
                if (post != null)
                    return Ok(post);
                else return NotFound();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound();
            }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<string>> AddPost([FromBody] AddPostViewModel value)
        {
            try
            {
                var post = new Post()
                {
                    Slug = value.Slug,
                    MarkDown = value.MarkDown,
                    Content = value.Content,
                    CreateDate = DateTime.Now,
                    Tag = value.Tag,
                    Title = value.Title,
                    UserId = this.User.Identity.Name
                };
                var result = await _post.AddOrEdit(post);
                await _hubContext.Clients.All.SendAsync("News", "Kevin","I posted an article");
                return Ok(new { result = result, Msg = "" });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new { result = false, Msg = ex.Message });
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<string>> DeletePost(string id)
        {
            try
            {
                var result = await _post.DeletePost(id);
                return Ok(new { Result = result, Msg = "" });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.HelpLink, ex.Message);
                return Ok(new { Result = false, Msg = ex.Message });
            }
        }
    }
}
