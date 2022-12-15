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
            CreateMap<APIUserModel, APIUser>()
                .ReverseMap()
                .ForMember(d => d.Roles, o => o.MapFrom<UserRolesResolver, APIUser>(s => s))
                .ForMember(d => d.Courses, o => o.MapFrom<UserCoursesResolver, ICollection<APIUserCourse>>(s => s.Courses));
            CreateMap<RegisterModel, APIUser>();
            CreateMap<UserAddModel, APIUser>()
                .ForMember(d => d.Courses, o => o.Ignore());
            CreateMap<UserEditModel, APIUser>()
                .ForMember(d => d.Courses, o => o.MapFrom<CoursesUserEditResolver, Tuple<string, List<string>>>(s => new Tuple<string, List<string>>(s.Email, s.Courses)));
            CreateMap<UserDetailsModel, APIUser>()
                .ReverseMap()
                .ForMember(d => d.Roles, o => o.MapFrom<UserRolesResolver, APIUser>(s => s))
                .ForMember(d => d.Courses, o => o.MapFrom<UserCoursesResolver, ICollection<APIUserCourse>>(s => s.Courses));
            CreateMap<IdentityError, ErrorEntry>()
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.Code, o => o.MapFrom(s => s.Code))
                .ForMember(d => d.Source, o => o.Ignore());
            CreateMap<IdentityRole, String>()
                .ConvertUsing(r => r.Name);

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

            CreateMap<Models.CourseData.AddModel, CourseData>()
                .ForMember(d => d.Course, o => o.MapFrom<CourseFromPublicIdResolver, string>(s => s.CoursePublicId))
                .ForMember(d => d.CourseId, o => o.MapFrom<CourseIdFromPublicIdResolver, string>(s => s.CoursePublicId));
            CreateMap<Models.CourseData.ItemModel, CourseData>()
                .ForMember(d => d.Course, o => o.MapFrom<CourseFromPublicIdResolver, string>(s => s.CoursePublicId))
                .ForMember(d => d.CourseId, o => o.MapFrom<CourseIdFromPublicIdResolver, string>(s => s.CoursePublicId))
                .ReverseMap()
                .ForMember(d => d.CoursePublicId, o => o.MapFrom<CoursePublicIdFromIdResolver, long>(s => s.CourseId));
            CreateMap<Models.CourseData.UpdateModel, CourseData>().ReverseMap();
            CreateMap<Models.CourseData.AddModel, Models.CourseData.ItemModel>().ReverseMap();
            CreateMap<Models.CourseData.ListItemModel, CourseData>()
                .ReverseMap();
        }
    }

    #region UserRolesResolver
    public class UserRolesResolver : IMemberValueResolver<object, object, APIUser, List<string>>
    {
        private readonly UserManager<APIUser> UserManager;

        public UserRolesResolver(UserManager<APIUser> userManager)
        {
            UserManager = userManager;
        }

        public List<string> Resolve(object source, object destination, APIUser sourceMember, List<string> destMember, ResolutionContext context)
        {
            var userRoles = UserManager.GetRolesAsync(sourceMember).GetAwaiter();

            return userRoles.GetResult().ToList();
        }
    }
    #endregion

    #region UserCoursesResolver
    public class UserCoursesResolver : IMemberValueResolver<object, object, ICollection<APIUserCourse>, List<string>>
    {
        private readonly ICourseService Courses;

        public UserCoursesResolver(ICourseService courses)
        {
            Courses = courses;
        }

        public List<string> Resolve(object source, object destination, ICollection<APIUserCourse> sourceMember, List<string> destMember, ResolutionContext context)
        {
            var courses = new List<Course>();
            foreach (var entity in sourceMember)
            {
                var course = Courses.Get(entity.CourseId);
                courses.Add(course);
            }
            var coursesNames = new List<string>();
            if (courses.Any())
            {
                coursesNames.AddRange(courses.Select(c => c.Name));
            }
            return coursesNames;
        }
    }
    #endregion

    #region CoursesUserAddEditResolver
    public class CoursesUserEditResolver : IMemberValueResolver<object, object, Tuple<string, List<string>>, ICollection<APIUserCourse>>
    {
        private readonly UserManager<APIUser> UserManager;
        private readonly ICourseService Courses;

        public CoursesUserEditResolver(UserManager<APIUser> userManager, ICourseService courses)
        {
            UserManager = userManager;
            Courses = courses;
        }

        public ICollection<APIUserCourse> Resolve(object source, object destination, Tuple<string, List<string>> sourceMember, ICollection<APIUserCourse> destMember, ResolutionContext context)
        {
            var userAwaiter = UserManager.FindByEmailAsync(sourceMember.Item1).GetAwaiter();
            var user = userAwaiter.GetResult();
            var courses = new List<Course>();
            sourceMember.Item2.ForEach(cpid => courses.Add(Courses.Get(cpid)));
            var result = new List<APIUserCourse>();
            foreach (var course in courses)
            {
                var temp = new APIUserCourse();
                temp.APIUserId = user.Id;
                temp.CourseId = course.Id;
                result.Add(temp);
            }
            return result;
        }
    }
    #endregion

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

    #region CourseFromPublicIdResolver
    public class CourseFromPublicIdResolver : IMemberValueResolver<object, object, string, Course>
    {
        public ICourseService Courses { get; }

        public CourseFromPublicIdResolver(ICourseService courses)
        {
            Courses = courses;
        }

        public Course Resolve(object source, object destination, string publicId,
            Course course, ResolutionContext context)
        {
            var entity = Courses.Get(publicId);

            return entity;
        }
    }
    #endregion

    #region CourseIdFromPublicIdResolver
    public class CourseIdFromPublicIdResolver : IMemberValueResolver<object, object, string, long>
    {
        public ICourseService Courses { get; }

        public CourseIdFromPublicIdResolver(ICourseService courses)
        {
            Courses = courses;
        }

        public long Resolve(object source, object destination, string publicId,
            long courseId, ResolutionContext context)
        {
            var entity = Courses.Get(publicId);

            return entity != null ? entity.Id : -1;
        }
    }
    #endregion

    #region CoursePublicIdFromIdResolver
    public class CoursePublicIdFromIdResolver : IMemberValueResolver<object, object, long, string>
    {
        public ICourseService Courses { get; }

        public CoursePublicIdFromIdResolver(ICourseService courses)
        {
            Courses = courses;
        }

        public string Resolve(object source, object destination, long id,
            string publicId, ResolutionContext context)
        {
            var entity = Courses.Get(id);

            return entity != null ? entity.PublicId : "";
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