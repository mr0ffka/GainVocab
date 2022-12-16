using AutoMapper;
using GainVocab.API.Core.Exceptions;
using GainVocab.API.Core.Interfaces;
using GainVocab.API.Core.Models.Pager;
using GainVocab.API.Data.Models;
using GainVocab.API.Data;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GainVocab.API.Core.Models.SupportIssue;
using Microsoft.EntityFrameworkCore;

namespace GainVocab.API.Core.Services
{
    public class SupportIssueService : GenericService<SupportIssue>, ISupportIssueService
    {
        private readonly ISupportIssueTypeService SupportIssueTypes;

        public SupportIssueService(DefaultDbContext context, ISupportIssueTypeService supportIssueTypes, IMapper mapper) : base(context, mapper)
        {
            SupportIssueTypes = supportIssueTypes;
        }

        public async Task Add(AddModel entity)
        {
            var mappedEntity = Mapper.Map<SupportIssue>(entity);
            mappedEntity.CreatedAt = DateTime.UtcNow;
            mappedEntity.UpdatedAt = DateTime.UtcNow;

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

        public SupportIssue Get(string publicId)
        {
            var entity = Context.SupportIssue
                .Where(e => e.PublicId == publicId)
                .FirstOrDefault();

            if (entity == null)
                throw new NotFoundException("SupportIssue", "Entity not found");

            return entity;
        }

        public async Task<PagedResult<ListItemModel>> GetList(FilterModel filter, PagerParams pager)
        {
            // filtres 
            var predicate = PredicateBuilder.New<SupportIssue>(true);

            if (filter.IsResolved != null && filter.IsResolved.Any())
            {
                var predicateOr = PredicateBuilder.New<SupportIssue>(true);

                foreach (var f in filter.IsResolved)
                {
                    predicateOr.Or(x => x.IsResolved == f);
                }

                predicate.And(predicateOr);
            }

            if (filter.ReporterId != null && filter.ReporterId.Any())
            {
                var predicateOr = PredicateBuilder.New<SupportIssue>(true);

                foreach (var f in filter.ReporterId)
                {
                    predicateOr.Or(x => x.ReporterId == f);
                }

                predicate.And(predicateOr);
            }

            if (filter.TypeId != null && filter.TypeId.Any())
            {
                var predicateOr = PredicateBuilder.New<SupportIssue>(true);

                foreach (var f in filter.TypeId)
                {
                    var type = SupportIssueTypes.Get(f);
                    if (type != null)
                    {
                        predicateOr.Or(x => x.IssueTypeId == type.Id);
                    }
                }

                predicate.And(predicateOr);
            }

            var query = Context.SupportIssue
                .Include(e => e.IssueType)
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
                throw new NotFoundException("SupportIssue", publicId);
            }

            Context.Remove(entity);
            await Context.SaveChangesAsync();
        }

        public void Resolve(string publicId)
        {
            var entity = Get(publicId);
            entity.IsResolved = true;

            try
            {
                Context.Update(entity);
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new BadRequestException("Something went wrong during database update");
            }
        }
    }
}
