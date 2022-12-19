using AutoMapper;
using GainVocab.API.Core.Exceptions;
using GainVocab.API.Core.Interfaces;
using GainVocab.API.Core.Models.CourseData;
using GainVocab.API.Core.Models.Pager;
using GainVocab.API.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GainVocab.API.App.Controllers
{
    [Route("api/course-data")]
    [ApiController]
    [Authorize]
    public class CourseDataController : ControllerBase
    {
        private readonly ICourseDataService CoursesData;
        private readonly ILogger<CourseDataController> Logger;
        private readonly IMapper Mapper;

        public CourseDataController(ILogger<CourseDataController> logger, ICourseDataService coursesData, IMapper mapper)
        {
            Logger = logger;
            CoursesData = coursesData;
            Mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> GetList([FromQuery] string coursePublicId, [FromQuery] FilterModel filter, [FromQuery] PagerParams pager)
        {
            var data = await CoursesData.GetList(coursePublicId, filter, pager);

            return Ok(data);
        }

        [HttpPost("add")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Add([FromBody] AddModel entity)
        {
            await CoursesData.Add(entity);

            return Ok();
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Update(string id, [FromBody] UpdateModel model)
        {
            await CoursesData.Update(id, model);
            return Ok();
        }

        [HttpGet("{publicId}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Get(string? publicId)
        {
            var entity = CoursesData.GetItemModel(publicId);

            if (entity == null)
            {
                throw new NotFoundException("Course data", "publicId");
            }

            return Ok(entity);
        }

        [HttpDelete("{publicId}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Remove(string? publicId)
        {
            await CoursesData.Remove(publicId);

            return Ok();
        }

        [HttpGet("count")]
        [Authorize(Roles = "Administrator")]
        public ActionResult GetCount()
        {
            var data = CoursesData.GetCount();

            return Ok(data);
        }

        [HttpGet("examples/count")]
        [Authorize(Roles = "Administrator")]
        public ActionResult GetExamplesCount()
        {
            var data = CoursesData.GetExamplesCount();

            return Ok(data);
        }
    }
}
