using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KevinBlogApi.Web.Model
{
    public class ListPostDescViewModel
    {
        public string PostId { get; set; }

        public string Title { get; set; }

        public string Slug { get; set; }

        public string Tags { get; set; }

        public string UserId { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
