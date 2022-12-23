using AutoMapper;
using GainVocab.API.Core.Interfaces;
using GainVocab.API.Core.Models.Language;
using GainVocab.API.Core.Models.Pager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GainVocab.API.App.Controllers
{
    [Route("api/course/language")]
    [ApiController]
    [Authorize]
    public class LanguageController : ControllerBase
    {
        private readonly ILanguageService Languages;
        private readonly ILogger<LanguageController> Logger;
        private readonly IMapper Mapper;

        public LanguageController(ILogger<LanguageController> logger, ILanguageService languages, IMapper mapper)
        {
            Logger = logger;
            Languages = languages;
            Mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> GetList([FromQuery] FilterModel filter, [FromQuery] PagerParams pager)
        {
            var data = await Languages.GetList(filter, pager);

            return Ok(data);
        }

        [HttpGet("options")]
        [Authorize(Roles = "Administrator, User")]
        public ActionResult GetOptionsList()
        {
            var data = Languages.GetOptionsList();

            return Ok(data);
        }

        [HttpPost("add")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Add([FromBody] AddModel entity)
        {
            await Languages.Add(entity);

            return Ok();
        }

        [HttpGet("{publicId}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Get(string? publicId)
        {
            var user = Languages.Get(publicId);

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
            await Languages.Remove(publicId);

            return Ok();
        }

        [HttpGet("count")]
        [Authorize(Roles = "Administrator")]
        public ActionResult GetCount()
        {
            var data = Languages.GetCount();

            return Ok(data);
        }
    }
}
