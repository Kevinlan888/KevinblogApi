using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;
        public PostsController(IPostRepository post, ILogger<PostsController> logger,
            IHubContext<ChatHub> hubContext, IMapper mapper)
        {
            _post = post;
            _logger = logger;
            _hubContext = hubContext;
            _mapper = mapper;
        }

        // GET all Post description
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var posts = await _post.ToListAsync();
            var postDescs = _mapper.Map<IEnumerable<Post>, IEnumerable<ListPostDescViewModel>>(posts);
            return Ok(postDescs);
        }

        // GET a specific post by slug
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
            catch (Exception ex)
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

        // Add a Post
        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<string>> AddPost([FromBody] AddPostViewModel value)
        {
            try
            {
                var post = await _post.GetPost(s => s.Slug == value.Slug);
                Post post2 = null;
                if (value.postId == null)
                {
                    if (post != null)
                    {
                        return Ok(new { result = false, msg = "slug already existed." });
                    }
                    else
                    {
                        post2 = new Post()
                        {
                            PostId = value.postId,
                            Slug = value.Slug,
                            MarkDown = value.MarkDown,
                            Content = value.Content,
                            CreateDate = DateTime.Now,
                            Tags = value.Tags,
                            Title = value.Title,
                            UserId = this.User.Identity.Name
                        };
                    }
                }
                else
                {
                    post2 = await _post.GetPost(s => s.PostId == value.postId);
                    if (post2 != null)
                    {
                        if (post != null && post.PostId != post2.PostId)
                        {
                            return Ok(new { result = false, msg = "slug already existed." });
                        }

                        post2.Slug = value.Slug;
                        post2.MarkDown = value.MarkDown;
                        post2.Content = value.Content;
                        post2.Title = value.Title;
                        post2.Tags = value.Tags;
                        post2.UpdateDate = DateTime.Now;
                    }
                    else
                    {
                        return Ok(new { result = false, msg = "Post not exist." });
                    }
                }

                var result = await _post.AddOrEdit(post2);
                await _hubContext.Clients.All.SendAsync("News", "Kevin", "I posted an article");
                return Ok(new { result = result, msg = "" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new { result = false, msg = ex.Message });
            }
        }

        // DELETE a post
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<string>> DeletePost(string id)
        {
            try
            {
                var result = await _post.DeletePost(id);
                return Ok(new { Result = result, msg = "" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.HelpLink, ex.Message);
                return Ok(new { Result = false, msg = ex.Message });
            }
        }

        [HttpGet]
        public async Task Testchat()
        {
            await _hubContext.Clients.All.SendAsync("News", "test", "hahah");
        }
    }
}
