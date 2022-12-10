using AutoMapper;
using GainVocab.API.Data;
using GainVocab.API.Core.Models;
using System.Diagnostics.Metrics;
using GainVocab.API.Data.Models;
using GainVocab.API.Core.Models.Users;
using Microsoft.AspNetCore.Identity;
using GainVocab.API.Core.Extensions.Errors;

namespace GainVocab.API.Core.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<APIUserModel, APIUser>().ReverseMap();
            CreateMap<RegisterModel, APIUser>().ReverseMap();
            CreateMap<UserAddModel, APIUser>().ReverseMap();
            CreateMap<UserEditModel, APIUser>().ReverseMap();
            CreateMap<IdentityError, ErrorEntry>()
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.Code, o => o.MapFrom(s => s.Code))
                .ForMember(d => d.Source, o => o.Ignore());
        }
    }
}