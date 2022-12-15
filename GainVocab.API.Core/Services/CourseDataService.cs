using AutoMapper;
using FluentValidation;
using GainVocab.API.Core.Exceptions;
using GainVocab.API.Core.Extensions.Errors;
using GainVocab.API.Core.Interfaces;
using GainVocab.API.Core.Models.CourseData;
using GainVocab.API.Core.Models.Pager;
using GainVocab.API.Data;
using GainVocab.API.Data.Models;
using LinqKit;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;

namespace GainVocab.API.Core.Services
{
    public class CourseDataService : GenericService<CourseData>, ICourseDataService
    {
        private readonly ICourseService Courses;

        public CourseDataService(DefaultDbContext context, ICourseService courses, IMapper mapper) : base(context, mapper)
        {
            Courses = courses;
        }

        public async Task Add(AddModel entity)
        {
            var mappedEntity = Mapper.Map<CourseData>(entity);

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

        public async Task Update(string id, UpdateModel model)
        {
            var entity = Get(id);
            var mappedEntity = Mapper.Map(model, entity);

            try
            {
                Context.Update(mappedEntity);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new BadRequestException("Something went wrong during database update");
            }
        }

        public CourseData Get(string publicId)
        {
            var entity = Context.CourseData
                .Where(e => e.PublicId == publicId)
                .FirstOrDefault();

            if (entity == null)
                throw new NotFoundException("CoursesData", "Entity not found");

            return entity;
        }

        public ItemModel GetItemModel(string publicId)
        {
            var query = Context.CourseData.FirstOrDefault(cd => cd.PublicId == publicId);

            var entity = Mapper.Map<ItemModel>(query);

            return entity;
        }

        public async Task<PagedResult<ListItemModel>> GetList(string coursePublicId, FilterModel filter, PagerParams pager)
        {
            // filtres 
            var course = Courses.Get(coursePublicId);

            var predicate = PredicateBuilder.New<CourseData>(true);
            predicate.And(x => x.CourseId == course.Id);

            if (!string.IsNullOrEmpty(filter.PublicId))
            {
                predicate.And(x => x.PublicId.ToLower().Contains(filter.PublicId.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter.Source))
            {
                predicate.And(x => x.Source.ToLower().Contains(filter.Source.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter.Translation))
            {
                predicate.And(x => x.Translation.ToLower().Contains(filter.Translation.ToLower()));
            }

            var query = Context.CourseData
                .AsExpandable()
                .Include(c => c.Course)
                .Where(predicate);

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
                throw new NotFoundException("CourseData", publicId);
            }

            Context.Remove(entity);
            await Context.SaveChangesAsync();
        }
    }
}
