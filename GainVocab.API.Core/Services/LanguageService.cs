using AutoMapper;
using GainVocab.API.Core.Exceptions;
using GainVocab.API.Core.Interfaces;
using GainVocab.API.Core.Models.Language;
using GainVocab.API.Core.Models.Pager;
using GainVocab.API.Data;
using GainVocab.API.Data.Models;
using LinqKit;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace GainVocab.API.Core.Services
{
    public class LanguageService : GenericService<Language>, ILanguageService
    {
        public LanguageService(DefaultDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task Add(LanguageAddModel entity)
        {
            var mappedEntity = Mapper.Map<Language>(entity);

            try
            {
                await Context.AddAsync(mappedEntity);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new BadRequestException("Something went wrong during database update");
            }
        }

        public Language Get(string publicId)
        {
            var entity = Context.Languages
                .Where(e => e.PublicId == publicId)
                .FirstOrDefault();

            if (entity == null)
                throw new NotFoundException("Languages", "Entity not found");

            return entity;
        }

        public LanguageListModel GetListModel(string publicId)
        {
            var user = Get(publicId);

            //var userRoles = await UserManager.GetRolesAsync(user);
            var result = Mapper.Map<Language, LanguageListModel>(user);
            //result.Roles = userRoles.ToList() ?? new List<string>();

            return result;
        }

        public async Task<PagedResult<LanguageListModel>> GetList(FilterModel filter, PagerParams pager)
        {
            // filtres 
            var predicate = PredicateBuilder.New<Language>(true);

            if (!string.IsNullOrEmpty(filter.Name))
            {
                predicate.And(x => x.Name.ToLower().Contains(filter.Name.ToLower()));
            }

            var query = Context.Languages
                .AsNoTracking()
                .AsExpandable()
                .Where(predicate);

            // sorting
            if (!string.IsNullOrEmpty(pager.SortBy))
            {
                var columnsSelectors = new Dictionary<string, Expression<Func<Language, object>>>
                {
                    { nameof(APIUser.FirstName).ToUpper(), x => x.Name! },
                };

                var selectedColumn = columnsSelectors[pager.SortBy.ToUpper()];

                query = pager.SortDirection == "ASC"
                    ? query.OrderBy(selectedColumn)
                    : query.OrderByDescending(selectedColumn);
            }

            var entities = query
                .Skip(pager.PageSize * (pager.PageNumber - 1))
                .Take(pager.PageSize)
                .ToList();

            var items = new List<LanguageListModel>();
            items = Mapper.Map<List<LanguageListModel>>(entities);

            //foreach (var user in entities)
            //{
            //    var userRoles = await UserManager.GetRolesAsync(user);
            //    var mappedUser = Mapper.Map<APIUser, APIUserModel>(user);
            //    mappedUser.Roles = userRoles.ToList() ?? new List<string>();
            //    items.Add(mappedUser);
            //}

            //if (filter.Roles != null && filter.Roles.Any())
            //{
            //    items = items.Where(u => filter.Roles.Select(x => x.ToString()).ToList().Intersect(u.Roles).Any()).ToList();
            //}

            return new PagedResult<LanguageListModel>
            {
                Items = items,
                PageNumber = pager.PageNumber,
                RecordNumber = pager.PageSize,
                TotalCount = items.Count
            };
        }

        public async Task Remove(string publicId)
        {
            var entity = Get(publicId);

            if (entity is null)
            {
                throw new NotFoundException("Languages", publicId);
            }

            Context.Remove(entity);
            await Context.SaveChangesAsync();
        }
    }
}
