using AutoMapper;
using GainVocab.API.Core.Exceptions;
using GainVocab.API.Core.Extensions.Errors;
using GainVocab.API.Core.Interfaces;
using GainVocab.API.Core.Models.Language;
using GainVocab.API.Core.Models.Pager;
using GainVocab.API.Core.Services;
using GainVocab.API.Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Collections.Generic;
using System.Security.Claims;

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

        [HttpPost("add")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Add([FromBody] LanguageAddModel entity)
        {
            await Languages.Add(entity);

            //if (response.Errors.Any())
            //{
            //    throw new BadRequestException("Error while adding user", Mapper.Map<List<IdentityError>, List<ErrorEntry>>(response.Errors.ToList()));
            //}

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
    }
}
