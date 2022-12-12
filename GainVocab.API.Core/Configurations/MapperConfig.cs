using AutoMapper;
using GainVocab.API.Data;
using GainVocab.API.Core.Models;
using System.Diagnostics.Metrics;
using GainVocab.API.Data.Models;
using GainVocab.API.Core.Models.Users;
using Microsoft.AspNetCore.Identity;
using GainVocab.API.Core.Extensions.Errors;
using GainVocab.API.Core.Models.Language;
using GainVocab.API.Core.Models.Course;
using GainVocab.API.Core.Interfaces;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using LinqKit;

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

            CreateMap<Models.Language.AddModel, Language>().ReverseMap();
            CreateMap<Models.Language.ListItemModel, Language>()
                .ReverseMap()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.PublicId))
                .ForMember(d => d.Courses, o => o.MapFrom<CoursesFromAndToCombinedResolver, List<Course>>(s => s.CoursesFrom.Concat(s.CoursesTo).ToList()));
            CreateMap<Models.Language.ItemModel, Language>()
                .ReverseMap()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.PublicId));

            CreateMap<Models.Course.AddModel, Course>()
                .ForMember(d => d.LanguageFrom, o => o.MapFrom<LanguagesByPublicIdResolver, string>(s => s.LanguageFrom!))
                .ForMember(d => d.LanguageTo, o => o.MapFrom<LanguagesByPublicIdResolver, string>(s => s.LanguageTo!))
                .ReverseMap();
            CreateMap<Models.Course.ListItemModel, Course>()
                .ReverseMap()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.PublicId))
                .ForMember(d => d.LanguageFrom, o => o.MapFrom(s => s.LanguageFrom.Name))
                .ForMember(d => d.LanguageTo, o => o.MapFrom(s => s.LanguageTo.Name));
            CreateMap<Models.Course.ItemModel, Course>()
                .ReverseMap()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.PublicId));
        }
    }

    #region LanguagesByPublicIdResolver
    public class LanguagesByPublicIdResolver : IMemberValueResolver<object, object, string, Language>
    {
        public ILanguageService Languages { get; }

        public LanguagesByPublicIdResolver(ILanguageService languages)
        {
            Languages = languages;
        }

        public Language Resolve(object source, object destination, string publicId,
            Language language, ResolutionContext context)
        {
            var entity = Languages.Get(publicId);

            return entity;
        }
    }
    #endregion

    #region CoursesFromAndToCombinedResolver
    public class CoursesFromAndToCombinedResolver : IMemberValueResolver<object, object, List<Course>, List<string>>
    {
        public CoursesFromAndToCombinedResolver()
        {
        }

        public List<string> Resolve(object source, object destination, List<Course> sourceMember, List<string> destMember, ResolutionContext context)
        {
            var result = new List<string>();
            result.AddRange(sourceMember.Select(s => s.Name));
            return result;
        }
    }
    #endregion
}