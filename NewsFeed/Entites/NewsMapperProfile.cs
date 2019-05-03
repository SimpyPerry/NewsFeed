using AutoMapper;
using NewsFeed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsFeed.Entites
{
    public class NewsMapperProfile : Profile
    {
        public NewsMapperProfile()
        {
            CreateMap<NewsFeedEntity, NewsFeedDTO>();
            CreateMap<NewsFeedDTO, NewsFeedEntity>();
        }
    }
}
