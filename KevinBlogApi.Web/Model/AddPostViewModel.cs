using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KevinBlogApi.Web.Model
{
    public class AddPostViewModel
    {
        [RegularExpression("([a-zA-Z-_0-9]){3,}", ErrorMessage = "Slug 仅允许包含连字符和下划线和数字字母")]
        public string Slug { get; set; }

        [MaxLength(50, ErrorMessage = "标题不大于50个字符")]
        public string Title { get; set; }

        public string MarkDown { get; set; }

        public string Content { get; set; }

        public string Tag { get; set; }
    }
}
