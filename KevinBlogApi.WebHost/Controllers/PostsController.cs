using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KevinBlogApi.Core.Model.Interface;
using Microsoft.AspNetCore.Mvc;

namespace KevinBlogApi.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private IPostRepository _post;
        public PostsController(IPostRepository post)
        {
            _post = post;
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
            var post = await _post.GetPost(s => s.Slug == slug);
            return Ok(post);
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
