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
using GainVocab.API.Core.Models.SupportIssue;

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
            CreateMap<IdentityUser, UserOptionModel>();

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
                .ForMember(d => d.CourseId, o => o.MapFrom<CourseIdFromPublicIdResolver, string>(s => s.CoursePublicId))
                .ForMember(d => d.Examples, o => o.Ignore());
            CreateMap<Models.CourseData.ItemModel, CourseData>()
                .ForMember(d => d.Course, o => o.MapFrom<CourseFromPublicIdResolver, string>(s => s.CoursePublicId))
                .ForMember(d => d.CourseId, o => o.MapFrom<CourseIdFromPublicIdResolver, string>(s => s.CoursePublicId))
                .ReverseMap()
                .ForMember(d => d.CoursePublicId, o => o.MapFrom<CoursePublicIdFromIdResolver, long>(s => s.CourseId))
                .ForMember(d => d.Examples, o => o.MapFrom(s => s.Examples));
            CreateMap<Models.CourseData.UpdateModel, CourseData>().ReverseMap();
            CreateMap<Models.CourseData.AddModel, Models.CourseData.ItemModel>().ReverseMap();
            CreateMap<Models.CourseData.ListItemModel, CourseData>()
                .ReverseMap()
                .ForMember(d => d.NoExamples, o => o.MapFrom(s => s.Examples.Count()));

            CreateMap<Models.CourseData.ExampleAddModel, CourseDataExample>().ReverseMap();
            CreateMap<Models.CourseData.ExampleEditModel, CourseDataExample>().ReverseMap();

            CreateMap<Models.SupportIssue.AddModel, SupportIssue>()
                .ForMember(d => d.IssueTypeId, o => o.MapFrom<SupportIssueTypeIdFromPublicIdResolver, string>(s => s.TypePublicId))
                .ForMember(d => d.IssueEntity, o => o.MapFrom<CourseDataFromPublicIdResolver, string>(s => s.IssueEntityId))
                .ForMember(d => d.IssueEntityId, o => o.Ignore())
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForMember(d => d.UpdatedAt, o => o.Ignore());
            CreateMap<Models.SupportIssue.ListItemModel, SupportIssue>()
                .ReverseMap()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.PublicId))
                .ForMember(d => d.TypeName, o => o.MapFrom<SupportIssueTypeNameFromIdResolver, long>(s => s.IssueTypeId))
                .ForMember(d => d.Reporter, o => o.MapFrom<UserDetailsByIdResolver, string>(s => s.ReporterId))
                .ForMember(d => d.IssueEntity, o => o.MapFrom<IssueEntityListItemFromDataPublicIdResolver, long?>(s => s.IssueEntityId))
                .ForMember(d => d.Message, o => o.MapFrom(s => s.IssueMessage))
                .ForMember(d => d.IsResolved, o => o.MapFrom(s => s.IsResolved));
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
    #region UserByIdResolver
    public class UserDetailsByIdResolver : IMemberValueResolver<object, object, string, UserDetailsModel>
    {
        private readonly IUsersService Users;
        private readonly IMapper Mapper;

        public UserDetailsByIdResolver(IUsersService users, IMapper mapper)
        {
            Users = users;
            Mapper = mapper;
        }

        public UserDetailsModel Resolve(object source, object destination, string sourceMember, UserDetailsModel destMember, ResolutionContext context)
        {
            var user = Users.Get(sourceMember);
            var mappedUser = Mapper.Map<APIUser, UserDetailsModel>(user);
            return mappedUser;
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

    #region SupportIssueTypeIdFromPublicIdResolver
    public class SupportIssueTypeIdFromPublicIdResolver : IMemberValueResolver<object, object, string, long>
    {
        public ISupportIssueTypeService SupportIssueTypes { get; }

        public SupportIssueTypeIdFromPublicIdResolver(ISupportIssueTypeService types)
        {
            SupportIssueTypes = types;
        }

        public long Resolve(object source, object destination, string publicId,
            long courseId, ResolutionContext context)
        {
            var entity = SupportIssueTypes.Get(publicId);

            return entity != null ? entity.Id : -1;
        }
    }
    #endregion

    #region SupportIssueTypeNameFromIdResolver
    public class SupportIssueTypeNameFromIdResolver : IMemberValueResolver<object, object, long, string>
    {
        public ISupportIssueTypeService SupportIssueTypes { get; }

        public SupportIssueTypeNameFromIdResolver(ISupportIssueTypeService types)
        {
            SupportIssueTypes = types;
        }

        public string Resolve(object source, object destination, long id,
            string name, ResolutionContext context)
        {
            var entity = SupportIssueTypes.Get(id);

            return entity != null ? entity.Name : "";
        }
    }
    #endregion

    #region SupportIssueTypeNameFromIdResolver
    public class IssueEntityListItemFromDataPublicIdResolver : IMemberValueResolver<object, object, long?, IssueEntityListItemModel>
    {
        public ICourseService Courses { get; }
        public ICourseDataService CourseData { get; }

        public IssueEntityListItemFromDataPublicIdResolver(ICourseService courses, ICourseDataService courseData)
        {
            Courses = courses;
            CourseData = courseData;
        }

        public IssueEntityListItemModel Resolve(object source, object destination, long? sourceMember,
            IssueEntityListItemModel destMember, ResolutionContext context)
        {
            if (sourceMember != null)
            {
                var courseData = CourseData.Get(sourceMember.Value);
                var course = Courses.Get(courseData.CourseId);

                var entity = new IssueEntityListItemModel
                {
                    EntityId = courseData.PublicId,
                    CourseName = course.Name,
                    LanguageFrom = course.LanguageFrom.Name,
                    LanguageTo = course.LanguageTo.Name,
                    Source = courseData.Source,
                    Translation = courseData.Translation
                };

                return entity;
            }
            return new IssueEntityListItemModel();
        }
    }
    #endregion


    #region CourseDataFromPublicIdResolver
    public class CourseDataFromPublicIdResolver : IMemberValueResolver<object, object, string, CourseData>
    {
        public ICourseDataService CourseData { get; }

        public CourseDataFromPublicIdResolver(ICourseDataService courseData)
        {
            CourseData = courseData;
        }

        public CourseData Resolve(object source, object destination, string publicId,
            CourseData course, ResolutionContext context)
        {
            var entity = CourseData.Get(publicId);

            return entity;
        }
    }
    #endregion
}