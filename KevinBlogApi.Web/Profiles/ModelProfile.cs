using AutoMapper;
using KevinBlogApi.Core.Model;
using KevinBlogApi.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KevinBlogApi.Web.Profiles
{
    public class ModelProfile : Profile
    {
        public ModelProfile()
        {
            CreateMap<Post, ListPostDescViewModel>();
        }
    }
}
