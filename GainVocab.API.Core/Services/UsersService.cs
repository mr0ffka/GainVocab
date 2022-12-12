using AutoMapper;
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

        public UsersService(DefaultDbContext context, UserManager<APIUser> userManager, IMapper mapper) : base(context, mapper)
        {
            UserManager = userManager;
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
            }

            return result;
        }

        public async Task<APIUser> GetAsync(string id)
        {
            var user = await Context.Users
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            if (user == null)
                throw new NotFoundException("User", "Entity not found");

            return user;
        }

        public async Task<APIUserModel> GetUserModel(string id)
        {
            var user = await GetAsync(id);

            var userRoles = await UserManager.GetRolesAsync(user);
            var result = Mapper.Map<APIUser, APIUserModel>(user);
            result.Roles = userRoles.ToList() ?? new List<string>();

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

            var query = Context.Users
                .AsNoTracking()
                .AsExpandable()
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

            var items = new List<APIUserModel>();

            foreach (var user in users)
            {
                var userRoles = await UserManager.GetRolesAsync(user);
                var mappedUser = Mapper.Map<APIUser, APIUserModel>(user);
                mappedUser.Roles = userRoles.ToList() ?? new List<string>();
                items.Add(mappedUser);
            }

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
            var user = await GetAsync(id);

            Mapper.Map(model, user);
            await UserManager.UpdateAsync(user);

            var userRoles = await UserManager.GetRolesAsync(user);
            await UserManager.RemoveFromRolesAsync(user, userRoles);
            await UserManager.AddToRolesAsync(user, model.Roles);
        }

        public async Task Remove(string id)
        {
            var user = await GetAsync(id);
            var result = await UserManager.DeleteAsync(user);
        }
    }
}
