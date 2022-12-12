using AutoMapper;
using GainVocab.API.Core.Exceptions;
using GainVocab.API.Core.Interfaces;
using GainVocab.API.Core.Models.Course;
using GainVocab.API.Core.Models.Pager;
using GainVocab.API.Data;
using GainVocab.API.Data.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GainVocab.API.Core.Services
{
    public class CourseService : GenericService<Course>, ICourseService
    {
        public CourseService(DefaultDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task Add(AddModel entity)
        {
            var mappedEntity = Mapper.Map<Course>(entity);

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

        public Course Get(string publicId)
        {
            var entity = Context.Course
                .Where(e => e.PublicId == publicId)
                .FirstOrDefault();

            if (entity == null)
                throw new NotFoundException("Languages", "Entity not found");

            return entity;
        }

        public ListItemModel GetListModel(string publicId)
        {
            var user = Get(publicId);

            //var userRoles = await UserManager.GetRolesAsync(user);
            var result = Mapper.Map<Course, ListItemModel>(user);
            //result.Roles = userRoles.ToList() ?? new List<string>();

            return result;
        }

        public List<ItemModel> GetOptionsList()
        {
            var query = Context.Course.ToList();

            var items = Mapper.Map<List<ItemModel>>(query);

            return items;
        }

        List<Course> ICourseService.GetListByPublicId(List<string> courses)
        {
            var query = Context.Course.Where(c => courses.Contains(c.PublicId));
            return query.ToList();
        }

        public async Task<PagedResult<ListItemModel>> GetList(FilterModel filter, PagerParams pager)
        {
            // filtres 
            var predicate = PredicateBuilder.New<Course>(true);

            if (!string.IsNullOrEmpty(filter.Name))
            {
                predicate.And(x => x.Name.ToLower().Contains(filter.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter.LanguageFrom))
            {
                predicate.And(x => x.LanguageFrom.PublicId.Equals(filter.LanguageFrom));
            }

            if (!string.IsNullOrEmpty(filter.LanguageTo))
            {
                predicate.And(x => x.LanguageTo.PublicId.Equals(filter.LanguageTo));
            }

            var query = Context.Course
                .AsExpandable()
                .Include(c => c.LanguageFrom)
                .Include(c => c.LanguageTo)
                .Where(predicate);

            // sorting
            if (!string.IsNullOrEmpty(pager.SortBy))
            {
                var columnsSelectors = new Dictionary<string, Expression<Func<Course, object>>>
                {
                    { nameof(Course.Name).ToUpper(), x => x.Name! },
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
                throw new NotFoundException("Course", publicId);
            }

            Context.Remove(entity);
            await Context.SaveChangesAsync();
        }
    }
}
