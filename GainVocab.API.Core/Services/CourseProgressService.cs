using AutoMapper;
using FluentValidation;
using GainVocab.API.Core.Exceptions;
using GainVocab.API.Core.Extensions.Errors;
using GainVocab.API.Core.Interfaces;
using GainVocab.API.Core.Models.CourseProgress;
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
    public class CourseProgressService : GenericService<CourseProgress>, ICourseProgressService
    {

        public CourseProgressService(DefaultDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<CourseProgress> Add(AddModel entity)
        {
            var mappedEntity = Mapper.Map<CourseProgress>(entity);

            try
            {
                await Context.AddAsync(mappedEntity);
                await Context.SaveChangesAsync();
                var lastEntity = Context.CourseProgress.OrderByDescending(i => i.Id).FirstOrDefault();
                return lastEntity;
            }
            catch (Exception ex)
            {
                throw new BadRequestException("Something went wrong during database update");
            }
        }

        public async Task Remove(long userCourseId)
        {
            try
            {
                var entity = Context.CourseProgress.FirstOrDefault(x => x.UserCourseId == userCourseId);
                if (entity is not null)
                {
                    Context.Remove(entity);
                    await Context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new BadRequestException("Something went wrong during database update");
            }
        }
    }
}
