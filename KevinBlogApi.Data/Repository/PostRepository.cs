using KevinBlogApi.Core.Model;
using KevinBlogApi.Core.Model.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            var ret = false;
            if(string.IsNullOrEmpty(post.PostId))
            {
                post.PostId = Guid.NewGuid().ToString();
                await _context.Posts.AddAsync(post);
            }
            else
            {
                _context.Posts.Update(post);
            }
            await _context.SaveChangesAsync();
            return ret;
        }

        public async Task<bool> DeletePost(string id)
        {
           var post = await _context.Posts.FirstOrDefaultAsync(s => s.PostId == id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                return true;
            }
            else return false;
        }

        public async Task<Post> GetPost(System.Linq.Expressions.Expression<Func<Post, bool>> expression)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(expression);
            return post;
        }

        public async Task<IEnumerable<Post>> ToListAsync()
        {
            var posts = await _context.Posts.ToListAsync();
            return posts;
        }
    }
}
