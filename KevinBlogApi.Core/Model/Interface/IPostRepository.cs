using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KevinBlogApi.Core.Model.Interface
{
    public interface IPostRepository
    {
        Task<bool> AddOrEdit(Post post);

        Task<Post> GetPost(System.Linq.Expressions.Expression<Func<Post, bool>> expression);
    }
}
