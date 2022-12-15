using AutoMapper;
using GainVocab.API.Core.Exceptions;
using GainVocab.API.Core.Extensions.Errors;
using GainVocab.API.Core.Interfaces;
using GainVocab.API.Core.Models.Pager;
using GainVocab.API.Core.Models.Users;
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
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IAuthManager AuthManager;
        private readonly UserManager<APIUser> UserManager;
        private readonly UsersService Users;
        private readonly ILogger<UsersController> Logger;
        private readonly IMapper Mapper;

        public UsersController(IAuthManager authManager, UserManager<APIUser> userManager, ILogger<UsersController> logger, UsersService users, IMapper mapper)
        {
            AuthManager = authManager;
            Logger = logger;
            UserManager = userManager;
            Users = users;
            Mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> GetList([FromQuery] FilterModel filter, [FromQuery] PagerParams pager)
        {
            var data = await Users.GetList(filter, pager);

            return Ok(data);
        }

        [HttpPost("add")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Add([FromBody] UserAddModel newUser)
        {
            var response = await Users.Add(newUser);

            if (response.Errors.Any())
            {
                throw new BadRequestException("Error while adding user", Mapper.Map<List<IdentityError>, List<ErrorEntry>>(response.Errors.ToList()));
            }

            return Ok(response.Succeeded);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Get(string id)
        {
            var user = await Users.GetUserModel(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("details/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> GetDetails(string id)
        {
            var user = await Users.GetDetails(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("roles/options")]
        [Authorize(Roles = "Administrator")]
        public ActionResult GetOptionsList()
        {
            var data = Users.GetRoleOptionsList();

            return Ok(data);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Update(string id, [FromBody] UserEditModel model)
        {
            await Users.Update(id, model);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Remove(string id)
        {
            await Users.Remove(id);

            return Ok();
        }
    }
}
