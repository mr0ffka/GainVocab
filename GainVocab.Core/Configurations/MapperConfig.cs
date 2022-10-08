using AutoMapper;
using GainVocab.Data;
using GainVocab.Core.Models;
using System.Diagnostics.Metrics;
using GainVocab.Data.Models;
using GainVocab.Core.Models.Users;

namespace GainVocab.Core.Configurations
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