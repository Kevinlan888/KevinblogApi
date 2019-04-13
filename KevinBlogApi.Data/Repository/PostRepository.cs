using KevinBlogApi.Core.Model;
using KevinBlogApi.Core.Model.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KevinBlogApi.Data.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly KevinBlogDataContext _context;
        public PostRepository(KevinBlogDataContext context)
        {
            _context = context;
        }

        public async Task<bool> AddOrEdit(Post post)
        {
            if(string.IsNullOrEmpty(post.PostId))
            {
                post.PostId = Guid.NewGuid().ToString();
                await _context.Posts.AddAsync(post);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                _context.Posts.Update(post);
                return true;
            }
        }

        public async Task<Post> GetPost(System.Linq.Expressions.Expression<Func<Post, bool>> expression)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(expression);
            return post;
        }
    }
}
