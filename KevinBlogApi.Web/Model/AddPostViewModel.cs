using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KevinBlogApi.Web.Model
{
    public class AddPostViewModel
    {
        public string Slug { get; set; }

        public string Title { get; set; }

        public string MarkDown { get; set; }

        public string Content { get; set; }

        public string Tag { get; set; }
    }
}
