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

        public UsersController(IAuthManager authManager, UserManager<APIUser> userManager, ILogger<UsersController> logger, UsersService users)
        {
            AuthManager = authManager;
            Logger = logger;
            UserManager = userManager;
            Users = users;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> GetList([FromQuery] FilterModel filter, [FromQuery] PagerParams pager)
        {
            var data = await Users.GetList(filter, pager);

            return Ok(data);
        }
    }
}
