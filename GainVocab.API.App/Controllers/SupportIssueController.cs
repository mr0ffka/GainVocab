using AutoMapper;
using GainVocab.API.Core.Interfaces;
using GainVocab.API.Core.Models.SupportIssue;
using GainVocab.API.Core.Models.Pager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GainVocab.API.Data.Models;

namespace GainVocab.API.App.Controllers
{
    [Route("api/support")]
    [ApiController]
    [Authorize]
    public class SupportIssueController : ControllerBase
    {
        private readonly ISupportIssueService SupportIssues;
        private readonly ISupportIssueTypeService SupportIssueTypes;
        private readonly ILogger<LanguageController> Logger;
        private readonly IMapper Mapper;

        public SupportIssueController(ILogger<LanguageController> logger, ISupportIssueService supportIssues, ISupportIssueTypeService supportIssueTypes, IMapper mapper)
        {
            Logger = logger;
            SupportIssues = supportIssues;
            SupportIssueTypes = supportIssueTypes;
            Mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> GetList([FromQuery] FilterModel filter, [FromQuery] PagerParams pager)
        {
            var data = await SupportIssues.GetList(filter, pager);

            return Ok(data);
        }

        [HttpPost("add")]
        [Authorize(Roles = "Administrator, User")]
        public async Task<ActionResult> Add([FromBody] AddModel entity)
        {
            await SupportIssues.Add(entity);

            return Ok();
        }

        [HttpPost("resolve/{publicId}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Resolve(string publicId)
        {
            SupportIssues.Resolve(publicId);

            return Ok();
        }

        [HttpDelete("{publicId}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Remove(string? publicId)
        {
            await SupportIssues.Remove(publicId);

            return Ok();
        }

        [HttpGet("count")]
        [Authorize(Roles = "Administrator")]
        public ActionResult GetCount()
        {
            var data = SupportIssues.GetCount();

            return Ok(data);
        }

        [HttpGet("types/options")]
        [Authorize(Roles = "Administrator")]
        public ActionResult GetOptionsList()
        {
            var data = SupportIssueTypes.GetOptionsList();

            return Ok(data);
        }
    }
}
