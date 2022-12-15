using AutoMapper;
using GainVocab.API.Core.Exceptions;
using GainVocab.API.Core.Interfaces;
using GainVocab.API.Core.Models.Language;
using GainVocab.API.Core.Models.Pager;
using GainVocab.API.Data;
using GainVocab.API.Data.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GainVocab.API.Core.Services
{
    public class LanguageService : GenericService<Language>, ILanguageService
    {
        private ICourseService CourseService;

        public LanguageService(DefaultDbContext context, ICourseService courseService, IMapper mapper) : base(context, mapper)
        {
            CourseService = courseService;
        }

        public async Task Add(AddModel entity)
        {
            var mappedEntity = Mapper.Map<Language>(entity);

            if (Context.Languages.Where(c => c.Name.Equals(mappedEntity.Name)).Any())
            {
                throw new BadRequestException($"Language '{mappedEntity.Name}' already exists!");
            }

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

        public ListItemModel GetListModel(string publicId)
        {
            var entity = Get(publicId);

            //var userRoles = await UserManager.GetRolesAsync(user);
            var result = Mapper.Map<Language, ListItemModel>(entity);
            //result.Roles = userRoles.ToList() ?? new List<string>();

            return result;
        }

        public List<ItemModel> GetOptionsList()
        {
            var query = Context.Languages.ToList();

            var items = Mapper.Map<List<ItemModel>>(query);

            return items;
        }

        public async Task<PagedResult<ListItemModel>> GetList(FilterModel filter, PagerParams pager)
        {
            // filtres 
            var predicate = PredicateBuilder.New<Language>(true);

            if (!string.IsNullOrEmpty(filter.Name))
            {
                predicate.And(x => x.Name.ToLower().Contains(filter.Name.ToLower()));
            }

            var query = Context.Languages
                .Include(l => l.CoursesFrom)
                .Include(l => l.CoursesFrom)
                .Where(predicate);

            // sorting
            if (!string.IsNullOrEmpty(pager.SortBy))
            {
                var columnsSelectors = new Dictionary<string, Expression<Func<Language, object>>>
                {
                    { nameof(Language.Name).ToUpper(), x => x.Name! },
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

            var items = new List<ListItemModel>();
            items = Mapper.Map<List<ListItemModel>>(entities);

            if (filter.Courses != null && filter.Courses.Any())
            {
                var coursesNames = CourseService.GetListByPublicId(filter.Courses).Select(c => c.Name).ToList();
                items = items.Where(u => u.Courses.Intersect(coursesNames).Any()).ToList();
            }

            return new PagedResult<ListItemModel>
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
