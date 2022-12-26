using AutoMapper;
using GainVocab.API.Core.Exceptions;
using GainVocab.API.Core.Interfaces;
using GainVocab.API.Data;
using GainVocab.API.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Core.Services
{
    public class UserCourseService : GenericService<APIUserCourse>, IUserCourseService
    {
        public UserCourseService(DefaultDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public APIUserCourse Get(string id)
        {
            var entity = Context.APIUserCourse
                .Where(e => e.PublicId == id)
                .Include(e => e.APIUser)
                    .ThenInclude(e => e.CoursesDone)
                .Include(e => e.Course)
                    .ThenInclude(e => e.Data)
                .Include(e => e.CourseProgress)
                    .ThenInclude(e => e.DataDone)
                .Include(e => e.CourseProgress)
                    .ThenInclude(e => e.CurrentCourseData)
                        .ThenInclude(e => e.Examples)
                .FirstOrDefault();

            if (entity == null)
                throw new NotFoundException("Courses", "Entity not found");

            return entity;
        }
    }
}
