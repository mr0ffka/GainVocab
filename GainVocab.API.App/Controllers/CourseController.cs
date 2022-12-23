using AutoMapper;
using GainVocab.API.Core.Exceptions;
using GainVocab.API.Core.Extensions.Errors;
using GainVocab.API.Core.Interfaces;
using GainVocab.API.Core.Models.Course;
using GainVocab.API.Core.Models.Pager;
using GainVocab.API.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg;

namespace GainVocab.API.App.Controllers
{
    [Route("api/course")]
    [ApiController]
    [Authorize]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService Courses;
        private readonly ILogger<CourseController> Logger;
        private readonly IMapper Mapper;

        public CourseController(ILogger<CourseController> logger, ICourseService courses, IMapper mapper)
        {
            Logger = logger;
            Courses = courses;
            Mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> GetList([FromQuery] FilterModel filter, [FromQuery] PagerParams pager)
        {
            var data = await Courses.GetList(filter, pager);

            return Ok(data);
        }

        [HttpPatch("{publicId}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Update(string publicId, [FromBody] string description)
        {
            await Courses.Update(publicId, description);

            return Ok();
        }

        [HttpGet("options")]
        [Authorize(Roles = "Administrator")]
        public ActionResult GetOptionsList()
        {
            var data = Courses.GetOptionsList();

            return Ok(data);
        }

        [HttpPost("add")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Add([FromBody] AddModel entity)
        {
            await Courses.Add(entity);

            return Ok();
        }

        [HttpGet("{publicId}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Get(string? publicId)
        {
            var user = Courses.Get(publicId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpDelete("{publicId}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Remove(string? publicId)
        {
            await Courses.Remove(publicId);

            return Ok();
        }

        [HttpGet("count")]
        [Authorize(Roles = "Administrator")]
        public ActionResult GetCount()
        {
            var data = Courses.GetCount();

            return Ok(data);
        }

        [HttpGet("me/available")]
        [Authorize(Roles = "Administrator, User")]
        public async Task<ActionResult> GetAvailableList([FromQuery] string userId, [FromQuery] FilterModel filter)
        {
            var uid = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            if (uid != userId)
                throw new UnauthorizedException(ErrorMessages.UnauthorizedMessage_NoAccess);

            var items = await Courses.GetAvailableList(userId, filter);

            return Ok(items);
        }

        [HttpPost("me/add")]
        [Authorize(Roles = "Administrator, User")]
        public async Task<ActionResult> AddUserToCourse([FromBody] AddUserToCourseModel entity)
        {
            var uid = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            if (uid != entity.UserId)
                throw new UnauthorizedException(ErrorMessages.UnauthorizedMessage_NoAccess);

            await Courses.AddUser(entity);

            return Ok();
        }
    }
}
