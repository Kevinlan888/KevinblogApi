using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KevinBlogApi.Core.Model
{
    public class Post
    {
        [Key]
        [MaxLength(255)]
        public string PostId { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        [RegularExpression("([a-zA-Z-_0-9]){3,}")]
        public string Slug { get; set; }

        public string Content { get; set; }

        public string MarkDown { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string Tag { get; set; }

        public string UserId { get; set; }
    }
}
