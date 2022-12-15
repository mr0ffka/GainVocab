﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using GainVocab.API.Core.Exceptions;
using GainVocab.API.Core.Extensions;
using GainVocab.API.Core.Interfaces;
using GainVocab.API.Core.Models.Pager;
using GainVocab.API.Core.Models.Users;
using GainVocab.API.Data;
using GainVocab.API.Data.Models;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace GainVocab.API.Core.Services
{
    public class UsersService : GenericService<APIUser>, IUsersService
    {
        private readonly UserManager<APIUser> UserManager;
        private readonly ICourseService CourseService;

        public UsersService(DefaultDbContext context, UserManager<APIUser> userManager, ICourseService courseService, IMapper mapper) : base(context, mapper)
        {
            UserManager = userManager;
            CourseService = courseService;
        }

        public async Task<IdentityResult> Add(UserAddModel newUser)
        {
            var user = Mapper.Map<APIUser>(newUser);
            user.UserName = newUser.Email;
            user.EmailConfirmed = true;

            var result = await UserManager.CreateAsync(user, newUser.Password);

            if (result.Succeeded)
            {
                await UserManager.AddToRolesAsync(user, newUser.Roles);
                var courses = new List<Course>();
                if (newUser.Courses != null && newUser.Courses.Any())
                {
                    newUser.Courses.ForEach(cpid => courses.Add(CourseService.Get(cpid)));
                    var coursesResult = new List<APIUserCourse>();
                    foreach (var course in courses)
                    {
                        var temp = new APIUserCourse();
                        temp.APIUserId = user.Id;
                        temp.CourseId = course.Id;
                        coursesResult.Add(temp);
                    }
                    user.Courses = coursesResult;
                    await UserManager.UpdateAsync(user);
                }
            }

            return result;
        }

        public APIUser GetAsync(string id)
        {
            var user = Context.Users
                .Include(x => x.Courses)
                .Where(u => u.Id == id)
                .FirstOrDefault();

            if (user == null)
                throw new NotFoundException("User", "Entity not found");

            return user;
        }

        public async Task<APIUserModel> GetUserModel(string id)
        {
            var user = GetAsync(id);
            var result = Mapper.Map<APIUser, APIUserModel>(user);

            return result;
        }

        public async Task<PagedResult<APIUserModel>> GetList(FilterModel filter, PagerParams pager)
        {
            // filtres 
            var predicate = PredicateBuilder.New<APIUser>(true);
            if (!string.IsNullOrEmpty(filter.FirstName))
            { 
                predicate.And(x => x.FirstName.ToLower().Contains(filter.FirstName.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter.LastName))
            {
                predicate.And(x => x.LastName.ToLower().Contains(filter.LastName.ToLower()));
            }

            if (filter.Courses != null && filter.Courses.Any())
            {
                var courses = CourseService.GetListByPublicId(filter.Courses).Select(c => c.Id).ToList();

                predicate.And(x => x.Courses.Any(c => courses.Contains(c.CourseId)));
            }

            var query = Context.Users
                .Include(u => u.Courses)
                .Where(predicate);

            // sorting
            if (!string.IsNullOrEmpty(pager.SortBy))
            {
                var columnsSelectors = new Dictionary<string, Expression<Func<APIUser, object>>>
                {
                    { nameof(APIUser.FirstName).ToUpper(), x => x.FirstName! },
                    { nameof(APIUser.LastName).ToUpper(), x => x.LastName! },
                    { nameof(APIUser.Email).ToUpper(), x => x.Email },
                };

                var selectedColumn = columnsSelectors[pager.SortBy.ToUpper()];

                query = pager.SortDirection == "ASC" 
                    ? query.OrderBy(selectedColumn) 
                    : query.OrderByDescending(selectedColumn);
            }

            var users = query
                .Skip(pager.PageSize * (pager.PageNumber - 1))
                .Take(pager.PageSize)
                .ToList();

            var items = Mapper.Map<List<APIUser>, List<APIUserModel>>(users);

            if (filter.Roles != null && filter.Roles.Any())
            {
                items = items.Where(u => filter.Roles.Select(x => x.ToString()).ToList().Intersect(u.Roles).Any()).ToList();
            }

            return new PagedResult<APIUserModel>
            {
                Items = items,
                PageNumber = pager.PageNumber,
                RecordNumber = pager.PageSize,
                TotalCount = items.Count
            };
        }

        public async Task Update(string id, UserEditModel model)
        {
            var user = GetAsync(id);

            Mapper.Map(model, user);
            await UserManager.UpdateAsync(user);

            var userRoles = await UserManager.GetRolesAsync(user);
            await UserManager.RemoveFromRolesAsync(user, userRoles);
            await UserManager.AddToRolesAsync(user, model.Roles);
        }

        public async Task Remove(string id)
        {
            var user = GetAsync(id);
            var result = await UserManager.DeleteAsync(user);
        }

        public async Task<UserDetailsModel> GetDetails(string id)
        {
            var user = GetAsync(id);
            var details = Mapper.Map<UserDetailsModel>(user);

            return details;

        }

        public List<string> GetRoleOptionsList()
        {
            var query = Context.Roles.ToList();

            var items = Mapper.Map<List<string>>(query);

            return items;
        }
    }
}
