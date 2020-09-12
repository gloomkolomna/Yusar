using AutoMapper;
using Yusar.Client.Models;
using Yusar.Core.Entities;

namespace Yusar.Client.Profiles
{
    public class YusarProfile : Profile
    {
        public YusarProfile()
        {
            CreateMap<SimpleString, SimpleStringModel>();
        }
    }
}
