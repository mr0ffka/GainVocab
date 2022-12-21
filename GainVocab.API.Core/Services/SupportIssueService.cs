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
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace GainVocab.API.Core.Services
{
    public class SupportIssueService : GenericService<SupportIssue>, ISupportIssueService
    {
        private readonly ISupportIssueTypeService SupportIssueTypes;
        private readonly IEmailService Emails;
        private readonly UserManager<APIUser> UserManager;

        public SupportIssueService(DefaultDbContext context, ISupportIssueTypeService supportIssueTypes, IEmailService emails, UserManager<APIUser> userManager, IMapper mapper) : base(context, mapper)
        {
            SupportIssueTypes = supportIssueTypes;
            Emails = emails;
            UserManager = userManager;
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

                var last = Context.SupportIssue.OrderByDescending(i => i.Id).FirstOrDefault();
                var admins = await UserManager.GetUsersInRoleAsync("Administrator");
                await Emails.SendNewIssueNotificationEmail(admins.Select(u => u.Email).ToList(), last.PublicId);
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
            var totalCount = Context.SupportIssue.Count();

            // filtres 
            var predicate = PredicateBuilder.New<SupportIssue>(true);

            if (!string.IsNullOrEmpty(filter.PublicId))
            {
                predicate.And(x => x.PublicId.Contains(filter.PublicId));
            }

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

            if (filter.Created != null && filter.Created.TrueForAll(x => x != "null" && x != null))
            {
                var from = DateTime.ParseExact(filter.Created[0], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal).ToUniversalTime().Date.AddDays(1);
                var to = DateTime.ParseExact(filter.Created[1], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal).ToUniversalTime().Date.AddDays(1);

                predicate.And(x => x.CreatedAt.Date >= from && x.CreatedAt.Date <= to);
            }

            if (filter.Updated != null && filter.Updated.TrueForAll(x => x != "null" && x != null))
            {
                var from = DateTime.ParseExact(filter.Updated[0], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal).ToUniversalTime().Date.AddDays(1);
                var to = DateTime.ParseExact(filter.Updated[1], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal).ToUniversalTime().Date.AddDays(1);

                predicate.And(x => x.UpdatedAt.Date >= from && x.UpdatedAt.Date <= to);
            }

            var query = Context.SupportIssue
                .Include(e => e.IssueType)
                .Where(predicate)
                .Skip(pager.PageSize * (pager.PageNumber - 1))
                .Take(pager.PageSize)
                .ToList();

            var items = new List<ListItemModel>();
            items = Mapper.Map<List<ListItemModel>>(query);

            return new PagedResult<ListItemModel>
            {
                Items = items,
                PageNumber = pager.PageNumber,
                RecordNumber = pager.PageSize,
                TotalCount = totalCount,
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

        public async Task Resolve(string publicId)
        {
            var entity = Get(publicId);
            entity.IsResolved = true;
            entity.UpdatedAt = DateTime.UtcNow;

            try
            {
                Context.Update(entity);
                Context.SaveChanges();

                var reporter = UserManager.Users.FirstOrDefault(u => u.Id == entity.ReporterId);
                if (reporter != null)
                {
                    await Emails.SendResolvedIssueNotificationEmail(reporter);
                }
            }
            catch (Exception ex)
            {
                throw new BadRequestException("Something went wrong during database update");
            }
        }

        public override int GetCount()
        {
            return Context.SupportIssue.Where(i => i.IsResolved == false).Count();
        }
    }
}
