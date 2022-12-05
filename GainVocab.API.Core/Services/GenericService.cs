using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.Metrics;
using GainVocab.API.Core.Interfaces;
using GainVocab.API.Core.Exceptions;
using GainVocab.API.Core.Models.Pager;
using GainVocab.API.Core.Models.Core;
using GainVocab.API.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GainVocab.API.Core.Services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        protected readonly DefaultDbContext Context;
        protected readonly IMapper Mapper;

        public GenericService(DefaultDbContext context, IMapper mapper)
        {
            this.Context = context;
            this.Mapper = mapper;
        }

        public async Task<T> AddAsync(T entity)
        {
            await Context.AddAsync(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task<TResult> AddAsync<TSource, TResult>(TSource source)
        {
            var entity = Mapper.Map<T>(source);

            await Context.AddAsync(entity);
            await Context.SaveChangesAsync();

            return Mapper.Map<TResult>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);

            if (entity is null)
            {
                throw new NotFoundException(typeof(T).Name, id);
            }

            Context.Set<T>().Remove(entity);
            await Context.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            var entity = await GetAsync(id);
            return entity != null;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await Context.Set<T>().ToListAsync();
        }

        public async Task<PagedResult<TResult>> GetAllAsync<TResult>(PagerParams pagerParams)
        {
            var totalSize = await Context.Set<T>().CountAsync();
            var items = await Context.Set<T>()
                .Skip(pagerParams.PageSize * (pagerParams.PageNumber - 1))
                .Take(pagerParams.PageSize)
                .ProjectTo<TResult>(Mapper.ConfigurationProvider)
                .ToListAsync();

            return new PagedResult<TResult>
            {
                Items = items,
                PageNumber = pagerParams.PageNumber,
                RecordNumber = pagerParams.PageSize,
                TotalCount = totalSize
            };
        }

        public async Task<List<TResult>> GetAllAsync<TResult>()
        {
            return await Context.Set<T>()
                .ProjectTo<TResult>(Mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<T> GetAsync(int? id)
        {
            if (id is null)
            {
                return null;
            }

            return await Context.Set<T>().FindAsync(id);
        }

        public async Task<TResult> GetAsync<TResult>(int? id)
        {
            var result = await Context.Set<T>().FindAsync(id);
            if (result is null)
            {
                throw new NotFoundException(typeof(T).Name, id.HasValue ? id : "No Key Provided");
            }

            return Mapper.Map<TResult>(result);
        }

        public async Task<T> GetAsync(string? id)
        {
            if (id is null)
            {
                return null;
            }

            return await Context.Set<T>().FindAsync(id);
        }

        public async Task<TResult> GetAsync<TResult>(string? id)
        {
            var result = await Context.Set<T>().FindAsync(id);
            if (result is null)
            {
                throw new NotFoundException(typeof(T).Name, string.IsNullOrEmpty(id) ? id : "No Key Provided");
            }

            return Mapper.Map<TResult>(result);
        }

        public async Task UpdateAsync(T entity)
        {
            Context.Update(entity);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateAsync<TSource>(int id, TSource source) where TSource : IBaseModel
        {
            if (id != source.Id)
            {
                throw new BadRequestException("Invalid Id used in request");
            }

            var entity = await GetAsync(id);

            if (entity == null)
            {
                throw new NotFoundException(typeof(T).Name, id);
            }

            Mapper.Map(source, entity);
            Context.Update(entity);
            await Context.SaveChangesAsync();
        }
    }
}