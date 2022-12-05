﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using GainVocab.API.Core.Interfaces;
using GainVocab.API.Core.Models.Pager;
using GainVocab.API.Core.Models.Users;
using GainVocab.API.Data;
using GainVocab.API.Data.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Expressions;

namespace GainVocab.API.Core.Services
{
    public class UsersService : GenericService<APIUser>, IUsersService
    {
        public UsersService(DefaultDbContext context, IMapper mapper) : base(context, mapper)
        {
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

            var totalCount = query.Count();

            var items = query
                .Skip(pager.PageSize * (pager.PageNumber - 1))
                .Take(pager.PageSize)
                .ProjectTo<APIUserModel>(Mapper.ConfigurationProvider)
                .ToList();

            return new PagedResult<APIUserModel>
            {
                Items = items,
                PageNumber = pager.PageNumber,
                RecordNumber = pager.PageSize,
                TotalCount = totalCount
            };
        }
    }
}
