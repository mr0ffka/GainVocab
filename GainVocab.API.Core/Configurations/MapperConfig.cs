using AutoMapper;
using GainVocab.API.Data;
using GainVocab.API.Core.Models;
using System.Diagnostics.Metrics;
using GainVocab.API.Data.Models;
using GainVocab.API.Core.Models.Users;

namespace GainVocab.API.Core.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<APIUserModel, APIUser>().ReverseMap();
            CreateMap<RegistrationModel, APIUser>().ReverseMap();
        }
    }
}